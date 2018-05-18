using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    /// <summary>
    /// Classe responsável por filtrar macros
    /// </summary>
    public class FiltroMacro
    {
        #region [ PROPRIEDADES ]

        public string NumeroLocomotiva { get; set; }
        public string NumeroTrem { get; set; }
        public string NumeroMacro { get; set; }
        public string CodigoOS { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public int Mais { get; set; }
        public int? Espaco { get; set; }
        public string Motivo { get; set; }
        public string Expressao { get; set; }
        public string Corredores { get; set; }
        public string PrefixoTrem { get; set; }
        public bool naoLidas { get; set; }
        public string cabines { get; set; }

        #endregion
    }
}
