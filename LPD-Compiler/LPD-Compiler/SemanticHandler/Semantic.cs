using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.SyntacticHandler;

namespace LPD_Compiler.SemanticHandler
{
    public class Semantic
    {
        public Stack<Item> tabelaDeSimbolos = new Stack<Item>();

        public void insereTabela(string simbolo, string tipo, int nivel, int rotulo) //o ultimo parametro nao sei ainda direito
        {
            Item item = new Item();

            item.simbolo = simbolo;
            item.tipo = tipo;
            item.nivel = nivel;
            item.rotulo = rotulo;

            tabelaDeSimbolos.Push(item);  
        }

        public void desempilhaTabela()
        {
            tabelaDeSimbolos.Pop(); 
        }

        public void colocaTipoTabela(string tipo, string ultimaVar)  
        {
            int posicao = 0, aux =0;

            foreach (var item in tabelaDeSimbolos)
            {
                if(item.simbolo == ultimaVar)
                {
                    posicao = aux;
                }
                if(aux >= posicao)
                {
                    item.tipo = string.Concat(item.tipo, tipo);
                }
                aux++;
            }
        }

        public int pesquisaDuplicVarTabela(string simbolo) //n sei se precisa ver o nivel aqui
        {
            foreach (var item in tabelaDeSimbolos)
            {
               if(item.simbolo == simbolo) //variavel
               {
                    return 1;
               }
            }
            return 0;

        }

        public int pesquisaDeclVarTabela(string simbolo) 
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro" || item.simbolo == simbolo && item.tipo == "varBooleano") 
                {
                   return 1;
                }
                               
            }
            return 0;
        }

        public int pesquisaDeclVarFuncTabela(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro" || item.simbolo == simbolo && item.tipo == "varBooleano")
                {
                   return 1;
                }
                else if(item.simbolo == simbolo && item.tipo == "funcInteiro" || item.simbolo == simbolo && item.tipo == "funcBooleano")
                {
                     return 2;
                }
            }
            return 0;
        }

        public int pesquisaDeclFuncTabela(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "funcInteiro" || item.simbolo == simbolo && item.tipo == "funcBooleano")
                {
                    return 1;
                }
            }
            return 0;
        }

        public int pesquisaDeclProcTabela(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "procedimento")
                {
                    return 1;
                }
            }
            return 0;
        }

        public int pesquisaTabela(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo)
                {
                    return 1;
                }
            }
            return 0;
        }

        public Item retornaUltimoAdd()
        {
            return tabelaDeSimbolos.Peek();
        }
    }
}
