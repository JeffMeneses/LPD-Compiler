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
using LPD_Compiler.SemanticHandler;

namespace LPD_Compiler.CompilerHandler
{
    public class Compiler
    {
        public Lexicon lexicon = new Lexicon();
        public Syntactic syntactic = new Syntactic();
        public Semantic semantic = new Semantic();
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
                List<string> output = syntactic.syntacticAnalyser(lexicon, semantic);
                lpdFile.createAssemblyFile(output);
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
            catch (SemanticException ex)
            {
                MessageBox.Show(ex.Message, "Erro Semantico", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
