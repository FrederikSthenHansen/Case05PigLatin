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

        private bool _isCapital(char input)
        {
            return Regex.IsMatch(input.ToString(), "[A-ZÆØÅ]");
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

        public char makeLetterCapital(char letter)
        {
            string val = letter.ToString();
            val = val.ToUpper();
            return val[0];
        }
        public char makeLetterLower(char letter)
        {
            string val = letter.ToString();
            val = val.ToLower();
            return val[0];
        }

        //may need to be multithreadded
        public string MakePigLatinWord(string inputWord)
        {
            bool firstLetterIsCapital = false;
            bool AllStartingConsonantsFound = false;
            string _consonants = "";
            string _punctuation = "";
            int _firstConsonantIndex = 0;
            int _firstWovelIndex = 999; //meant to indexOutofRange
            int _firstLetterIndex = 0;
            int _lastletterIndex = 9999;
            char[] charWord = inputWord.ToCharArray();

            //store starting consonants in order and store last punctuation if word ends with that
            for (int c = 0; c < charWord.Length; c++)
            {
                //Is the the char a letter?
                if (_isAlphaBet(charWord[c]) == true)
                {
                    //Note the index of the first letter in the word 
                    //(To handle my implementation of capital/lowercase correctly, if the word starts with punctuation)
                    _firstLetterIndex = c;

                    //Is the first char a vowel or consonant?
                    if (_isConsonant(charWord[c]) == true & AllStartingConsonantsFound == false)
                    {
                        //Consonant

                        if (_consonants == "")
                        {
                            _firstConsonantIndex = c;

                            //if the first first consonant is the first letter in the word and a capital letter?
                            if (_isCapital(inputWord[c]) && c == _firstLetterIndex)
                            {
                                //replace the consonant with a lowerCase version.
                                charWord[c] = makeLetterLower(inputWord[c]);
                                firstLetterIsCapital = true;
                            }
                        }

                        //add the consonant to our consonants string for later use.
                        _consonants = _consonants + charWord[c];
                    }
                    else
                    {
                        //Wovel
                        AllStartingConsonantsFound = true;

                        //set the index of the first Wovel.
                        //(to be used in case we need the first Wovel to be a Capital letter).

                        if (_firstWovelIndex == 999)
                        { _firstWovelIndex = c; }
                        /// TODO: check at c+1 er en er punctuation og angiv sidse bogstavplacering hvis sandt.
                        /// 

                        try
                        {
                            if (_isAlphaBet(charWord[c + 1]) != true)
                            {
                                _lastletterIndex = c;
                            }
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            //do nothing, since this should only trigger if the word has no ending punctuation
                        }
                    }
                }

                //If the word contains no more letters we need to store punctuation, and remove from the word.
                //This is done so we can affix any consonants at the end of the word, before adding punctuation again.
                else 
                {
                    //is the char at the beginnin of the word?
                    if (c > _lastletterIndex)
                    {
                        _punctuation = _punctuation + $"{charWord[c]}";
                    }
                }
            }

        
                //Does the word start with a consonant?
                if (_consonants!= "")
            {
                if (firstLetterIsCapital == true)
                {
                    //if _firstWovelindex is not out of range, the word starts with a capitol letter
                    if(_firstWovelIndex!=999)
                    {
                        //therefore the first Wovel needs to be turned capitol.
                       charWord[_firstWovelIndex] = makeLetterCapital(inputWord[_firstWovelIndex]); 
                    }
                 
                }

                //with all character alterations complete we have no more use for charWord
                //and as such we turn it back into to our string, overwriting the old content in the process.
                inputWord = new string(charWord);
                // remove punctuation, perform consonant pig-latin and re-add the punctuation
                
                inputWord = inputWord.Remove(_firstConsonantIndex, _consonants.Length);
                inputWord = removeEndingPunctuation(inputWord,_lastletterIndex + 1, _punctuation.Length);
                inputWord = inputWord + _consonants + "ay" + _punctuation;
            }
            else
            {
                //remove punctuation, perform simple vowel pig-latin and re-add the punctuation
                inputWord = removeEndingPunctuation(inputWord, _lastletterIndex + 1, _punctuation.Length);
                inputWord = inputWord + "yay" + _punctuation;
            }

            
            return inputWord;  
        }
        private string removeEndingPunctuation(string word, int start, int amount)
        {
            //is there punctuation to be removed?
            if(amount!=0)
            {
                word = word.Remove(start-1, amount);
            }
            return word;
        }
    }
}
