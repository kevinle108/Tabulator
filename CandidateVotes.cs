using System;
using System.Collections.Generic;
using System.Text;

namespace Tabulator
{
    class CandidateVotes
    {
        public string Name;
        public int Count;

        public CandidateVotes(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public void Display()
        {
            Console.WriteLine($"{Name}: {Count}");
        }
    }
}
