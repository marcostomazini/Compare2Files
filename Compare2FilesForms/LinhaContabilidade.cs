using System.Collections.Generic;

namespace Compare2FilesForms
{
    public class LinhaContabilidade
    {
        public LinhaContabilidade(string linha)
        {
            Linha = linha;
        }

        public string Endosso => Linha.Substring(33, 10);
        public string Fatura => Linha.Substring(48, 10);
        public string ValorPremio => Linha.Substring(228, 13);
        public string Documento => Linha.Substring(352, 7);
        public string DataSinistro => Linha.Substring(477, 10);
        public string ValorSinistro => Linha.Substring(519, 12);
        public string TipoSinistro => Linha.Substring(805, 2);
        public string ParTip => Linha.Substring(60, 1);
        public string CodigoExterno => Linha.Substring(795, 10);
        public string ContaCorporativo => Linha.Substring(388, 18);

        public string DescricaoTipoSinitro
        {
            get
            {
                switch (TipoSinistro)
                {
                    case "02":
                        return "CO-PAR ou Avulsa";
                    case "03":
                        return "Recurso Glosa (Própria 250)";
                    case "04":
                        return "Recurso Glosa (Próxima 260)";
                    case "05":
                        return "Movimento de acerto";
                    case "01":
                        return "Glosa 790";
                    case "00":
                        return "Sinistro/Prêmio";
                    default:
                        return "";
                }
            }
        }

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
                    Texto = Linha.Substring(inicial, coluna.Tamanho),
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
                && string.Equals(TipoSinistro, other.TipoSinistro) 
                && string.Equals(ValorPremio, other.ValorPremio) 
                && string.Equals(Documento, other.Documento)
                && string.Equals(ValorSinistro, other.ValorSinistro)
                && string.Equals(DataSinistro, other.DataSinistro)
                && string.Equals(ParTip, other.ParTip)
                && string.Equals(CodigoExterno, other.CodigoExterno);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Endosso != null ? Endosso.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ (Fatura != null ? Fatura.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TipoSinistro != null ? TipoSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValorPremio != null ? ValorPremio.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Documento != null ? Documento.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ValorSinistro != null ? ValorSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (DataSinistro != null ? DataSinistro.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ParTip != null ? ParTip.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CodigoExterno != null ? CodigoExterno.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}