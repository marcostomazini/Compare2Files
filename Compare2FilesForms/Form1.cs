using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = (openFileDialog1.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label2.Text = (openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var palavraIgnorada = string.Empty;
            var letra = "X";

            if (label1.Text == "Arquivo 1" || label2.Text == "Arquivo 2")
            {
                MessageBox.Show("Selecione os arquivos!!!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IList<string> linesA = File.ReadAllLines(label1.Text).Select(x => x.ToUpper()).ToList();
            IList<string> linesB = File.ReadAllLines(label2.Text).Select(x => x.ToUpper()).ToList();

            var ignorarColunas = new List<Tuple<int, int>>
            {
                // Data da ocorrência do sinistro.
                new Tuple<int, int>(477, 10), 
                // nosso numero
                new Tuple<int, int>(804, 15)
            };

            foreach (var item in ignorarColunas.OrderBy(x => x.Item1))
            {
                palavraIgnorada = letra;
                for (var i = 1; i < (item.Item2); i++)
                {
                    palavraIgnorada += letra;
                }

                linesA = linesA.Select(x => x.Remove(item.Item1, item.Item2).Insert(item.Item1, palavraIgnorada)).ToArray();
                linesB = linesB.Select(x => x.Remove(item.Item1, item.Item2).Insert(item.Item1, palavraIgnorada)).ToArray();
            }
            
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
