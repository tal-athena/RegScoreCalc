using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegScoreCalc.Helpers
{
    public class CalculateFilter
    {
        ViewsManager _views;
        private int _filterId;
        public CalculateFilter(ViewsManager views)
        {
            _views = views;
            
        }
        public void Calculate(int filterId, BackgroundWorker currentWorker)
        {

            _filterId = filterId;

            //Delete previous values from DocsToFilters table where FilterId == filterId
            DeleteFromDocsToFilters(filterId);


            //Create dictionary
            CalculationDictionary dictionary = new CalculationDictionary();
            Dictionary<double, ColorReason> excludedDocsDictionary = new Dictionary<double, ColorReason>();


            //1. Get Groups for current filter
            var filter = _views.MainForm.datasetBilling.ICDFilters.FindByFilterID(filterId);
	        var groupIDs = !filter.IsGroupIDsNull() ? filter.GroupIDs.Split(',') : new string[] { };
            if (filter.GroupIDs.Length > 0)
            {
				//Loop 1
                Loop1(dictionary, groupIDs);
                currentWorker.ReportProgress(30);

                //Loop 2
                Loop2(dictionary, groupIDs);
                currentWorker.ReportProgress(60);


                //Loop 3
                Loop3(filterId, excludedDocsDictionary);
                currentWorker.ReportProgress(90);

                //Merge
                Merge(dictionary, excludedDocsDictionary, groupIDs);

                //Write this to the database
                SaveToDatabase(filterId, excludedDocsDictionary);
                //SaveDictionaryToDatabase(filterId, dictionary);
            }
        }

        //Save to DB
        private void SaveDictionaryToDatabase(int filterId, CalculationDictionary dictionary)
        {
            var dictionaryItems = dictionary.GetDictionary();
            foreach (KeyValuePair<DictionaryKey, ColorReason> item in dictionaryItems)
            {
                var newRedRow = _views.MainForm.datasetBilling.DocsToFilters.NewDocsToFiltersRow();
                newRedRow.ED_ENC_NUM = item.Key.ED_ENC_NUM;
                newRedRow.FilterID = filterId;
                newRedRow.Type = item.Value.Color;
                newRedRow.Reason = item.Value.Reason;

                _views.MainForm.datasetBilling.DocsToFilters.Rows.Add(newRedRow);
            }
            //Save data to database
            try
            {
                _views.MainForm.adapterDocsToFiltersBilling.Update(_views.MainForm.datasetBilling.DocsToFilters);
            }
			catch (Exception ex)
			{
				MainForm.ShowExceptionMessage(ex);
			}
		}
        private void SaveToDatabase(int filterId, Dictionary<double, ColorReason> excludedDocsDictionary)
        {
            foreach (KeyValuePair<double, ColorReason> item in excludedDocsDictionary)
            {
                var newRedRow = _views.MainForm.datasetBilling.DocsToFilters.NewDocsToFiltersRow();
                newRedRow.ED_ENC_NUM = item.Key;
                newRedRow.FilterID = filterId;
                newRedRow.Type = item.Value.Color;
                newRedRow.Reason = item.Value.Reason;

                _views.MainForm.datasetBilling.DocsToFilters.Rows.Add(newRedRow);
            }

            //Save data to database
	        try
	        {
		        _views.MainForm.adapterDocsToFiltersBilling.Update(_views.MainForm.datasetBilling.DocsToFilters);
		        _views.MainForm.adapterDocsToFiltersBilling.Fill(_views.MainForm.datasetBilling.DocsToFilters);
	        }
	        catch (Exception ex)
	        {
		        MainForm.ShowExceptionMessage(ex);
	        }
        }

        private void Merge(CalculationDictionary dictionary, Dictionary<double, ColorReason> excludedDocsDictionary, string[] groupIDs)
        {
            foreach (var groupID in groupIDs)
            {
                //Create group
                Group currentGroup = new Group(_views, groupID);

                //Get only for this group
                CalculationDictionary dictionaryOfCurrentGroup = currentGroup.GetItemsForCurrentGroup(groupID, dictionary);
                var dictionaryOfCurrentGroupObject = dictionaryOfCurrentGroup.GetDictionary();

                foreach (KeyValuePair<DictionaryKey, ColorReason> item in dictionaryOfCurrentGroupObject)
                {
                    //                     If (entry.ED_ENC_NUM and current filter is in dictionary[document-ED_ENC_NUM, FilterID] 
                    //and the entry in dictionary[document-ED_ENC_NUM, FilterID] is Red or Exclude) Then
                    //CONTINUE; /* Red and exclude are not over written by green at this point */
                    try
                    {
                        ColorReason colorReason;
                        bool exc = excludedDocsDictionary.TryGetValue(item.Key.ED_ENC_NUM, out colorReason);
                        if (exc != false)
                        {
                            if (excludedDocsDictionary[item.Key.ED_ENC_NUM].Color == "RED")
                            {
                                continue;
                            }
                            else if (excludedDocsDictionary[item.Key.ED_ENC_NUM].Color == "EXCLUDE")
                            {
                                continue;
                            }
                            //2. If (DOCUMENT is already in dictionary) AND color is green AND Category of document is not in the "Excluded" group 
                            //Then:  assign to it color/reason from the dictionary[groupID,ED_ENC_NUM] entry (same as above)
                            else if (excludedDocsDictionary[item.Key.ED_ENC_NUM].Color == "GREEN")
                            {
                                //Get document
                                var document = _views.MainForm.datasetBilling.Documents.FindByPrimaryKey(item.Key.ED_ENC_NUM);

                                //Check if category is in excluded group
                                  var excludedCategory = ExcludedByCategoryType(document.Category, _filterId);
                                  if (excludedCategory.Length == 0)
                                  {
                                      excludedDocsDictionary[item.Key.ED_ENC_NUM] = item.Value;
                                  }
                            }
                        }
                        else
                        {
                            //1. Is DOCUMENT is NOT already in dictionary - assign to it color/reason from the dictionary[groupID,ED_ENC_NUM] entry
                            excludedDocsDictionary.Add(item.Key.ED_ENC_NUM, item.Value);
                        }

                    }
					catch (Exception ex)
					{
						MainForm.ShowExceptionMessage(ex);
					}
				}
            }
        }

        private void Loop3(int filterId, Dictionary<double, ColorReason> excludedDocsDictionary)
        {
            foreach (var document in _views.MainForm.datasetBilling.Documents)
            {
                //Check if the category is excluded
	            if (!document.IsCategoryNull())
	            {
					var excludedCategory = ExcludedByCategoryType(document.Category, filterId);
					if (excludedCategory.Length > 0)
					{
						if (excludedCategory == "Concordant")
						{
							//Make it green
							var reason = "Entered Manually";
							excludedDocsDictionary.Add(document.ED_ENC_NUM, new ColorReason() { Color = "GREEN", Reason = reason });
						}
						else if (excludedCategory == "Discordant")
						{
							//Make it red
							var reason = "Entered Manually";
							excludedDocsDictionary.Add(document.ED_ENC_NUM, new ColorReason() { Color = "RED", Reason = reason });
						}
						else
						{
							var reason = "EXCLUDE";
							excludedDocsDictionary.Add(document.ED_ENC_NUM, new ColorReason() { Color = "EXCLUDE", Reason = reason });
						}
					}
				}
            }
        }

        private void Loop2(CalculationDictionary dictionary, string[] groupIDs)
        {
            foreach (var groupID in groupIDs)
            {
                //Create group
                Group currentGroup = new Group(_views, groupID);

                foreach (var document in _views.MainForm.datasetBilling.Documents)
                {
                    //If (Doc has one-way or not one-way code form Group) Then dictionary[groupID,ED_ENC_NUM] = Green
                    var icdInStandard = currentGroup.DocumentHaveICDInStandardWithoutOneWay(document);
                    if (icdInStandard.Length > 0)
                    {
                        // I would like to check here if dictionary already has an entry for groupID, document.ED_ENC_NUM which is green
                        // If it does then - let's leave the entry from Loop 1 because the reason there is more complete - it also indicates
                        // that DISC has the code. Can we do that?   Ok? Looks good - let's try
                        // Looks good
                        // One thing: in the reason, it is important to see DISC: first
                        // we should probably change that in Loop1 - possible?
                        var dictionaryKey = new DictionaryKey(){ GroupID = groupID, ED_ENC_NUM = document.ED_ENC_NUM};

                        //If dictionary have this key
                        if (dictionary.GetDictionary().ContainsKey(dictionaryKey))
                        {
                            //Check for the color of the entry with this key
                            if (dictionary.GetDictionary()[dictionaryKey].Color != "GREEN")
                            {
                                //Make it green
                                var reason = "EDMD: " + icdInStandard;
                                dictionary.Add(groupID, document.ED_ENC_NUM, "GREEN", reason);
                            }
                        }
                        else
                        {
                            //Add new entry with color GREEN
                            var reason = "EDMD: " + icdInStandard;
                            dictionary.Add(groupID, document.ED_ENC_NUM, "GREEN", reason);
                        }
                    }
                }
            }
        }

        private void Loop1(CalculationDictionary dictionary, string[] groupIDs)
        {
            foreach (var groupID in groupIDs)
            {

                Group currentGroup = new Group(_views, groupID);

                //Get all rows from billing table
                foreach (RegScoreCalc.Data.BillingDataSet.BillingRow billingRow in _views.MainForm.datasetBilling.Billing)
                {
                    //Check if the billingROw have the ICD code
                    if (billingRow.ICD9.Length > 0)
                    {

                        //Check if the ICD9Code of line is in current group - HERE WE do not need to check One-way
                        if (currentGroup.IsICD9InGroupSingleWithoutOneWay(billingRow.ICD9))
                        {

                            //Remember the billing code that is in the standard list
                            var billingICD9Code = billingRow.ICD9;

                            //Get document with ED_ENC_NUM
							var document = _views.MainForm.datasetBilling.Documents.FindByPrimaryKey(billingRow.ED_ENC_NUM);

                            //If (Document with ED_ENC_NUM of line has single-code (one-way or not) in Group)
                            //Then dictionary[groupID,ED_ENC_NUM] = Green
                            var documentICDInStandard = currentGroup.DocumentHaveICDInStandardGroup(document);
                            var documentICDInCombinations = currentGroup.DocumentHaveICDInCombinationGroup(document);
                            if (documentICDInStandard.Length > 0)
                            {
                                //Make it green
                                var reason = "DISC: " + billingICD9Code + " EDMD: " + documentICDInStandard;
                                dictionary.Add(groupID, document.ED_ENC_NUM, "GREEN", reason);
                            }
                            else if (documentICDInCombinations.Length > 0)// If (Document with ED_ENC_NUM of line has combination in Group) Then dictionary[groupID,ED_ENC_NUM] = Green
                            {
                                //Make it green (excellent!!! - We can see already how faster it is to fine tune the filter calculation after our refactoring!!!)
                                // Can you open notepad so we could chat?
                                var reason =  "DISC: " + billingICD9Code + " EDMD: " + documentICDInCombinations;
                                dictionary.Add(groupID, document.ED_ENC_NUM, "GREEN", reason);
                            }
                            else
                            {
                                //Make it red
                                var reason = "DISC: " + billingICD9Code;
                                dictionary.Add(groupID, document.ED_ENC_NUM, "RED", reason);
                            }
                        }
                    }
                }
            }
        }


        private string ExcludedByCategoryType(int categoryId, int filterId)
        {
            var row = _views.MainForm.datasetBilling.CategoryToFilterExclusion.Select("CategoryID = " + categoryId.ToString() + " AND FilterID = " + filterId.ToString());
            if (row.Length > 0)
            {
                return ((RegScoreCalc.Data.BillingDataSet.CategoryToFilterExclusionRow)row[0]).Type;
            }
            return "";
        }


        //Delete all from DocsToFilters where FilteID == filterId
        public void DeleteFromDocsToFilters(int filterId)
        {
            var connection = _views.MainForm.adapterDocsToFiltersBilling.Connection;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            OleDbCommand cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM DocsToFilters WHERE FilterID = @filterID";
            cmd.Parameters.Clear();
            cmd.Parameters.Add(new OleDbParameter("@filterID", filterId.ToString()));
            cmd.ExecuteNonQuery();

            _views.MainForm.adapterDocsToFiltersBilling.Fill(_views.MainForm.datasetBilling.DocsToFilters);
        }
    }



    public class CalculationDictionary
    {
        private Dictionary<DictionaryKey, ColorReason> dictionary;

        public CalculationDictionary()
        {
            dictionary = new Dictionary<DictionaryKey, ColorReason>();
        }

        public Dictionary<DictionaryKey, ColorReason> GetDictionary()
        {
            return dictionary;
        }

        public void Add(string groupID, double ed_enc_no, string color, string reason)
        {
            var colorReason = new ColorReason() { Color = color, Reason = reason };
            var key = new DictionaryKey()
            {
                ED_ENC_NUM = ed_enc_no,
                GroupID = groupID
            };
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = colorReason;
            }
            else
            {
                dictionary.Add(key, colorReason);
            }

        }
    }

    public class DictionaryKey
    {
        public string GroupID { get; set; }
        public double ED_ENC_NUM { get; set; }

        public override int GetHashCode()
        {
            return ED_ENC_NUM.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            DictionaryKey other = obj as DictionaryKey;
            return other != null && other.ED_ENC_NUM == this.ED_ENC_NUM && other.GroupID == this.GroupID;
        }
    }
    public struct ColorReason
    {
        public string Color;
        public string Reason;
    }
    public struct ICD9CodeStandard
    {
        public string ICD;
        public bool RegExp;
    }
    public struct ICD9CodeCombination
    {
        public List<string> ICDs;
        public bool RegExp;
    }

    public class Group
    {
        ViewsManager _views;
        string[] separator = new string[] { " + " };
        private List<ICD9CodeStandard> Standard { get; set; }
        private List<ICD9CodeStandard> SingleWithoutOneWay { get; set; }
        private List<ICD9CodeCombination> Combinations { get; set; }


        public Group(ViewsManager views, string groupID)
        {
            //Standard - Single WITH One-Way
            Standard = new List<ICD9CodeStandard>();

            //SingleWithoutOneWay - Single WITHOUT One-Way
            SingleWithoutOneWay = new List<ICD9CodeStandard>();
            Combinations = new List<ICD9CodeCombination>();
            _views = views;

            //Get codes for current group
            var codes = (RegScoreCalc.Data.BillingDataSet.ICDCodesRow[])_views.MainForm.datasetBilling.ICDCodes.Select("GroupID = " + groupID);

            //get all standardCodes -- HAVE BOTH ONE-WAY and STANDARD
            var standardCodes = codes.Where(p => p.Combination == false).ToList();
            foreach (var code in standardCodes)
            {
                Standard.Add(new ICD9CodeStandard()
                {
                    ICD = code.ICD9Code,
                    RegExp = code.RegExp
                });
            }

            //Have only standard codes that are not ONE-WAY
            var oneWay = codes.Where(p => p.Combination == false && p.OneWay == false).ToList();
            foreach (var code in oneWay)
            {
                SingleWithoutOneWay.Add(new ICD9CodeStandard()
                {
                    ICD = code.ICD9Code,
                    RegExp = code.RegExp
                });
            }

            //get all Combinations
            var combinationCodes = codes.Where(p => p.Combination == true).ToList();
            foreach (var code in combinationCodes)
            {
                var strList = new List<string>();
                var combinationCodesArray = code.ICD9Code.Split(separator, StringSplitOptions.None);
                foreach (var combinationCode in combinationCodesArray)
                {
                    strList.Add(combinationCode);
                }

                Combinations.Add(new ICD9CodeCombination()
                {
                    ICDs = strList,
                    RegExp = code.RegExp
                });
            }

        }


        // Checks only standard ICD9 codes list
        public bool IsStandardICD9InGroup(string ICD9)
        {
            foreach (var code in Standard)
            {
                if (code.RegExp)
                {
                    Regex regex = new Regex(code.ICD);
                    if (regex.Match(ICD9).Success)
                    {
                        return true;
                    }
                }
                else
                {
                    if (code.ICD == ICD9)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Checks in single one-way list
        public bool IsICD9InGroupSingleWithoutOneWay(string ICD9)
        {
            foreach (var code in SingleWithoutOneWay)
            {
                if (code.RegExp)
                {
                    Regex regex = new Regex(code.ICD);
                    if (regex.Match(ICD9).Success)
                    {
                        return true;
                    }
                }
                else
                {
                    if (code.ICD == ICD9)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //// loops over all the group combinations and checks if one of them is in the input list
        //public bool IsGroupCombinationExist(List<string> allICD9CodesOfDoc)
        //{
        //    return false;
        //}

       



        //Return "" if no match and ICD if there is match
        public string DocumentHaveICDInStandardGroup(RegScoreCalc.Data.BillingDataSet.DocumentsRow document)
        {
            foreach (var code in Standard)
            {
                if (code.RegExp)
                {
                    Regex regex = new Regex(code.ICD);
                    if (!String.IsNullOrEmpty(document.ICD9_1) && regex.Match(document.ICD9_1).Success)
                    {
                        return document.ICD9_1;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_2) && regex.Match(document.ICD9_2).Success)
                    {
                        return document.ICD9_2;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_3) && regex.Match(document.ICD9_3).Success)
                    {
                        return document.ICD9_3;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_4) && regex.Match(document.ICD9_4).Success)
                    {
                        return document.ICD9_4;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_5) && regex.Match(document.ICD9_5).Success)
                    {
                        return document.ICD9_5;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_6) && regex.Match(document.ICD9_6).Success)
                    {
                        return document.ICD9_6;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_7) && regex.Match(document.ICD9_7).Success)
                    {
                        return document.ICD9_7;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_8) && regex.Match(document.ICD9_8).Success)
                    {
                        return document.ICD9_8;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_9) && regex.Match(document.ICD9_9).Success)
                    {
                        return document.ICD9_9;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_10) && regex.Match(document.ICD9_10).Success)
                    {
                        return document.ICD9_10;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_11) && regex.Match(document.ICD9_11).Success)
                    {
                        return document.ICD9_11;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_12) && regex.Match(document.ICD9_12).Success)
                    {
                        return document.ICD9_12;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_13) && regex.Match(document.ICD9_13).Success)
                    {
                        return document.ICD9_13;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_14) && regex.Match(document.ICD9_14).Success)
                    {
                        return document.ICD9_14;
                    }
                }
                else
                {
                    if (document.ICD9_1 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_2 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_3 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_4 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_5 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_6 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_7 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_8 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_9 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_10 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_11 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_12 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_13 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_14 == code.ICD)
                    {
                        return code.ICD;
                    }
                }
            }
            return "";
        }


        //Return "" if no match and ICD if there is match
        public string DocumentHaveICDInCombinationGroup(RegScoreCalc.Data.BillingDataSet.DocumentsRow document)
        {
            foreach (var code in Combinations)
            {
                var returnCode = String.Join(" + ", code.ICDs);
                bool exist = false;
                foreach (var icd in code.ICDs)
                {
                    if (code.RegExp)
                    {
                        Regex regex = new Regex(icd);
                        if (regex.Match(document.ICD9_1).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_2).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_3).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_4).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_5).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_6).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_7).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_8).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_9).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_10).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_11).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_12).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_13).Success)
                        {
                            exist = true;
                        }
                        else if (regex.Match(document.ICD9_14).Success)
                        {
                            exist = true;
                        }
                        else
                        {
                            exist = false;
                            break;
                        }
                    }
                    else
                    {
                        if (document.ICD9_1 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_2 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_3 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_4 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_5 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_6 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_7 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_8 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_9 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_10 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_11 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_12 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_13 == icd)
                        {
                            exist = true;
                        }
                        else if (document.ICD9_14 == icd)
                        {
                            exist = true;
                        }
                        else
                        {
                            exist = false;
                            break;
                        }
                    }
                }

                if (exist)
                {
                    return returnCode;
                }
            }
            return "";
        }


        public CalculationDictionary GetItemsForCurrentGroup(string groupID, CalculationDictionary dictionary)
        {
            var returnDictionary = new CalculationDictionary();
            var dicionaryObject = dictionary.GetDictionary();
            foreach (KeyValuePair<DictionaryKey, ColorReason> entry in dicionaryObject)
            {
                if (entry.Key.GroupID == groupID)
                {
                    returnDictionary.Add(entry.Key.GroupID, entry.Key.ED_ENC_NUM, entry.Value.Color, entry.Value.Reason);
                }
            }

            return returnDictionary;
        }

        public string DocumentHaveICDInStandardWithoutOneWay(Data.BillingDataSet.DocumentsRow document)
        {
            foreach (var code in SingleWithoutOneWay)
            {
                if (code.RegExp)
                {
                    Regex regex = new Regex(code.ICD);
                    if (!String.IsNullOrEmpty(document.ICD9_1) && regex.Match(document.ICD9_1).Success)
                    {
                        return document.ICD9_1;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_2) && regex.Match(document.ICD9_2).Success)
                    {
                        return document.ICD9_2;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_3) && regex.Match(document.ICD9_3).Success)
                    {
                        return document.ICD9_3;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_4) && regex.Match(document.ICD9_4).Success)
                    {
                        return document.ICD9_4;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_5) && regex.Match(document.ICD9_5).Success)
                    {
                        return document.ICD9_5;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_6) && regex.Match(document.ICD9_6).Success)
                    {
                        return document.ICD9_6;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_7) && regex.Match(document.ICD9_7).Success)
                    {
                        return document.ICD9_7;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_8) && regex.Match(document.ICD9_8).Success)
                    {
                        return document.ICD9_8;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_9) && regex.Match(document.ICD9_9).Success)
                    {
                        return document.ICD9_9;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_10) && regex.Match(document.ICD9_10).Success)
                    {
                        return document.ICD9_10;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_11) && regex.Match(document.ICD9_11).Success)
                    {
                        return document.ICD9_11;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_12) && regex.Match(document.ICD9_12).Success)
                    {
                        return document.ICD9_12;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_13) && regex.Match(document.ICD9_13).Success)
                    {
                        return document.ICD9_13;
                    }
                    if (!String.IsNullOrEmpty(document.ICD9_14) && regex.Match(document.ICD9_14).Success)
                    {
                        return document.ICD9_14;
                    }
                }
                else
                {
                    if (document.ICD9_1 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_2 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_3 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_4 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_5 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_6 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_7 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_8 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_9 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_10 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_11 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_12 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_13 == code.ICD)
                    {
                        return code.ICD;
                    }
                    if (document.ICD9_14 == code.ICD)
                    {
                        return code.ICD;
                    }
                }
            }
            return "";
        }
    }




}


