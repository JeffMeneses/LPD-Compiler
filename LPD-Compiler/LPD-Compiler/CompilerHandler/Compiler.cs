using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.FileHandler;
using System.Runtime.InteropServices.WindowsRuntime;

namespace LPD_Compiler.CompilerHandler
{
    public class Compiler
    {
        public Lexicon lexicon = new Lexicon();

        public void runCompiler(LpdFile ldpFile)
        {
            ldpFile.openLpdFile();
            ldpFile.readFile(ldpFile.name);

            try
            {
                lexicon.lexicalAnalyser(ldpFile);
            }
            catch (LexiconException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
