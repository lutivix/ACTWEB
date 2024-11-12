using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{    
    public partial class popupLDL : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]        

        double Data { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string verificaKm { get; set; }
        public string id_aut { get; set; }
        public string sb { get; set; }
        public string corredores { get; set; }

        public bool retirando { get; set; }

        public static bool podeSolRetirada;

        //C931
        public string cpf;      
  
        //P1414
        public string cpf2;

        public enum StatusBarraComandos
        {
            Novo = 1,
            Edicao = 2
        }

        #endregion

        #region EMQ
        [DllImport(@"DLLMQWeb.dll")]
        /// <summary>
        /// Envia mensagem para fila do MQ
        /// </summary>
        /// <param name="prmIdSolicitacao">[ int ]: - Identificador Solicitação</param>
        /// <param name="prmIdTipoSituacao">[ int ]: - Identificador Tipo da Situação </param>
        /// <param name="prmData">[ double ]: - Data Atual</param>
        /// <param name="prmIdSecao">[ int ]: - Identificador da Seção</param>
        /// <param name="prmIdTipoInterdicao">[ int ]: - Identificador do Tipo de Interdição</param>
        /// <param name="prmDuracao">[ int ]: - Duração</param>
        /// <param name="prmIdTipoManutencao">[ double ]: - Identificador do Tipo de Manutenção</param>
        /// <param name="prmKM">[ double ]: - Km</param>
        /// <param name="prmMatResponsavel">[ char[] ]: - Matricula do Responsável da Via</param>
        /// <param name="prmOBS">[ char[] ]: - Observações</param>
        /// <param name="prmMatUsuarioLogado">[ char[] ]: - Matricula do Usuário Logado</param>
        /// <param name="prmTpUser">[ char ]: - Tipo de Usuário, neste caso [ W = Web ]</param>
        static extern void DLLSendSOI(int prmIdSLT_ACTWEB,
                                      int prmId_Tipo_Situacao,
                                      double prmData,
                                      int prmIdSecao,
                                      int prmId_Tipo_Interdicao,
                                      int prmDuracao,
                                      int prmId_Tipo_Manutencao,
                                      double prmKM,
                                      char[] prmMat_Responsavel,
                                      char[] prmObservacoes,
                                      char[] prmMat_Usuario,
                                      char prmTpUser,
                                      char[]prmTelefone_responsavel,
                                      char[]prmPrefixo,
                                      char[] prmCauda);

        [DllImport(@"DLLMQWeb.dll")]
        /// <summary>
        /// Envia mensagem para fila do MQ
        /// </summary>
        /// <param name="prmIdSolicitacao">[ int ]: - Identificador Solicitação</param>
        /// <param name="prmIdTipoInterdicao">[ int ]: - Identificador do Tipo de Interdição</param>
        /// <param name="prmMatUsuarioLogado">[ char[] ]: - Matricula do Usuário Logado</param>
        /// <param name="prmIdTipoCirculacao">[ int ]: - Identificador do Tipo de Circulação </param>
        /// <param name="prmTpUser">[ char ]: - Tipo de Usuário, neste caso [ W = Web ]</param>               
        static extern void DLLSendSRI(int prmIdSLT_ACTWEB,
                                     int prmIdSLT_ACT,
                                     int prmId_Tipo_Circulacao,
                                     char[] prmMat_Usuario,
                                     char prmTpUser);

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendSAR(int prmIdSLT_ACTWEB,
                                        int prmIdSLT_ACT,
                                        int prmId_Tipo_Circulacao,
                                        char[] prmMat_Usuario,
                                        char prmTpUser,
                                        char[] prmCPF_Responsavel,
                                        char[] prmJustificativa);

        #endregion

        #region [ EVENTOS ]

        #region [ PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDadosObsercacao.Attributes.Add("onkeyup", "return ismaxlength(this);");

            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblOperadorCCV_Nome.Text = lblUsuarioLogado.Text = ulNome = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            txtDadosOperadorCCV.Text = lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            

            if (!IsPostBack)
            {
                //txtPrefixo.Enabled = false;
                //lblPrefixo.Enabled = false;

                //tbCauda.Enabled = false;
                //lbCauda.Enabled = false;
        
                ViewState["ordenacao"] = "ASC";
                var dataIni = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
                var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
                txtDataInicial.Text = dataIni.ToShortDateString();
                txtDataFinal.Text = dataFim.ToShortDateString();
                lblCanalCom.Text = "Canal de comunicação de Entrada";
                lblPrefixo.Enabled = true;
                lblPrefixo.Visible = true;
                lblPrefixo.Text = "Prefixo:";
                lbCauda.Enabled = true;
                lbCauda.Visible = true;
                lbCauda.Text = "Cauda:";

                ControleFormulario(StatusBarraComandos.Novo);
                txtDadosDataAtual.Text = DateTime.Now.ToShortDateString();
                Pesquisar(null);



                //ddlDadosSecao.SelectedItem.Text = "BRUPOI";
                //ddlDadosSecao.SelectedItem.Value = "20200";
                //ddlDadosTipoDaInterdicao.SelectedItem.Text = "Interdição de LDL";
                //ddlDadosTipoDaInterdicao.SelectedItem.Value = "3";
                //txtDadosDuracao.Text = "10";
                //ddlDadosTipoDaManutencao.SelectedItem.Text = "Programada";
                //ddlDadosTipoDaManutencao.SelectedItem.Value = "1";
                //txtDadosKm.Text = "16,225";
                //rdDadosTelefone.Checked = true;
                //txtDadosTelefone.Text = "(31) 3427-1755 r. 521";
                //txtDadosResponsavel.Text = "30441295";
                //lblResponsavel_Nome.Text = "Julio Cesar Sousa Lima";
                //txtDadosObsercacao.Text = "Teste de solicitação de interdição";

                ddlDadosSecao.Focus();

                
            }           
        }

        #endregion

        #region [ COMBOS ]

        public void ComboDadosSecoes()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosSecao.DataValueField = "SecaoID";
            ddlDadosSecao.DataTextField = "SecaoNome";
            ddlDadosSecao.DataSource = interdicaoController.ObterComboInterdicao_ListaTodasSecoes();
            ddlDadosSecao.DataBind();
        }
        public void ComboDadosTipoSituacao()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosTipoDaSituacao.DataValueField = "Tipo_SituacaoCodigo";
            ddlDadosTipoDaSituacao.DataTextField = "Tipo_SituacaoNome";
            ddlDadosTipoDaSituacao.DataSource = interdicaoController.ObterCombo_TIPO_SITUACAO();
            ddlDadosTipoDaSituacao.DataBind();
        }
        public void ComboDadosTipoInterdicao()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosTipoDaInterdicao.DataValueField = "Tipo_InterdicaoCodigo";
            ddlDadosTipoDaInterdicao.DataTextField = "Tipo_InterdicaoNome";
            ddlDadosTipoDaInterdicao.DataSource = interdicaoController.ObterCombo_TIPO_INTERDICAO();
            ddlDadosTipoDaInterdicao.DataBind();
        }
        public void ComboDadosTipoManutencao()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosTipoDaManutencao.DataValueField = "Tipo_ManutencaoCodigo";
            ddlDadosTipoDaManutencao.DataTextField = "Tipo_ManutencaoNome";
            ddlDadosTipoDaManutencao.DataSource = interdicaoController.ObterCombo_TIPO_MANUTENCAO();
            ddlDadosTipoDaManutencao.DataBind();
        }
        public void ComboDadosTipoCirculacao()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosTipoDaCirculacao.DataValueField = "Tipo_CirculacaoCodigo";
            ddlDadosTipoDaCirculacao.DataTextField = "Tipo_CirculacaoNome";
            ddlDadosTipoDaCirculacao.DataSource = interdicaoController.ObterCombo_TIPO_CIRCULACAO();
            ddlDadosTipoDaCirculacao.DataBind();
        }
        public void ComboDadosMotivo()
        {
            var interdicaoController = new InterdicaoController();
            ddlDadosMotivo.DataValueField = "Id";
            ddlDadosMotivo.DataTextField = "Descricao";
            double manut = 0;
            if (ddlDadosTipoDaManutencao.SelectedItem.Value.Length > 0)
                manut = double.Parse(ddlDadosTipoDaManutencao.SelectedItem.Value);               
            ddlDadosMotivo.DataSource = interdicaoController.ObterCombo_MOTIVO_LDL(manut);
            ddlDadosMotivo.DataBind();
        }
        public void ComboFiltroSecoes()
        {
            var interdicaoController = new InterdicaoController();
            ddlFiltroSecao.DataValueField = "SecaoID";
            ddlFiltroSecao.DataTextField = "SecaoNome";

            var aux = new List<string>();
            if (clbCorredorLDL.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredorLDL.Items.Count; i++)
                {
                    if (clbCorredorLDL.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredorLDL.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }

            ddlFiltroSecao.DataSource = interdicaoController.ObterComboInterdicaoFiltro_SECAO(corredores);
            ddlFiltroSecao.DataBind();
        }
        public void ComboFiltroTipoSituacao()
        {
            var interdicaoController = new InterdicaoController();
            ddlFiltroTipoDaSituacao.DataValueField = "Id";
            ddlFiltroTipoDaSituacao.DataTextField = "Descricao";
            ddlFiltroTipoDaSituacao.DataSource = interdicaoController.ObterComboInterdicaoFiltro_TIPO_SITUACAO();
            ddlFiltroTipoDaSituacao.DataBind();
        }

        #endregion

        #region [ GRID ]

        protected void Pesquisar(string ordenacao)
        {
            var itens = ObterListaDeInterdicoes(ordenacao);
            rptListaInterdicoes.DataSource = itens;
            rptListaInterdicoes.DataBind();

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        #endregion

        #region [ BARRA DE COMANDOS ]

        protected void lnkCriar_Click(object sender, EventArgs e)
        {
            var restricaoController = new RestricaoController();
            int duracao = 0;

            if (txtDadosKm.Text != string.Empty && ddlDadosSecao.SelectedItem.Value != "0")
                verificaKm = restricaoController.VerificaKM(double.Parse(txtDadosKm.Text), double.Parse(ddlDadosSecao.SelectedItem.Value));

            try
            {
                duracao =  int.Parse(txtDadosDuracaoSolicitada.Text);
                if( (duracao < 0) || (duracao > 1440) )
                        duracao = 0;
            }
            catch(Exception)
            {
                duracao = 0;
            }
            if(!rdSimTrem.Checked && !rdNaoTrem.Checked)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Favor informar ocupação em SB!' });", true);
                return;
            }

            if(rdSimTrem.Checked)
            {
                if (txtPrefixo.Text.Length != 4)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'SB ocupada sem Prefixo informado!' });", true);
                    return;
                }

                if (tbCauda.Text == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Trem informado sem cauda!' });", true);
                    return;
                }
            }
            
            

            //C1047 - 01/2022 - início
            if ((lblUsuarioPerfil.Text == "OP VP") || (lblUsuarioPerfil.Text == "OP VP R"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Perfil não Permitido!' });", true);
                ControleFormulario(StatusBarraComandos.Novo);
                Pesquisar(null);
                return;
            }
            //C1047 - 01/2022 - fim


            if (duracao != 0)
            {

                if (verificaKm == "ok")
                {
                    if (DLLSendSOI())
                    {
                        UsuarioAutController usuario = new UsuarioAutController();
                        string CPF = txtDadosResponsavel.Text.Trim();
                        string matricula = lblUsuarioMatricula.Text.Trim();
                        string usuarioID = lblUsuarioLogado.Text.Trim();
                        string acao = "criação";
                        usuario.AtualizarDataUltSol(CPF, matricula, usuarioID, acao);
                        usuario.AtualizarDataUltSolBSOP(CPF, usuarioID, "7");

                        ControleFormulario(StatusBarraComandos.Novo);
                        Pesquisar(null);
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Km " + verificaKm + "' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', Duração:" + txtDadosDuracaoSolicitada.Text + " deve estar entre 1 e 1440 });", true);
                 
        }
        protected void lnkAtualizarLista_Click(object sender, EventArgs e)
        {
            lblCanalCom.Text = "Canal de comunicação de Entrada";
            Pesquisar(null);
        }
        protected void bntFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void bntFiltroLimpar_Click(object sender, EventArgs e)
        {
            ddlFiltroTipoDaSituacao.SelectedIndex = ddlFiltroSecao.SelectedIndex = 0;
            ddlFiltroSecao.SelectedIndex = 0;
            txtFiltroAutorizacao.Text = txtFiltroKm.Text = txtFiltroObservacao.Text = string.Empty;
            //lblCanalCom.Text = "Canal de comunicação de Entrada";

            var dataIni = DateTime.Parse(DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            var dataFim = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
            txtDataInicial.Text = dataIni.ToShortDateString();
            txtDataFinal.Text = dataFim.ToShortDateString();

            clbCorredorLDL.ClearSelection();
            Pesquisar(null);
        }
        protected void lnkLImpar_Click(object sender, EventArgs e)
        {
            ControleFormulario(StatusBarraComandos.Novo);
            Pesquisar(null);
        }               
        protected void lnkEdite_Click(object sender, EventArgs e)
        {
            Panel1.Visible = false;

            lblCanalCom.Text = "Canal de comunicação de Retirada ou Atualização";

            ControleFormulario(StatusBarraComandos.Edicao);
            LinkButton btn = (LinkButton)(sender);

            var interdicaoController = new InterdicaoController();
            var dados = interdicaoController.ObterInterdicaoPorID(double.Parse(btn.CommandArgument));
            if (dados != null)
            {
                lblIdentificador.Text = dados.Solicitacao_ID_ACTWEB.ToString();
                ddlDadosTipoDaSituacao.SelectedItem.Value = dados.Tipo_Situacao_ID != null ? dados.Tipo_Situacao_ID.ToString() : "0";
                ddlDadosTipoDaSituacao.SelectedItem.Text = dados.Situacao_Nome != null ? dados.Situacao_Nome : string.Empty;
                txtDadosDataAtual.Text = dados.Data != null ? dados.Data.ToShortDateString() : string.Empty;
                ddlDadosSecao.SelectedItem.Value = dados.Secao_ID.ToString() != null ? dados.Secao_ID.ToString() : "0";
                ddlDadosSecao.SelectedItem.Text = dados.Secao_Nome != null ? dados.Secao_Nome : string.Empty;
                ddlDadosTipoDaInterdicao.SelectedItem.Value = dados.Tipo_Interdicao_ID != null ? dados.Tipo_Interdicao_ID.ToString() : "0";
                ddlDadosTipoDaInterdicao.SelectedItem.Text = dados.Tipo_Interdicao_Nome != null ? dados.Tipo_Interdicao_Nome : string.Empty;
                txtDadosDuracaoSolicitada.Text = dados.Duracao_Solicitada != null ? dados.Duracao_Solicitada.ToString() : string.Empty;
                ddlDadosTipoDaManutencao.SelectedItem.Value = dados.Tipo_Manutencao_ID != null ? dados.Tipo_Manutencao_ID.ToString() : "0";
                ddlDadosTipoDaManutencao.SelectedItem.Text = dados.Tipo_Manutencao_Nome != null ? dados.Tipo_Manutencao_Nome : string.Empty;

                ddlDadosMotivo.SelectedItem.Value = dados.Motivo_ID.ToString();

                txtAutorizacao.Text = dados.Interdicao_Motivo.ToString();

                txtTelefoneResponsavel.Text = dados.Telefone_responsavel != null ? dados.Telefone_responsavel : string.Empty;

                txtPrefixo.Text = dados.Prefixo != null ? dados.Prefixo : string.Empty;
                tbCauda.Text = string.Empty;
                if (dados.Cauda != string.Empty)
                {
                    tbCauda.Text = dados.Cauda;
                }
                
                
                
                if (dados.Tipo_Circulacao_ID > 0)
                {
                    if (dados.Situacao_Nome != "R - Retirada")
                    {
                        ddlDadosTipoDaCirculacao.SelectedItem.Value = dados.Tipo_Circulacao_ID.ToString();
                        ddlDadosTipoDaCirculacao.SelectedItem.Text = dados.Tipo_Circulacao_Nome;
                        ddlDadosTipoDaCirculacao.Enabled = true;
                        lnkRetirar.CssClass = "btn btn-danger";
                    }
                    else
                    {
                        ddlDadosTipoDaCirculacao.SelectedItem.Value = dados.Tipo_Circulacao_ID.ToString();
                        ddlDadosTipoDaCirculacao.SelectedItem.Text = dados.Tipo_Circulacao_Nome;
                        ddlDadosTipoDaCirculacao.Enabled = false;
                        lnkRetirar.CssClass = "btn btn-danger disabled";
                    }
                }
                else
                {
                    if (dados.Situacao_Nome == "N - Negada" || dados.Situacao_Nome == "A - ARetirar")
                    {
                        ddlDadosTipoDaCirculacao.SelectedItem.Value = dados.Tipo_Circulacao_ID.ToString();
                        ddlDadosTipoDaCirculacao.SelectedItem.Text = dados.Tipo_Circulacao_Nome;
                        ddlDadosTipoDaCirculacao.Enabled = false;
                        lnkRetirar.CssClass = "btn btn-danger disabled";
                    }
                    else
                    {
                        ddlDadosTipoDaCirculacao.SelectedItem.Text = "Selecione";
                        ddlDadosTipoDaCirculacao.SelectedIndex = 0;
                        ddlDadosTipoDaCirculacao.Enabled = true;
                        lnkRetirar.CssClass = "btn btn-danger";
                    }
                }
                txtDadosKm.Text = dados.Km != null ? dados.Km.ToString() : string.Empty;
                if (dados.Telefone_SN == "S")
                {
                    rdDadosTelefone.Checked = true;
                    txtDadosTelefone.Text = dados.Telefone_Numero;
                    rdDadosRadio.Checked = rdDadosMacro.Checked = txtDadosMacro.Enabled = false;
                }
                if (dados.Radio_SN == "S")
                {
                    rdDadosRadio.Checked = true;
                    rdDadosTelefone.Checked = txtDadosTelefone.Enabled = rdDadosMacro.Checked = txtDadosMacro.Enabled = false;
                }
                if (dados.Macro_SN == "S")
                {
                    rdDadosMacro.Checked = true;
                    txtDadosMacro.Text = dados.Macro_Numero;
                    rdDadosRadio.Checked = rdDadosTelefone.Checked = txtDadosTelefone.Enabled = false;
                }
                txtDadosResponsavel.Text = dados.Responsavel_Matricula != null ? dados.Responsavel_Matricula : string.Empty;
                lblResponsavel_Nome.Text = dados.Responsavel_Nome != null ? dados.Responsavel_Nome : string.Empty;
                txtDadosEquipamentos.Text = dados.Equipamentos != null ? dados.Equipamentos : string.Empty;
                txtDadosObsercacao.Text = dados.Observacao != null ? dados.Observacao : string.Empty;
            }
        }
        protected void lnkRetirar_Click(object sender, EventArgs e)
        {
            retirando = true;
            id_aut = txtAutorizacao.Text;
            sb = ddlDadosSecao.SelectedItem.Text;
            cpf = txtDadosResponsavel.Text;
            cpf2 = txbCPF2.Text;//P1414

            lblCanalCom.Text = "Canal de comunicação de Retirada";

            //C1047 - 01/2022 - início
            if ((lblUsuarioPerfil.Text == "OP VP") || (lblUsuarioPerfil.Text == "OP VP R"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Perfil não Permitido!' });", true);
                ControleFormulario(StatusBarraComandos.Novo);
                Pesquisar(null);
                return;
            }
            //C1047 - 01/2022 - fim
            
            try
            {

                if (podeSolRetirada)
                {
                    podeSolRetirada = false;
                    //txtAutorizacao.Visible = false;

                    //chanar outra pag
                    //Response.Redirect("<script>window.open('/Restricoes/popupConfirmacaoLDL.aspx</script>");

                    //MessageBox

                    //ring teste = "<script>window.open('/Restricoes/confimacaoRetiradaLDL/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>";
                    //sponse.Write("<script>window.open('/Restricoes/confimacaoRetiradaLDL/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
                    //sponse.Write("<script>window.open('/Restricoes/confimacaoRetirarLDL/WebForm1.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
                    //sponse.Write("/Restricoes/confimacaoRetirarLDL/WebForm1.aspx");
                    //Response.Write("<script> " +
                    //        " var url = '/Restricoes/confirmacaoRetirarLDL/WebForm1.aspx?id=22';" +
                    //        " var newW = 800; " +
                    //        " var newH = 260; " +
                    //        " var left = (screen.width-newW)/2; " +
                    //        " var top = (screen.height-newH)/2; " +
                    //        " var newwindow = window.open(url, 'name', 'width='+newW+',height='+newH+',left='+left+',top='+top); " +
                    //        " newwindow.resizeTo(newW, newH); " +
                    //        " newwindow.moveTo(left, top); " +
                    //        " newwindow.focus();</script>");

                    //Response.Write("<script>window.open('/Macros/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");

                    if (DLLSendSRI())
                    {
                        ControleFormulario(StatusBarraComandos.Novo);
                        Pesquisar(null);
                        Panel1.Visible = true;
                        txtAutorizacao.Text = string.Empty;
                    }
                    retirando = false;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void lnkNovoResponsavel_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                        " var url = '/Cadastro/Responsavel_LDL.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "'; " +
                        " var newW = 800; " +
                        " var newH = 260; " +
                        " var left = (screen.width-newW)/2; " +
                        " var top = (screen.height-newH)/2; " +
                        " var newwindow = window.open(url, 'name', 'width='+newW+',height='+newH+',left='+left+',top='+top); " +
                        " newwindow.resizeTo(newW, newH); " +
                        " newwindow.moveTo(left, top); " +
                        " newwindow.focus();</script>");
        }
        protected void lnkCancelar_Click(object sender, EventArgs e)
        {
            ControleFormulario(StatusBarraComandos.Novo);
            Pesquisar(null);
            Panel1.Visible = true;
            txtAutorizacao.Text = string.Empty;
            lblCanalCom.Text = "Canal de comunicação de Entrada";
            //txtAutorizacao.Visible = false;
            bntFiltroLimpar_Click(sender, e);
        }
        protected void lnkLdl_Click(object sender, EventArgs e)
        {
            //var ordenacao = ViewState["ordenacao"].ToString();

            //if (ordenacao == "ASC")
            //{
            //    ViewState["ordenacao"] = "DESC";
            //    Pesquisar("OL.LDL_CODIGOIDENT " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            //}
            //else
            //{
            //    ViewState["ordenacao"] = "ASC";
            //    Pesquisar("OL.LDL_CODIGOIDENT " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            //}
        }
        protected void lnkMotivo_Click(object sender, EventArgs e)
        {

        }
        protected void lnkSituacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("SLT_ID_TP_SITUACAO " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("SLT_ID_TP_SITUACAO " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }
        protected void lnkSecao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EV.EV_NOM_MAC " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EV.EV_NOM_MAC " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }
        protected void lnkManutencao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TM.TP_MNT_NOME " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TM.TP_MNT_NOME " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }
        protected void lnkData_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_DATA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_DATA " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkKM_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_KM " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_KM " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }
        protected void lnkObservacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_OBSERVACAO " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_OBSERVACAO " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }
        protected void lnkDuracaoSolicitada_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_DURACAO_SOLICITADA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_DURACAO_SOLICITADA " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkDuracaoAutorizada_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_DURACAO_AUTORIZADA " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_DURACAO_AUTORIZADA " + ViewState["ordenacao"].ToString());
            }
        }
        /*protected void lnkAutorizacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("II.SLT_ID_ACT_AUT_INTER " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("II.SLT_ID_ACT_AUT_INTER " + ViewState["ordenacao"].ToString() + ", SLT_DATA desc");
            }
        }*/
        protected void lnkAtualizarCPF_Click(object sender, EventArgs e)//P707
        {
            retirando = true;

            //Quando clicar em atualizar, vai mandar mensagem pro bloqueio pra fazer o que precisa.
            var restricaoController = new RestricaoController();
            

            if (txtDadosKm.Text != string.Empty && ddlDadosSecao.SelectedItem.Value != "0")
                verificaKm = restricaoController.VerificaKM(double.Parse(txtDadosKm.Text), double.Parse(ddlDadosSecao.SelectedItem.Value));

            if (verificaKm == "ok")
            {
                if (DLLSendSOA())
                {
                    ControleFormulario(StatusBarraComandos.Novo);
                    Pesquisar(null);
                    Panel1.Visible = true;
                    txtAutorizacao.Text = string.Empty;
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Km " + verificaKm + "' });", true);

            
            /**            
            id_aut = txtAutorizacao.Text;
            sb = ddlDadosSecao.SelectedItem.Text;
            lblCanalCom.Text = "Canal de comunicação de Atualização";

            try
            {

                if (podeSolRetirada)
                {
                    podeSolRetirada = false;
                    //txtAutorizacao.Visible = false;

                    //chanar outra pag
                    //Response.Redirect("<script>window.open('/Restricoes/popupConfirmacaoLDL.aspx</script>");

                    //MessageBox

                    //ring teste = "<script>window.open('/Restricoes/confimacaoRetiradaLDL/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>";
                    //sponse.Write("<script>window.open('/Restricoes/confimacaoRetiradaLDL/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
                    //sponse.Write("<script>window.open('/Restricoes/confimacaoRetirarLDL/WebForm1.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
                    //sponse.Write("/Restricoes/confimacaoRetirarLDL/WebForm1.aspx");
                    //Response.Write("<script> " +
                    //        " var url = '/Restricoes/confirmacaoRetirarLDL/WebForm1.aspx?id=22';" +
                    //        " var newW = 800; " +
                    //        " var newH = 260; " +
                    //        " var left = (screen.width-newW)/2; " +
                    //        " var top = (screen.height-newH)/2; " +
                    //        " var newwindow = window.open(url, 'name', 'width='+newW+',height='+newH+',left='+left+',top='+top); " +
                    //        " newwindow.resizeTo(newW, newH); " +
                    //        " newwindow.moveTo(left, top); " +
                    //        " newwindow.focus();</script>");

                    //Response.Write("<script>window.open('/Macros/PopupEnviarParadaImediata.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'width=680, height=330, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");

                    if (DLLSendSOA())
                    {
                        ControleFormulario(StatusBarraComandos.Novo);
                        Pesquisar(null);
                        Panel1.Visible = true;
                        txtAutorizacao.Text = string.Empty;
                    }
                    retirando = false;
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            /**/
        }

        #endregion

        #endregion

        #region [ ACESSO A DADOS ]

        protected List<Interdicao> ObterListaDeInterdicoes(string ordenacao)
        {
            double? _situacao = null;
            double? _secao = null;
            double? _autorizacao = null;
            decimal? _km = null;
            string _observacao = null;
            DateTime? _data_inicial = null;
            DateTime? _data_final = null;

            if (txtFiltroAutorizacao.Text.Length > 0) _autorizacao = double.Parse(txtFiltroAutorizacao.Text); else _autorizacao = null;
            if (double.Parse(ddlFiltroTipoDaSituacao.SelectedItem.Value) > 0) _situacao = double.Parse(ddlFiltroTipoDaSituacao.SelectedItem.Value); else _situacao = null;
            if (double.Parse(ddlFiltroSecao.SelectedItem.Value) > 0) _secao = double.Parse(ddlFiltroSecao.SelectedItem.Value); else _secao = null;
            if (txtFiltroKm.Text.Length > 0) _km = decimal.Parse(txtFiltroKm.Text); else _km = null;
            if (txtFiltroObservacao.Text.Length > 0) _observacao = txtFiltroObservacao.Text; else _observacao = null;
            if (txtDataInicial.Text.Length > 0) _data_inicial = DateTime.Parse(txtDataInicial.Text + " 00:00:00"); else _data_inicial = null;
            if (txtDataFinal.Text.Length > 0) _data_final = DateTime.Parse(txtDataFinal.Text + " 23:59:59"); else _data_final = null;

            var aux = new List<string>();
            if (clbCorredorLDL.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredorLDL.Items.Count; i++)
                {
                    if (clbCorredorLDL.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredorLDL.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }


            var interdicaoController = new InterdicaoController();
            var dados = interdicaoController.ObterListaInterdicoes(new Entities.FiltroInterdicao()
            {
                Autorizacao = _autorizacao,
                Data_Inicial = _data_inicial,
                Data_Final = _data_final,
                Situacao = _situacao,
                Secao = _secao,
                km = _km,
                Observacao = _observacao,
                Corredores = corredores
            }, ordenacao);


            return dados;
        }
        protected unsafe bool DLLSendSOI() // Solicitação de Criação de Interdição
        {
            #region [ PROPRIEDADES ]

            var interdicaoController = new InterdicaoController();
            Interdicao inter = new Interdicao();
            bool retorno = false;

            #endregion

            try
            {
                
                if (interdicaoController.ExisteLDLAtivaNaSecao(double.Parse(ddlDadosSecao.SelectedItem.Value)))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe uma LDL vigente na SB!' });", true);
                    return false;
                }

                var existeInterdicaoNaSecao = interdicaoController.ExisteInterdicaoNaSecao(double.Parse(ddlDadosSecao.SelectedItem.Value));
                if (existeInterdicaoNaSecao.Tipo_Situacao_ID != 1 && existeInterdicaoNaSecao.Tipo_Situacao_ID != 2 && existeInterdicaoNaSecao.Tipo_Situacao_ID != 6)
                {

                    if (ddlDadosTipoDaSituacao.SelectedItem.Value.Length > 0) inter.Tipo_Situacao_ID = double.Parse(ddlDadosTipoDaSituacao.SelectedItem.Value);
                    inter.Situacao_Nome = ddlDadosTipoDaSituacao.SelectedItem.Text.Length > 0 ? ddlDadosTipoDaSituacao.SelectedItem.Text : null;
                    if (txtDadosDataAtual.Text.Length > 0)
                        inter.Data = DateTime.Now;
                    if (ddlDadosSecao.SelectedItem.Value.Length > 0)
                        inter.Secao_ID = double.Parse(ddlDadosSecao.SelectedItem.Value);
                    inter.Secao_Nome = ddlDadosSecao.SelectedItem.Text.Length > 0 ? ddlDadosSecao.SelectedItem.Text : null;
                    if (ddlDadosTipoDaInterdicao.SelectedItem.Value.Length > 0)
                        inter.Tipo_Interdicao_ID = double.Parse(ddlDadosTipoDaInterdicao.SelectedItem.Value);
                    inter.Tipo_Interdicao_Nome = ddlDadosTipoDaInterdicao.SelectedItem.Text.Length > 0 ? ddlDadosTipoDaInterdicao.SelectedItem.Text : null;
                    if (txtDadosDuracaoSolicitada.Text.Length > 0)
                        inter.Duracao_Solicitada = double.Parse(Uteis.TocarVirgulaPorPonto(txtDadosDuracaoSolicitada.Text));
                    if (ddlDadosTipoDaManutencao.SelectedItem.Value.Length > 0)
                        inter.Tipo_Manutencao_ID = double.Parse(ddlDadosTipoDaManutencao.SelectedItem.Value);
                    inter.Tipo_Manutencao_Nome = ddlDadosTipoDaManutencao.SelectedItem.Text.Length > 0 ? ddlDadosTipoDaManutencao.SelectedItem.Text : null;
                    if (ddlDadosTipoDaCirculacao.Text.Length > 0)
                        inter.Tipo_Circulacao_ID = double.Parse(ddlDadosTipoDaCirculacao.SelectedItem.Value);
                    if (txtDadosKm.Text.Length > 0)
                        inter.Km = decimal.Parse(txtDadosKm.Text);
                    inter.Telefone_SN = rdDadosTelefone.Checked ? "S" : "N";
                    inter.Telefone_Numero = txtDadosTelefone.Text.Length > 0 ? txtDadosTelefone.Text : string.Empty;
                    inter.Radio_SN = rdDadosRadio.Checked ? "S" : "N";
                    inter.Macro_SN = rdDadosMacro.Checked ? "S" : "N";
                    inter.Macro_Numero = txtDadosMacro.Text.Length > 0 ? txtDadosMacro.Text : string.Empty;
                    inter.Responsavel_Matricula = txtDadosResponsavel.Text.Length > 0 ? txtDadosResponsavel.Text : string.Empty;
                    inter.Responsavel_CPF2 = txbCPF2.Text.Length > 0 ? txbCPF2.Text : string.Empty;//P1414
                    inter.Responsavel_Nome = lblResponsavel_Nome.Text.Length > 0 ? lblResponsavel_Nome.Text : string.Empty;
                    inter.Equipamentos = txtDadosEquipamentos.Text.Length > 0 ? txtDadosEquipamentos.Text : string.Empty;
                    if (ddlDadosMotivo.SelectedItem.Value.Length > 0)
                        inter.Motivo_ID = double.Parse(ddlDadosMotivo.SelectedItem.Value);
                    inter.Observacao = txtDadosObsercacao.Text.Length > 0 ? txtDadosObsercacao.Text : string.Empty;
                    inter.Usuario_Logado_Matricula = lblUsuarioMatricula.Text.Length > 0 ? inter.Usuario_Logado_Matricula = lblUsuarioMatricula.Text : string.Empty;
                    inter.Ativo_SN = "S";
                    inter.Telefone_responsavel = txtTelefoneResponsavel.Text.Length > 0 ? txtTelefoneResponsavel.Text : string.Empty;
                    inter.Prefixo = txtPrefixo.Text.Length > 0 ? txtPrefixo.Text : string.Empty;
                    inter.Cauda = tbCauda.Text.Length > 0 ? tbCauda.Text : "0";

                    char[] usuariologado = new char[10];
                    char[] responsavel = new char[12];
                    char[] observacao = new char[38];
                    char[] Telefone_responsavel = new char[11];
                    char[] Prefixo = new char[4];
                    char[] Cauda = new char[8];//C1393


                    for (int i = 0; i <= 35; i++)
                    {
                        if (i < inter.Observacao.Length)
                            observacao[i] = inter.Observacao[i];
                        else
                            observacao[i] = char.MinValue;
                    }

                    for (int i = 0; i <= 9; i++)
                    {
                        if (i < lblUsuarioMatricula.Text.Length)
                            usuariologado[i] = lblUsuarioMatricula.Text[i];
                        else
                            usuariologado[i] = char.MinValue;
                    }

                    for (int i = 0; i <= 11; i++)
                    {
                        if (i < inter.Responsavel_Matricula.Length)
                            responsavel[i] = inter.Responsavel_Matricula[i];
                        else
                            responsavel[i] = char.MinValue;
                    }

                    for (int i = 0; i <= 10; i++)
                    {
                        if (i < inter.Telefone_responsavel.Length)
                            Telefone_responsavel[i] = inter.Telefone_responsavel[i];
                        else
                            Telefone_responsavel[i] = char.MinValue;
                    }

                    for (int i = 0; i <= 3; i++)
                    {
                        if (i < inter.Prefixo.Length)
                            Prefixo[i] = inter.Prefixo[i];
                        else
                            Prefixo[i] = char.MinValue;
                    }

                    //C1393
                    for (int i = 0; i <= 6 ; i++)
                    {
                        if (i < inter.Cauda.Length)
                            Cauda[i] = inter.Cauda[i];
                        else
                            Cauda[i] = char.MinValue;
                    }

                        inter.Usuario_Logado_Nome = lblUsuarioLogado.Text;

                    inter.Solicitacao_ID_ACT = (int)interdicaoController.ObterIdInterdicao();   // Pega o ID na tabela SOLICITACOES_LDL no ACT
                    inter.Solicitacao_ID_ACTWEB = (int)interdicaoController.ObterIdSolicitacao(); // Pega o ID na tabela SOLICITACAO_INTERDICAO no ACTWEB

                    DLLSendSOI((int)inter.Solicitacao_ID_ACTWEB, (int)inter.Tipo_Situacao_ID, inter.Data.ToOADate(), (int)inter.Secao_ID,
                                (int)inter.Tipo_Interdicao_ID, (int)inter.Duracao_Solicitada, (int)inter.Tipo_Manutencao_ID, (double)inter.Km, responsavel,
                                observacao, usuariologado, 'W', Telefone_responsavel, Prefixo, Cauda);

                    if (interdicaoController.Inserir(inter, ulMatricula))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Solicitação de criação de: " + inter.Tipo_Interdicao_Nome + " foi enviada ao ACT pelo usuário " + ulMatricula + " - " + ulPerfil + "' });", true);
                        retorno = true;
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível enviar a solicitação de criação de: " + inter.Tipo_Interdicao_Nome + "' });", true);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "LDL", null, inter.Solicitacao_ID_ACTWEB.ToString(), "Não foi possível enviar a solicitação de criação de: " + inter.Tipo_Interdicao_Nome, Uteis.OPERACAO.Solicitou.ToString());
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe solicitação/interdição na SB: " + existeInterdicaoNaSecao.Secao_Nome + "' });", true);
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Interdição", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return retorno;
        }
        protected unsafe bool DLLSendSRI() // Solicitação de Remoção de Interdição
        {
            var interdicaoController = new InterdicaoController();
            bool retorno = false;
            int Interdicao_ID = 0;

            Interdicao_ID = (int)interdicaoController.ObterIdInterdicaoPorSolicitacao_ID(double.Parse(lblIdentificador.Text));

            var secao = interdicaoController.ObterSecaoPorIdSolicitacao(Interdicao_ID);

            char[] usuario = new char[10];
            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }


            if (ddlDadosTipoDaCirculacao.SelectedItem.Value != "0")
            {
                if (ddlDadosTipoDaSituacao.SelectedItem.Text != "R - Retirada")
                {
                    if (lblIdentificador.Text.Length > 0)
                    {
                        if (ddlDadosTipoDaSituacao.SelectedItem.Text != "S - Solicitada" && ddlDadosTipoDaSituacao.SelectedItem.Text != "C - Confirmada")
                        {
                            if (interdicaoController.Retirar(double.Parse(lblIdentificador.Text), double.Parse(ddlDadosTipoDaCirculacao.SelectedItem.Value), ulMatricula))
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Solicitação de Remoção de interdição enviada ao ACT. Usuário " + ulMatricula + " - " + ulPerfil + "' });", true);
                                retorno = true;
                            }
                        }
                        else
                        {
                            DLLSendSRI(int.Parse(lblIdentificador.Text), Interdicao_ID, int.Parse(ddlDadosTipoDaCirculacao.SelectedItem.Value), usuario, 'W');
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Solicitação de retirada de: " + ddlDadosTipoDaInterdicao.SelectedItem.Text + " foi enviada ao ACT pelo usuário " + ulMatricula + " - " + ulPerfil + "' });", true);
                            LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "LDL", null, Interdicao_ID.ToString(), "Solicitação de Remoção de interdição enviada ao ACT. SB: "+ secao +"", Uteis.OPERACAO.Solicitou.ToString());
                            
                            retorno = true;

                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione uma interdição no grid abaixo para retirar a interdição.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Esta interdição já foi retirada anteriormente, favor selecionar outra.' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O campo Circulação é obrigatório para a solicitação de retirada da " + ddlDadosTipoDaInterdicao.SelectedItem.Text + ".' });", true);

            Thread.Sleep(3000);
            return retorno;
        }
        protected unsafe bool DLLSendSOA() // Solicitação de Remoção de Interdição
        {
            var interdicaoController = new InterdicaoController();
            bool retorno = false;
            int Interdicao_ID = 0;

            Interdicao_ID = (int)interdicaoController.ObterIdInterdicaoPorSolicitacao_ID(double.Parse(lblIdentificador.Text));

            var secao = interdicaoController.ObterSecaoPorIdSolicitacao(Interdicao_ID);

            char[] usuario = new char[10];
            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }

            char[] cpf = new char[12];
            if (txtDadosResponsavel.Text.Length == 11)
            {                
                for (int i = 0; i <= 11; i++)
                {
                    if (i < txtDadosResponsavel.Text.Length)
                        cpf[i] = txtDadosResponsavel.Text[i];
                    else
                        cpf[i] = char.MinValue;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O campo CPF é obrigatório para a atualização de " + ddlDadosTipoDaInterdicao.SelectedItem.Text + ".' });", true);
                return false;
            }
           

            char[] just = new char[38];
            /**
           if (tbJustificativa.Text.Length > 0)
           {
               for (int i = 0; i < 38; i++)
               {
                   if (i < tbJustificativa.Text.Length)
                       just[i] = tbJustificativa.Text[i];
                   else
                       just[i] = char.MinValue;
               }
           }
           else
           {
               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'O campo Justificativa é obrigatório para a atualização de CPF de " + ddlDadosTipoDaInterdicao.SelectedItem.Text + ".' });", true);
               return false;
           }
           /**/
                

            if (ddlDadosTipoDaSituacao.SelectedItem.Text == "C - Confirmada")
            {
                if (lblIdentificador.Text.Length > 0)
                {
                       
                        DLLSendSAR(int.Parse(lblIdentificador.Text), Interdicao_ID, int.Parse(ddlDadosTipoDaCirculacao.SelectedItem.Value), usuario, 'W', cpf, just);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Solicitação de alteraçãop de CPF de: " + ddlDadosTipoDaInterdicao.SelectedItem.Text + " foi enviada ao ACT pelo usuário " + ulMatricula + " - " + ulPerfil + "' });", true);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "LDL", null, Interdicao_ID.ToString(), "Solicitação de Alteração de CPF de interdição enviada ao ACT. SB: " + secao + "", Uteis.OPERACAO.Solicitou.ToString());

                        retorno = true;
                       
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione uma interdição no grid abaixo para retirar a interdição.' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Esta interdição não foi confirmada, favor selecionar outra.' });", true);
           

            Thread.Sleep(3000);
            return retorno;
        }
        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void txtDadosResponsavel_TextChanged(object sender, EventArgs e)
        {
            if (txtDadosResponsavel.Text.Length > 0)
            {
                RestricaoController responsavel = new RestricaoController();

                var dados = responsavel.PermiteLDL(txtDadosResponsavel.Text);

                if (dados != null && dados.Matricula != null)
                {
                    if (dados.LDL != "Não" && dados.Ativo == true)
                    {
                        lblResponsavel_Nome.Text = dados.Nome.Trim();
                        txtTelefoneResponsavel.Focus();
                    }
                    else
                    {
                        lblResponsavel_Nome.Text =
                        txtDadosResponsavel.Text = string.Empty;
                        txtDadosResponsavel.Focus();
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Responsável sem permissão.\");", true);

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Responsável sem permissão.' });", true);
                    }
                }
                else if (txtDadosResponsavel.Text.Length != 11)
                {
                    lblResponsavel_Nome.Text =
                    txtDadosResponsavel.Text = string.Empty;
                    txtDadosResponsavel.Focus();
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"CPF Inválido.\");", true);
                }
                else
                {
                    lblResponsavel_Nome.Text =
                    txtDadosResponsavel.Text = string.Empty;
                    txtDadosResponsavel.Focus();
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Responsável não localizado ou não tem permissão.\");", true);
                }

            }
            else
            {
                lblResponsavel_Nome.Text = string.Empty;
                txtDadosResponsavel.Focus();
            }                
        }
        protected void ddlDadosSecao_SelectedIndexChanged(object sender, EventArgs e)
        {
            var restricaoController = new RestricaoController();
            var kms = restricaoController.ObtemKmDaSecao(double.Parse(ddlDadosSecao.SelectedItem.Value));
            if (kms.Count > 0)
                lblMensagem.Text = string.Format("de: {0} - até: {1}", kms[0], kms[1]);
            else
                lblMensagem.Text = string.Format(" ");
            ddlDadosSecao.Focus();
        }

        //protected void RadioButton_CheckedChanged(Object sender, EventArgs e)
        //{
        //    txtPrefixo.eb = true;
        //    lblPrefixo.Visible = true;
        //}

        protected void clbCorredorLDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboFiltroSecoes();
        }    

        [System.Web.Services.WebMethod]
        public static void DeleteRestriction(string id)
        {
            podeSolRetirada = true;
            // Complete the action
            //if (DLLSendSRI())
            //{
            //    ControleFormulario(StatusBarraComandos.Novo);
            //    Pesquisar(null);
            //    Panel1.Visible = true;
            //    txtAutorizacao.Text = string.Empty;
            //}           
        }

        #endregion

        #region [ CONTROLE DE FORMULÁRIO ]

        private void ControleFormulario(StatusBarraComandos status)
        {
            switch (status)
            {
                case StatusBarraComandos.Novo:
                    ComboDadosSecoes();
                    ComboDadosTipoSituacao();
                    ComboDadosTipoInterdicao();
                    ComboDadosTipoManutencao();
                    ComboDadosTipoCirculacao();
                    //ComboDadosMotivo();
                    ComboFiltroSecoes();
                    ComboFiltroTipoSituacao();

                    ddlDadosSecao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlDadosTipoDaSituacao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlDadosTipoDaInterdicao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlDadosTipoDaManutencao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlDadosTipoDaCirculacao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlDadosMotivo.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlFiltroSecao.Items.Insert(0, new ListItem("Selecione", "0"));
                    ddlFiltroTipoDaSituacao.Items.Insert(0, new ListItem("Selecione", "0"));

                    ddlDadosSecao.SelectedItem.Text = "Selecione";
                    ddlDadosSecao.SelectedItem.Value = "0";
                    ddlDadosTipoDaSituacao.SelectedItem.Text = "S - Solicitada";
                    ddlDadosTipoDaSituacao.SelectedItem.Value = "1";
                    ddlDadosTipoDaInterdicao.SelectedItem.Text = "Selecione";
                    ddlDadosTipoDaInterdicao.SelectedItem.Value = "0";                    
                    ddlDadosTipoDaCirculacao.SelectedItem.Text = "Selecione";
                    ddlDadosTipoDaCirculacao.SelectedItem.Value = "0";
                    ddlDadosMotivo.SelectedItem.Text = "Selecione";
                    ddlDadosMotivo.SelectedItem.Value = "0";
                    ddlFiltroSecao.SelectedItem.Text = "Selecione";
                    ddlFiltroSecao.SelectedItem.Value = "0";
                    ddlFiltroTipoDaSituacao.SelectedItem.Text = "Selecione";
                    ddlFiltroTipoDaSituacao.SelectedItem.Value = "0";

                    ComboDadosMotivo();
                    ddlDadosTipoDaManutencao.SelectedItem.Text = "Selecione";
                    ddlDadosTipoDaManutencao.SelectedItem.Value = "0";

                    ddlDadosTipoDaSituacao.Enabled = false;
                    txtDadosDataAtual.Enabled = false;
                    ddlDadosSecao.Enabled = true;
                    //ddlDadosTipoDaInterdicao.Enabled = false;
                    txtDadosDuracaoSolicitada.Enabled = true;
                    ddlDadosTipoDaManutencao.Enabled = true;
                    ddlDadosTipoDaCirculacao.Enabled = false;
                    txtDadosKm.Enabled = true;
                    txtDadosTelefone.Enabled = true;
                    txtDadosResponsavel.Enabled = true;
                    rdDadosTelefone.Checked = true;
                    rdDadosTelefone.Enabled = true;
                    rdDadosMacro.Enabled = true;
                    rdDadosMacro.Checked = false;
                    rdDadosRadio.Enabled = true;
                    rdDadosRadio.Checked = false;
                    txtDadosEquipamentos.Enabled = true;
                    txtDadosMacro.Enabled = false;
                    txtDadosObsercacao.Enabled = true;
                    //txtAutorizacao.Enabled = true;

                    txtDadosDuracaoSolicitada.Text = string.Empty;
                    txtDadosKm.Text = string.Empty;
                    txtDadosTelefone.Text = string.Empty;
                    txtDadosResponsavel.Text = string.Empty;
                    lblResponsavel_Nome.Text = string.Empty;
                    txtDadosEquipamentos.Text = string.Empty;
                    txtDadosMacro.Text = string.Empty;
                    txtDadosObsercacao.Text = string.Empty;
                    lblMensagem.Text = string.Empty;
                    txtPrefixo.Text = string.Empty;
                    txtTelefoneResponsavel.Text = string.Empty;
                    tbCauda.Text = string.Empty;
                    //tbJustificativa.Text = string.Empty;



                    lnkCriar.Enabled = true;
                    lnkCriar.CssClass = "btn btn-success";
                    lnkRetirar.Enabled = false;
                    lnkRetirar.CssClass = "btn btn-danger disabled";
                    lnkCancelar.Enabled = false;
                    lnkCancelar.CssClass = "btn btn-warning disabled";
                    lnkAtualizarLista.Enabled = true;
                    lnkAtualizarLista.CssClass = "btn btn-default";
                    lnkLImpar.Enabled = true;
                    lnkLImpar.CssClass = "btn btn-primary";
                    lnkNovoResponsavel.Enabled = true;
                    lnkNovoResponsavel.CssClass = "btn btn-info";

                    if (ulPerfil != "ADM")
                    {
                        lnkNovoResponsavel.Visible = false;
                    }

                    //P707 - se for atualizar CPF precisa ser só ADM?? não
                    lnkAtualizarCPF.Enabled = true;
                    lnkAtualizarCPF.CssClass = "btn btn-info";

                    //if (ulPerfil != "ADM")
                    //{
                    //    lnkAtualizarCPF.Visible = false;
                    //}                    

                    ddlDadosSecao.Focus();

                    break;
                case StatusBarraComandos.Edicao:
                    ddlDadosTipoDaSituacao.Enabled = false;
                    txtDadosDataAtual.Enabled = false;
                    ddlDadosSecao.Enabled = false;
                    //ddlDadosTipoDaInterdicao.Enabled = false;
                    txtDadosDuracaoSolicitada.Enabled = false;
                    ddlDadosTipoDaManutencao.Enabled = false;
                    ddlDadosTipoDaCirculacao.Enabled = true;
                    txtDadosKm.Enabled = false;
                    rdDadosTelefone.Enabled = false;
                    txtDadosTelefone.Enabled = false;
                    txtDadosResponsavel.Enabled = true;//P707
                    rdDadosRadio.Enabled = false;
                    txtDadosEquipamentos.Enabled = false;
                    rdDadosMacro.Enabled = false;
                    txtDadosMacro.Enabled = false;
                    txtDadosObsercacao.Enabled = false;
                    txtTelefoneResponsavel.Enabled = true;
                    //txtAutorizacao.Enabled = true;

                    lnkCriar.Enabled = false;
                    lnkCriar.CssClass = "btn btn-success disabled";
                    lnkRetirar.Enabled = true;
                    lnkRetirar.CssClass = "btn btn-danger";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-warning";
                    lnkAtualizarLista.Enabled = false;
                    lnkAtualizarLista.CssClass = "btn btn-default disabled";
                    lnkLImpar.Enabled = false;
                    lnkLImpar.CssClass = "btn btn-primary disabled";
                    lnkNovoResponsavel.Enabled = false;
                    lnkNovoResponsavel.CssClass = "btn btn-info disabled";
                    txtPrefixo.Text = string.Empty;
                    tbCauda.Text = string.Empty;
                    //tbJustificativa.Text = string.Empty;
                    //txtTelefoneResponsavel.Text = string.Empty;
                    //txtDadosResponsavel.Text = string.Empty;
                    //lblResponsavel_Nome.Text = string.Empty;

                    if (ulPerfil != "ADM")
                    {
                        lnkNovoResponsavel.Visible = false;
                    }

                    //P707 - se for atualizar CPF precisa ser só ADM??
                    //if (ulPerfil != "ADM")
                    //{
                    //    lnkAtualizarCPF.Visible = false;
                    //}     

                    break;
            }
        }

        #endregion

        protected void ddlDadosTipoDaManutencao_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboDadosMotivo();
        }

        protected void txbCPF2_TextChanged(object sender, EventArgs e)
        {
            //P1414
            if (txbCPF2.Text.Length > 0)
            {
                RestricaoController responsavel = new RestricaoController();

                var dados = responsavel.PermiteLDL(txbCPF2.Text);

                if (dados != null && dados.Matricula != null)
                {
                    if (dados.LDL != "Não" && dados.Ativo == true)
                    {
                        lResponsavel2.Text = dados.Nome.Trim();
                        txtTelefoneResponsavel.Focus();
                    }
                    else
                    {
                        lResponsavel2.Text =
                        txbCPF2.Text = string.Empty;
                        txbCPF2.Focus();
                        ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Responsável 2 sem permissão.\");", true);

                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Responsável sem permissão.' });", true);
                    }
                }
                else if (txbCPF2.Text.Length != 11)
                {
                    lResponsavel2.Text =
                    txbCPF2.Text = string.Empty;
                    txbCPF2.Focus();
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"CPF 2 Inválido.\");", true);
                }
                else
                {
                    lResponsavel2.Text =
                    txbCPF2.Text = string.Empty;
                    txbCPF2.Focus();
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Responsável 2 não localizado ou não tem permissão.\");", true);
                }

            }
            else
            {
                lResponsavel2.Text = string.Empty;
                txbCPF2.Focus();
            }        

        }



        

       

        
    }
}