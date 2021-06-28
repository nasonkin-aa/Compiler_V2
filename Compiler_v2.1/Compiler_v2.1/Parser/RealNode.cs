using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class RealNode : Node
    {
        private Lexema lexema;

        public RealNode(Lexema lexema)
        {
            this.lexema = lexema;
        }
    }
}
