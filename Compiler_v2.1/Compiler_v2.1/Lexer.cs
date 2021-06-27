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
        public enum State { Start, Number, Variable, ChoiceLex, ArOperator, ReserveWords, Semicolon, Integer, Float}
        State state;
        BinaryReader Reader;
        int Ln = 0;
        int Ch = 0;
        string LexName;
        //public List<Lexema> NamLexema = new List<Lexema>();

        public Lexer(BinaryReader reader)
        {
            Reader = reader;
        }
         public Lexema AddLex(int Ln, int Ch, string LexName, string Buff,string Value)
         {
            Ch = Ch - 1;
            return new Lexema(Ln, Ch, LexName, Buff, Value);
         }
        private void GetNext()
        {
            if (Reader.PeekChar() != -1)
            {
                sm[0] = Reader.ReadChar();
                Ch++;
            }
            else
            {
                buf = null;
                
            }
          
        }
        private void AddLexName()
        {
            LexName = state.ToString();
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
        public Lexema GetLexem()
        {

            ClearBuf();

            while ( buf != null )
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

                        else if (Char.IsLetter(sm[0]) || sm[0] == '_')
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
                            state = State.Number;
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

                    case State.Number:
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
                        AddLexName();

                        state = State.Start;
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                    case State.Float:
                        if (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r')
                        {
                            AddValue();
                            AddLexName();

                            state = State.Start ;
                            return new Lexema(Ln, Ch, LexName, buf, Value);

                        }
                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();

                        }
                        
                        break;

                    case State.Integer:
                        AddValue();
                        AddLexName();

                        state = State.Start;
                        AddLex(Ln, Ch, LexName, buf, Value);
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                    case State.Variable:
                        AddValue();
                        AddLexName();

                        state = State.Start;
                        AddLex(Ln, Ch, LexName, buf, Value);
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                        

                    case State.ReserveWords:
                        AddValue();
                        AddLexName();

                        state = State.Start;                      
                        return new Lexema( Ln, Ch, LexName, buf, Value);

                        

                    case State.ArOperator:
                        if (IsArOperator.Contains(buf)
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            AddValue();
                            AddLexName();

                            state = State.Start;
                            return new Lexema(Ln, Ch, LexName, buf, Value);

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
            Console.WriteLine("biba");
            return new Lexema(0, 0, LexName, "", "");

            AddLexName();



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