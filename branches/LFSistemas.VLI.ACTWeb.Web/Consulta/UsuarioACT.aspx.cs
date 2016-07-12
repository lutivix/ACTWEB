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
    public partial class UsuarioACT : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<UsuariosACT> itens { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
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

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                ViewState["ordenacao"] = "ASC";

                //CarregarPerfis();
                
                Pesquisar(null, Navigation.None);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            Pesquisar(null, Navigation.None);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //ddlPerfis.SelectedIndex = 0;
            txtMatricula.Text = string.Empty;
            txtNome.Text = string.Empty;
            Pesquisar(null, Navigation.None);
        }
        protected void lnkNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/UsuarioACT.aspx?matricula=" + Uteis.Criptografar("novo", "a#3G6**@") + "&flag=novousuario&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }
        protected void lnkUsuariosACT_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)(sender);
            string matricula = btn.CommandArgument;
            Response.Redirect("/Cadastro/UsuarioACT.aspx?matricula=" + Uteis.Criptografar(matricula.ToLower(), "a#3G6**@") + "&flag=consulta&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }

        protected void lnkMatricula_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MATRICULA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MATRICULA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkNome_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("NOME " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("NOME " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkPerfil_Click(object sender, EventArgs e)
        {

        }
        protected void lnkEmail_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EMAIL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EMAIL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkMaleta_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MALETA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MALETA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkUltimoAcesso_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ULT. ACESSO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ULT. ACESSO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkAtivo_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ATIVO_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ATIVO_SN " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkCPF_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("CPF " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("CPF " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkLDL_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("LDL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("LDL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkPerfil_Click1(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ABREVIADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ABREVIADO " + ViewState["ordenacao"].ToString(), Navigation.None);
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

        #region [ MÉTODOS DE APOIO ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var usuarioController = new UsuarioACTController();

            itens = usuarioController.ObterTodos(new UsuariosACT()
            {
                Matricula = txtMatricula.Text.Length > 0 ? txtMatricula.Text.Trim() : string.Empty,
                Nome = txtNome.Text
            });

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "NOME ASC":
                        itens = itens.OrderBy(o => o.Nome).ToList();
                        break;
                    case "NOME DESC":
                        itens = itens.OrderByDescending(o => o.Nome).ToList();
                        break;
                    case "MATRICULA ASC":
                        itens = itens.OrderBy(o => o.Matricula).ToList();
                        break;
                    case "MATRICULA DESC":
                        itens = itens.OrderByDescending(o => o.Matricula).ToList();
                        break;
                    case "CPF ASC":
                        itens = itens.OrderBy(o => o.CPF).ToList();
                        break;
                    case "CPF DESC":
                        itens = itens.OrderByDescending(o => o.CPF).ToList();
                        break;
                    case "LDL ASC":
                        itens = itens.OrderBy(o => o.LDL).ToList();
                        break;
                    case "LDL DESC":
                        itens = itens.OrderByDescending(o => o.LDL).ToList();
                        break;
                    case "PERFIL ASC":
                        itens = itens.OrderBy(o => o.Perfil).ToList();
                        break;
                    case "PERFIL DESC":
                        itens = itens.OrderByDescending(o => o.Perfil).ToList();
                        break;
                    default:
                        itens = itens.OrderBy(o => o.Nome).ToList();
                        break;
                }

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

                this.RepeaterItens.DataSource = objPds;
                this.RepeaterItens.DataBind();

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

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


        #region [ MÉTODOS DE ACESSO A DADOS ]

        //protected void CarregarPerfis()
        //{
        //    var pesquisa = new ComboBoxController();

        //    ddlPerfis.DataValueField = "Id";
        //    ddlPerfis.DataTextField = "Descricao";
        //    ddlPerfis.DataSource = pesquisa.ComboBoxPerfis();
        //    ddlPerfis.DataBind();
        //    ddlPerfis.Items.Insert(0, "Selecione!");
        //    ddlPerfis.SelectedIndex = 0;
        //}

        #endregion

    }
}