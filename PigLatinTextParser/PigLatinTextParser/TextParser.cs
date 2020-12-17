using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PigLatinTextParser
{
    class TextParser
    {

        

        private bool _isConsonant(char input)
        {
            bool ret= "bcdfghjklmnprstvxz".IndexOf(input.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0;
            return ret;
        }



        public string makePigLatin(string inputText)
        {
            foreach (char c in inputText)
            char myChar = 'c'; //få myChar udplukket af teksten.

            
            
            return "";
        }
    }
}
