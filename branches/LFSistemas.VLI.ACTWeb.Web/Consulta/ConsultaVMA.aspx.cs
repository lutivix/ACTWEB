using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaVMA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Request.QueryString["mm"].ToString();

                ComboFiltroSB();
            }
        }


        #region [ CARREGA COMBOS ]

        public void ComboFiltroSB()
        {
            var VMAController = new VMAController();
            ddlFiltroSB.DataSource = VMAController.ObterFiltroSB();
            ddlFiltroSB.DataValueField = "Id";
            ddlFiltroSB.DataTextField = "Descricao";
            ddlFiltroSB.DataBind();
            ddlFiltroSB.Items.Insert(0, new ListItem("Selecione", ""));
        }

        #endregion

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            var pt = new VMAController();
            var vma = pt.ObterVMA(ddlFiltroSB.SelectedItem.Text.Trim());
            var pontosDeTroca = pt.ObterPontosDeTrocaPorSB(ddlFiltroSB.SelectedItem.Value.ToString());

            Limpar();

            if (pontosDeTroca.SB_Nome != null) // <%---------------------- COM PONTOS DE TROCA  ----------------------%>
            {
                comPontodeTroca.Visible = true;
                lblNomeSbComPontoTroca.Text = "CONSULTA DE VMA - " + vma[0].SB_Nome.ToUpper();
                lblVelocidade_VMA_ComPontoTroca_L1.Text = vma[0].Velocidade != null ? vma[0].Velocidade : string.Empty;
                lblSentido_VMA_ComPontoTroca_L1.Text = vma[0].Sentido != null ? vma[0].Sentido : string.Empty;
                lblKmInicialFinal_ComPontoTroca_L1.Text = vma[0].km_Inicial_Final != null ? vma[0].km_Inicial_Final : string.Empty;
                lblInicioFim_VMA_ComPontoTroca_L1.Text = vma[0].Inicio_Fim != null ? vma[0].Inicio_Fim : string.Empty;
                lblLatitude_VMA_ComPontoTroca_L1.Text = vma[0].Latitude_VMA != null ? vma[0].Latitude_VMA : string.Empty;
                lblLongitude_VMA_ComPontoTroca_L1.Text = vma[0].Longitude_VMA != null ? vma[0].Longitude_VMA : string.Empty;
                lblTamanhoPatio_VMA_ComPontoTroca_L1.Text = vma[0].Tamanho_Patio != null ? vma[0].Tamanho_Patio : string.Empty;

                lblVelocidade_VMA_ComPontoTroca_L2.Text = vma[1].Velocidade;
                lblSentido_VMA_ComPontoTroca_L2.Text = vma[1].Sentido;
                lblKmInicialFinal_ComPontoTroca_L2.Text = vma[1].km_Inicial_Final;
                lblInicioFim_VMA_ComPontoTroca_L2.Text = vma[1].Inicio_Fim;
                lblLatitude_VMA_ComPontoTroca_L2.Text = vma[1].Latitude_VMA;
                lblLongitude_VMA_ComPontoTroca_L2.Text = vma[1].Longitude_VMA;
                lblTamanhoPatio_VMA_ComPontoTroca_L2.Text = vma[1].Tamanho_Patio;

                if (vma[0].Reducao != null)
                {
                    if (vma[0].Reducao == "T")
                        lblReducao_VMA_ComPontoTroca.Text = "Trecho SEM redução de 10% na VMA para trens com prefixo X e SEM redução de 20% prefixo D.";
                    else
                        lblReducao_VMA_ComPontoTroca.Text = "Trecho COM redução de 10% na VMA para trens com prefixo X e COM redução de 20% prefixo D.";
                }
                else
                    lblReducao_VMA_ComPontoTroca.Text = "Trecho COM redução de 10% na VMA para trens com prefixo X e COM redução de 20% prefixo D.";

                lblPontosDeTrocaNomeSB.Text = "ÚLTIMOS PONTOS DE TROCA ENVIADOS - " + pontosDeTroca.SB_Nome.ToUpper();
                lblVelocidade_PT.Text = pontosDeTroca.Velocidade;
                lblUltimaLicenca_PT.Text = pontosDeTroca.Ultima_Licenca;
                lblKm_Troca_PT.Text = pontosDeTroca.Km_Troca;
                lblLatitude_PT.Text = pontosDeTroca.Latitude;
                lblLongitude_PT.Text = pontosDeTroca.Longitude;
            }
            else if (vma.Count > 0 && pontosDeTroca.SB_Nome == null) // <%---------------------- SEM PONTOS DE TROCA  ----------------------%>
            {
                semPontodeTroca.Visible = true;
                lblNomeSbSemPontoTroca.Text = "CONSULTA DE VMA - " + vma[0].SB_Nome.ToUpper();
                lblVelocidade_VMA_SemPontoTroca_L1.Text = vma[0].Velocidade != null ? vma[0].Velocidade : string.Empty;
                lblSentido_VMA_SemPontoTroca_L1.Text = vma[0].Sentido != null ? vma[0].Sentido : string.Empty;
                lblKmInicialFinal_SemPontoTroca_L1.Text = vma[0].km_Inicial_Final != null ? vma[0].km_Inicial_Final : string.Empty;
                lblInicioFim_VMA_SemPontoTroca_L1.Text = vma[0].Inicio_Fim != null ? vma[0].Inicio_Fim : string.Empty;
                lblLatitude_VMA_SemPontoTroca_L1.Text = vma[0].Latitude_VMA != null ? vma[0].Latitude_VMA : string.Empty;
                lblLongitude_VMA_SemPontoTroca_L1.Text = vma[0].Longitude_VMA != null ? vma[0].Longitude_VMA : string.Empty;
                lblTamanhoPatio_VMA_SemPontoTroca_L1.Text = vma[0].Tamanho_Patio != null ? vma[0].Tamanho_Patio : string.Empty;

                lblVelocidade_VMA_SemPontoTroca_L2.Text = vma[1].Velocidade;
                lblSentido_VMA_SemPontoTroca_L2.Text = vma[1].Sentido;
                lblKmInicialFinal_SemPontoTroca_L2.Text = vma[1].km_Inicial_Final;
                lblInicioFim_VMA_SemPontoTroca_L2.Text = vma[1].Inicio_Fim;
                lblLatitude_VMA_SemPontoTroca_L2.Text = vma[1].Latitude_VMA;
                lblLongitude_VMA_SemPontoTroca_L2.Text = vma[1].Longitude_VMA;
                lblTamanhoPatio_VMA_SemPontoTroca_L2.Text = vma[1].Tamanho_Patio;

                if (vma[0].Reducao != null)
                {
                    if (vma[0].Reducao == "T")
                        lblReducao_VMA_SemPontoTroca.Text = "Trecho SEM redução de 10% na VMA para trens com prefixo X e SEM redução de 20% prefixo D.";
                    else
                        lblReducao_VMA_SemPontoTroca.Text = "Trecho COM redução de 10% na VMA para trens com prefixo X e COM redução de 20% prefixo D.";
                }
                else
                    lblReducao_VMA_SemPontoTroca.Text = "Trecho COM redução de 10% na VMA para trens com prefixo X e COM redução de 20% prefixo D.";
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não existe VMA para esta SB.' });", true);
        }

        protected void lnkLImpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }

        protected void Limpar()
        {
            semPontodeTroca.Visible = comPontodeTroca.Visible = false;
            lblNomeSbSemPontoTroca.Text =
                lblNomeSbComPontoTroca.Text =
                lblPontosDeTrocaNomeSB.Text =

                lblVelocidade_VMA_ComPontoTroca_L1.Text =
                lblVelocidade_VMA_ComPontoTroca_L2.Text =
                lblVelocidade_VMA_SemPontoTroca_L1.Text =
                lblVelocidade_VMA_SemPontoTroca_L2.Text =
                lblSentido_VMA_ComPontoTroca_L1.Text =
                lblSentido_VMA_ComPontoTroca_L2.Text =
                lblSentido_VMA_SemPontoTroca_L1.Text =
                lblSentido_VMA_SemPontoTroca_L2.Text =
                lblKmInicialFinal_ComPontoTroca_L1.Text =
                lblKmInicialFinal_ComPontoTroca_L2.Text =
                lblKmInicialFinal_SemPontoTroca_L1.Text =
                lblKmInicialFinal_SemPontoTroca_L2.Text =
                lblInicioFim_VMA_ComPontoTroca_L1.Text =
                lblInicioFim_VMA_ComPontoTroca_L2.Text =
                lblInicioFim_VMA_SemPontoTroca_L1.Text =
                lblInicioFim_VMA_SemPontoTroca_L2.Text =
                lblLatitude_VMA_ComPontoTroca_L1.Text =
                lblLatitude_VMA_ComPontoTroca_L2.Text =
                lblLatitude_VMA_SemPontoTroca_L1.Text =
                lblLatitude_VMA_SemPontoTroca_L2.Text =
                lblLongitude_VMA_ComPontoTroca_L1.Text =
                lblLongitude_VMA_ComPontoTroca_L2.Text =
                lblLongitude_VMA_SemPontoTroca_L1.Text =
                lblLongitude_VMA_SemPontoTroca_L2.Text =
                lblTamanhoPatio_VMA_ComPontoTroca_L1.Text =
                lblTamanhoPatio_VMA_ComPontoTroca_L2.Text =
                lblTamanhoPatio_VMA_SemPontoTroca_L1.Text =
                lblTamanhoPatio_VMA_SemPontoTroca_L2.Text =

                lblVelocidade_PT.Text =
                lblUltimaLicenca_PT.Text =
                lblKm_Troca_PT.Text =
                lblLatitude_PT.Text =
                lblLongitude_PT.Text = string.Empty;

            ddlFiltroSB.SelectedIndex = 0;
        }
    }
}