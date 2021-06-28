using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class Lexer
    {
        public static int FileCounter;

        Program program = new Program();
        char[] sm = new char[1];
        string buf = "";
        
        private string[] IsReserveWords = { "program", "var", "integer", "real", "bool", "begin",
            "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] IsArOperator = { "*", "/", "div", "mod", "and", "or", "+", "-", "=", "<", 
            ">", "<>", "<=", ">=", "in", "not"};
        private string[] IsSeparators = { ";", ".", ":",",","..","[","]"};
        private string[] IsAssigments = { ":=", "/=", "*=", "+=","-="};
        private string[] IsSpaceSymbol = { " ", "\r", "\n", "\0", "\t" };

        IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        string Value = "";
        public enum State { Start, Number, Variable, ChoiceLex, ArOperator,
            ReserveWords, Separators, Integer, Real,RealExp,String,Assigments,
            BlockComment,StringComment, Error}
        State state;
        BinaryReader Reader;
        int Ln = 0;
        int Ch = 0;
        int ChCounter = 0;
        string LexName;
        char SaveSymbol = '\0';

        public bool Check1 = true;
        public bool Check2 = true;
        public Lexer(BinaryReader reader)
        {
            Reader = reader;
        }
         public Lexema AddLex(int Ln, int Ch, string LexName, string Buff,string Value)
         {
            return new Lexema(Ln, Ch, LexName, Buff, Value);
         }
        public void Errors(string error)
        {
            Check1 = false;
            Check2 = false;
            string errors = $"{Ln}:{Ch} {error}";
            Console.WriteLine(errors);
        }
        private void GetNext()
        {
            if (Reader.PeekChar() != -1)
            {
                sm[0] = Reader.ReadChar();
                ChCounter++;
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
            Check1 = true;
            while (  Check2 || Check1   )
            {
                Check2 = true;
                switch (state)
                {
                    case State.Start:
                        Ch = ChCounter;
                        if (sm[0] == ' ' || sm[0] == '\t'  || sm[0] == '\r')
                        {
                            GetNext();
                        }
                        else if(sm[0] == '\n' || sm[0] == '\0')
                        {
                            GetNext();
                            ChCounter = 1;
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
                        else if (IsSeparators.Contains(sm[0].ToString()))
                        {
                            ClearBuf();
                            if (SaveSymbol != '\0')
                            {
                                buf += SaveSymbol;
                                SaveSymbol = '\0';
                            }
                            AddBuf(sm[0]);
                            state = State.Separators;
                            GetNext();
                        }
                        else if (IsArOperator.Contains(sm[0].ToString()))
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.ArOperator;
                            GetNext();
                        }
                        else if (sm[0] == '\'')
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.String;
                            GetNext();
                        }
                        else if (sm[0] == '{')
                        {
                            ClearBuf();
                            AddBuf(sm[0]);
                            state = State.BlockComment;
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
                            && (IsSpaceSymbol.Contains(sm[0].ToString()) || sm[0] == ';'))
                        {
                            state = State.Variable;
                        }
                        else if (IsReserveWords.Contains(buf) 
                            && (IsSpaceSymbol.Contains(sm[0].ToString()) || sm[0] == ';'))
                        {
                            state = State.ReserveWords;
                        }
                        else if (IsArOperator.Contains(buf)
                            && (IsSpaceSymbol.Contains(sm[0].ToString()) || sm[0] == ';'))
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
                            && (IsSpaceSymbol.Contains(sm[0].ToString())
                            || IsSeparators.Contains(sm[0].ToString())))
                        {
                            state = State.Integer;
                        }
                        else if (sm[0] == '.' )
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = State.Real;
                        }
                        else if (Char.ToLower(sm[0]) == 'e')
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = State.RealExp;
                        }
                        else if (Char.IsDigit(sm[0]) )
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Error;
                        }
                        break;

                    case State.Separators:
                        if (IsSeparators.Contains((buf).ToString())
                            && (IsSpaceSymbol.Contains(sm[0].ToString())
                            || Char.IsLetterOrDigit(sm[0])))
                        {
                            AddValue();
                            AddLexName();
                            state = State.Start;
                            Check2 = false;
                            return new Lexema(Ln, Ch, LexName, buf, Value);
                        }
                        else if (IsAssigments.Contains((buf).ToString())
                            && (IsSpaceSymbol.Contains(sm[0].ToString())))
                        {
                            state = State.Assigments;
                        }
                        else if (IsSeparators.Contains((buf).ToString()))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Error;
                        }
                        break;

                    case State.Real:
                        if (IsSpaceSymbol.Contains(sm[0].ToString()))
                        {
                            AddValue();
                            AddLexName();
                            state = State.Start ;
                            Check2 = false;
                            return new Lexema(Ln, Ch, LexName, buf, Value);
                        }
                        else if (Char.IsDigit(sm[0]))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else if (buf[buf.Length-1] != '.' && sm[0] == 'e')
                        {
                            AddBuf(sm[0]);
                            GetNext();
                            state = State.RealExp;
                        }
                        else if (sm[0] == '.' && buf[buf.Length - 1] == '.')
                        {
                            SaveSymbol = '.';
                            buf = buf.Substring(0, buf.Length - 1);
                            if (Int32.TryParse(buf, out int z))
                            {
                                AddValue();
                                state = State.Start;
                                return new Lexema(Ln, Ch, Convert.ToString(State.Integer), buf, Value);
                            }
                        }
                        else
                        {
                            state = State.Error;
                        }
                        break;

                    case State.RealExp:
                        if ((sm[0] == '+' || sm[0] == '-') && buf[buf.Length -1] == 'e')
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else if (Char.IsDigit(sm[0]))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else if (float.TryParse(buf, NumberStyles.Float, formatter, out float y) )
                        {
                            Value = Convert.ToString(float.Parse(buf, formatter));
                            AddLexName();
                            state = State.Start;
                            Check2 = false;
                            return new Lexema(Ln, Ch, LexName, buf, Value);
                        }
                        else
                        {
                            state = State.Error;
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

                    case State.Assigments:
                        AddValue();
                        AddLexName();
                        state = State.Start;
                        Check2 = false;
                        return new Lexema(Ln, Ch, LexName, buf, Value);

                    case State.ArOperator:
                        if (IsArOperator.Contains(buf)
                            && (IsSpaceSymbol.Contains(sm[0].ToString())))
                        {
                            AddValue();
                            AddLexName();
                            state = State.Start;
                            Check2 = false;
                            return new Lexema(Ln, Ch, LexName, buf, Value);
                        }
                        else if (IsAssigments.Contains((buf).ToString())
                          && (IsSpaceSymbol.Contains(sm[0].ToString())))
                        {
                            state = State.Assigments;
                        }
                        else if (buf == "//")
                        {
                            state = State.StringComment;
                        }
                        else if (Char.IsLetter(sm[0]) 
                            || IsArOperator.Contains((buf).ToString()) 
                            || IsAssigments.Contains((buf).ToString()))
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Error;
                        }
                        break;

                    case State.String:
                        if (sm[0] == '\'' )
                        {
                        AddBuf(sm[0]);
                        Value = buf.Trim(new Char[] { '\'' });
                        AddLexName();
                        state = State.Start;
                        Check2 = false;
                        return new Lexema(Ln, Ch, LexName, buf, Value);
                        }
                        else if(Reader.PeekChar() != -1)
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            AddBuf(sm[0]);
                            state = State.Error;
                        }
                        break;

                    case State.BlockComment:
                        if (sm[0] == '}')
                        {
                            GetNext();
                            AddBuf(sm[0]);
                            ClearBuf();
                        }
                        else if (Reader.PeekChar() != -1)
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Start;
                        }
                        break;

                    case State.StringComment:
                        if (sm[0] == '\r')
                        {
                            ClearBuf();
                            GetNext();
                        }
                        else if (Reader.PeekChar() != -1)
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        else
                        {
                            state = State.Start;
                        }
                        break;

                    case State.Error:
                        AddBuf(sm[0]);
                        AddValue();
                        AddLexName();
                        state = State.Start;
                        GetNext();
                        Errors("syntax error: "+ buf);
                        break;
                }
            }
            return new Lexema(0, 0, LexName, "", "");
            Console.WriteLine("Файл-" + (FileCounter));
        }
    }
}