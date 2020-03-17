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
            if (oldFile.Text == "Arquivo 1" || newFile.Text == "Arquivo 2")
            {
                MessageBox.Show("Selecione os arquivos!!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IList<LinhaContabilidade> linesA = File.ReadAllLines(oldFile.Text).Skip(1).Select(x => new LinhaContabilidade(x.PrepareToCompare())).ToList();
            IList<LinhaContabilidade> linesB = File.ReadAllLines(newFile.Text).Skip(1).Select(x => new LinhaContabilidade(x.PrepareToCompare())).ToList();

            IList<LinhaContabilidade> onlyA = linesA.Except(linesB).ToList();
            IList<LinhaContabilidade> onlyB = linesB.Except(linesA).ToList();

            var total = onlyA.Union(onlyB).Distinct();

            if (!total.Any())
            {
                MessageBox.Show("Arquivo idêntico!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var comparadorLinhas = new ComparadorLinhasComLayout(onlyA, onlyB);
            var analiseDiferenca = comparadorLinhas.Analisar();

            var result = MessageBox.Show(string.Format("Foi encontrado diferenças nos arquivos.\n\nTotal de diferenças: {0} linhas.\n\nDeseja salvar as diferenças ?", onlyA.Distinct().Count()), 
                "Confirmação", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return;

            var savefile = new SaveFileDialog
            {
                FileName = string.Format("Comparacao{0:yyyyMMdd}.txt", DateTime.Now),
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (savefile.ShowDialog() != DialogResult.OK)
                return;
                
            var directory = Path.GetDirectoryName(savefile.FileName);

            File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_Arquivo_Antigo.txt", DateTime.Now)), onlyA.Select(x => x.Linha));
            File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_Arquivo_Novo.txt", DateTime.Now)), onlyB.Select(x => x.Linha));
            File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0:yyyyMMdd}_DIFF.txt", DateTime.Now)), analiseDiferenca);
            MessageBox.Show("Salvo com sucesso!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
