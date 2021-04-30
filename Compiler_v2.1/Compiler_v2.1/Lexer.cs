using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class Lexer
    {
        public static int FileCounter;

        string TestResult = "";
        Program program = new Program();
        string Path = "";
        char[] sm = new char[1];
        string buf = "";
        public string result = "";

        private string[] IsReserveWords = { "program", "var", "integer", "real", "bool", "begin",
            "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] IsArOperator = { "*", "/", "div", "mod", "and", "or", "+", "-", "=", "<", 
            ">", "<>", "<=", ">=", "in", "not",":" ,":="};
        private char IsSemicolon = ';';

        string Value = "";
        public enum States { Start, Nunber, Variable, ChoiceLex, ArOperator, ReserveWords, Semicolon, FIN }
        States Lexema;
        StringReader sr;
        int Ln = 0;
        int Ch = 0;

        public List<Lexema> NamLexema = new List<Lexema>();

        public Lexer(string Path)
        {
            this.Path = Path;
        }
        void AddLex(int Ln, int Ch, States Lexema, string Buff,string Value)
        {
            this.NamLexema.Add(new Lexema(Ln, Ch, Lexema, Buff, Value));
        }
        private void GetNext()
        {
            sr.Read(sm, 0, 1);
            Ch++;
        }
        private void ClearBuf()
        {
            buf = "";
        }

        private void AddBuf(char symb)
        {
            buf += symb;
        }
        public void GetLexem(string AllTextProgram)
        {
            sr = new StringReader(AllTextProgram);
            string PathResultFile = Path.Remove(Path.LastIndexOf('(')) + "(result).txt"; 

            while (Lexema != States.FIN)
            {
                switch (Lexema)
                {
                    case States.Start:
                        if (sm[0] == ' ' || sm[0] == '\t'  || sm[0] == '\r')
                        {
                            GetNext();
                        }

                        else if(sm[0] == '\n' || sm[0] == '\0')
                        {

                            GetNext();
                            Ch = 0;
                            Ln++;
                        }

                        else if (Char.IsLetter(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            Lexema = States.ChoiceLex;
                            GetNext();
                        }

                        else if (Char.IsDigit(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            Lexema = States.Nunber;
                            GetNext();
                        }
                        else if (sm[0] == IsSemicolon)
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            Lexema = States.Semicolon;
                            GetNext();
                        }

                        else if (IsArOperator.Any(str => str == sm[0].ToString()))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            Lexema = States.ArOperator;
                            GetNext();
                        }

                        else if (sm[0] == '.')
                        {
                            AddBuf(sm[0]);
                            Lexema = States.FIN;
                        }
                        
                        
                        break;

                    case States.ChoiceLex:
                        if (!IsReserveWords.Any(str => str == buf)
                            && !IsArOperator.Any(str => str == buf) 
                            && ( sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {
                            Lexema = States.Variable;
                        }

                        else if (IsReserveWords.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {
                            Lexema = States.ReserveWords;
                        }

                        else if (IsArOperator.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {

                            Lexema = States.ArOperator;
                        }

                        else 
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                    break;

                    case States.Nunber:
                        if (Int32.TryParse(buf, out int x) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            Value = buf;
                            AddLex(Ln, Ch, Lexema, buf,Value);
                            Lexema = States.Start;
                            ClearBuf();
                        }

                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        break;

                    case States.Semicolon:
                        AddLex( Ln, Ch, Lexema, buf, Value);
                        ClearBuf();
                        Lexema = States.Start;
                        break;

                    case States.Variable:
                        AddLex(Ln, Ch, Lexema, buf, Value);
                        ClearBuf();                      
                        Lexema = States.Start;
                        break;

                    case States.ReserveWords:
                        AddLex(Ln, Ch, Lexema, buf, Value);
                        Lexema = States.Start;                      
                        ClearBuf();
                        break;

                    case States.ArOperator:
                        if (IsArOperator.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            AddLex( Ln, Ch, Lexema, buf, Value);
                            ClearBuf();
                            Lexema = States.Start;
                        }
                        else if(sm[0] != ' ' || sm[0] != '\n' || sm[0] != '\t' || sm[0] != '\0' || sm[0] != '\r')
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        
                        break;
                    case States.FIN:
                        break;
                }
            }
            Console.WriteLine("Файл-" + (FileCounter));

            var ResultFile = new StreamReader(PathResultFile);
            //Console.WriteLine(buf);
            for (int i = 0; i < NamLexema.Count; i++)
            {
                result = Convert.ToString(NamLexema[i].Ln) + ":"
                    + Convert.ToString(NamLexema[i].Ch) + "\t" + NamLexema[i].States
                    + "\t" + "'" + NamLexema[i].Buff + "'"+".";
                Console.WriteLine(result);

                string LineResult = ResultFile.ReadLine();
                //Console.WriteLine(LineResult);

                if (result == LineResult)
                {
                    TestResult = "Тест пройден";
                }
                else
                {
                    TestResult = "Тест не пройден";
                    break;
                }

            }
            Console.WriteLine(TestResult);

        }

    }
}