"""Indexes documents from the Documents table."""


import sqlite3
import sys
import util
import math
import time
import spacy

from math import log10
from calc_dim_01 import process_document_spacy

class Batch(object):
    def __init__(self, i, document_ids):
        self.number = i
        self.document_ids = document_ids

def create_parser(usage):
    """Create an object to use for the parsing of command-line arguments."""
    from optparse import OptionParser
    parser = OptionParser(usage)
    parser.add_option(
            '--debug',
            '-d',
            dest='debug',
            default=False,
            action='store_true',
            help='Show debug information')
    parser.add_option(
            '--limit',
            '-l',
            dest='limit',
            type='int',
            default=0,
            help='Limit the number of documents to process')
    parser.add_option(
            '--subprocesses',
            '-N',
            dest='subprocesses',
            type='int',
            default=1,
            help='Specify the number of subprocesses to use')
    parser.add_option(
            '--batch-size',
            '-b',
            dest='batch_size',
            type='int',
            default=100,
            help='Specify the number of documents to process per batch')
    return parser

def index(filename, nlp):
    """
    Perform indexing.  Each document is stemmed, and then the non-excluded
    dimensions are counted for that document.  The result is put into the
    DocumentsToDimensions table.
    """
    conn = sqlite3.connect(filename)
    c = conn.cursor()
    params = util.get_params(c, filename)
    stemmer = params['stemmer']
    print ('index(): stemmer: %s' % stemmer)


    all_dim = util.get_dimensions(c, 0)
    assert all_dim, "You must calculate dimensions prior to indexing."

    all_include = util.get_all_include_regex(c)

    c.execute('SELECT COUNT(ED_ENC_NUM) FROM Documents')
    num_total_docs = int(c.fetchone()[0])

    c.execute('DELETE FROM DocumentsToDimensions')

    c.execute("SELECT COUNT(*) FROM Dimensions WHERE PartOfSpeech = 'bigram'")
    nBigrams = int(c.fetchone()[0])
    print ('Number of bigrams: ', nBigrams)
    do_bigrams = nBigrams > 0

    c.execute("SELECT COUNT(*) FROM Dimensions WHERE PartOfSpeech = 'trigram'")
    nTrigrams = int(c.fetchone()[0])
    print ('Number of trigrams: ', nTrigrams)
    do_trigrams = nTrigrams > 0

    #
    # If the POS column contains "unigram", then it means we didn't perform POS tagging when calculating dimensions.
    #
    c.execute("SELECT COUNT(*) FROM Dimensions WHERE PartOfSpeech = 'unigram'")
    pos_tag = int(c.fetchone()[0]) == 0

    cmd = 'SELECT ED_ENC_NUM FROM Documents'
    # if options.limit:
    #    cmd += ' LIMIT %d' % options.limit
    #    num_total_docs = min(options.limit, num_total_docs)

    #
    # TODO: why is fetchmany not working?
    #
    #document_ids = c.execute(cmd).fetchmany()
    document_ids = []
    for row in c.execute(cmd):
        document_ids.append(row[0])
    print ("fetched %d document ids" % len(document_ids))
    
    #
    # Terminate the SQL connection so that the subprocesses can use it.
    #
    conn.commit()
    conn.close()

    #
    # https://docs.python.org/2/library/array.html#module-array
    #
    
    main_process(nlp, document_ids, filename, stemmer, all_include, pos_tag, do_bigrams, do_trigrams, all_dim)    
    
    conn = sqlite3.connect(filename)
    c = conn.cursor()
    for dim_id, _, _ in all_dim:
        c.execute("""SELECT COUNT(DimensionId)
                FROM DocumentsToDimensions
                WHERE DimensionId = ?""", (dim_id,))
        freq = int(c.fetchone()[0])
        idf = log10(num_total_docs/(1+freq))
        c.execute(
                'UPDATE Dimensions SET IDF = ? WHERE DimensionId = ?',
                (idf, dim_id))

    #
    # Save and exit.
    #
    conn.commit()
    c.close()

def main_process(nlp, document_ids, fpath, stemmer, all_include, pos_tag, do_bigrams, do_trigrams, all_dim):
    """Read document numbers from input_queue, read the actual documents from the database, process_document them, write back to the database."""
    documents = {}
    value_list = []
    conn = cursor = None
    while True:
        delay = 1
        try:
            conn = sqlite3.connect(fpath)
            cursor = conn.cursor()
            for doc_number in document_ids:
                documents[doc_number] = cursor.execute("SELECT NOTE_TEXT FROM Documents WHERE ED_ENC_NUM = ?", (doc_number,)).fetchone()[0]
            break
        except sqlite3.OperationalError:
            print ("index_document(): database is locked, trying again in %ds" %delay)
            time.sleep(delay)
            delay += 1
        finally:
            if cursor:
                cursor.close()
            if conn:
                conn.close()
    value_list = []

    for ordinal, doc_number in enumerate(documents):
        proc = process_document_spacy(nlp, documents[doc_number], stemmer, all_include, pos_tag, do_bigrams, do_trigrams)
        for dimension, term, pos in all_dim:
            if pos == 'bigram':
                count = proc['bigrams_counter'][term]
            elif pos == 'trigram':
                count = proc['trigrams_counter'][term]
            elif pos == 'regex':
                count = proc['inclusions_counter'][term]
            else:
                count = proc['stemmed_counter'][(pos, term)]
            if not count:
                continue
            value_list.append((dimension, doc_number, count))

    conn = cursor = None
    while True:
        delay = 1
        try:
            conn = sqlite3.connect(fpath)
            cursor = conn.cursor()
            cursor.executemany('INSERT INTO DocumentsToDimensions VALUES ( ?, ?, ? )', value_list)
            conn.commit()
            break
        except sqlite3.OperationalError:
            print ("index_document(): database is locked, trying again in %ds" % delay)
            time.sleep(delay)
            delay += 1
        finally:
            if cursor:
                cursor.close()
            if conn:
                conn.close()
    print ("index_document():", len(documents))
def main():
    parser = create_parser('usage: %s file.sqlite3 [options]' % __file__)
    options, args = parser.parse_args()
    if not len(args):
        parser.error('invalid number of arguments')
    index(args[0], options)
    return 0

if __name__ == '__main__':
    sys.exit(main())
