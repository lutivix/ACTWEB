using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class UsuarioACT : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulTipoOperador { get; set; }
        public string ulMaleta { get; set; }
        public string Matricula { get; set; }
        public string Flag { get; set; }
        public int Id { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulTipoOperador = string.Format("{0}", ViewState["uTipoOperador"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            Matricula = Uteis.Descriptografar(Request.QueryString["matricula"].ToString(), "a#3G6**@").ToUpper();
            Flag = Request.QueryString["flag"];

            if (!Page.IsPostBack)
            {
                
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula;
                lblUsuarioPerfil.Text = ulTipoOperador;
                lblUsuarioMaleta.Text = ulMaleta;

                var nivelAcessoController = new NivelAcessoController();

                CarregarTipoOperadores();
                Controle(Flag);

                LabelMensagem.Visible = false;
            }
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        public void CarregaDados(string matricula)
        {
            var usuarioController = new UsuarioACTController();
            var usuario = usuarioController.ObterPorMatricula(matricula);
             
            txtNomeACT.Text = usuario.Nome.Trim();
            txtMatriculaACT.Text = usuario.Matricula.Trim().ToUpper();
            txtSenhaACT.Attributes.Add("value", usuario.Senha);
            ddlTipoOperador.SelectedValue = usuario.Tipo_Operador_ID.ToString();
            txtCPFACT.Text = usuario.CPF != null ? usuario.CPF.Trim() : string.Empty;
            //txtCPFACT.Text = usuario.CPF.Trim();
            chkPermiteLDLACT.Checked = usuario.Permite_LDL == "S" ? true : false;
            txtMatriculaACT.Enabled = false;
            //txtEmail.Text = usuario.Email != null ? usuario.Email.Trim() : string.Empty;

        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            var usuario = new Entities.UsuariosACT();

            var usuarioController = new UsuarioACTController();
            //double nivelAcessoSelecionado = double.Parse(ddlPerfil.SelectedValue);

            //if (nivelAcessoSelecionado == 1 || nivelAcessoSelecionado == 3)
            //    nivelAcessoSelecionado = 7000;
            //else
            //    nivelAcessoSelecionado = txtMaleta.Text != string.Empty ? Convert.ToDouble(txtMaleta.Text) : 0;

            usuario.Nome = txtNomeACT.Text.ToUpper().Trim();
            usuario.Matricula = txtMatriculaACT.Text.ToUpper().Trim();
            //usuario.Senha = Uteis.Criptografar(txtSenhaACT.Text.ToUpper().Trim(), "a#3G6**@").ToString();
            usuario.Senha = txtSenhaACT.Text.ToUpper().ToString();
            usuario.CPF = txtCPFACT.Text.Trim();
            //usuario.Email = txtEmailACT.Text.Trim();
            usuario.Permite_LDL = chkPermiteLDLACT.Checked ? "S" : "N";
            usuario.Tipo_Operador_ID = ddlTipoOperador.SelectedIndex;




            if (Matricula == "NOVO")
            {
                var dados = usuarioController.ObterPorMatricula(usuario.Matricula);
                if (dados == null)
                {
                    if (usuarioController.Inserir(usuario, ulMatricula))
                    {
                        LabelMensagem.Visible = true;
                        limparCampos();
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Matrícula já está cadastrada!' });", true);
            }
            else
            {
                usuario.ID = usuarioController.ObterPorMatricula(Matricula).ID;
                if (usuarioController.Atualizar(usuario, ulMatricula))
                {
                    limparCampos();
                    if (Flag == "alterasenha")
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    else
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível realizar esta operação, tente novamente mais tarde.' }); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'", true);
            }
        }
        protected void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Excluir(Matricula, Uteis.usuario_Matricula))
                Response.Write("<script>alert('Usuário Excluido com sucesso, por " + Uteis.usuario_Matricula + " - " + Uteis.usuario_Perfil + "'); window.location='/Consulta/UsuarioACT.aspx'</script>");
        }
        protected void ButtonCancelar_Click(object sender, EventArgs e)
        {
            if (txtMatriculaACT.Text != null)
            //if (txtMatricula.Text != null && ddlPerfil != null)
            {
                if (this.Flag == "consulta" || this.Flag == "novousuario")
                    Response.Redirect("/Consulta/UsuarioACT.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
                else
                    Response.Redirect("/Default.aspx");
            }
        }

        #endregion

        //protected void CarregarPerfis()
        //{
        //    var pesquisa = new ComboBoxController();


        //    ddlPerfil.DataValueField = "Id";
        //    ddlPerfil.DataTextField = "Descricao";
        //    ddlPerfil.DataSource = pesquisa.ComboBoxPerfis();
        //    ddlPerfil.DataBind();
        //    ddlPerfil.Items.Insert(0, "Selecione!");
        //    ddlPerfil.SelectedIndex = 0;
        //}

        protected void CarregarTipoOperadores()
        {
            var pesquisa = new ComboBoxController();

            ddlTipoOperador.DataValueField = "Id";
            ddlTipoOperador.DataTextField = "Descricao";
            ddlTipoOperador.DataSource = pesquisa.ComboBoxTipoOperadores();
            ddlTipoOperador.DataBind();
            ddlTipoOperador.Items.Insert(0, "Selecione!");
            ddlTipoOperador.SelectedIndex = 0;
        }

        #region [ MÉTODOS DE APOIO ]

        protected void limparCampos()
        {
            txtNomeACT.Text = string.Empty;
            txtMatriculaACT.Text = string.Empty;
            txtSenhaACT.Text = string.Empty;
             
             
            //txtEmailACT.Text = string.Empty;
        }
        protected bool Excluir(string matricula, string usuarioLogado)
        {
            bool retorno = false;
            var usuarioController = new UsuarioACTController();

            if (usuarioController.Excluir(matricula, usuarioLogado))
                retorno = true;

            return retorno;
        }
        protected void Controle(string flag)
        {
            switch (flag)
            {
                case "consulta":
                    lblTitulo.Text = "Alteração de Usuário";
                    txtMatriculaACT.Enabled = false;
                    //txtNome.Enabled = txtSenha.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = true;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(Matricula);

                    break;
                case "alterasenha":
                    lblTitulo.Text = "Alteração de Senha de Usuário";
                    txtSenhaACT.Enabled = true;
                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;

                case "dadosusuario":
                    lblTitulo.Text = "Exibindo dados de Usuário";

                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = txtSenha.Enabled = chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = false;
                    ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;
                case "novousuario":
                    lblTitulo.Text = "Cadastro de Usuário";
                    //txtMaleta.Text = "0";
                    break;
            }
        }

        protected void ddlPermiteLDL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //private void CarregaDados(string Matricula)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion
    }
}