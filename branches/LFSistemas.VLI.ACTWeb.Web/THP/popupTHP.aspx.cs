using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LFSistemas.VLI.ACTWeb.Web.THP
{
    public partial class popupTHP : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<TremHoraParado> itens { get; set; }
        public string op;
        public string corredores { get; set; }
        public string grupos { get; set; }
        public string motivos { get; set; }
        public string categorias { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            op = Request.QueryString["op"];
            if (!Page.IsPostBack)
            {
                CarregaCombos();
                Pesquisar(null);
            }
        }
        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            cblMotivos.ClearSelection();
            cblCorredor.ClearSelection();
            cblGrupos.ClearSelection();
            cblCategorias.ClearSelection();
            Pesquisar(null);
        }
        protected void lnkCodigo_OS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Codigo_OS " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Codigo_OS " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkPrefixo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Prefixo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Prefixo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkLocal_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Local " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Local " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkTempo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Tempo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Tempo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkMotivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Motivo " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Motivo " + ViewState["ordenacao"].ToString());
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString());
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("Corredor " + ViewState["ordenacao"].ToString());
            }
        }
        /*protected void lnkAcao_Click(object sender, EventArgs e)
        {
            if (lblUsuarioPerfil.Text == "SUP" || lblUsuarioPerfil.Text == "ADM")
            {
                LinkButton btn = (LinkButton)(sender);
                double id = double.Parse(btn.CommandArgument);

                abaDados.CarregaDados(id);
                tabAbas.ActiveTabIndex = 1;
                tpAcao.Enabled = true;
                tpPesquisa.Enabled = false;
                pnlFiltros.Enabled = false;
            }
            else
                Response.Write("<script>alert('Usuário não tem permissão para acessar esta opção, se necessário comunique ao Supervisor do CCO.'); </script>");
        }*/

        #endregion

        #endregion
     
        #region [ MÉTODOS DE APOIO ]
        protected void Pesquisar(string ordenacao)
        {
            var pesquisar = new THPController();
            pesquisar.flag = op;

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
                        auxGrupo.Add(string.Format("{0}", cblGrupos.Items[i].Value));
                    }
                }

                grupos = string.Join(",", auxGrupo);
            }

            var auxMotivo = new List<string>();
            if (cblMotivos.Items.Count > 0)
            {
                for (int i = 0; i < cblMotivos.Items.Count; i++)
                {
                    if (cblMotivos.Items[i].Selected)
                    {
                        auxMotivo.Add(string.Format("'{0}'", cblMotivos.Items[i].Value));
                    }
                }

                motivos = string.Join(",", auxMotivo);
            }

            var auxCategoria = new List<string>();
            if (cblCategorias.Items.Count > 0)
            {
                for (int i = 0; i < cblCategorias.Items.Count; i++)
                {
                    if (cblCategorias.Items[i].Selected)
                    {
                        auxCategoria.Add(string.Format("{0}", cblCategorias.Items[i].Value));
                    }
                }

                categorias = string.Join(",", auxCategoria);
            }

            itens = pesquisar.ObterPorFiltro(new TremHoraParado()
            {
                Motivo = motivos,
                Corredor_ID = corredores,
                Grupo_ID = grupos,
                Categoria = categorias
            });

            
            if (itens.Count > 0)
            {
                itens = itens.OrderByDescending(o => o.Intervalo).ToList();

                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();

            }
            else
            {
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();
            }

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        protected void CarregaCombos()
        {
            var combo = new ComboBoxController();
            cblGrupos.DataValueField = "Id";
            cblGrupos.DataTextField = "Descricao";
            cblGrupos.DataSource = combo.ComboBoxGrupos();
            cblGrupos.DataBind();

            cblMotivos.DataValueField = "Id";
            cblMotivos.DataTextField = "Descricao";
            cblMotivos.DataSource = combo.ComboBoxMotivoParadaTremCOMId();
            cblMotivos.DataBind();

            cblCategorias.DataValueField = "Id";
            cblCategorias.DataTextField = "Descricao";
            cblCategorias.DataSource = combo.ComboBoxCategorias();
            cblCategorias.DataBind();
        }

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
            Pesquisar(null);
        }

        #endregion
    }
}