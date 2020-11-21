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

        public void insereTabela(string simbolo, string tipo, int nivel, int rotulo) //o ultimo parametro nao sei ainda direito -ok
        {
            Item item = new Item();

            item.simbolo = simbolo;
            item.tipo = tipo;
            item.nivel = nivel;
            item.rotulo = rotulo;

            tabelaDeSimbolos.Push(item);  
        }

        public void desempilhaTabela(string indicador)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == indicador)
                    break;
                tabelaDeSimbolos.Pop();
            }
        }

        public void colocaTipoTabela(string tipo, string ultimaVar)  //VERIFICAR
        {

            foreach (var item in tabelaDeSimbolos)
            {
                if(item.simbolo == ultimaVar)
                {
                    item.tipo = string.Concat(item.tipo, tipo);
                    break;
                }               
                item.tipo = string.Concat(item.tipo, tipo);
               
            }
        }

        public int pesquisaDuplicVarTabela(string simbolo) //ok
        {
            int flag = 0;
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro" && flag == 0 ||
                    item.simbolo == simbolo && item.tipo == "varBooleano" && flag == 0 ||
                    item.simbolo == simbolo && item.tipo == "var" && flag == 0)
                {
                    if (item.simbolo == simbolo)
                    {
                        return 1;
                    }
                }
                else
                {
                    if (item.tipo == "funcInteiro" || item.tipo == "funcBooleano" || item.tipo == "procedimento" || item.tipo == "nomedeprograma")
                    {
                        flag++; //mudou de nivel, significa que pode haver variaveis com mesmo nome
                        if (item.simbolo == simbolo)
                        {
                            return 1;
                        }
                    }
                }
            }
            return 0;

        }

        public int pesquisaDeclVarTabela(string simbolo) //ok
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

        public int pesquisaDeclVarFuncTabela(string simbolo) //ok
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

        public int pesquisaDeclFuncTabela(string simbolo)//ok
        {
            return pesquisaTabela(simbolo);
        }

        public int pesquisaDeclProcTabela(string simbolo) //ok
        {
            return pesquisaTabela(simbolo);
        }

        public int pesquisaTabela(string simbolo) //ok
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

        public Item retornaUltimoAdd() //ok
        {
            return tabelaDeSimbolos.Peek();
        }
    }
}
