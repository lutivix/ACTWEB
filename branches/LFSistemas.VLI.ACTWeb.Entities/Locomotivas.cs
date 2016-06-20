using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Locomotivas
    {
        public string MCT_ID_MCT {get; set;}
        public string MCT_NOM_MCT { get; set; }
        public string MR_GRMN { get; set; }
        public string PB_ID_PB { get; set; }
        public string ME_MSG_NUM { get; set; }
        public string MCT_IND_MCI { get; set; }
        public string MCT_IND_OBC { get; set; }
        public string MCT_IND_MCR_BIN { get; set; }
        public string MCT_CNV_VERSAO { get; set; }
        public string MCT_OBC_VERSAO { get; set; }
        public string MCT_MAP_VERSAO { get; set; }
        public DateTime MCT_TIMESTAMP_MCT { get; set; }
        public DateTime MCT_DT_ATUALI_OBC { get; set; }
        public DateTime MCT_DT_ATUALI_MAP { get; set; }
        public DateTime MCT_DT_INC { get; set; }
        public string MCT_EST_HAB { get; set; }

        public string LOC_ID_NUM_LOCO { get; set; }
        public string LOC_TP_LOCO { get; set; }
        public string LOC_TP_VEIC { get; set; }


    }
}
