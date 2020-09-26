using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;

namespace LPD_Compiler.SyntacticHandler
{
    public class Syntactic
    {

        //ANALISA X

        public void analisaEscreva()
        {
            //Lexico(token) - pega 1 token por vez
            if (token.simbolo == "sabre_parenteses")
            {
                //Lexico(token)
                if (token.simbolo == "sidentificador")
                {
                    //Lexico(token)
                    if (token.simbolo == "sfecha_parenteses")
                    {
                        //Lexico(token)
                    }
                    else
                    {
                        //ERRO
                    }
                }
                else
                {
                    //ERRO
                }
            }
            else
            {
                //ERRO
            }
        }

        public void analisaEnquanto()
        {
            //Lexico(token);
            // analisaExpressao();
            if (token.simbolo == "sfaca")
            {
                //Lexico(token)
                //analisaComandoSimples();
            }
            else
            {
                //ERRO
            }
        }

        public void analisaSe()
        {
            //Lexico(token);
            //analisaExpressao();
            if (token.simbolo == "sentao")
            {
                //Lexico(token);
                //analisaComandoSimples();
                if (token.simbolo == "ssenao")
                {
                    //Lexico(token);                 
                    //analisaComandoSimples();
                }
            }
            else
            {
                //ERRO
            }
        }

        public void analisaSubRotinas()
        {

            while (token.simbolo == "sprocedimento" || token.simbolo == "sfuncao")
            {
                if (token.simbolo == "sprocedimento")
                {
                    //analisaDeclaracaoProcedimento();      
                }
                else
                {
                    //analisaDeclaracaoFuncao();
                }

                if (token.simbolo == "sponto_virgula")
                {
                    //Lexico(token);
                }
                else
                {
                    //ERRO
                }
            }
        }

        public void analisaDeclaracaoProcedimento()
        {
            //Lexico(token);
            if (token.simbolo == "sidentificador")
            {
                //Lexico(token);
                if (token.simbolo == "sponto_virgula")
                {
                    //analisaBloco();
                }
                else
                {
                    //ERRO
                }
            }
            else
            {
                //ERRO
            }
        }

        public void analisaDeclaracaoFuncao()
        {
            //Lexico(token);
            if (token.simbolo == "sidentificador")
            {
                //Lexico(token);
                if (token.simbolo == "sdoispontos")
                {
                    //Lexico(token);
                    if (token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                    {
                        //Lexico(token);
                        if (token.simbolo == "sponto_virgula")
                        {
                            //analisaBloco();
                        }
                        else
                        {
                            //ERRO
                        }
                    }
                    else
                    {
                        //ERRO
                    }
                }
                else
                {
                    //ERRO
                }
            }
            else
            {
                //ERRO
            }
        }

        public void analisaExpressao()
        {
            //analisaExpressaoSimples();
            
            if(token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdiff")
            {
                //Lexico(token);
                //analisaExpressaoSimples();
            }
        }

        public void analaisaExpressaoSimples()
        {
            if(token.simbolo == "smais" || token.simbolo == "smenos")
            {
                //Lexico(token);
                //analisaTermo();
                
                while(token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou")
                {
                    //Lexico(token);
                    //analisaTermo();
                }
            }
        }

        public void analisaTermo()
        {
            //analisaFator();

            while(token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                //Lexico(token);
                //analisaFator();
            }
        }

        public void analisaFator()
        {
            if(token.simbolo == "sidentificador")
            {
                //analisaChamadaFuncao
            }
            else
            {
                if(token.simbolo == "snumero")
                {
                    //Lexico(token);
                }
                else
                {
                    if(token.simbolo == "snao")
                    {
                        //Lexico(token);
                        //analisaFator();
                    }
                    else
                    {
                        if(token.simbolo == "sabre_parenteses")
                        {
                            //Lexico(token);
                            //analisaExpressao(token);  
                            if(token.simbolo == "sfecha_parenteses")
                            {
                                //Lexico(token);
                            }
                            else
                            {
                                //ERRO
                            }
                        }
                        else
                        {
                            if(token.simbolo == "sverdadeiro" || token.simbolo == "sfalso")
                            {
                                //Lexico(token);
                            }
                            else
                            {
                                //ERRO
                            }
                        }
                    }
                }
            }
        }

    }
}
