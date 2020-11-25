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
    }
}
