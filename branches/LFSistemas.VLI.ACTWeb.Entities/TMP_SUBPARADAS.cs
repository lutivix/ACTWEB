using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class TMP_SUBPARADAS
    {
        #region [ PROPRIEDADES ]

        public double UTP_ID { get; set; }
        public double UTPS_ID { get; set; }
        public double COD_MOTIVO { get; set; }
        public string Motivo { get; set; }
        public DateTime DT_INI_PARADA { get; set; }
        public DateTime DT_FIM_PARADA { get; set; }
        public double TempoSubparada { get; set; }
        public double USU_ID { get; set; }
        public string Matricula { get; set; }
        public DateTime DT_REGISTRO { get; set; }

        #endregion
    }
}
