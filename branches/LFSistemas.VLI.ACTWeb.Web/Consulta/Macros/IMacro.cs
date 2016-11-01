namespace LFSistemas.VLI.ACTWeb.Web.Consulta.Macros
{
    public interface IMacro
    {
        Entities.Macro EntidadeMacro { get; set; }
        string Mascara { get; set; }
        string Texto { get; set; }
        string TamanhoMascara { get; set; }
    }
}
