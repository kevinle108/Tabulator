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
            VoteList voteList;
            Round curRound;
            List<Round> pastRounds = new List<Round>();
            bool done = false;
            voteList = new VoteList("Mayor.csv");
            while (!done)
            {
                curRound = voteList.Count();
                Console.WriteLine($"\n\rRound {pastRounds.Count+1}");

                curRound.Display();
                if (curRound.HasWinner())
                {
                    Console.WriteLine($"{curRound.Tally[0].Name} is the winner");
                    done = true;
                }
                else
                {
                    List<string> toElim = curRound.NamesToEliminate(pastRounds);
                    if (curRound.Tally.Count == 2 && toElim.Count == 1) // broke a tie out of 2 total candidates, we can declare a winner here
                    {
                        string losingCandidate = toElim[0];
                        string winningCandidate = curRound.Tally.Where(x => x.Name != toElim[0]).Select(x => x.Name).ToList()[0];
                        Console.WriteLine($"\n\r{winningCandidate} is the winner!");
                        return;
                    }
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
    }
}
