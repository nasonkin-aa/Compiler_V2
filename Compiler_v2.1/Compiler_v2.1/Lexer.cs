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
        //Dictionary<string, string> NamLexema = new Dictionary<string, string>();

        char[] sm = new char[1];
        string buf = "";
        private string[] IsReserveWords = { "program", "var", "integer", "real", "bool", "begin", "end", "if", "then", "else", "while", "do", "read", "write", "true", "false" };
        private string[] IsArOperator = { "*", "/", "div", "mod", "and", "or", "+", "-", "=", "<", ">", "<>", "<=", ">=", "in", "not" };
        enum States { Start, NUM, VAR, FIN, ChoiceLex, ArOp, ResW, COM }
        States state;
        StringReader sr;
        int Ln = 0;
        int Ch = 0;

        public List<Lexema> NamLexema = new List<Lexema>();

        void AddLex(List<Lexema> lexemas,int Ln, int Ch,string States, string Buff)
        {
            lexemas.Add(new Lexema(Ln, Ch, States, Buff));
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
            while (state != States.FIN)
            {
                switch (state)
                {
                    case States.Start:
                        if (sm[0] == ' ')
                        {
                            GetNext();
                        }
                        else if(sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r')
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
                            state = States.NUM;
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
                            && sm[0] == ' ')
                        {
                            state = States.VAR;
                           

                        }
                        else if (IsReserveWords.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {

                            state = States.ResW;

                        }
                        else if (IsArOperator.Any(str => str == buf) 
                            && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {

                            state = States.ArOp;
                        }
                        else 
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                    break;
                    case States.NUM:
                        if (Int32.TryParse(buf, out int x) && (sm[0] == ' ' || sm[0] == '\n' || sm[0] == '\t' || sm[0] == '\0' || sm[0] == '\r'))
                        {
                            
                            Console.WriteLine( Ln.ToString()+Ch.ToString() + "num");
                            state = States.Start;
                            AddLex(NamLexema,Ln, Ch, "num", buf);
                            ClearBuf();
                        }
                        else
                        {
                            AddBuf(sm[0]);
                            GetNext();
                        }
                        break;

                    case States.VAR:
                        ClearBuf();
                        Console.WriteLine("VAR");
                        state = States.Start;
                        break;

                    case States.ResW:
                        
                        Console.WriteLine("resW");
                        state = States.Start;
                        AddLex(NamLexema, Ln, Ch, "resW", buf);
                        ClearBuf();
                        break;
                    case States.ArOp:
                        ClearBuf();
                        Console.WriteLine("ArOp");
                        state = States.Start;
                        break;


                    case States.FIN:
                        
                        //Console.WriteLine(buf);
                        break;
                }

            }
            //Console.WriteLine(buf);
            for (int i = 0; i < NamLexema.Count; i++)
            {
                Console.WriteLine(Convert.ToString(NamLexema[i].Ln) + Convert.ToString(NamLexema[i].Ch)+ NamLexema[i].Stats + NamLexema[i].Buff);
            }
        }
        

    }
}
