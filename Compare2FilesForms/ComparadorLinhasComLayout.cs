using System;
using System.Collections.Generic;
using System.Linq;

namespace Compare2FilesForms
{
    internal class ComparadorLinhasComLayout
    {
        private readonly List<LinhaContabilidade> linhasAntigas;
        private readonly List<LinhaContabilidade> linhasNovas;

        public ComparadorLinhasComLayout(IEnumerable<LinhaContabilidade> linhasAntigas, IEnumerable<LinhaContabilidade> linhasNovas)
        {
            this.linhasAntigas = linhasAntigas.ToList();
            this.linhasNovas = linhasNovas.ToList(); 
        }

        public IList<string> Analisar()
        {
            var retorno = new List<string>();

            foreach (var linhaNova in linhasNovas)
            {
                var numeroFatura = linhaNova.Fatura;
                var linhasAntigasComMesmaFatura = linhasAntigas.Where(x => x.Fatura == numeroFatura).ToList();

                if (linhasAntigasComMesmaFatura.Any())
                {
                    var valoresNovos = linhaNova.ExtrairValores();

                    retorno.Add(linhaNova.Linha);
                    retorno.Add($"- Fatura: {linhaNova.Fatura}, Sinistro: {linhaNova.Sinistro}, Dt. Sinistro: {linhaNova.DataSinistro}, Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}");
                    retorno.Add("- Linhas com mesma fatura:");

                    foreach (var linhaAntigasComMesmaFatura in linhasAntigasComMesmaFatura)
                    {
                        retorno.Add(linhaAntigasComMesmaFatura.Linha);
                        var valoresAntigos = linhaAntigasComMesmaFatura.ExtrairValores();

                        foreach (var valorAntigo in valoresAntigos)
                        {
                            var colunaConhecida = ColunasConhecidasComAlteracaoCadastral
                                .Where(x => x.Item2 == valorAntigo.Inicial)
                                .FirstOrDefault(x => x.Item3 == valorAntigo.Coluna.Tamanho);

                            if (colunaConhecida != null)
                                continue;

                            var valorNovo = valoresNovos
                                .Where(x => x.Inicial == valorAntigo.Inicial)
                                .First(x => x.Coluna.Tamanho == valorAntigo.Coluna.Tamanho);

                            if (valorNovo.Texto != valorAntigo.Texto)
                                retorno.Add($"Coluna: {valorNovo.Coluna.Nome} - Inicial: {valorNovo.Inicial + 1} - Tamanho: {valorNovo.Coluna.Tamanho} - ANTES: '{valorAntigo.Texto}' - NOVO: '{valorNovo.Texto}'");
                        }
                    }

                    retorno.Add(string.Empty);
                    retorno.Add(string.Empty);
                }
                else
                {
                    retorno.Add($"Fatura: {linhaNova.Fatura}, Sinistro: {linhaNova.Sinistro}, Dt. Sinistro: {linhaNova.DataSinistro}, Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}");
                    retorno.Add($"{linhaNova.Linha}");

                    var linhasAntigasSemelhantes = linhasAntigas.Where(x => Equals(x, linhaNova)).ToList();

                    if (!linhasAntigasSemelhantes.Any())
                    {
                        retorno.Add("- Não encontrada linha semelhante!");
                        continue;
                    }

                    foreach (var linhaAntigaSemelhante in linhasAntigasSemelhantes)
                    {
                        retorno.Add($"{linhaAntigaSemelhante.Linha}");

                        var inicioDiferenca = 0;

                        string textoNovo;
                        string textoAntigo;

                        foreach (var colunaConhecida in ColunasConhecidasComAlteracaoCadastral)
                        {
                            textoNovo = linhaNova.Linha.Substring(colunaConhecida.Item2, colunaConhecida.Item3);
                            textoAntigo =
                                linhaAntigaSemelhante.Linha.Substring(colunaConhecida.Item2, colunaConhecida.Item3);

                            if (textoNovo == textoAntigo)
                                continue;

                            retorno.Add($"- Diferença {colunaConhecida.Item1} (Posição: {colunaConhecida.Item2 + 1} - Tamanho {colunaConhecida.Item3})");
                            retorno.Add($"  => N: {textoNovo}");
                            retorno.Add($"  => A: {textoAntigo}");
                        }

                        textoNovo = string.Empty;
                        textoAntigo = string.Empty;

                        for (var i = 0; i < linhaNova.Linha.Length; i++)
                        {
                            var colunaConhecida =  ColunasConhecidasComAlteracaoCadastral
                                .FirstOrDefault(x =>  i >= x.Item2 && i < x.Item2 + x.Item3);

                            if (colunaConhecida != null)
                                continue;

                            if (linhaNova.Linha[i] != linhaAntigaSemelhante.Linha[i])
                            {
                                if (inicioDiferenca == 0)
                                    inicioDiferenca = i + 1;

                                textoNovo = textoNovo + linhaNova.Linha[i];
                                textoAntigo = textoAntigo + linhaAntigaSemelhante.Linha[i];
                            }
                            else if (linhaNova.Linha[i] == linhaAntigaSemelhante.Linha[i])
                            {
                                if (inicioDiferenca <= 0)
                                    continue;

                                retorno.Add($"- Diferença: {inicioDiferenca}");
                                retorno.Add($"  => N: {textoNovo}");
                                retorno.Add($"  => A: {textoAntigo}");

                                inicioDiferenca = 0;
                                textoNovo = string.Empty;
                                textoAntigo = string.Empty;
                            }
                        }
                    }

                    retorno.Add("-----------------------------------------------------------");
                    retorno.Add(string.Empty);
                }
            }

            return retorno;
        }

        //Colunas conhecidas que podem gerar diferença por alteração cadastral
        private static IEnumerable<Tuple<string, int, int>> ColunasConhecidasComAlteracaoCadastral =>
            new List<Tuple<string, int, int>>
            {
                new Tuple<string, int, int>("Data cancelamento apólice", 71, 10),
                new Tuple<string, int, int>("Valor IOF", 308, 13),
                new Tuple<string, int, int>("Nome conta financeira", 570, 50),
                new Tuple<string, int, int>("Nome segurado", 81, 50),
                new Tuple<string, int, int>("Data vencimento", 342, 10),
                new Tuple<string, int, int>("Sucursal", 3, 5),
                new Tuple<string, int, int>("Sucursal Sinistro", 514, 5),
                new Tuple<string, int, int>("Susep", 549, 6),
                new Tuple<string, int, int>("Data aviso sinistro (Data contábil fatura)", 487, 10),
                new Tuple<string, int, int>("Data movimento sinistro (Data contábil fatura)", 497, 10),
                new Tuple<string, int, int>("Data emissão ordem pagamento (Data contábil fatura)", 620, 10)
            };
    }
}