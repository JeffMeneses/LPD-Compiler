using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;

namespace LPD_Compiler.LexiconHandler
{
    class Lexicon
    {
        public List<Token> listTokens = new List<Token>();

        public void lexicalAnalyser(LpdFile lpdfile)
        {
            char character = lpdfile.getCharacter();
            int length = lpdfile.content.Length;
            Token token;

            while (lpdfile.i < length)
            {
                while((character == '{' || character == ' ') && lpdfile.i < length)
                {
                    if(character == '{')
                    {
                        while(character != '}' && lpdfile.i < length)
                        {
                            character = lpdfile.getCharacter();
                        }
                        character = lpdfile.getCharacter();
                    }
                    while (character == ' ' && lpdfile.i < length)
                    {
                        character = lpdfile.getCharacter();
                    }
                }
                if(lpdfile.i < length)
                {
                    token = getToken(character, lpdfile);
                    listTokens.Add(token);
                }
            }
        }

        public Token getToken(char character, LpdFile lpdFile)
        {

           if(Char.IsNumber(character) == true)
           {
                return trataDigito(character, lpdFile);
            }
            else
           {
                if(Char.IsLetter(character) == true)
                {
                    return trataIdPalavraReservada(character, lpdFile);
                }
                else
                {
                    if(character == ':')
                    {
                        return trataAtribuicao(character, lpdFile);
                    }
                    else
                    {
                        if(character == '+' || character == '-' || character == '*')
                        {
                            //return trataOperadorAritmetico(character, lpdFile);
                        }
                        else
                        {
                            if(character == '<' || character == '>' || character == '=')
                            {
                                //return trataOperadorRelacional(character, lpdFile);
                            }
                            else
                            {
                                if (character == ';' || character == '(' || character == ')' || character == '.')
                                {
                                    //return trataPontuacao(character, lpdFile);
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

            return null;
        }

        // Trata X

        public Token trataDigito(char character, LpdFile lpdFile)
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

        public Token trataIdPalavraReservada(char character, LpdFile lpdFile)
        {
            Token token = new Token();
            string id;
            id = String.Copy(Char.ToString(character));

            character = lpdFile.getCharacter();

            while (Char.IsLetter(character))
            {
                id = id + character;
                character = lpdFile.getCharacter();
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
                    token.simbolo = "sinicio";
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

        public Token trataAtribuicao(char character, LpdFile lpdFile)
        {
            Token token = new Token();
            string atribuicao;
            atribuicao = String.Copy(Char.ToString(character));

            character = lpdFile.getCharacter();

            if (character == '=')
            {
                atribuicao = String.Copy(Char.ToString(character));
                token.simbolo = "satribuicao";
            }
            else
            {
                token.simbolo = "sdoispontos";
            }
            token.lexema = atribuicao;

            return token;
        }

        // Test function
        public void showListToken()
        {

        }
    }
}
