import os.path as P
import os
import sqlite3
import datetime
import json
from medacy.ner.model import Model

def create_parser():
    from optparse import OptionParser
    p = OptionParser("usage: python %prog training.sqlite3 test.sqlite3 [options]")
    p.add_option("-t", "--temporary-dir", type="string", dest="temporary_dir", default=None, help="Specify the directory to use for storing temporary files")
    return p

def main():
    parser = create_parser()
    opts, args = parser.parse_args()
    if len(args) != 1:
        parser.error("invalid number of arguments")
    
    model = Model.load_external('medacy_model_clinical_notes')
    
    sqlite_file = args[0];

    conn = sqlite3.connect(sqlite_file);
    cursor = conn.cursor()

    cursor.execute('SELECT ED_ENC_NUM, NOTE_TEXT FROM Documents')
    for i, (num, raw) in enumerate(cursor):        
        annotation = model.predict(raw)
        #print (json.dumps(annotation.annotations))
        #entities = annotation.entities;
        conn.execute('UPDATE Documents SET Result=? WHERE ED_ENC_NUM = ?', (json.dumps(annotation.annotations), num))
        if i % 20 == 0:
            conn.commit()
    conn.commit()
    conn.close();
   
if __name__ == "__main__":
    import sys
    sys.exit(main())

