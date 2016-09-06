using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class LocomotivasCorredor
    {
        public string Corredor_DS { get; set; }
        public int Corredor_ID { get; set; }
        public string Localidade_ID { get; set; }
        public string Localidade_DS { get; set; }
        public string ModeloLoco_ID { get; set; }
        public string ModeloLoco_DS { get; set; }
        public int QtdeLoco { get; set; }
        public int QtdeVagoes { get; set; }

        public double TempoAcumulado { get; set; }

        public Decimal TB { get; set; }

        public double Confiabilidade { get; set; }
        public string Loco_ID { get; set; }
        public string TempoProxManut { get; set; }


        public string Trem { get; set; }
        public string OS { get; set; }
        public string DataPartida { get; set; }
        public string Estado { get; set; }
        public string Licenciamento { get; set; }
        public string DataPrevisaoChegada { get; set; }
        public string Locomotivas { get; set; }

        public string UltDataManut { get; set; }
        public string TempoForaOficina { get; set; }
        public string TempoVoltarOficina { get; set; }
 

        public string Campo1 { get; set; }
        public string Campo2 { get; set; }
        public string Campo3 { get; set; }
        public string Campo4 { get; set; }
        public string Campo5 { get; set; }
        public string Campo6 { get; set; }
        public string Campo7 { get; set; }
        public string Campo8 { get; set; }
        public string Campo9 { get; set; }


        public string Titulo1 { get; set; }
        public string Titulo2 { get; set; }
        public string Titulo3 { get; set; }
        public string Titulo4 { get; set; }
        public string Titulo5 { get; set; }
        public string Titulo6 { get; set; }
        public string Titulo7 { get; set; }
        public string Titulo8 { get; set; }
        public string Titulo9 { get; set; }
    }
}
