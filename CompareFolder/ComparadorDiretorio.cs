using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Compare2FilesForms;

namespace CompareFolder
{
    internal class ComparadorDiretorio
    {
        private readonly string diretorioInicial;
        private string DiretorioInicialNovo => diretorioInicial + "NOVO\\";
        private string DiretorioInicialAtual => diretorioInicial + "ATUAL\\";
        private readonly IList<string> listaNaoEncontradoArquivoAtual;
        private readonly IList<string> listaArquivoIdentico;
        private readonly IList<string> listaArquivoDiferente;

        public ComparadorDiretorio(string diretorioInicial)
        {
            this.diretorioInicial = diretorioInicial;

            if (this.diretorioInicial.EndsWith("\\") == false)
                this.diretorioInicial += "\\";

            listaNaoEncontradoArquivoAtual = new List<string>();
            listaArquivoIdentico = new List<string>();
            listaArquivoDiferente = new List<string>();
        }

        public void Comparar()
        {
            var arquivosNovos = Directory.GetFiles(DiretorioInicialNovo, "*.*", SearchOption.AllDirectories);
            var arquivosAtuais = Directory.GetFiles(DiretorioInicialAtual, "*.*", SearchOption.AllDirectories);

            foreach (var arquivoNovo in arquivosNovos)
            {
                var nomeArquivo = arquivoNovo.Replace(DiretorioInicialNovo, string.Empty);

                var arquivoAtual = arquivosAtuais.FirstOrDefault(x => x.EndsWith(nomeArquivo));

                if (string.IsNullOrEmpty(arquivoAtual))
                {
                    listaNaoEncontradoArquivoAtual.Add(arquivoNovo);
                }
                else
                {
                    bool NaoEhCabecalhoNemRodape(string x) => x.StartsWith("H") == false && x.StartsWith("T") == false;
                    LinhaContabilidade SelecaoLinhaContabilidade(string x) => new LinhaContabilidade(x.PrepareToCompare());

                    IList<LinhaContabilidade> linhasArquivoNovo = File
                        .ReadAllLines(arquivoNovo)
                        .Where(NaoEhCabecalhoNemRodape)
                        .Select(SelecaoLinhaContabilidade)
                        .ToList();
                    
                    IList<LinhaContabilidade> linhasArquivoAtual = File
                        .ReadAllLines(arquivoAtual)
                        .Where(NaoEhCabecalhoNemRodape)
                        .Select(SelecaoLinhaContabilidade)
                        .ToList();

                    IList<LinhaContabilidade> linhasQueExistemApenasNoArquivoNovo = linhasArquivoNovo.Except(linhasArquivoAtual).ToList();
                    IList<LinhaContabilidade> linhasQueExistemApenasNoArquivoAtual = linhasArquivoAtual.Except(linhasArquivoNovo).ToList();

                    var linhasDiferentes = linhasQueExistemApenasNoArquivoNovo.Union(linhasQueExistemApenasNoArquivoAtual).Distinct();

                    if (!linhasDiferentes.Any())
                    {
                        listaArquivoIdentico.Add(arquivoNovo + " => " + arquivoAtual);
                    }
                    else
                    {
                        var comparadorLinhas = new ComparadorLinhasComLayout(linhasQueExistemApenasNoArquivoNovo, linhasQueExistemApenasNoArquivoAtual);
                        var listaAnaliseDiferenca = comparadorLinhas.Analisar();

                        listaArquivoDiferente.Add(arquivoNovo + " => " + arquivoAtual);
                        
                        foreach (var analiseDiferenca in listaAnaliseDiferenca)
                            listaArquivoDiferente.Add(analiseDiferenca);
                    }
                }
            }

            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Identico.txt", listaArquivoIdentico);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Diferente.txt", listaArquivoDiferente);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Arquivo_Nao_Encontrado.txt", listaNaoEncontradoArquivoAtual);
        }

        private void Gravar(string nomeArquivoGravacao, IList<string> linhas)
        {
            if (linhas.Any())
                File.WriteAllLines(Path.Combine(diretorioInicial, nomeArquivoGravacao), linhas);
        }
    }
}