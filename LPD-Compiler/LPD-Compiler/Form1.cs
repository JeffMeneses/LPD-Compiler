using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Documents;
using LPD_Compiler.FileHandler;
using LPD_Compiler.LexiconHandler;
using LPD_Compiler.CompilerHandler;
using System.IO;



namespace LPD_Compiler
{
    public partial class Form1 : Form
    {
        Compiler compiler;
        public LpdFile lpdFile;


        public Form1()
        {
            InitializeComponent();
        }

        /*private void Form1_Load(object sender, EventArgs e)
        {

        }*/

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            lpdFile = new LpdFile();
            int i = 1;
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
                    lpdFile.name = openFileDialog1.FileName;
                    FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                    StreamReader m_streamReader = new StreamReader(fs);
                    // Lê o arquivo usando a classe StreamReader
                    m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
                    // Lê cada linha do stream e faz o parse até a última linha
                    this.richTextBox1.Text = "";
                    string strLine = m_streamReader.ReadLine();
                    while (strLine != null)
                    {
                        /*s = Convert.ToString(i); 
                        if(i < 10)*/
                        this.richTextBox1.Text += strLine + "\n";
                        /* else
                             this.richTextBox1.Text += s+"    "+ strLine + "\n";*/
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

        private void sairToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void compilarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Clear();
            int index = 0;
            lpdFile = new LpdFile();

            if(openFileDialog1.FileName != "") lpdFile.name = openFileDialog1.FileName;

            if (lpdFile.name == null)
            {
                //MessageBox.Show("Você precisa abrir um arquivo primeiro!\n", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                FileStream fs = new FileStream("temporary.txt", FileMode.Create, FileAccess.Write);
                StreamWriter m_streamWriter = new StreamWriter(fs);
                m_streamWriter.Flush();
                // Escreve para o arquivo usando a classe StreamWriter
                m_streamWriter.BaseStream.Seek(0, SeekOrigin.Begin);
                // escreve no controle richtextbox
                m_streamWriter.Write(this.richTextBox1.Text);
                // fecha o arquivo
                m_streamWriter.Flush();
                m_streamWriter.Close();

                lpdFile.name = "temporary.txt";
            }

            lpdFile.readFile(lpdFile.name);

            if(lpdFile.content.Count() == 0) MessageBox.Show("Você precisa abrir um arquivo primeiro!\n", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                compiler = new Compiler();
                compiler.runCompiler(lpdFile);
                if (compiler.returnError() != 0)
                {
                    //int n = dataGridView1.Rows.Add();
                    //dataGridView1.Rows[n].Cells[0].Value = compiler.syntactic.line;
                    //dataGridView1.Rows[n].Cells[1].Value = compiler.syntactic.message;
                    index = (compiler.syntactic.line - 1);
                    try
                    {
                        richTextBox1.Select(richTextBox1.GetFirstCharIndexFromLine(index), richTextBox1.Lines[index].Length);
                        richTextBox1.SelectionColor = Color.Red;
                    }
                    catch(Exception)
                    {

                    }

                }
            }
        }

        // NEW DESIGN CODE
        public int getWidth()
        {
            int w = 25;
            // get total lines of richTextBox1    
            int line = richTextBox1.Lines.Length;

            if (line <= 99)
            {
                w = 20 + (int)richTextBox1.Font.Size;
            }
            else if (line <= 999)
            {
                w = 30 + (int)richTextBox1.Font.Size;
            }
            else
            {
                w = 50 + (int)richTextBox1.Font.Size;
            }

            return w;
        }

        public void AddLineNumbers()
        {
            // create & set Point pt to (0,0)    
            Point pt = new Point(0, 0);
            // get First Index & First Line from richTextBox1    
            int First_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int First_Line = richTextBox1.GetLineFromCharIndex(First_Index);
            // set X & Y coordinates of Point pt to ClientRectangle Width & Height respectively    
            pt.X = ClientRectangle.Width;
            pt.Y = ClientRectangle.Height;
            // get Last Index & Last Line from richTextBox1    
            int Last_Index = richTextBox1.GetCharIndexFromPosition(pt);
            int Last_Line = richTextBox1.GetLineFromCharIndex(Last_Index);
            // set Center alignment to LineNumberTextBox    
            LineNumberTextBox.SelectionAlignment = HorizontalAlignment.Center;
            // set LineNumberTextBox text to null & width to getWidth() function value    
            LineNumberTextBox.Text = "";
            LineNumberTextBox.Width = getWidth();
            // now add each line number to LineNumberTextBox upto last line    
            for (int i = First_Line; i <= Last_Line + 1; i++)
            {
                LineNumberTextBox.Text += i + 1 + "\n";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            LineNumberTextBox.ForeColor = Color.Green;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            Point pt = richTextBox1.GetPositionFromCharIndex(richTextBox1.SelectionStart);
            if (pt.X == 1)
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            LineNumberTextBox.Text = "";
            AddLineNumbers();
            LineNumberTextBox.Invalidate();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "")
            {
                AddLineNumbers();
            }
        }

        private void richTextBox1_FontChanged(object sender, EventArgs e)
        {
            LineNumberTextBox.Font = richTextBox1.Font;
            richTextBox1.Select();
            AddLineNumbers();
        }

        private void LineNumberTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            richTextBox1.Select();
            LineNumberTextBox.DeselectAll();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AddLineNumbers();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Green, ButtonBorderStyle.Solid);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Green, ButtonBorderStyle.Solid);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.Green, ButtonBorderStyle.Dashed);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void salvarComoToolStripMenuItem_Click(object sender, EventArgs e)
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
                    lpdFile.name = saveFileDialog1.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro : " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(lpdFile.name != null)
                { 
                    FileStream fs = new FileStream(lpdFile.name, FileMode.Create, FileAccess.Write);
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
    }
}
