using System.Collections.Generic;

namespace Compare2FilesForms
{
    public class Coluna
    {
        public string Nome { get; }
        public int Tamanho { get; }

        public Coluna(string nome, int tamanho)
        {
            Nome = nome;
            Tamanho = tamanho;
        }

        public static IList<Coluna> ListaColunasLinha => new List<Coluna>
        {
            new Coluna("EmpresaCorporativo", 2),
            new Coluna("RegTip", 1),
            new Coluna("FilCodigoExterno", 5),
            new Coluna("Estrutura", 5),
            new Coluna("DataContabil", 10),
            new Coluna("ContratoNumero", 10),
            new Coluna("nvl(p_Endosso, p_NumeroFatura)", 10),
            new Coluna("Parcela", 5),
            new Coluna("Fatura", 10),
            new Coluna("DctTip", 1),
            new Coluna("MvtTip", 1),
            new Coluna("ParTip", 1),
            new Coluna("DataAdesao", 10),
            new Coluna("CancelamentoData", 10),
            new Coluna("p_NomeSegurado", 50),
            new Coluna("FIXO_1", 87),
            new Coluna("DataContabilSinistro", 10),
            new Coluna("Valor", 16),
            new Coluna("ZEROS_1", 64),
            new Coluna("ValorIOF", 16),
            new Coluna("ZEROS_2", 16),
            new Coluna("ZEROS_2", 2),
            new Coluna("DataVencimento", 10),
            new Coluna("CGCodigoExterno", 7),
            new Coluna("ZEROS_3", 2),
            new Coluna("ZEROS_3", 1),
            new Coluna("CodigoConvenio", 2),
            new Coluna("RegimePagamento", 23),
            new Coluna("Natureza", 1),
            new Coluna("CNPJCPF", 18),
            new Coluna("DataContabilSinistro", 10),
            new Coluna("Competencia(+ regra)", 10),
            new Coluna("DataAdesao(+ regra)", 10),
            new Coluna("OrigemCorp", 2),
            new Coluna("EstipulanteNumero", 10),
            new Coluna("ZEROS_4", 8),
            new Coluna("PME", 5),
            new Coluna("FisicaJuridica", 1),
            new Coluna("ZEROS_5", 1),
            new Coluna("AnoDataRecebimento", 4),
            new Coluna("p_Endosso(numero guia)", 10),
            new Coluna("DataAtendimento", 10),
            new Coluna("DataRecebimento", 10),
            new Coluna("DataLancamento", 10),
            new Coluna("CodigoDif", 7),
            new Coluna("FilCodigoExterno", 5),
            new Coluna("ValorGlosado(+ regra)", 15),
            new Coluna("ZEROS_5", 15),
            new Coluna("Hierarquia", 6),
            new Coluna("AnoDataVencimento", 4),
            new Coluna("Endosso", 6),
            new Coluna("00050", 5),
            new Coluna("Nome", 50),
            new Coluna("DataContabilSinistro", 10),
            new Coluna("sauliq", 6),
            new Coluna("RegimePagamento", 10),
            new Coluna("Corresponsabilidade", 2),
            new Coluna("FIXO_2", 1),
            new Coluna("DataArquivo", 10),
            new Coluna("DataEmissao", 10),
            new Coluna("NumeroPeg", 16),
            new Coluna("Valor", 16),
            new Coluna("ZEROS_6", 32),
            new Coluna("vPISVLR(-> p_Valor)", 16),
            new Coluna("vCOFINSVLR(-> p_Valor)", 16),
            new Coluna("ZEROS_6", 16),
            new Coluna("DataArquivo", 10),
            new Coluna("FIXO_2", 1),
            new Coluna("nvl(p_CCCodigoExterno, p_CCCodigoExternoPadrao)", 10),
            new Coluna("Tipo Sinistro", 2),
            new Coluna("NumeroDocumento", 15)
        };
    }
}