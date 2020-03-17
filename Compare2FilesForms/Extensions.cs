using System.Collections;
using System.Text;

namespace Compare2FilesForms
{
    public static class Extensions
    {
        public static string PrepareToCompare(this string linha)
        {
            const int ultimoCaracterNomeSegurado = 130;
            const int ultimoCaracterNomeFavorecido = 619;

            var sb = new StringBuilder(linha)
            {
                [ultimoCaracterNomeSegurado] = ' ', [ultimoCaracterNomeFavorecido] = ' '
            };

            return sb.ToString().ToUpper();
        }
    }
}
