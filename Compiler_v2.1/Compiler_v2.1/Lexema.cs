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
        public readonly string Stats;
        public readonly string Buff;

        public Lexema(int Ln, int Ch, string Stats, string Buff)
        {
            this.Ln = Ln;
            this.Ch = Ch;
            this.Stats = Stats;
            this.Buff = Buff;
        }
    }
}
