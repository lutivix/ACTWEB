using System;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public interface IMacro50
    {
        string tipo { get; set; }
        double identificador_tag_lda { get; set; }
        double identificador_lda { get; set; }
        double identificador_env { get; set; }
        double numeroMacro { get; set; }
        string mascara { get; set; }
        string mascara2 { get; set; }
        string maleta { get; set; }
        string texto { get; set; }
        string loco { get; set; }
        double mct { get; set; }
        string referencia { get; set; }
        string latitude { get; set; }
        string longitude { get; set; }
        string uPosicionamento { get; set; }
        DateTime horario { get; set; }
        double codigoOS { get; set; }
        string trem { get; set; }
        string tag_leitura { get; set; }
    }
}
