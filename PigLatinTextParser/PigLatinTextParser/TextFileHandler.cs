using System;
using System.Collections.Generic;
using System.Text;

namespace PigLatinTextParser
{
    class TextFileHandler
    {
       public string[] MyLines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");
    }
}
