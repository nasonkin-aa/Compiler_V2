using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class UnaryOpNode : Node
    {
        private Node operand;
        private Lexema lexema;

        public UnaryOpNode(Node operand, Lexema lexema)
        {
            this.operand = operand;
            this.lexema = lexema;
        }
    }
}
