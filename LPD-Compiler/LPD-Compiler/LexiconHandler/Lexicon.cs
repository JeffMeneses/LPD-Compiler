using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPD_Compiler.LexiconHandler
{
    class Lexicon
    {
        public List<Token> listTokens = new List<Token>();

        public void lexicalAnalyser(List<String> lines)
        {

        }

        public void ler()
        {

        }

        public void getToken(char caracter)
        {

           if(Char.IsNumber(caracter) == true)
           {
                //trataDigito(caracter);
           }
           else
           {
                if(Char.IsLetter(caracter) == true)
                {
                    //trataIdPalavraReservada(caracter);
                }
                else
                {
                    if(caracter == ':')
                    {
                        //trataAtribuição(caracter);
                    }
                    else
                    {
                        if(caracter == '+' || caracter == '-' || caracter == '*')
                        {
                            //trataOperadorAritmetico(caracter);
                        }
                        else
                        {
                            if(caracter == '<' || caracter == '>' || caracter == '=')
                            {
                                //trataOperadorRelacional(caracter);
                            }
                            else
                            {
                                if (caracter == ';' || caracter == '(' || caracter == ')' || caracter == '.')
                                {
                                    //trataPontuacao(caracter);
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

        // Trata X

        public void trataDigito(char caracter)
        {
            string num;
            num = String.Copy(Char.ToString(caracter));
            
            //ler(caracter);
            while(Char.IsNumber(caracter))
            {
                num = string.Concat(caracter);
                //ler(caracter);
            }
            listTokens.Add(new Token("snumero", num));
        }

        public void trataIdPalavraReservada(char caracter)
        {
            string id;
            id = String.Copy(Char.ToString(caracter));

            //ler(caracter);

            while (Char.IsLetter(caracter))
            {
                id = string.Concat(caracter);
                //ler(caracter);
            }

            switch (id)
            {
                case "programa":
                    listTokens.Add(new Token("sprograma", id));
                    break;
                case "se":
                    listTokens.Add(new Token("sse", id));
                    break;
                case "senao":
                    listTokens.Add(new Token("ssenao", id));
                    break;
                case "enquanto":
                    listTokens.Add(new Token("senquanto", id));
                    break;
                case "faca":
                    listTokens.Add(new Token("sfaca", id));
                    break;
                case "inicio":
                    listTokens.Add(new Token("sinicio", id));
                    break;
                case "fim":
                    listTokens.Add(new Token("sfim", id));
                    break;
                case "escreva":
                    listTokens.Add(new Token("sescreva", id));
                    break;
                case "leia":
                    listTokens.Add(new Token("sleia", id));
                    break;
                case "var":
                    listTokens.Add(new Token("svar", id));
                    break;
                case "inteiro":
                    listTokens.Add(new Token("sinteiro", id));
                    break;
                case "booleano":
                    listTokens.Add(new Token("sbooleano", id));
                    break;
                case "verdadeiro":
                    listTokens.Add(new Token("sverdadeiro", id));
                    break;
                case "falso":
                    listTokens.Add(new Token("sfalso", id));
                    break;
                case "procedimento":
                    listTokens.Add(new Token("sprocedimento", id));
                    break;
                case "funcao":
                    listTokens.Add(new Token("sfuncao", id));
                    break;
                case "div":
                    listTokens.Add(new Token("sdiv", id));
                    break;
                case "e":
                    listTokens.Add(new Token("se", id));
                    break;
                case "ou":
                    listTokens.Add(new Token("sou", id));
                    break;
                case "nao":
                    listTokens.Add(new Token("snao", id));
                    break;
                default:
                    listTokens.Add(new Token("sidentificador", id));
                    break;
            }
        }

        public void trataAtribuicao(char caracter)
        {
            string atribuicao;
            atribuicao = String.Copy(Char.ToString(caracter));

            //caracter = ler();

            if(caracter == '=')
            {
                atribuicao = String.Copy(Char.ToString(caracter)); 
                listTokens.Add(new Token("satribuicao", atribuicao));
            }
            else
            {
                listTokens.Add(new Token("sdoispontos", atribuicao));
            }
           
        }

        // Test function
        public void showListToken()
        {

        }
    }
}
