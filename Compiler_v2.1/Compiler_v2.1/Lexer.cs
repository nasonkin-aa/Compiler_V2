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
        string DirNameResult = @"D:\Git\Compiler_V2\Compiler_v2.1\Compiler_v2.1\Tests";


        char[] sm = new char[1];
        string buf = "";
        public string result = "";
        private string[] IsReserveWords = { "program", "var", "integer", "real", "bool", "begin", "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] IsArOperator = { "*", "/", "div", "mod", "and", "or", "+", "-", "=", "<", ">", "<>", "<=", ">=", "in", "not" };
        private char IsSemicolon = ';';
        string Output = "";
        public enum States { Start, Nunber, Variable, ChoiceLex, ArOperator, ReserveWords, Semicolon, FIN }
        States state;
        StringReader sr;
        int Ln = 0;
        int Ch = 0;

        public List<Lexema> NamLexema = new List<Lexema>();

        void AddLex(List<Lexema> lexemas, int Ln, int Ch, States States, string Buff,string Output)
        {
            lexemas.Add(new Lexema(Ln, Ch, States, Buff, Output));
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
        public void Analysis(string AllTextProgram)
        {
            sr = new StringReader(AllTextProgram);
            IEnumerable<string> Code = Directory.EnumerateFiles(DirNameResult, "*result*");

            while (state != States.FIN)
            {
                switch (state)
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
                            state = States.ChoiceLex;
                            GetNext();
                        }

                        else if (Char.IsDigit(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = States.Nunber;
                            GetNext();
                        }
                        else if (sm[0] == IsSemicolon)
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = States.Semicolon;
                            GetNext();
                        }

                        else if (IsArOperator.Any(str => str == sm[0].ToString()))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = States.ArOperator;
                            GetNext();
                        }

                        else if (sm[0] == '.')
                        {
                            AddBuf(sm[0]);
                            state = States.FIN;
                        }
                        
                        
                        break;

                    case States.ChoiceLex:
                        if (!IsReserveWords.Any(str => str == buf)
                            && !IsArOperator.Any(str => str == buf) 
                            && ( sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r' || sm[0] == ';'))
                        {
                            state = States.Variable;
                        }

                        else if (IsReserveWords.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r' || sm[0] == ';'))
                        {
                            state = States.ReserveWords;
                        }

                        else if (IsArOperator.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r' || sm[0] == ';'))
                        {

                            state = States.ArOperator;
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
                            Output = buf;
                            AddLex(NamLexema,Ln, Ch, state, buf,Output);
                            state = States.Start;
                            ClearBuf();
                        }

                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        break;

                    case States.Semicolon:
                        AddLex(NamLexema, Ln, Ch, state, buf, Output);
                        ClearBuf();
                        state = States.Start;
                        break;

                    case States.Variable:
                        AddLex(NamLexema, Ln, Ch, state, buf, Output);
                        ClearBuf();                      
                        state = States.Start;
                        break;

                    case States.ReserveWords:
                        AddLex(NamLexema, Ln, Ch, state, buf, Output);
                        state = States.Start;                      
                        ClearBuf();
                        break;

                    case States.ArOperator:
                        if (IsArOperator.Any(str => str == buf) && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            AddLex(NamLexema, Ln, Ch, state, buf, Output);
                            ClearBuf();
                            state = States.Start;
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
            //Console.WriteLine(buf);
            for (int i = 0; i < NamLexema.Count; i++)
            {
               // result = "a";
                result = "\t" + Convert.ToString(NamLexema[i].Ln) + ":"
                    + Convert.ToString(NamLexema[i].Ch) + "\t" + NamLexema[i].States
                    + "\t" + "'" + NamLexema[i].Buff + "'"+".";
                Console.WriteLine(result);


                //StreamReader f = new StreamReader("test.txt");
                foreach (string file in Code)
                {
                    StreamReader sr = new StreamReader(file);
                    string FileResult = sr.ReadLine();
                    Console.WriteLine(FileResult);
                    if (result == FileResult)
                    {
                        Console.WriteLine("ok");
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }

                }


            }
        }
        

    }
}
