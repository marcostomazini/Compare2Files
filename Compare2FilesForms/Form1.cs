using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Compare2FilesForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                oldFile.Text = (openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                newFile.Text = (openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var palavraIgnorada = string.Empty;

            if (oldFile.Text == "Arquivo 1" || newFile.Text == "Arquivo 2")
            {
                MessageBox.Show("Selecione os arquivos!!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            IList<string> linesA = File.ReadAllLines(oldFile.Text).Select(x => x.PrepareToCompare()).ToList();
            IList<string> linesB = File.ReadAllLines(newFile.Text).Select(x => x.PrepareToCompare()).ToList();

            IList<string> onlyA = linesA.Except(linesB).ToList();
            IList<string> onlyB = linesB.Except(linesA).ToList();

            var total = onlyA.Union(onlyB).Distinct();

            if (!total.Any())
            {
                MessageBox.Show("Arquivo idêntico!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var comparadorLinhas = new ComparadorLinhas(onlyA, onlyB);
            IList<string> analiseDiferenca = comparadorLinhas.Analisar();

            DialogResult result = MessageBox.Show(string.Format("Foi encontrado diferenças nos arquivos.\n\nTotal de diferenças: {0} linhas.\n\nDeseja salvar as diferenças ?", onlyA.Distinct().Count()), 
                "Confirmação", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {            
                var savefile = new SaveFileDialog();
                // set a default file name
                savefile.FileName = string.Format("Comparacao{0:yyyyMMdd}.txt", DateTime.Now);
                // set filters - this can be done in properties as well
                savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    var directory = Path.GetDirectoryName(savefile.FileName);
                    var asad = Path.GetFileNameWithoutExtension(savefile.FileName);

                    File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_Arquivo_Antigo.txt", DateTime.Now)), onlyA);
                    File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_Arquivo_Novo.txt", DateTime.Now)), onlyB);
                    File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_DIFF.txt", DateTime.Now)), analiseDiferenca);
                    MessageBox.Show("Salvo com sucesso!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }
    }
}
