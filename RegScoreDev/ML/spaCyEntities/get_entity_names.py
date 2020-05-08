import os.path as P
import sqlite3
import json
import spacy

def main():

    nlp = spacy.load("en")
    
    if "ner" not in nlp.pipe_names:
        ner = nlp.create_pipe("ner")
        nlp.add_pipe(ner, last=True)
    # otherwise, get it so we can add labels
    else:
        ner = nlp.get_pipe("ner")
    
    print('--------------------------');
    for label in ner.labels:
        print(label, end='\n')   
    print('--------------------------');

if __name__ == "__main__":
    import sys
    sys.exit(main())

