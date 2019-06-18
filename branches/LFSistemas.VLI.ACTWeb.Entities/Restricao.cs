using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Restricao
    {
        #region [ PROPRIEDADES ]

        public string Tipo { get; set; }
        public string CirculacaoID { get; set; }
        public string ProgramadaID { get; set; }
        public string RestricaoID { get; set; }
        public string Secao_Elemento { get; set; }
        public double? Secao_ElementoID { get; set; }
        public string Tipo_Restricao { get; set; }
        public double? Tipo_RestricaoID { get; set; }
        public string SubTipo_VR { get; set; }
        public double? SubTipo_VRID { get; set; }
        public DateTime? Data_Inicial { get; set; }
        public DateTime? Data_Final { get; set; }
        public double? Velocidade { get; set; }
        public decimal? Km_Inicial { get; set; }
        public decimal? Km_Final { get; set; }
        public string Lat_Inicial { get; set; }
        public string Lat_Final { get; set; }
        public string Lon_Inicial { get; set; }
        public string Lon_Final { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
        public string Responsavel { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public double? Duracao { get; set; }
        public DateTime DataIniProg { get; set; }

        
        #endregion
    }
}
