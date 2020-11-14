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
            string lineOfText = "1,00000400,Kate Stewart]Miley Houston]David Michael,Miley Houston]David Michael,Roger Schlegel,";

            Vote test = new Vote(lineOfText);
            test.Display();

            Console.WriteLine($"1st Choice: {test.FirstChoice()}");

            Console.WriteLine();
            Console.WriteLine("Testing input file:");
            Console.WriteLine();
            StreamReader file = new StreamReader("Mayor.csv");
            string line = file.ReadLine();
            Console.WriteLine(line);
            Vote testVote = new Vote(line);
            testVote.Display();

            Console.WriteLine();
            Console.WriteLine("Eliminate David Michael:");
            testVote = testVote.Eliminate("David Michael");
            testVote.Display();

            Console.WriteLine();
            Console.WriteLine("Is vote exhausted? ...");
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
