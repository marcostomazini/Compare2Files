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
        /// Deve ignorar caso o valor � preenchido com valor vazio
        /// A contabilidade ATUAL n�o limpa alguns campos atrav�s de uma atribui��o com valor NULO, dentro do foreach de sinistro.
        /// Assim os campos s�o preenchidos indevidamente com o valor do registro anterior.
        /// A NOVA contabilidade faz a limpeza das variaveis e assim os valores ser�o NULOS, preenchendo a coluna com valor padr�o
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
        /// Deve ignorar diferen�a caso o valor seja diferente alguns centavos.
        /// O intu�to � ignorar poss�veis diferen�as causadas por falta de arredondamento de d�zima na contabilidade ATUAL
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