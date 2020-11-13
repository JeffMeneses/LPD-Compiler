using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;

namespace LPD_Compiler.SemanticHandler
{
    public class Item
    {
        public String simbolo;
        public String tipo;
        public int nivel;
        public int rotulo;
        //int memoria;

        public Item()
        {

        }

        public Item(String simbolo, String tipo, int nivel, int rotulo)
        {
            this.simbolo = simbolo;
            this.tipo = tipo;
            this.nivel = nivel;
            this.rotulo = rotulo;

        }

    }
}

