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
            //Console.WriteLine("Testing input file:");
            //StreamReader file = new StreamReader("Mayor.csv");
            //string line = file.ReadLine();
            //file.Close();
            //Console.WriteLine(line);
            //Vote testVote = new Vote(line);
            //testVote.Display();

            //Console.WriteLine($"\r\nFirst Choice: {testVote.FirstChoice()}");
            //Console.WriteLine("\r\nEliminate David Michael:");
            //testVote = testVote.Eliminate("David Michael");
            //testVote.Display();

            //Console.WriteLine("\r\nIs vote exhausted? ...");
            //if (testVote.IsExhausted() == true)
            //{
            //    Console.WriteLine("Vote is exhausted! Remove this vote.");
            //}
            //else if (testVote.IsExhausted() == false)
            //{
            //    Console.WriteLine("Vote is still good!");
            //}

            //Console.WriteLine("\r\nEliminate Roger Schlegel:");
            //testVote = testVote.Eliminate("Roger Schlegel");
            //testVote.Display();

            //Console.WriteLine("\r\nIs vote exhausted? ...");
            //if (testVote.IsExhausted() == true)
            //{
            //    Console.WriteLine("Vote is exhausted! Remove this vote.");
            //}
            //else if (testVote.IsExhausted() == false)
            //{
            //    Console.WriteLine("Vote is still good!");
            //}

            //Console.WriteLine("\r\nTesting CanidateVote Class:");
            //CandidateVotes voteCount1 = new CandidateVotes("Kevin", 5);
            //voteCount1.Display();
            //voteCount1.Count++;
            //voteCount1.Display();

            //Console.WriteLine("\r\n...Testing VoteList Class:");
            //VoteList votes = new VoteList("Mayor.csv");
            //votes.Display();
            //Console.WriteLine("\r\n...Testing Eliminate Roger Schlegel:");

            //votes.Eliminate("Roger Schlegel");
            //votes.Display();

            //Round round = votes.Count();
            //Console.WriteLine($"Displaying vote counts...");
            //round.Display();

            //Console.WriteLine($"Displaying winner...");
            //Console.WriteLine(round.HasWinner() ? "yes winner!" : "no winner...");


            //List<Round> pastRounds = new List<Round>();
            //var toElim = round.NamesToEliminate(pastRounds);
            //Console.WriteLine($"Names to eliminate: {toElim.Count}");
            //foreach (string name in toElim)
            //{
            //    Console.WriteLine(name);
            //}

            //Console.WriteLine("Test elimination rounds");
            //Round testRound = new Round();
            //testRound.Tally.Add(new CandidateVotes("A", 5000));
            //testRound.Tally.Add(new CandidateVotes("B1", 4000));
            //testRound.Tally.Add(new CandidateVotes("B2", 4000));
            //testRound.Tally.Add(new CandidateVotes("C", 2500));
            //testRound.Tally.Add(new CandidateVotes("D", 1000));
            //testRound.Tally.Add(new CandidateVotes("E", 400));
            //testRound.Tally.Add(new CandidateVotes("F", 100));

            //testRound.Display();

            //Console.WriteLine("\n\rNames to eliminate:");
            //List<Round> pastRounds = new List<Round>();
            //List<string> toElim = testRound.NamesToEliminate(pastRounds);
            //foreach (string name in toElim)
            //{
            //    Console.WriteLine(name);
            //}



            VoteList voteList;
            Round curRound;
            List<Round> pastRounds = new List<Round>();
            bool done = false;
            voteList = new VoteList("Mayor.csv");
            while (!done)
            {
                curRound = voteList.Count();
                Console.WriteLine();
                curRound.Display();
                if (curRound.HasWinner())
                {
                    Console.WriteLine($"{curRound.Tally[0].Name} is the winner");
                    done = true;
                }
                else
                {
                    List<string> toElim = curRound.NamesToEliminate(pastRounds);
                    string elimNamesMessage = "After this round, " + toElim.Aggregate((i, j) => i + " & " + j) + " will be eliminated";
                    Console.WriteLine(elimNamesMessage);
                    pastRounds.Add(curRound);
                    foreach (string name in toElim)
                    {
                        voteList.Eliminate(name);
                    }
                }
            }


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
