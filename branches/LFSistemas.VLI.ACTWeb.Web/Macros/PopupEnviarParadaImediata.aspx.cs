using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;


namespace LFSistemas.VLI.ACTWeb.Web.Macros
{
    public partial class PopupEnviarParadaImediata : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Trem> DadosTrem { get; set; }
        public List<Mcts> DadosMcts { get; set; }


        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"], "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"], "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"], "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"], "a#3G6**@").ToUpper();

                // - Carrega combo Trens
                ddlTrens.DataValueField = "Trem_ID";
                ddlTrens.DataTextField = "Prefixo";
                ddlTrens.DataSource = CarregarComboTrens();
                ddlTrens.DataBind();
                ddlTrens.Items.Insert(0, "Selecione!");
                ddlTrens.SelectedIndex = 0;

                // - Carrega combo Mcts
                ddlMcts.DataValueField = "MCT_ID_MCT";
                ddlMcts.DataTextField = "MCT_NOM_MCT";
                ddlMcts.DataSource = CarregarComboMcts();
                ddlMcts.DataBind();
                ddlMcts.Items.Insert(0, "Selecione!");
                ddlMcts.SelectedIndex = 0;

            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkEnviar_OnClick(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (ValidaFormulario())
                {
                    var usuario = new UsuarioDAO().ObterPorLogin(lblUsuarioMatricula.Text.Trim().ToUpper(),
                        Uteis.Criptografar(txtSenha.Text.ToUpper(), "a#3G6**@").ToString());
                    if (usuario != null && usuario.Ativo_SN != "N")
                    {
                        List<EnviarMacro> itens = new List<EnviarMacro>();
                        EnviarMacro m200 = new EnviarMacro();
                        EnviarMacro m38 = new EnviarMacro();

                        m200.MWE_TEXTO = "1001";
                        m200.MWE_NUM_MACRO = 200;
                        m200.MWE_DT_ENV = DateTime.Now;
                        m200.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                        m200.MWE_SIT_MWE = char.Parse("P");
                        m200.MWE_IND_MCR = char.Parse("B");

                        itens.Add(m200);

                        m38.MWE_TEXTO = "";
                        m38.MWE_NUM_MACRO = 38;
                        m38.MWE_DT_ENV = DateTime.Now;
                        m38.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                        m38.MWE_SIT_MWE = char.Parse("P");
                        m38.MWE_IND_MCR = char.Parse("B");

                        itens.Add(m38);

                        if (EnviandoMacro(itens, null, "E", lblUsuarioMatricula.Text))
                        {
                            LimpaCampos();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!",
                                " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Parada Imediata, enviada com sucesso!' });",
                                true);
                            //Response.Write("<script>alert('Parada Imediata, enviada com sucesso!');this.window.close();</script>");
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!",
                            " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Senha inválida, ou usuário sem permissão!' });",
                            true);
                        //Response.Write("<script>alert('Senha inválida, ou usuário sem permissão!');this.window.close();</script>");
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!",
                        " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Gentileza verificar se os campos: Loco, Check e Senha foram informados, os mesmos são obrigatórios!' });",
                        true);
                }
            }
        }

        protected void lnkLimpar_OnClick(object sender, EventArgs e)
        {
            LimpaCampos();
        }


        #endregion

        #region [ MÉTODOS DE SELEÇÃO DOS COMBOS ]

        // - Método muda seleção da combo Trens
        protected void ddlTrens_SelectedIndexChanged(object sender, EventArgs e)
        {
            // - Seta a combo Mcts pelo value da combo Trens
            if (ddlTrens.SelectedValue != "Selecione!")
            {
                double dados = SelecionaMctsPeloTrem(double.Parse(ddlTrens.SelectedValue));
                if (dados > 0)
                {
                    ddlMcts.DataValueField = "mct_id_mct";
                    ddlMcts.DataTextField = "mct_nom_mct";
                    ddlMcts.SelectedValue = dados.ToString(CultureInfo.InvariantCulture);
                }
                else
                    ddlMcts.SelectedValue = "Selecione!";
            }

        }
        // - Método nuda seleção da combo Mcts
        protected void ddlMcts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // - Seta a combo Trens pelo value da combo Mcts
            if (ddlMcts.SelectedValue != "Selecione!")
            {
                double dados = SelecionaTemPeloMcts(double.Parse(ddlMcts.SelectedValue));
                if (dados > 0)
                {
                    ddlTrens.DataValueField = "mct_id_mct";
                    ddlTrens.DataTextField = "mct_nom_mct";
                    ddlTrens.SelectedValue = dados.ToString(CultureInfo.InvariantCulture);
                }
                else
                    ddlTrens.SelectedValue = "Selecione!";
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected bool ValidaFormulario()
        {
            bool Retorno = false;

            if (ddlMcts.SelectedValue != "Selecione!" && chkEnvia.Checked && txtSenha.Text.Length > 0)
                Retorno = true;

            return Retorno;
        }
        protected void LimpaCampos()
        {
            ddlTrens.SelectedValue = "Selecione!";
            ddlMcts.SelectedValue = "Selecione!";
            txtSenha.Text = string.Empty;
            chkEnvia.Checked = false;
        }


        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected List<Trem> CarregarComboTrens()
        {
            var macroController = new MacroController();
            DadosTrem = macroController.ObterTodosTrens(new FiltroTrens()
            {
                loc_Id = string.Empty,
                loc_Descricao = string.Empty

            });

            return DadosTrem;
        }
        protected List<Mcts> CarregarComboMcts()
        {
            var macroController = new MacroController();
            DadosMcts = macroController.ObterTodosMcts(new FiltroMcts()
            {
                loc_Id = string.Empty,
                loc_Descricao = string.Empty

            });

            return DadosMcts;
        }
        protected double SelecionaMctsPeloTrem(double mctIdMct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaMctsPeloTrem(mctIdMct);
        }
        protected double SelecionaTemPeloMcts(double mctIdMct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaTremPeloMcts(mctIdMct);
        }

        protected bool EnviandoMacro(List<EnviarMacro> macro, string macrolidaid, string resposta, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.EnviarMacro(macro, macrolidaid, resposta, usuarioLogado);
        }

        #endregion
    }
}