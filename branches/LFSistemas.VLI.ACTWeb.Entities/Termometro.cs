using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class Termometro
    {
        #region [ PROPRIEDADES ]

        public double Termometro_ID { get; set; }
        public string Corredor_ID { get; set; }
        public string Corredor { get; set; }
        public string Estacao { get; set; }
        public string Trecho { get; set; }
        public double Temperatura_1 { get; set; }
        public double Temperatura_2 { get; set; }
        public DateTime Leitura { get; set; }
        public string Falha { get; set; }
        public string Critico { get; set; }
        public string Acao { get; set; }
        public string Status { get; set; }
        public string Ocorrencia { get; set; }
        public string Secao { get; set; }
        public double Velocidade { get; set; }

        #endregion
    }
}
