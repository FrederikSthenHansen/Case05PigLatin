using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PigLatinTextParser
{
    class TextFileHandler
    {
        TextParser myParser = new TextParser();
        
        
        
        private string[] RawTextArray;
        private string[] TreatedTextArray= new string[] {""};
        private string TreatedText;


        //Needs Refactoring to accept docx format
        public string[] ReadFiles(string path)
        {
            string[] files = Directory.GetFiles(path);
            string[] ret= new string[] {""};
            foreach (string myFile in files) {ret= System.IO.File.ReadAllLines(myFile); }

            return ret;
        }

        public void WritePigLatinFile(string filePath) 
        {
            RawTextArray = ReadFiles(filePath);

            //First we take in the string array (bunch of text lines) from readfiles
            foreach (var text in RawTextArray)
            {
                //Strings are broken down into individual words
                string[] words = myParser.BreakUpText(text);

                
                for (int w=0;w<words.Length; w++ )
                {   //All words in each line are turned to Pig latin.
                    words[w] = myParser.MakePigLatinWord(words[w]);
                    
                    //each line is reconstruced word by word
                    TreatedTextArray[w] = myParser.RebuildTextLine(words);
                }
                
                //Then all lines are added back together to reform the text.
                TreatedText = myParser.RebuildWholeText(TreatedTextArray);
                
            }
            //the finished text is printed to console
            Console.Write(TreatedText);

            //TODO: write finished text to new .TXT file in output folder.
        }
    }
}

//foreach (string file in files)
//{public string[] MyLines = System.IO.File.ReadAllLines(file); } 
    


            



     
      

       
    

