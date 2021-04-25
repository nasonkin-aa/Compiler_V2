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
        public Lexer.States States;
        public readonly string Output;

        public Lexema(int ln, int ch, Lexer.States states, string buff,string output)
        {
            Ln = ln;
            Ch = ch;
            this.States = states;
            Buff = buff;
            Output = output;

        }
    }
}
