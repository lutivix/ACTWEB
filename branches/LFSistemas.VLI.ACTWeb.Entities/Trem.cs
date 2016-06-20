namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class Trem
    {
        #region [ PROPRIEDADES ]

        public double Trem_ID { get; set; }
        public string Prefixo { get; set; }
        public string Locomotiva { get; set; }
        public double Mct_ID { get; set; }
        public double? Cod_OF { get; set; }

        #endregion
    }
}
