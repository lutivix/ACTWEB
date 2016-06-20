namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class Mcts
    {
        #region [ PROPRIEDADES ]

        public double Trem_ID { get; set; }
        public string Prefixo { get; set; }
        public string Locomotiva { get; set; }
        public double Mct { get; set; }
        public double? Cod_OF { get; set; }

        public double MCT_ID_MCT { get; set; }
        public string MCT_NOM_MCT { get; set; }

        #endregion
    }
}
