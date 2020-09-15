using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;

namespace LPD_Compiler.LexiconHandler
{
    public class Lexicon
    {
        public List<Token> listTokens = new List<Token>();
        char character;

        public void lexicalAnalyser(LpdFile lpdfile)
        {
            character = lpdfile.getCharacter();
            int length = lpdfile.content.Length;
            Token token;

            while (!lpdfile.isEndOfFile())
            {
                while((character == '{' || character == '/' || character == ' ' || character == '\t') && !lpdfile.isEndOfFile())
                {
                    if(character == '{')
                    {
                        while(character != '}' && !lpdfile.isEndOfFile())
                        {
                            character = lpdfile.getCharacter();
                        }
                        character = lpdfile.getCharacter();
                    }
                    if(character == '/')
                    {
                        character = lpdfile.getCharacter();
                        if(character == '*')
                        {
                            int starFlag = 0;
                            while((character != '/' || starFlag != 1) && !lpdfile.isEndOfFile())
                            {
                                character = lpdfile.getCharacter();
                                if (character == '*')
                                {
                                    starFlag = 1;
                                    if(!lpdfile.isEndOfFile()) character = lpdfile.getCharacter();
                                    if(character != '/') starFlag = 0;
                                }                         
                            }
                            character = lpdfile.getCharacter();
                        }
                        else
                        {
                            throw new LexiconException(lpdfile.currentLine + 1);
                        }                       
                    }
                    while ((character == ' ' || character == '\t') && !lpdfile.isEndOfFile())
                    {
                        character = lpdfile.getCharacter();
                    }
                }
                if(!lpdfile.isEndOfFile())
                {
                    try
                    {
                        token = getToken(lpdfile);
                        listTokens.Add(token);
                    }
                    catch (LexiconException ex)
                    {
                        MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }                  
                }
            }
        }

        public Token getToken(LpdFile lpdFile)
        {

           if(Char.IsNumber(character) == true)
           {
                return trataDigito(lpdFile);
            }
            else
           {
                if(Char.IsLetter(character) == true)
                {
                    return trataIdPalavraReservada(lpdFile);
                }
                else
                {
                    if(character == ':')
                    {
                        return trataAtribuicao(lpdFile);
                    }
                    else
                    {
                        if(character == '+' || character == '-' || character == '*')
                        {
                            return trataOperadorAritmetico(lpdFile);
                        }
                        else
                        {
                            if(character == '<' || character == '>' || character == '=' || character == '!')
                            {
                                return trataOperadorRelacional(lpdFile);
                            }
                            else
                            {
                                if (character == ';' || character == '(' || character == ')' || character == '.' || character == ',')
                                {
                                    return trataPontuacao(lpdFile);
                                }
                                else
                                {
                                    throw new LexiconException(lpdFile.currentLine+1); 
                                }
                            }
                        }
                    }
                }
           }

        }

        // Trata X

        public Token trataDigito(LpdFile lpdFile)
        {
            Token token = new Token();
            string num;
            num = String.Copy(Char.ToString(character));
            
            character = lpdFile.getCharacter();
            while(Char.IsNumber(character))
            {
                num = num + character;
                character = lpdFile.getCharacter();
            }
            token.simbolo = "snumero";
            token.lexema = num;

            return token;
        }

        public Token trataIdPalavraReservada(LpdFile lpdFile)
        {
            Token token = new Token();
            string id;
            id = String.Copy(Char.ToString(character));
            int line = lpdFile.currentLine;

            character = lpdFile.getCharacter();

            while (Char.IsLetter(character) || Char.IsNumber(character) || character == '_')
            {
                if (line == lpdFile.currentLine)
                {
                    id = id + character;
                    character = lpdFile.getCharacter();
                }
                else
                {
                    break;
                }
            }

            token.lexema = id;
            switch (id)
            {
                case "programa":
                    token.simbolo = "sprograma";
                    break;
                case "se":
                    token.simbolo = "sse";
                    break;
                case "senao":
                    token.simbolo = "ssenao";
                    break;
                case "enquanto":
                    token.simbolo = "senquanto";
                    break;
                case "faca":
                    token.simbolo = "sfaca";
                    break;
                case "inicio":
                    token.simbolo = "sinício";
                    break;
                case "fim":
                    token.simbolo = "sfim";
                    break;
                case "escreva":
                    token.simbolo = "sescreva";
                    break;
                case "leia":
                    token.simbolo = "sleia";
                    break;
                case "var":
                    token.simbolo = "svar";
                    break;
                case "inteiro":
                    token.simbolo = "sinteiro";
                    break;
                case "booleano":
                    token.simbolo = "sbooleano";
                    break;
                case "verdadeiro":
                    token.simbolo = "sverdadeiro";
                    break;
                case "falso":
                    token.simbolo = "sfalso";
                    break;
                case "procedimento":
                    token.simbolo = "sprocedimento";
                    break;
                case "funcao":
                    token.simbolo = "sfuncao";
                    break;
                case "div":
                    token.simbolo = "sdiv";
                    break;
                case "e":
                    token.simbolo = "se";
                    break;
                case "ou":
                    token.simbolo = "sou";
                    break;
                case "nao":
                    token.simbolo = "snao";
                    break;
                default:
                    token.simbolo = "sidentificador";
                    break;
            }

            return token;
        }

        public Token trataAtribuicao(LpdFile lpdFile)
        {
            Token token = new Token();
            string atribuicao;
            atribuicao = String.Copy(Char.ToString(character));

            character = lpdFile.getCharacter();

            if (character == '=')
            {
                atribuicao = atribuicao + character;
                token.simbolo = "satribuicao";
                character = lpdFile.getCharacter();
            }
            else
            {
                token.simbolo = "sdoispontos";
            }
            token.lexema = atribuicao;

            return token;
        }

        public Token trataOperadorAritmetico(LpdFile lpdFile)
        {
            Token token = new Token();

            if (character == '+')
                token.simbolo = "smais";
            else if (character == '-')
                token.simbolo = "smenos";
            else if (character == '*')
                token.simbolo = "smult";

            token.lexema = character.ToString();
            character = lpdFile.getCharacter();

            return token;
        }

        public Token trataOperadorRelacional(LpdFile lpdFile)
        {
            Token token = new Token();
            string operador = String.Copy(Char.ToString(character));

            character = lpdFile.getCharacter();

            if(character == '=')
            {
                operador = operador + character;
                character = lpdFile.getCharacter();
            }

            token.lexema = operador;
            switch(operador)
            {
                case "<":
                    token.simbolo = "smenor";
                    break;
                case ">":
                    token.simbolo = "smaior";
                    break;
                case "=":
                    token.simbolo = "sig";
                    break;
                case "!=":
                    token.simbolo = "sdif";
                    break;
                case "<=":
                    token.simbolo = "smenorig";
                    break;
                case ">=":
                    token.simbolo = "smaiorig";
                    break;
                default:
                    throw new LexiconException(lpdFile.currentLine + 1);          
            }

            return token;
        }

        public Token trataPontuacao(LpdFile lpdFile)
        {
            Token token = new Token();

            if (character == ';')
                token.simbolo = "sponto_vírgula";
            else if (character == ',')
                token.simbolo = "svírgula";
            else if (character == '(')
                token.simbolo = "sabre_parênteses";
            else if (character == ')')
                token.simbolo = "sfecha_parênteses";
            else if (character == '.')
                token.simbolo = "sponto";

            token.lexema = character.ToString();
            character = lpdFile.getCharacter();

            return token;
        }
    }
}
