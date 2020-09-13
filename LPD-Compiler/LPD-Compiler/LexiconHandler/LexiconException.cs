using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPD_Compiler.LexiconHandler
{
    class LexiconException : Exception
    {
        public LexiconException()
        {

        }

        public LexiconException(int line)
            :base(String.Format("Unknown token found on line: {0}", line))
        {

        }
    }
}
