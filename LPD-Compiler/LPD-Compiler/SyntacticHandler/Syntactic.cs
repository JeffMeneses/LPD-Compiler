﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.SyntacticHandler;
using LPD_Compiler.SemanticHandler;
using LPD_Compiler.CodeGeneratorHandler;

namespace LPD_Compiler.SyntacticHandler
{
    public class Syntactic
    {
        public Postfix postfix = new Postfix();
        public List<string> expression = new List<string>();
        public int parenthesisCount = 0;
        public Token token;
        public string message = "", flagVar = "";
        public int line = 0;

        // semantic
        public Semantic semantic;
        public bool isReturnDeclared = false;
        public int ReturnDeclaredCount = 0;
        public Stack<string> FunctionSatck = new Stack<string>();
        public int ReturnExpectedCount = 0;

        // codeGenerator
        public int rotulo;
        public CodeGenerator codeGenerator = new CodeGenerator();
        public int quantDecleredVars;
        public int quantAllocatedVars = 0;

        public Stack<int> localDecleredVars = new Stack<int>();
        public Stack<int> localAllocatedVars = new Stack<int>();

        public int dataStackIndex = 0;
        public string atribVarName;

        public List<string> syntacticAnalyser(Lexicon lexicon, Semantic semantic)
        {
            this.semantic = semantic;
            rotulo = 1;
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sprograma")
            {
                codeGenerator.generate("", "START", "", "");
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    semantic.insereTabela(token.lexema, "nomedeprograma", 0, 0); //ver os 2 ultimos parametros
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        codeGenerator.generate("", "ALLOC", quantAllocatedVars.ToString(), "1");
                        quantAllocatedVars += 1;

                        analisaBloco(lexicon, semantic);
                        if (!isErrorToken(token) && token.simbolo == "sponto")
                        {
                            quantAllocatedVars -= 1;
                            codeGenerator.generate("", "DALLOC", quantAllocatedVars.ToString(), "1");

                            codeGenerator.generate("", "HLT", "", "");
                            Console.WriteLine("Deu certo");
                            //codeGenerator.test();
                            return codeGenerator.outputCode;
                            // se acabou arquivo ou é comentário
                            // return Sucesso
                        }
                        else
                        {
                            line = token.line;
                            //message = "Ponto final não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado '.'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                    else
                    {
                        line = token.line;
                        //message = "Ponto e vírgula não encontrado";
                        message = "'" + token.lexema + "'" + " encontrado, mas era esperado ';'";
                        throw new SyntacticException(token.line, message);
                    }
                }
                else
                {
                    line = token.line;
                    //message = "Identificador não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                    throw new SyntacticException(token.line, message);
                }
            }
            else
            {
                line = token.line;
                //message = "Palavra reservada 'programa' não encontrada";
                message = "'"+token.lexema + "'" + " encontrado, mas era esperado 'programa'";
                throw new SyntacticException(token.line, message);
            }
        }

        public void analisaBloco(Lexicon lexicon, Semantic semantic)
        {
            updateToken(lexicon);
            analisaEtVariaveis(lexicon, semantic);

            if (quantDecleredVars > 0)
            {
                codeGenerator.generate("", "ALLOC", quantAllocatedVars.ToString(), quantDecleredVars.ToString());
                quantAllocatedVars += quantDecleredVars;
            }
                localDecleredVars.Push(quantDecleredVars);
                localAllocatedVars.Push(quantAllocatedVars);

            analisaSubrotinas(lexicon, semantic);
            analisaComandos(lexicon, semantic);

            if (localDecleredVars.Count > 0)
            {
                quantDecleredVars = localDecleredVars.Pop();
                quantAllocatedVars = localAllocatedVars.Pop();

                quantAllocatedVars -= quantDecleredVars;
                if(quantDecleredVars > 0) codeGenerator.generate("", "DALLOC", quantAllocatedVars.ToString(), quantDecleredVars.ToString());
            }
            
        }

        public void analisaEtVariaveis(Lexicon lexicon, Semantic semantic)
        {
            quantDecleredVars = 0;

            if (!isErrorToken(token) && token.simbolo == "svar")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    while (!isErrorToken(token) && token.simbolo == "sidentificador")
                    {
                        analisaVariaveis(lexicon, semantic);
                        if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                        {
                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            //message = "Ponto e vírgula não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado ';'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                }
                else
                {
                    line = token.line;
                    //message = "Identificador não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                    throw new SyntacticException(token.line, message);
                }
            }
        }

        public void analisaVariaveis(Lexicon lexicon, Semantic semantic)
        {
            flagVar = "";
            int cont = 0;
            do
            {
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    if (semantic.pesquisaDuplicVarTabela(token.lexema) == 0) //n ha duplicidade
                    {
                        if (cont == 0)//
                            flagVar = token.lexema;//
                        semantic.insereTabela(token.lexema, "var", 0, quantAllocatedVars + quantDecleredVars); //
                        quantDecleredVars++;
                        updateToken(lexicon);
                        if (!isErrorToken(token) && (token.simbolo == "svirgula" || token.simbolo == "sdoispontos"))
                        {
                            if (!isErrorToken(token) && token.simbolo == "svirgula")
                            {
                                updateToken(lexicon);
                                if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                                {
                                    line = token.line;
                                    //message = "Dois pontos não encontrado";
                                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado ':'";
                                    throw new SyntacticException(token.line, message);
                                }
                            }
                        }
                        else
                        {
                            line = token.line;
                            //message = "Vírgula ou dois pontos não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado ', ou :'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                    else  //se ha duplicidade
                    {
                        line = token.line;
                        message = "Variavel com identificador duplicado";
                        throw new SemanticException(token.line, "Variavel com identificador duplicado");
                    }
                }
                else
                {
                    line = token.line;
                    //message = "Identificador não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                    throw new SyntacticException(token.line, message);
                }
                cont++;//
            } while (token.simbolo != "sdoispontos");
            updateToken(lexicon);
            analisaTipo(lexicon, semantic);
        }

        public void analisaTipo(Lexicon lexicon, Semantic semantic)
        {
            if (!isErrorToken(token) && (token.simbolo != "sinteiro" && token.simbolo != "sbooleano"))
            {
                line = token.line;
                //message = "Tipo inteiro ou booleano não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'tipo inteiro ou booleano'";
                throw new SyntacticException(token.line, message);
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
            if (!isErrorToken(token) && token.simbolo == "sinicio")
            {
                updateToken(lexicon);
                analisaComandoSimples(lexicon, semantic);
                while (token.simbolo != "sfim")
                {
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo != "sfim")
                        {
                            analisaComandoSimples(lexicon, semantic);
                        }
                    }
                    else
                    {
                        line = token.line;
                        //message = "Ponto e vírgula não encontrado";
                        message = "'" + token.lexema + "'" + " encontrado, mas era esperado ';'";
                        throw new SyntacticException(token.line, message);
                    }
                }
                updateToken(lexicon);
            }
            else
            {
                line = token.line;
                //message = "Inicio não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'inicio'";
                throw new SyntacticException(token.line, message);
            }
        }

        public void analisaComandoSimples(Lexicon lexicon, Semantic semantic)
        {
            isReturnDeclared = false;

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
            string flagFuncName;
            atribVarName = token.lexema;

            if (semantic.pesquisaProcTabela(token.lexema) != 0)
            {
                analisaChamadaProcedimento(lexicon);
            }
            else
            {
                int ret = semantic.validaAtribuicao(token.lexema);
                if (ret != -1)
                {
                    flagFuncName = token.lexema;
                    updateToken(lexicon);

                    if (FunctionSatck.Count > 0 && flagFuncName.Equals(FunctionSatck.Peek())) // return de função
                    {
                        analisaAtribuicao(lexicon, semantic);
                        if (semantic.validaCompatibilidadeTipo(postfix.convertedExpression) != ret)
                        {
                            line = token.line;
                            message = "Tipos nao compativeis";
                            throw new SemanticException(token.line, "Tipos nao compativeis");
                        }
                        isReturnDeclared = true;
                        ReturnDeclaredCount++;
                        flagFuncName = "";
                    }

                    else if (!isErrorToken(token) && token.simbolo == "satribuicao")
                    {
                        analisaAtribuicao(lexicon, semantic);
                        if (semantic.validaCompatibilidadeTipo(postfix.convertedExpression) != ret)
                        {
                            line = token.line;
                            message = "Tipos nao compativeis";
                            throw new SemanticException(token.line, "Tipos nao compativeis");
                        }
                    }
                    else
                    {
                        if (semantic.pesquisaFuncTabela(flagFuncName) != 0)
                        {
                            line = token.line;
                            message = "Chamada de função inválida";
                            throw new SemanticException(token.line, "Chamada de função inválida");
                        }
                    }
                }
                else
                {
                    line = token.line;
                    message = "Formato da Atribuicao ou da chamada de Procedimento incorreto";
                    throw new SemanticException(token.line, "Formato da Atribuicao ou da chamada de Procedimento incorreto");
                }
            }
        }

        public void analisaLeia(Lexicon lexicon, Semantic semantic)
        {
            Item itemTable;
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sabre_parenteses")
            {
                updateToken(lexicon);
                if (!isErrorToken(token) && token.simbolo == "sidentificador")
                {
                    if (semantic.pesquisaDeclVarTabela(token.lexema) == 1 && semantic.validaEscrevaELeia(token.lexema) != 0) //se foi declarada
                    {
                        itemTable = semantic.getPesquisaTabela(token.lexema);
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                        {
                            codeGenerator.generate("", "RD", "", "");
                            codeGenerator.generate("", "STR", itemTable.rotulo.ToString(), "");
                            dataStackIndex++;

                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            //message = "Fecha parentes não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado ')'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                    else
                    {
                        line = token.line;
                        message = "Formato Leia incorreto";
                        throw new SemanticException(token.line, "Formato Leia incorreto");
                    }
                }
                else
                {
                    line = token.line;
                    //message = "Identificador não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                    throw new SyntacticException(token.line, message);
                }
            }
            else
            {
                line = token.line;
                //message = "Abre parentes não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado '('";
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
                    if (semantic.pesquisaDeclVarFuncTabela(token.lexema) != 0 && semantic.validaEscrevaELeia(token.lexema) != 0)
                    {
                        string readVarName = token.lexema;
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sfecha_parenteses")
                        {
                            if (semantic.getPesquisaTabela(readVarName).tipo == "funcInteiro" || semantic.getPesquisaTabela(readVarName).tipo == "funcBooleano")
                            {
                                codeGenerator.generate("", "CALL", semantic.getPesquisaTabela(readVarName).rotulo.ToString(), "");
                                codeGenerator.generate("", "LDV", "0", "");
                            }
                            else
                                codeGenerator.generate("", "LDV", semantic.getPesquisaTabela(readVarName).rotulo.ToString(), "");
                            codeGenerator.generate("", "PRN", "", "");

                            updateToken(lexicon);
                        }
                        else
                        {
                            line = token.line;
                            //message = "Fecha parentes não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado ')'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                    else
                    {
                        line = token.line;
                        message = "Formato Escreva incorreto";
                        throw new SemanticException(token.line, "Formato Escreva incorreto");
                    }
                }
                else
                {
                    line = token.line;
                    //message = "Identificador não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                    throw new SyntacticException(token.line, message);
                }
            }
            else
            {
                line = token.line;
                //message = "Abre parentes não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado '('";
                throw new SyntacticException(token.line, message);
            }
        }

        public void analisaEnquanto(Lexicon lexicon, Semantic semantic)
        {
            int auxrot1, auxrot2;

            auxrot1 = rotulo;
            codeGenerator.generate(rotulo.ToString(), "NULL", "", "");
            rotulo++;

            updateToken(lexicon);
            postfix.clearExpression();
            analisaExpressao(lexicon, semantic);
            if (semantic.validaCompatibilidadeTipo(postfix.convertedExpression) == 0)
            {
                if (!isErrorToken(token) && token.simbolo == "sfaca")
                {
                    auxrot2 = rotulo;
                    codeGenerator.generate("", "JMPF", rotulo.ToString(), "");
                    rotulo++;

                    //bool isReturnWhile = false;
                    // TODO: CHECK RETURN ON WHILE

                    updateToken(lexicon);
                    analisaComandoSimples(lexicon, semantic);

                    codeGenerator.generate("", "JMP", auxrot1.ToString(), "");
                    codeGenerator.generate(auxrot2.ToString(), "NULL", "", "");

                    /*if (isReturnDeclared && (ReturnExpectedCount > 0))
                    {
                        if(isReturnWhile)
                        {
                            isReturnDeclared = false;
                        }
                    }*/

                }
                else
                {
                    line = token.line;
                    //message = "Faça não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'faca'";
                    throw new SyntacticException(token.line, message);
                }
            }
            else
            {
                line = token.line-1;
                message = "Expressao do 'enquanto' nao booleana";
                throw new SemanticException(token.line, "Expressao do 'enquanto' nao booleana");
            }
        }

        public void analisaSe(Lexicon lexicon, Semantic semantic)
        {
            int auxrot1, auxrot2;

            updateToken(lexicon);
            postfix.clearExpression();
            analisaExpressao(lexicon, semantic);
            if (semantic.validaCompatibilidadeTipo(postfix.convertedExpression) == 0)
            {
                if (!isErrorToken(token) && token.simbolo == "sentao")
                {
                    auxrot1 = rotulo;
                    codeGenerator.generate("", "JMPF", auxrot1.ToString(), "");
                    rotulo++;

                    bool ifReturnDeclared = false, elseReturnDeclared = false;

                    updateToken(lexicon);
                    analisaComandoSimples(lexicon, semantic);

                    ifReturnDeclared = isReturnDeclared;

                    auxrot2 = rotulo;
                    codeGenerator.generate("", "JMP", auxrot2.ToString(), "");
                    rotulo++;

                    codeGenerator.generate(auxrot1.ToString(), "NULL", "", "");

                    if (!isErrorToken(token) && token.simbolo == "ssenao")
                    {
                        updateToken(lexicon);
                        analisaComandoSimples(lexicon, semantic);

                        elseReturnDeclared = isReturnDeclared;
                    }
                    else
                    {
                        elseReturnDeclared = false;
                    }
                    isReturnDeclared = ifReturnDeclared && elseReturnDeclared;

                    codeGenerator.generate(auxrot2.ToString(), "NULL", "", "");
                }
                else
                {
                    line = token.line;
                    //message = "Então não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'entao'";
                    throw new SyntacticException(token.line, message);
                }
            }
            else
            {
                line = token.line-1;
                message = "Expressao do 'se' nao booleana";
                throw new SemanticException(token.line, "Expressao do 'se' nao booleana");
            }
        }

        public void analisaSubrotinas(Lexicon lexicon, Semantic semantic)
        {
            int auxrot = rotulo, flag = 0;

            if (!isErrorToken(token) && (token.simbolo == "sprocedimento" || token.simbolo == "sfuncao"))
            {
                codeGenerator.generate("", "JMP", rotulo.ToString(), "");
                rotulo++;
                flag = 1;
            }

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
                    codeGenerator.generate("", "RETURN", "", "");
                    updateToken(lexicon);
                }
                else
                {
                    line = token.line;
                    //message = "Ponto e vírgula não encontrado";
                    message = "'" + token.lexema + "'" + " encontrado, mas era esperado ';'";
                    throw new SyntacticException(token.line, message);
                }
            }
            if(flag == 1)
            {
                codeGenerator.generate(auxrot.ToString(), "NULL", "", "");
            }
        }

        public void analisaDeclaracaoProcedimento(Lexicon lexicon, Semantic semantic)
        {
            string aux = "";
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                if (semantic.pesquisaDeclProcTabela(token.lexema) == 0) //se nao achou, nao existe um proc com esse id
                {
                    aux = token.lexema;
                    semantic.insereTabela(token.lexema, "procedimento", 0, rotulo); //entao insere
                    codeGenerator.generate(rotulo.ToString(), "NULL", "", "");
                    rotulo++;
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sponto_virgula")
                    {
                        analisaBloco(lexicon, semantic);
                    }
                    else
                    {
                        line = token.line;
                        //message = "Identificador não encontrado";
                        message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                        throw new SyntacticException(token.line, message);
                    }
                }
                else
                {
                    line = token.line;
                    message = "Estrutura com identificador duplicado";
                    throw new SemanticException(token.line, "Estrutura com identificador duplicado");
                }
            }
            else
            {
                line = token.line;
                //message = "Ponto e vírgula não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado ';'";
                throw new SyntacticException(token.line, message);
            }
            semantic.desempilhaTabela(aux);
        }

        public void analisaDeclaracaoFuncao(Lexicon lexicon, Semantic semantic)
        {
            string aux = token.lexema;
            updateToken(lexicon);
            if (!isErrorToken(token) && token.simbolo == "sidentificador")
            {
                FunctionSatck.Push(token.lexema);
                if (semantic.pesquisaDeclFuncTabela(token.lexema) == 0) //senao achar, nao foi declarada ainda
                {
                    aux = token.lexema;
                    semantic.insereTabela(token.lexema, "func", 0, rotulo); //entao insere
                    codeGenerator.generate(rotulo.ToString(), "NULL", "", "");
                    rotulo++;
                    updateToken(lexicon);
                    if (!isErrorToken(token) && token.simbolo == "sdoispontos")
                    {
                        updateToken(lexicon);
                        if (!isErrorToken(token) && token.simbolo == "sinteiro" || token.simbolo == "sbooleano")
                        {
                            Item item;
                            if (token.simbolo == "sinteiro")
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
                                ReturnExpectedCount++;
                                analisaBloco(lexicon, semantic);
                                ReturnExpectedCount--;

                                // WARNING
                                if(!isReturnDeclared)
                                {
                                    if(ReturnDeclaredCount > 0)
                                    {
                                        line = token.line;
                                        message = "A última linha da função deve ser de retorno";
                                        throw new SemanticException(token.line, "A última linha da função deve ser de retorno");
                                    }
                                    else
                                    {
                                        line = token.line;
                                        message = "A função deve ter um retorno";
                                        throw new SemanticException(token.line, "A função deve ter um retorno");
                                    }
                                }
                                ReturnDeclaredCount = 0; //TESTE
                                FunctionSatck.Pop();
                            }
                            //else
                            //{
                            //throw new SyntacticException(token.line, "");
                            //}
                        }
                        else
                        {
                            line = token.line;
                            //message = "Tipo inteiro ou booleano não encontrado";
                            message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'tipo inteiro ou booleano'";
                            throw new SyntacticException(token.line, message);
                        }
                    }
                    else
                    {
                        line = token.line;
                        //message = "Dois pontos não encontrado";
                        message = "'" + token.lexema + "'" + " encontrado, mas era esperado ':'";
                        throw new SyntacticException(token.line, message);
                    }
                }
                else
                {
                    line = token.line;
                    message = "Estrutura com identificador duplicado";
                    throw new SemanticException(token.line, "Estrutura com identificador duplicado");
                }
            }
            else
            {
                line = token.line;
                //message = "Identificador não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'identificador'";
                throw new SyntacticException(token.line, message);
            }
            semantic.desempilhaTabela(aux);
        }

        public void analisaExpressao(Lexicon lexicon, Semantic semantic)
        {
            if (parenthesisCount == 0)
                expression.Clear();
            analisaExpressaoSimples(lexicon, semantic);

            if (!isErrorToken(token) && (token.simbolo == "smaior" || token.simbolo == "smaiorig" || token.simbolo == "sig" || token.simbolo == "smenor" || token.simbolo == "smenorig" || token.simbolo == "sdif"))
            {
                expression.Add(token.lexema);
                updateToken(lexicon);
                analisaExpressaoSimples(lexicon, semantic);
            }

            if (parenthesisCount == 0)
            {
                postfix = new Postfix(expression);
                postfix.convertExpression();

                codeGenerator.generateExpression(postfix.convertedExpression, semantic);
            }
        }

        public void analisaExpressaoSimples(Lexicon lexicon, Semantic semantic)
        {
            if (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos"))
            {
                expression.Add(token.lexema);
                updateToken(lexicon);
            }
            analisaTermo(lexicon, semantic);

            while (!isErrorToken(token) && (token.simbolo == "smais" || token.simbolo == "smenos" || token.simbolo == "sou"))
            {
                expression.Add(token.lexema);
                updateToken(lexicon);
                analisaTermo(lexicon, semantic);
            }
        }

        public void analisaTermo(Lexicon lexicon, Semantic semantic)
        {
            analisaFator(lexicon, semantic);

            while (token.simbolo == "smult" || token.simbolo == "sdiv" || token.simbolo == "se")
            {
                expression.Add(token.lexema);
                updateToken(lexicon);
                analisaFator(lexicon, semantic);
            }
        }

        public void analisaFator(Lexicon lexicon, Semantic semantic)
        {
            if (token.simbolo == "sidentificador")
            {
                if (semantic.pesquisaTabela(token.lexema) == 1)
                {
                    Item item = semantic.retornaUltimoAdd(); //nao sei se esta correto
                    if ((item.tipo == "funcInteiro" || item.tipo == "funcBooleano") && semantic.pesquisaDeclVarFuncTabela(token.lexema) == 2)
                    {
                        analisaChamadaFuncao(lexicon);
                    }
                    else
                    {
                        expression.Add(token.lexema);
                        updateToken(lexicon);
                    }
                }
                else
                {
                    line = token.line;
                    message = "Estrutura com identificador nao declarada";
                    throw new SemanticException(token.line, "Estrutura com identificador nao declarada");
                }

            }
            else
            {
                if (token.simbolo == "snumero")
                {
                    expression.Add(token.lexema);
                    updateToken(lexicon);
                }
                else
                {
                    if (token.simbolo == "snao")
                    {
                        expression.Add(token.lexema);
                        updateToken(lexicon);
                        analisaFator(lexicon, semantic);
                    }
                    else
                    {
                        if (token.simbolo == "sabre_parenteses")
                        {
                            expression.Add(token.lexema);
                            parenthesisCount++;
                            updateToken(lexicon);
                            analisaExpressao(lexicon, semantic);  //analisaExpressao(token); 
                            if (token.simbolo == "sfecha_parenteses")
                            {
                                expression.Add(token.lexema);
                                parenthesisCount--;
                                updateToken(lexicon);
                            }
                            else
                            {
                                line = token.line;
                                //message = "Fecha parenteses não encontrado";
                                message = "'" + token.lexema + "'" + " encontrado, mas era esperado ')'";
                                throw new SyntacticException(token.line, message);
                            }
                        }
                        else
                        {
                            if (token.simbolo == "sverdadeiro" || token.simbolo == "sfalso")
                            {
                                expression.Add(token.lexema);
                                updateToken(lexicon);
                            }
                            else
                            {
                                line = token.line;
                                //message = "Verdadeiro ou falso não encontrado";
                                message = "'" + token.lexema + "'" + " encontrado, mas era esperado 'expressao'";
                                throw new SyntacticException(token.line, message);
                            }
                        }
                    }
                }
            }
        }

        public void analisaAtribuicao(Lexicon lexicon, Semantic semantic)
        {
            Item itemTable;

            updateToken(lexicon);
            postfix.clearExpression();
            analisaExpressao(lexicon, semantic);

            itemTable = semantic.getPesquisaTabela(atribVarName);

            if (itemTable.tipo == "funcInteiro" || itemTable.tipo == "funcBooleano")
            {
                codeGenerator.generate("", "STR", "0", "");
            }
            else
            {
                codeGenerator.generate("", "STR", itemTable.rotulo.ToString(), "");
            }
        }

        public void analisaChamadaProcedimento(Lexicon lexicon)
        {
            codeGenerator.generate("", "CALL", semantic.getPesquisaTabela(token.lexema).rotulo.ToString(), "");
            updateToken(lexicon);
        }

        public void analisaChamadaFuncao(Lexicon lexicon)
        {
            expression.Add(token.lexema);
            updateToken(lexicon);
        }

        public bool isErrorToken(Token token)
        {
            if (token.simbolo == "serro")
            {
                line = token.line;
                message = "Token não reconhecido";
                throw new LexiconException(token.line, "Token não reconhecido");
            }
            else if (token.simbolo == "serro_comentario")
            {
                line = token.line;
                message = "Token não reconhecido";
                throw new LexiconException(token.line, "Comentário iniciado mas não finalizado");
            }
            return false;
        }

        public Token updateToken(Lexicon lexicon)
        {
            if (lexicon.i >= lexicon.listTokens.Count)
            {
                line = token.line;
                //message = "Ponto final não encontrado";
                message = "'" + token.lexema + "'" + " encontrado, mas era esperado '.'";
                throw new SyntacticException(token.line, message);
            }

            token = lexicon.readToken();
            return token;
        }

    }
}