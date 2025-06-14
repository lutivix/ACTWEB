﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class THP
    {
        public double Trem_ID { get; set; }
        public string Grupo_ID { get; set; }
        public string Grupo { get; set; }
        public string Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public double Codigo_OS { get; set; }
        public string Prefixo { get; set; }
        public string Local { get; set; }
        public string Tempo { get; set; }
        public double Sb_ID { get; set; }
        public string TempoTotal { get; set; }
        public string Codigo { get; set; }
        public string Motivo { get; set; }
        public string Cor { get; set; }
        public TimeSpan Intervalo { get; set; }
    }
}
