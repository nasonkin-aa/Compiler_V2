﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    abstract class Node
    {
        abstract public string Print(int priority = 0);
        abstract public string GetValue();

    }
}
