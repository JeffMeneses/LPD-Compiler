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

        public void insereTabela(string simbolo, string tipo, int nivel, int rotulo)
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
            int cont = 0;
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == indicador)
                    break;
                cont++;
            }

            for (int i = 0; i < cont; i++)
                tabelaDeSimbolos.Pop();
        }

        public void colocaTipoTabela(string tipo, string ultimaVar)
        {

            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == ultimaVar)
                {
                    item.tipo = string.Concat(item.tipo, tipo);
                    break;
                }
                item.tipo = string.Concat(item.tipo, tipo);

            }
        }

        public int pesquisaDuplicVarTabela(string simbolo)
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
                else if (item.simbolo == simbolo && item.tipo == "funcInteiro" || item.simbolo == simbolo && item.tipo == "funcBooleano")
                {
                    return 2;
                }
            }
            return 0;
        }

        public int pesquisaDeclFuncTabela(string simbolo)
        {
            return pesquisaTabela(simbolo);
        }

        public int pesquisaDeclProcTabela(string simbolo)
        {
            return pesquisaTabela(simbolo);
        }

        public int pesquisaProcTabela(string simbolo)
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

        public int validaEscrevaELeia(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro")
                {
                    return 1;
                }
            }
            return 0;
        }

        public int validaAtribuicao(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro")
                {
                    return 1;
                }
                else if (item.simbolo == simbolo && item.tipo == "varBooleano")
                {
                    return 0;
                }
            }
            return -1;
        }

        public int retornaTipo(string simbolo)
        {
            foreach (var item in tabelaDeSimbolos)
            {
                if (item.simbolo == simbolo && item.tipo == "varInteiro")
                {
                    return 1;
                }
                else if (item.simbolo == simbolo && item.tipo == "varBooleano")
                {
                    return 2;
                }
            }
            return 0;
        }

        public int validaCompatibilidadeTipo(List<string> expressao)
        {
            int posicao = 0;
            List<string> exAux = new List<string>();


            foreach (string termo in expressao)
            {
                exAux.Add(expressao[posicao]);
                posicao++;
            }

            posicao = 0;

            foreach (string termo in expressao)
            {
                if (expressao.Count != 1)
                {
                    if (termo == "+" || termo == "-" || termo == "*" || termo == "div")
                    {
                        if (int.TryParse(exAux[posicao - 2], out _) || retornaTipo(exAux[posicao - 2]) == 1)
                        {
                            if (int.TryParse(exAux[posicao - 1], out _) || retornaTipo(exAux[posicao - 1]) == 1)
                            {
                                if ((posicao + 1) == expressao.Count)
                                    return 1; // inteiro
                                exAux[posicao] = "1";
                            }
                        }

                    }
                    else if (termo == ">" || termo == "<" || termo == ">=" || termo == "<=")
                    {

                        if (int.TryParse(exAux[posicao - 2], out _) || retornaTipo(exAux[posicao - 2]) == 1)
                        {
                            if (int.TryParse(exAux[posicao - 1], out _) || retornaTipo(exAux[posicao - 1]) == 1)
                            {
                                if ((posicao + 1) == exAux.Count)
                                    return 0; // booleano
                                exAux[posicao] = "bool";
                            }
                        }

                    }
                    else if (termo == "=" || termo == "!=")
                    {
                        if (retornaTipo(exAux[posicao - 2]) == 2 || exAux[posicao - 2] == "bool")
                        {
                            if (retornaTipo(exAux[posicao - 1]) == 2 || exAux[posicao - 1] == "bool")
                            {
                                if ((posicao + 1) == expressao.Count)
                                    return 0; // booleano
                                exAux[posicao] = "bool";
                            }
                        }

                    }
                    else if (termo == "-u" || termo == "+u")
                    {
                        if (int.TryParse(exAux[posicao - 1], out _) || retornaTipo(exAux[posicao - 1]) == 1)
                        {
                            if ((posicao + 1) == expressao.Count)
                                return 1; // inteiro
                            exAux[posicao] = "1";
                        }

                    }
                    else if (termo == "nao")
                    {
                        if (retornaTipo(exAux[posicao - 1]) == 2 || exAux[posicao - 1] == "bool")
                        {
                            if ((posicao + 1) == expressao.Count)
                                return 0; // booleano
                            exAux[posicao] = "bool";
                        }

                    }
                    else if (termo == "e" || termo == "ou")
                    {
                        if (retornaTipo(exAux[posicao - 2]) == 2 || exAux[posicao - 2] == "bool")
                        {
                            if (retornaTipo(exAux[posicao - 1]) == 2 || exAux[posicao - 1] == "bool")
                            {
                                if ((posicao + 1) == expressao.Count)
                                    return 0; // booleano
                                exAux[posicao] = "bool";
                            }
                        }

                    }
                }
                else
                {
                    if (retornaTipo(exAux[posicao]) == 2 || exAux[posicao] == "falso" || exAux[posicao] == "verdadeiro")
                    {
                        return 0;
                    }
                    else if (int.TryParse(exAux[posicao], out _) || retornaTipo(exAux[posicao]) == 1)
                    {
                        return 1;
                    }
                }
                posicao++;
            }

            return -1; //erro
        }

        public Item retornaUltimoAdd()
        {
            return tabelaDeSimbolos.Peek();
        }
    }
}
