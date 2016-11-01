using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Informacao_OBC : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string Locos { get; set; }

        public int UserListCount { get; set; }
        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_NowViewing"] = value;
            }
        }
        public enum Navigation
        {
            None,
            Primeira,
            Proxima,
            Anterior,
            Ultima,
            Pager,
            Sorting
        }
        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            Locos = Uteis.Descriptografar(Request.QueryString["locos"].ToString(), "a#3G6**@").ToUpper();

            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

                Pesquisar(null, Navigation.None);
            }

        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkFiltroPesquisar_Click(object sender, EventArgs e)
        {
            Locos = txtLocomotivas.Text.Length > 0 ? txtLocomotivas.Text : string.Empty;
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            clbCorredor.ClearSelection();
            txtLocomotivas.Text = Locos = string.Empty;
            txtLocomotivas.Focus();
            Pesquisar(null, Navigation.None);
        }
        protected void lnkGerarExcel_Click(object sender, EventArgs e)
        {
            Locos = txtLocomotivas.Text.Length > 0 ? txtLocomotivas.Text : string.Empty;
            GeraExcel(null);
        }

        protected void lnkAtivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("IOBC.OBC_ATIVO_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("IOBC.OBC_ATIVO_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_NOM_MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_NOM_MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("IOBC.OBC_CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("IOBC.OBC_CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkFrota_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("IOBC.OBC_FROTA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("IOBC.OBC_FROTA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkMCI_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_IND_MCI " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_IND_MCI " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkOBC_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_IND_OBC " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_IND_OBC " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR.MR_PRF_ACT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR.MR_PRF_ACT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkUltima_Comunicacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_TIMESTAMP_MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_TIMESTAMP_MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkAtualizacao_OBC_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_DT_ATUALI_OBC " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_DT_ATUALI_OBC " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkAtualizacao_Mapa_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_DT_ATUALI_MAP " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_DT_ATUALI_MAP " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkVersao_Mapa_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_MAP_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_MAP_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkVersao_OBC_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_OBC_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_OBC_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkVersao_MCT_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MCT.MCT_CNV_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MCT.MCT_CNV_VERSAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkProximo_a_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MR.MR_LAND_MARK " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MR.MR_LAND_MARK " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {
            //Fill repeater for Previous event
            Pesquisar(null, Navigation.Anterior);
        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Next event
            Pesquisar(null, Navigation.Proxima);
        }
        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for First event
            Pesquisar(null, Navigation.Primeira);
        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Last event
            Pesquisar(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;
            Pesquisar(null, Navigation.Anterior);
        }

        #endregion

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var rel = new OBCController();

            string corredor = null;
            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));

                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add(string.Format("'{0}'", "Centro Leste"));
                    aux.Add(string.Format("'{0}'", "Centro Sudeste"));
                    aux.Add(string.Format("'{0}'", "Minas Bahia"));
                    aux.Add(string.Format("'{0}'", "Minas Rio"));
                    aux.Add(string.Format("'{0}'", "Centro Norte"));
                }
            }
            corredor = string.Join(",", aux);

            var itens = rel.RelatorioOBC(Locos, corredor, ordenacao);

            if (itens.Count > 0)
            {
                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = itens;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

                switch (navigation)
                {
                    case Navigation.Proxima:
                        NowViewing++;
                        break;
                    case Navigation.Anterior:
                        NowViewing--;
                        break;
                    case Navigation.Ultima:
                        NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(ddlPageSize.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkProximaPagina.Enabled = !objPds.IsLastPage;
                lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkUltimaPagina.Enabled = !objPds.IsLastPage;

                RepeaterInformacao_OBC.DataSource = objPds;
                RepeaterInformacao_OBC.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count); 
        }

        protected void GeraExcel(string ordenacao)
        {
            StringBuilder sb = new StringBuilder();
            var rel = new OBCController();

            string corredor = null;
            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));

                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add(string.Format("'{0}'", "Centro Leste"));
                    aux.Add(string.Format("'{0}'", "Centro Sudeste"));
                    aux.Add(string.Format("'{0}'", "Minas Bahia"));
                    aux.Add(string.Format("'{0}'", "Minas Rio"));
                    aux.Add(string.Format("'{0}'", "Norte Sul"));
                }
            }
            corredor = string.Join(",", aux);

            var itens = rel.RelatorioOBC(Locos, corredor, ordenacao);

            sb.AppendLine("LOCO; CORREDOR; FROTA; MCI; OBC; VERSÃO MCT; VERSÃO OBC; VERSÃO MAPA; ÚLTIMA COMUNICAÇÃO; ATUALIZAÇÃO OBC; ATUALIZAÇÃO MAPA");

            foreach (var item in itens)
            {
                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", item.Loco, item.Corredor, item.Frota, item.MCI, item.Tem_OBC, item.Versao_MCT, item.Versao_OBC, item.Versao_MAPA, item.Ultima_Comunicacao, item.Atualizacao_OBC, item.Atualizacao_Mapa));
            }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=informacao_obc.csv");
            Response.Write(sb.ToString());
            Response.End();
        }

        void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill repeater for Pager event
            Pesquisar(null, Navigation.Pager);
        }
        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            ddlPageSize.SelectedIndexChanged += new EventHandler(ddlPageSize_SelectedIndexChanged);
        }

        #endregion  
    }
}