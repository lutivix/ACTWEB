using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Rel_THP_Filtro
    {
        #region [ FILTRO DE PESQUISA ]

        public string THP_ID { get; set; }
        public string Data_INI { get; set; }
        public string Data_FIM { get; set; }
        public string Classe { get; set; }
        public string OS { get; set; }
        public string Prefixo { get; set; }
        public string SB { get; set; }
        public string Corredor_ID { get; set; }
        public string Rota_ID { get; set; }
        public string SubRota_ID { get; set; }
        public string Grupo_ID { get; set; }
        public string Motivo_ID { get; set; }
        public bool TremEncerrado { get; set; }

        #endregion
    }
}
