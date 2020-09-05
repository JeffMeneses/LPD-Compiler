using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;

namespace LPD_Compiler
{
    public partial class Form1 : Form
    {
        LpdFile lpdFile = new LpdFile();
        Lexicon lexicon = new Lexicon();
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void arquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();

            lpdFile.openLpdFile();
            lines = lpdFile.readFile(lpdFile.name);

            lexicon.lexicalAnalyser(lines);
            lexicon.showListToken();
        }
    }
}
