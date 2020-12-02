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

            int amtNeeded = Tally.Sum(x => x.Count);
            List<int> justCounts = Tally.Select(x => x.Count).Distinct().ToList();
            for (int i = 0; i < justCounts.Count; i++)
            {
                int numOfOccurences = Tally.Where(x => x.Count == justCounts[i]).ToList().Count;
                int sumOfLowerVotes = Tally.GetRange(i, Tally.Count - i).Sum(x => x.Count);
                if (!(justCounts[i] * numOfOccurences + sumOfLowerVotes > amtNeeded))
                {
                    namesToElim.AddRange(Tally.Where(x => x.Count == justCounts[i]).Select(x => x.Name).ToList());
                }
            }
            if (namesToElim.Count == 0) // if no names are added up to this point, take the minimum count
            {
                int minCount = Tally.Min(x => x.Count);
                int numOfOccurences = Tally.Where(x => x.Count == minCount).ToList().Count;
                if (numOfOccurences > 1)
                {
                    
                    List<CandidateVotes> tiedCurRound = Tally.Where(x => x.Count == minCount).ToList();
                    string foundTiedMessage = "Found a tie between: " + tiedCurRound.Select(x => x.Name).Aggregate((i, j) => i + " & " + j);
                    Console.WriteLine(foundTiedMessage);

                    if (pastRounds.Count > 0) // view pastround for Tie-Breaker
                    {
                        Console.WriteLine("Looking at previous round to break the tie...");
                        Round prevRound = pastRounds.Last();
                        List<CandidateVotes> filteredPastRound = prevRound.Tally.Where(x => tiedCurRound.Any(y => y.Name == x.Name)).OrderByDescending(x => x.Count).ToList();
                        string prevRoundElimName = filteredPastRound.Last().Name;
                        int prevRoundElimCount = filteredPastRound.Last().Count;
                        int prevRoundElimCountOccur = filteredPastRound.Where(x => x.Count == prevRoundElimCount).ToList().Count;
                        if (prevRoundElimCountOccur > 1) // prev round cannot break tie, ask user to select who to eliminate
                        {
                            List<CandidateVotes> prevTied = filteredPastRound.Where(x => x.Count == prevRoundElimCount).ToList();
                            Console.WriteLine("In the previous round, these candidates are tied:");
                            foreach (CandidateVotes candidate in prevTied)
                            {
                                candidate.Display();
                            }
                            bool ask = true;
                            string input;
                            List<string> prevTiedNames = prevTied.Select(x => x.Name).ToList();
                            while (ask)
                            {
                                Console.Write("\n\rChoose 1 candidate to eliminate: ");
                                input = Console.ReadLine();
                                if (prevTiedNames.Contains(input))
                                {
                                    ask = false;
                                    namesToElim.Add(input);
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine($"{prevRoundElimName} will be eliminated via previous round tie-breaker");
                            namesToElim.Add(prevRoundElimName);
                        }
                    }
                    else // there is no past rounds
                    {
                        Console.WriteLine("No previous round to break tie.");
                        bool ask = true;
                        string input;
                        List<string> tiedNames = tiedCurRound.Select(x => x.Name).ToList();
                        while (ask)
                        {
                            Console.Write("\n\rChoose 1 candidate to eliminate: ");
                            input = Console.ReadLine();
                            if (tiedNames.Contains(input))
                            {
                                ask = false;
                                namesToElim.Add(input);
                            }
                        }
                    }
                }
                else
                {
                    namesToElim.AddRange(Tally.Where(x => x.Count == minCount).Select(x => x.Name).ToList());
                }
            }
            return namesToElim;

        }
    }
}
