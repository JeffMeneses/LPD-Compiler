using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.SyntacticHandler;

namespace LPD_Compiler.SyntacticHandler
{
    public class Syntactic
    {
        public Token token;

        public void syntacticAnalyser(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(!isErrorToken(token) && token.simbolo == "sprograma")
            {
                token = lexicon.readToken();
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        analisaBloco(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sponto")
                        {
                            Console.WriteLine("Deu certo");
                            // se acabou arquivo ou é comentário
                            // return Sucesso
                        }
                        else
                        {
                            throw new SyntacticException(token.line);
                        }
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
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
            if (!isErrorToken(token) && token.simbolo == "svar")
            {
                token = lexicon.readToken();
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    while(!isErrorToken(token) && token.simbolo == "sidentificador")
                    {
                        analisaVariaveis(lexicon);
                        if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                        {
                            token = lexicon.readToken();
                        }
                        else
                        {
                            throw new SyntacticException(token.line);
                        }
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
        }

        public void analisaVariaveis(Lexicon lexicon)
        {
            do
            {
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (!isErrorToken(token) && (token.simbolo == "svirgula" || token.simbolo == "sdoispontos"))
                    {
                        if (!isErrorToken(token) && token.simbolo == "svirgula")
                        {
                            token = lexicon.readToken();
                            if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                            {
                                throw new SyntacticException(token.line);
                            }
                        }
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            } while (token.simbolo != "sdoispontos");
            token = lexicon.readToken();
            analisaTipo(lexicon);
        }

        public void analisaTipo(Lexicon lexicon)
        {
            if(!isErrorToken(token) && (token.simbolo != "sinteiro" && token.simbolo != "sbooleano"))
            {
                throw new SyntacticException(token.line);
            }
            token = lexicon.readToken();
        }

        public void analisaComandos(Lexicon lexicon)
        {
            if(!isErrorToken(token) && token.simbolo == "sinicio")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
                while(token.simbolo != "sfim")
                {
                    if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        token = lexicon.readToken();
                        if(!isErrorToken(token) && token.simbolo != "sfim")
                        {
                            analisaComandoSimples(lexicon);
                        }
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                token = lexicon.readToken();
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaComandoSimples(Lexicon lexicon)
        {
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
                AnalisaAtribChprocedimento(lexicon);
            else if (!isErrorToken(token) && token.simbolo == "sse")
                analisaSe(lexicon);
            else if (!isErrorToken(token) && token.simbolo == "senquanto")
                analisaEnquanto(lexicon);
            else if (!isErrorToken(token) && token.simbolo == "sleia")
                analisaLeia(lexicon);
            else if (!isErrorToken(token) && token.simbolo == "sescreva")
                analisaEscreva(lexicon);
            else
                analisaComandos(lexicon);
        }

        public void AnalisaAtribChprocedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if(!isErrorToken(token) && token.simbolo == "satribuicao")
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
            if(!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                token = lexicon.readToken();
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                    {
                        token = lexicon.readToken();
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaEscreva(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                token = lexicon.readToken();
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    token = lexicon.readToken();
                    if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                    {
                        token = lexicon.readToken();
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaEnquanto(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaExpressao(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sfaca")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaSe(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaExpressao(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sentao")
            {
                token = lexicon.readToken();
                analisaComandoSimples(lexicon);
                if (!isErrorToken(token) && token.simbolo == "ssenao")
                {
                    token = lexicon.readToken();                 
                    analisaComandoSimples(lexicon);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaSubrotinas(Lexicon lexicon)
        {
            while (!isErrorToken(token) && (token.simbolo == "sprocedimento" || token.simbolo == "sfuncao"))
            {
                if (!isErrorToken(token) && token.simbolo == "sprocedimento")
                {
                    analisaDeclaracaoProcedimento(lexicon);      
                }
                else
                {
                    analisaDeclaracaoFuncao(lexicon);
                }

                if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                {
                    token = lexicon.readToken();
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
        }

        public void analisaDeclaracaoProcedimento(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                token = lexicon.readToken();
                if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                {
                    analisaBloco(lexicon);
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaDeclaracaoFuncao(Lexicon lexicon)
        {
            token = lexicon.readToken();
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                token = lexicon.readToken();
                if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                {
                    token = lexicon.readToken();
                    if (!isErrorToken(token) && token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                    {
                        token = lexicon.readToken();
                        if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                        {
                            analisaBloco(lexicon);
                        }
                        //else
                        //{
                            throw new SyntacticException(token.line);
                        //}
                    }
                    else
                    {
                        throw new SyntacticException(token.line);
                    }
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaExpressao(Lexicon lexicon)
        {
            analisaExpressaoSimples(lexicon);

            if (!isErrorToken(token) && (token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdif"))
            {
                token = lexicon.readToken();
                analisaExpressaoSimples(lexicon);
            }
        }

        public void analisaExpressaoSimples(Lexicon lexicon)
        {
            if (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos"))
                token = lexicon.readToken();
            analisaTermo(lexicon);

            while (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou"))
            {
                token = lexicon.readToken();
                analisaTermo(lexicon);
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
                                throw new SyntacticException(token.line);
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
                                throw new SyntacticException(token.line);
                            }
                        }
                    }
                }
            }
        }

        public void analisaAtribuicao(Lexicon lexicon)
        {
            token = lexicon.readToken();
            analisaExpressao(lexicon);
        }

        public void analisaChamadaProcedimento(Lexicon lexicon)
        {
            //token = lexicon.readToken();
        }

        public void analisaChamadaFuncao(Lexicon lexicon)
        {
            token = lexicon.readToken();
        }

        public bool isErrorToken(Token token)
        {
            if (token.simbolo == "serro")
            {
                throw new LexiconException(token.line);
            }
            return false;
        }

    }
}