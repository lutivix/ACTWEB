using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Macro50
    {
        #region [ PROPRIEDADES ]

        public double ID { get; set; }
        public string Tipo { get; set; }
        public string Locomotiva { get; set; }
        public string Trem { get; set; }
        public double? CodigoOS { get; set; }
        public DateTime Horario { get; set; }
        public double NumeroMacro { get; set; }
        public string Mensagem { get; set; }
        public string Texto { get; set; }
        public double MCT { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Leitura { get; set; }
        public double Leitura_ID { get; set; }

        // [ CAMPOS USADOS NO RELATÓRIO ]
        public string Operador { get; set; }
        public string Tempo_Leitura { get; set; }
        public string Tempo_Resposta { get; set; }
        public string Corredor { get; set; }
        public string Lida { get; set; }
        public string Respondida { get; set; }

        #endregion
    }
}
