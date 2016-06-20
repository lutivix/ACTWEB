using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class GravaLog
    {
        #region [ PROPRIEDADES ]

        public DateTime DATA_HORA { get; set; }
        public string MATRICULA { get; set; }
        public string MACRO { get; set; }
        public string TEXTO { get; set; }

        #endregion
    }
}
