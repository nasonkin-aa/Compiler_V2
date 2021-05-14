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
            string result = "";
            string TestResult = "";
            string LineResult = "";
            Lexema Lex;
            IEnumerable<string> CodeFile = Directory.EnumerateFiles(DirNameCode, "*code*");
            foreach (string s in CodeFile)
            {
                string PathResultFile = s.Remove(s.LastIndexOf('(')) + "(result).txt";
                var ResultFile = new StreamReader(PathResultFile);

                Lexer.FileCounter++;
                Lexer lexer = new Lexer(s);
                StreamReader StRead = new StreamReader(s);
                
                string AllTextProgram = StRead.ReadToEnd();

                Lex = lexer.GetLexem(AllTextProgram);
                while (Lex.Ch != 0 )
                {
                    Lex = lexer.GetLexem(AllTextProgram);
                    LineResult = ResultFile.ReadLine();
                    Console.WriteLine(LineResult);

                    result = Convert.ToString(Lex.Ln) + ":"
                    + Convert.ToString(Lex.Ch) + "\t" + Lex.States
                    + "\t" + "'" + Lex.Buff + "'" + "\t" + Lex.Value + ".";
                    Console.WriteLine(result);

                    if (result == LineResult)
                    {
                        TestResult = "Тест пройден";
                    }
                    else
                    {
                        TestResult = "Тест не пройден";
                        // break;
                    }
//Console.WriteLine(Lex);
                }
            }

            Console.ReadKey();
        }
    }
}
