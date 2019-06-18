using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Threading;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using System.Text;
using System.Globalization;
namespace LFSistemas.VLI.ACTWeb.Web.Restricoes
{
    public partial class popupRestricoes : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        double DataIni { get; set; }
        double DataFim { get; set; }
        public DateTime? Data1 { get; set; }
        public DateTime? Data2 { get; set; }
        public decimal? Km1 { get; set; }
        public decimal? Km2 { get; set; }
        public decimal? KmSB1 { get; set; }
        public decimal? KmSB2 { get; set; }
        public double? Vel { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        enum TpUser
        {
            _UserOperador = 'O',
            _UserSupervisor = 'S',
            _UserDesenvolvedor = 'X',
            _UserDesconhecido = 'D'
        };

        enum MsgCentBloq
        {
            CML = 171501, RML = 171502, MML = 171503, MLC = 171504,
            CRE = 171505, RRE = 171506, AMC = 171507, ARE = 171508,
            CRA = 171509, REI = 171510, AVO = 171511
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct MsgCentBloqCRA
        {
            public ulong tipo;
            public double timestamp;
            public string matricula;      // [TAMMATRICULA]
            public char tipousuario;
            public ulong idsecao;
            public uint duracao;
            public ulong idtipo;
            public ulong idsubtipo;
            public uint velocidade;
            public double kminicio;
            public double kmfinal;
            public fixed char observacoes[36];    // [TAMOBSERVACAO]
            public fixed char latitudeinicio[8];  // [TAMCOORDENADA]
            public fixed char longitudeinicio[8]; // [TAMCOORDENADA]
            public fixed char latitudefim[8];     // [TAMCOORDENADA]
            public fixed char longitudefim[8];    // [TAMCOORDENADA]
            public double vigenciainicial;
            public double vigenciafinal;
            public fixed char responsavel[20];    // [TAMRESPONSAVEL]
            public ulong idconfirmacao;
        };

        /// <summary>
        /// Envia mensagem para fila do MQ para Criar Resrtrições
        /// </summary>
        /// <param name="prmSecao">[ int ]: - Identificador da Seção</param>
        /// <param name="prmDuracao">[ int ]: - Valor da duração </param>
        /// <param name="prmTipo">[ int ]: - Identificador do tipo de restrição</param>
        /// <param name="prmSubtipo">[ int ]: - Identificador do SubTipo VR</param>
        /// <param name="prmVelocidade">[ int ]: - Velocidade</param>
        /// <param name="prmKMInicio">[ double ]: - Km Inicial</param>
        /// <param name="prmKKMFim">[ double ]: - Km Final</param>
        /// <param name="prmOBS">[ char ]: - Observação</param>
        /// <param name="prmVigenciaInicial">[ double ]: - Data Inicial</param>
        /// <param name="prmVigenciaFinal">[ double ]: - Data Final</param>
        /// <param name="prmResponsavel">[ char ]: - Responsável</param>
        /// <param name="prmConfirmacao">[ int ]: - Identificador da confirmação</param>
        /// <param name="prmMatricula">[ char ]: - Matrícula do usuário logado </param>
        /// <param name="prmTpUser">[ char ]: - Tipo de Usuário, neste caso [ W = Web ]</param>
        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendCRA(int prmSecao,
                                      int prmDuracao,
                                      int prmTipo,
                                      int prmSubtipo,
                                      int prmVelocidade,
                                      double prmKMInicio,
                                      double prmKKMFim,
                                      char[] prmOBS,
                                      double prmVigenciaInicial,
                                      double prmVigenciaFinal,
                                      char[] prmResponsavel,
                                      int prmConfirmacao,
                                      char[] prmMatricula,
                                      char prmTpUser);


        /// <summary>
        /// Envia mensagem para fila do MQ para Remover Resrtrições
        /// </summary>
        /// <param name="prmIDRestricao">[ int ]: - Identificador da Restrição</param>
        /// <param name="prmMatricula">[ char ]: - Matrícula do usuário logado</param>
        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendRRE(int prmIDRestricao, char[] prmMatricula);

        string verificaKmInicio { get; set; }
        string verificaKmFinal { get; set; }
        public List<Restricao> ListaRestricoes { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblUsuarioLogado.Text = ulNome = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!IsPostBack)
            {
                var DataAtual = DateTime.Now;
                var HoraAtual = DataAtual.ToShortTimeString();
                var NovaData = DataAtual.AddHours(8);
                var NovaHora = NovaData.ToShortTimeString();

                LimpaCampos();

                ListaRestricoes = ObterListaDeRestricoes();
                rptListaRestricoes.DataSource = ListaRestricoes;
                rptListaRestricoes.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", ListaRestricoes.Count);

                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("PP - Prospecção", "6"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("EE - Equipe eletro-eletrônica", "5"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("HL - Homens trabalhando as margens da linha", "4"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("HT - Homens trabalhando na linha", "3"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("RL - Ronda de linha", "2"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("US - Ultrassom", "1"));
                //ddlDadosSubTipoVR.Items.Insert(0, new ListItem("Selecione", "0"));

                //ddlDadosSecoes.SelectedItem.Text = "ETHET2";
                //ddlDadosSecoes.SelectedItem.Value = "5103";
                //ddlDadosTipoRestricao.SelectedItem.Text = "VR";
                //ddlDadosTipoRestricao.SelectedItem.Value = "26";
                //ddlDadosSubTipoVR.SelectedItem.Text = "HL - Homens trabalhando as margens da linha";
                //ddlDadosSubTipoVR.SelectedItem.Value = "4";

                //txtDadosDuracao.Text = "9";
                //txtDadosKm_Inicio.Text = "679";
                //txtDadosKm_Final.Text = "690";
                //txtDadosVelocidade.Text = "VR";
                //txtDadosResponsavel.Text = "Teste";
                //txtDadosObs.Text = "Teste";


                txtDadosDataInicial.Text = DataAtual.ToShortDateString();
                txtDadosHoraInicial.Text = HoraAtual;
                txtDadosDataFinal.Text = NovaData.ToShortDateString();

                ddlDadosSecoes.Focus();
            }

            HabilitaDesabilitaCombos(true);
            lnkRemoverRonda.Enabled = false;
            lnkRemoverRonda.CssClass = "btn btn-success disabled";
            lnkProrrogarDataFinal.Enabled = false;
            lnkProrrogarDataFinal.CssClass = "btn btn-success disabled";
        }

        #endregion

        #region [ CARREGA COMBOS ]

        public void ComboFiltroSB()
        {
            var restricaoController = new RestricaoController();
            ddlFiltroSB.DataSource = restricaoController.ObterFiltroSB();
            ddlFiltroSB.DataBind();
            ddlFiltroSB.Items.Insert(0, new ListItem("Selecione", ""));
        }

        public void ComboFiltroTipo()
        {
            var restricaoController = new RestricaoController();
            ddlFiltroTipo.DataSource = restricaoController.ObterFiltroTipo();
            ddlFiltroTipo.DataBind();
            ddlFiltroTipo.Items.Insert(0, new ListItem("Selecione", ""));
        }

        public void ComboDadosSecoes()
        {
            var restricaoController = new RestricaoController();
            ddlDadosSecoes.DataValueField = "SecaoID";
            ddlDadosSecoes.DataTextField = "SecaoNome";
            ddlDadosSecoes.DataSource = restricaoController.ObterComboRestricao_ListaTodasSecoes();
            ddlDadosSecoes.DataBind();
            ddlDadosSecoes.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        public void ComboDadosTipoRestricao()
        {
            var restricaoController = new RestricaoController();
            ddlDadosTipoRestricao.DataValueField = "Tipo_RestricaoID";
            ddlDadosTipoRestricao.DataTextField = "Tipo_RestricaoNome";
            ddlDadosTipoRestricao.DataSource = restricaoController.ObterComboRestricao_ListaTodosTipoRestricao();
            ddlDadosTipoRestricao.DataBind();
            ddlDadosTipoRestricao.Items.Insert(0, new ListItem("Selecione", "0"));
        }

        protected void HabilitaDesabilitaCombos(bool habilita)
        {

            if (lblUsuarioPerfil.Text == "OP VP")
            {
                ddlDadosTipoRestricao.SelectedItem.Text = "VR";
                ddlDadosTipoRestricao.SelectedItem.Value = "26";
                txtDadosVelocidade.Text = "VR";

                ddlFiltroTipo.SelectedItem.Text = "VR";
                ddlFiltroTipo.SelectedItem.Value = "26";

                ddlDadosSecoes.Enabled =
                    ddlDadosSubTipoVR.Enabled =
                    txtDadosDataInicial.Enabled =
                    txtDadosHoraInicial.Enabled =
                    txtDadosDataFinal.Enabled =
                    txtDadosHoraFinal.Enabled =
                    txtDadosKm_Inicio.Enabled =
                    txtDadosKm_Final.Enabled =
                    txtDadosResponsavel.Enabled =
                    txtDadosCpf.Enabled =
                    txtDadosObs.Enabled =
                    txtTelefone.Enabled = habilita;

                ddlDadosTipoRestricao.Enabled =
                txtDadosDuracao.Enabled =
                    txtDadosVelocidade.Enabled = !habilita;
            }
            else if ((lblUsuarioPerfil.Text == "CCM") || (lblUsuarioPerfil.Text == "CTD") || (lblUsuarioPerfil.Text == "CTD - LOCO") || (lblUsuarioPerfil.Text == "CTD - VAG"))
            {
                ddlDadosTipoRestricao.SelectedItem.Text = "Boletim de Serviço";
                ddlDadosTipoRestricao.SelectedItem.Value = "26";
                txtDadosVelocidade.Text = "VR";

                ddlFiltroTipo.SelectedItem.Text = "VR";
                ddlFiltroTipo.SelectedItem.Value = "26";

                ddlDadosSecoes.Enabled =
                    ddlDadosSubTipoVR.Enabled =
                    txtDadosDataInicial.Enabled =
                    txtDadosHoraInicial.Enabled =
                    txtDadosDataFinal.Enabled =
                    txtDadosHoraFinal.Enabled =
                    txtDadosKm_Inicio.Enabled =
                    txtDadosKm_Final.Enabled =
                    txtDadosResponsavel.Enabled =
                    txtDadosCpf.Enabled =
                    txtDadosObs.Enabled =
                    txtTelefone.Enabled = habilita;

                ddlDadosTipoRestricao.Enabled =
                txtDadosDuracao.Enabled =
                    txtDadosVelocidade.Enabled = !habilita;
            }
            else if (lblUsuarioPerfil.Text == "OP ELE")
            {
                ddlDadosTipoRestricao.SelectedItem.Text = "VR";
                ddlDadosTipoRestricao.SelectedItem.Value = "26";
                txtDadosVelocidade.Text = "VR";
                ddlDadosSubTipoVR.SelectedItem.Text = "EE";
                ddlDadosSubTipoVR.SelectedItem.Value = "5";

                ddlFiltroTipo.SelectedItem.Text = "VR";
                ddlFiltroTipo.SelectedItem.Value = "26";

                ddlDadosSecoes.Enabled =
                    txtDadosDataInicial.Enabled =
                    txtDadosHoraInicial.Enabled =
                    txtDadosDataFinal.Enabled =
                    txtDadosHoraFinal.Enabled =
                    txtDadosKm_Inicio.Enabled =
                    txtDadosKm_Final.Enabled =
                    txtDadosResponsavel.Enabled =
                    txtDadosCpf.Enabled =
                    txtDadosObs.Enabled =
                    txtTelefone.Enabled = habilita;

                ddlDadosTipoRestricao.Enabled =
                    ddlDadosSubTipoVR.Enabled =
                    txtDadosDuracao.Enabled =
                    txtDadosVelocidade.Enabled = !habilita;
            }
            else
            {
                if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR" || ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "IF")
                {
                    ddlDadosSecoes.Enabled =
                        ddlDadosTipoRestricao.Enabled =
                        ddlDadosSubTipoVR.Enabled =
                        txtDadosDataInicial.Enabled =
                        txtDadosHoraInicial.Enabled =
                        txtDadosDataFinal.Enabled =
                        txtDadosHoraFinal.Enabled =
                        txtDadosKm_Inicio.Enabled =
                        txtDadosKm_Final.Enabled =
                        txtDadosResponsavel.Enabled =
                        txtDadosCpf.Enabled =
                        txtDadosObs.Enabled =
                        txtTelefone.Enabled = habilita;

                    txtDadosDuracao.Enabled =
                        txtDadosVelocidade.Enabled = !habilita;
                }
                else
                {
                    ddlDadosSecoes.Enabled =
                       ddlDadosTipoRestricao.Enabled =
                       txtDadosDuracao.Enabled =
                       txtDadosKm_Inicio.Enabled =
                       txtDadosKm_Final.Enabled =
                       txtDadosVelocidade.Enabled =
                       txtDadosObs.Enabled = habilita;

                    ddlDadosSubTipoVR.Enabled =
                       txtDadosDataInicial.Enabled =
                       txtDadosHoraInicial.Enabled =
                       txtDadosDataFinal.Enabled =
                       txtDadosHoraFinal.Enabled =
                        txtDadosResponsavel.Enabled = 
                        txtDadosCpf.Enabled = 
                        txtTelefone.Enabled = !habilita;
                }
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkCriarRestricao_Click(object sender, EventArgs e)
        {
            #region [ PROPRIEDADES ]

            var restricaoController = new RestricaoController();
            Restricao rr = new Restricao();


            if (txtDadosKm_Inicio.Text != string.Empty && ddlDadosSecoes.SelectedItem.Value != "0")
            {
                verificaKmInicio = restricaoController.VerificaKM(double.Parse(txtDadosKm_Inicio.Text), double.Parse(ddlDadosSecoes.SelectedItem.Value));
            }
            if (txtDadosKm_Final.Text != string.Empty && ddlDadosSecoes.SelectedItem.Value != "0")
            {
                verificaKmFinal = restricaoController.VerificaKM(double.Parse(txtDadosKm_Final.Text), double.Parse(ddlDadosSecoes.SelectedItem.Value));
            }

            Data1 = null;
            Data2 = null;

            if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "IF"
                || ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR")
            {
                string dataInicial = txtDadosDataInicial.Text.Trim();
                string dataFinal = txtDadosDataFinal.Text.Trim();
                string horaInicial = txtDadosHoraInicial.Text.Trim() + ":00";
                string horaFinal = txtDadosHoraFinal.Text.Trim() + ":00";

                Data1 = Uteis.ConverteStringParaDateTime(dataInicial, horaInicial);
                Data2 = Uteis.ConverteStringParaDateTime(dataFinal, horaFinal);


                if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR"
                   && Data1 != null && Data2 != null)
                {
                    DateTime tempData1 = (DateTime)Data1;
                    DateTime tempData2 = (DateTime)Data2;


                    // Restrição de tempo anterior a data atual
                    if (DateTime.Compare(tempData1, DateTime.Now.AddMinutes(-30)) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possivel criar uma restrição com a data e hora inicial 30 minutos antes do horário.' });", true);
                        return;
                    }

                    // Data final inferior a data inicial ou a data atual
                    if (DateTime.Compare(tempData1, tempData2) > 0
                        || DateTime.Compare(tempData2, DateTime.Now) < 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possivel criar restrição com a data final inferior a data inicial ou data final inferior a data atual.' });", true);
                        return;
                    }

                    LimitesRestricao limite = new RestricaoController().ObterLimiteTempoRestricao();

                    // Inicio muito tarde
                    double tempoAceitavel = limite.tempoParaInicio;
                    if (tempoAceitavel != 0 && DateTime.Compare(tempData1, DateTime.Now.AddMinutes(tempoAceitavel)) >= 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possivel criar uma restrição após " + DateTime.Now.AddMinutes(tempoAceitavel).ToString() + "' });", true);
                        return;
                    }

                    // Duração acima do desejado
                    TimeSpan duration = tempData2 - tempData1;
                    double duracaoLimite = limite.duracaoMaxima;
                    if (duracaoLimite != 0 && duracaoLimite < duration.TotalMinutes)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possivel criar uma restrição deste tipo com a duração acima de " + duracaoLimite + " minutos.' });", true);
                        return;
                    }
                }
            }

            if (txtDadosKm_Inicio.Text != string.Empty)
            {
                //Km1 = decimal.Parse(Uteis.TocarVirgulaPorPonto(txtDadosKm_Inicio.Text));
                Km1 = txtDadosKm_Inicio.Text != string.Empty ? decimal.Parse(txtDadosKm_Inicio.Text) : 0;
            }
            else
            {
                Km1 = null;
            }
            if (txtDadosKm_Final.Text != string.Empty)
            {
                //Km2 = decimal.Parse(Uteis.TocarVirgulaPorPonto(txtDadosKm_Final.Text));
                Km2 = txtDadosKm_Final.Text != string.Empty ? decimal.Parse(txtDadosKm_Final.Text) : 0;
            }
            else
            {
                Km2 = null;
            }
            if (txtDadosVelocidade.Text != string.Empty
                && txtDadosVelocidade.Text != "VR"
                && txtDadosVelocidade.Text != "IF")
            {
                Vel = double.Parse(txtDadosVelocidade.Text);
            }
            else
            {
                Vel = null;
            }

            string subtipoVR = ddlDadosSubTipoVR.SelectedItem.Text.Substring(0, 2);
            var kms = restricaoController.ObtemKmDaSecao(double.Parse(ddlDadosSecoes.SelectedItem.Value));
            if (kms != null)
            {
                KmSB1 = decimal.Parse(kms[0]);
                KmSB2 = decimal.Parse(kms[1]);
            }


            #endregion

            if (!restricaoController.PermiteBS(double.Parse(txtDadosCpf.Text), double.Parse(ddlDadosSubTipoVR.SelectedItem.Value)))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A criação da restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT, devido o CPF informado não ter permissão para criação de BS.' });", true);
                return;
            }

            if (!restricaoController.ExisteRestricao(double.Parse(ddlDadosSecoes.SelectedItem.Value), double.Parse(ddlDadosTipoRestricao.SelectedItem.Value), double.Parse(ddlDadosSubTipoVR.SelectedItem.Value), Data1, Data2, Vel, Km1, Km2))
            {
                if (verificaKmInicio == "ok" && verificaKmFinal == "ok")
                {
                    try
                    {
                        if (subtipoVR == "HT")
                        {
                            if ((restricaoController.ExisteHTProgramada((double.Parse(ddlDadosSecoes.SelectedItem.Value)), Km1, Km2)) || (restricaoController.ExisteHTCircualacao((double.Parse(ddlDadosSecoes.SelectedItem.Value)), Km1, Km2)))
                            {
                               ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A criação da restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT, devido haver uma restrição no mesmo KM informado na Seção de Bloqueio.' });", true);
                                return;
                            }
                        }
                        if ((ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR") && (restricaoController.ExisteInterdicao(double.Parse(ddlDadosSecoes.SelectedItem.Value))))
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A criação da restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT, devido haver uma interdiçao na Seção de Bloqueio.' });", true);
                            return;
                        }
                        #region [VERIFICA DUPLICAÇÃO DE SUBTIPO]

                        //Verifica se o boletim de serviço é tipo velocidade restrita
                        if ((ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR"))
                        {
                            //verifica se é do subtipo que entra na regra de não duplicação
                            //os subtipos são RL, PP, US, e EE
                            switch (Convert.ToInt32(ddlDadosSubTipoVR.Text))
                            {
                                case 1:
                                case 2:
                                case 5:
                                case 6:
                                    //método de verificação
                                    if (!PodeCriarBS())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show        ({ title: 'ATENÇÃO!', message: 'A criação da restrição " + ddlDadosSecoes.SelectedItem.Text +           " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT, devido          já existir uma restrição do mesmo subtipo na seção.' });", true);
                                        return;
                                    }
                                    
                                    break;
                            }
                        }
                        #endregion

                        var retorno = SendMessageCRE();
                        if (retorno == true)
                        {
                            if ((int.Parse(ddlDadosTipoRestricao.SelectedItem.Value) == 26) || (int.Parse(ddlDadosTipoRestricao.SelectedItem.Value) == 27))
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Restrição programada com sucesso. " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + "' });", true);
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Restrição criada com sucesso. " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + "' });", true);

                            LimpaCampos();
                            AtualizarListaDeRestricoes();
                            HabilitaDesabilitaCombos(true);
                        }
                        else
                        {
                            if ((int.Parse(ddlDadosTipoRestricao.SelectedItem.Value) == 26) || (int.Parse(ddlDadosTipoRestricao.SelectedItem.Value) == 27))
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A programação da restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT.' });", true);
                            else
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A criação da restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " não pode ser solicitada ao ACT.' });", true);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                else
                {
                    string erro = string.Empty;

                    if (verificaKmInicio != "ok" && verificaKmFinal != "ok")
                        if (verificaKmInicio != null && verificaKmFinal != null)
                            erro += "Km inicial: " + verificaKmInicio + " - Km final: " + verificaKmFinal;
                        else
                            erro += "É obrigatório selecionar uma Seção!";
                    else if (verificaKmInicio != "ok")
                        erro += "Km inicial: " + verificaKmInicio;
                    else
                        erro += "Km final: " + verificaKmFinal;

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: '" + erro + "' });", true);
                    if (verificaKmInicio != "ok" && verificaKmFinal != "ok")
                        txtDadosKm_Inicio.Focus();
                    else if (verificaKmInicio != "ok")
                        txtDadosKm_Inicio.Focus();
                    else
                        txtDadosKm_Final.Focus();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A restrição " + ddlDadosSecoes.SelectedItem.Text + " - " + ddlDadosTipoRestricao.SelectedItem.Text + " já existe.' });", true);
            }
        }
        protected void lnkDadosLimpar_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            HabilitaDesabilitaCombos(true);
        }
        protected void lnkAtualizarLista_Click(object sender, EventArgs e)
        {
            AtualizarListaDeRestricoes();
        }
        protected void lnkRestricoesPorData_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Restricoes/popupRelatorioRestricoesPorData.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkRestricoesVigentes_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Restricoes/popupRelatorioRestricoesVigentes.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkRestricoesDeTemperatura_Click(object sender, EventArgs e)
        {
            Response.Write("<script> " +
                            "   var wOpen; " +
                            "   sOptions = 'status=no, menubar=no, scrollbars=yes, resizable=yes, toolbar=no'; " +
                            "   sOptions = sOptions + ', width=' + (screen.availWidth - 10).toString(); " +
                            "   sOptions = sOptions + ', height=' + (screen.availHeight - 122).toString(); " +
                            "   sOptions = sOptions + ', screenX=0, screenY=0, left=0, top=0'; " +
                            "   wOpen = window.open('/Restricoes/popupRelatorioRestricoesTemperatura.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString() + "', '', 'scrollbars=yes, resizable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0' ); " +
                            "   wOpen.focus(); " +
                            "   wOpen.moveTo( 0, 0 ); " +
                            "   wOpen.resizeTo( screen.availWidth, screen.availHeight ); " +
                            "</script>");
        }
        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            AtualizarListaDeRestricoes();
        }
        protected void lnkFiltroLimpar_Click(object sender, EventArgs e)
        {
            ddlFiltroSB.SelectedIndex = 0;
            txtFiltroKm_Inicial.Text = txtFiltroKm_Final.Text =
            txtFiltroObs.Text =
            txtFiltroNumeroRestricao.Text = string.Empty;

            AtualizarListaDeRestricoes();
        }
        protected void lnkRemoverRestricao_Click(object sender, EventArgs e)
        {
            try
            {
                if (SendMessageRRE())
                {
                    AtualizarListaDeRestricoes();
                }
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected void lnkRemoverRonda_Click(object sender, EventArgs e)
        {
            List<string> selecionados = new List<string>();

            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaRestricoes.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField1 = (HiddenField)rptListaRestricoes.Items[i].FindControl("HiddenField1");

                //Pegando o CheckBox dentro do repeater
                CheckBox chkRestricao = (CheckBox)rptListaRestricoes.Items[i].FindControl("chkRestricao");

                //Verificar se foi selecionado
                if (chkRestricao.Checked)
                {
                    selecionados.Add(string.Format("'{0}'", HiddenField1.Value));
                    //Pegar o Value e o Text dos itens selecionados do repeater

                }
            }
            var itens = string.Join(",", selecionados);
        }
        protected void lnkEdite_Click(object sender, EventArgs e)
        {
            LimpaCampos();
            LinkButton btn = (LinkButton)(sender);

            var aux = btn.CommandArgument.Split(':');

            string tipo = aux[0].ToString();
            double id = 0;

            if (tipo == "PP") id = double.Parse(aux[1].ToString());
            if (tipo == "CC") id = double.Parse(aux[2].ToString());
            if (tipo == "PC") id = double.Parse(aux[2].ToString());

            var restricaoController = new RestricaoController();
            var dados = restricaoController.ObterRestricaoPorID(tipo, id);
            if (dados != null)
            {
                ddlDadosSecoes.SelectedItem.Text = dados.Secao_Elemento.ToString();
                ddlDadosSecoes.SelectedItem.Value = dados.Secao_ElementoID.ToString();
                ddlDadosTipoRestricao.SelectedItem.Text = dados.Tipo_Restricao.ToString();
                ddlDadosTipoRestricao.SelectedItem.Value = dados.Tipo_RestricaoID.ToString();
                if (dados.SubTipo_VR != null)
                {
                    ddlDadosSubTipoVR.SelectedItem.Text = dados.SubTipo_VR.ToString();
                    ddlDadosSubTipoVR.SelectedItem.Value = dados.SubTipo_VRID.ToString();
                }
                else
                {
                    ddlDadosSubTipoVR.SelectedItem.Text = "Selecione";
                    ddlDadosSubTipoVR.SelectedItem.Value = "0";
                }

                var kms = restricaoController.ObtemKmDaSecao(dados.Secao_ElementoID);
                if (kms.Count > 0)
                    lblMensagem.Text = string.Format("de: {0} - até: {1}", kms[0], kms[1]);
                else
                    lblMensagem.Text = string.Format(" ");

                if (dados.Tipo_Restricao == "VR" || dados.Tipo_Restricao == "IF")
                {
                    txtDadosDuracao.Enabled = txtDadosVelocidade.Enabled = false;
                    ddlDadosSubTipoVR.Enabled = txtDadosDataInicial.Enabled = txtDadosHoraInicial.Enabled =
                    txtDadosDataFinal.Enabled = txtDadosHoraFinal.Enabled = txtDadosResponsavel.Enabled = txtDadosCpf.Enabled = txtTelefone.Enabled = true;
                    txtDadosDataInicial.Text = dados.Data_Inicial != null ? dados.Data_Inicial.Value.ToString().Substring(0, 10) : string.Empty;
                    txtDadosHoraInicial.Text = dados.Data_Inicial != null ? dados.Data_Inicial.Value.ToString().Substring(11, 8) : string.Empty;
                    txtDadosDataFinal.Text = dados.Data_Final != null ? dados.Data_Final.Value.ToString().Substring(0, 10) : string.Empty;
                    txtDadosHoraFinal.Text = dados.Data_Final != null ? dados.Data_Final.Value.ToString().Substring(11, 8) : string.Empty;
                    txtDadosKm_Inicio.Text = dados.Km_Inicial != null ? dados.Km_Inicial.Value.ToString() : string.Empty;
                    txtDadosKm_Final.Text = dados.Km_Final != null ? dados.Km_Final.Value.ToString() : string.Empty;
                    txtDadosResponsavel.Text = dados.Responsavel != null ? dados.Responsavel : string.Empty;
                    txtDadosCpf.Text = dados.Cpf != null ? dados.Cpf : string.Empty;
                    txtDadosObs.Text = dados.Observacao != null ? dados.Observacao : string.Empty;
                    txtTelefone.Text = dados.Telefone != null ? dados.Telefone : string.Empty;
                }
                else
                {
                    txtDadosDuracao.Enabled = txtDadosVelocidade.Enabled = true;
                    ddlDadosSubTipoVR.Enabled = txtDadosDataInicial.Enabled = txtDadosHoraInicial.Enabled =
                        txtDadosDataFinal.Enabled = txtDadosHoraFinal.Enabled = txtDadosResponsavel.Enabled = txtDadosCpf.Enabled = txtTelefone.Enabled = false;
                    txtDadosDuracao.Text = dados.Data_Inicial != null ? string.Format("{0}", (dados.Data_Final.Value - dados.Data_Inicial.Value).TotalMinutes) : string.Empty;
                    txtDadosKm_Inicio.Text = dados.Km_Inicial != null ? dados.Km_Inicial.Value.ToString() : string.Empty;
                    txtDadosKm_Final.Text = dados.Km_Final != null ? dados.Km_Final.Value.ToString() : string.Empty;
                    txtDadosVelocidade.Text = dados.Velocidade != null ? dados.Velocidade.Value.ToString() : string.Empty;
                    txtDadosObs.Text = dados.Observacao != null ? dados.Observacao : string.Empty;

                }

            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected unsafe bool SendMessageCRE()
        {
            #region [ PROPRIEDADES ]

            var restricaoController = new RestricaoController();
            Restricao rr = new Restricao();
            bool retorno = false;
            string msg = string.Empty;

            #endregion

            try
            {

                rr.Secao_Elemento = ddlDadosSecoes.SelectedItem.Value != string.Empty ? ddlDadosSecoes.SelectedItem.Text : string.Empty;
                rr.Secao_ElementoID = ddlDadosSecoes.SelectedItem.Value != string.Empty ? double.Parse(ddlDadosSecoes.SelectedItem.Value) : 0;
                rr.Tipo_Restricao = ddlDadosTipoRestricao.SelectedItem.Value != "0" ? ddlDadosTipoRestricao.SelectedItem.Text : string.Empty;
                rr.Tipo_RestricaoID = ddlDadosTipoRestricao.SelectedItem.Value != "0" ? double.Parse(ddlDadosTipoRestricao.SelectedItem.Value) : 0;

                if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "IF" || ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR")
                {
                    string dataInicial = txtDadosDataInicial.Text.Trim();
                    string dataFinal = txtDadosDataFinal.Text.Trim();
                    string horaInicial = txtDadosHoraInicial.Text.Trim() + ":00";
                    string horaFinal = txtDadosHoraFinal.Text.Trim() + ":00";

                    DataIni = Uteis.ConverteStringParaDateTime(dataInicial, horaInicial).ToOADate();
                    DataFim = Uteis.ConverteStringParaDateTime(dataFinal, horaFinal).ToOADate();
                }
                else
                {
                    DataIni = 0;
                    DataFim = 0;
                }

                rr.SubTipo_VR = ddlDadosSubTipoVR.SelectedItem.Text != "Selecione" ? ddlDadosSubTipoVR.SelectedItem.Text.Substring(0, 2) : string.Empty;
                rr.SubTipo_VRID = ddlDadosSubTipoVR.SelectedItem.Value != "0" ? double.Parse(ddlDadosSubTipoVR.SelectedItem.Value) : 0;
                rr.Responsavel = txtDadosResponsavel.Text.Length > 0 ? Uteis.RetirarAcentosCaracteresEspeciais(txtDadosResponsavel.Text) : string.Empty;
                rr.Cpf = txtDadosCpf.Text != string.Empty ? Uteis.RetirarAcentosCaracteresEspeciais(txtDadosCpf.Text) : string.Empty ;
                rr.Telefone = txtTelefone.Text != string.Empty ? Uteis.RetirarAcentosCaracteresEspeciais(txtTelefone.Text) : string.Empty;

                rr.Km_Inicial = txtDadosKm_Inicio.Text != string.Empty ? decimal.Parse(txtDadosKm_Inicio.Text) : 0;
                rr.Km_Final = txtDadosKm_Final.Text != string.Empty ? decimal.Parse(txtDadosKm_Final.Text) : 0;
                rr.Lat_Inicial = string.Empty;
                rr.Lat_Final = string.Empty;
                rr.Lon_Inicial = string.Empty;
                rr.Lon_Final = string.Empty;
                rr.Duracao = txtDadosDuracao.Text != string.Empty ? double.Parse(txtDadosDuracao.Text) : 0;
                rr.Velocidade = (txtDadosVelocidade.Text != string.Empty && txtDadosVelocidade.Text != "IF" && txtDadosVelocidade.Text != "VR") ? double.Parse(txtDadosVelocidade.Text) : 0;
                rr.Observacao = txtDadosObs.Text != string.Empty ? Uteis.RetirarAcentosCaracteresEspeciais(txtDadosObs.Text) : string.Empty;

                int IdConfirmacao = (int)restricaoController.ObtemIdRestricaoCirculacao();

                char[] responsavel = new char[20];
                char[] usuario = new char[10];
                char[] obs = new char[36];

                for (int i = 0; i <= 35; i++)
                {
                    if (i < rr.Observacao.Length)
                        obs[i] = rr.Observacao[i];
                    else
                        obs[i] = char.MinValue;
                }

                for (int i = 0; i <= 9; i++)
                {
                    if (i < lblUsuarioMatricula.Text.Length)
                        usuario[i] = lblUsuarioMatricula.Text[i];
                    else
                        usuario[i] = char.MinValue;
                }

                for (int i = 0; i <= 19; i++)
                {
                    if (i < rr.Responsavel.Length)
                        responsavel[i] = rr.Responsavel[i];
                    else
                        responsavel[i] = char.MinValue;
                }

                var dataHoraEnvio = DateTime.Now;

                DLLSendCRA((int)rr.Secao_ElementoID, (int)rr.Duracao, (int)rr.Tipo_RestricaoID, (int)rr.SubTipo_VRID,
                    (int)rr.Velocidade, (double)rr.Km_Inicial, (double)rr.Km_Final, obs, DataIni,
                    DataFim, responsavel, IdConfirmacao, usuario, 'W');

                TimeSpan horaCriacao = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

                DateTime hora = DateTime.Now;
                DateTime tempoLimite = hora.AddSeconds(30);
                bool programouRestricao = false;

                while ((DateTime.Now) < tempoLimite && programouRestricao == false)
                {
                    Thread.Sleep(3000);
                    if (((int)rr.Tipo_RestricaoID == 26) || ((int)rr.Tipo_RestricaoID == 27))
                    {
                        if (restricaoController.ChecaVR(IdConfirmacao))
                        {
                            retorno = programouRestricao = true;
                            LogDAO.GravaLogBanco(dataHoraEnvio, lblUsuarioMatricula.Text, "Restrições", null, IdConfirmacao.ToString(), "Restrição programada com sucesso. SB: " + rr.Secao_Elemento + " - TR: " + rr.Tipo_Restricao + " - STR: " + rr.SubTipo_VR + " - DTI: " + DateTime.FromOADate(DataIni) + " - DTF: " + DateTime.FromOADate(DataFim) + " - KMI: " + rr.Km_Inicial + " - KMF: " + rr.Km_Final + " - RESP: " + rr.Responsavel + " - OBS: " + rr.Observacao, Uteis.OPERACAO.Programou.ToString());
                        }
                        else
                            msg = "A Restrição não pode ser programada.";
                    }
                    else
                    {
                        if (restricaoController.ChecaRestricao(IdConfirmacao))
                        {
                            retorno = programouRestricao = true;
                            if (DataIni != 0 && DataFim != 0)
                                LogDAO.GravaLogBanco(dataHoraEnvio, lblUsuarioMatricula.Text, "Restrições", null, IdConfirmacao.ToString(), "Restrição criada com sucesso. SB: " + rr.Secao_Elemento + " - TR: " + rr.Tipo_Restricao + " - STR: " + rr.SubTipo_VR + " - DTI: " + DateTime.FromOADate(DataIni) + " - DTF: " + DateTime.FromOADate(DataFim) + " - KMI: " + rr.Km_Inicial + " - KMF: " + rr.Km_Final + " - RESP: " + rr.Responsavel + " - OBS: " + rr.Observacao, Uteis.OPERACAO.Criou.ToString());
                            else
                                LogDAO.GravaLogBanco(dataHoraEnvio, lblUsuarioMatricula.Text, "Restrições", null, IdConfirmacao.ToString(), "Restrição criada com sucesso. SB: " + rr.Secao_Elemento + " - TR: " + rr.Tipo_Restricao + " - STR: " + rr.SubTipo_VR + " - KMI: " + rr.Km_Inicial + " - KMF: " + rr.Km_Final + " - RESP: " + rr.Responsavel + " - OBS: " + rr.Observacao, Uteis.OPERACAO.Solicitou.ToString());
                        }
                        else
                            msg = "A Restrição não pode ser criada.";
                    }
                }

                if (retorno == false)
                    LogDAO.GravaLogBanco(dataHoraEnvio, lblUsuarioMatricula.Text, "Restrições", null, IdConfirmacao.ToString(), msg + " SB: " + rr.Secao_Elemento + " - TR: " + rr.Tipo_Restricao + " - STR: " + rr.SubTipo_VR + " - DTI: " + DateTime.FromOADate(DataIni) + " - DTF: " + DateTime.FromOADate(DataFim) + " - KMI: " + rr.Km_Inicial + " - KMF: " + rr.Km_Final + " - RESP: " + rr.Responsavel + " - OBS: " + rr.Observacao, Uteis.OPERACAO.Inseriu.ToString());

            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Restrições", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return retorno;
        }
        protected unsafe bool SendMessageRRE()
        {
            bool retorno = false;

            try
            {
                char[] usuario = new char[10];
                for (int i = 0; i <= 9; i++)
                {
                    if (i < lblUsuarioMatricula.Text.Length)
                        usuario[i] = lblUsuarioMatricula.Text[i];
                    else
                        usuario[i] = char.MinValue;
                }

                var selecionados = new List<Tuple<string, string, string>>();

                int contador = 0;


                if (bool.Parse(Request.Form["confirm_value"]))
                {
                    //Pegar todos os itens do repeater
                    for (int i = 0; i < rptListaRestricoes.Items.Count; i++)
                    {
                        //Pegando o HiddenField dentro do repeater
                        HiddenField HiddenField1 = (HiddenField)rptListaRestricoes.Items[i].FindControl("HiddenField1");

                        //Pegando o CheckBox dentro do repeater
                        CheckBox chkRestricao = (CheckBox)rptListaRestricoes.Items[i].FindControl("chkRestricao");

                        //Verificar se foi selecionado
                        if (chkRestricao.Checked)
                        {
                            string[] item = HiddenField1.Value.Split(':');

                            if (item[4] != "038")
                            {
                                string tipo = item[0].ToString();
                                int id = 0;

                                if (tipo == "PP") id = int.Parse(item[1].ToString());
                                if (tipo == "CC") id = int.Parse(item[2].ToString());
                                if (tipo == "PC") id = int.Parse(item[2].ToString());

                                DLLSendRRE(id, usuario);
                                LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Restrições", null, item[1].ToString(), "Foi enviado " + item[1].ToString() + " para avaliação de exclusão.", Uteis.OPERACAO.Removeu.ToString());

                                contador++;
                            }
                            else
                            {
                                var tuple = Tuple.Create("Tipo: " + item[2].ToString(), "  Seção: " + item[1].ToString(), "  Restrição nº: " + item[0].ToString());
                                selecionados.Add(tuple);
                            }
                        }
                    }

                    Thread.Sleep(3000);
                    if (selecionados.Count > 0)
                    {
                        var itens = string.Join(",", selecionados);

                        StringBuilder sb = new StringBuilder();
                        sb.Append(@"\n");

                        for (int i = 0; i < selecionados.Count; i++)
                        {
                            sb.Append(@"\n");
                            sb.Append(selecionados[i]);
                        }
                        sb.Append(@"\n");
                        sb.Append(@"\n");

                        sb.Replace("{", "");
                        sb.Replace("}", "");
                        sb.Replace("(", "");
                        sb.Replace(")", "");

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'As restrições: " + sb.ToString() + " não pode(m) ser enviada(s) para avaliação de exclusão pelo ACTWEB.' });", true);
                    }
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Foram enviadas " + contador.ToString() + " para avaliação de exclusão.' });", true);
                }
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Restrições", ex.Message.Trim());
                throw new Exception(ex.Message);
            }


            return retorno = true;
        }
        protected void AtualizarListaDeRestricoes()
        {
            ListaRestricoes = ObterListaDeRestricoes();
            rptListaRestricoes.DataSource = ListaRestricoes;
            rptListaRestricoes.DataBind();

            lblTotal.Text = string.Format("{0:0,0}", ListaRestricoes.Count);
        }
        protected void LimpaCampos()
        {
            ComboFiltroSB();
            ComboDadosSecoes();
            ComboDadosTipoRestricao();
            ComboFiltroTipo();

            ddlDadosSubTipoVR.Items.Clear();

            if (ddlDadosSubTipoVR.Items.Count == 0)
            {
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("PP - Prospecção", "6"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("EE - Equipe eletro-eletrônica", "5"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("HL - Homens trabalhando as margens da linha", "4"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("HT - Homens trabalhando na linha", "3"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("RL - Ronda de linha", "2"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("US - Ultrassom", "1"));
                ddlDadosSubTipoVR.Items.Insert(0, new ListItem("Selecione", "0"));
            }

            ddlDadosSecoes.SelectedItem.Text = "Selecione";
            ddlDadosSecoes.SelectedIndex = 0;
            ddlDadosTipoRestricao.SelectedItem.Text = "Selecione";
            ddlDadosTipoRestricao.SelectedIndex = 0;
            ddlDadosSubTipoVR.SelectedItem.Text = "Selecione";
            ddlDadosSubTipoVR.SelectedIndex = 0;
            ddlFiltroSB.SelectedItem.Text = "Selecione";
            ddlFiltroSB.SelectedIndex = 0;

            txtDadosDuracao.Text = txtDadosDataInicial.Text =
                txtDadosDataFinal.Text = txtDadosHoraInicial.Text =
                txtDadosHoraFinal.Text = txtDadosKm_Inicio.Text =
                txtDadosKm_Final.Text = txtDadosVelocidade.Text =
                txtDadosResponsavel.Text = txtDadosCpf.Text =
                txtDadosObs.Text = txtTelefone.Text = lblMensagem.Text = 
                txtFiltroKm_Inicial.Text = txtFiltroKm_Final.Text = 
                txtFiltroObs.Text = txtFiltroNumeroRestricao.Text = string.Empty;

            ddlDadosSecoes.Focus();
        }
        protected void ddlDadosSecoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var restricaoController = new RestricaoController();
            var kms = restricaoController.ObtemKmDaSecao(double.Parse(ddlDadosSecoes.SelectedItem.Value));
            if (kms.Count > 0)
                lblMensagem.Text = string.Format("de: {0} - até: {1}", kms[0], kms[1]);
            else
                lblMensagem.Text = string.Format(" ");
            ddlDadosSecoes.Focus();
        }
        protected void ddlDadosTipoRestricao_SelectedIndexChanged(object sender, EventArgs e)
        {
            var DataAtual = DateTime.Now;
            var HoraAtual = DataAtual.ToShortTimeString();
            var NovaData = DataAtual.AddHours(8);
            var NovaHora = NovaData.ToShortTimeString();


            if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "IF" || ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR")
            {
                txtDadosDataInicial.Text = DataAtual.ToShortDateString();
                txtDadosHoraInicial.Text = HoraAtual;
                //SLF104 - Luciano desabilitando preenchimento automático da data final
                //txtDadosDataFinal.Text = NovaData.ToShortDateString();
                txtDadosDataFinal.Text =
                txtDadosHoraFinal.Text = string.Empty;
            }
            else
            {
                txtDadosDataInicial.Text =
                txtDadosHoraInicial.Text =
                txtDadosDataFinal.Text =
                txtDadosHoraFinal.Text = string.Empty;
            }
            ddlDadosTipoRestricao.Focus();
        }

        protected string FormataHora(string hora)
        {
            string Retorno = hora;

            if (hora.Length == 1)
                Retorno = "0" + hora + ":00:00";
            if (hora.Length == 2)
                Retorno = hora + ":00:00";
            if (hora.Length == 3)
                Retorno = hora + "00:00";
            if (hora.Length == 4)
                Retorno = hora + "0:00";
            if (hora.Length == 5)
                Retorno = hora + ":00";
            if (hora.Length == 6)
                Retorno = hora + "00";
            if (hora.Length == 7)
                Retorno = hora + "0";

            return Retorno;
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected List<Restricao> ObterListaDeRestricoes()
        {
            decimal? _kmIni = null;
            decimal? _kmFim = null;
            double? _RestricaoID = null;


            if (txtFiltroKm_Inicial.Text.Length > 0) _kmIni = decimal.Parse(txtFiltroKm_Inicial.Text); else _kmIni = null;
            if (txtFiltroKm_Final.Text.Length > 0) _kmFim = decimal.Parse(txtFiltroKm_Final.Text); else _kmFim = null;
            if (txtFiltroNumeroRestricao.Text.Length > 0) _RestricaoID = double.Parse(txtFiltroNumeroRestricao.Text); else _RestricaoID = null;

            var _perfil = ulPerfil == "OP VP" ? "VR" : ddlFiltroTipo.SelectedItem.Value != string.Empty ? ddlFiltroTipo.SelectedItem.Value : null;
            var _subTipo = ulPerfil == "OP ELE" ? "EE" : ddlDadosSubTipoVR.SelectedItem.Value != "0" ? ddlDadosSubTipoVR.SelectedItem.Value : null;
            var _sb = ddlFiltroSB.SelectedItem.Value != string.Empty ? ddlFiltroSB.SelectedItem.Value : null;
            var _obs = txtFiltroObs.Text != string.Empty ? txtFiltroObs.Text : null;


            var restricaoController = new RestricaoController();


            var dados = restricaoController.ObterListaRestricoes(new Entities.FiltroRestricao()
            {
                RestricaoID = _RestricaoID,
                SB = _sb,
                Km_Inicial = _kmIni,
                Km_Final = _kmFim,
                Observacao = _obs,
                Tipo_Restricao = _perfil,
                Subtipo_VR = _subTipo
            });


            return dados;
        }
        #endregion

        protected void txtDadosDataFinal_TextChanged(object sender, EventArgs e)
        {

            if (ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "IF" || ddlDadosTipoRestricao.SelectedItem.Text.Substring(0, 2) == "VR")
            {
                string dataInicial = txtDadosDataInicial.Text.Trim();
                string dataFinal = txtDadosDataFinal.Text.Trim();
                string horaInicial = txtDadosHoraInicial.Text.Trim() + ":00";
                string horaFinal = txtDadosHoraFinal.Text.Trim() + ":00";

                Data1 = Uteis.ConverteStringParaDateTime(dataInicial, horaInicial);
                Data2 = Uteis.ConverteStringParaDateTime(dataFinal, horaFinal);
            }
            else
            {
                Data1 = null;
                Data2 = null;
            }

            if (Data1 != null && Data2 != null)
            {
                var dias = Data1 - Data2;


            }
        }

        //método de verificação de duplicação de subtipos de VR
        protected bool PodeCriarBS()
        {

            var restricaoController = new RestricaoController();

            double secao = double.Parse(ddlDadosSecoes.SelectedItem.Value);
            double subtipo = double.Parse(ddlDadosSubTipoVR.SelectedItem.Value);
            string dataFinal = txtDadosDataFinal.Text.Trim();
            string horaFinal = txtDadosHoraFinal.Text.Trim() + ":00";
            string dataInicial = txtDadosDataInicial.Text.Trim();
            string horaInicial = txtDadosHoraInicial.Text.Trim() + ":00";
            DateTime dataEntrada = Uteis.ConverteStringParaDateTime(dataInicial, horaInicial);
            DateTime dataFinalBSAtual = Uteis.ConverteStringParaDateTime(dataFinal, horaFinal);
            DateTime dataAtual = dataFinalBSAtual.Date;

            //Adiciona 23 horas 59 minutos e 59 segundos a data final de programação da BS
            DateTime dataFim = dataAtual.AddSeconds(86399);

            bool podeCriar = restricaoController.VerificaBSmesmoTipo(secao, subtipo, dataFinalBSAtual, dataFim, dataAtual);

            if (podeCriar)
            {
                  return true;
            }

            return false;
        }
    }
}