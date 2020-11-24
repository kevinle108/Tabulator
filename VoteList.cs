using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Tabulator
{
    class VoteList
    {
        List<Vote> Votes;

        public VoteList(string fileName)
        {
            List<Vote> list = new List<Vote>();
            StreamReader file = new StreamReader(fileName);
            string line = "";
            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                count++;
                list.Add(new Vote(line));
            }
            Console.WriteLine($"{count} lines read!");
            Votes = list;
        }

        public void Display()
        {
            Console.WriteLine($"Displaying List of {Votes.Count} votes");

            for (int i = 0; i < Votes.Count; i++)
            {
                Console.WriteLine($"Votes[{i}]");
                Votes[i].Display();
            }
        }

        public void Eliminate(string nameToEliminate)
        {
            foreach (Vote vote in Votes)
            {
                vote.Eliminate(nameToEliminate);
            }
            // clean up Empty votes
            Votes.RemoveAll(x => x.Names.Count == 0);
        }

        public List<CandidateVotes> Count()
        {
            List<CandidateVotes> tallies = new List<CandidateVotes>();
            foreach (Vote vote in Votes)
            {
                if (vote.FirstChoice() != "")
                {
                    
                    string firstChoiceName = vote.FirstChoice();
                    if (tallies.Count == 0)
                    {
                        tallies.Add(new CandidateVotes(firstChoiceName, 1));
                    }
                    else
                    {
                        int nameIndex = tallies.FindIndex(x => x.Name == firstChoiceName);
                        if (nameIndex == -1)
                        {
                            tallies.Add(new CandidateVotes(firstChoiceName, 1));
                        }
                        else
                        {
                            tallies[nameIndex].Count++;
                        }
                    }
                    
                }
            }
            tallies = tallies.OrderByDescending(x => x.Count).ToList();
            return tallies;
        }
        
    }
}
