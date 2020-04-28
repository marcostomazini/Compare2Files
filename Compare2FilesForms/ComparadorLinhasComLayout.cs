using System.Collections.Generic;
using System.Linq;

namespace Compare2FilesForms
{
    public class ComparadorLinhasComLayout
    {
        private readonly List<LinhaContabilidade> linhasAntigas;
        private readonly List<LinhaContabilidade> linhasNovas;

        public ComparadorLinhasComLayout(IEnumerable<LinhaContabilidade> linhasAntigas, IEnumerable<LinhaContabilidade> linhasNovas)
        {
            this.linhasAntigas = linhasAntigas.ToList();
            this.linhasNovas = linhasNovas.ToList(); 
        }

        public IList<string> Analisar(out List<string> resultadoListaDiferencaIgnorada)
        {
            resultadoListaDiferencaIgnorada = new List<string>();
            var resultadoListaComDiferenca = new List<string>();

            var listaDiferencaIgnorada = new List<string>();
            var listaComDiferenca = new List<string>();
            
            foreach (var linhaNova in linhasNovas)
            {
                var linhasAntigasComMesmaFaturaSinistro = linhasAntigas
                    .Where(x => x.Fatura == linhaNova.Fatura)
                    .Where(x => x.Sinistro == linhaNova.Sinistro)
                    .Where(x => x.ParTip == linhaNova.ParTip)
                    .Where(x => x.CodigoExterno == linhaNova.CodigoExterno)
                    .ToList();

                if (linhasAntigasComMesmaFaturaSinistro.Any())
                {
                    var valoresNovos = linhaNova.ExtrairValores();
                    
                    foreach (var linhaAntigasComMesmaFatura in linhasAntigasComMesmaFaturaSinistro)
                    {
                        var valoresAntigos = linhaAntigasComMesmaFatura.ExtrairValores();

                        foreach (var valorAntigo in valoresAntigos)
                        {
                            var valorNovo = valoresNovos
                                .Where(x => x.Inicial == valorAntigo.Inicial)
                                .First(x => x.Coluna.Tamanho == valorAntigo.Coluna.Tamanho);

                            var textoDiferenca = $"Coluna: {valorNovo.Coluna.Nome} - Inicial: {valorNovo.Inicial + 1} - Tamanho: {valorNovo.Coluna.Tamanho} - ANTES: '{valorAntigo.Texto}' - NOVO: '{valorNovo.Texto}'";

                            if (valorNovo.DeveIgnoradarDiferenca(valorAntigo))
                            {
                                listaDiferencaIgnorada.Add(linhaAntigasComMesmaFatura.Linha);
                                listaDiferencaIgnorada.Add(textoDiferenca);
                            }
                            else if (valorNovo.Texto != valorAntigo.Texto)
                            {
                                listaComDiferenca.Add(linhaAntigasComMesmaFatura.Linha);
                                listaComDiferenca.Add(textoDiferenca);
                            }
                        }
                    }
                }
                else
                {
                    listaComDiferenca.Add("- Não encontrada linha no arquivo, com mesma fatura, sinitro, parTip, codigo externo");
                }

                var textoLinha = $"- Fatura: {linhaNova.Fatura}, Sinistro: {linhaNova.Sinistro}, Dt. Sinistro: {linhaNova.DataSinistro}, Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}, ParTip: {linhaNova.ParTip}, Codigo Externo: {linhaNova.CodigoExterno}, Tipo Sinistro: {linhaNova.DescricaoTipoSinitro}";

                if (listaComDiferenca.Any())
                {
                    resultadoListaComDiferenca.Add("   " + linhaNova.Linha);
                    resultadoListaComDiferenca.Add("   " + textoLinha);
                    resultadoListaComDiferenca.AddRange(listaComDiferenca.Select(texto => "      " + texto));
                    resultadoListaComDiferenca.Add("-----------------------------------------------------------");
                }

                if (listaDiferencaIgnorada.Any())
                {
                    resultadoListaDiferencaIgnorada.Add("   " + linhaNova.Linha);
                    resultadoListaDiferencaIgnorada.Add("   " + textoLinha);
                    resultadoListaDiferencaIgnorada.AddRange(listaDiferencaIgnorada.Select(textoIgnorada => "      " + textoIgnorada));
                    resultadoListaDiferencaIgnorada.Add("-----------------------------------------------------------");
                }
            }

            return resultadoListaComDiferenca;
        }
    }
}