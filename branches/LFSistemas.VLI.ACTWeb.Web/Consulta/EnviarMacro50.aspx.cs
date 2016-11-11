using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class EnviarMacro501 : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string _identificador_lda { get; set; }
        public string _identificador_tag_lda { get; set; }
        public string _tipo { get; set; }
        public string _macro { get; set; }
        public string _mct { get; set; }
        public string _trem { get; set; }
        public string _resposta { get; set; }

        public List<Entities.Trem> dadosTrem { get; set; }
        public List<Entities.Mcts> dadosMcts { get; set; }

        AbreviaturasController abreviar = new AbreviaturasController();

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

                _identificador_lda = Request.QueryString["identificador_lda"] != null ? Request.QueryString["identificador_lda"] : null;
                _identificador_tag_lda = Request.QueryString["identificador_tag_lda"] != null ? Request.QueryString["identificador_tag_lda"] : null;

                _tipo = Request.QueryString["tipo"] != null ? Request.QueryString["tipo"] : null;
                _macro = Request.QueryString["macro"] != null ? Request.QueryString["macro"] : null;
                _mct = Request.QueryString["mct"] != null ? Request.QueryString["mct"] : null;
                _trem = Request.QueryString["trem"] != null ? Request.QueryString["trem"] : null;
                

                txtIdentificador_lda.Text = _identificador_lda;
                txtIdentificador_tag_lda.Text = _identificador_tag_lda;
                carregarCombos();

                if (_mct != null)
                {
                    ddlTrens.SelectedValue = SelecionaTemPeloMcts(double.Parse(_mct)).ToString();
                    ddlMcts.SelectedValue = SelecionaMctsPeloTrem(_trem).ToString();
                    ddlMcts.Enabled = ddlTrens.Enabled = false;

                    txtMensagem.Focus();
                }
                else
                    ddlMcts.Focus();

                Uteis.abreviados = abreviar.ObterTodos();
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void bntEnviar_Click(object sender, EventArgs e)
        {
            if (validaFormulario())
            {
                if (txtMensagem.Text.Length > 0)
                {
                    var textoAbreviado = Uteis.RetirarAcentosCaracteresEspeciais(Uteis.ObterAbreviado(txtMensagem.Text).Trim());

                    _resposta = Request.QueryString["resposta"] != null ? Request.QueryString["resposta"] : "E";

                    string texto = txtMensagem.Text.Length > 190 ? Uteis.FormataTextoMacro(50, lblUsuarioMaleta.Text, textoAbreviado.Substring(0, 190)) : Uteis.FormataTextoMacro(50, lblUsuarioMaleta.Text, textoAbreviado);

                    List<EnviarMacro> itens = new List<EnviarMacro>();
                    EnviarMacro m50 = new EnviarMacro();
                    m50.MWE_NUM_MACRO = 50;
                    m50.MWE_DT_ENV = DateTime.Now;
                    m50.MWE_TEXTO = texto;
                    m50.MWE_ID_MCT = double.Parse(ddlMcts.SelectedValue);
                    m50.MWE_SIT_MWE = char.Parse("P");
                    m50.MWE_IND_MCR = char.Parse("N");

                    itens.Add(m50);

                    if (EnviandoMacro(itens, txtIdentificador_lda.Text, _resposta, lblUsuarioMatricula.Text))
                        Response.Write("<script>alert('Macro 50 enviada com sucesso, por " + lblUsuarioMatricula.Text + " - " + lblUsuarioPerfil.Text + "'); this.window.close();</script>");

                    if (_resposta == "R")
                    {
                        if (MudaTagLeituraParaR("R", double.Parse(txtIdentificador_tag_lda.Text), txtIdentificador_lda.Text, texto, lblUsuarioMatricula.Text))
                        { }
                    }
                }

                limpaCampos();
            }
            else
            {
                Response.Write("<script>alert('Os campos MCT e Mensagem são obrigatórios! ');</script>");
            }
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

        protected void carregarCombos()
        {
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
        protected bool validaFormulario()
        {
            return ddlMcts.SelectedValue != "Selecione o Mcts!" ? txtMensagem.Text != string.Empty ? true : false : false;
        }
        protected void limpaCampos()
        {
            ddlTrens.SelectedValue = "Selecione o Trem!";
            ddlMcts.SelectedValue = "Selecione o Mcts!";
            txtMensagem.Text = string.Empty;
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
        protected double SelecionaMctsPeloTrem(string trem)
        {
            var macroController = new MacroController();
            return macroController.SelecionaMctsPeloTrem(trem);
        }
        protected double SelecionaTemPeloMcts(double mct_id_mct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaTremPeloMcts(mct_id_mct);
        }

        /// <summary>
        /// Envia a macro
        /// </summary>
        /// <param name="macro">Objeto macro a ser enviado</param>
        /// <param name="identificador_lda">[ string ] - Identificador da macro lida</param>
        /// <param name="tipo">[ string ]: - Tipo: E = Enviando Macro | R = Respondendo Macro</param>
        /// <returns>Retorna "true" se a macro foi enviada com sucesso ou "false" caso contrário</returns>
        protected bool EnviandoMacro(List<EnviarMacro> macros, string identificador_lda, string resposta, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.EnviarMacro(macros, identificador_lda, resposta, usuarioLogado);
        }

        /// <summary>
        /// Altera a tag de leitura da macro lida para R
        /// </summary>
        /// <param name="leituraid">[ string ]: - Identificador da tag de leitura</param>
        /// <param name="identificador_lda">[ string ]: - Identificador da macro lida</param>
        /// <param name="texto">[ string ]: - Texto do log</param>
        /// <returns>Retorna "true" se a tag leitura foi alterada pra R ou "false" caso contrário</returns>
        protected bool MudaTagLeituraParaR(string tipo, double identificador_tag_lda, string identificador_lda, string texto, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.MudaTagLeituraParaR(tipo, identificador_tag_lda, identificador_lda, texto, usuarioLogado);
        }


        #endregion
    }
}