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
        public enum State { Start, Number, Variable, ChoiceLex, ArOperator, ReserveWords, Semicolon, Integer, Float, Error}
        State state;
        BinaryReader Reader;
        int Ln = 0;
        int Ch = 0;
        string LexName;
        //public List<Lexema> NamLexema = new List<Lexema>();

        public bool Check1 = true;
        public bool Check2 = true;
        
        public Lexer(BinaryReader reader)
        {
            Reader = reader;
        }
         public Lexema AddLex(int Ln, int Ch, string LexName, string Buff,string Value)
         {
            Check2 = false;
            //Ch = Ch - 1;
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
                sm[0] = ' ';
                Check1 = false;
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

            while (  Check2 || Check1   )
            {
                Check2 = true;
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
                        else
                        {
                            state = State.Error;
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

                        else if (Char.IsLetter(sm[0]) || Char.IsDigit(sm[0]))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Error;
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
                        Check2 = false;
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                    case State.Float:
                        if (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r')
                        {
                            AddValue();
                            AddLexName();

                            state = State.Start ;
                            Check2 = false;
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
                        Check2 = false;                     
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                    case State.Variable:
                        AddValue();
                        AddLexName();

                        state = State.Start;
                        Check2 = false;

                        return new Lexema(Ln, Ch, LexName, buf, Value);


                    case State.ReserveWords:
                        AddValue();
                        AddLexName();

                        state = State.Start;
                        Check2 = false;

                        return new Lexema( Ln, Ch, LexName, buf, Value);

                        

                    case State.ArOperator:
                        if (IsArOperator.Contains(buf)
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            AddValue();
                            AddLexName();

                            state = State.Start;
                            Check2 = false;

                            return new Lexema(Ln, Ch, LexName, buf, Value);

                        }
                        //else if(sm[0] != ' ' || sm[0] != '\n' || sm[0] != '\t' || sm[0] != '\0' || sm[0] != '\r')
                        //{
                        //    AddBuf(sm[0]);
                        //    GetNext();
                        //}
                        //дописать
                        break;

                    case State.Error:
                        AddValue();
                        AddLexName();

                        state = State.Start;
                        Check1 = false;
                        Check2 = false;

                        return new Lexema(Ln, Ch, LexName, buf, Value);

                }
                
            }
            
            return new Lexema(0, 0, LexName, "", "");

            Console.WriteLine("Файл-" + (FileCounter));

        }

    }
}