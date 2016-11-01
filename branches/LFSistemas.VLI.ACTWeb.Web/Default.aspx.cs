using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.IO;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class _Default : System.Web.UI.Page
    {
        #region [ ATRIBUTOS ]

        UsuarioController acessos = new UsuarioController();

        private Entities.Usuarios usuario;
        public Entities.Usuarios Usuario
        {
            get
            {
                if (this.usuario == null)
                {
                    var usuarioController = new UsuarioController();

                    this.usuario = usuarioController.ObterPorLogin(Page.User.Identity.Name);
                }

                return this.usuario;
            }
        }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<Downloads> itensDownloads { get; set; }
        public List<Slide> itens { get; set; }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!IsPostBack)
            {
                Pesquisar(null);
            }
        }

        protected void lnkLocomotivas_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Consulta/Informacao_OBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "&locos=" + Uteis.Criptografar(txtLocomotivas.Text.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");

            txtLocomotivas.Text = string.Empty;
        }

        protected void Pesquisar(string ordenacao)
        {
            var downloads = new DownloadsController();

            itensDownloads = downloads.ObterDownloadsPorFiltro(new Downloads()
            {
                Arquivo = null,
                Descricao = null,
                Modulo_do_Sistema = 1,
                Previsao_Atualizacao = null,
                Versao = null,
                Liberado_SN = null,
                Ativo_SN = "'S'"
            }, "tela_consulta", ordenacao);

            if (itensDownloads.Count > 0)
            {
                this.RepeaterItens.DataSource = itensDownloads;
                this.RepeaterItens.DataBind();
            }

            var bannersController = new BannersController();

            banners = bannersController.ObterPorFiltro(new Banner()
            {
                Ativo = "S"
            });

            if (banners.Count > 0)
            {
                rptBanners.DataSource = banners;
                rptBanners.DataBind();
            }
        }

        protected void lnkDownload_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);

            double ID = double.Parse(btn.CommandArgument);

            var pesquisar = new DownloadsController();

            var item = pesquisar.ObterDownloadsPorId(ID);

            if (File.Exists(MapPath("/download/" + item.Arquivo)))
            {

                string caminhoArquivo = (sender as LinkButton).CommandArgument;
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + item.Arquivo);
                Response.WriteFile(MapPath("/download/" + item.Arquivo));

                LogDAO.GravaLogBanco(DateTime.Now, Usuario.Matricula, "Downloads", null, null, "Usuário: " + Usuario.Nome + ", efetuou download de " + item.Descricao, Uteis.OPERACAO.Baixou.ToString());
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('O arquivo não está no servidor! Gentileza entrar em contato: plantao@grtechbr.com.br '); </script>");
            }
        }




        public List<Banner> banners { get; set; }
    }
}