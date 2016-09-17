using MailerAndLoger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailerAndLoger
{
    class AllCombinations
    {
        static List<string> chars = new List<string>(new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" });

        static IEnumerable<string> Combinations(List<string> characters, int length)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                // only want 1 character, just return this one
                if (length == 1)
                    yield return characters[i];

                // want more than one character, return this one plus all combinations one shorter
                // only use characters after the current one for the rest of the combinations
                else
                    foreach (string next in Combinations(characters.GetRange(i + 1, characters.Count - (i + 1)), length - 1))
                        yield return characters[i] + next;
            }
        }

        static void Main(string[] args)
        {
            string method_caller = new StackTrace().GetFrame(1).GetMethod().DeclaringType.FullName.ToString();
            string fileNameLoc = Strings.ERROR_PATH_DISK + "combinations.txt";

            for (int i=1; i<=10; i++)
            {
                IEnumerable<string> res = Combinations(chars, i);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameLoc, true))
                {
                    foreach (string s in res)
                    {
                        file.WriteLine(s);
                    }
                }
            }
            
        }

    }
}
