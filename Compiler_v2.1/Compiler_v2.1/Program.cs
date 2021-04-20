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
            StreamReader sr = new StreamReader("input.txt");
            Lexer lexer = new Lexer();
         
            string AllTextProgram = sr.ReadToEnd(); ;
            lexer.Analysis(AllTextProgram);
         
            Console.ReadKey();
        }
    }
}
