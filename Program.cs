using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tabulator
{
    class MainClass
    {
        public static void Main()
        {
            Console.WriteLine("Testing input file:");
            StreamReader file = new StreamReader("Mayor.csv");
            string line = file.ReadLine();
            file.Close();
            Console.WriteLine(line);
            Vote testVote = new Vote(line);
            testVote.Display();

            Console.WriteLine($"\r\nFirst Choice: {testVote.FirstChoice()}");
            Console.WriteLine("\r\nEliminate David Michael:");
            testVote = testVote.Eliminate("David Michael");
            testVote.Display();

            Console.WriteLine("\r\nIs vote exhausted? ...");
            if (testVote.IsExhausted() == true)
            {
                Console.WriteLine("Vote is exhausted! Remove this vote.");
            }
            else if (testVote.IsExhausted() == false)
            {
                Console.WriteLine("Vote is still good!");
            }

            Console.WriteLine("\r\nEliminate Roger Schlegel:");
            testVote = testVote.Eliminate("Roger Schlegel");
            testVote.Display();

            Console.WriteLine("\r\nIs vote exhausted? ...");
            if (testVote.IsExhausted() == true)
            {
                Console.WriteLine("Vote is exhausted! Remove this vote.");
            }
            else if (testVote.IsExhausted() == false)
            {
                Console.WriteLine("Vote is still good!");
            }

            Console.WriteLine("\r\nTesting CanidateVote Class:");
            CandidateVotes voteCount1 = new CandidateVotes("Kevin", 5);
            voteCount1.Display();
            voteCount1.Count++;
            voteCount1.Display();
        }

        public List<string> CsvParser(string line)
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

            return nameList;
        }
    }
}
