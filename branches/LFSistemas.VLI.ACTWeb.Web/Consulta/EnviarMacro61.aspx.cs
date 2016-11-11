using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class EnviarMacro611 : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

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

        public string usuarioLogado { get; set; }
        public string usuarioMatricula { get; set; }
        public string usuarioPerfil { get; set; }
        public List<Entities.Trem> dadosTrem { get; set; }
        public List<Entities.Mcts> dadosMcts { get; set; }
        public double QtdeMaxima { get; set; }

        AbreviaturasController abreviar = new AbreviaturasController();
        DataTable dt = new DataTable();

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
                ddlMcts.Items.Insert(0, "Selecione a Loco!");
                ddlMcts.SelectedIndex = 0;
                Uteis.abreviados = abreviar.ObterTodos();

                ViewState["Contador"] = ListaTemporarios().ToString();
            }
            lblQtdeMaxima.Text = QunatidadeMaximaDeMacrosEnviadasPorPerfil(lblUsuarioPerfil.Text).ToString();
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        // Método utilizado para enviar mensagens (Macro) ao clicar no botão enviar
        protected void bntEnviar_Click(object sender, EventArgs e)
        {
            var tmp = new MacroController();
            var itens = tmp.ObterMacrosTemporariasPorUsuario(61, lblUsuarioMatricula.Text);

            #region [ MAIS DE 1 TREM SELECIONADOS ]

            if (itens.Count > 0)
            {

                if (txtMensagem.Text.Length > 0)
                {
                    var textoAbreviado = Uteis.ObterAbreviado(Uteis.RetirarAcentosCaracteresEspeciais(txtMensagem.Text));

                    string texto = txtMensagem.Text.Length > 140 ? Uteis.FormataTextoMacro(61, lblUsuarioPerfil.Text, textoAbreviado.Substring(0, 140)) : Uteis.FormataTextoMacro(61, lblUsuarioPerfil.Text, textoAbreviado);
                    string[] textoQuebrado = texto.Split('_');
                    if (textoQuebrado.Length < 3)
                        Response.Write("<script>alert('Texto da mensagem não anexado na Macro 61!');</script>");
                    else
                    {
                        
                        //Pegar todos os itens do repeater
                        for (int i = 0; i < itens.Count; i++)
                        {
                            List<EnviarMacro> macros = new List<EnviarMacro>();
                            EnviarMacro m61 = new EnviarMacro();
                            m61.MWE_NUM_MACRO = itens[i].Macro;
                            m61.MWE_DT_ENV = itens[i].Data;
                            m61.MWE_TEXTO = texto;
                            m61.MWE_ID_MCT = itens[i].Mct_ID;
                            m61.MWE_SIT_MWE = char.Parse("P");
                            m61.MWE_IND_MCR = char.Parse("N");

                            macros.Add(m61);
                            if (EnviandoMacro(macros, null, "E", lblUsuarioMatricula.Text))
                            {
                                RemoveItemDaLista(itens[i].Id);
                            }
                        }


                        Response.Write("<script>alert('Macro 61 enviada com sucesso, por " + lblUsuarioMatricula.Text + " - " + lblUsuarioPerfil.Text + "');</script>");
                        ListaTemporarios();
                    }
                }
                else Response.Write("<script>alert('Insira o texto da mensagem.');</script>");
            }

            #endregion

            #region [ APENAS 1 TREM SELECIONADOS ]

            else
            {
                if (ddlTrens.SelectedItem.Text == "Selecione o Trem!" && ddlMcts.SelectedItem.Value == "Selecione a Loco!")
                {
                    Response.Write("<script>alert('Selecione um ( Trem ou Loco) ou Adicione itens a lista antes de enviar.');</script>");
                }
                else
                {
                    if (txtMensagem.Text.Length > 0)
                    {
                        var textoAbreviado = Uteis.RetirarAcentosCaracteresEspeciais(Uteis.ObterAbreviado(txtMensagem.Text).Trim());

                        string texto = txtMensagem.Text.Length > 140 ? Uteis.FormataTextoMacro(61, lblUsuarioPerfil.Text, textoAbreviado.Substring(0, 140)) : Uteis.FormataTextoMacro(61, lblUsuarioPerfil.Text, textoAbreviado);
                        string[] textoQuebrado = texto.Split('_');
                        if (textoQuebrado.Length < 3)
                            Response.Write("<script>alert('Texto da mensagem não anexado na Macro 61!');</script>");

                        List<EnviarMacro> macros = new List<EnviarMacro>();
                        EnviarMacro m61 = new EnviarMacro();
                        m61.MWE_NUM_MACRO = 61;
                        m61.MWE_DT_ENV = DateTime.Now;
                        m61.MWE_TEXTO = texto;
                        m61.MWE_ID_MCT = double.Parse(ddlMcts.SelectedItem.Value);
                        m61.MWE_SIT_MWE = char.Parse("P");
                        m61.MWE_IND_MCR = char.Parse("N");

                        macros.Add(m61);

                        if (EnviandoMacro(macros, null, "E", lblUsuarioMatricula.Text))
                        {
                            Response.Write("<script>alert('Macro 61 enviada com sucesso, por " + lblUsuarioMatricula.Text + " - " + lblUsuarioPerfil.Text + "');</script>");
                            ListaTemporarios();
                        }
                    }
                    else Response.Write("<script>alert('Insira o texto da mensagem.');</script>");
                }
            }

            #endregion

            ViewState["Contador"] = "0";
        }

        // Método utilizado para limpar os campos do formulário clicar no botão limpar
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            limpaCampos();
        }
        protected void lnkAddItem_Click(object sender, EventArgs e)
        {
            double Contador = double.Parse(ViewState["Contador"].ToString());
            if (Contador < QtdeMaxima)
            {
                if (ddlTrens.SelectedItem.Text == "Selecione o Trem!" && ddlMcts.SelectedItem.Value == "Selecione a Loco!")
                {
                    Response.Write("<script>alert('Selecione um ( Trem ou Loco) ou Adicione itens a lista antes de enviar.');</script>");
                }
                else
                {
                    if (AdicionaItemNaLista())
                    {
                        //ddlTrens.SelectedValue = "Selecione o Trem!";
                        //ddlTrens.SelectedIndex = 0;
                        //ddlMcts.SelectedValue = "Selecione a Loco!";
                        //ddlMcts.SelectedIndex = 0;
                        ViewState["Contador"] = double.Parse(ViewState["Contador"].ToString()) + 1;
                    }
                }
            }
            else Response.Write("<script>alert('Limite máximo de trens selecionado.');</script>");
        }
        protected void lnkDelItem_Click(object sender, EventArgs e)
        {
            if (rptListaMacrosTemporarias.Items.Count > 0)
            {
                var macro = new MacroController();

                if (bool.Parse(Request.Form["confirm_value"]))
                {
                    List<string> itens = new List<string>();
                    for (int i = 0; i < rptListaMacrosTemporarias.Items.Count; i++)
                    {
                        HiddenField HiddenField1 = (HiddenField)rptListaMacrosTemporarias.Items[i].FindControl("HiddenField1");
                        CheckBox chkRestricao = (CheckBox)rptListaMacrosTemporarias.Items[i].FindControl("chkRestricao");
                        if (chkRestricao.Checked)
                            itens.Add(HiddenField1.Value);
                    }

                    if (itens.Count > 0)
                    {
                        //Pegar todos os itens do repeater
                        for (int i = 0; i < rptListaMacrosTemporarias.Items.Count; i++)
                        {
                            //Pegando o HiddenField dentro do repeater
                            HiddenField HiddenField1 = (HiddenField)rptListaMacrosTemporarias.Items[i].FindControl("HiddenField1");

                            //Pegando o CheckBox dentro do repeater
                            CheckBox chkRestricao = (CheckBox)rptListaMacrosTemporarias.Items[i].FindControl("chkRestricao");

                            //Verificar se foi selecionado
                            if (chkRestricao.Checked)
                            {
                                if (macro.RemoveMacrosTemporarias(int.Parse(HiddenField1.Value)))
                                    ViewState["Contador"] = double.Parse(ViewState["Contador"].ToString()) - 1;
                            }
                        }
                    }
                    else Response.Write("<script>alert('Selecione pelo menos 1 item da lista antes de clicar em remover.');</script>");
                }
            }
            else Response.Write("<script>alert('Não é possível retirar itens da lista quando a mesma se encontra vazia.');</script>");

            ListaTemporarios();
        }

        #endregion

        #region [ MÉTODOS DE SELEÇÃO DOS COMBOS ]

        // Método muda seleção da combo Trens
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

        // Método nuda seleção da combo Mcts
        protected void ddlMcts_SelectedIndexChanged(object sender, EventArgs e)
        {
            // - Seta a combo Trens pelo value da combo Mcts
            if (ddlMcts.SelectedValue != "Selecione o Mcts!")
            {
                double dados = SelecionaTemPeloMcts(double.Parse(ddlMcts.SelectedValue));
                if (dados > 0)
                {
                    ddlTrens.DataValueField = "TM_ID_TRM";
                    ddlTrens.DataTextField = "TM_PRF_ACT";
                    ddlTrens.SelectedValue = dados.ToString();
                }
                else
                    ddlTrens.SelectedValue = "Selecione o Trem!";
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected bool AdicionaItemNaLista()
        {
            bool retorno = false;
            var item = new TMP_MACROS();
            var tmp = new MacroController();

            item.Matricula = lblUsuarioMatricula.Text;
            item.Data = DateTime.Now;
            item.Macro = 61;
            if (ddlMcts.SelectedItem.Value != "Selecione a Loco!")
            {
                item.Mct_ID = double.Parse(ddlMcts.SelectedItem.Value);
                item.Mct = ddlMcts.SelectedItem.Text;
            }
            if (ddlTrens.SelectedItem.Value != "Selecione o Trem!")
            {
                item.Trm_ID = double.Parse(ddlTrens.SelectedItem.Value);
                item.Trem = ddlTrens.SelectedItem.Text;
            }

            if (!tmp.TemMacrosTemporarias(item, lblUsuarioMatricula.Text))
                if (tmp.SalvarMacrosTemporarias(item, lblUsuarioMatricula.Text))
                {
                    ListaTemporarios();
                    retorno = true;
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O Trem e o MCT selecionados já estão na lista.' });", true);

            return retorno;
        }
        protected bool RemoveItemDaLista(double Id)
        {
            bool retorno = false;
            var macro = new MacroController();

            if (macro.RemoveMacrosTemporarias(Id))
                retorno = true;

            return retorno;
        }

        protected bool EnviandoMacro(List<EnviarMacro> macros, string macrolidaid, string resposta, string usuarioLogado)
        {
            var macroController = new MacroController();
            return macroController.EnviarMacro(macros, macrolidaid, resposta, usuarioLogado);
        }
        public void addUsuario(string _usuario, string _matricula, string _perfil)
        {
            this.usuarioLogado = _usuario.ToString();
            this.usuarioMatricula = _matricula.ToString();
            this.usuarioPerfil = _perfil.ToString();
        }

        // Método que retorna "true" se os campos obrigatórios do formulários estiverem preenchidos ou "false" caso contrario
        protected bool validaFormulario()
        {
            return ddlMcts.SelectedValue != "Selecione o Mcts!" ? txtMensagem.Text != string.Empty ? true : false : false;
        }

        // Método utilizado para limpar os campos do formulário
        protected void limpaCampos()
        {
            ddlTrens.SelectedValue = "Selecione o Trem!";
            ddlTrens.SelectedIndex = 0;
            ddlMcts.SelectedValue = "Selecione a Loco!";
            ddlMcts.SelectedIndex = 0;
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
        protected double SelecionaTemPeloMcts(double mct_id_mct)
        {
            var macroController = new MacroController();
            return macroController.SelecionaTremPeloMcts(mct_id_mct);
        }

        #endregion

        public double ListaTemporarios()
        {
            var tmp = new MacroController();
            var itens = tmp.ObterMacrosTemporariasPorUsuario(61, lblUsuarioMatricula.Text);
            rptListaMacrosTemporarias.DataSource = itens.OrderBy(x => x.Trem);
            rptListaMacrosTemporarias.DataBind();

            lblTotal.Text = itens.Count.ToString();

            return itens.Count;
        }

        protected double QunatidadeMaximaDeMacrosEnviadasPorPerfil(string perfil)
        {

            QtdeMaxima = Usuario.Qtde_MC61;

            //switch (perfil)
            //{
            //    case "ADM":
            //        QtdeMaxima = 200;
            //        break;
            //    case "CTD":
            //        QtdeMaxima = 1;
            //        break;
            //    case "OP VP":
            //        QtdeMaxima = 1;
            //        break;
            //    case "OP ELE":
            //        QtdeMaxima = 1;
            //        break;
            //    case "CCE":
            //        QtdeMaxima = 15;
            //        break;
            //    case "PCM":
            //        QtdeMaxima = 1;
            //        break;
            //    case "ADM VP":
            //        QtdeMaxima = 1;
            //        break;
            //    case "ADM ELE":
            //        QtdeMaxima = 1;
            //        break;
            //    default:
            //        break;
            //}

            return QtdeMaxima;
        }
    }
}