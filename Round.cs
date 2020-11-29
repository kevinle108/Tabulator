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
            List<string> names = new List<string>();
            if (Tally.Select(x => x.Count).Distinct().ToList().Count == 1) // there is a tie
            {
                int tiedCount = Tally[0].Count;
                List<string> namesTied = Tally.Where(x => x.Count == tiedCount).Select(x => x.Name).ToList();
                Console.WriteLine($"\n\rThere are {namesTied.Count} candidates tied with {tiedCount} votes each:");
                foreach (string name in namesTied)
                {
                    Console.WriteLine(name);
                }
                bool askName = true;
                while (askName)
                {
                    Console.Write("\n\rEnter the name of the candidate to eliminate: ");
                    string inputName = Console.ReadLine();
                    if (namesTied.Contains(inputName))
                    {
                        askName = false;
                        names.Add(inputName);
                        return names;
                    }
                }
            }
            
            int savedIndex = 0;
            int needed = Tally.Sum(x => x.Count) / 2;
            for (int i = Tally.Count-1; i > 0; i--)
            {
                if (Tally.GetRange(i, Tally.Count - i).Sum(x => x.Count) < needed)
                {
                    savedIndex = i;
                }
            }
            if (savedIndex == 0) // if tied, use pastrounds for tiebreaker
            {
                if (pastRounds.Count > 0)
                {
                    string name1 = Tally[savedIndex].Name;
                    Round lastRound = pastRounds[pastRounds.Count - 1];
                    int name1Count = lastRound.Tally[savedIndex].Count;
                    int name2Count = lastRound.Tally[savedIndex + 1].Count;
                    if (name1Count > name2Count)
                    {
                        names.Add(Tally[savedIndex + 1].Name);
                    }
                    else
                    {
                        names.Add(Tally[savedIndex].Name);
                    }
                    return names;
                }
            }
            if (Tally[savedIndex].Count == Tally[savedIndex-1].Count)
            {
                
                savedIndex += 1;
            }
            return Tally.GetRange(savedIndex, Tally.Count - savedIndex).Select(x => x.Name).ToList();
        }
    }
}
