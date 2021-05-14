using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class Lexema
    {
        public readonly int Ln;
        public readonly int Ch;
        public readonly string Buff;
        public Lexer.State States;
        public readonly string Value;



        public Lexema(int Ln, int Ch, Lexer.State Lexema, string Buff,string Value)
        {
            this.Ln = Ln;   
            this.Ch = Ch;
            this.States = Lexema;
            this.Buff = Buff;
            this.Value = Value;

        }
    }
}
