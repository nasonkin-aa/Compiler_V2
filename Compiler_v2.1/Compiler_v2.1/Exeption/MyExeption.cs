using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler_v2._1
{
    public class MyExeption : Exception
    {
       
        public MyExeption(string text) : base(text)
        {
        }
    }
}

