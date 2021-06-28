using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class BinaryOpNode : Node
    {
        private Lexema operation;
        private Node left;
        private Node right;

        public BinaryOpNode(Lexema operation, Node left, Node right)
        {
            this.operation = operation;
            this.left = left;
            this.right = right;
        }
    }
}
