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
using LPD_Compiler.CompilerHandler;
using System.IO;



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

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e) //abrir - http://www.macoratti.net/15/05/c_rctbp1.htm
        {
            int i = 0;
            string s;
            //define as propriedades do controle 
            //OpenFileDialog
            this.openFileDialog1.Multiselect = true;
            this.openFileDialog1.Title = "Selecionar Arquivo";
            openFileDialog1.InitialDirectory = @"C:\Dados\";
            //filtra para exibir somente arquivos textos
            openFileDialog1.Filter = "Images (*.TXT)|*.TXT|" + "All files (*.*)|*.*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ReadOnlyChecked = true;
            openFileDialog1.ShowReadOnly = true;
            DialogResult dr = this.openFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader m_streamReader = new StreamReader(fs);
                    // Lê o arquivo usando a classe StreamReader
                    m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    // Lê cada linha do stream e faz o parse até a última linha
                    this.richTextBox1.Text = "";
                    string strLine = m_streamReader.ReadLine();
                    while (strLine != null)
                    {
                        s = Convert.ToString(i); 
                        if(i < 10)
                            this.richTextBox1.Text += s + "     " + strLine + "\n";
                        else
                            this.richTextBox1.Text += s+"    "+ strLine + "\n";
                        strLine = m_streamReader.ReadLine();
                        i++;
                    }
                    // Fecha o stream
                    m_streamReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro : " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e) //salvar
        {
            try
            {
                // Pega o nome do arquivo para salvar
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    // abre um stream para escrita e cria um StreamWriter para implementar o stream
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                    StreamWriter m_streamWriter = new StreamWriter(fs);
                    m_streamWriter.Flush();
                    // Escreve para o arquivo usando a classe StreamWriter
                    m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                    // escreve no controle richtextbox
                    m_streamWriter.Write(this.richTextBox1.Text);
                    // fecha o arquivo
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e) //Compilar
        {
            compiler = new Compiler();
            compiler.runCompiler();

            /*TODO
             * if(compiler.runCompiler == sem erro)
             * MessageBox.Show(ex.Message, "Código compilado com sucesso!", MessageBoxButtons.OK, MessageBoxIcon.Error);
             * else
             * adiciona linha no dataGridView1 com o N da linha e o que está errado Erro - Console
             * mais grifar a linha N no Código que cotém o Erro
             */
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) //Codigo
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) //Console
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
