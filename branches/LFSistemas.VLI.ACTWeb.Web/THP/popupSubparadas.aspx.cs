using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.THP
{
    public partial class popupSubparadas : System.Web.UI.Page
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
        public string UTPID { get; set; }
        public List<Entities.ComboBox> dadosMotivoParada { get; set; }
        public double QtdeMaxima { get; set; }
        public double TempoDisponivel { get; set; }
        public TempoParadaSubParadas tempoParadaOriginal { get; set; }

        TempoParadaSubParadasController TempoParadaSubParadasController = new TempoParadaSubParadasController();

        AbreviaturasController abreviar = new AbreviaturasController();

        DataTable dt = new DataTable();

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            UTPID = Uteis.Descriptografar(Request.QueryString["ui"].ToString(), "a#3G6**@").ToUpper();

            tempoParadaOriginal = TempoParadaSubParadasController.ObterParada(UTPID);
            ListaTemporarios();
            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                // - Carrega combo Trens
                ddlMotivoParada.DataValueField = "ID";
                ddlMotivoParada.DataTextField = "Descricao";
                ddlMotivoParada.DataSource = CarregaComboMotivos();
                ddlMotivoParada.DataBind();
                ddlMotivoParada.Items.Insert(0, "Selecione o motivo!");
                ddlMotivoParada.SelectedIndex = 0;

                Uteis.abreviados = abreviar.ObterTodos();

                BindRepeater();
                //ListaTemporarios();
                //ViewState["Contador"] = ListaTemporarios().Count;
                 
                //Pesquisar(UTPID);

            }
          
        }

        protected void Pesquisar(string UTPID)
        {

        }
        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        // Método utilizado para enviar mensagens (Macro) ao clicar no botão enviar
        protected void bntEnviar_Click(object sender, EventArgs e)
        {
            var tmp = new TempoParadaSubParadasController();
            var itens = tmp.ObterSubparadasTemporariasPorUsuario(double.Parse(UTPID), lblUsuarioMatricula.Text);



            //Pegar todos os itens do repeater
            for (int i = 0; i < itens.Count; i++)
            {
                var parada = new TMP_SUBPARADAS();

                parada.COD_MOTIVO = itens[i].COD_MOTIVO;
                parada.DT_INI_PARADA = itens[i].DT_INI_PARADA;
                parada.DT_FIM_PARADA = itens[i].DT_FIM_PARADA;
                parada.Matricula = itens[i].Matricula;
                parada.UTP_ID = itens[i].UTP_ID;
                parada.UTPS_ID = itens[i].UTPS_ID;
                var retorno = tmp.SalvarSubParadas(parada);


                if (retorno)
                {
                    RemoveItemDaLista(itens[i].UTP_ID);
                }
            }


            Response.Write("<script>alert('Macro 61 enviada com sucesso, por " + lblUsuarioMatricula.Text + " - " + lblUsuarioPerfil.Text + "');</script>");
            ListaTemporarios();

            ViewState["Contador"] = "0";
        }

        // Método utilizado para limpar os campos do formulário clicar no botão limpar
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            limpaCampos();
        }
        protected void lnkAddItem_Click(object sender, EventArgs e)
        {
            //double Contador = double.Parse(ViewState["Contador"].ToString());
            //if (Contador < QtdeMaxima)
            //{
            if (ddlMotivoParada.SelectedItem.Text == "Selecione o motivo da parada." && txtboxTempoParada.Text == null)
            {
                Response.Write("<script>alert('Selecione um motivo e digite o tempo da parada antes de adicionar itens na lista.');</script>");
            }
            else
            {
                if (AdicionaItemNaLista())
                {
                    ViewState["Contador"] = double.Parse(ViewState["Contador"].ToString()) + 1;
                    Pesquisar(UTPID);
                    BindRepeater();
                }
            }
            //}
            //else Response.Write("<script>alert('Limite máximo de trens selecionado.');</script>");
        }


        protected void lnkDelItem_Click(object sender, EventArgs e)
        {
            var item = new TempoParadaSubParadasController().ObterSubparadasTemporariasPorUsuario(double.Parse(UTPID), Usuario.Matricula);
            if (rptListaSubParadasTemporarias.Items.Count > 0)
            {
                var subparada = new TempoParadaSubParadasController();

                if (bool.Parse(Request.Form["confirm_value"]))
                {
                    List<string> itens = new List<string>();
                    for (int i = 0; i < rptListaSubParadasTemporarias.Items.Count; i++)
                    {
                        
                        HiddenField hfSubParada = (HiddenField)rptListaSubParadasTemporarias.Items[i].FindControl("hfSubParada");
                        CheckBox ChkboxSubParada = (CheckBox)rptListaSubParadasTemporarias.Items[i].FindControl("ChkboxSubParada");
                        if (ChkboxSubParada.Checked)
                            itens.Add(hfSubParada.Value);
                    }

                    if (itens.Count > 0)
                    {
                        //Pegar todos os itens do repeater
                        for (int i = 0; i < rptListaSubParadasTemporarias.Items.Count; i++)
                        {
                            //Pegando o HiddenField dentro do repeater
                            HiddenField HiddenField1 = (HiddenField)rptListaSubParadasTemporarias.Items[i].FindControl("hfSubParada");

                            //Pegando o CheckBox dentro do repeater
                            CheckBox ChkboxSubParada = (CheckBox)rptListaSubParadasTemporarias.Items[i].FindControl("ChkboxSubParada");

                            //Verificar se foi selecionado
                            if (ChkboxSubParada.Checked)
                            {
                                if (subparada.RemoveSubparadasTemporarias(int.Parse(HiddenField1.Value)))
                                    ViewState["Contador"] = double.Parse(ViewState["Contador"].ToString()) - 1;
                            }
                        }
                    }
                    else Response.Write("<script>alert('Selecione pelo menos 1 item da lista antes de clicar em remover.');</script>");
                }
            }
            else Response.Write("<script>alert('Não é possível retirar itens da lista quando a mesma se encontra vazia.');</script>");

            ListaTemporarios();
            BindRepeater();
        }
         
        #endregion


        #region [ MÉTODOS DE APOIO ]

        protected bool AdicionaItemNaLista()
        {
            bool retorno = false;
            var item = new TMP_SUBPARADAS();
            var tmp = new TempoParadaSubParadasController();

            item.Matricula = lblUsuarioMatricula.Text;
            item.DT_REGISTRO = DateTime.Now;
            item.DT_INI_PARADA = DateTime.Parse(tempoParadaOriginal.InicioParada);
            item.UTP_ID = double.Parse(UTPID);
            item.DT_FIM_PARADA = DateTime.Parse(tempoParadaOriginal.InicioParada).AddMinutes(double.Parse(txtboxTempoParada.Text));
            item.USU_ID = double.Parse(lblUsuarioMatricula.Text);
            item.TempoSubparada = double.Parse(txtboxTempoParada.Text);

            //TempoDisponivel = tempoTotal.Add(- int.Parse(txtboxTempoParada.Text));

            if ( tempoParadaOriginal.TempoRestante - double.Parse(txtboxTempoParada.Text) >= 0)
            {
                if (ddlMotivoParada.SelectedItem.Value != "Selecione o motivo da parada.")
                {
                    item.COD_MOTIVO = double.Parse(ddlMotivoParada.SelectedItem.Value);
                    item.Motivo = ddlMotivoParada.SelectedItem.Text;
                }

                if (txtboxTempoParada.Text != string.Empty)
                {
                    item.TempoSubparada = double.Parse(txtboxTempoParada.Text);
                }

                if (!tmp.TemSubparadasTemporarias(item, lblUsuarioMatricula.Text))
                    if (tmp.SalvarSubparadasTemporarias(item, lblUsuarioMatricula.Text))
                    {
                        ListaTemporarios();
                        //tempoParadaOriginal.TempoRestante = tempoParadaOriginal.TempoRestante - int.Parse(txtboxTempoParada.Text);
                        //lblTempoRestante.Text = "Tempo Restante (Min.): " + string.Format("{0:d2}:{1:d2}", (int)TimeSpan.FromMinutes(double.Parse((tempoParadaOriginal.TempoRestante).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((tempoParadaOriginal.TempoRestante).ToString())).Seconds);
                        limpaCampos();
                        retorno = true;
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O Trem e o MCT selecionados já estão na lista.' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Tempo total excedido.' });", true);
            return retorno;
        }
        protected bool RemoveItemDaLista(double Id)
        {
            bool retorno = false;
            var macro = new TempoParadaSubParadasController();

            if (macro.RemoveSubparadasTemporarias(Id))
                retorno = true;

            return retorno;
        }

        //protected bool EnviandoMacro(List<EnviarMacro> macros, string macrolidaid, string resposta, string usuarioLogado)
        //{
        //    var macroController = new MacroController();
        //    return macroController.EnviarMacro(macros, macrolidaid, resposta, usuarioLogado);
        //}
        public void addUsuario(string _usuario, string _matricula, string _perfil)
        {
            this.usuarioLogado = _usuario.ToString();
            this.usuarioMatricula = _matricula.ToString();
            this.usuarioPerfil = _perfil.ToString();
        }

        // Método que retorna "true" se os campos obrigatórios do formulários estiverem preenchidos ou "false" caso contrario
        protected bool validaFormulario()
        {
            return ddlMotivoParada.SelectedValue != "Selecione o motivo da parada." ? txtboxTempoParada.Text != string.Empty ? true : false : false;
        }

        // Método utilizado para limpar os campos do formulário
        protected void limpaCampos()
        {  
            ddlMotivoParada.SelectedIndex = 0;
            txtboxTempoParada.Text = string.Empty;
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected List<ComboBox> CarregaComboMotivos()
        {
            var comboBoxController = new ComboBoxController();
            dadosMotivoParada = comboBoxController.ComboBoxMotivoParadaTrem();
            return dadosMotivoParada;
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

        public void BindRepeater(){
        var itens = new TempoParadaSubParadasController().ObterSubparadasTemporariasPorUsuario(double.Parse(UTPID), Usuario.Matricula);
            tempoParadaOriginal.TempoRestante = tempoParadaOriginal.TempoParadaOriginal;

            rptListaSubParadasTemporarias.DataSource = itens.OrderBy(x => x.Motivo);
            rptListaSubParadasTemporarias.DataBind();

            lblTotal.Text = itens.Count.ToString();
        }


        public List<TMP_SUBPARADAS> ListaTemporarios()
        {
            var itens = new TempoParadaSubParadasController().ObterSubparadasTemporariasPorUsuario(double.Parse(UTPID), Usuario.Matricula);
            tempoParadaOriginal.TempoRestante = tempoParadaOriginal.TempoParadaOriginal;



            //rptListaSubParadasTemporarias.DataSource = itens.OrderBy(x => x.Motivo);
            //rptListaSubParadasTemporarias.DataBind();

            lblTotal.Text = itens.Count.ToString();

            double dif = 0;
            double aux;

            if (itens.Count > 0)
            {
                for (int i = 0; i < itens.Count; i++)
                {
                    dif += itens[i].TempoSubparada;
                }
                 tempoParadaOriginal.TempoRestante = tempoParadaOriginal.TempoRestante - dif;
            } 

            lblTempoRestante.Text = "Tempo Restante (Min.): " + string.Format("{0:d2}:{1:d2}", (int)TimeSpan.FromMinutes(double.Parse((tempoParadaOriginal.TempoRestante).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((tempoParadaOriginal.TempoRestante).ToString())).Seconds);

            //lblTempoRestante.Text = "Tempo Restante (Min.): " + string.Format("{0:d2}:{1:d2}", TimeSpan.Parse(aux.ToString()).Minutes, TimeSpan.Parse(aux.ToString()).Seconds);

            lblTremOS.Text = tempoParadaOriginal.OS.ToString() != null ? "Trem OS: " + tempoParadaOriginal.OS.ToString() : string.Empty;
            lblTempoTotalOriginal.Text = dif != null ? "Tempo (Min.): " + string.Format("{0:d2}:{1:d2}", (int)TimeSpan.FromMinutes(double.Parse((tempoParadaOriginal.TempoParadaOriginal).ToString())).Minutes, (int)TimeSpan.FromSeconds(double.Parse((tempoParadaOriginal.TempoParadaOriginal).ToString())).Seconds) : string.Empty;

            ViewState["Contador"] = itens.Count;

            return itens.ToList();
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

        protected void ddlMotivoParada_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}