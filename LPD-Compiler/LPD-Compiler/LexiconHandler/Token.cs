﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LPD_Compiler.LexiconHandler
{
    class Token
    {
        public String simbolo;
        public String lexema;

        public Token(String simbolo, String lexema)
        {
            this.simbolo = simbolo;
            this.lexema = lexema;
        }
    }
}
