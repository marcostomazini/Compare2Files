using System;
using System.IO;

namespace CompareFolder
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length != 1)
                throw new Exception("Parâmetro inválido. Deve ser informado um diretório inicial, onde: " +
                                    "- Deve existir uma subpasta ATUAL e outra sub pasta NOVO" +
                                    "- Abaixo de cada subpasta uma outra subpasta com a data do arquivo");

            var diretorioInicial = args[0];

            if (Directory.Exists(diretorioInicial) == false)
                throw new Exception($"Diretório {diretorioInicial} não encontrado");

            var comparadorDiretorio = new ComparadorDiretorio(diretorioInicial);
            comparadorDiretorio.Comparar();
        }
    }
}
