using System;
using System.Collections.Generic;
using System.Text;

namespace PigLatinTextParser
{
    class TextFileHandler
    {
        private const string Path = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\PigLatinTextParser\PigLatinTextParser\InputText\inputTexT.txt";
        public string[] MyLines = System.IO.File.ReadAllLines(Path);
    }
}
