using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.FileHandler;
using System.Runtime.InteropServices.WindowsRuntime;
using LPD_Compiler.SyntacticHandler;

namespace LPD_Compiler.CompilerHandler
{
    public class Compiler
    {
        public Lexicon lexicon = new Lexicon();
        public Syntactic syntactic = new Syntactic();
        int flag = 0;

        public void runCompiler(LpdFile lpdFile)
        {
            

            try
            {
                lexicon.lexicalAnalyser(lpdFile);
            }
            catch (LexiconException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag++;
            }

            try
            {
                syntactic.syntacticAnalyser(lexicon);
            }
            catch (LexiconException ex)
            {
                MessageBox.Show(ex.Message, "Erro Léxico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag++;
            }
            catch (SyntacticException ex)
            {
                MessageBox.Show(ex.Message, "Erro Sintático", MessageBoxButtons.OK, MessageBoxIcon.Error);
                flag++;
            }
        }


        public int returnError()
        {
            if (flag == 0)
            {
                MessageBox.Show("Código compilado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return 0;
            }
            return flag;
        }
    }
}
