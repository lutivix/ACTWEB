using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class FiltroInterdicao
    {
        #region [ PROPRIEDADES ]

        public double? Autorizacao { get; set; }
        public DateTime? Data_Inicial { get; set; }
        public DateTime? Data_Final { get; set; }
        public double? Situacao { get; set; }
        public double? Secao { get; set; }
        public decimal? km { get; set; }
        public string Observacao { get; set; }
        public string Ativo_SN { get; set; }

        #endregion
    }
}
