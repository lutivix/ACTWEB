using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.IO;

namespace LFSistemas.VLI.ACTWeb.Web.Painel
{
    public partial class PainelCCONovo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FileInfo file_info = new FileInfo(@"D:\Actweb\Screenshot\PainelCCO\01_Painel_Full.jpg");

            lblUltimaAtualizacao.Text = file_info.LastWriteTime.ToString("dd/MM/yyyy HH:mm:ss");

            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
        }
    }
}