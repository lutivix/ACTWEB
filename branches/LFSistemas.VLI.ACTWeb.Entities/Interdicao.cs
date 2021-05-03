using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Interdicao
    {
        #region [ PROPRIEDADES ]

        public double Solicitacao_ID_ACTWEB { get; set; }
        public double Solicitacao_ID_ACT { get; set; }
        public double? Secao_ID { get; set; }
        public string Secao_Nome { get; set; }
        public double? Tipo_Situacao_ID { get; set; }
        public string Situacao_Nome  { get; set; }
        public double? Tipo_Interdicao_ID { get; set; }
        public string Tipo_Interdicao_Nome { get; set; }
        public double? Tipo_Manutencao_ID { get; set; }
        public string Tipo_Manutencao_Nome { get; set; }
        public double? Tipo_Circulacao_ID { get; set; }
        public string Tipo_Circulacao_Nome { get; set; }
        public double Motivo_ID { get; set; }
        public string Motivo { get; set; }
        public string Motivo_Desc { get; set; }
        public double? Interdicao_Motivo { get; set; }
        public string Cod_Interdicao { get; set; }
        public string Responsavel_Matricula { get; set; }
        public string Responsavel_Nome { get; set; }
        public DateTime Data { get; set; }
        public double? Duracao_Solicitada { get; set; }
        public double? Duracao_Autorizada { get; set; }
        public decimal? Km { get; set; }
        public string Telefone_SN { get; set; }
        public string Telefone_Numero { get; set; }
        public string Radio_SN { get; set; }
        public string Macro_SN { get; set; }
        public string Macro_Numero { get; set; }
        public string Equipamentos { get; set; }
        public string Observacao { get; set; }
        public string Usuario_Logado_Matricula { get; set; }
        public string Usuario_Logado_Nome { get; set; }
        public double? Aut_Interdicao_Act { get; set; }
        public string Ativo_SN { get; set; }
        public string Cod_Ldl { get; set; }
        public string Telefone_responsavel { get; set; }
        public string Prefixo { get; set; }
        public double Cauda { get; set; }

        #endregion
    }
}
