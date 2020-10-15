using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPD_Compiler.SyntacticHandler
{
    public class SyntacticException : Exception
    {
        public SyntacticException()
        {

        }

        public SyntacticException(int line)
            : base(String.Format("Syntactic error found on line {0}", line))
        {

        }

    }
}
