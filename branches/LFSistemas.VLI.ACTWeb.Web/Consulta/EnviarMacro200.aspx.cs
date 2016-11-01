using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class EnviarMacro2001 : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Trem> dadosTrem { get; set; }
        public List<Entities.Mcts> dadosMcts { get; set; }


        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper(); 

                // - Carrega combo Trens
                ddlTrens.DataValueField = "Trem_ID";
                ddlTrens.DataTextField = "Prefixo";
                ddlTrens.DataSource = CarregarComboTrens();
                ddlTrens.DataBind();
                ddlTrens.Items.Insert(0, "Selecione o Trem!");
                ddlTrens.SelectedIndex = 0;

                // - Carrega combo Mcts
                ddlMcts.DataValueField = "MCT_ID_MCT";
                ddlMcts.DataTextField = "MCT_NOM_MCT";
                ddlMcts.DataSource = CarregarComboMcts();
                ddlMcts.DataBind();
                ddlMcts.Items.Insert(0, "Selecione o Mcts!");
                ddlMcts.SelectedIndex = 0;

            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void bntEnviar_Click(object sender, EventArgs e)
        {
            if (validaFormulario())
            {
                EnviarMacro m200 = new EnviarMacro();

                if (!rdHabilitaSV.Checked && !rdDesabilitaSV.Checked && !rdHabilitaTP.Checked && !rdDesabilitaTP.Checked)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Os campos MCT, Habilita e Desabilita são obrigatórios!' });", true);
                }
                else if (rdHabilitaSV.Checked)
                {
                    m200.MWE_TEXTO = "1008";

                    m200.MWE_NUM_MACRO = 200;
                    m200.MWE_DT_ENV = DateTime.Now;
                    m200.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                    m200.MWE_SIT_MWE = char.Parse("P");
                    m200.MWE_IND_MCR = char.Parse("B");

                    if (EnviandoMacro(m200, null, "E", lblUsuarioMatricula.Text))
                        Response.Write("<script>alert('Macro 200 enviada com sucesso!');this.window.close();</script>");
                    limpaCampos();
                }
                else if (rdDesabilitaSV.Checked)
                {
                    m200.MWE_TEXTO = "1004";

                    m200.MWE_NUM_MACRO = 200;
                    m200.MWE_DT_ENV = DateTime.Now;
                    m200.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                    m200.MWE_SIT_MWE = char.Parse("P");
                    m200.MWE_IND_MCR = char.Parse("B");

                    if (EnviandoMacro(m200, null, "E", lblUsuarioMatricula.Text))
                        Response.Write("<script>alert('Macro 200 enviada com sucesso!');this.window.close();</script>");
                    limpaCampos();
                }
                else if (rdHabilitaTP.Checked)
                {
                    m200.MWE_TEXTO = "1040";

                    m200.MWE_NUM_MACRO = 200;
                    m200.MWE_DT_ENV = DateTime.Now;
                    m200.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                    m200.MWE_SIT_MWE = char.Parse("P");
                    m200.MWE_IND_MCR = char.Parse("B");

                    if (EnviandoMacro(m200, null, "E", lblUsuarioMatricula.Text))
                        Response.Write("<script>alert('Macro 200 enviada com sucesso!');this.window.close();</script>");
                    limpaCampos();
                }
                else if (rdDesabilitaTP.Checked)
                {
                    m200.MWE_TEXTO = "1020";

                    m200.MWE_NUM_MACRO = 200;
                    m200.MWE_DT_ENV = DateTime.Now;
                    m200.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                    m200.MWE_SIT_MWE = char.Parse("P");
                    m200.MWE_IND_MCR = char.Parse("B");

                    if (EnviandoMacro(m200, null, "E", lblUsuarioMatricula.Text))
                        Response.Write("<script>alert('Macro 200 enviada com sucesso!');this.window.close();</script>");
                    limpaCampos();
                }
            }
            else Response.Write("<script>alert('Os campos MCT, Habilita e Desabilita são obrigatórios!');</script>");
        }
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            limpaCampos();
        }

        #endregion

        #region [ MÉTODOS DE SELEÇÃO DOS COMBOS ]

        // - Método muda seleção da combo Trens
        protected void ddlTrens_SelectedIndexChanged(object sender, EventArgs e)
        {
            // - Seta a combo Mcts pelo value da combo Trens
            if (ddlTrens.SelectedValue != "Selecione o Trem!")
            {
                double dados = SelecionaMctsPeloTrem(double.Parse(ddlTrens.SelectedValue));
                if (dados > 0)
                {
                    ddlMcts.DataValueField = "mct_id_mct";
                    ddlMcts.DataTextField = "mct_nom_mct";
                    ddlMcts.SelectedValue = dados.ToString();
                }
                else
                    ddlMcts.SelectedValue = "Selecione o Mcts!";
            }

        }
        // - Método nuda seleção da combo Mcts
        protected void ddlMcts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // - Seta a combo Trens pelo value da combo Mcts
            if (ddlMcts.SelectedValue != "Selecione o Mcts!")
            {
                double dados = SelecionaTemPeloMcts(double.Parse(ddlMcts.SelectedValue));
                if (dados > 0)
                {
                    ddlTrens.DataValueField = "mct_id_mct";
                    ddlTrens.DataTextField = "mct_nom_mct";
                    ddlTrens.SelectedValue = dados.ToString();
                }
                else
                    ddlTrens.SelectedValue = "Selecione o Trem!";
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected bool validaFormulario()
        {
            return ddlMcts.SelectedValue != "Selecione o Mcts!" ? true : false;
        }
        protected void limpaCampos()
        {
            ddlTrens.SelectedValue = "Selecione o Trem!";
            ddlMcts.SelectedValue = "Selecione o Mcts!";
            rdHabilitaSV.Checked = rdDesabilitaSV.Checked =
            rdHabilitaTP.Checked = rdDesabilitaTP.Checked = false;
        }


        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected List<Entities.Trem> CarregarComboTrens()
        {
            var macroController = new MacroController();
            dadosTrem = macroController.ObterTodosTrens(new Entities.FiltroTrens()
            {
                loc_Id = string.Empty,
                loc_Descricao = string.Empty

            });

            return dadosTrem;
        }
        protected List<Entities.Mcts> CarregarComboMcts()
        {
            var macroController = new MacroController();
            dadosMcts = macroController.ObterTodosMcts(new Entities.FiltroMcts()
            {
                loc_Id = string.Empty,
                loc_Descricao = string.Empty

            });

            return dadosMcts;
        }
        protected double SelecionaMctsPeloTrem(double mct_id_mct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaMctsPeloTrem(mct_id_mct);
        }
        protected double SelecionaTemPeloMcts(double mct_id_mct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaTremPeloMcts(mct_id_mct);
        }

        protected bool EnviandoMacro(EnviarMacro macro, string macrolidaid, string resposta, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.EnviarMacro(macro, macrolidaid, resposta, usuarioLogado);
        }

        #endregion
    }
}