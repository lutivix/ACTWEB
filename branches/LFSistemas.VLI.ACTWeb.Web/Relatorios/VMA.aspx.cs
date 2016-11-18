using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class VMA : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string corredores { get; set; }
        public string secoes { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

                ComboFiltroSB();
                Pesquisar(null);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void lnkSB_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EV_NOM_MAC " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EV_NOM_MAC " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkVelocidade_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_VMA_PE " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_VMA_PE " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkSentido_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_IND_LADO " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_IND_LADO " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkKM_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_KM_PE " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_KM_PE " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkInicioFim_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_IND_INI_FIM " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_IND_INI_FIM " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkLatitude_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_LAT_PE " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_LAT_PE " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkLongitude_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PE_LON_PE " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PE_LON_PE " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkComprimentoUtil_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EV_CMP_UTI " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EV_CMP_UTI " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("E.EV_IND_SIT " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("E.EV_IND_SIT " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("NM_COR_NOME " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("NM_COR_NOME " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkFiltroLimpar_Click(object sender, EventArgs e)
        {
            clbCorredor.ClearSelection();
            clbFiltroSB.ClearSelection();
            chkFiltroAllSB.Checked = false;
        }
        protected void lnkFiltroGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var secao = new List<string>();
            var corredor = new List<string>();

            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        corredor.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }
                if (corredor.Count <= 0)
                {
                    corredor.Add("1");
                    corredor.Add("2");
                    corredor.Add("3");
                    corredor.Add("4");
                    corredor.Add("5");
                }

                if (corredor.Count != clbCorredor.Items.Count)
                    corredores = string.Join(", ", corredor);
                else
                    corredores = null;

            }

            if (clbFiltroSB.Items.Count > 0)
            {
                for (int i = 0; i < clbFiltroSB.Items.Count; i++)
                {
                    if (clbFiltroSB.Items[i].Selected)
                    {
                        secao.Add(string.Format("{0}", clbFiltroSB.Items[i].Value));
                    }
                }

                if (secao.Count != clbFiltroSB.Items.Count)
                    secoes = string.Join(", ", secao);
                else
                    secoes = null;
            }
            else
                secoes = null;

            var vmaController = new VMAController();
            var itens = vmaController.ObterVMAporCorredor(secoes, corredores, null);

            sb.AppendLine("SEÇÃO;VELOCIDADE;LADO;KM INICIAL/FINAL;INICIO/FIM;LATITUDE;LONGITUDE;COMPRIMENTO ÚTIL;STATUS;CORREDOR");

            foreach (var vma in itens)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", vma.SB_Nome, vma.Velocidade, vma.Sentido, vma.km_Inicial_Final, vma.Inicio_Fim, vma.Latitude_VMA, vma.Longitude_VMA, vma.Tamanho_Patio, vma.Status, vma.Corredor));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=Relatorio_VMA.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        #endregion

        #region [ CARREGA COMBOS ]

        public void ComboFiltroSB()
        {
            var VMAController = new VMAController();
            clbFiltroSB.DataValueField = "Id";
            clbFiltroSB.DataTextField = "Descricao";
            clbFiltroSB.DataSource = VMAController.ObterFiltroSB();
            clbFiltroSB.DataBind();
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao)
        {
            var secao = new List<string>();
            var corredor = new List<string>();

            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        corredor.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }
                if (corredor.Count <= 0)
                {
                    corredor.Add("1");
                    corredor.Add("2");
                    corredor.Add("3");
                    corredor.Add("4");
                    corredor.Add("5");
                }

                if (corredor.Count != clbCorredor.Items.Count)
                    corredores = string.Join(", ", corredor);
                else
                    corredores = null;
            }

            if (clbFiltroSB.Items.Count > 0)
            {
                for (int i = 0; i < clbFiltroSB.Items.Count; i++)
                {
                    if (clbFiltroSB.Items[i].Selected)
                    {
                        secao.Add(string.Format("{0}", clbFiltroSB.Items[i].Value));
                    }
                }

                if (secao.Count != clbFiltroSB.Items.Count)
                    secoes = string.Join(", ", secao);
                else
                    secoes = null;
            }
            else
                secoes = null;

            var vmaController = new VMAController();
            var itens = vmaController.ObterVMAporCorredor(secoes, corredores, ordenacao);

            this.RepeaterItens.DataSource = itens;
            this.RepeaterItens.DataBind();

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        #endregion
    }
}