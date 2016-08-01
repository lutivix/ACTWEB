using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class TempoParadaConfirmacao
        {
        public string Prefixo { get; set; }
        public string OS { get; set; }
        public string Local { get; set; }
        public string InicioParada { get; set; }
        public string FimParada { get; set; }
        public string TempoParada { get; set; }
        public string ConfirmacaoDespachador { get; set; }
        public string TempoRespostaDespachador { get; set; }
        public string MotivoParadaMaquinista { get; set; }
        public string MotivoParadaDespachador { get; set; }
        public string Despachador { get; set; }
        public string PostoTrabalho { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string TextoAlarme { get; set; }

        //public string TTR { get; set; }
        //public string AlarmeVigente { get; set; }
    }
} 