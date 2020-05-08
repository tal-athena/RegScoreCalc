"""
Classifies documents in a test database.
"""

import sqlite3
import sys
import subprocess as sub
import re

import os
import os.path as P

import util
from constants import SVM_CLASSIFY

REGEX = re.compile(r'^Precision/recall on test set: (\d+\.\d+)%/(\d+\.\d+)%$')

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
            '--test-results', 
            '-t', 
            dest='test_results', 
            default=False,
            action='store_true',
            help='Print precision and recall.  Do NOT modify the database')
    return parser

def classify(path, svm_test, svm_classifier, test_results, temporary_dir):
    tmp_file = P.join(temporary_dir, 'svm_classify.txt')
    conn = sqlite3.connect(path)
    p = sub.Popen([SVM_CLASSIFY, svm_test, svm_classifier, tmp_file],
            stderr=sub.PIPE, stdout=sub.PIPE)
    stdout, stderr = p.communicate()
    if stderr:
        print >> sys.stderr, stderr
        return

    precision, recall = -1, -1
    for line in stdout.split('\n'):
        print (line)
        match = REGEX.match(line.strip())
        if match:
            precision, recall = match.groups()

    c = conn.cursor()
    params = util.get_params(c, path)
    scores = map(lambda f: int(float(f)/params['CLASSIFY_CLIP']*100), 
            open(tmp_file).read().strip().split('\n'))

    if test_results:
        assert precision >= 0 and recall >= 0
        print 'precision:', precision, 'recall:', recall
        c.execute('SELECT ED_ENC_NUM, Score FROM Documents')
        for (i, (doc_id, score)) in enumerate(c):
            if i >= len(scores):
                print >> sys.stderr, 'Premature end of training file'
                assert False
            if score < 0 and scores[i] < 0:
                continue
            elif score > 0 and scores[i] > 0:
                continue
            print doc_id, 'expected:', score, 'actual:', scores[i]
    else:
        c_inner = conn.cursor()
        c.execute('SELECT ED_ENC_NUM FROM Documents')
        for i, (doc_id,) in enumerate(c):
            if i >= len(scores):
                print >> sys.stderr, 'Premature end of training file'
                assert False
            c_inner.execute('UPDATE Documents SET Score = ? WHERE ED_ENC_NUM = ?',
                    (scores[i], doc_id))
        c_inner.close()

    c.close()
    conn.commit()

def main():
    parser = create_parser('usage: %s test.sqlite3 svm_test svm_classifier [options]' % __file__)
    options, args = parser.parse_args()
    if len(args) != 3:
        parser.error('invalid number of arguments')
    classify(args[0], args[1], args[2], options.test_results, P.dirname(args[0]))
    return 0

if __name__ == '__main__':
    sys.exit(main())
