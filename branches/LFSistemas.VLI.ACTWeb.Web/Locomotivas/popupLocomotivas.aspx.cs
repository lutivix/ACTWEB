using System;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using LFSistemas.VLI.ACTWeb.Entities;
using LFSistemas.VLI.ACTWeb.Controllers;
using System.Threading;

namespace LFSistemas.VLI.ACTWeb.Web.Locomotivas
{
    public partial class popupLocomotivas : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]    

        public string Operacao { get; set; }

        LocomotivasController Loco = new LocomotivasController();

        public enum TpOperacaoMCT
        {
            _IncluirMCT      = 1,
            _ExcluirMCT      = 2,
            _AlterarMCT      = 3,
            _AlterarMCI      = 4,
            _ReinicioMCI     = 5,
            _AlteraMapaOBC   = 6,
            _AlterarLoco     = 7,
            _IncluirLoco     = 8,
            _HabilitaMCT     = 9,
            _DesabilitaMCT   = 10,
            _ExcluirLoco     = 11 
        };

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendCML(int idMCT, char[] nomeMCT, char[]  prmMatricula, char tpuser, int proprietario);

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendRML(int idMCT, char[] nomeMCT, char[] mat, char tpuser);

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendMML(int idMCT, char[] nomeMCT, int tipoMensagem, char[] mat, char tpuser, int proprietario);

        [DllImport(@"DLLMQWeb.dlll")]
        static extern void DLLSendAMC(int idMCT, int versaoMCI, int blnOBC, char[] mat, char tpuser);

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendAVO(int idMCT, double versaoMapa, double versaoOBC, char[] mat, char tpuser);

        [DllImport(@"DLLMQWeb.dll")]
        static extern void DLLSendMGC(int idMCT, char[] nomeMCT, int blnOBC);

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Request.QueryString["mm"].ToString();

                AtualizaDadosGrid();

                ViewState["Operacao"] = "Inicio";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
                divProcess.Visible = false;
                
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void btnIncluirMCT_Click(object sender, EventArgs e)
        {
            ViewState["Operacao"] = "IncluirMCT";
            HabilitaDesabilita(ViewState["Operacao"].ToString());
        }
        protected void btnAlterarMCT_Click(object sender, EventArgs e)
        {
            ddlProprietarioLocomotiva.Enabled = true;
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "AlterarMCT";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnHabilitarMCT_Click(object sender, EventArgs e)
        {
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "HabilitarMCT";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnDesabilitarMCT_Click(object sender, EventArgs e)
        {
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "DesabilitarMCT";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnExcluirMCT_Click(object sender, EventArgs e)
        {
            ViewState["Operacao"] = "ExcluirMCT";
            HabilitaDesabilita(ViewState["Operacao"].ToString());
        }
        protected void btnIncluirLoco_Click(object sender, EventArgs e)
        {
            ddlProprietarioLocomotiva.Visible = true;
            CarregaCombos("locomotivas");
            ViewState["Operacao"] = "IncluirLoco";
            HabilitaDesabilita(ViewState["Operacao"].ToString());
        }
        protected void btnAlterarLoco_Click(object sender, EventArgs e)
        {
            ddlProprietarioLocomotiva.Enabled = true;
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "AlterarLoco";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnExcluirLoco_Click(object sender, EventArgs e)
        {
            ViewState["Operacao"] = "ExcluirLoco";
            HabilitaDesabilita(ViewState["Operacao"].ToString());
        }
        protected void btnReinicioMCI_Click(object sender, EventArgs e)
        {
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "ReinicioMCI";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnVersaoMCI_Click(object sender, EventArgs e)
        {
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "VersaoMCI";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnVersaoMapa_Click(object sender, EventArgs e)
        {
            if (txtAtualiza_MCT.Text != string.Empty)
            {
                ViewState["Operacao"] = "VersaoMapa";
                HabilitaDesabilita(ViewState["Operacao"].ToString());
            }
            else
                Response.Write("<script>alert('Selecione um MCT');</script>");
        }
        protected void btnDesconectar_Click(object sender, EventArgs e)
        {
            Response.Write("<script>window.close();</script>");
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ddlProprietarioLocomotiva.Visible = false;
            ViewState["Operacao"] = "Inicio";
            HabilitaDesabilita(ViewState["Operacao"].ToString());
            LimpaDados();
            
        }
        protected void bntFiltrar_Click(object sender, EventArgs e)
        {
            AtualizaDadosGrid();
        }
        protected void bntLimpar_Click(object sender, EventArgs e)
        {
            txtFiltroLocomotiva.Text =
            txtFiltroMapa.Text =
            txtFiltroMCT.Text =
            txtFiltroVeiculo.Text = string.Empty;

            rdFiltroOrdenar.Items[0].Selected =
            rdFiltroOBC.Items[2].Selected =
            rdFiltroMacroBinaria.Items[2].Selected =
            rdFiltroAtivo.Items[2].Selected = true;
            AtualizaDadosGrid();
        }
        protected void Detalhes_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);

            var restricaoController = new LocomotivasController();

            string[] dados = btn.CommandArgument.Split(':');

            chkTem_OBC.Checked          = dados[0].Substring(1, dados[0].Length - 1) == "T" ? true : false;
            txtAtualiza_MCT.Text        = dados[1].Length <= 1 ? string.Empty : dados[1].Substring(1, dados[1].Length - 1);
            txtAtualiza_Locomotiva.Text = dados[2].Length <= 1 ? string.Empty : dados[2].Substring(1, dados[2].Length - 1);
            txtAtualiza_VersaoOBC.Text  = dados[3].Length <= 1 ? string.Empty : dados[3].Substring(1, dados[3].Length - 1);
            txtAtualiza_VersaoMapa.Text = dados[4].Length <= 1 ? string.Empty : dados[4].Substring(1, dados[4].Length - 1);
            txtAtualiza_VersaoMCI.Text  = dados[5].Length <= 1 ? string.Empty : dados[5].Substring(1, dados[5].Length - 1);
            string prop = dados[6].Length <= 1 ? string.Empty : dados[6].Substring(1, dados[6].Length - 1);

            CarregaCombos("locomotivas");
            
            if(prop == "RUMO")
            {
                ddlProprietarioLocomotiva.SelectedValue = "2";
            }
            else if (prop == "FCA")
            {
                ddlProprietarioLocomotiva.SelectedValue = "1";
            }
            else
            {
                ddlProprietarioLocomotiva.ClearSelection();
            }

            ddlProprietarioLocomotiva.Visible = true;
            ddlProprietarioLocomotiva.Enabled = false;
            
            btnCancelar.Enabled = true;
        }
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            string opcao;
            opcao = ViewState["Operacao"].ToString();
            switch (opcao)
            {
                case "IncluirMCT":
                    divProcess.Visible = true;
                    if (SendCML())                                  // Inclui MCT
                    {
                        divProcess.Visible = false;
                        Response.Write("<script>alert('A inclusão do novo MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "AlterarMCT":
                    if (SendMML((int)TpOperacaoMCT._AlterarMCT))    // Altera MCT
                    {
                        Response.Write("<script>alert('A alteração do MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "HabilitarMCT":
                    if (SendMML((int)TpOperacaoMCT._HabilitaMCT))   // Habilita MCT
                    {
                        Response.Write("<script>alert('A habilitação do MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "DesabilitarMCT":
                    if (SendMML((int)TpOperacaoMCT._DesabilitaMCT))  // Desabilita MCT
                    {
                        Response.Write("<script>alert('A desabilitação do MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }   
                    break;
                case "IncluirLoco":
                    if (SendCML())                                  // Incluir Locomotiva
                    {
                        Response.Write("<script>alert('A inclusão da nova Locomotiva foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "AlterarLoco":
                    if (SendMML((int)TpOperacaoMCT._AlterarLoco))   // Alterar Locomotiva
                    {
                        Response.Write("<script>alert('A alteração do MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "ExcluirLoco":
                    if (SendMML((int)TpOperacaoMCT._ExcluirLoco))   // Excluir Locomotiva - Desativado
                    {
                        Response.Write("<script>alert('A exclusão do MCT foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "ReinicioMCI":
                    if (SendMGC())                                  // Reinicio MCI
                    {
                        Response.Write("<script>alert('O reinício do MCI foi solicitado ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "VersaoMCI":
                    if (SendAMC())                                  // Alterar Versão MCI
                    {
                        Response.Write("<script>alert('A alteração da versão do MCI foi solicitada ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;
                case "VersaoMapa":
                    if (SendAVO())                                  // Alterar Versão OBC/Mapa
                    {
                        Response.Write("<script>alert('A alteração de Mapa/OBC foi solicitado ao ACT.');</script>");
                        HabilitaDesabilita("Inicio");
                        LimpaDados();
                        AtualizaDadosGrid();
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected unsafe bool SendCML()                     // Incluir MCT, Incluir Locomotiva
        {
            #region [ PROPRIEDADES ]

            Operacao = ViewState["Operacao"].ToString();
            bool Retorno = false;
            int idMCT = 0;
            int proprietario = 0;
            char[] usuario = new char[10];
            char[] nomemct = new char [4];

            for (int i = 0; i < txtAtualiza_Locomotiva.Text.Length; i++)
            {
                if (i < txtAtualiza_Locomotiva.Text.Length)
                    nomemct[i] = txtAtualiza_Locomotiva.Text[i];
                else
                    nomemct[i] = char.MinValue;
            }
            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }

            #endregion

            try
            {
                #region [ INCLUIR MCT ]

                if (Operacao == "IncluirMCT")
                {
                    if (txtAtualiza_MCT.Text.Trim().Length > 0)
                    {
                        idMCT = int.Parse(txtAtualiza_MCT.Text.ToString());
                        if (!Loco.existeMCT(idMCT))
                        {
                            DLLSendCML(idMCT, nomemct, usuario, 'W', 0);
                            LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A inclusão do novo MCT "+ txtAtualiza_MCT.Text +" foi solicitada ao ACT. ", Uteis.OPERACAO.Solicitou.ToString());
                            Retorno = true;
                        }
                        else
                        {
                            Response.Write("<script>alert('Já existe o MCT " + idMCT + "');</script>");
                            txtAtualiza_MCT.Focus();
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Por Favor digite um MCT!');</script>");
                        txtAtualiza_MCT.Focus();
                    }
                }

                #endregion

                #region [ INCLUIR LOCOMOTIVA ]

                else if (Operacao == "IncluirLoco")
                {
                    if (txtAtualiza_Locomotiva.Text.Trim().Length > 0)
                    {
                        idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;
                        if(ddlProprietarioLocomotiva.SelectedValue == "1")
                        {
                            proprietario = 1;
                        }
                        else if (ddlProprietarioLocomotiva.SelectedValue == "2")
                        {
                            proprietario = 2;
                        }
                        else
                        {
                            proprietario = 0;
                        }

                        if (!Loco.existeMCTName(txtAtualiza_Locomotiva.Text) || proprietario == 2)
                        {
                            DLLSendCML(idMCT, nomemct, usuario, 'W', proprietario);
                            LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A inclusão da nova Locomotiva "+ txtAtualiza_Locomotiva.Text +" foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                            Retorno = true;
                        }
                        else
                        {
                            Response.Write("<script>Já existe a Locomotiva " + txtAtualiza_Locomotiva.Text + "');</script>");
                            txtAtualiza_Locomotiva.Focus();
                        }
                    }
                    else
                    {
                        Response.Write("<script>alert('Por Favor digite uma Locomotiva!');</script>");
                        txtAtualiza_Locomotiva.Focus();
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }                    
        protected unsafe bool SendMML(int operacao)         // Alterar MCT, Alterar Locomotiva, Habilitar MCT, Desabilitar MCT, Excluir Locomotiva
        {
            #region [ PROPRIEDADES ]

            Operacao = ViewState["Operacao"].ToString();
            bool Retorno = false;
            int idMCT = 0;
            int idLoco = 0;
            int proprietario = 0;
            char[] usuario = new char[10];
            char[] nomemct = new char[4];

            idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;
            idLoco = txtAtualiza_Locomotiva.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_Locomotiva.Text.ToString()) : 0;

            for (int i = 0; i < txtAtualiza_Locomotiva.Text.Length; i++)
            {
                if (i < txtAtualiza_Locomotiva.Text.Length)
                    nomemct[i] = txtAtualiza_Locomotiva.Text[i];
                else
                    nomemct[i] = char.MinValue;
            }

            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }

            #endregion

            try
            {
                #region [ ALTERA MCT ]

                if (Operacao == "AlterarMCT")
                {
                    try
                    {
                        if (ddlProprietarioLocomotiva.SelectedValue == "1")
                        {
                            proprietario = 1;
                        }
                        else if (ddlProprietarioLocomotiva.SelectedValue == "2")
                        {
                            proprietario = 2;
                        }
                        else
                        {
                            proprietario = 0;
                        }
                        
                        if (txtAtualiza_Locomotiva.Text.Trim().Length <= 0)
                        {
                            Response.Write("<script>alert('Por Favor digite uma Locomotiva!');</script>");
                            return Retorno = false;
                        }
                        if (!Loco.existeMCTAddress(idMCT))  //Se o MCT não mais existe.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " não existe mais.');</script>");
                            return Retorno = false;
                        }
                        if ((Loco.LocomotivaAssociadaMCT(idLoco) && txtAtualiza_Locomotiva.Text != string.Empty) && (proprietario != 2)) //Se NÂO está havendo troca de locomotiva do MCT.
                        {
                            Response.Write("<script>alert('Já existe um MCT associado à Locomotiva " + txtAtualiza_Locomotiva.Text + ".');</script>");
                            return Retorno = false;
                        }
                        if (Loco.MCTCirculando(idMCT)) //Se o MCT está associado a algum trem.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " está \"circulando\" com a Locomotiva " + txtAtualiza_Locomotiva.Text + " no Trem " + Loco.MCTCirculando(idMCT) + ".');</script>");
                            return Retorno = false;
                        }
                        if ((Loco.existeMCTName(txtAtualiza_Locomotiva.Text)) && (proprietario != 2))    //Se já existe a nova locomotiva em algum MCT.
                        {
                            Response.Write("<script>alert('Já existe a Locomotiva  " + txtAtualiza_Locomotiva.Text + " e ele esta no MCT " + txtAtualiza_MCT.Text + ".');</script>");
                            return Retorno = false;
                        }

                        DLLSendMML(idMCT, nomemct, operacao, usuario, 'W', proprietario);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A alteração do MCT " + txtAtualiza_Locomotiva.Text + " foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

                #endregion

                #region [ ALTERA LOCOMOTIVA ]

                else if (Operacao == "AlterarLoco")
                {
                    try
                    {
                        idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;
                        if (ddlProprietarioLocomotiva.SelectedValue == "1")
                        {
                            proprietario = 1;
                        }
                        else if (ddlProprietarioLocomotiva.SelectedValue == "2")
                        {
                            proprietario = 2;
                        }
                        else
                        {
                            proprietario = 0;
                        }


                        if (txtAtualiza_MCT.Text.Trim().Length <= 0)
                        {
                            Response.Write("<script>alert('Por Favor digite um MCT!');</script>");
                            return Retorno = false;
                        }
                        if (Loco.locoSelecionada(idLoco) == idLoco.ToString())
                        {
                            Response.Write("<script>alert('A Locomotiva " + txtAtualiza_Locomotiva.Text + " está em Operação no Sistema ACT e não pode ser removida! Finalize as operações e depois tente novamente!');</script>");
                            return Retorno = false;
                        }
                        if (Loco.MCTCirculando(idMCT)) //Se o MCT está associado a algum trem.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " está \"circulando\" com a Locomotiva " + txtAtualiza_Locomotiva.Text + " no Trem " + Loco.MCTCirculando(idMCT) + ".');</script>");
                            return Retorno = false;
                        }

                        DLLSendMML(idMCT, nomemct, operacao, usuario, 'W', proprietario);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A alteração do MCT " + txtAtualiza_MCT.Text + " foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }                        
                }

                #endregion

                #region [ HABILITAR MCT ]

                else if (Operacao == "HabilitarMCT")
                {
                    try
                    {
                        idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;

                        if (txtAtualiza_MCT.Text.Trim().Length <= 0)
                        {
                            Response.Write("<script>alert('Por Favor selecione um MCT!');</script>");
                            return Retorno = false;
                        }
                        if (!Loco.existeMCTAddress(idMCT))  //Se o MCT não mais existe.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " não existe mais.');</script>");
                            return Retorno = false;
                        }
                        if (!Loco.existeMCTName(txtAtualiza_Locomotiva.Text))    //Se o MCT não está mais na mesma locomotiva.
                        {
                            Response.Write("<script>alert('O MCT   " + txtAtualiza_MCT.Text + "  não está mais associado á Locomotiva  " + txtAtualiza_Locomotiva.Text + ".');</script>");
                            return Retorno = false;
                        }
                        if (Loco.MCTCirculando(idMCT)) //Se o MCT está associado a algum trem.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " está \"circulando\" com a Locomotiva " + txtAtualiza_Locomotiva.Text + " no Trem " + Loco.MCTCirculando(idMCT) + ".');</script>");
                            return Retorno = false;
                        }

                        DLLSendMML(idMCT, nomemct, operacao, usuario, 'W', 0);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A habilitação do MCT " + txtAtualiza_MCT.Text + " foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }    
                }

                #endregion

                #region [ DESABILITAR MCT ]

                else if (Operacao == "DesabilitarMCT")
                {
                    try
                    {
                        idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;

                        if (txtAtualiza_MCT.Text.Trim().Length <= 0)
                        {
                            Response.Write("<script>alert('Por Favor selecione um MCT!');</script>");
                            return Retorno = false;
                        }
                        if (!Loco.existeMCTAddress(idMCT))  //Se o MCT não mais existe.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " não existe mais.');</script>");
                            return Retorno = false;
                        }
                        if (!Loco.existeMCTName(txtAtualiza_Locomotiva.Text))    //Se o MCT não está mais na mesma locomotiva.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " não está mais associado á Locomotiva " + txtAtualiza_Locomotiva.Text + ".');</script>");
                            return Retorno = false;
                        }
                        if (Loco.MCTCirculando(idMCT)) //Se o MCT está associado a algum trem.
                        {
                            Response.Write("<script>alert('O MCT " + txtAtualiza_MCT.Text + " está \"circulando\" com a Locomotiva " + txtAtualiza_Locomotiva.Text + " no Trem " + Loco.MCTCirculando(idMCT) + ".');</script>");
                            return Retorno = false;
                        }

                        DLLSendMML(idMCT, nomemct, operacao, usuario, 'W', 0);
                        LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A desabilitação do MCT " + txtAtualiza_MCT.Text + " foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    } 
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno = true;
        }
        protected unsafe bool SendAMC()                     // Altera Versão MCI
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            int idMCT = 0;
            int idMCI = 0;
            char[] usuario = new char[10];
            int temOBC = chkTem_OBC.Checked ? 1 : 0;

            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }
            if (txtAtualiza_MCT.Text.Trim().Length > 0)
                idMCT = int.Parse(txtAtualiza_MCT.Text.ToString());
            if (txtAtualiza_VersaoMCI.Text.Trim().Length > 0)
                idMCI = int.Parse(txtAtualiza_VersaoMCI.Text.ToString());

            #endregion

            try
            {
                #region [ ALTERA VERSÃO MCI ]

                DLLSendAMC(idMCT, idMCI, temOBC, usuario, 'W');
                LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A alteração da versão do MCI " + txtAtualiza_VersaoMCI.Text + " - OBC " + chkTem_OBC.Checked.ToString() +" foi solicitada ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                Retorno = true;

                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }
            
            return Retorno;
        }
        protected unsafe bool SendAVO()                     // Altera Versão OBC/Mapa
        {
            #region [ PROPRIEDADES ]

            Operacao = ViewState["Operacao"].ToString();
            bool Retorno = false;            
            int idMCT = 0;
            double idMapa = 0;
            double idOBC = 0;
            char[] usuario = new char[10];

            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }

            if (txtAtualiza_MCT.Text.Trim().Length > 0)
                idMCT = int.Parse(txtAtualiza_MCT.Text.ToString());
            if (txtAtualiza_VersaoMCI.Text.Trim().Length > 0)
                idMapa = double.Parse(txtAtualiza_VersaoMapa.Text.ToString());
            if (txtAtualiza_VersaoOBC.Text.Trim().Length > 0)
                idOBC = double.Parse(txtAtualiza_VersaoOBC.Text.ToString());

            #endregion

            try
            {
                #region [ ALTERA VERSÃO OBC/MAPA ]

                DLLSendAVO(idMCT, idMapa, idOBC, usuario, 'W');
                LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "A alteração de Mapa " + txtAtualiza_VersaoMapa.Text + "/ OBC "+ txtAtualiza_VersaoOBC.Text +" foi solicitado ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                Retorno = true;
                
                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }
        protected unsafe bool SendMGC()                     // Reinicio MCI 
        {
            #region [ PROPRIEDADES ]

            Operacao = ViewState["Operacao"].ToString();
            bool Retorno = false;
            int idMCT = 0;
            char[] nomemct = new char[4];
            int temOBC = chkTem_OBC.Checked ? 1 : 0;

            idMCT = txtAtualiza_MCT.Text.Trim().Length > 0 ? int.Parse(txtAtualiza_MCT.Text.ToString()) : 0;

            for (int i = 0; i < txtAtualiza_Locomotiva.Text.Length; i++)
            {
                if (i < txtAtualiza_Locomotiva.Text.Length)
                    nomemct[i] = txtAtualiza_Locomotiva.Text[i];
                else
                    nomemct[i] = char.MinValue;
            }

            #endregion

            try
            {
                #region [ REINICIO DE MCI ]

                DLLSendMGC(idMCT, nomemct, temOBC);
                LogDAO.GravaLogBanco(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", null, idMCT.ToString(), "O reinício do MCI " + txtAtualiza_VersaoMCI.Text + " foi solicitado ao ACT.", Uteis.OPERACAO.Solicitou.ToString());
                
                Retorno = true;

                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }
        protected unsafe bool SendRML()
        {
            #region [ PROPRIEDADES ]

            bool Retorno = false;
            int idMCT = 0;
            char[] usuario = new char[10];
            char[] nomemct = new char[4];

            for (int i = 0; i < 4; i++)
            {
                nomemct[i] = char.MinValue;
            }

            for (int i = 0; i <= 9; i++)
            {
                if (i < lblUsuarioMatricula.Text.Length)
                    usuario[i] = lblUsuarioMatricula.Text[i];
                else
                    usuario[i] = char.MinValue;
            }

            if (txtAtualiza_MCT.Text.Trim().Length > 0)
                idMCT = int.Parse(txtAtualiza_MCT.Text.ToString());

            #endregion

            try
            {
                #region [  ]

                DLLSendCML(idMCT, nomemct, usuario, 'W', 0);
                Retorno = true;

                #endregion
            }
            catch (Exception ex)
            {
                LogDAO.GravaLogSistema(DateTime.Now, lblUsuarioMatricula.Text, "Locomotivas", ex.Message.Trim());
                throw new Exception(ex.Message);
            }

            return Retorno;
        }

        protected void LimpaDados()
        {
            txtAtualiza_Locomotiva.Text =
            txtAtualiza_MCT.Text  =
            txtAtualiza_VersaoMapa.Text =
            txtAtualiza_VersaoMCI.Text =
            txtAtualiza_VersaoOBC.Text = string.Empty;
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void AtualizaDadosGrid()
        {
            Thread.Sleep(2000);
            var locoController = new LocomotivasController();
            var itens = locoController.ObterListaLocomotivas(new Entities.FiltroLocomotivas()
            {
                Ordem = rdFiltroOrdenar.Items[0].Selected ? true : false,
                Obc = rdFiltroOBC.Items[0].Selected ? "T" : rdFiltroOBC.Items[1].Selected ? "F" : "",
                Macro = rdFiltroMacroBinaria.Items[0].Selected ? "T" : rdFiltroMacroBinaria.Items[1].Selected ? "F" : "",
                Ativo = rdFiltroAtivo.Items[0].Selected ? "H" : rdFiltroAtivo.Items[1].Selected ? "D" : "",
                Veiculo = txtFiltroVeiculo.Text,
                Tipo_Locomotiva = txFiltroTiopoLocomotiva.Text,
                Mapa = txtFiltroMapa.Text,
                Locomotiva = txtFiltroLocomotiva.Text,
                Mct = txtFiltroMCT.Text
            });

            rptLocomotivas.DataSource = itens;
            rptLocomotivas.DataBind();

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        protected void CarregaCombos(string origem)
        {
            var pesquisa = new ComboBoxController();

            var proprietario = pesquisa.CarregaCombo_Proprietario(origem);
            if (proprietario.Count > 0)
            {
                ddlProprietarioLocomotiva.DataValueField = "ID";
                ddlProprietarioLocomotiva.DataTextField = "DESCRICAO";
                ddlProprietarioLocomotiva.DataSource = proprietario;
                ddlProprietarioLocomotiva.DataBind();
                ddlProprietarioLocomotiva.Items.Insert(0, new ListItem("Selecione", ""));
            }
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DE TELA ]

        protected void HabilitaDesabilita(string operacao)
        {
            #region HabilitaDesabilita
            switch (operacao)
            {
                case "Inicio":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled =
                    btnDesconectar.Enabled = true;

                    btnExcluirLoco.Enabled =
                    btnExcluirMCT.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = false;

                    lblTem_OBC.Visible =
                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled = 
                    txtAtualiza_Locomotiva.Enabled = 
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    break;
                case "IncluirMCT":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = 
                    txtAtualiza_MCT.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_Locomotiva.Enabled = 
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    txtAtualiza_MCT.Focus();

                    break;
                case "AlterarMCT":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled =
                    txtAtualiza_Locomotiva.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled = 
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = true;

                    txtAtualiza_Locomotiva.Focus();

                    break;
                case "HabilitarMCT":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    btnConfirmar.Focus();
                    break;
                case "DesabilitarMCT":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled =  
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    btnConfirmar.Focus();
                    break;
                case "ExcluirMCT":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled =  
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    btnConfirmar.Focus();
                    break;
                case "IncluirLoco":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = true;

                    txtAtualiza_Locomotiva.Focus();

                    break;

                case "AlterarLoco":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    txtAtualiza_MCT.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = true;

                    txtAtualiza_Locomotiva.Focus();

                    break;
                    break;
                case "ExcluirLoco":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled =  
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    btnConfirmar.Focus();
                    break;
                case "ReinicioMCI":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    lblTem_OBC.Visible =
                    chkTem_OBC.Visible = 
                    btnDesconectar.Enabled =
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled = 
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    btnConfirmar.Focus();
                    break;
                case "VersaoMCI":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    lblTem_OBC.Visible =
                    chkTem_OBC.Visible = 
                    btnDesconectar.Enabled =
                    txtAtualiza_VersaoMCI.Enabled =                    
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoOBC.Enabled = 
                    txtAtualiza_VersaoMapa.Enabled =  false;
                    ddlProprietarioLocomotiva.Visible = false;

                    txtAtualiza_VersaoMCI.Focus();
                    break;
                case "VersaoMapa":
                    btnIncluirMCT.Enabled = 
                    btnAlterarMCT.Enabled = 
                    btnHabilitarMCT.Enabled = 
                    btnDesabilitarMCT.Enabled = 
                    btnIncluirLoco.Enabled = 
                    btnAlterarLoco.Enabled = 
                    btnExcluirLoco.Enabled = 
                    btnReinicioMCI.Enabled = 
                    btnVersaoMCI.Enabled = 
                    btnVersaoMapa.Enabled = 
                    btnExcluirMCT.Enabled = false;

                    btnDesconectar.Enabled =
                    txtAtualiza_VersaoOBC.Enabled =
                    txtAtualiza_VersaoMapa.Enabled =                                       
                    btnCancelar.Enabled = 
                    btnConfirmar.Enabled = true;

                    chkTem_OBC.Visible = 
                    txtAtualiza_MCT.Enabled =
                    txtAtualiza_Locomotiva.Enabled =
                    txtAtualiza_VersaoMCI.Enabled = false;
                    ddlProprietarioLocomotiva.Visible = false;

                    txtAtualiza_VersaoMapa.Focus();
                    break;
            }
            #endregion
        }

        #endregion

    }
}