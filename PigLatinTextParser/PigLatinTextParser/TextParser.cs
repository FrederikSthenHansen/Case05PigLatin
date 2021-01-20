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

        private int getNestedArrayTotalLenght(string[][] input)
        { int ret=0;
            foreach (string[] array in input)
            {
                ret = ret + array.Length;
            }
            return ret;
        }

        public string[] BreakUpText(string inputtext) 
        {
            string[] arraySpace = new string[] { "" };
            string[] ret= new string[] { "" };


            if (inputtext != null)
            {   
                //Simple breakup of sentence by spaces
                arraySpace = inputtext.Split(' ');

                //Further breakup of sentence by CRLF
                string[][] arrayCRLF = new string[arraySpace.Length][];
                for (int x=0; x< arraySpace.Length;x++)
                {
                    //regex to check if the an uppercase letter is sorrounded by lowercase letters. this one is unicode compliant.
                    string divideByUppercase = @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})"; 

                    //further divide the split textfuther to avoid words clustering becasue CRLF were lost when reading the text 
                    //from the input files.
                    arrayCRLF[x] = Regex.Split(arraySpace[x], divideByUppercase, RegexOptions.Compiled);

                    //force line changes in ouput
                    //for (int y=0; y < arrayCRLF[x].Length; y++)
                    //{
                    //    arrayCRLF[x][y] = arrayCRLF[x][y] + " /r/n";
                    //}
                    
                }

                //Make the return-array big enough to house  all the content of the nested array.
                 ret = new string[getNestedArrayTotalLenght(arrayCRLF)];


                //input all values from the nested array into "ret"
                int arr1 = 0;
                int arr2 = 0;
                for (int r=0; r < ret.Length; r++)
                {
                    try 
                    {
                        ret[r] = arrayCRLF[arr2][arr1];
                    }
                    catch(IndexOutOfRangeException) 
                    {
                        //if arr1 is out of index we reset it to 0 (-1 because arr1 increases by 1 after this operation).
                        // also we increase r2 to move onto the next array
                        r--;
                        arr1=-1;
                        arr2++;
                    }

                    arr1++;
                }
            }
            
            
            return ret;
        }

        public string RebuildWholeText(string[] inputArray)
        {
            string ret="";
            foreach (string line in inputArray)
            {
                ret =string.Join("", inputArray);
                ret = ret + System.Environment.NewLine;
                return ret;
            }
            return ret;
        }
        public string RebuildTextLine(string inputWord)
        {
            string ret = "";

           
                ret = ret + inputWord+ " ";
            

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="letter">This must be a letter</param>
        /// <returns>Returns the requested letter in upper case.</returns>
        public char makeLetterCapital(char letter)
        {
            string val = letter.ToString();
            val = val.ToUpper();
            return val[0];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="letter">This must be a letter</param>
        /// <returns>Returns the requested letter in lower case.</returns>
        private char makeLetterLower(char letter)
        {
            string val = letter.ToString();
            val = val.ToLower();
            return val[0];
        }

        /// <summary>
        /// Removes the requested amount of characters from the requested start index, in the requested string
        /// </summary>
        /// <param name="word">The String you want to remove punctuation from</param>
        /// <param name="start">the index of the first char to be removed</param>
        /// <param name="amount">the total amount of chars to be removed</param>
        /// <returns>The requested string, with the requested characters removed.</returns>
        private string removeEndingPunctuation(string word, int start, int amount)
        {
            //is there punctuation to be removed? 
            if (amount != 0 && checkIndexIsLegal(start,word.Length)==true)
            {
                //start-1 why??
                word = word.Remove(start, amount);
            }
            return word;
        }

        /// <summary>
        /// Method to prevent indexing out of range. Just provide your index and lenght as they are.
        /// </summary>
        /// <param name="index"> The index you want to make sure is legal </param>
        /// <param name="lenght"> The lenght of the array you are trying to index in</param>
        /// <returns>returns a bool to indicate whether the index is inside your array or not.</returns>
        private bool checkIndexIsLegal(int index, int lenght)
        {
          bool ret= false;

          if(index>=0 && index < lenght)
          {
            ret = true;
          }

          return ret;
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

            int _lastletterIndex (char[] inputArray)
            { int ret=0;
                //reverse forloop to find last letter.
                for (int c= inputArray.Length-1; c >= 0; c--)
                {
                    if (_isAlphaBet(inputArray[c])==true) { ret = c; break; }
                }
                return ret;
            }
            char[] charWord = inputWord.ToCharArray();

            //store starting consonants in order and store last punctuation if word ends with that
            for (int c = 0; c < charWord.Length; c++)
            {
                //Is the the char a letter?
                if (_isAlphaBet(charWord[c]) == true)
                {
                    //Note the index of the first letter in the word 
                    //(To handle my implementation of capital/lowercase correctly, if the word starts with punctuation)
                    // looking at c-1 to make prevent every letter from being noted at the first. 
                    //  adde check to make sure c-1 is not out of range.
                    if(checkIndexIsLegal(c-1,charWord.Length)==true) 
                    {
                        if (_isAlphaBet(charWord[c - 1]) == false) 
                        { _firstLetterIndex = c; }
                    }
                    

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
                                //Check the word is not all caps (fx. USA, FIFA)
                                if (checkIndexIsLegal(_lastletterIndex(charWord), charWord.Length) == true)
                                {
                                    if (_isCapital(charWord[_lastletterIndex(charWord)]) == false)
                                    {
                                        //replace the consonant with a lowerCase version.
                                        charWord[c] = makeLetterLower(inputWord[c]);
                                        firstLetterIsCapital = true;
                                    } 
                                }
                                
                            }
                        }

                        //add the consonant to our consonants string for later use.
                        _consonants = _consonants + charWord[c];
                    }
                    else
                    {
                        //Wovel OR consonant at the end of the word

                        AllStartingConsonantsFound = true;

                        //set the index of the first Wovel.
                        //(to be used in case we need the first Wovel to be a Capital letter).

                        if (_firstWovelIndex == 999)
                        { _firstWovelIndex = c; }

                    }
                }

                //If the word contains no more letters we need to store punctuation, and remove from the word.
                //This is done so we can affix any consonants at the end of the word, before adding punctuation again.
                else
                {
                    //is the char at the beginnin of the word?
                    if (c > _lastletterIndex(charWord))
                    {
                        _punctuation = _punctuation + $"{charWord[c]}";
                    }
                }
            }


            //Does the word start with a consonant?
            if (_consonants != "")
            {
                if (firstLetterIsCapital == true)
                {
                    //if _firstWovelindex is not out of range, the word starts with a capitol letter
                    if (_firstWovelIndex != 999)
                    {
                        //therefore the first Wovel needs to be turned capitol.
                        charWord[_firstWovelIndex] = makeLetterCapital(inputWord[_firstWovelIndex]);
                    }

                }

                //with all character alterations complete we have no more use for charWord
                //and as such we turn it back into to our string, overwriting the old content in the process.
                inputWord = new string(charWord);
                // remove punctuation, perform consonant pig-latin and re-add the punctuation
                
                inputWord = removeEndingPunctuation(inputWord, _lastletterIndex(charWord) + 1, _punctuation.Length);
                inputWord = inputWord.Remove(_firstConsonantIndex, _consonants.Length);
                inputWord = inputWord + _consonants + "ay" + _punctuation;
            }

            //Word has no starting consonants
            else            {                


                //if there is no characters in the inputputword, we cannot run the code in else block,
                //since that would index out of range
                if (inputWord!="")
                {
                        //Check if the Word starts with a letter (if not it is not a word, and should not be modified)
                        if (_isAlphaBet(inputWord[_firstLetterIndex]) == true)
                        {
                            inputWord = removeEndingPunctuation(inputWord, _lastletterIndex(charWord)+1, _punctuation.Length);
                            inputWord = inputWord + "yay" + _punctuation;
                            //remove punctuation, perform simple vowel pig-latin and re-add the punctuation

                        } 
                }
                
            }

            return inputWord;
        }
    } 
}
