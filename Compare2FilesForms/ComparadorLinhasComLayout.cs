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

        public IList<string> Analisar()
        {
            var retorno = new List<string>();

            foreach (var linhaNova in linhasNovas)
            {
                retorno.Add("   " + linhaNova.Linha);
                retorno.Add($"   - Fatura: {linhaNova.Fatura}, Sinistro: {linhaNova.Sinistro}, Dt. Sinistro: {linhaNova.DataSinistro}, Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}, ParTip: {linhaNova.ParTip}, Codigo Externo: {linhaNova.CodigoExterno}");

                var linhasAntigasComMesmaFaturaSinistro = linhasAntigas
                    .Where(x => x.Fatura == linhaNova.Fatura)
                    .Where(x => x.Sinistro == linhaNova.Sinistro)
                    .Where(x => x.ParTip == linhaNova.ParTip)
                    .Where(x => x.CodigoExterno == linhaNova.CodigoExterno)
                    .ToList();

                if (linhasAntigasComMesmaFaturaSinistro.Any())
                {
                    retorno.Add("   - Linhas semelhantes (Mesma fatura, sinitro, parTip, codigo externo:");

                    var valoresNovos = linhaNova.ExtrairValores();
                    
                    foreach (var linhaAntigasComMesmaFatura in linhasAntigasComMesmaFaturaSinistro)
                    {
                        retorno.Add("      " + linhaAntigasComMesmaFatura.Linha);
                        var valoresAntigos = linhaAntigasComMesmaFatura.ExtrairValores();

                        foreach (var valorAntigo in valoresAntigos)
                        {
                            var valorNovo = valoresNovos
                                .Where(x => x.Inicial == valorAntigo.Inicial)
                                .First(x => x.Coluna.Tamanho == valorAntigo.Coluna.Tamanho);

                            if (valorNovo.Texto != valorAntigo.Texto)
                                retorno.Add($"      Coluna: {valorNovo.Coluna.Nome} - Inicial: {valorNovo.Inicial + 1} - Tamanho: {valorNovo.Coluna.Tamanho} - ANTES: '{valorAntigo.Texto}' - NOVO: '{valorNovo.Texto}'");
                        }
                    }
                }
                else
                {
                    retorno.Add("   - Não encontrada linha, no arquivo ATUAL, com mesma fatura, sinitro, parTip, codigo externo");
                }

                retorno.Add("-----------------------------------------------------------");
            }

            return retorno;
        }
    }
}