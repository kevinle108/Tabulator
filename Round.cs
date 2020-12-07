using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tabulator
{
    class Round
    {
        public List<CandidateVotes> Tally = new List<CandidateVotes>();

        public Round()
        {
            
        }

        public void Display()
        {
            foreach (CandidateVotes nameVotes in Tally)
            {
                nameVotes.Display();
            }
        }

        public bool HasWinner()
        {
            if (Tally.Count > 0)
            {
                int total = Tally.Sum(x => x.Count);
                return (Tally[0].Count > total / 2) ? true : false;
            }
            return false;
        }

        public List<string> NamesToEliminate(List<Round> pastRounds)
        {
            List<string> namesToElim = new List<string>();
            int leastCount = Tally.Min(x => x.Count);
            List<string> leastNames = Tally.Where(x => x.Count == leastCount).Select(x => x.Name).ToList();
            if (leastNames.Count == 1)
            {
                namesToElim.AddRange(leastNames);
            }
            else // more than 1 name with least amount of votes
            {
                if (pastRounds.Count != 0)
                {
                    Console.WriteLine("Looking at previous rounds to break the tie...");
                    List<string> elimNamesRemaining = leastNames;
                    int elimCount = leastCount;
                    for (int i = pastRounds.Count - 1; i >= 0; i--) // loop thru previous rounds
                    {
                        List<CandidateVotes> filteredPrevRound = pastRounds[i].Tally.Where(x => elimNamesRemaining.Any(y => y == x.Name)).ToList();
                        elimNamesRemaining = filteredPrevRound.Where(x => x.Count == filteredPrevRound.Min(x => x.Count)).Select(x => x.Name).ToList();
                        if (elimNamesRemaining.Count == 1) // tie can be broken with previous round
                        {
                            namesToElim = elimNamesRemaining;
                            break;
                        }
                    }
                    if (namesToElim.Count == 0) // if tie-breaker was not found using previous rounds, then ask the user
                    {
                        Console.WriteLine($"There was a tie for elimination that couldn't be broken with previous rounds. The following candidates are tied:");
                        foreach (string name in leastNames)
                        {
                            Console.WriteLine(name);
                        }
                        AskWhoToElim(namesToElim, leastNames);
                    }
                }
                else // there are no past rounds to break the tie;
                {
                    AskWhoToElim(namesToElim, leastNames);
                }
            }
            return namesToElim;
        }

        private static void AskWhoToElim(List<string> namesToElim, List<string> leastNames)
        {
            bool ask = true;
            string input;
            while (ask)
            {
                Console.Write("\n\rPlease enter a candidate to eliminate: ");
                if (leastNames.Contains(input = Console.ReadLine()))
                {
                    ask = false;
                    namesToElim.Add(input);
                }
            }
        }
    }
}
