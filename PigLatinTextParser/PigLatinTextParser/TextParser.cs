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

        public string[] BreakUpText (string inputtext)
        {
            string[] array= inputtext.Split(' ');
            return array;
        }

        //may need to be multithreadded
        public string MakePigLatinWord(string inputWord)
        {
            bool AllStartingConsonantsFound = false;
            string _consonants = "";

            //store starting consonants in order
            foreach (char c in inputWord)
            {
                
                if (_isConsonant(c) == true & AllStartingConsonantsFound == false)
                {
                    _consonants = _consonants + c;
                }
                else
                {
                    AllStartingConsonantsFound = true;
                    break;
                }

            }

            //Does the word start with a consonant?
            if (_consonants!= "")
            {
                //consonant pig-latin
                inputWord = inputWord.Remove(0, _consonants.Length);
                inputWord = inputWord + _consonants + "ay";
            }
            else
            {
                //simple vowel pig-latin
                inputWord = inputWord + "yay";
            }
            
            return inputWord;
        }
    }
}
