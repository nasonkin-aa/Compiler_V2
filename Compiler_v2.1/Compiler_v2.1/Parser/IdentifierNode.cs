﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    class IdentifierNode: Node
    {
        string Name;
        private Lexema lexema;

        public IdentifierNode(Lexema lexema)
        {
            this.lexema = lexema;
        }
    }
}