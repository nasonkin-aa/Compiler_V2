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

            IEnumerable<string> Code = Directory.EnumerateFiles(DirNameCode, "*code*");
            foreach (string s in Code)
            {
                Lexer lexer = new Lexer();
                StreamReader sr = new StreamReader(s);
                string AllTextProgram = sr.ReadToEnd(); ;
                lexer.Analysis(AllTextProgram);
                
            }

            Console.ReadKey();
        }
    }
}
