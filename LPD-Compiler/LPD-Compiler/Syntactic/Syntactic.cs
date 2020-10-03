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
                        Analisa_bloco(lexicon);
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

        public void Analisa_bloco(Lexicon lexicon)
        {
            token = lexicon.readToken();
            Analisa_et_variaveis(lexicon);
            Analisa_subrotinas(lexicon);
            Analisa_comandos(lexicon);
        }

        public void Analisa_et_variaveis(Lexicon lexicon)
        {
            if (token.simbolo == "svar")
            {
                token = lexicon.readToken();
                if(token.simbolo == "sidentificador")
                {
                    while(token.simbolo == "sidentificador")
                    {
                        Analisa_variaveis(lexicon);
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

        public void Analisa_variaveis(Lexicon lexicon)
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
            Analisa_Tipo(lexicon);
        }

        public void Analisa_Tipo(Lexicon lexicon)
        {
            if(token.simbolo != "sinteiro" && token.simbolo != "sbooleano")
            {
                // ERRO
            }
            token = lexicon.readToken();
        }

        public void Analisa_comandos(Lexicon lexicon)
        {
            if(token.simbolo == "sinicio")
            {
                token = lexicon.readToken();
                Analisa_comando_simples(lexicon);
                while(token.simbolo != "sfim")
                {
                    if(token.simbolo == "sponto_vírgula")
                    {
                        token = lexicon.readToken();
                        if(token.simbolo != "sfim")
                        {
                            Analisa_comando_simples(lexicon);
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

        public void Analisa_comando_simples(Lexicon lexicon)
        {
            if (token.simbolo == "sidentificador")
                Analisa_atrib_chprocedimento(lexicon);
            else if (token.simbolo == "sse")
                Analisa_se(lexicon);
            else if (token.simbolo == "senquanto")
                Analisa_enquanto(lexicon);
            else if (token.simbolo == "sleia")
                Analisa_leia(lexicon);
            else if (token.simbolo == "sescreva")
                Analisa_escreva(lexicon);
            else
                Analisa_comandos(lexicon);
        }

        public void Analisa_atrib_chprocedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(token.simbolo == "satribuicao")
            {
                Analisa_Atribuicao(lexicon);
            }
            else
            {
                Analisa_chamada_procedimento(lexicon);
            }
        }

        public void Analisa_leia(Lexicon lexicon)
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

        /*public void analisaEscreva()
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

            if (token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdiff")
            {
                //Lexico(token);
                //analisaExpressaoSimples();
            }
        }

        public void analaisaExpressaoSimples()
        {
            if (token.simbolo == "smais" || token.simbolo == "smenos")
            {
                //Lexico(token);
                //analisaTermo();

                while (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou")
                {
                    //Lexico(token);
                    //analisaTermo();
                }
            }
        }

        public void analisaTermo()
        {
            //analisaFator();

            while (token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                //Lexico(token);
                //analisaFator();
            }
        }

        public void analisaFator()
        {
            if (token.simbolo == "sidentificador")
            {
                //analisaChamadaFuncao
            }
            else
            {
                if (token.simbolo == "snumero")
                {
                    //Lexico(token);
                }
                else
                {
                    if (token.simbolo == "snao")
                    {
                        //Lexico(token);
                        //analisaFator();
                    }
                    else
                    {
                        if (token.simbolo == "sabre_parenteses")
                        {
                            //Lexico(token);
                            //analisaExpressao(token);  
                            if (token.simbolo == "sfecha_parenteses")
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
                            if (token.simbolo == "sverdadeiro" || token.simbolo == "sfalso")
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
        }*/


        // TODO: analisaAtribuicao, analisaChamadaProcedimento e analisaFuncao
    }
}