using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tabulator
{
    class Vote
    {
        // a vote is a List<List<String>>
        List<List<String>> Item = new List<List<String>>();
        readonly char NAME_SEPARATOR = ']';
        public Vote(string line)
        {
            List<List<string>> vote = new List<List<string>>(); // create an empty nested list

            // uncomment to allow names to contain commas and double quotes
            Regex regx = new Regex(',' + "(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            var postSplit = regx.Split(line).Skip(2);
            postSplit = postSplit.Select(x => x.Trim('"'));

            // uncomment to not allow commas and double quotes in names
            //var postSplit = lineOfText.Split(',').Skip(2); // ignores first 2 elements

            // if the lineOfText ends with a comma, then last element will be an empty string ""
            // remove this last empty string from the list
            if (postSplit.Last() == "")
            {
                postSplit = postSplit.SkipLast(1).ToList();
            }
            

            foreach (string ele in postSplit)
            {
                if (ele.Contains(NAME_SEPARATOR))
                {
                    vote.Add(ele.Split(']').ToList());
                }
                else
                {
                    vote.Add(new List<string> { ele });
                }
            }

            Item = vote;
        }

        public void Display()
        {
            for (int i = 0; i < Item.Count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Vote.Item[{i}]");
                for (int j = 0; j < Item[i].Count; j++)
                {
                    Console.WriteLine(Item[i][j]);
                }
            }
        }

        public string FirstChoice()
        {
            // use Aggregate() in case there is more than 1 candidate for 1st Choice
            return Item[0].Aggregate((message, name) => $"{message} & {name}");
        }

        public Vote Eliminate(string name)
        {
            for (int i = 0; i < Item.Count; i++)
            {
                Item[i].RemoveAll(x => x == name);
            }

            // clean up choices that have null candidates
            Item.RemoveAll(x => x.Count == 0);

            return this;
        }

        public bool IsExhausted()
        {
            return Item[0].Count > 1;
        }

        

    }
}