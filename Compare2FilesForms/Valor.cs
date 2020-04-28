using System;
using System.Windows.Forms;

namespace Compare2FilesForms
{
    public class Valor
    {
        public Coluna Coluna { get; set; }
        public int Inicial { get; set; }
        public string Texto { get; set; }

        public bool DeveIgnoradarDiferenca(Valor valorComparacao)
        {
            if (Coluna.ValorDiferencaMinima > 0)
            {
                var valorDouble = GetValorDouble();
                var valorDoubleComparacao = valorComparacao.GetValorDouble();

                if (Math.Round(Math.Abs(valorDouble - valorDoubleComparacao), 2) <= Coluna.ValorDiferencaMinima)
                    return true;
            }

            return false;
        }

        private double GetValorDouble()
        {
            return Convert.ToDouble(Texto
                .Replace("-", string.Empty)
                .Replace(".", ","));
        }
    }
}