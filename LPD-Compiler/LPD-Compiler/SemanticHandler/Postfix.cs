using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LPD_Compiler.LexiconHandler;

namespace LPD_Compiler.SemanticHandler
{
    public class Postfix
    {
        public List<Token> expression = new List<Token>();
        public List<Token> convertedExpression = new List<Token>();

        public Stack<Token> stackSymbols = new Stack<Token>();

        public Postfix()
        {

        }

        public Postfix(List<Token> expression)
        {
            this.expression = expression;
        }

        public void convertExpression()
        {

            foreach(var token in expression.ToList())
            {
                if(token.simbolo == "sidentificador")
                {
                    convertedExpression.Add(token);
                    continue;
                }

                if (stackSymbols.Count != 0)
                    if (menorPrecedenciaAB(token, stackSymbols.Last()))
                        desempilhaSimbolo(token);
                    else
                        empilhaSimbolo(token);
                else
                    if (menorPrecedenciaAB(token, null))
                        desempilhaSimbolo(token);
                    else
                        empilhaSimbolo(token);
            }
            desempilhaTudo();
            Console.WriteLine("Expressao: ", convertedExpression);
        }

        public void test()
        {
            List<Token> test = new List<Token>();

            test.Add(new Token("sidentificador", "a"));
            test.Add(new Token("smais", "+"));
            test.Add(new Token("sidentificador", "b"));
            test.Add(new Token("smaior", ">"));
            test.Add(new Token("sidentificador", "3"));
            test.Add(new Token("se", "and"));
            test.Add(new Token("sidentificador", "c"));
            test.Add(new Token("smaior", ">"));
            test.Add(new Token("sidentificador", "d"));

            this.expression = test;
            convertExpression();
        }

        public bool menorPrecedenciaAB(Token tokenA, Token tokenB)
        {
            int precedenciaA, precedenciaB;

            precedenciaA = checkPrecedencia(tokenA);
            precedenciaB = checkPrecedencia(tokenB);

            if (precedenciaB == -1)
                return true;
            if (precedenciaA <= precedenciaB)
                return true;
            return false;
        }

        public int checkPrecedencia(Token token)
        {
            if (token == null) return -1;
            switch (token.simbolo)
            {
                //case '-u':
                //case '+u':
                //case "snao": return 0;

                case "smult":
                case "sdiv": return 6;

                case "smenos":
                case "smais": return 5;

                case "smenor":
                case "smenorig":
                case "smaiorig":
                case "smaior": return 4;

                case "sig":
                case "sdif": return 3;

                case "se": return 2;

                case "sou": return 1;
            }

            return -1;
        }

        public void desempilhaSimbolo(Token token)
        {
            Token aux;
            if(stackSymbols.Count != 0)
            {
                aux = stackSymbols.Pop();
                convertedExpression.Add(aux);
            }

            stackSymbols.Push(token);
        }

        public void empilhaSimbolo(Token token)
        {
            stackSymbols.Push(token);
        }

        public void desempilhaTudo()
        {
            Token token;
            while(stackSymbols.Count != 0)
            {
                token = stackSymbols.Pop();
                convertedExpression.Add(token);
            }
        }
    }
}
