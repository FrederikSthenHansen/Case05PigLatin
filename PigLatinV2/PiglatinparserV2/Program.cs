using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiglatinparserV2
{
    class Program
    {
        static void Main(string[] args)
        {

            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            object miss = System.Reflection.Missing.Value;
            object path = @"YourFilepath\file.docx";
            object path2 = @"C:\Users\SA02- Frederik\Documents\Case05PigLatin\testInputFiles\InputText.txt";
            object readOnly = true;
            Microsoft.Office.Interop.Word.Document docs = word.Documents.Open(ref path2, ref miss, ref readOnly, ref miss, ref miss,
                        ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss, ref miss);
            string totaltext = "";      //the whole document
            for (int i = 0; i < docs.Paragraphs.Count; i++)
            {
                //Determine the beginning of an entire paragraph and intercept the table name
                //Get the column name
                //......
                totaltext += docs.Paragraphs[i + 1].Range.Text.ToString();
            }
            Console.Write(totaltext);
            docs.Close();
            word.Quit();


            Console.WriteLine("press any key to close program");
            Console.ReadKey();
        }
    }
}
