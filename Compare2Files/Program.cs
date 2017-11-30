using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compare2Files
{
    class teste
    {
        public teste()
        {

        }
        public teste(string t)
        {
            Descricao = t;
        }
        public string Descricao { get; set; }
    }

    class Program
    {

        static void Main(string[] args)
        {
            var especialidade = new teste();
            var especialidade2 = new teste();

            var especialidades = new List<teste>();
            var t = new teste("teste"); 
            especialidades.Add(new teste("RADIOLOGIA ODONTOLOGIA"));

            especialidades.Add(t);

            var listaEspecialidadeURA = new Dictionary<int, string>();
            listaEspecialidadeURA.Add(991, "CLINICA GERAL;CLINICA MEDICA");
            listaEspecialidadeURA.Add(994, "ENDODONTIA");
            listaEspecialidadeURA.Add(1000, "CIRURGIA");
            listaEspecialidadeURA.Add(998, "PERIODONTIA");
            listaEspecialidadeURA.Add(995, "ODONTOPEDIATRIA");
            listaEspecialidadeURA.Add(993, "RADIOLOGIA");
            listaEspecialidadeURA.Add(711, "GINECOLOGIA");
            listaEspecialidadeURA.Add(721, "OBSTETRICIA");
            listaEspecialidadeURA.Add(1263, "ORTOPEDIA");
            listaEspecialidadeURA.Add(136, "OFTALMOLOGIA");
            listaEspecialidadeURA.Add(117, "DERMATOLOGIA");
            listaEspecialidadeURA.Add(140, "PEDIATRIA");
            listaEspecialidadeURA.Add(105, "CARDIOLOGIA");
            listaEspecialidadeURA.Add(118, "ENDOCRINOLOGIA");

            string descricaoEspecialidade = "TESTE;RADIOasdLOGIA";
            var arrayEspc = descricaoEspecialidade.Split(';');

            Stopwatch sw = Stopwatch.StartNew();

            if (arrayEspc.Length > 1)
            {
                especialidade = especialidades
                    .FirstOrDefault(e =>
                    e.Descricao.ToUpper().Contains(arrayEspc[0]));
                if (especialidade == null)
                {
                    especialidade = especialidades
                        .FirstOrDefault(e =>
                        e.Descricao.ToUpper().Contains(arrayEspc[1]));
                }
            }
            else
            {
                especialidade = especialidades
                    .FirstOrDefault(e =>
                    e.Descricao.ToUpper().Contains(arrayEspc[0]));
            }

            var tempo1 = sw.Elapsed;      
            sw.Stop();

            sw = Stopwatch.StartNew();            

            especialidade2 = especialidades
                   .FirstOrDefault(e => arrayEspc.Any(e.Descricao.ToUpper().Contains));

            var tempo2 = sw.Elapsed;
            sw.Stop();

            var asd = "RADIOLOGIA ODONTOLOGIA".ToUpper().Contains(arrayEspc[1]);

            var sad = arrayEspc.Any("RADIOLOGIA ODONTOLOGIA".ToUpper().Contains);

            var dsa = Array.IndexOf(arrayEspc, "RADIOLOGIA ODONTOLOGIA".ToUpper());

            var sdaasdespecialidade = especialidades
                   .FirstOrDefault(e => arrayEspc.Any(e.Descricao.ToUpper().Contains));

            var asdespecialidade = especialidades
                   .Where(e => arrayEspc.Any(e.Descricao.ToUpper().Contains)).FirstOrDefault();

            especialidade = especialidades
                   .FirstOrDefault(e => arrayEspc.Contains(e.Descricao.ToUpper()));



            var asdsa = abreviarNome("RECOGNITION COMPANHIA BRASILEIRA DE AUTOMACAO BANCARIA");
            String directory = @"D:\Projetos\CompareTwoFiles\Files\";
            String[] linesA = File.ReadAllLines(Path.Combine(directory, "DEPOIS COM PERWDOCDM1_50.20170913"));
            String[] linesB = File.ReadAllLines(Path.Combine(directory, "DEPOIS SEM PERWDOCDM1_50.20170918"));

            IEnumerable<String> onlyB = linesB.Except(linesA);

            File.WriteAllLines(Path.Combine(directory, "Result.txt"), onlyB);
        }

        private static string abreviarNome(string nome)
        {
            var exceccoes = new String[] { "ME", "LTDA", "SA", "MEI", "EPP", "EIRELI", "Individual" };

            if (string.IsNullOrEmpty(nome))
            {
                return nome;
            }
            else
            {
                var tokens = nome.Trim().Split(' ');
                var tokensNomes = new List<string>();
                foreach (var t in tokens)
                {
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        tokensNomes.Add(t);
                    }
                }
                if (tokensNomes.Count > 2)
                {
                    var nomeAbreviado = tokens[0] + " ";
                    for (int i = 1; i < tokensNomes.Count - 1; i++)
                    {
                        if (!exceccoes.Contains(tokensNomes[i]))
                            nomeAbreviado += tokensNomes[i][0] + ". ";
                        else
                            nomeAbreviado += tokensNomes[i] + " ";
                    }
                    return nomeAbreviado += tokensNomes.Last();
                }
                return nome;
            }
        }
    }
}
