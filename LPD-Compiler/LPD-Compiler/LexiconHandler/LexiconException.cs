using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPD_Compiler.LexiconHandler
{
    public class LexiconException : Exception
    {
        public LexiconException()
        {

        }

        public LexiconException(int line, string message)
            :base(String.Format("{1} na linha {0}.", line, message))
        {

        }
    }
}
