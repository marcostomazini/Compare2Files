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
        private readonly IList<string> listaNaoEncontradoArquivoNovo;
        private readonly IList<string> listaArquivoIdentico;
        private readonly IList<string> listaLinhaDiferente;
        private readonly IList<string> listaLinhaDiferencaIgnorada;

        public ComparadorDiretorio(string diretorioInicial)
        {
            this.diretorioInicial = diretorioInicial;

            if (this.diretorioInicial.EndsWith("\\") == false)
                this.diretorioInicial += "\\";

            listaNaoEncontradoArquivoAtual = new List<string>();
            listaNaoEncontradoArquivoNovo = new List<string>();
            listaArquivoIdentico = new List<string>();
            listaLinhaDiferente = new List<string>();
            listaLinhaDiferencaIgnorada = new List<string>();
        }

        public void Comparar()
        {
            var arquivosNovos = Directory.GetFiles(DiretorioInicialNovo, "*.*", SearchOption.AllDirectories);
            var arquivosAtuais = Directory.GetFiles(DiretorioInicialAtual, "*.*", SearchOption.AllDirectories);

            for (var i = 0; i < arquivosNovos.Length; i++)
            {
                var arquivoNovo = arquivosNovos[i];

                Console.WriteLine($"{i + 1} de {arquivosNovos.Length} - {arquivoNovo}");
                
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
                        
                        File.Move(arquivoNovo, arquivoNovo.Replace("\\NOVO\\", "\\NOVO_ok\\"));
                        File.Move(arquivoAtual, arquivoAtual.Replace("\\ATUAL\\", "\\ATUAL_ok\\"));
                    }
                    else
                    {
                        if (linhasQueExistemApenasNoArquivoNovo.Count > 200)
                        {
                            listaLinhaDiferente.Add(arquivoNovo + " => " + arquivoAtual);
                            listaLinhaDiferente.Add("   Diferença acima de 200 registros (Conferir o tamanho do arquivo)");
                            listaLinhaDiferente.Add($"      {linhasQueExistemApenasNoArquivoNovo.Count} linhas que existem apenas no novo");
                            listaLinhaDiferente.Add($"      {linhasQueExistemApenasNoArquivoAtual.Count} linhas que existem apenas no atual");
                        }
                        else
                        {
                            var comparadorLinhas = new ComparadorLinhasComLayout(linhasQueExistemApenasNoArquivoAtual,
                                linhasQueExistemApenasNoArquivoNovo);

                            var listaAnaliseDiferenca = comparadorLinhas.Analisar(out var listaAnaliseIgnorada);

                            if (listaAnaliseDiferenca.Any())
                            {
                                listaLinhaDiferente.Add(arquivoNovo + " => " + arquivoAtual);

                                foreach (var analiseDiferenca in listaAnaliseDiferenca)
                                    listaLinhaDiferente.Add(analiseDiferenca);
                            }

                            if (listaAnaliseIgnorada.Any())
                            {
                                listaLinhaDiferencaIgnorada.Add(arquivoNovo + " => " + arquivoAtual);

                                foreach (var analiseDiferencaIgnorada in listaAnaliseIgnorada)
                                    listaLinhaDiferencaIgnorada.Add(analiseDiferencaIgnorada);

                                //Só existe diferença ignorada
                                if (listaAnaliseDiferenca.Count == 0)
                                {
                                    listaArquivoIdentico.Add(arquivoNovo + " => " + arquivoAtual);
                                    listaArquivoIdentico.Add("   Existem diferenças mas todas foram ignoradas");

                                    File.Move(arquivoNovo, arquivoNovo.Replace("\\NOVO\\", "\\NOVO_ok\\"));
                                    File.Move(arquivoAtual, arquivoAtual.Replace("\\ATUAL\\", "\\ATUAL_ok\\"));
                                }
                            }
                        }
                    }
                }
            }

            foreach (var arquivoAtual in arquivosAtuais)
            {
                var nomeArquivo = arquivoAtual.Replace(DiretorioInicialAtual, string.Empty);

                var arquivoNovo = arquivosNovos.FirstOrDefault(x => x.EndsWith(nomeArquivo));

                if (string.IsNullOrEmpty(arquivoNovo))
                {
                    listaNaoEncontradoArquivoNovo.Add(nomeArquivo);
                }
            }

            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Identico.txt", listaArquivoIdentico);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Diferente.txt", listaLinhaDiferente);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Ignorado.txt", listaLinhaDiferencaIgnorada);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Arquivo_Novo_Sem_ATUAL.txt", listaNaoEncontradoArquivoAtual);
            Gravar($"Comparacao{DateTime.Today:yyyyMMdd}_Arquivo_Atual_Sem_NOVO.txt", listaNaoEncontradoArquivoNovo);
        }

        private void Gravar(string nomeArquivoGravacao, IList<string> linhas)
        {
            File.WriteAllLines(Path.Combine(diretorioInicial, nomeArquivoGravacao), linhas);
        }
    }
}