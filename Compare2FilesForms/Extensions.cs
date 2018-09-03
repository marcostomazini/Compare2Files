using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compare2FilesForms
{
    public static class Extensions
    {

        public static string PrepareToCompare(this string linha)
        {
            const int UltimoCaracterNomeSegurado = 130;
            const int ultimoCaracterNomeFavorecido = 619;

            var sb = new StringBuilder(linha);
            sb[UltimoCaracterNomeSegurado] = ' ';
            sb[ultimoCaracterNomeFavorecido] = ' ';

            return sb.ToString().ToUpper();
        }
    }
}
