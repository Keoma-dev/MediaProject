using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaWeb.Utility
{
    public class StringEdits
    {

        public static string FirstLettterToUpper(string s)
        {
            s.ToLower();
            string[] seperateWords = s.Split(" ");
            int index = 0;

            foreach (var word in seperateWords)
            {
               seperateWords[index]=char.ToUpper(word[0]) + word.Substring(1);
                index++;
            }

            return string.Join(" ", seperateWords);
        }
    }
}
