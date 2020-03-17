using System.Collections.Generic;

namespace Compare2FilesForms
{
    internal class LinhaContabilidade
    {
        public LinhaContabilidade(string linha)
        {
            Linha = linha;
        }

        public string Endosso => Linha.Substring(33, 10);
        public string Fatura => Linha.Substring(48, 10);
        public string ValorPremio => Linha.Substring(228, 13);
        public string Documento => Linha.Substring(352, 7);
        public string Sinistro => Linha.Substring(467, 10);
        public string DataSinistro => Linha.Substring(477, 10);
        public string ValorSinistro => Linha.Substring(519, 12);
        public string TipoMovimentacao => Linha.Substring(802, 2);

        public IList<Valor> ExtrairValores()
        {
            var listaValores = new List<Valor>();

            var inicial = 0;
            foreach (var coluna in Coluna.ListaColunasLinha)
            {
                if (inicial + coluna.Tamanho > Linha.Length)
                    break;

                var valor = new Valor
                {
                    Coluna = coluna,
                    Inicial = inicial,
                    Texto = Linha.Substring(inicial, coluna.Tamanho)
                };

                listaValores.Add(valor);

                inicial += coluna.Tamanho;
            }

            return listaValores;
        }
       
        public string Linha { get; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(LinhaContabilidade) 
                && Equals((LinhaContabilidade) obj);
        }

        protected bool Equals(LinhaContabilidade other)
        {
            return string.Equals(Endosso, other.Endosso) 
                && string.Equals(Fatura, other.Fatura) 
                && string.Equals(TipoMovimentacao, other.TipoMovimentacao) 
                && string.Equals(ValorPremio, other.ValorPremio) 
                && string.Equals(Documento, other.Documento)
                && string.Equals(Sinistro, other.Sinistro)
                && string.Equals(ValorSinistro, other.ValorSinistro)
                && string.Equals(DataSinistro, other.DataSinistro);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Endosso != null ? Endosso.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Fatura != null ? Fatura.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TipoMovimentacao != null ? TipoMovimentacao.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValorPremio != null ? ValorPremio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Documento != null ? Documento.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValorSinistro != null ? ValorSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DataSinistro != null ? DataSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Sinistro != null ? Sinistro.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}