using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using LFSistemas.VLI.ACTWeb.Web.Cadastro;
using LFSistemas.VLI.ACTWeb.Web.Consulta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web
{
    public partial class ACTWEB : System.Web.UI.MasterPage
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

        public List<Entities.Display> itens { get; set; }

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = Usuario.Nome.ToString();
            ulMatricula = Usuario.Matricula.ToString();
            ulPerfil = Usuario.Perfil_Abreviado.ToString();
            ulMaleta = Usuario.CodigoMaleta.ToString();

            if (!Page.IsPostBack)
            {
                DateTime dtIni = DateTime.Now.AddDays(-30);
                DateTime dtFim = DateTime.Now.AddDays(-1);

                dtIni = (dtIni.AddHours(-dtIni.Hour).AddMinutes(-dtIni.Minute).AddSeconds(-dtIni.Second));
                dtFim = (dtFim.AddHours((23 - dtFim.Hour)).AddMinutes((60 - dtFim.Minute)).AddSeconds(((60 * 60 * 24) - dtFim.Second - 1)));

                lblQtdeAcessos.Text = string.Format("{0}", acessos.ObterQtdeAcessos(ulMatricula, dtIni, dtFim).ToString());

                this.lblPerfilUsuarioLogado.Text = ulPerfil.ToUpper();
                this.LabelDataAtual.Text = DateTime.Now.ToString("dd/MM/yyyy");

                this.lblUsuarioLogado.Text = ulNome.Length > 11 ? ulNome.Substring(0, 11) : Usuario.Nome;
                ControleDeNivelAcesso();

                var displayController = new DisplayController();
                string mensagem = string.Empty;
                itens = displayController.ObterTodosPorFiltro(new Display()
                {
                    Mensagem = null,
                    Data = null,
                    Ativo = null
                });
                if (itens.Count > 0)
                {
                    mensagem = String.Join(mensagem + "&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;", itens.Select(x => x.Mensagem.ToString()));
                }

                lblDisplay.Text = mensagem;
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkMacro50_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Consulta/ConsultaMacro50.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkMacro61_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.open('/Consulta/EnviarMacro61.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToString(), "a#3G6**@") + "&', '', 'width=680, height=670, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
        }
        protected void lnkMacro200_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.open('/Consulta/EnviarMacro200.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
        }
        protected void lnkMacroFrota_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Enviar/MacroFrota.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkLocomotivas_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Locomotivas/popupLocomotivas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkRestricoes_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Restricoes/popupRestricoes.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkRelatorio_CCO_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Relatorios/pupupCCO.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkPainelCCO_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelCCO.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkPainelBaixada_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelBaixada.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkProjetosFcaNaLf_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=8ZsD5PZ6SEKMY9hZ~DY8qg&si=6.295390213.1412357&df=351.92.0&rp=1&xf=1&el=0&CSig=45E83206FB0D5B06D7A4CD76B3231F384E8ED7D2', 'ACTWEB - Projetos FCA na LF Sistemas', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkProjetosFcaNaFca_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=hE12Ekv81U6UFcG6CfbQew&si=6.295391729.1412357&df=351.435925.1412357&rp=1&xf=1&el=0&CSig=190CAF029CE411C7F864905C42B747BC2CE30CA7', 'ACTWEB - Projetos FCA na FCA', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkTodosProjetos_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=ml8K_qf7I0eostm66UQing&si=6.289261473.1412357&df=351.435937.1412357&rp=1&xf=1&el=0&CSig=170DF04004444797E0AE31041CB4BFE0B94FE2D8', 'ACTWEB - Todos os Projetos FCA', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkGOP_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://172.21.26.23/GOP/module/', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");

        }
        protected void lnkEldoradoAraguari_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelEldoradoAraguari.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkAraguariBoavista_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelAraguariBoaVista.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkBarraMansaGarcas_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelBarraMansaGarças.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkCarneiroMontesClaros_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelGen.CarneiroMontesClaros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");

        }
        protected void lnkCat_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                              "   var wOpen; " +
                              "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                              "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                              "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                              "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                              "   wOpen = window.open(' /Painel/PainelCat.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                              "   wOpen.focus(); " +
                              "   wOpen.moveTo( 0, 0 ); " +
                              "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                              "</script>");

        }
        protected void lnkAlarmes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaAlarmesTelecomandadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelatorioP7_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/RelatorioAlarmesTelecomandadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkAlteraSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Usuario.aspx?matricula=" + Uteis.Criptografar(ulMatricula, "a#3G6**@") + "&flag=alterasenha&lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkDadosdoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Usuario.aspx?matricula=" + Uteis.Criptografar(ulMatricula, "a#3G6**@") + "&flag=dadosusuario&lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkConsultaOBC_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://actweb1/cco/IND_MCT_OBC/', 'ACTWEB - Todos os Projetos FCA', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkConsultaVMA_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaVMA.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkConsultaRadios_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaRadios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkRelatorioVMA_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Desenvolvimento.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelatorioCCO_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/CCO.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkPCTM_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkAcessos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Desenvolvimento.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkEstatisticas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Desenvolvimento.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/Usuario.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkMetaframe_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://172.21.26.40/', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");

        }
        protected void lnkEgraph_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://172.21.26.44:8080/e-graph-server/webGraphics/index', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");

        }
        protected void lnkInterdicoes_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                                        "   var wOpen; " +
                                        "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                                        "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                                        "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                                        "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                                        "   wOpen = window.open('/Restricoes/popupLDL.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                                        "   wOpen.focus(); " +
                                        "   wOpen.moveTo( 0, 0 ); " +
                                        "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                                        "</script>");
        }
        protected void lnkImportaOBC_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Importa_OBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkAbreviar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/Abreviaturas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void LinkOBC_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaOBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkBanners_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaBanners.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelarorioVMA_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/VMA.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkDownloads_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkMeta_PCTM_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelatorioMACROS_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/Macros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkLogs_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmLogs.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkMacroConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/Macro.aspx");
        }
        protected void lnkTermometrosConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaTermometros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkTermometrosRelatorio_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmTermometros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkPlus_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Plus.aspx");
        }
        protected void lnkTHP_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaTHP.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        protected void lnkGirloLocomotivas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarGiroLocomotivas.aspx?");
        }
        protected void lnkAlocacaoProgramada_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarAlocacaoProgramada.aspx?");
        }


        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void ControleDeNivelAcesso()
        {
            if (usuario.Perfil_ID == "1") // Perfil: Administrador
            {
                sub_consultaVMA_consulta.Visible = sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnRestricoes.Visible = mnLocomotivas.Visible =
                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = mnSavi.Visible = mnConsultaVMA.Visible = mnTelecomandadas.Visible = sub_relatorio_CCO.Visible =
                submenusub_itemCAT.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_interdicoes.Visible =  sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_VMA.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnControleRadios.Visible =
                sub_dadosapoio_plus.Visible = sub_macros_macrofrota.Visible = mnTHP.Visible = true;

                sub_relatorio_MACROS.Visible =  false;

            }
            if (usuario.Perfil_ID == "2") // Perfil: Padrão
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnSavi.Visible = sub_macros_macro61.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnControleRadios.Visible = mnTHP.Visible =  true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "3") // Perfil: CAT
            {
                sub_macros_consulta.Visible = sub_macros_macro50.Visible = mnPainel.Visible = mnTermometros.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = submenusub_itemCAT.Visible = mnSavi.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible =  sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnLocomotivas.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "4") // Perfil: CTD ou HelpDesk
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro61.Visible =  mnPainel.Visible =
                mnTermometros.Visible = mnTelecomandadas.Visible = mnConsultaVMA.Visible = mnLocomotivas.Visible = submenusub_itemCAT.Visible =
                lnkImportaOBC.Visible = sub_relatorio_VMA.Visible = mnSavi.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnUsuarios.Visible = mnRestricoes.Visible = sub_macros_macro50.Visible = mnIndicadores.Visible =  sub_relatorio_CCO.Visible =
                sub_interdicoes.Visible = sub_macros_macro200.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = 
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "5") // Perfil: Inspetor
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro200.Visible = mnConsultaVMA.Visible = mnPainel.Visible =
                mnTelecomandadas.Visible = mnTermometros.Visible = submenusub_itemCAT.Visible = mnSavi.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnRestricoes.Visible = mnIndicadores.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_relatorio_CCO.Visible = 
                sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = 
                mnUsuarios.Visible = mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "6") // Perfil: OP VP
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnRestricoes.Visible = mnTermometros.Visible = mnConsultaVMA.Visible = sub_macros_macro61.Visible =
                mnTelecomandadas.Visible = mnSavi.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible =
                sub_relatorio_CCO.Visible = sub_interdicoes.Visible = mnTelecomandadas.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = 
                sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =  mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "7") // Perfil: OP ELE
            {
                sub_macros_consulta.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = sub_macros_macro61.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = submenusub_itemCAT.Visible = mnSavi.Visible = mnRestricoes.Visible = sub_restricao.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible =
                mnControleRadios.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible = 
                sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible =  mnRelatorios.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "8") // Perfil: CCE
            {
                sub_macros_consulta.Visible = sub_macros_macro61.Visible = sub_consultaVMA_consulta.Visible = mnPainel.Visible =
                mnTermometros.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnSavi.Visible = sub_relatorio_MACROS.Visible = mnTHP.Visible =
                mnControleRadios.Visible = true;

                sub_locomotivas_trocaloco.Visible = sub_restricao.Visible = sub_interdicoes.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                sub_alarmes.Visible = sub_relatorio.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnUsuarios.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =  mnLocomotivas.Visible = mnTelecomandadas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "9") // Perfil: PCM
            {
                sub_macros_consulta.Visible = sub_macros_macro61.Visible = mnPainel.Visible = mnTermometros.Visible = mnConsultaVMA.Visible = mnTelecomandadas.Visible =
                sub_relatorio_VMA.Visible = mnSavi.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =  mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "10") // Perfil: ADM VP
            {
                sub_macros_consulta.Visible = mnRestricoes.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = mnConsultaVMA.Visible =
                sub_macros_macro61.Visible = mnTelecomandadas.Visible = mnSavi.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible =
                mnControleRadios.Visible = sub_interdicoes.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = 
                sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =  mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "11") // Perfil: ADM ELE
            {
                sub_macros_consulta.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = sub_macros_macro61.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_CCO.Visible = mnSavi.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible =
                mnControleRadios.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnRestricoes.Visible =
                mnIndicadores.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible =  mnLocomotivas.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "12") // Perfil: OP VM
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTelecomandadas.Visible = mnConsultaVMA.Visible = mnSavi.Visible = sub_cat.Visible = submenusub_itemCAT .Visible =
                sub_carneiromontesclaros.Visible = sub_eldoradoaraguari.Visible = sub_barramansagarcas.Visible = sub_araguariboavista.Visible = submenusub_itemCTD.Visible =
                sub_trens_online.Visible = sub_baixada.Visible = sub_cco.Visible = submenu_itemCCO.Visible = mnTHP.Visible = mnControleRadios.Visible = true;

                mnTermometros.Visible = mnRestricoes.Visible = mnLocomotivas.Visible = sub_cco.Visible = mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = 
                sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible = mnRestricoes.Visible = mnIndicadores.Visible = 
                sub_relatorio_CCO.Visible = mnGraficoTrens.Visible = sub_macros_macro61.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = 
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = 
                sub_dadosapoio_logs.Visible =  sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = sub_dadosapoio_abreviar.Visible =
                mnRelatorios.Visible = false;
            }
            if (usuario.Perfil_ID == "13") // Perfil: TEL
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnSavi.Visible = sub_macros_macro61.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = submenusub_itemCAT.Visible = sub_relatorio_VMA.Visible = mnControleRadios.Visible = sub_macros_macro61.Visible = mnTHP.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = false;
            }
            if (usuario.Perfil_ID == "14") // Perfil: CAT ADM
            {
                sub_macros_consulta.Visible = sub_macros_macro50.Visible = mnPainel.Visible = mnTermometros.Visible = mnConsultaVMA.Visible =
                mnTelecomandadas.Visible = submenusub_itemCAT.Visible = mnSavi.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = mnControleRadios.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_meta_pctm.Visible = mnRelatorios.Visible = sub_relatorio_PCTM.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                sub_dadosapoio_abreviar.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_dadosapoio_display.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = sub_dadosapoio_banners.Visible =  false;
            }
        }

        #endregion
    }
}