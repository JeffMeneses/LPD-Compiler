﻿using System;
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
using LPD_Compiler.CompilerHandler;

namespace LPD_Compiler
{
    public partial class Form1 : Form
    {
        Compiler compiler;

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
            compiler = new Compiler();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();

            compiler.runCompiler();

            if (compiler.lexicon.listTokens != null)
            {
                foreach (var i in compiler.lexicon.listTokens)
                {
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = n;
                    dataGridView1.Rows[n].Cells[1].Value = i.simbolo;
                    dataGridView1.Rows[n].Cells[2].Value = i.lexema;
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
