using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class FiltroRestricao
    {
        #region [ PROPRIEDADES ]

        public double? RestricaoID { get; set; }
        public string SB { get; set; } // Pátio/Estação

        public string Secao { get; set; }
        public string Tipo_Restricao { get; set; }
        public string Duracao { get; set; }
        public string Subtipo_VR { get; set; }
        public DateTime Data_Inicial { get; set; }
        public DateTime Data_Final { get; set; }
        public decimal? Km_Inicial { get; set; }
        public decimal? Km_Final { get; set; }
        public string Velocidade { get; set; }
        public string Responsavel { get; set; }
        public string Observacao { get; set; }
        public string Status { get; set; }

        #endregion
    }
}
