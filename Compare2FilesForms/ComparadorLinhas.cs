﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Compare2FilesForms
{
    internal class ComparadorLinhas
    {
        private readonly List<LinhaContabilidade> _linhasAntigas;
        private readonly List<LinhaContabilidade> _linhasNovas;

        public ComparadorLinhas(IEnumerable<LinhaContabilidade> linhasAntigas, IEnumerable<LinhaContabilidade> linhasNovas)
        {
            _linhasAntigas = linhasAntigas.ToList();
            _linhasNovas = linhasNovas.ToList(); 
        }

        public IList<string> Analisar()
        {
            //Diferenças conhecidas que podem ser geradas por alteração cadastral
            var colunasConhecidas = new List<Tuple<string, int, int>>
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
            
            var retorno = new List<string>();

            foreach (var linhaNova in _linhasNovas)
            {
                retorno.Add($"Fatura: {linhaNova.Fatura}, Endosso: {linhaNova.Endosso}, Dt. Sinistro: {linhaNova.DataSinistro}, Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}");
                retorno.Add($"{linhaNova.Linha}");
                
                var linhasAntigasSemelhantes = _linhasAntigas.Where(x => Equals(x, linhaNova)).ToList();

                if (!linhasAntigasSemelhantes.Any())
                {
                    retorno.Add("- Não encontrada linha semelhante!");
                    continue;
                }

                foreach (var linhaAntigaSemelhante in linhasAntigasSemelhantes)
                {
                    retorno.Add(string.Format("{0}", linhaAntigaSemelhante.Linha));

                    var inicioDiferenca = 0;

                    string textoNovo;
                    string textoAntigo;

                    foreach (var colunaConhecida in colunasConhecidas)
                    {
                        textoNovo = linhaNova.Linha.Substring(colunaConhecida.Item2, colunaConhecida.Item3);
                        textoAntigo = linhaAntigaSemelhante.Linha.Substring(colunaConhecida.Item2, colunaConhecida.Item3);

                        if (textoNovo == textoAntigo) 
                            continue;

                        retorno.Add(string.Format("- Diferença {0} (Posição: {1} - Tamanho {2})", colunaConhecida.Item1, colunaConhecida.Item2 + 1, colunaConhecida.Item3));
                        retorno.Add(string.Format("  => N: {0}", textoNovo));
                        retorno.Add(string.Format("  => A: {0}", textoAntigo));
                    }

                    textoNovo = string.Empty;
                    textoAntigo = string.Empty;

                    for (var i = 0; i < linhaNova.Linha.Length; i++)
                    {
                        var colunaConhecida =
                            colunasConhecidas.FirstOrDefault(x => i >= x.Item2 && i < x.Item2 + x.Item3);

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
                            
                            retorno.Add(string.Format("- Diferença: {0}", inicioDiferenca));
                            retorno.Add(string.Format("  => N: {0}", textoNovo));
                            retorno.Add(string.Format("  => A: {0}", textoAntigo));

                            inicioDiferenca = 0;
                            textoNovo = string.Empty;
                            textoAntigo = string.Empty;
                        }
                    }
                }
                
                retorno.Add("-----------------------------------------------------------");
                retorno.Add(string.Empty);
            }

            return retorno;
        }
    }
}