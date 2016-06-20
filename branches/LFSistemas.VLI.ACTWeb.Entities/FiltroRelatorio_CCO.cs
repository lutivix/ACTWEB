using System;

namespace LFSistemas.VLI.ACTWeb.Entities
{
    public class FiltroRelatorio_CCO
    {
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Matricula { get; set; }
        public string Operador { get; set; }
        public string PostoTrabalho { get; set; }
    }
}
