using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;
using System.Collections.Generic;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class UsuariosAutorizados : System.Web.UI.Page
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

                CarregaCombos(null);
                //CarregarTipoOperadores();
                Controle(Flag);

                LabelMensagem.Visible = false;
            }

            HabilitaDesabilitaCombos();
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        public void CarregaDados(string matricula)
        {
            var usuarioController = new UsuarioAutController();
            var usuario = usuarioController.ObterPorMatricula(matricula);
            List<string> subtipos = new List<string>();
            bool subtiposVazio = false;

            subtipos = usuarioController.ObterSubtiposAut(usuario.Usuario_ID);

            txtNomeACT.Text = usuario.Nome.Trim();
            txtMatriculaACT.Text = usuario.Matricula.Trim().ToUpper();
            txtGerencia.Text = usuario.Gerencia.Trim();
            txtEmpresa.Text = usuario.Empresa.Trim();
            txtCPF.Text = usuario.CPF != null ? usuario.CPF.Trim() : string.Empty;
            txtSupervisao.Text = usuario.Supervisao.Trim();
            ddlCorredores.SelectedIndex = ddlCorredores.Items.IndexOf(ddlCorredores.Items.FindByText(usuario.Nome_Corredor));
            //chkAtivo.Checked = usuario.Ativo_SN == "Sim" ? true : false;
            if (usuario.PermissaoLDL.Equals("Sim"))
            {
                ddlPermissoes.SelectedValue = "0";
            }
            else
            {
                ddlPermissoes.SelectedValue = "1";
            }
            txtMatriculaACT.Enabled = false;

            if (!subtiposVazio)
            {
                for (int i = 0; i < subtipos.Count; i++)
                {
                    int index = cblSubtipos.Items.IndexOf(cblSubtipos.Items.FindByValue(subtipos[i]));

                    cblSubtipos.Items[index].Selected = true;
                }
            }

            chkAtivo.Checked = subtipos.Count > 0 ? true : false;

            //P714 - Item 7 é a LDL

            if(cblSubtipos.Items.FindByValue("7").Selected) 
            //if(cblSubtipos.Items[0].Selected)
            {
                ddlPermissoes.SelectedValue = "0";
                usuario.PermissaoLDL = "S";
            }                
            else
            {
                ddlPermissoes.SelectedValue = "1";
                usuario.PermissaoLDL = "N";
            }
                
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void ButtonSalvar_Click(object sender, EventArgs e)
        {
            var usuarioAutController = new UsuarioAutController();

            UsuarioAutorizado usuario = new UsuarioAutorizado();

            usuario.Matricula = txtMatriculaACT.Text.Trim();
            usuario.Nome = txtNomeACT.Text.Trim();
            
            if(txtCPF.Text.Trim().Length < 11 )
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'CPF incompleto.' });", true);
                return;
            }
            else
            {
                usuario.CPF = txtCPF.Text.Trim();
            }
            if (ddlCorredores.Items[0].Selected)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione um corredor.' });", true);
                return;
            }
            else
            {
                usuario.ID_Corredor = int.Parse(ddlCorredores.SelectedValue);
            }           
            usuario.Supervisao = txtSupervisao.Text.Trim();
            usuario.Gerencia = txtGerencia.Text.Trim();
            usuario.Empresa = txtEmpresa.Text.Trim();
            usuario.Ativo_SN = chkAtivo.Checked ? "S" : "N";

            //if (cblSubtipos.Items[0].Selected)
            if (cblSubtipos.Items.FindByValue("7").Selected)
            {
                ddlPermissoes.SelectedValue = "0";
                usuario.PermissaoLDL = "S";
            }
            else
            {
                ddlPermissoes.SelectedValue = "1";
                usuario.PermissaoLDL = "N";
            }

            var grupoSubtiposVR = new List<string>();

            for (int i = 0; i < cblSubtipos.Items.Count; i++)
            {
                if (cblSubtipos.Items[i].Selected)
                {
                    grupoSubtiposVR.Add(string.Format("'{0}'", cblSubtipos.Items[i].Value));
                }
            }

                if (Matricula == "NOVO")
                {
                    var existeCPF = new UsuarioACTController().ObterUsuarioACTporCPF2(txtCPF.Text);
                    var existeMatricula = new UsuarioACTController().ObterUsuarioACTporMatricula2(txtMatriculaACT.Text);
                    if (!existeCPF && !existeMatricula)
                    {
                        if (usuarioAutController.SalvarUsuario(usuario, ulMatricula))
                        {
                            UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                            usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Inserir");
                            usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                            usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                            LabelMensagem.Visible = true;
                            limparCampos();
                            Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuariosAutorizados.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com o CPF/Matrícula informado.' });", true);
                }
                else
                {
                    usuario.Usuario_ID = usuarioAutController.ObterPorMatricula(Matricula).Usuario_ID;

                    if (usuarioAutController.Atualizar(usuario, ulMatricula))
                    {
                        UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                        //P714 - Vai ser tabela de histórico e não pode delEtar nada/ só desativa :D
                        //Luciano 18/06/2020                        
                        usuarioAutController.DeletarSubtiposAssociados(usuarioAux, ulMatricula);

                        usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Atualizar");
                        usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                        usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                        limparCampos();
                        if (Flag == "alterasenha")
                            Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        else
                            Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "'); window.location='/Consulta/UsuariosAutorizados.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível realizar esta operação, tente novamente mais tarde.' }); window.location='/Default.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'", true);
                }                
        }

        protected void ButtonSalvarECriarOutro_Click(object sender, EventArgs e)
        {
            var usuarioAutController = new UsuarioAutController();

            UsuarioAutorizado usuario = new UsuarioAutorizado();

            usuario.Matricula = txtMatriculaACT.Text.Trim();
            usuario.Nome = txtNomeACT.Text.Trim();

            if (txtCPF.Text.Trim().Length < 11)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'CPF incompleto.' });", true);
                return;
            }
            else
            {
                usuario.CPF = txtCPF.Text.Trim();
            }
            if (ddlCorredores.Items[0].Selected)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione um corredor.' });", true);
                return;
            }
            else
            {
                usuario.ID_Corredor = int.Parse(ddlCorredores.SelectedValue);
            }
            usuario.Supervisao = txtSupervisao.Text.Trim();
            usuario.Gerencia = txtGerencia.Text.Trim();
            usuario.Empresa = txtEmpresa.Text.Trim();
            usuario.Ativo_SN = chkAtivo.Checked ? "S" : "N";

            //if (cblSubtipos.Items[0].Selected)
            if (cblSubtipos.Items.FindByValue("7").Selected)
            {
                ddlPermissoes.SelectedValue = "0";
                usuario.PermissaoLDL = "S";
            }
            else
            {
                ddlPermissoes.SelectedValue = "1";
                usuario.PermissaoLDL = "N";
            }

            var grupoSubtiposVR = new List<string>();

            for (int i = 0; i < cblSubtipos.Items.Count; i++)
            {
                if (cblSubtipos.Items[i].Selected)
                {
                    grupoSubtiposVR.Add(string.Format("'{0}'", cblSubtipos.Items[i].Value));
                }
            }

            if (Matricula == "NOVO")
            {
                var existeCPF = new UsuarioACTController().ObterUsuarioACTporCPF2(txtCPF.Text);
                var existeMatricula = new UsuarioACTController().ObterUsuarioACTporMatricula2(txtMatriculaACT.Text);
                if (!existeCPF && !existeMatricula)
                {
                    if (usuarioAutController.SalvarUsuario(usuario, ulMatricula))
                    {
                        UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                        usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Inserir");
                        usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                        usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                        LabelMensagem.Visible = true;
                        limparCampos();
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "')</script>");
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com o CPF/Matrícula informado.' });", true);
            }
            else
            {
                usuario.Usuario_ID = usuarioAutController.ObterPorMatricula(Matricula).Usuario_ID;

                if (usuarioAutController.Atualizar(usuario, ulMatricula))
                {
                    UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                    //P714 - Vai ser tabela de histórico e não pode delEtar nada/ só desativa :D
                    //Luciano 18/06/2020                        
                    usuarioAutController.DeletarSubtiposAssociados(usuarioAux, ulMatricula);

                    usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Atualizar");
                    usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                    usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                    limparCampos();
                    if (Flag == "alterasenha")
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "');</script>");
                    else
                        Response.Write("<script>alert('Usuário salvo com sucesso, por " + ulMatricula + " - " + ulTipoOperador + "')</script>");
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
                    Response.Redirect("/Consulta/UsuariosAutorizados.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulTipoOperador.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
                else
                    Response.Redirect("/Default.aspx");
            }
        }

        protected void txtCPF_TextChanged(object sender, EventArgs e)
        {
            var existe = new UsuarioACTController().ObterUsuarioACTporCPF2(txtCPF.Text);
            if (existe)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com o CPF: " + txtCPF.Text.Trim() + "' });", true);
            }
        }

        protected void txtMatriculaACT_TextChanged(object sender, EventArgs e)
        {
            var existe = new UsuarioACTController().ObterUsuarioACTporMatricula2(txtMatriculaACT.Text);
            if (existe)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um usuário cadastrado no banco de dados com a Matrícula: " + txtMatriculaACT.Text.Trim() + "' });", true);
            }
        }

        #endregion

        //protected void CarregarTipoOperadores()
        //{
        //    var pesquisa = new ComboBoxController();

        //    ddlPerfil.DataValueField = "Id";
        //    ddlPerfil.DataTextField = "Descricao";
        //    ddlPerfil.DataSource = pesquisa.ComboBoxPerfisACT();
        //    ddlPerfil.DataBind();
        //    ddlPerfil.Items.Insert(0, "Selecione!");
        //    ddlPerfil.SelectedIndex = 0;
        //}

        #region [ MÉTODOS DE APOIO ]

        protected void limparCampos()
        {
            txtNomeACT.Text = string.Empty;
            txtMatriculaACT.Text = string.Empty;
            //txtSenhaACT.Text = string.Empty;
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
                    //txtNome.Enabled = txtSenha.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = true;
                    chkAtivo.Enabled = true;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(Matricula);

                    break;
                case "alterasenha":
                    lblTitulo.Text = "Alteração de Senha de Usuário";
                    //txtSenhaACT.Enabled = true;
                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = chkAtivo.Enabled = false;
                    chkAtivo.Enabled = false;
                    ButtonSalvar.Visible = ButtonCancelar.Visible = true;
                    CarregaDados(ulMatricula);
                    break;

                case "dadosusuario":
                    lblTitulo.Text = "Exibindo dados de Usuário";

                    //txtMatricula.Enabled = txtNome.Enabled = ddlPerfil.Enabled = txtMaleta.Enabled = txtEmail.Enabled = txtSenha.Enabled = chkAtivo.Enabled = false;
                    chkAtivo.Enabled = false;
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
                    //usuario.PermissaoLDL = "S";
                }
                else
                {
                    ddlPermissoes.SelectedValue = "1";
                    //usuario.PermissaoLDL = "S";
                }
            }

            var corredores = pesquisa.ComboBoxCorredores();

            if (corredores.Count > 0)
            {
                ddlCorredores.DataValueField = "ID";
                ddlCorredores.DataTextField = "DESCRICAO";
                ddlCorredores.DataSource = corredores;
                ddlCorredores.DataBind();
                ddlCorredores.Items.Insert(0, "Selecione um Corredor");
            }
        }

        protected void HabilitaDesabilitaCombos()
        {
            if (lblUsuarioPerfil.Text != "ADM")
            {
                ButtonSalvar.Enabled = false;

            }      
        }

        #endregion

        protected void cblSubtipos_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool ativo = false;
            for (int i = 0; i < 7; i++)
            {               

                if(cblSubtipos.Items[i].Selected)
                {
                    ativo = true;
                    break;
                }
                    
            }
            chkAtivo.Checked = ativo;
        }

    }
}