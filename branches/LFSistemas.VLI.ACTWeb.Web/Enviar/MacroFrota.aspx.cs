using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Enviar
{
    public partial class MacroFrota : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        //public List<Radios> itens { get; set; }

        public string corredores { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        AbreviaturasController abreviar = new AbreviaturasController();

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                var pesquisa = new TremController();
                var circulando = pesquisa.ObterTodosTrensCirculando();
                if (circulando.Count > 0)
                    lblTrensCirculando.Text = string.Format("{0:0,0}", circulando.Count);
            }
        }
        #endregion

        protected void lnkEnviar_Click(object sender, EventArgs e)
        {
            var macro = new MacroPraFrota();
            var enviar = new MacroController();
            var texto = string.Empty;
            int enviados = 0;

            if (txtMensagem.Text.Length > 0)
            {
                Uteis.abreviados = abreviar.ObterTodos();

                var textoAbreviado = Uteis.ObterAbreviado(Uteis.RetirarAcentosCaracteresEspeciais(txtMensagem.Text));

                texto = textoAbreviado.Length <= 190 ? textoAbreviado : textoAbreviado.Substring(0, 190); // txtMensagem.Text.Length > 190 ? Uteis.FormataTextoMacro(50, null, textoAbreviado.Substring(0, 190)) : Uteis.FormataTextoMacro(50, null, textoAbreviado);
            }

            var pesquisa = new TremController();

            var circulando = pesquisa.ObterTodosTrensCirculando();
            if (circulando.Count > 0)
            {
                for (int i = 0; i < circulando.Count; i++)
                {
                    //if (i < 1)
                    //{
                        macro.ME_ID = new IDController().ObterProximoID("ACTPP.MENSAGENS_ENVIADAS_ID.NEXTVAL");
                        macro.MCT_ID = circulando[i].Mct_ID;
                        macro.TREM_ID = circulando[i].Trem_ID;
                        macro.PREFIXO = circulando[i].Prefixo;
                        macro.LOCO = circulando[i].Locomotiva;
                        macro.COD_OF = circulando[i].Cod_OF;
                        macro.TEXTO = texto;

                        if (enviar.EnviarMacroPraFrota(macro, lblUsuarioMatricula.Text))
                            enviados++;
                    //}
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível localizar os trens que estão circulando.' });", true);

            if (enviados > 0)
            {
                txtMensagem.Text = string.Empty;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Macro enviada para " + string.Format("{0:0,0}", enviados) + " trens.' });", true);
            }
        }
    }
}