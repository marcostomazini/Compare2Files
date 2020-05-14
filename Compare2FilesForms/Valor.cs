using System;

namespace Compare2FilesForms
{
    public class Valor
    {
        public Coluna Coluna { get; set; }
        public int Inicial { get; set; }
        public string Texto { get; set; }

        public bool DeveIgnoradarDiferenca(string valorAntigo)
        {
            return DeveIgnorarCampoValorComDiferencaMinima(valorAntigo) 
                   || DeveIgnorarCampoPreenchidoComValorPadrao(valorAntigo);
        }

        /// <summary>
        /// Deve ignorar caso o valor é preenchido com valor vazio
        /// A contabilidade ATUAL não limpa alguns campos através de uma atribuição com valor NULO, dentro do foreach de sinistro.
        /// Assim os campos são preenchidos indevidamente com o valor do registro anterior.
        /// A NOVA contabilidade faz a limpeza das variaveis e assim os valores serão NULOS, preenchendo a coluna com valor padrão
        /// </summary>
        /// <param name="valorAntigo"></param>
        /// <returns></returns>
        private bool DeveIgnorarCampoPreenchidoComValorPadrao(string valorAntigo)
        {
            if (string.IsNullOrEmpty(Coluna.TextoPadraoPermitido))
                return false;

            return valorAntigo != Texto 
                && Texto == Coluna.TextoPadraoPermitido;
        }

        /// <summary>
        /// Deve ignorar diferença caso o valor seja diferente alguns centavos.
        /// O intuíto é ignorar possíveis diferenças causadas por falta de arredondamento de dízima na contabilidade ATUAL
        /// </summary>
        /// <param name="valorAntigo"></param>
        /// <returns></returns>
        private bool DeveIgnorarCampoValorComDiferencaMinima(string valorAntigo)
        {
            if (Coluna.ValorDiferencaMinima <= 0)
                return false;
            
            var valorDouble = GetValorDouble(Texto);
            var valorDoubleComparacao = GetValorDouble(valorAntigo);

            return Math.Round(Math.Abs(valorDouble - valorDoubleComparacao), 2) <= Coluna.ValorDiferencaMinima;
        }

        private static double GetValorDouble(string valorTexto)
        {
            return Convert.ToDouble(valorTexto
                .Replace("-", string.Empty)
                .Replace(".", ","));
        }
    }
}