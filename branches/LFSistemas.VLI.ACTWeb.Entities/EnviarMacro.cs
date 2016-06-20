using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class EnviarMacro
    {
        #region [ PROPRIEDADES ]

        public double MWE_ID_MWE { get; set; }
        public double MWE_NUM_MACRO { get; set; }
        public DateTime MWE_DT_ENV { get; set; }
        public string MWE_TEXTO { get; set; }
        public double MWE_ID_MCT { get; set; }
        public char MWE_SIT_MWE { get; set; }
        public char MWE_IND_MCR { get; set; }

        #endregion
    }
}
