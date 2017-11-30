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

            string[] linesA = File.ReadAllLines(label1.Text);
            string[] linesB = File.ReadAllLines(label2.Text);


            var ignorarColunas = new List<Tuple<int, int>>();

            //ignorarColunas.Add(new Tuple<int, int>(71, 10));
            ignorarColunas.Add(new Tuple<int, int>(477, 10)); // Data da ocorrência do sinistro.
            ignorarColunas.Add(new Tuple<int, int>(804, 15)); // nosso numero

            palavraIgnorada = string.Empty;
            foreach (var item in ignorarColunas.OrderBy(x => x.Item1))
            {
                palavraIgnorada = letra;
                for (int i = 1; i < (item.Item2); i++)
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

            DialogResult result = MessageBox.Show(string.Format("Foi encontrado diferenças nos arquivos.\n\nTotal de diferenças: {0} linhas.\n\nDeseja salvar as diferenças ?", onlyA.Distinct().Count()), 
                "Confirmação", 
                MessageBoxButtons.YesNo, 
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {            
                SaveFileDialog savefile = new SaveFileDialog();
                // set a default file name
                savefile.FileName = string.Format("Comparacao{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
                // set filters - this can be done in properties as well
                savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    var directory = Path.GetDirectoryName(savefile.FileName);
                    var asad = Path.GetFileNameWithoutExtension(savefile.FileName);

                    //File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0}.txt", DateTime.Now.ToString("yyyyMMdd"))), total);
                    File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0}_1.txt", DateTime.Now.ToString("yyyyMMdd"))), onlyA);
                    File.WriteAllLines(Path.Combine(directory, string.Format("Comparacao{0}_2.txt", DateTime.Now.ToString("yyyyMMdd"))), onlyB);
                    MessageBox.Show("Salvo com sucesso!", string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            
        }
    }
}
