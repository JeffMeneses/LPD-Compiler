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
            updateToken(lexicon);
            if(!isErrorToken(token) && token.simbolo == "sprograma")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    updateToken(lexicon);
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
            updateToken(lexicon);
            analisaEtVariaveis(lexicon);
            analisaSubrotinas(lexicon);
            analisaComandos(lexicon);
        }

        public void analisaEtVariaveis(Lexicon lexicon)
        {
            if (!isErrorToken(token) && token.simbolo == "svar")
            {
                updateToken(lexicon);
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    while(!isErrorToken(token) && token.simbolo == "sidentificador")
                    {
                        analisaVariaveis(lexicon);
                        if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                        {
                            updateToken(lexicon);
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
                    updateToken(lexicon);
                    if (!isErrorToken(token) && (token.simbolo == "svirgula" || token.simbolo == "sdoispontos"))
                    {
                        if (!isErrorToken(token) && token.simbolo == "svirgula")
                        {
                            updateToken(lexicon);
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
            updateToken(lexicon);
            analisaTipo(lexicon);
        }

        public void analisaTipo(Lexicon lexicon)
        {
            if(!isErrorToken(token) && (token.simbolo != "sinteiro" && token.simbolo != "sbooleano"))
            {
                throw new SyntacticException(token.line);
            }
            updateToken(lexicon);
        }

        public void analisaComandos(Lexicon lexicon)
        {
            if(!isErrorToken(token) && token.simbolo == "sinicio")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon);
                while(token.simbolo != "sfim")
                {
                    if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        updateToken(lexicon);
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
                updateToken(lexicon);
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
            updateToken(lexicon);
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
            updateToken(lexicon);
            if(!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                updateToken(lexicon);
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                    {
                        updateToken(lexicon);
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
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                    {
                        updateToken(lexicon);
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
            updateToken(lexicon);
            analisaExpressao(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sfaca")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon);
            }
            else
            {
                throw new SyntacticException(token.line);
            }
        }

        public void analisaSe(Lexicon lexicon)
        {
            updateToken(lexicon);
            analisaExpressao(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sentao")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon);
                if (!isErrorToken(token) && token.simbolo == "ssenao")
                {
                    updateToken(lexicon);                 
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
                    updateToken(lexicon);
                }
                else
                {
                    throw new SyntacticException(token.line);
                }
            }
        }

        public void analisaDeclaracaoProcedimento(Lexicon lexicon)
        {
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                updateToken(lexicon);
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
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                {
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                    {
                        updateToken(lexicon);
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
                updateToken(lexicon);
                analisaExpressaoSimples(lexicon);
            }
        }

        public void analisaExpressaoSimples(Lexicon lexicon)
        {
            if (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos"))
                updateToken(lexicon);
            analisaTermo(lexicon);

            while (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou"))
            {
                updateToken(lexicon);
                analisaTermo(lexicon);
            }
        }

        public void analisaTermo(Lexicon lexicon)
        {
            analisaFator(lexicon);

            while (token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                updateToken(lexicon);
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
                    updateToken(lexicon);
                }
                else
                {
                    if (token.simbolo == "snao")
                    {
                        updateToken(lexicon);
                        analisaFator(lexicon);
                    }
                    else
                    {
                        if (token.simbolo == "sabre_parenteses")
                        {
                            updateToken(lexicon);
                            analisaExpressao(lexicon);  //analisaExpressao(token); 
                            if (token.simbolo == "sfecha_parenteses")
                            {
                                updateToken(lexicon);
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
                                updateToken(lexicon);
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
            updateToken(lexicon);
            analisaExpressao(lexicon);
        }

        public void analisaChamadaProcedimento(Lexicon lexicon)
        {
            //updateToken(lexicon);
        }

        public void analisaChamadaFuncao(Lexicon lexicon)
        {
            updateToken(lexicon);
        }

        public bool isErrorToken(Token token)
        {
            if (token.simbolo == "serro")
            {
                throw new LexiconException(token.line);
            }
            return false;
        }

        public Token updateToken(Lexicon lexicon)
        {
            if(lexicon.i >= lexicon.listTokens.Count) throw new LexiconException(token.line);

            token = lexicon.readToken();
            return token;
        }

    }
}