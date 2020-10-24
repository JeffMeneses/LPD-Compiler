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
        public Stack<string> tabela = new Stack<string>();
        public List<string> listaSimbolosTab = new List<string>();

        public void insereTabela(string lexema, string simbolo)
        {
            tabela.Push(lexema);  //adiciona elemento no inicio da pilha

            colocaTipoTabela(lexema,simbolo);
        }

        public void desempilhaTabela()
        {
            tabela.Pop(); //retira o elemento mais recente colocado na pilha
        }

        public int pesquisaTabela(string lexema)
        {
            return 1; //retorna posicao do lexema na pilha caso exista
        }

        public void pesquisaDuplicVarTabela(string lexema)
        {

        }

        public void colocaTipoTabela(string lexema, string tipo)
        {
            int posicao = pesquisaTabela(lexema);

            listaSimbolosTab[posicao] = tipo; 
            //a posicao em que o lexema foi colocado na pilha corresponde a posicao do seu simbolo/tipo na listaSimbolosTab
           
        }

        public void pesquisaDeclVarTabela(string lexema) 
        {

        }

        public void pesquisaDeclFuncTabela(string lexema)
        {

        }

        public void pesquisaDeclProcTabela(string lexema)
        {

        }


    }
}

/*
   pilha.Peek(); //pega o elemento mais recente/topo sem retirá-lo

   pilha.Clear(); //limpa todos os elementos da pilha

   pilha.Count; 
 */