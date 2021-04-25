using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compiler_v2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            string dirName = @"C:\Users\andr\source\repos\files\files\Test";

            IEnumerable<string> files = Directory.EnumerateFiles(dirName, "*test*");
            foreach (string s in files)
            {
                StreamReader sr = new StreamReader(s);
                Lexer lexer = new Lexer();
                string AllTextProgram = sr.ReadToEnd(); ;
                lexer.Analysis(AllTextProgram);
            }

            Console.ReadKey();
        }
    }
}
