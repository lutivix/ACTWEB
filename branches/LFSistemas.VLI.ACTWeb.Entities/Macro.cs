using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class Macro
    {
        #region [ PROPRIEDADES ]

        public double ID { get; set; }
        public string Tipo { get; set; }
        public string Locomotiva { get; set; }
        public string Trem { get; set; }
        public string TremID { get; set; }
        public string Prefixo7D { get; set; }
        public double CodigoOS { get; set; }
        public DateTime Horario { get; set; }
        public DateTime? Tratado { get; set; }
        public double NumeroMacro { get; set; }
        public string Texto { get; set; }
        public double MCT { get; set; }
        public string DescricaoMacro { get; set; }
        public string Mascara { get; set; }
        public string Maleta { get; set; }
        public string Origem { get; set; }
        public string Destino { get; set; }
        public double Tamanho { get; set; }
        public double Peso { get; set; }
        public double VersaoOBC { get; set; }
        public double Mapa { get; set; }
        public double TamanhoTrem { get; set; }
        public string Localizacao { get; set; }

        public string TamanhoMascara { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Corredor { get; set; }
        public string SB { get; set; }
        public string KM { get; set; }
        public string Status { get; set; }

        public DateTime Confirmacao_Leitura { get; set; }
        public string Operador { get; set; }
        public string Tempo_Leitua { get; set; }
        public string Tempo_Resposta { get; set; }
        public string Tempo_Decorrido { get; set; }

        #endregion
    }
}
