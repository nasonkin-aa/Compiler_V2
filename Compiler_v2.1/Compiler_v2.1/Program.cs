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
            string DirNameCode = @"D:\Git\Compiler_V2\Compiler_v2.1\Compiler_v2.1\Tests";

            IEnumerable<string> CodeFile = Directory.EnumerateFiles(DirNameCode, "*code*");
            foreach (string s in CodeFile)
            {
                Lexer.FileCounter++;
                Lexer lexer = new Lexer(s);
                StreamReader sr = new StreamReader(s);
                string AllTextProgram = sr.ReadToEnd(); ;
                lexer.GetLexem(AllTextProgram);
                
            }

            Console.ReadKey();
        }
    }
}
