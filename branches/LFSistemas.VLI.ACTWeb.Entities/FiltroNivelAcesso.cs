namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por filtrar usuários
    /// </summary>
    public class FiltroNivelAcesso
    {
        #region [ PROPRIEDADES ]

        public int NivelAcessoId { get; set; }
        public string NivelAcessoDescricao { get; set; }
        public string MatriculaUsuario { get; set; }
        public string NomeUsuario { get; set; }

        #endregion
    }
}
