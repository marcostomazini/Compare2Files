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
            
            foreach (var linhaNova in linhasNovas)
            {
                var listaDiferencaIgnorada = new List<string>();
                var listaComDiferenca = new List<string>();

                var linhasAntigasComMesmaFaturaSinistro = linhasAntigas
                    .Where(x => x.Fatura == linhaNova.Fatura)
                    .Where(x => x.Endosso == linhaNova.Endosso)
                    .Where(x => x.ParTip == linhaNova.ParTip)
                    .Where(x => x.CodigoExterno == linhaNova.CodigoExterno)
                    .Where(x => x.ContaCorporativo == linhaNova.ContaCorporativo)
                    .ToList();

                bool SeletorPorValor(LinhaContabilidade x) => x.ValorPremio == linhaNova.ValorPremio && x.ValorSinistro == linhaNova.ValorSinistro;
                
                //Caso exista mais de 1 linha semelhante.... pega a linha com mesmo valor
                if (linhasAntigasComMesmaFaturaSinistro.Count > 1 && linhasAntigasComMesmaFaturaSinistro.Any(SeletorPorValor))
                    linhasAntigasComMesmaFaturaSinistro = linhasAntigasComMesmaFaturaSinistro.Where(SeletorPorValor).ToList();
                
                if (linhasAntigasComMesmaFaturaSinistro.Any())
                {
                    var valoresNovos = linhaNova.ExtrairValores();
                    
                    foreach (var linhaAntigasComMesmaFatura in linhasAntigasComMesmaFaturaSinistro)
                    {
                        var listaDiferencaDaLinhas = new List<string>();
                        var listaDiferencaIgnoradaDaLinhas = new List<string>();
                        
                        var valoresAntigos = linhaAntigasComMesmaFatura.ExtrairValores();
                        
                        foreach (var valorAntigo in valoresAntigos)
                        {
                            var valorNovo = valoresNovos
                                .Where(x => x.Inicial == valorAntigo.Inicial)
                                .First(x => x.Coluna.Tamanho == valorAntigo.Coluna.Tamanho);

                            var textoDiferenca = $"Coluna: {valorNovo.Coluna.Nome} - Inicial: {valorNovo.Inicial + 1} - Tamanho: {valorNovo.Coluna.Tamanho} - ANTES: '{valorAntigo.Texto}' - NOVO: '{valorNovo.Texto}'";

                            if (valorNovo.Texto != valorAntigo.Texto)
                            {
                                if (valorNovo.DeveIgnoradarDiferenca(valorAntigo.Texto))
                                    listaDiferencaIgnoradaDaLinhas.Add(textoDiferenca);
                                else
                                    listaDiferencaDaLinhas.Add(textoDiferenca);
                            }
                        }


                        if (listaDiferencaIgnoradaDaLinhas.Any())
                        {
                            listaDiferencaIgnorada.Add(linhaAntigasComMesmaFatura.Linha);
                            listaDiferencaIgnorada.AddRange(listaDiferencaIgnoradaDaLinhas);
                        }
                        
                        if (listaDiferencaDaLinhas.Any())
                        {
                            listaComDiferenca.Add(linhaAntigasComMesmaFatura.Linha);
                            listaComDiferenca.AddRange(listaDiferencaDaLinhas);
                        }
                    }
                }
                else
                {
                    listaComDiferenca.Add("- Não encontrada linha no arquivo, com mesma fatura, sinitro, parTip, codigo externo");
                }

                var textoLinha = $"- Fatura: {linhaNova.Fatura}, Endosso: {linhaNova.Endosso}, Dt. Sinistro: {linhaNova.DataSinistro}, " +
                                 $"Vlr Sinistro: {linhaNova.ValorSinistro}, Vlr Prêmio: {linhaNova.ValorPremio}, ParTip: {linhaNova.ParTip}, " +
                                 $"Codigo Externo: {linhaNova.CodigoExterno}, Tipo Sinistro: {linhaNova.DescricaoTipoSinitro}, " +
                                 $"Conta Corporativo: {linhaNova.ContaCorporativo}";

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