using System;
using System.Collections.Generic;
using System.Linq;

namespace Tabulator
{
    class Vote
    {
        // a vote is a List<List<String>>
        public List<String> Names = new List<String>();
        readonly char NAME_SEPARATOR = ']';
        public Vote(string line)
        {
            List<string> nameList = new List<string>();
            string buildName = "";
            string state = "start";
            foreach (char c in line)
            {
                if (state == "start")
                {
                    if (c == ',')
                    {
                        nameList.Add(buildName.Trim('"'));
                        buildName = "";
                        state = "start";
                    }
                    else if (c == '"')
                    {
                        buildName += c;
                        state = "insideQuoted";
                    }
                    else
                    {
                        buildName += c;
                        state = "insideUnquoted";
                    }
                }
                else if (state == "insideUnquoted")
                {
                    if (c == ',')
                    {
                        nameList.Add(buildName.Trim('"'));
                        buildName = "";
                        state = "start";
                    }
                    else
                    {
                        buildName += c;
                        state = "insideUnquoted";
                    }
                }
                else if (state == "insideQuoted")
                {
                    if (c == '"')
                    {
                        buildName += c;
                        state = "maybeOutsideQuoted";
                    }
                    else
                    {
                        buildName += c;
                        state = "insideQuoted";
                    }
                }
                else if (state == "maybeOutsideQuoted")
                {
                    if (c == '"')
                    {
                        state = "insideQuoted";
                    }
                    else if (c == ',')
                    {
                        nameList.Add(buildName.Trim('"'));
                        buildName = "";
                        state = "start";
                    }
                }
            }

            if (!String.IsNullOrEmpty(buildName))
            {
                nameList.Add(buildName.Trim('"'));
            }

            // remove the first two elements in NameList because they are the image # and precinct code
            nameList.RemoveRange(0, 2);

            Names = nameList;
        }

        public void Display()
        {
            for (int i = 0; i < Names.Count; i++)
            {
                Console.WriteLine($"   Vote.Names[{i}]: {Names[i]}");
            }
        }

        public string FirstChoice()
        {
            // use Aggregate() in case there is more than 1 candidate for 1st Choice
            if (Names.Count == 0)
            {
                return "";
            }
            else if (Names[0].Contains(NAME_SEPARATOR))
            {
                return "";
            } 
            else
            {
                if (Names[0] == "")
                {
                    if (String.IsNullOrEmpty(Names[1]) || Names[1].Contains(NAME_SEPARATOR))
                    {
                        return "";
                    }
                    else
                    {
                        return Names[1];
                    }
                }
                else
                {
                    return Names[0];
                }
            }
        }

        public Vote Eliminate(string nameToEliminate)
        {

            Names.RemoveAll(x => x == nameToEliminate); // to remove exact matches

            // to find and remove matches where there is more than 1 candidate / rank
            for (int i = 0; i < Names.Count; i++) 
            {
                if (Names[i].Contains(nameToEliminate) && Names[i].Contains(NAME_SEPARATOR))
                {
                    List<string> split = Names[i].Split(NAME_SEPARATOR).ToList();
                    split.RemoveAll(x => x == nameToEliminate);
                    Names[i] = String.Join(NAME_SEPARATOR, split); //rebuild the string
                }
            }
            return this;
        }
    }
}