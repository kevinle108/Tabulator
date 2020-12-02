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
            Votes.RemoveAll(x => x.Names[0].Contains(Vote.NAME_SEPARATOR) == true);
        }

        public Round Count()
        {
            Round round = new Round();
            foreach (Vote vote in Votes)
            {
                if (vote.FirstChoice() != "")
                {

                    string firstChoiceName = vote.FirstChoice();
                    if (round.Tally.Count == 0)
                    {
                        round.Tally.Add(new CandidateVotes(firstChoiceName, 1));
                    }
                    else
                    {
                        int nameIndex = round.Tally.FindIndex(x => x.Name == firstChoiceName);
                        if (nameIndex == -1)
                        {
                            round.Tally.Add(new CandidateVotes(firstChoiceName, 1));
                        }
                        else
                        {
                            round.Tally[nameIndex].Count++;
                        }
                    }
                    
                }
            }
            round.Tally = round.Tally.OrderByDescending(x => x.Count).ToList();
            return round;
        }
        
    }
}
