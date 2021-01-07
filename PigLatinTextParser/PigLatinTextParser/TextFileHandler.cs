using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PigLatinTextParser
{
    class TextFileHandler
    {
        string[] files = Directory.GetFiles(@"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\..");
        public string[] RawText;
        public void ReadFiles()
        {
            string[] ret= new string[] {""};
            foreach (string myFile in files) {ret= System.IO.File.ReadAllLines(myFile); }
            RawText = ret;
            //return ret;
        }
           
    }
}

//foreach (string file in files)
//{public string[] MyLines = System.IO.File.ReadAllLines(file); } 
    


            



     
      

       
    

