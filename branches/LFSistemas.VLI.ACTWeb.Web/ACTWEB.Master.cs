using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        #region [ DADOS DE APOIO ]

        protected void lnkAbreviar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Abreviaturas.aspx");
        }
        protected void lnkBanners_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Banners.aspx");
        }
        protected void lnkCorredores_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Importa_Corredores.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkDisplay_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDisplay.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkDownloads_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkLogs_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmLogs.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkMeta_PCTM_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkPerfis_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Perfis.aspx");
        }

        protected void lnkMaquinistas_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/Cadastro/Cadastro_Maquinistas.aspx");
            Response.Redirect("/Consulta/ConsultaMaquinistas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        protected void lnkRelatoriosPGOF_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/RelatoriosPGOF.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        protected void lnkMetaTempo_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/Cadastro/Cadastro_MetaTempo.aspx");
            Response.Redirect("/Consulta/ConsultaMetaTempo.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        protected void lnkQuadroTracao_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/Cadastro/Cadastro_QuadroTracao.aspx");
            Response.Redirect("/Consulta/ConsultaQuadroTracao.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        protected void lnkTempoConfiab_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/Cadastro/Cadastro_TempoConfiab.aspx");
            Response.Redirect("/Consulta/ConsultaTempoConfiab.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        protected void lnkPlus_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Plus.aspx");
        }
         
        protected void lnkVelocidade_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_VelocidadePrefixo.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        protected void lnkGOP_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://172.21.26.23/GOP/module/', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");

        }
        protected void lnkEgraph_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://172.21.26.44:8080/e-graph-server/webGraphics/index', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");

        }

        #region [ ALARMES ]
        protected void lnkRelatorioAlarmes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/RelatoriosAlarmes.aspx?");
        }

        #endregion

        #region [ INDICADORES ]

        protected void lnkProjetosFcaNaLf_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=8ZsD5PZ6SEKMY9hZ~DY8qg&si=6.295390213.1412357&df=351.92.0&rp=1&xf=1&el=0&CSig=45E83206FB0D5B06D7A4CD76B3231F384E8ED7D2', 'ACTWEB - Projetos FCA na LF Sistemas', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkProjetosFcaNaFca_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.openf('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=hE12Ekv81U6UFcG6CfbQew&si=6.295391729.1412357&df=351.435925.1412357&rp=1&xf=1&el=0&CSig=190CAF029CE411C7F864905C42B747BC2CE30CA7', 'ACTWEB - Projetos FCA na FCA', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }
        protected void lnkTodosProjetos_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=ml8K_qf7I0eostm66UQing&si=6.289261473.1412357&df=351.435937.1412357&rp=1&xf=1&el=0&CSig=170DF04004444797E0AE31041CB4BFE0B94FE2D8', 'ACTWEB - Todos os Projetos FCA', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }

        #endregion

        #region [ LOCOMOTIVAS ]

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
        protected void lnkImportaOBC_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Importa_OBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #region [ MACROS ]

        protected void lnkMacroConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/Macro.aspx");
        }
        protected void lnkMacro50_Click(object sender, EventArgs e)
        {
            //Response.Write("<script> " +
            //                "   var wOpen; " +
            //                "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
            //                "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
            //                "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
            //                "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
            //                "   wOpen = window.open('/Consulta/ConsultaMacro50.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
            //                "   wOpen.focus(); " +
            //                "   wOpen.moveTo( 0, 0 ); " +
            //                "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
            //                "</script>");

            //Response.Redirect("/Consulta/Cabines.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

            //Response.Redirect("/Consulta/Cabines.aspx");

            //Response.Write("<script>window.open('/Consulta/Cabines.aspx', 'popup','height=full, width=full'); </script>");


            Response.Write("<script> " +
                             "   var wOpen; " +
                             "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                             "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                             "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                             "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                             "   wOpen = window.open('/Consulta/Cabines.aspx', 'popup', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
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

        protected void lnkParadaImediata_OnClick(object sender, EventArgs e)
        {
            Response.Write("<script>window.open('/Macros/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
        }

        #endregion

        #region [ PAINEIS ]

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
        protected void lnkPainelCCONovo_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelCCONovo.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
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
        protected void lnkPainelEFVM_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=yes,menubar=yes,scrollbars=yes,resizable=yes,toolbar=yes'; " +
                            "   sOptions = sOptions + ',width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ',height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ',screenX=0,screenY=0,left=0,top=0'; " +
                            "   wOpen = window.open(' /Painel/PainelEFVM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }

        #endregion

        #region [ RÁDIOS ]

        protected void lnkConsultaRadios_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaRadios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        #endregion

        #region [ RELATÓRIOS ]

        protected void lnkRelatorioCCO_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/CCO.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelatorioMACROS_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/Macros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkPCTM_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        protected void lnkTHP_Relatorios_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/THP/popupTHP_Relatorios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }

        #endregion

        #region [ RESTRIÇÕES ]

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

        #endregion

        #region [ SISTEMAS ]

        protected void lnkMenus_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Menus.aspx");
        }
        protected void lnkSubmenus_Click(object sender, EventArgs e)
        {
            Response.Redirect("/DadosApoio/Consultar_Submenus.aspx");
        }

        #endregion

        #region [ TELECOMANDADAS ]

        protected void lnkAlarmes_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaAlarmesTelecomandadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelatorioP7_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/RelatorioAlarmesTelecomandadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #region [ THP ]

        protected void lnkTHP_Click(object sender, EventArgs e)
        {
            Response.Redirect("/THP/ConsultaTHP.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkAlocacaoProgramada_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarAlocacaoProgramada.aspx?");
        }
        protected void lnkGirloLocomotivas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarGiroLocomotivas.aspx?");
        }

        protected void lnkParadaPosicionamento_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaAlarmesParadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        //protected void lnkTHP_Subparadas_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("/THP/ConsultaTHP_Subparadas.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        //}

        #endregion

        #region [ TERMÔMETROS ]

        protected void lnkTermometrosConsultar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaTermometros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }
        protected void lnkTermometrosRelatorio_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/frmTermometros.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #region [ TRENS ]
        protected void lnkUltLocalizacao_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Consulta/popupTrensOnline.aspx', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }

        protected void lnkTempoParadaConfirmacao_Click(object sender, EventArgs e)
        {
            Response.Redirect("/THP/ConsultaTHP_Confirmacao.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #region [ USUÁRIOS ]

        protected void lnkUsuariosACT_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkUsuarios_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/Usuario.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkUsuarios_Aut_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/UsuariosAutorizados.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkAcessos_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Desenvolvimento.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkEstatisticas_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Desenvolvimento.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }



        #endregion

        #region [ VMA ]

        protected void lnkConsultaVMA_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaVMA.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkRelarorioVMA_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Relatorios/VMA.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

        #endregion

        #region [ LDL DE PÁTIOS ]
        protected void lnkLdlPatios_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://actldl.fcacco.br/'); </script>");
        }

        #endregion

        #region [ WAYSIDE ]
        protected void lnkFleetONE_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://actwsd.fcacco.br/fleetone'); </script>");
        }

        protected void lnkMonitoramentoWayside_Click(object sender, EventArgs e)
        {
            Response.Write("<script> window.open('http://actmat.fcacco.br/'); </script>");
        }

        #endregion

        #region [ PAINEL VP ]

        protected void lnkRelVP_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/VP/Consultar_Faixas.aspx");

            Response.Write("<script> " +
                             "   var wOpen; " +
                             "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                             "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                             "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                             "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                             "   wOpen = window.open('/VP/Consultar_Faixas.aspx', 'popup', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                             "   wOpen.focus(); " +
                             "   wOpen.moveTo( 0, 0 ); " +
                             "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                             "</script>");
        }

        protected void lnkPainelVP_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                             "   var wOpen; " +
                             "   sOptions = 'status=no, menubar=no, scrollbars=no, resizable=no, toolbar=no'; " +
                             "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                             "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                             "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                             "   wOpen = window.open('/PainelVP/', '', 'scrollbars=no, resizable=no, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                             "   wOpen.focus(); " +
                             "   wOpen.moveTo( 0, 0 ); " +
                             "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                             "</script>");                             
            //System.Diagnostics.Process.Start("http://localhost:9868/PainelVP/");
            
            //Response.Write("<script> window.open('https://app2.clarizen.com/Clarizen/Ext/WidgetRoadmapPage.aspx?wt=Roadmap&uid=9.6097377.1412357&wid=8ZsD5PZ6SEKMY9hZ~DY8qg&si=6.295390213.1412357&df=351.92.0&rp=1&xf=1&el=0&CSig=45E83206FB0D5B06D7A4CD76B3231F384E8ED7D2', 'ACTWEB - Projetos FCA na LF Sistemas', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); </script>");
        }

        protected void lnkPainelMaquinas_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                             "   var wOpen; " +
                             "   sOptions = 'status=no, menubar=no, scrollbars=no, resizable=no, toolbar=no'; " +
                             "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                             "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                             "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                             "   wOpen = window.open('/PainelMaquinas/', '', 'scrollbars=no, resizable=no, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                             "   wOpen.focus(); " +
                             "   wOpen.moveTo( 0, 0 ); " +
                             "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                             "</script>");

        }
        

        #endregion

        protected void lnkAlteraSenha_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/UsuarioSenha.aspx?matricula=" + Uteis.Criptografar(ulMatricula, "a#3G6**@") + "&flag=alterasenha&lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkDadosdoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/Usuario.aspx?matricula=" + Uteis.Criptografar(ulMatricula, "a#3G6**@") + "&flag=dadosusuario&lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        protected void lnkDadosdoUsuarioACT_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/UsuarioACT.aspx?matricula=" + Uteis.Criptografar(ulMatricula, "a#3G6**@") + "&flag=dadosusuario&lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void ControleDeNivelAcesso()
        {
            if (usuario.Perfil_ID == "1") // Perfil: ADMINISTRADOR - ADM
            {
                sub_consultaVMA_consulta.Visible = sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnRestricoes.Visible = mnLocomotivas.Visible =
                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = mnGOP.Visible = mnVMA.Visible = mnTelecomandadas.Visible = sub_relatorio_CCO.Visible =
                lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_interdicoes.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_VMA.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRadios.Visible = mnPgof.Visible = 
                sub_pgof_RelatoriosPGOF.Visible = sub_dadosapoio_plus.Visible = sub_macros_macrofrota.Visible = mnTHP.Visible = sub_thp_consultar.Visible = 
                sub_thp_THP_Relatorios.Visible = sub_macros_parada_imediata.Visible = sub_baixada.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = 
                sub_consultarusuariosautorizados.Visible = true;

                mnSistema.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = 
                sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = false;
                //sub_relatorio_MACROS.Visible =  false;

            }
            if (usuario.Perfil_ID == "2") // Perfil: PADRÃO - PAD
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnGOP.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_VMA.Visible = mnRadios.Visible = mnTHP.Visible = sub_thp_consultar.Visible =
                sub_thp_THP_Relatorios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnSistema.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible = 
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "3") // Perfil: CENTRO DE APOIO AO TREM - CAT
            {
                sub_macros_consulta.Visible = sub_macros_macro50.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnSistema.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnLocomotivas.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "4") // Perfil: CENTRO DE TOMADA DE DECISÃO / HELPDESK - CTD
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro61.Visible = mnPainel.Visible =
                mnTermometros.Visible = mnRestricoes.Visible = mnTelecomandadas.Visible = mnVMA.Visible = mnLocomotivas.Visible = sub_interdicoes.Visible =
                lnkImportaOBC.Visible = sub_relatorio_VMA.Visible = mnGOP.Visible = mnTHP.Visible = sub_thp_consultar.Visible =
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_macros_macro50.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible =
                sub_macros_macro200.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                mnPgof.Visible = sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "5") // Perfil: INSPETOR - INSP
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro200.Visible = mnVMA.Visible = mnPainel.Visible =
                mnTelecomandadas.Visible = mnTermometros.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = 
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible =
                mnUsuarios.Visible = sub_consultarusuariosautorizados.Visible = true;

                mnRestricoes.Visible = mnIndicadores.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_relatorio_CCO.Visible =
                sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = 
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = 
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = false;
            }
            if (usuario.Perfil_ID == "6") // Perfil: OPERADOR DE VIA PERMANENTE - OP VP
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnRestricoes.Visible = mnTermometros.Visible = mnVMA.Visible = sub_macros_macro61.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible =
                sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible =
                sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "7") // Perfil: OPERADOR ELETROELETRÔNICO - OP ELE
            {
                sub_macros_consulta.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = mnRestricoes.Visible = sub_restricao.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = 
                sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible =
                sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = sub_consultarusuariosautorizados.Visible = false;
            }
            if (usuario.Perfil_ID == "8") // Perfil: CENTRO DE CONTROLE DE EMERGÊNCIA - CCE
            {
                sub_macros_consulta.Visible = sub_macros_macro61.Visible = sub_consultaVMA_consulta.Visible = mnPainel.Visible = mnTelecomandadas.Visible =
                mnTermometros.Visible = sub_relatorio_VMA.Visible = mnGOP.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                mnRadios.Visible = sub_baixada.Visible = sub_macros_macrofrota.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_restricao.Visible = sub_interdicoes.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                sub_alarmes.Visible = sub_relatorio.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuariosautorizados.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "9") // Perfil: PROGRAMAÇÃO E CONTROLE DE MANUTENÇÃO - PCM
            {
                sub_macros_consulta.Visible = sub_macros_macro61.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible = mnTelecomandadas.Visible =
                sub_relatorio_VMA.Visible = mnGOP.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = mnRadios.Visible = 
                sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =  
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "10") // Perfil: ADMINISTRADOR DE VIA PERMANENTE - ADM VP
            {
                mnUsuarios.Visible = sub_macros_consulta.Visible = mnRestricoes.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible =
                sub_macros_macro61.Visible = mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                mnRadios.Visible = sub_interdicoes.Visible = sub_baixada.Visible = sub_consultarusuariosautorizados.Visible = sub_restricao.Visible =  true;

                sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible =
                sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = 
                sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "11") // Perfil: ADMINISTRADOR ELETROELETRÔNICO - ADM ELE
            {
                sub_macros_consulta.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_CCO.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnRestricoes.Visible =
                mnIndicadores.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = 
                mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = 
                sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "12") // Perfil: OPERADOR VITORIA MINAS - OP VM
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTelecomandadas.Visible = mnVMA.Visible = mnGOP.Visible =
                sub_trens_ult_localizacao.Visible = sub_cco.Visible = sub_cco_novo.Visible = submenu_itemCCO.Visible = submenu_itemCCOnovo.Visible =
                mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = mnTermometros.Visible = mnRestricoes.Visible = mnLocomotivas.Visible = sub_cco.Visible = sub_cco_novo.Visible = sub_locomotivas_trocaloco.Visible =
                sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible = mnRestricoes.Visible = mnIndicadores.Visible =
                sub_relatorio_CCO.Visible = mnGraficoTrens.Visible = sub_macros_macro61.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = sub_dadosapoio_abreviaturas.Visible =
                mnRelatorios.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "13") // Perfil: ADMINISTRADOR TELECON - ADM TEL
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnGOP.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_VMA.Visible = mnRadios.Visible = sub_macros_macro61.Visible = mnTHP.Visible = sub_thp_consultar.Visible = 
                sub_thp_THP_Relatorios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "14") // CENTRO DE APOIO AO TREM - ADM - ADM CAT
            {
                sub_macros_consulta.Visible = sub_macros_macro50.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = mnRadios.Visible =
                mnRelatorios.Visible = sub_relatorio_PCTM.Visible = sub_pgof_RelatoriosPGOF.Visible = mnPgof.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = mnRestricoes.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_dadosapoio_display.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible =
                sub_dadosapoio_banners.Visible = mnSistema.Visible = sub_macros_parada_imediata.Visible = mnDadosApoio.Visible =
                sub_dadosapoio_meta_pctm.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "15") // Perfil: SUPERVISOR - SUP
            {
                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro200.Visible = mnVMA.Visible = mnPainel.Visible = sub_macros_macro61.Visible =
                mnTelecomandadas.Visible = mnTermometros.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible =
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = sub_consultarusuariosautorizados.Visible = true;

                mnRestricoes.Visible = mnIndicadores.Visible = sub_macros_macro50.Visible = sub_relatorio_CCO.Visible =
                sub_interdicoes.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = 
                sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = 
                sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = 
                sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "16") // Perfil: ESPECIALISTA - ESP
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnGOP.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_VMA.Visible = mnRadios.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                sub_macros_parada_imediata.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnSistema.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "17") // Perfil: CENTRO DE TOMADA DE DECISÃO / LOCOMOTIVAS - CTD - LOCO
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro61.Visible = mnPainel.Visible = mnRestricoes.Visible = sub_interdicoes.Visible =
                mnTermometros.Visible = mnTelecomandadas.Visible = mnVMA.Visible = mnLocomotivas.Visible =
                lnkImportaOBC.Visible = sub_relatorio_VMA.Visible = mnGOP.Visible = mnTHP.Visible = sub_thp_consultar.Visible = 
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_macros_macro50.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible =
                sub_macros_macro200.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                mnPgof.Visible = sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_consultarusuariosautorizados.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "18") // Perfil: CENTRO DE TOMADA DE DECISÃO / LOCOMOTIVAS - CTD - VAG
            {
                sub_locomotivas_trocaloco.Visible = sub_macros_consulta.Visible = sub_macros_macro61.Visible = mnPainel.Visible = mnRestricoes.Visible = sub_interdicoes.Visible =
                mnTermometros.Visible = mnTelecomandadas.Visible = mnVMA.Visible = mnLocomotivas.Visible =
                lnkImportaOBC.Visible = sub_relatorio_VMA.Visible = mnGOP.Visible = mnTHP.Visible = sub_thp_consultar.Visible = 
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_macros_macro50.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible =
                sub_macros_macro200.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =
                mnRelatorios.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                mnPgof.Visible = sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = 
                sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_consultarusuariosautorizados.Visible =
                sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            if (usuario.Perfil_ID == "19") // Perfil: DESPACHADOR - DSP
            {
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnGOP.Visible = sub_macros_macro61.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = sub_relatorio_VMA.Visible = mnRadios.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = 
                sub_baixada.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible =
                mnRestricoes.Visible = mnRestricoes.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = sub_interdicoes.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnSistema.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = 
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_consultarusuariosautorizados.Visible = false;
            }
            if (usuario.Perfil_ID == "20") // Perfil: CENTRO DE CONTROLE MANUTENÇÃO - CCM => Deve ser igual OP ELE (id 7) //P714
            {
                /**
                sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnGOP.Visible = sub_macros_macro61.Visible = mnVMA.Visible = mnRestricoes.Visible = sub_interdicoes.Visible =
                mnTelecomandadas.Visible = sub_relatorio_VMA.Visible = mnRadios.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro61.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible =
                mnDadosApoio.Visible = mnSistema.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible =
                sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible =
                sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                 * submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = 
                 * sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = sub_consultarusuariosWeb.Visible = false;
                /**/

                sub_macros_consulta.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = sub_macros_macro61.Visible = mnVMA.Visible = sub_interdicoes.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = mnRestricoes.Visible = sub_restricao.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible =
                sub_thp_THP_Relatorios.Visible = mnRadios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible = 
                sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible =
                sub_dadosapoio_logs.Visible = mnRelatorios.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible = 
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
            //C1047 - 01/2022 - início
            if (usuario.Perfil_ID == "21") // Perfil: OPERADOR DE VIA PERMANENTE PADRÃO - OP VP P
            {
                mnRestricoes.Visible = sub_restricao.Visible = sub_interdicoes.Visible = mnUsuarios.Visible = sub_macros_consulta.Visible = mnPainel.Visible =
                mnTermometros.Visible = mnVMA.Visible = mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible =
                sub_thp_THP_Relatorios.Visible = sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_macros_macro61.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible = mnIndicadores.Visible =
                sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible =
                sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnRelatorios.Visible =
                mnLocomotivas.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible =
                sub_macros_parada_imediata.Visible = submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible =
                sub_eldoradoaraguari.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible =
                sub_alarmes_alarmes_relatorio_alarmes.Visible = sub_consultarusuariosautorizados.Visible = false;
            }
            if (usuario.Perfil_ID == "22") // Perfil: OPERADOR DE VIA PERMANENTE RONDA - OP VP R
            {
                mnRestricoes.Visible = sub_restricao.Visible = mnUsuarios.Visible = sub_macros_consulta.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible =
                mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                sub_baixada.Visible = true;

                mnUsuarios.Visible = sub_macros_macro61.Visible = sub_interdicoes.Visible = sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible = sub_dadosapoio_abreviaturas.Visible =
                sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible = sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible =
                mnRelatorios.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible = sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible =
                sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible =submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible =
                sub_barramansagarcas.Visible = sub_eldoradoaraguari.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = sub_consultarusuariosautorizados.Visible = false;
            }
            //C1074 - Novo Perfil CTD vagões/Via Permanente
            if (usuario.Perfil_ID == "23") // Perfil: CTD VAGÕES/VIA PERMANENTE ANÁLOGO CTD_VV (ANÁLOGO AO ADMINISTRADOR DE VIA PERMANENTE - ADM VP)
            {
                mnUsuarios.Visible = sub_macros_consulta.Visible = mnRestricoes.Visible = mnRestricoes.Visible = mnPainel.Visible = mnTermometros.Visible = mnVMA.Visible =
                sub_macros_macro61.Visible = mnTelecomandadas.Visible = mnGOP.Visible = sub_relatorio_VMA.Visible = mnTHP.Visible = sub_thp_consultar.Visible = sub_thp_THP_Relatorios.Visible =
                mnRadios.Visible = sub_interdicoes.Visible = sub_baixada.Visible = sub_consultarusuariosautorizados.Visible = sub_restricao.Visible = true;

                sub_locomotivas_trocaloco.Visible = sub_macros_macro50.Visible = sub_macros_macro200.Visible =
                mnIndicadores.Visible = sub_relatorio_CCO.Visible = lnkImportaOBC.Visible = mnDadosApoio.Visible =
                sub_dadosapoio_abreviaturas.Visible = sub_dadosapoio_downloads.Visible = sub_relatorio_MACROS.Visible = sub_dadosapoio_meta_pctm.Visible =
                sub_relatorio_PCTM.Visible = sub_dadosapoio_logs.Visible = mnLocomotivas.Visible = sub_macros_macrofrota.Visible =
                sub_dadosapoio_plus.Visible = mnSistema.Visible = mnPgof.Visible = sub_pgof_RelatoriosPGOF.Visible = sub_macros_parada_imediata.Visible =
                submenusub_itemCAT.Visible = submenusub_itemCTD.Visible = sub_araguariboavista.Visible = sub_barramansagarcas.Visible =
                sub_consultarusuariosWeb.Visible = sub_consultarusuarios.Visible = sub_acessos.Visible = sub_estatisticas.Visible =
                sub_eldoradoaraguari.Visible = sub_carneiromontesclaros.Visible = sub_cat.Visible = sub_alarmes_alarmes_relatorio_alarmes.Visible = false;
            }
        }

        #endregion


        
    }
}