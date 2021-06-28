using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class IntegerNode : Node
    {
        int Value;
        Lexema lexema;

        public IntegerNode(Lexema lexema)
        {
            this.lexema = lexema;
        }
    }
}
