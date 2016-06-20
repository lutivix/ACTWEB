using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Log
    {
        public double Log_ID { get; set; }
        public DateTime Publicacao { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Modulo { get; set; }
        public string Matricula { get; set; }
        public string Usuario { get; set; }
        public string Identificacao_LDA { get; set; }
        public string Identificacao_ENV { get; set; }
        public string Texto { get; set; }
        public string Operacao { get; set; }
    }
}
