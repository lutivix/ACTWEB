using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class VMA
    {
        #region [ PROPRIEDADES ]

        public string SB_Nome { get; set; }
        public string Velocidade { get; set; }
        public string Sentido { get; set; }
        public string km_Inicial_Final { get; set; }
        public string Inicio_Fim { get; set; }
        public string Latitude_VMA { get; set; }
        public string Longitude_VMA { get; set; }
        public string Tamanho_Patio { get; set; }
        public string Status { get; set; }
        public string Reducao { get; set; }
        public string Corredor { get; set; }

        #endregion
    }
}
