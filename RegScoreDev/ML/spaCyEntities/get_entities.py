import os.path as P
import sqlite3
import json
import spacy

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
    
    nlp = spacy.load("en")
    
    sqlite_file = args[0];

    conn = sqlite3.connect(sqlite_file);

    get_entity_names(conn, nlp)

    cursor = conn.cursor()

    total_cnt = cursor.execute("SELECT COUNT(*) FROM Documents").fetchone()[0]

    cursor.execute('SELECT ED_ENC_NUM, NOTE_TEXT FROM Documents')

    step = int(total_cnt / 50 + 1);
    print (step);
    for i, (num, raw) in enumerate(cursor):

        split_string = [raw[j: j + 1000000] for j in range(0, len(raw), 1000000)]

        cur_pos = 0
        results = []
        for split_doc in split_string:

            doc = nlp(split_doc);     
            
            for ent in doc.ents:
                results.append(EntityResults(ent.text, ent.label_, ent.start_char + cur_pos, ent.end_char + cur_pos).__dict__)

            cur_pos += 1000000

        conn.execute('UPDATE Documents SET Result=? WHERE ED_ENC_NUM = ?', (json.dumps(results), num))
        if i % 50 == 0:
            conn.commit()
        if i % step == 0:            
            print(int(1.0 * i / total_cnt * 50))        
    conn.commit()
    conn.close();

class EntityResults:
    text = None
    label = None
    start = None
    end = None
    def __init__(self, text, label, start, end):
        self.text = text
        self.label = label
        self.start = start
        self.end = end    

def get_entity_names(conn, nlp):
    sql_create_projects_table = """ CREATE TABLE IF NOT EXISTS EntityNames (
                                        id integer PRIMARY KEY AUTOINCREMENT,
                                        label text
                                    ); """
    create_table(conn, sql_create_projects_table)
    
    c = conn.cursor()

    c.execute("DELETE FROM EntityNames")
    conn.commit()

    if "ner" not in nlp.pipe_names:
        ner = nlp.create_pipe("ner")
        nlp.add_pipe(ner, last=True)
    # otherwise, get it so we can add labels
    else:
        ner = nlp.get_pipe("ner")
    
    for label in ner.labels:
        c.execute('INSERT INTO EntityNames (label) VALUES(?)', (label,))

    conn.commit()
def create_table(conn, create_table_sql):
    """ create a table from the create_table_sql statement
    :param conn: Connection object
    :param create_table_sql: a CREATE TABLE statement
    :return:
    """
    try:
        c = conn.cursor()
        c.execute(create_table_sql)        
        conn.commit()
    except sqlite3.OperationalError as e:
        print(e)

if __name__ == "__main__":
    import sys
    sys.exit(main())

