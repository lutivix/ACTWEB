using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class Prototipo
    {
        public int ID { get; set; }
        public DateTime? Data { get; set; }
        public int SB { get; set; }
        public int OS { get; set; }
        public string PrefixoTrem { get; set; }
        public int Motivo { get; set; }
        public string Justificativa { get; set; }
        public int IDTrem { get; set; }
        public double? ParadaPlanejada { get; set; }
        public double? ParadaRealizada { get; set; }
        public double? MovimentacaoPlanejada { get; set; }
        public double? MovimentacaoRealizada { get; set; }
        public double? PatioPlanejado { get; set; }
        public double? PatioRealizado { get; set; }
        public double? CirculacaoPlanejada { get; set; }
        public double? CirculacaoRealizada { get; set; }
        public DateTime registro { get; set; }
        public int Trecho { get; set; }
        public int Rota { get; set; }
        public int Subrota { get; set; }
        public char Classe { get; set; }
    }
}
