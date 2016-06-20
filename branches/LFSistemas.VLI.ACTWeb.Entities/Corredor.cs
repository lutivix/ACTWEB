using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Corredor
    {
        public double MR_ID { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public DateTime Horario { get; set; }
        public string Nome { get; set; }
        public string KM { get; set; }
    }
}
