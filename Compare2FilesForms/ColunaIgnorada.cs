using System;
using System.Collections.Generic;
using System.Text;

namespace Compare2FilesForms
{
    public class ColunaIgnorada
    {
        private static readonly List<Tuple<int, int>> ColunasIgnoradas = new List<Tuple<int, int>>
        {
            // Data da ocorrência do sinistro.
            new Tuple<int, int>(477, 10),
            // Nosso numero
            new Tuple<int, int>(804, 15),
            // Nome segurado
            new Tuple<int, int>(81, 50),
            // Nome favorecido
            new Tuple<int, int>(570, 50)
        };

        public static List<string> IgnorarColunas(IList<string> linhas)
        {
            var novasLinhas = new List<string>();

            foreach (var linha in linhas)
            {
                var linhaBuilder = new StringBuilder(linha);

                foreach (var ignorarColuna in ColunasIgnoradas)
                    for (var i = ignorarColuna.Item1; i < ignorarColuna.Item2; i++)
                        linhaBuilder[i] = 'X';

                novasLinhas.Add(linhaBuilder.ToString());
            }

            return novasLinhas;
        }
    }
}