namespace Compare2FilesForms
{
    internal class LinhaContabilidade
    {
        private readonly string _linha;
        private readonly string _endosso;
        private readonly string _numeroFatura;
        private readonly string _valorPremio;
        private readonly string _numeroDocumento;
        private readonly string _sequenciaMovimentoSinistro;
        private readonly string _valorSinistro;

        public LinhaContabilidade(string linha)
        {
            _linha = linha;

            _endosso = linha.Substring(33, 10);
            _numeroFatura = linha.Substring(48, 10);
            _valorPremio = linha.Substring(228, 13);
            _numeroDocumento = linha.Substring(352, 7);
            _sequenciaMovimentoSinistro = linha.Substring(400, 6);
            _valorSinistro = linha.Substring(519, 12);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == typeof(LinhaContabilidade) 
                && Equals((LinhaContabilidade) obj);
        }

        protected bool Equals(LinhaContabilidade other)
        {
            return string.Equals(_endosso, other._endosso) 
                && string.Equals(_numeroFatura, other._numeroFatura) 
                && string.Equals(_valorPremio, other._valorPremio) 
                && string.Equals(_numeroDocumento, other._numeroDocumento)
                && string.Equals(_sequenciaMovimentoSinistro, other._sequenciaMovimentoSinistro)
                && string.Equals(_valorSinistro, other._valorSinistro);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _numeroFatura != null ? _numeroFatura.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (_endosso != null ? _endosso.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_valorPremio != null ? _valorPremio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_numeroDocumento != null ? _numeroDocumento.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_sequenciaMovimentoSinistro != null ? _sequenciaMovimentoSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (_valorSinistro != null ? _valorSinistro.GetHashCode() : 0);
                return hashCode;
            }
        }

        public string GetLinha()
        {
            return _linha;
        }

        public string GetValorPremio()
        {
            return _valorPremio;
        }

        public string GetNumeroFatura()
        {
            return _numeroFatura;
        }
    }
}