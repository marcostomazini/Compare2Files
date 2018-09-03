namespace Compare2FilesForms
{
    internal class LinhaContabilidade
    {
        public LinhaContabilidade(string linha)
        {
            Linha = linha;
        }

        public string Endosso { get { return Linha.Substring(33, 10); } }
        public string Fatura { get { return Linha.Substring(48, 10); } }
        public string ValorPremio { get { return Linha.Substring(228, 13); } }
        public string Documento { get { return Linha.Substring(352, 7); } }
        public string Sinistro { get { return Linha.Substring(467, 10); } }
        public string DataSinistro { get { return Linha.Substring(477, 10); } }
        public string ValorSinistro { get { return Linha.Substring(519, 12); } }
        public string TipoMovimentacao { get { return Linha.Substring(802, 2); } }
        public string Linha { get; private set; }


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