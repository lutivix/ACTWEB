using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaQuadroTracao : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public List<QuadroTracao> itens { get; set; }
        public string corredores { get; set; }
        public string grupos { get; set; }
        public string TiposLoco { get; set; }

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

        protected override void OnInit(EventArgs e)
        {
            abaDados.Voltar += new Abas.QuadroTracao.VoltarEventHandler(Voltar);

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();
                ViewState["ordenacao"] = "ASC";

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula.ToUpper();
                lblUsuarioPerfil.Text = ulPerfil.ToUpper();
                lblUsuarioMaleta.Text = ulMaleta.ToUpper();

                CarregaCombos();
                Pesquisar(null, Navigation.None);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLoadLocalidades_Click(object sender, EventArgs e)
        {
            CarregaCombos();
        }
        protected void cblCorredores_SelectedIndexChanged(object sender, EventArgs e)
        {
            var auxCorredor = new List<string>();

            if (cblCorredor.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredor.Items.Count; i++)
                {
                    if (cblCorredor.Items[i].Selected)
                    {
                        auxCorredor.Add(string.Format("'{0}'", cblCorredor.Items[i].Value));
                    }
                }

                corredores = string.Join(",", auxCorredor);
            }

            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxLocalidades(corredores);
            cblGrupos.DataBind();

            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    cblGrupos.Items[i].Selected = true;

                }

            }

        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //txtFiltroMotivo.Text = string.Empty;
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
            Pesquisar(null, Navigation.None);
        }
        protected void lnkExportar_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
                string result = string.Empty;
                DateTime horaFim = DateTime.Now;
                var pesquisar = new QuadroTracaoController();

                var auxCorredor = new List<string>();
                if (cblCorredor.Items.Count > 0)
                {
                    for (int i = 0; i < cblCorredor.Items.Count; i++)
                    {
                        if (cblCorredor.Items[i].Selected)
                        {
                            auxCorredor.Add(string.Format("'{0}'", cblCorredor.Items[i].Value));
                        }
                    }

                    corredores = string.Join(",", auxCorredor);
                }

                var auxGrupo = new List<string>();
                if (cblGrupos.Items.Count > 0)
                {
                    for (int i = 0; i < cblGrupos.Items.Count; i++)
                    {
                        if (cblGrupos.Items[i].Selected)
                        {
                            auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
                        }
                    }

                    grupos = string.Join(",", auxGrupo);
                }

                var auxTpLoco = new List<string>();
                if (cblTiposLoco.Items.Count > 0)
                {
                    for (int i = 0; i < cblTiposLoco.Items.Count; i++)
                    {
                        if (cblTiposLoco.Items[i].Selected)
                        {
                            auxTpLoco.Add(string.Format("'{0}'", cblTiposLoco.Items[i].Value));
                        }
                    }

                    TiposLoco = string.Join(",", auxTpLoco);
                }

                itens = pesquisar.ObterPorFiltro(grupos, TiposLoco);

                if (itens.Count > 0)
                {
                    sb.AppendLine("TIPO LOCO;CORREDOR; ROTA; ESTACAO ORIGEM; ESTACAO DESTINO; IDA/VOLTA; CAPAC.TRACAO;");


                     foreach (var Quadro in itens)
                     {
                         sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", Quadro.Locomotiva_TP, Quadro.Corredor_DS, Quadro.Rota_DS, Quadro.Estacao_Orig_ID, Quadro.Estacao_Dest_ID, Quadro.Ida_Volta_DS, Quadro.Capac_Tracao_QT.ToString()));
                     }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                }

            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=quadro_tracao.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
        protected void lnkAcao_Click(object sender, EventArgs e)
        {
            if (lblUsuarioPerfil.Text == "SUP" || lblUsuarioPerfil.Text == "ADM")
            {
                LinkButton btn = (LinkButton)(sender);
                int id = int.Parse(btn.CommandArgument);

                abaDados.CarregaDados(id);
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
                pnlFiltros.Enabled = false;
            }
            else
                Response.Write("<script>alert('Usuário não tem permissão para acessar esta opção, se necessário comunique ao Supervisor do CCO.'); </script>");
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
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

        #region [ EVENTOS ]

        protected void Voltar()
        {
            tabAbas.ActiveTabIndex = 0;
            tpAcao.Enabled = false;
            tpPesquisa.Enabled = true;
            pnlFiltros.Enabled = true;
            Pesquisar(null, Navigation.None);
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var pesquisar = new QuadroTracaoController();

            var auxCorredor = new List<string>();
            if (cblCorredor.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredor.Items.Count; i++)
                {
                    if (cblCorredor.Items[i].Selected)
                    {
                        auxCorredor.Add(string.Format("'{0}'", cblCorredor.Items[i].Value));
                    }
                }

                corredores = string.Join(",", auxCorredor);
            }

            var auxGrupo = new List<string>();
            if (cblGrupos.Items.Count > 0)
            {
                for (int i = 0; i < cblGrupos.Items.Count; i++)
                {
                    if (cblGrupos.Items[i].Selected)
                    {
                        auxGrupo.Add(string.Format("'{0}'", cblGrupos.Items[i].Value));
                    }
                }

                grupos = string.Join(",", auxGrupo);
            }

            var auxTpLoco = new List<string>();
            if (cblTiposLoco.Items.Count > 0)
            {
                for (int i = 0; i < cblTiposLoco.Items.Count; i++)
                {
                    if (cblTiposLoco.Items[i].Selected)
                    {
                        auxTpLoco.Add(string.Format("'{0}'", cblTiposLoco.Items[i].Value));
                    }
                }

                TiposLoco = string.Join(",", auxTpLoco);
            }

            itens = pesquisar.ObterPorFiltro(grupos, TiposLoco);

            if (itens.Count > 0)
            {

                //switch (ordenacao)
                //{
                //    case "TREM ASC":
                //        itens = itens.OrderBy(o => o.Trem).ToList();
                //        break;
                //    case "TREM DESC":
                //        itens = itens.OrderByDescending(o => o.Trem).ToList();
                //        break;
                //    case "LOCO ASC":
                //        itens = itens.OrderBy(o => o.Loco).ToList();
                //        break;
                //    case "LOCO DESC":
                //        itens = itens.OrderByDescending(o => o.Loco).ToList();
                //        break;
                //    case "TIPO ASC":
                //        itens = itens.OrderBy(o => o.Tipo_Loco).ToList();
                //        break;
                //    case "TIPO DESC":
                //        itens = itens.OrderByDescending(o => o.Tipo_Loco).ToList();
                //        break;
                //    case "SITUACAO ASC":
                //        itens = itens.OrderBy(o => o.Situacao).ToList();
                //        break;
                //    case "SITUACAO DESC":
                //        itens = itens.OrderByDescending(o => o.Situacao).ToList();
                //        break;
                //    case "CORREDOR ASC":
                //        itens = itens.OrderBy(o => o.Corredor).ToList();
                //        break;
                //    case "CORREDOR DESC":
                //        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                //        break;
                //    case "Atualizacao ASC":
                //        itens = itens.OrderBy(o => o.Atualizacao).ToList();
                //        break;
                //    case "Atualizacao DESC":
                //        itens = itens.OrderByDescending(o => o.Atualizacao).ToList();
                //        break;
                //    case "ATIVO ASC":
                //        itens = itens.OrderBy(o => o.Ativo_SN).ToList();
                //        break;
                //    case "ATIVO DESC":
                //        itens = itens.OrderByDescending(o => o.Ativo_SN).ToList();
                //        break;
                //    default:
                //        itens = itens.OrderBy(o => o.Loco).ToList();
                //        break;
                //}

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

                RepeaterItens.DataSource = objPds;
                RepeaterItens.DataBind();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não encontrou registros.' });", true);
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            }

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }
        protected void CarregaCombos()
        {
            var auxCorredor = new List<string>();

            if (cblCorredor.Items.Count > 0)
            {
                for (int i = 0; i < cblCorredor.Items.Count; i++)
                {
                    if (cblCorredor.Items[i].Selected)
                    {
                        auxCorredor.Add(string.Format("'{0}'", cblCorredor.Items[i].Value));
                    }
                }

                corredores = string.Join(",", auxCorredor);
            }

            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxLocalidades(corredores);
            cblGrupos.DataBind();

            cblTiposLoco.DataValueField = "Id";
            cblTiposLoco.DataTextField = "Descricao";
            cblTiposLoco.DataSource = combo.ComboBoxTipoLocomotivas();
            cblTiposLoco.DataBind();
        }

        #endregion

    }
}