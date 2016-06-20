using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por mapear uma tabela do banco em um objetos
    /// </summary>
    public class NivelAcesso
    {
        #region [ PROPRIEDADES ]

        public int Id { get; set; }
        public string Nome { get; set; }
        public Boolean Ativo { get; set; }

        #endregion
    }
}
