using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PigLatinTextParser
{
    class TextParser
    {


        private bool _isAlphaBet(char input)
        { 
            return Regex.IsMatch(input.ToString(), "[a-zæøå]", RegexOptions.IgnoreCase);
        }
        private bool _isConsonant(char input)
        {
            return "bcdfghjklmnprstvxz".IndexOf(input.ToString(), StringComparison.InvariantCultureIgnoreCase) >= 0; ;
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
            string _punctuation = "";
            int _firstConsonantIndex=0;

            //store starting consonants in order and store last punctuation if word ends with that
            for (int c=0;c<inputWord.Length;c++)
            {
                //Is the the fist char a letter?
                if (_isAlphaBet(inputWord[c]) == true)
                {
                    if (_isConsonant(inputWord[c]) == true & AllStartingConsonantsFound == false)
                    {
                        if (_consonants == "")
                        {
                            _firstConsonantIndex = c;
                        }
                        _consonants = _consonants + inputWord[c];
                    }
                    else
                    {
                        AllStartingConsonantsFound = true;
                        break;
                    }
                }
            }

            //If the last char is punctuation store it, and remove from the word.
            if (_isAlphaBet(inputWord[inputWord.Length - 1]) == false)
            {
                _punctuation = $"{inputWord[inputWord.Length - 1]}";
                inputWord = inputWord.Remove(inputWord.Length - 1);   
            }

            //Does the word start with a consonant?
            if (_consonants!= "")
            {
                //consonant pig-latin and re-add the punctuation
                inputWord = inputWord.Remove(_firstConsonantIndex, _consonants.Length);
                inputWord = inputWord + _consonants + "ay" + _punctuation;
            }
            else
            {
                //simple vowel pig-latin and re-add the punctuation
                inputWord = inputWord + "yay" + _punctuation;
            }
            return inputWord;
        }
    }
}
