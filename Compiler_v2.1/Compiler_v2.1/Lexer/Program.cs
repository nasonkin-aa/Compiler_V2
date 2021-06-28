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
                var StRead = new BinaryReader(File.OpenRead(s));
                Lexer lexer = new Lexer(StRead);
                
                //Lex = lexer.GetLexem();

                while (StRead.PeekChar() != -1 && (lexer.Check1 || lexer.Check2)) 
                {
                    Lex = lexer.GetLexem();

                    LineResult = ResultFile.ReadLine();
                    //Console.WriteLine(LineResult);

                    if((lexer.Check1 || lexer.Check2))
                    {
                        result = Lex.Ln + ":"
                        + Lex.Ch + "\t" + Lex.States
                        + "\t" + "\"" + Lex.Buff + "\"" + "\t" + Lex.Value;
                        Console.WriteLine(result);

                        if (result == LineResult)
                        {
                            TestResult = "Тест пройден";
                        }
                        else
                        {
                            TestResult = "Тест не пройден";
                            //break;
                        }
                    }
                    //Console.WriteLine(TestResult);

                }
                
                Console.WriteLine(TestResult);
              
            }

            Console.ReadKey();
        }
    }
}
