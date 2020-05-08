"""
Stems documents and determines the total dimension.  
Initializes tables in the database.
"""

import sqlite3
import sys
import nltk
import util
import re
import collections

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
            '--pos-tag',
            '-p',
            dest='pos_tag',
            default=False,
            action='store_true',
            help='Perform POS tagging (slow)')
    return parser

def calc_trigrams(words):
    result = []
    for i in range(3, len(words)):
        result.append(tuple(words[i-3:i]))
    return result

def match_exclude_regex(pos, term, exclude_regex):
    """
    Returns True if the specified (PartOfSpeech, Term) matches an exclusion
    regex, False otherwise.
    """
    for regex, parts_of_speech in exclude_regex:
        if not set(['*', pos]).intersection(set(parts_of_speech)):
            continue
        if regex.match(term):
            return True
    return False

def search_include_regex(raw, include_regex):
    """Returns a list of all occurences of all the specified regexes in raw."""
    result = []
    for regex in include_regex:
        result += regex.findall(raw)
    return result

def process_document(
        raw, stemmer, include, pos_tag, do_bigrams=False, do_trigrams=False):
    """Tokenize, tag, stem, etc.  Returns a dictionary of results."""
    tokens = nltk.wordpunct_tokenize(raw.lower())
    text = nltk.Text(tokens)
    if pos_tag:
        pos_tagged = nltk.pos_tag(text)
        stemmed = [(pos, stemmer.stem(term)) for (pos, term) in pos_tagged]
    else:
        stemmed = [("unigram", stemmer.stem(term)) for term in tokens]
    inclusions = search_include_regex(raw, include)

    bigrams = []
    trigrams = []
    if do_bigrams:
        bigrams = [ ' '.join(b) for b in nltk.bigrams(text) ]
    if do_trigrams:
        trigrams = [ ' '.join(t) for t in calc_trigrams(text) ]

    stemmed_counter = collections.Counter(stemmed)
    inclusions_counter = collections.Counter(inclusions)
    bigrams_counter = collections.Counter(bigrams)
    trigrams_counter = collections.Counter(trigrams)
    result = {"stemmed": stemmed,
              "bigrams": bigrams,
              "trigrams": trigrams,
              "inclusions": inclusions,
              "stemmed_counter": stemmed_counter,
              "bigrams_counter": bigrams_counter,
              "trigrams_counter": trigrams_counter,
              "inclusions_counter": inclusions_counter}
    return result

def init_dim(c):
    """
    Initializes the DocumentsToDimensions and Dimensions tables.
    If they exist, they are destroyed.
    """
    util.drop_tables(c, [ 'DocumentsToDimensions', 'Dimensions' ])
    c.execute("""CREATE TABLE Dimensions (
            DimensionId LONG PRIMARY KEY,
            Term TEXT, 
            PartOfSpeech TEXT, 
            Exclude BOOLEAN,
            IDF FLOAT,
            MRMR FLOAT
        )""")

    c.execute("""CREATE TABLE DocumentsToDimensions (
            DimensionId LONG, 
            ED_ENC_NUM DOUBLE,
            Count INTEGER,
            FOREIGN KEY(DimensionId) REFERENCES Dimensions(DimensionId),
            FOREIGN KEY(ED_ENC_NUM) REFERENCES Documents(ED_ENC_NUM),
            PRIMARY KEY(DimensionId, ED_ENC_NUM)
        )""")

def populate_dim(c, words, bigrams, trigrams, inclusions, exclude_regex):
    """Populates the Dimensions table."""

    cmd = 'INSERT INTO Dimensions VALUES (?, ?, ?, ?, 0, 0)'
    for i,(pos,term) in enumerate(words):
        excluded = match_exclude_regex(pos, term, exclude_regex)
        #
        # Feature numbers must start with 1 in SVMlight.
        #
        c.execute(cmd, (i+1, term, pos, excluded))

    for pos, terms in zip(['bigram', 'trigram'], [bigrams, trigrams]):
        for term in terms:            
            i += 1
            excluded = match_exclude_regex(pos, term, exclude_regex)
            c.execute(cmd, (i+1, term, pos, excluded))

    for term in inclusions:
        i += 1
        c.execute(cmd, (i+1, term, 'regex', 0))

def calc_dim(path, limit=0, pos_tag=False, language = 'en'):
    """
    Stems each document.  Determines all possible dimensions.  Creates
    dimensions-related tables and populates them.

    conn    The connection to the database to work with.
    stemmer The stemmer to use.
    limit   The number of documents to process.  If zero, all documents.
    bigrams Process bigrams.
    trigrams
            Process trigrams.
    """
    conn = sqlite3.connect(path)
    c = conn.cursor()
    params = util.get_params(c, path)

    stemmer = params['stemmer']
    bigrams = params['bigrams']
    trigrams = params['trigrams']
    print 'calc_dim(): stemmer: %s bigrams: %s trigrams: %s' % (
            stemmer, bigrams, trigrams)

    exclude = util.get_all_exclude_regex(c)
    include = util.get_all_include_regex(c)

    stemmers = { 'porter' : nltk.PorterStemmer(), 
            'lancaster' : nltk.LancasterStemmer() }
    try:
        stemmer = stemmers[stemmer]
    except KeyError:
        print 'unsupported stemmer:', stemmer
        return 1

    num_doc = 0
    c.execute('SELECT ED_ENC_NUM FROM Documents')
    for doc in c:
        num_doc += 1
    cmd = 'SELECT ED_ENC_NUM, NOTE_TEXT, Score FROM Documents'
    if limit:
        cmd += ' LIMIT %d' % limit
        num_doc = min(limit, num_doc)
    c.execute(cmd)

    all_words = set()
    all_bigrams = set()
    all_trigrams = set()
    all_inclusions = set()

    all_doc = []
    for i, (num, raw, score) in enumerate(c):
        if i % 100 == 0:
            print 'calc_dim(): processing document %s (%d/%d)' % (str(num), i+1, num_doc)
        proc = process_document(
                raw, stemmer, include, pos_tag, bigrams, trigrams)
        all_words = all_words.union(set(proc['stemmed']))

        all_inclusions = all_inclusions.union(set(proc['inclusions']))

        if bigrams:
            all_bigrams = all_bigrams.union(set(proc['bigrams']))
        if trigrams:
            all_trigrams = all_trigrams.union(set(proc['trigrams']))

        all_doc.append(num)

    all_words = list(all_words)
    all_words.sort()

    all_bigrams = list(all_bigrams)
    all_bigrams.sort()

    all_trigrams = list(all_trigrams)
    all_trigrams.sort()

    all_inclusions = list(all_inclusions)
    all_inclusions.sort()

    init_dim(c)
    populate_dim(c, all_words, all_bigrams, 
            all_trigrams, all_inclusions, exclude)

    c.execute("SELECT COUNT(*) FROM Dimensions")
    nDims = int(c.fetchone()[0])
    #
    # Save and exit.
    #
    c.close()
    conn.commit()

def main():
    parser = create_parser('usage: %s file.sqlite3 [options]' % __file__)
    options, args = parser.parse_args()
    if not len(args):
        parser.error('invalid number of arguments')
    calc_dim(args[0], options.limit, options.pos_tag)
    return 0

if __name__ == '__main__':
    sys.exit(main())
