using System;
using System.Collections.Generic;
using System.Linq;

namespace Tabulator
{
    class Vote
    {
        // a vote is a List<List<String>>
        List<List<String>> Item = new List<List<String>>();
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

            foreach (string str in nameList)
            {
                if (str.Contains(NAME_SEPARATOR))
                {
                    Item.Add(str.Split(NAME_SEPARATOR).ToList());
                }
                else
                {
                    Item.Add(new List<string> { str });
                }
            }
        }

        public void Display()
        {
            for (int i = 0; i < Item.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Vote.Item[{i}] Count:{Item[i].Count}");
                for (int j = 0; j < Item[i].Count; j++)
                {
                    Console.WriteLine($"   {j}:{Item[i][j]}");
                }
            }
        }

        public string FirstChoice()
        {
            // use Aggregate() in case there is more than 1 candidate for 1st Choice
            if (Item.Count < 1)
            {
                return "THIS VOTE IS EMPTY!";
            }
            else 
            {
                return Item[0].Aggregate((message, name) => $"{message} & {name}");
            }
            
        }

        public Vote Eliminate(string name)
        {
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].RemoveAll(x => x == name);
            }

            // clean up choices that have null candidates
            Item.RemoveAll(x => x.Count == 0);
            return this;
        }

        public bool IsExhausted()
        {
            return Item[0].Count > 1;
        }
    }
}