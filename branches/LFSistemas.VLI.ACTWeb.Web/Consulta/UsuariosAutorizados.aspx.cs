using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
   public partial class UsuariosAutorizados : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<UsuarioAutorizado> itens { get; set; }
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

        List<SupervisaoLDL> ListaSupLDLs = new SupervisaoLDLController().BuscarTodas();
        static int sup;
        string matricula;
        static int corredor;
        

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

                CarregaCombos(null);
                Pesquisar(null, Navigation.None);
                HabilitaDesabilitaFuncoes();
            }

            lblAviso.Text = "";
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {
            var usuarioFiltro = PreencherFiltro();

            Pesquisar(null, Navigation.None, usuarioFiltro);
        }
        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            //ddlPerfis.SelectedIndex = 0;
            txtMatricula.Text = string.Empty;
            txtNome.Text = string.Empty;
            ddlPermissoes.SelectedValue = "2";
            CarregaCombos(null);
            Pesquisar(null, Navigation.None);
        }
        protected void lnkNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Cadastro/UsuariosAutorizados.aspx?matricula=" + Uteis.Criptografar("novo", "a#3G6**@") + "&flag=novousuario&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
        }
        protected void lnkUsuarios_Aut_Click(object sender, EventArgs e)
        {
            UsuarioController usectrl = new UsuarioController();
            var usuario = usectrl.ObterPorMatricula(lblUsuarioMatricula.Text);   

            if( 
                    (usuario.Perfil_ID != "1") 
                    //&& (usuario.Perfil_ID != "5") //C1365
              )
            {
                ///C1365 - Inspetores não poderão mais alterar usuários autorizados, apenas ADM's
                lblAviso.Text = "----  Apenas Administradores podem editar os usuários!   ----"; //C1365
            }
            else
            {
                LinkButton btn = (LinkButton)(sender);
                string matricula = btn.CommandArgument;
                Response.Redirect("/Cadastro/UsuariosAutorizados.aspx?matricula=" + Uteis.Criptografar(matricula.ToLower(), "a#3G6**@") + "&flag=consulta&lu=" + Uteis.Criptografar(ViewState["ulNome"].ToString().ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ViewState["uMatricula"].ToString().ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ViewState["uPerfil"].ToString().ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ViewState["ulMaleta"].ToString().ToLower(), "a#3G6**@"));
            }
            
        }

        protected void lnkExcel_Click(object sender, EventArgs e)
        {
            var usuarioFiltro = PreencherFiltro();
            Excel(null, Navigation.None, usuarioFiltro);
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

        protected void lnkCor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkSuperv_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("SUPERVISAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("SUPERVISAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkGer_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("GERENCIA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("GERENCIA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkEmpresa_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("EMPRESA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("EMPRESA " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkPerLDL_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("PERMISSAOLDL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("PERMISSAOLDL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkUltSol_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ULTSOL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ULTSOL " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkAtivo_Click1(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("ATIVO" + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("ATIVO" + ViewState["ordenacao"].ToString(), Navigation.None);
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
            var usuarioController = new UsuariosAutController();

            itens = usuarioController.ObterTodos(new UsuarioAutorizado()
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
                    case "CORREDOR ASC":
                        itens = itens.OrderBy(o => o.Nome_Corredor).ToList();
                        break;
                    case "CORREDOR DESC":
                        itens = itens.OrderByDescending(o => o.Nome_Corredor).ToList();
                        break;
                    case "SUPERVISAO ASC":
                        itens = itens.OrderBy(o => o.Supervisao).ToList();
                        break;
                    case "SUPERVISAO DESC":
                        itens = itens.OrderByDescending(o => o.Supervisao).ToList();
                        break;
                    case "GERENCIA ASC":
                        itens = itens.OrderBy(o => o.Gerencia).ToList();
                        break;
                    case "GERENCIA DESC":
                        itens = itens.OrderByDescending(o => o.Gerencia).ToList();
                        break;
                    case "EMPRESA ASC":
                        itens = itens.OrderBy(o => o.Gerencia).ToList();
                        break;
                    case "EMPRESA DESC":
                        itens = itens.OrderByDescending(o => o.Gerencia).ToList();
                        break;
                    case "PERMISSAOLDL ASC":
                        itens = itens.OrderBy(o => o.PermissaoLDL).ToList();
                        break;
                    case "PERMISSAOLDL DESC":
                        itens = itens.OrderByDescending(o => o.PermissaoLDL).ToList();
                        break;
                    case "ULTSOL ASC":
                        itens = itens.OrderBy(o => o.UltSolicitacao).ToList();
                        break;
                    case "ULTSOL DESC":
                        itens = itens.OrderByDescending(o => o.UltSolicitacao).ToList();
                        break;
                    case "ATIVO ASC":
                        itens = itens.OrderBy(o => o.SituacaoAtividade).ToList();
                        break;
                    case "ATIVO DESC":
                        itens = itens.OrderByDescending(o => o.SituacaoAtividade).ToList();
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

        protected void Pesquisar(string ordenacao, Navigation navigation, UsuarioAutorizado usuarioFiltro)
        {
            var usuarioController = new UsuariosAutController();

            if (cblSubtipos.Items.FindByValue("7").Selected)
            //if (cblSubtipos.Items[0].Selected)
            {
                ddlPermissoes.SelectedValue = "0";
                usuarioFiltro.PermissaoLDL = "S";
            }
            else
            {
                ddlPermissoes.SelectedValue = "1";
                usuarioFiltro.PermissaoLDL = "N";
            }

            //P1461 - Luara - inclusão de supervisões no Filtro
            //Pegar todos os itens do repeater
            List<string> selecionados = new List<string>();

            foreach(ListItem item in cblSupervisoes.Items)
            {
                if(item.Selected)
                {
                    selecionados.Add(item.Value);
                }
                
            }

            var supervisoes = string.Join(",", selecionados);

            itens = usuarioController.ObterTodosfiltro(new UsuarioAutorizado()
            {
                Matricula = txtMatricula.Text.Length > 0 ? txtMatricula.Text.Trim() : string.Empty,
                Nome = txtNome.Text,
                Subtipos_BS = usuarioFiltro.Subtipos_BS,
                corredores_id = usuarioFiltro.corredores_id,
                CPF = usuarioFiltro.CPF,
                PermissaoLDL = usuarioFiltro.PermissaoLDL,
                Supervisoes_LDL = supervisoes
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
                    case "CORREDOR ASC":
                        itens = itens.OrderBy(o => o.Nome_Corredor).ToList();
                        break;
                    case "CORREDOR DESC":
                        itens = itens.OrderByDescending(o => o.Nome_Corredor).ToList();
                        break;
                    case "SUPERVISAO ASC":
                        itens = itens.OrderBy(o => o.Supervisao).ToList();
                        break;
                    case "SUPERVISAO DESC":
                        itens = itens.OrderByDescending(o => o.Supervisao).ToList();
                        break;
                    case "GERENCIA ASC":
                        itens = itens.OrderBy(o => o.Gerencia).ToList();
                        break;
                    case "GERENCIA DESC":
                        itens = itens.OrderByDescending(o => o.Gerencia).ToList();
                        break;
                    case "EMPRESA ASC":
                        itens = itens.OrderBy(o => o.Gerencia).ToList();
                        break;
                    case "EMPRESA DESC":
                        itens = itens.OrderByDescending(o => o.Gerencia).ToList();
                        break;
                    case "PERMISSAOLDL ASC":
                        itens = itens.OrderBy(o => o.PermissaoLDL).ToList();
                        break;
                    case "PERMISSAOLDL DESC":
                        itens = itens.OrderByDescending(o => o.PermissaoLDL).ToList();
                        break;
                    case "ULTSOL ASC":
                        itens = itens.OrderBy(o => o.UltSolicitacao).ToList();
                        break;
                    case "ULTSOL DESC":
                        itens = itens.OrderByDescending(o => o.UltSolicitacao).ToList();
                        break;
                    case "ATIVO ASC":
                        itens = itens.OrderBy(o => o.SituacaoAtividade).ToList();
                        break;
                    case "ATIVO DESC":
                        itens = itens.OrderByDescending(o => o.SituacaoAtividade).ToList();
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

        protected void Excel(string ordenacao, Navigation navigation, UsuarioAutorizado usuarioFiltro)
        {
            UsuariosAutController usuarios = new UsuariosAutController();

            List<UsuarioAutorizado> itens = usuarios.ObterTodosfiltro(usuarioFiltro);

            if (itens.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                try
                {
                    sb.AppendLine("ID;MATRICULA;NOME;CPF;CORREDOR_ID;SUPERVISAO;GERENCIA;EMPRESA;PERMITE_LDL;ULTIMA_SOLICITACAO;ATIVO");

                    foreach (var item in itens)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", item.Usuario_ID, item.Matricula, item.Nome, item.CPF, item.ID_Corredor, item.Supervisao, item.Gerencia, item.Empresa, item.PermissaoLDL, item.UltSolicitacao, item.Ativo_SN));
                    }
                }
                catch (Exception ex)
                {
                    new Exception(ex.Message);
                }

                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=Usuarios_Autorizados.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
            {
                RepeaterItens.DataSource = itens;
                RepeaterItens.DataBind();

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A pesquisa não retornou registros.' });", true);
            }


        }

        protected void CarregaCombos(string origem)
        {
            var pesquisa = new ComboBoxController();

            var subtipos = pesquisa.ComboBoxSubtipos();

            if (subtipos.Count > 0)
            {
                cblSubtipos.DataValueField = "ID";
                cblSubtipos.DataTextField = "DESCRICAO";
                cblSubtipos.DataSource = subtipos;
                cblSubtipos.DataBind();

                if (cblSubtipos.Items.FindByValue("7").Selected)
                //if (cblSubtipos.Items[0].Selected)
                {
                    ddlPermissoes.SelectedValue = "0";
                    //usuarioFiltro.PermissaoLDL = "S";
                   // permiteLDLFiltro = "S";
                }
                else
                {
                    ddlPermissoes.SelectedValue = "1";
                    //usuarioFiltro.PermissaoLDL = "N";
                    //permiteLDLFiltro = "N";
                }
            }

            var corredores = pesquisa.ComboBoxCorredores();

            if (corredores.Count > 0)
            {                
                cblCorredores.DataValueField = "ID";
                cblCorredores.DataTextField = "DESCRICAO";
                cblCorredores.DataSource = corredores;
                cblCorredores.DataBind();

            }

            CarregarSupervisoes(string.Empty);
        }

        public UsuarioAutorizado PreencherFiltro()
        {
            UsuarioAutorizado usuarioFiltro = new UsuarioAutorizado();

            string subtiposFiltro = "";
            string corredoresFiltro = "";
            string permiteLDLFiltro = "";
            string cpfFiltro = "";
            string nomeFiltro = "";
            string matriculaFiltro = "";

            List<string> subtipos = new List<string>();
            List<string> corredores_id = new List<string>();

            for (int i = 0; i <= cblSubtipos.Items.Count - 1; i++)
            {
                if (cblSubtipos.Items[i].Selected)
                {
                    subtipos.Add(cblSubtipos.Items[i].Value);
                }
            }

            for (int i = 0; i <= cblCorredores.Items.Count - 1; i++)
            {
                if (cblCorredores.Items[i].Selected)
                {
                    corredores_id.Add(cblCorredores.Items[i].Value);
                }
            }

            if (subtipos.Count > 0)
            {
                subtiposFiltro = string.Join(",", subtipos);
            }

            if (corredores_id.Count > 0)
            {
                corredoresFiltro = string.Join(",", corredores_id);
            }

            if (cblSubtipos.Items.FindByValue("7").Selected)
           // if (cblSubtipos.Items[0].Selected == true)
            {
                permiteLDLFiltro = "S";
            }
            else {
                permiteLDLFiltro = "N";
            }

            cpfFiltro = txtCPF.Text.Trim();
            nomeFiltro = txtNome.Text.Trim();
            matriculaFiltro = txtMatricula.Text.Trim();

            usuarioFiltro.Subtipos_BS = subtiposFiltro;
            usuarioFiltro.corredores_id = corredoresFiltro;
            usuarioFiltro.PermissaoLDL = permiteLDLFiltro;
            usuarioFiltro.CPF = cpfFiltro;
            usuarioFiltro.Nome = nomeFiltro;
            usuarioFiltro.Matricula = matriculaFiltro;
            

            return usuarioFiltro;
            
        }

        public void HabilitaDesabilitaFuncoes()
        {
            if (lblUsuarioPerfil.Text != "ADM")
            {
                lnkNovo.Visible = false;

            }
        }


        #endregion

        #region [ VERIFICAÇÃO DE SUPERVISÃO LDL ]
        private void CarregarSupervisoes(string corredoresSelecionados)
        {
            var pesquisa = new ComboBoxController();
            var supervisoes = pesquisa.ComboBoxSupervisoes(corredoresSelecionados);

            cblSupervisoes.Items.Clear(); // Limpa antes, para não duplicar

            

            if(corredoresSelecionados == string.Empty)
            {
                foreach (var sup in ListaSupLDLs)
                {
                    ListItem item = new ListItem();
                    item.Text = sup.Nome;        // Nome visível
                    item.Value = sup.Id.ToString();  // ID da supervisão
                    item.Attributes["data-corredor-id"] = sup.IdCorredor.ToString(); // <<< Corredor associado
                    string teste = item.Attributes["data-corredor-id"];
                    cblSupervisoes.Items.Add(item);                    
                }
            }
            else
            {
                foreach (var sup in ListaSupLDLs)
                {
                    if ( corredoresSelecionados.Contains(sup.IdCorredor.ToString()) )
                    {
                        ListItem item = new ListItem();
                        item.Text = sup.Nome;        // Nome visível
                        item.Value = sup.Id.ToString();  // ID da supervisão
                        item.Attributes["data-corredor-id"] = sup.IdCorredor.ToString(); // <<< Corredor associado
                        string teste = item.Attributes["data-corredor-id"];
                        cblSupervisoes.Items.Add(item);
                    }
                    
                }
            }

            if(matricula != null)
            {
                var usuariocont = new UsuarioAutController();
                var usuario = usuariocont.ObterPorMatricula(matricula);
                var sups = usuariocont.ObterSupsLDLAut(usuario.Usuario_ID);

                foreach (ListItem item in cblSupervisoes.Items)
                {
                    if (sups.Find(p => p == item.Value) != null)
                    {
                        item.Selected = true;
                    }
                }
            }
            
                     
            //cblSupervisoes.DataTextField = "DESCRICAO";
            //cblSupervisoes.DataValueField = "ID";
            //cblSupervisoes.DataSource = supervisoes;
            //cblSupervisoes.Attributes
            //cblSupervisoes.DataBind();
        }

        protected void cblCorredores_SelectedIndexChanged(object sender, EventArgs e)
        {
            //P1461
            string corAtivos = string.Empty;


            foreach (ListItem item in cblCorredores.Items)
            {
                if (item.Selected)
                {
                    corredor = int.Parse(item.Value);

                    if(corAtivos == string.Empty)
                        corAtivos += item.Value; 
                    else
                        corAtivos += "," + item.Value; 
                }
            }


            CarregarSupervisoes(corAtivos);
        }

        protected void cblSupervisoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //P1461
            // 1. Collect all unique corredor IDs associated with selected supervisoes
            HashSet<string> selectedCorredorIds = new HashSet<string>();

            // Ensure ListaSupLDLs is populated and available (declared as class member)
            if (ListaSupLDLs != null)
            {
                foreach (ListItem supervisaoItem in cblSupervisoes.Items)
                {
                    if (supervisaoItem.Selected)
                    {
                        // Find the SupervisaoLDL object for the selected item
                        SupervisaoLDL selectedSupervisao = ListaSupLDLs.Find(s => s.Id.ToString() == supervisaoItem.Value);
                        if (selectedSupervisao != null)
                        {
                            // Add the associated corredor ID to the set
                            selectedCorredorIds.Add(selectedSupervisao.IdCorredor.ToString());
                        }
                    }
                }
            }

            // 2. Update cblCorredores based on the collected corredor IDs
            foreach (ListItem corredorItem in cblCorredores.Items)
            {
                // Select if the corredor ID is in the set, deselect otherwise
                corredorItem.Selected = selectedCorredorIds.Contains(corredorItem.Value);
            }

            // Update the 'sup' variable based on the first selected item (maintaining original behavior if needed)
            ListItem firstSelectedSupervisao = null;
            foreach (ListItem item in cblSupervisoes.Items) {
                if (item.Selected) {
                    firstSelectedSupervisao = item;
                    break;
                }
            }
            if (firstSelectedSupervisao != null) {
                sup = int.Parse(firstSelectedSupervisao.Value);
            } else {
                // If nothing is selected in cblSupervisoes, set sup to 0 (or appropriate default)
                sup = 0;
                // Also ensure cblCorredores is cleared if no supervisao is selected
                // The loop above already handles this by deselecting all corredores if selectedCorredorIds is empty.
            }
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