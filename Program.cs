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
        }
    }
}
