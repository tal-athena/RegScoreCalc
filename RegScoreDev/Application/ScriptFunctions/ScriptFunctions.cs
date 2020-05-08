using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ScriptFunctionLibrary
{
    public class ScriptFunctionsType
    {
        public ScriptFunctionsType()
        {
            description = "just a type";
        }
        public string description { get; set; }
    }
    public static class ScriptFunctions
    {
        private static Regex regExpDateTime = new Regex("(1[0 - 2] | 0[1 - 9] |[1 - 9]) / (3[0 - 1] | 0?[1 - 9] |[1 - 2][0 - 9]) /[0 - 9]{4}    (\\s(2[0-3]|[0-1][0-9]) :[0-5] [0-9])?", RegexOptions.Compiled);

        public static DateTime? getDateAfter(string note, string[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                string pattern = array[i];
                string[] array2 = Regex.Split(note, pattern, RegexOptions.IgnoreCase);
                int num = 0;
                string[] array3 = array2;
                for (int j = 0; j < array3.Length; j++)
                {
                    string input = array3[j];
                    if (num++ != 0)
                    {
                        Match match = regExpDateTime.Match(input);
                        if (match.Success)
                        {
                            try
                            {
                                DateTime value = DateTime.Parse(match.Value, new CultureInfo("en - US", false));
                                DateTime? result = new DateTime?(value);
                                return result;
                            }
                            catch (FormatException)
                            {
                                DateTime? result = null;
                                return result;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static string Hello()
        {
            return "hello";
        }
        public static string[] GetSentenceWith(string NOTE_TEXT, string srch, string matches_wrap = "%s")
        {
            if (srch == "")
                return null;

            string[] splitSentences = Regex.Split(NOTE_TEXT, @"(?<=[\.!\?])\s+").Where(x => x.ToLower().Contains(srch.ToLower())).ToArray();

            for (var i = 0; i < splitSentences.Count(); i ++)
            {
                var sentence = "";

                // Remove the return line

                splitSentences[i] = splitSentences[i].Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                
                var j = 0;
                while (j < splitSentences[i].Length)
                {
                    if (j + srch.Length < splitSentences[i].Length && splitSentences[i].Substring(j, srch.Length).ToLower() == srch.ToLower())
                    {
                        sentence += matches_wrap.Replace("%s", splitSentences[i].Substring(j, srch.Length));
                        j += srch.Length;
                    } else
                    {
                        sentence += splitSentences[i][j];
                        j++;
                    }
                }

                splitSentences[i] = sentence;                
            }
            
            /*          
            string retVal = string.Empty;
            foreach (string sentence in splitSentences)
            {
                retVal += sentence + "\n";
            }*/
            return splitSentences;
        }

        public static string GetDatesEvent(string NOTE_TEXT, int sentBefore = 0, int sentAfter = 0)
        {
            string exp = "(\\d{1,2}\\.\\s(Januar?|Februar?|März?|April?|Mai?|Juni?|Juli?|August?|September?|Oktober?|November?|Dezember?)\\s\\d{4})|(\\d{1,2}\\.\\d{1,2}\\.\\d{4})";

            string[] splitSentences = Regex.Split(NOTE_TEXT, @"\r\n").ToArray();
              
            string retVal = "";
            

            for (var i = 0; i < splitSentences.Length; i++)
            {
                splitSentences[i] = splitSentences[i].Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");

                var match = Regex.Match(splitSentences[i], exp);
                if (match.Success)
                {
                    var description = "";
                    for (var j = (i - sentBefore) > 0 ? (i - sentBefore) : 0 ; j <= ((i + sentAfter < splitSentences.Length - 1) ? (i + sentAfter) : splitSentences.Length - 1); j ++)
                    {
                        description += splitSentences[j] + "\n";
                    }
                    description = description.Replace(",", "");
                    retVal += match + "," + description + ",";
                }
                    
            }

            return retVal;
        }

        public static List<Tuple<string, string, string>> Help()
        {
            return new List<Tuple<string, string, string>>() {
                new Tuple<string, string, string>("GetSentenceWith", 
                                    "Return all the sentences in the text where the regular expression appears\nstring[] ScriptFunctions.GetSentenceWith(NOTE_TEXT, string searchText, string matches_wrap = \"%s\");",
                                    @"var sentences = ScriptFunctions.GetSentenceWith(NOTE_TEXT, ""beschluss"", ""<b>%s</b>"");                                        
                                        string retVal = """";
                                        foreach (string s in sentences)
                                        {
                                            retVal += s + ""\n"";
                                        }
                                        return retVal; "),
                new Tuple<string, string, string>("getDateAfter", 
                                                    "getDateAfter", 
                                                    "ScriptFunctions.getDateAfter(NOTE_TEXT, array);"),
                new Tuple<string, string, string>("GetDatesEvent", 
                                                    "Return all the sentences in the text where the dates appear\nstring ScriptFunctions.GetDatesEvent(NOTE_TEXT, before, after);", 
                                                    @"return ScriptFunctions.GetDatesEvent(NOTE_TEXT, 2, 2);"),
            };
        }
    }
}
