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
        string Path = "";

        
        Program program = new Program();
        char[] sm = new char[1];
        string buf = "";

        private string[] IsReserveWords = { "program", "var", "integer", "real", "bool", "begin",
            "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] IsArOperator = { "*", "/", "div", "mod", "and", "or", "+", "-", "=", "<", 
            ">", "<>", "<=", ">=", "in", "not",":" ,":="};
        private char IsSemicolon = ';';

        string Value = "";
        public enum State { Start, Nunber, Variable, ChoiceLex, ArOperator, ReserveWords, Semicolon, Integer, Float, FIN }
        State state;
        StringReader sr;
        int Ln = 0;
        int Ch = 0;

        //public List<Lexema> NamLexema = new List<Lexema>();

        public Lexer(string Path)
        {
            this.Path = Path;
        }
         public Lexema AddLex(int Ln, int Ch, State Lexema, string Buff,string Value)
        {
            return new Lexema(Ln, Ch, Lexema, Buff, Value);
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
        private void AddValue()
        {
            Value = buf;
        }

        private void AddBuf(char symb)
        {
            buf += symb;
        }
        public Lexema GetLexem(string AllTextProgram)
        {
            
            sr = new StringReader(AllTextProgram);

            ClearBuf();
            while (sm[0] != null || buf != null )
            {
                switch (state)
                {
                    case State.Start:
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
                            state = State.ChoiceLex;
                            GetNext();
                        }

                        else if (Char.IsDigit(sm[0]))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.Nunber;
                            GetNext();
                        }
                        else if (sm[0] == IsSemicolon)
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.Semicolon;
                            GetNext();
                        }
                        
                        //else if (IsArOperator.Any(str => str == sm[0].ToString()))
                        else if (IsArOperator.Contains(sm[0].ToString()))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.ArOperator;
                            GetNext();
                        }

                  
                        break;

                    case State.ChoiceLex:
                        if (!IsReserveWords.Contains(buf)
                            && !IsArOperator.Contains(buf) 
                            && ( sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {
                            state = State.Variable;
                        }

                        else if (IsReserveWords.Contains(buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {
                            state = State.ReserveWords;
                        }

                        else if (IsArOperator.Contains(buf)
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' 
                            || sm[0] == '\r' || sm[0] == ';'))
                        {

                            state = State.ArOperator;
                        }

                        else 
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                    break;

                    case State.Nunber:
                        if (Int32.TryParse(buf, out int x) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                           
                            
                            state = State.Integer;
                        }
                        else if (sm[0] == '.')
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = State.Float;
                        }

                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        break;

                    case State.Semicolon:
                        AddValue();
                        ClearBuf();
                        state = State.Start;
                        return new Lexema(Ln, Ch, state, buf, Value);



                    case State.Float:
                        if (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r')
                        {
                            AddValue();
                            
                            ClearBuf();
                            state = State.Start ;
                            return new Lexema(Ln, Ch, state, buf, Value);

                        }
                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();

                        }
                        
                        break;

                    case State.Integer:
                        AddValue();
                        state = State.Start;
                        return new Lexema(Ln, Ch, state, buf, Value);

                    case State.Variable:
                        AddValue();
                        state = State.Start;
                        return new Lexema(Ln, Ch, state, buf, Value);

                        

                    case State.ReserveWords:
                        AddValue();
                        state = State.Start;                      
                        return new Lexema( Ln, Ch, state, buf, Value);

                        

                    case State.ArOperator:
                        if (IsArOperator.Contains(buf)
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            AddValue();
                          
                            state = State.Start;
                            return new Lexema(Ln, Ch, state, buf, Value);

                        }
                        //else if(sm[0] != ' ' || sm[0] != '\n' || sm[0] != '\t' || sm[0] != '\0' || sm[0] != '\r')
                        //{
                        //    AddBuf(sm[0]);
                        //    GetNext();
                        //}
                        //дописать
                        break;
                }
            }
            return new Lexema(0, 0, State.FIN, "", "");



            Console.WriteLine("Файл-" + (FileCounter));

            //Console.WriteLine(buf);
            //for (int i = 0; i < NamLexema.Count; i++)
            //{
            //    result = Convert.ToString(NamLexema[i].Ln) + ":"
            //        + Convert.ToString(NamLexema[i].Ch) + "\t" + NamLexema[i].States
            //        + "\t" + "'" + NamLexema[i].Buff + "'" + "\t" + NamLexema[i].Value + ".";
            //    Console.WriteLine(result);


            //    string LineResult = ResultFile.ReadLine();
            //    //Console.WriteLine(LineResult);

            //    if (result == LineResult)
            //    {
            //        TestResult = "Тест пройден";
            //    }
            //    else
            //    {
            //        TestResult = "Тест не пройден";
            //       // break;
            //    }

            //}

            //Console.WriteLine(TestResult);

        }

    }
}