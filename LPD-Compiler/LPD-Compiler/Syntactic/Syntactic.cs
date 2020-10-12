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
        public Token token;

        public void syntacticAnalyser(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(token.simbolo == "sprograma")
            {
                token = lexicon.readToken();
                if (token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (token.simbolo == "sponto_vírgula")
                    {
                        analisaBloco(lexicon);
                        if (token.simbolo == "sponto")
                        {
                            // se acabou arquivo ou é comentário
                            // return Sucesso
                        }
                        else
                        {
                            // ERRO
                        }
                    }
                    else
                    {
                        // ERRO
                    }
                }
                else
                {
                    // ERRO
                }
            }
            else
            {
                // ERRO
            }
        }

        public void analisaBloco(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaEtVariaveis(lexicon);
            analisaSubrotinas(lexicon);
            analisaComandos(lexicon);
        }

        public void analisaEtVariaveis(Lexicon lexicon)
        {
            if (token.simbolo == "svar")
            {
                token = lexicon.readToken();
                if(token.simbolo == "sidentificador")
                {
                    while(token.simbolo == "sidentificador")
                    {
                        analisaVariaveis(lexicon);
                        if(token.simbolo == "sponto_vírgula")
                        {
                            token = lexicon.readToken();
                        }
                        else
                        {
                            // ERRO
                        }
                    }
                }
                else
                {
                    // ERRO
                }
            }
        }

        public void analisaVariaveis(Lexicon lexicon)
        {
            do
            {
                if (token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (token.simbolo == "svírgula" || token.simbolo == "sdoispontos")
                    {
                        if (token.simbolo == "svírgula")
                        {
                            token = lexicon.readToken();
                            if (token.simbolo == "sdoispontos")
                            {
                                // ERRO
                            }
                        }
                    }
                    else
                    {
                        // ERRO
                    }
                }
                else
                {
                    // ERRO
                }
            } while (token.simbolo != "sdoispontos");
            token = lexicon.readToken();
            analisaTipo(lexicon);
        }

        public void analisaTipo(Lexicon lexicon)
        {
            if(token.simbolo != "sinteiro" && token.simbolo != "sbooleano")
            {
                // ERRO
            }
            token = lexicon.readToken();
        }

        public void analisaComandos(Lexicon lexicon)
        {
            if(token.simbolo == "sinicio")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
                while(token.simbolo != "sfim")
                {
                    if(token.simbolo == "sponto_vírgula")
                    {
                        token = lexicon.readToken();
                        if(token.simbolo != "sfim")
                        {
                            analisaComandoSimples(lexicon);
                        }
                    }
                    else
                    {
                        // ERRO
                    }
                }
                token = lexicon.readToken();
            }
            else
            {
                // ERRO
            }
        }

        public void analisaComandoSimples(Lexicon lexicon)
        {
            if (token.simbolo == "sidentificador")
                AnalisaAtribChprocedimento(lexicon);
            else if (token.simbolo == "sse")
                analisaSe(lexicon);
            else if (token.simbolo == "senquanto")
                analisaEnquanto(lexicon);
            else if (token.simbolo == "sleia")
                analisaLeia(lexicon);
            else if (token.simbolo == "sescreva")
                analisaEscreva(lexicon);
            else
                analisaComandos(lexicon);
        }

        public void AnalisaAtribChprocedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(token.simbolo == "satribuicao")
            {
                analisaAtribuicao(lexicon);
            }
            else
            {
                analisaChamadaProcedimento(lexicon);
            }
        }

        public void analisaLeia(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(token.simbolo == "sabre_parênteses")
            {
                token = lexicon.readToken();
                if(token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (token.simbolo == "sfecha_parênteses")
                    {
                        token = lexicon.readToken();
                    }
                    else
                    {
                        // ERRO
                    }
                }
                else
                {
                    // ERRO
                }
            }
            else
            {
                // ERRO
            }
        }

        public void analisaEscreva(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (token.simbolo == "sabre_parenteses")
            {
                token = lexicon.readToken();
                if (token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (token.simbolo == "sfecha_parenteses")
                    {
                        token = lexicon.readToken();
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

        public void analisaEnquanto(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaExpressao(lexicon);
            if (token.simbolo == "sfaca")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
            }
            else
            {
                //ERRO
            }
        }

        public void analisaSe(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaExpressao(lexicon);
            if (token.simbolo == "sentao")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
                if (token.simbolo == "ssenao")
                {
                    token = lexicon.readToken();                 
                    analisaComandoSimples(lexicon);
                }
            }
            else
            {
                //ERRO
            }
        }

        public void analisaSubrotinas(Lexicon lexicon)
        {
            while (token.simbolo == "sprocedimento" || token.simbolo == "sfuncao")
            {
                if (token.simbolo == "sprocedimento")
                {
                    analisaDeclaracaoProcedimento(lexicon);      
                }
                else
                {
                    analisaDeclaracaoFuncao(lexicon);
                }

                if (token.simbolo == "sponto_virgula")
                {
                    token = lexicon.readToken();
                }
                else
                {
                    //ERRO
                }
            }
        }

        public void analisaDeclaracaoProcedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (token.simbolo == "sidentificador")
            {
                token = lexicon.readToken();
                if (token.simbolo == "sponto_virgula")
                {
                    analisaBloco(lexicon);
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

        public void analisaDeclaracaoFuncao(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (token.simbolo == "sidentificador")
            {
                token = lexicon.readToken();
                if (token.simbolo == "sdoispontos")
                {
                    token = lexicon.readToken();
                    if (token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                    {
                        token = lexicon.readToken();
                        if (token.simbolo == "sponto_virgula")
                        {
                            analisaBloco(lexicon);
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

        public void analisaExpressao(Lexicon lexicon)
        {
            analisaExpressaoSimples(lexicon);

            if (token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdiff")
            {
                token = lexicon.readToken();
                analisaExpressaoSimples(lexicon);
            }
        }

        public void analisaExpressaoSimples(Lexicon lexicon)
        {
            if (token.simbolo == "smais" || token.simbolo == "smenos")
            {
                token = lexicon.readToken();
                analisaTermo(lexicon);

                while (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou")
                {
                    token = lexicon.readToken();
                    analisaTermo(lexicon);
                }
            }
        }

        public void analisaTermo(Lexicon lexicon)
        {
            analisaFator(lexicon);

            while (token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                token = lexicon.readToken();
                analisaFator(lexicon);
            }
        }

        public void analisaFator(Lexicon lexicon)
        {
            if (token.simbolo == "sidentificador")
            {
                analisaChamadaFuncao(lexicon);
            }
            else
            {
                if (token.simbolo == "snumero")
                {
                    token = lexicon.readToken();
                }
                else
                {
                    if (token.simbolo == "snao")
                    {
                        token = lexicon.readToken();
                        analisaFator(lexicon);
                    }
                    else
                    {
                        if (token.simbolo == "sabre_parenteses")
                        {
                            token = lexicon.readToken();
                            analisaExpressao(lexicon);  //analisaExpressao(token); 
                            if (token.simbolo == "sfecha_parenteses")
                            {
                                token = lexicon.readToken();
                            }
                            else
                            {
                                //ERRO
                            }
                        }
                        else
                        {
                            if (token.simbolo == "sverdadeiro" || token.simbolo == "sfalso")
                            {
                                token = lexicon.readToken();
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

        public void analisaAtribuicao(Lexicon lexicon)
        {
            analisaExpressao(lexicon);
        }

        public void analisaChamadaProcedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
        }

        public void analisaChamadaFuncao(Lexicon lexicon)
        {
            token = lexicon.readToken();
        }

    }
}