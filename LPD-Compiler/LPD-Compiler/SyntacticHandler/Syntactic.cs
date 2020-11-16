using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.SyntacticHandler;
using LPD_Compiler.SemanticHandler;

namespace LPD_Compiler.SyntacticHandler
{
    public class Syntactic
    {
        public Token token;
        public string message="", flagVar ="";
        public int line= 0;

        public void syntacticAnalyser(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if(!isErrorToken(token) && token.simbolo == "sprograma")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    semantic.insereTabela(token.lexema, "nomedeprograma", 0,0); //ver os 2 ultimos parametros
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        analisaBloco(lexicon, semantic);
                        if (!isErrorToken(token) && token.simbolo == "sponto")
                        {
                            Console.WriteLine("Deu certo");
                            // se acabou arquivo ou é comentário
                            // return Sucesso
                        }
                        else
                        {
                            line = token.line;
                            message = "Ponto final não encontrado";
                            throw new SyntacticException(token.line, "Ponto final não encontrado");                            
                        }
                    }
                    else
                    {
                        line = token.line;
                        message = "Ponto e vírgula não encontrado";
                        throw new SyntacticException(token.line, "Ponto e vírgula não encontrado");
                    }
                }
                else
                {
                    line = token.line;
                    message = "Identificador não encontrado";
                    throw new SyntacticException(token.line, "Identificador não encontrado");
                }
            }
            else
            {
                line = token.line;
                message = "Palavra reservada 'programa' não encontrada";
                throw new SyntacticException(token.line, "Palavra reservada 'programa' não encontrada");
            }
        }

        public void analisaBloco(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            analisaEtVariaveis(lexicon, semantic);
            analisaSubrotinas(lexicon, semantic);
            analisaComandos(lexicon, semantic);
        }

        public void analisaEtVariaveis(Lexicon lexicon, Semantic semantic)
        {
            if (!isErrorToken(token) && token.simbolo == "svar")
            {
                updateToken(lexicon);
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    while(!isErrorToken(token) && token.simbolo == "sidentificador")
                    {
                        analisaVariaveis(lexicon, semantic);
                        if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                        {
                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            message = "Ponto e vírgula não encontrado";
                            throw new SyntacticException(token.line, "Ponto e vírgula não encontrado");
                        }
                    }
                }
                else
                {
                    line = token.line;
                    message = "Identificador não encontrado";
                    throw new SyntacticException(token.line, "Identificador não encontrado");
                }
            }
        }

        public void analisaVariaveis(Lexicon lexicon, Semantic semantic)
        {
            flagVar = "";
            do
            {
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    if (semantic.pesquisaDuplicVarTabela(token.lexema) == 0) //n ha duplicidade
                    {
                        flagVar = token.lexema;//
                        semantic.insereTabela(token.lexema, "var", 0, 0); //
                        updateToken(lexicon);
                        if (!isErrorToken(token) && (token.simbolo == "svirgula" || token.simbolo == "sdoispontos"))
                        {
                            if (!isErrorToken(token) && token.simbolo == "svirgula")
                            {
                                updateToken(lexicon);
                                if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                                {
                                    line = token.line;
                                    message = "Dois pontos não encontrado";
                                    throw new SyntacticException(token.line, "Dois pontos não encontrado");
                                }
                            }
                        }
                        else
                        {
                            line = token.line;
                            message = "Vírgula ou dois pontos não encontrado";
                            throw new SyntacticException(token.line, "Vírgula ou dois pontos não encontrado");
                        }
                    }
                    else  //se ha duplicidade
                    {
                        //ERRO SEMANTICO
                    }
                }
                else
                {
                    line = token.line;
                    message = "Identificador não encontrado";
                    throw new SyntacticException(token.line, "Identificador não encontrado");
                }
            } while (token.simbolo != "sdoispontos");
            updateToken(lexicon);
            analisaTipo(lexicon, semantic);
        }

        public void analisaTipo(Lexicon lexicon, Semantic semantic)
        {
            if(!isErrorToken(token) && (token.simbolo != "sinteiro" && token.simbolo != "sbooleano"))
            {
                line = token.line;
                message = "Tipo inteiro ou booleano não encontrado";
                throw new SyntacticException(token.line, "Tipo inteiro ou booleano não encontrado");
            }
            else
            {
                if (token.simbolo == "sinteiro")
                {
                    semantic.colocaTipoTabela("Inteiro", flagVar);//
                }
                else
                {
                    semantic.colocaTipoTabela("Booleano", flagVar);//
                }
            }
            updateToken(lexicon);
        }

        public void analisaComandos(Lexicon lexicon, Semantic semantic)
        {
            if(!isErrorToken(token) && token.simbolo == "sinicio")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon, semantic);
                while(token.simbolo != "sfim")
                {
                    if(!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        updateToken(lexicon);
                        if(!isErrorToken(token) && token.simbolo != "sfim")
                        {
                            analisaComandoSimples(lexicon, semantic);
                        }
                    }
                    else
                    {
                        line = token.line;
                        message = "Ponto e vírgula não encontrado";
                        throw new SyntacticException(token.line, "Ponto e vírgula não encontrado");
                    }
                }
                updateToken(lexicon);
            }
            else
            {
                line = token.line;
                message = "Inicio não encontrado";
                throw new SyntacticException(token.line, "Inicio não encontrado");
            }
        }

        public void analisaComandoSimples(Lexicon lexicon, Semantic semantic)
        {
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
                AnalisaAtribChprocedimento(lexicon, semantic);
            else if (!isErrorToken(token) && token.simbolo == "sse")
                analisaSe(lexicon, semantic);
            else if (!isErrorToken(token) && token.simbolo == "senquanto")
                analisaEnquanto(lexicon, semantic);
            else if (!isErrorToken(token) && token.simbolo == "sleia")
                analisaLeia(lexicon, semantic);
            else if (!isErrorToken(token) && token.simbolo == "sescreva")
                analisaEscreva(lexicon, semantic);
            else
                analisaComandos(lexicon, semantic);
        }

        public void AnalisaAtribChprocedimento(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if(!isErrorToken(token) && token.simbolo == "satribuicao")
            {
                analisaAtribuicao(lexicon, semantic);
            }
            else
            {
                analisaChamadaProcedimento(lexicon);
            }
        }

        public void analisaLeia(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if(!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                updateToken(lexicon);
                if(!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    if (semantic.pesquisaDeclVarTabela(token.lexema) == 1) //se foi declarada
                    {
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                        {
                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            message = "Fecha parentes não encontrado";
                            throw new SyntacticException(token.line, "Fecha parentes não encontrado");
                        }
                    }
                    else
                    {
                        //ERRO SEMANTICO
                    }
                }
                else
                {
                    line = token.line;
                    message = "Identificador não encontrado";
                    throw new SyntacticException(token.line, "Identificador não encontrado");
                }
            }
            else
            {
                line = token.line;
                message = "Abre parentes não encontrado";
                throw new SyntacticException(token.line, "Abre parentes não encontrado");
            }
        }

        public void analisaEscreva(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    if (semantic.pesquisaDeclVarFuncTabela(token.lexema) != 0)
                    {
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                        {
                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            message = "Fecha parentes não encontrado";
                            throw new SyntacticException(token.line, "Fecha parentes não encontrado");
                        }
                    }
                    else
                    {
                        //ERRO
                    }
                }
                else
                {
                    line = token.line;
                    message = "Identificador não encontrado";
                    throw new SyntacticException(token.line, "Identificador não encontrado");
                }
            }
            else
            {
                line = token.line;
                message = "Abre parentes não encontrado";
                throw new SyntacticException(token.line, "Abre parentes não encontrado");
            }
        }

        public void analisaEnquanto(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            analisaExpressao(lexicon, semantic);
            if (!isErrorToken(token) && token.simbolo == "sfaca")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon, semantic);
            }
            else
            {
                line = token.line;
                message = "Faça não encontrado";
                throw new SyntacticException(token.line, "Faça não encontrado");
            }
        }

        public void analisaSe(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            analisaExpressao(lexicon, semantic);
            if (!isErrorToken(token) && token.simbolo == "sentao")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon, semantic);
                if (!isErrorToken(token) && token.simbolo == "ssenao")
                {
                    updateToken(lexicon);                 
                    analisaComandoSimples(lexicon, semantic);
                }
            }
            else
            {
                line = token.line;
                message = "Então não encontrado";
                throw new SyntacticException(token.line, "Então não encontrado");
            }
        }

        public void analisaSubrotinas(Lexicon lexicon, Semantic semantic)
        {
            while (!isErrorToken(token) && (token.simbolo == "sprocedimento" || token.simbolo == "sfuncao"))
            {
                if (!isErrorToken(token) && token.simbolo == "sprocedimento")
                {
                    analisaDeclaracaoProcedimento(lexicon, semantic);      
                }
                else
                {
                    analisaDeclaracaoFuncao(lexicon, semantic);
                }

                if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                {
                    updateToken(lexicon);
                }
                else
                {
                    line = token.line;
                    message = "Ponto e vírgula não encontrado";
                    throw new SyntacticException(token.line, "Ponto e vírgula não encontrado");
                }
            }
        }

        public void analisaDeclaracaoProcedimento(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                if (semantic.pesquisaDeclProcTabela(token.lexema) == 0) //se nao achou, nao existe um proc com esse id
                {
                    semantic.insereTabela(token.lexema, "procedimento", 0, 0); //entao insere 
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        analisaBloco(lexicon, semantic);
                    }
                    else
                    {
                        line = token.line;
                        message = "Identificador não encontrado";
                        throw new SyntacticException(token.line, "Identificador não encontrado");
                    }
                }
                else
                {
                    //ERRO
                }
            }
            else
            {
                line = token.line;
                message = "Ponto e vírgula não encontrado";
                throw new SyntacticException(token.line, "Ponto e vírgula não encontrado");
            }
            semantic.desempilhaTabela(); //TODO desempilhar ate o id do proc 
        }

        public void analisaDeclaracaoFuncao(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                if (semantic.pesquisaDeclFuncTabela(token.lexema) == 0) //senao achar, nao foi declarada ainda
                {
                    semantic.insereTabela(token.lexema, "func", 0, 0); //entao insere
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                    {
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                        {
                            Item item;
                            if(token.simbolo == "sinteiro")
                            {
                                item = semantic.retornaUltimoAdd();
                                item.tipo = string.Concat(item.tipo, "Inteiro");
                            }
                            else
                            {
                                item = semantic.retornaUltimoAdd();
                                item.tipo = string.Concat(item.tipo, "Booleano"); //não sei se isso vai funcionar
                            }
                            updateToken(lexicon);
                            if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                            {
                                analisaBloco(lexicon, semantic);
                            }
                            //else
                            //{
                            //throw new SyntacticException(token.line, "");
                            //}
                        }
                        else
                        {
                            line = token.line;
                            message = "Tipo inteiro ou booleano não encontrado";
                            throw new SyntacticException(token.line, "Tipo inteiro ou booleano não encontrado");
                        }
                    }
                    else
                    {
                        line = token.line;
                        message = "Dois pontos não encontrado";
                        throw new SyntacticException(token.line, "Dois pontos não encontrado");
                    } 
                }
                else
                {
                    //ERRO
                }
            }
            else
            {
                line = token.line;
                message = "Identificador não encontrado";
                throw new SyntacticException(token.line, "Identificador não encontrado");
            }
            semantic.desempilhaTabela();
        }

        public void analisaExpressao(Lexicon lexicon, Semantic semantic)
        {
            analisaExpressaoSimples(lexicon, semantic);

            if (!isErrorToken(token) && (token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdif"))
            {
                updateToken(lexicon);
                analisaExpressaoSimples(lexicon, semantic);
            }
        }

        public void analisaExpressaoSimples(Lexicon lexicon, Semantic semantic)
        {
            if (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos"))
                updateToken(lexicon);
            analisaTermo(lexicon, semantic);

            while (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou"))
            {
                updateToken(lexicon);
                analisaTermo(lexicon, semantic);
            }
        }

        public void analisaTermo(Lexicon lexicon, Semantic semantic)
        {
            analisaFator(lexicon, semantic);

            while (token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                updateToken(lexicon);
                analisaFator(lexicon, semantic);
            }
        }

        public void analisaFator(Lexicon lexicon, Semantic semantic)
        {
            if (token.simbolo == "sidentificador")
            {
                if(semantic.pesquisaTabela(token.lexema) == 1)
                {
                    Item item = semantic.retornaUltimoAdd(); //nao sei se esta correto
                    if (item.tipo == "funcInteiro" || item.tipo == "funcBooleano")
                    {
                        analisaChamadaFuncao(lexicon);
                    }
                    else
                    {
                        updateToken(lexicon);
                    }
                }
                else
                {
                    //ERRO
                }
                
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
                        analisaFator(lexicon, semantic);
                    }
                    else
                    {
                        if (token.simbolo == "sabre_parenteses")
                        {
                            updateToken(lexicon);
                            analisaExpressao(lexicon, semantic);  //analisaExpressao(token); 
                            if (token.simbolo == "sfecha_parenteses")
                            {
                                updateToken(lexicon);
                            }
                            else
                            {
                                line = token.line;
                                message = "Fecha parenteses não encontrado";
                                throw new SyntacticException(token.line, "Fecha parenteses não encontrado");
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
                                line = token.line;
                                message = "Verdadeiro ou falso não encontrado";
                                throw new SyntacticException(token.line, "Verdadeiro ou falso não encontrado");
                            }
                        }
                    }
                }
            }
        }

        public void analisaAtribuicao(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            analisaExpressao(lexicon, semantic);
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
                line = token.line;
                message = "Token não reconhecido";
                throw new LexiconException(token.line);
            }
            return false;
        }

        public Token updateToken(Lexicon lexicon)
        {
            if (lexicon.i >= lexicon.listTokens.Count)
            {
                line = token.line;
                message = "Ponto final não encontrado";
                throw new SyntacticException(token.line, "Ponto final não encontrado");
            }

            token = lexicon.readToken();
            return token;
        }

    }
}