using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Web.UI;
using System.Collections.Generic;
using LFSistemas.VLI.ACTWeb.DataAccessObjects;
using System.Web.UI.WebControls;

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

        //C1126
        static string nome;
        static string cpf;
        static string matricula;
        static int sup;
        static string gerencia;
        static int corredor;
        
        static bool ativo;
        static bool subiu = false;        

        bool subtiposVazio = false;
        static int[] subs = new int[7];

        //  P1461 - Corredores em checkboxes - Luara
        bool corredoresVazio = false;
        static int[] corres =  new int [7];

        List<SupervisaoLDL> ListaSupLDLs = new SupervisaoLDLController().BuscarTodas() ;
        bool supsVazio = false;
        static int[] sups;

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

               
                sups = new int[ListaSupLDLs.Count];


                CarregaCombos(null);
                //CarregarTipoOperadores();
                Controle(Flag);

                //C1126 - Carregando valores iniciais para comparação futura
                nome = txtNomeACT.Text;
                cpf = txtCPF.Text;
                matricula = txtMatriculaACT.Text;
                gerencia = txtGerencia.Text;
                //corredor = ddlCorredores.SelectedIndex;
                corredor = 0; //P1461
                ativo = chkAtivo.Checked;
                //sup = txtSupervisao.Text;
                sup = 0;
                if (!subtiposVazio)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        subs[i] = cblSubtipos.Items[i].Selected ? 1:0 ;
                    }
                }

                if (!corredoresVazio)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        corres[i] = cblCorredores.Items[i].Selected ? 1 : 0;
                    }
                }

                if (!supsVazio)
                {
                    for (int i = 0; i < cblSupervisoes.Items.Count; i++)
                    {
                        sups[i] = cblSupervisoes.Items[i].Selected ? 1 : 0;
                    }
                }

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
            List<string> supervisoes = new List<string>();
            subtiposVazio = false;

            subtipos = usuarioController.ObterSubtiposAut(usuario.Usuario_ID);
            supervisoes = usuarioController.ObterSupsLDLAut(usuario.Usuario_ID);


            txtNomeACT.Text = usuario.Nome.Trim();
            txtMatriculaACT.Text = usuario.Matricula.Trim().ToUpper();
            txtGerencia.Text = usuario.Gerencia.Trim();
            txtEmpresa.Text = usuario.Empresa.Trim();
            txtCPF.Text = usuario.CPF != null ? usuario.CPF.Trim() : string.Empty;
            txtSupervisao.Text = usuario.Supervisao.Trim();
            cblCorredores.SelectedIndex = cblCorredores.Items.IndexOf(cblCorredores.Items.FindByText(usuario.Nome_Corredor)); //P1461
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

            CarregarSupervisoes(cblCorredores.SelectedIndex.ToString());


            //  P1461
            for (int i = 0; i < supervisoes.Count; i++)
            {
                int index = cblSupervisoes.Items.IndexOf(cblSupervisoes.Items.FindByValue(supervisoes[i]));

                cblSupervisoes.Items[index].Selected = true;

                int corr = int.Parse(cblSupervisoes.Items[index].Attributes["data-corredor-id"]);

                cblCorredores.Items[corr].Selected = true;

                //break;//    P1461 - Por enquanto só pode uma supervisão por operador
                
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
            List<SupervisaoLDL> listEnvioSups = new List<SupervisaoLDL>();
            foreach(SupervisaoLDL item in ListaSupLDLs)
            {
                foreach(ListItem box in cblSupervisoes.Items)
                {
                    if(box.Selected)
                    {
                        if (box.Value == item.Id.ToString())
                            listEnvioSups.Add(item);
                    }                    
                }
            }

            //C1126
            if ((nome == txtNomeACT.Text) &&
                 (cpf == txtCPF.Text) &&
                 (matricula == txtMatriculaACT.Text) &&
                 (gerencia == txtGerencia.Text) &&
                 (corredor == int.Parse(cblCorredores.Items[cblCorredores.SelectedIndex].Value)) &&
                 (ativo == chkAtivo.Checked) &&
                 (sup == int.Parse(cblSupervisoes.Items[cblSupervisoes.SelectedIndex].Value))  )
                //true)
            {
                bool lsubs = true;
                bool lcorres = true;//P1461
                bool lsups = true;//P1461

                int teste = -1;
                for (int i = 0; i < 7; i++)
                {
                    teste = cblSubtipos.Items[i].Selected ? 1 : 0;
                    if (teste != subs[i])
                    {
                        lsubs = false;
                        break;
                    }                        
                }

                for (int i = 0; i < 7; i++)
                {
                    teste = cblCorredores.Items[i].Selected ? 1 : 0;
                    if (teste != corres[i])
                    {
                        lcorres = false;
                        break;
                    }
                }

                for (int i = 0; i < cblSupervisoes.Items.Count; i++)
                {
                    teste = cblSupervisoes.Items[i].Selected ? 1 : 0;
                    if (teste != sups[i])
                    {
                        lsups = false;
                        break;
                    }
                }

                if(lsubs && lcorres && lsups)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não houve nenhuma alteração na página!' });", true);
                    return;
                }                
            }
            else
            {
                nome = string.Empty;
                cpf = string.Empty;
                matricula = string.Empty;
                gerencia = string.Empty;
                corredor = 0;
                ativo = false;
                sup = 0;

                for (int i = 0; i < 7; i++)
                {
                    subs[i] = 0;
                }
            }

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

            if(  (usuarioAutController.JaExisteCPF(usuario.CPF))  && (Matricula == "NOVO")  )
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'CPF já existe!' });", true);
                return;
            }

            if (cblCorredores.SelectedItem == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione um corredor.' });", true);
                return;
            }
            else
            {
                usuario.ID_Corredor = int.Parse(cblCorredores.SelectedValue);//P1461
            }
            usuario.Supervisao = cblSupervisoes.Items[cblSupervisoes.SelectedIndex].Text.Trim();//P1461
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

            //C1126 - logar os subtipos atuais.
            List<string> subtipos = new List<string>();
            string subaux = string.Empty;          
  



            if (Matricula == "NOVO")
            {
                var existeCPF = new UsuarioACTController().ObterUsuarioACTporCPF2(txtCPF.Text);
                var existeMatricula = new UsuarioACTController().ObterUsuarioACTporMatricula2(txtMatriculaACT.Text);
                if (!existeCPF && !existeMatricula)
                {
                    if (usuarioAutController.SalvarUsuario(usuario, ulMatricula))
                    {
                        UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                        subtipos = usuarioAutController.ObterSubtiposAut(usuarioAux.Usuario_ID);

                        for (int i = 0; i < subtipos.Count; i++)
                        {
                            subtipos[i] = VerificaSubtipo(subtipos[i]);
                        }

                        subaux = string.Join(",", subtipos);

                        if (subaux != string.Empty)
                            LogDAO.GravaLogBanco(DateTime.Now, ulMatricula, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Permissões Atuais: " + subaux, Uteis.OPERACAO.Imprimiu.ToString());
                        else
                            LogDAO.GravaLogBanco(DateTime.Now, ulMatricula, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Sem permissões Atuais!", Uteis.OPERACAO.Imprimiu.ToString());


                        usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Inserir");
                        usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                        usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                        //  P1461 - Associação das novas supervisões - Luara - 25/04/2025
                        usuarioAutController.AssociarSupervisoes(listEnvioSups, usuarioAux, ulMatricula);

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

                subtipos = usuarioAutController.ObterSubtiposAut(usuario.Usuario_ID);

                for (int i = 0; i < subtipos.Count; i++ )
                {
                    subtipos[i] = VerificaSubtipo(subtipos[i]);
                }

                subaux = string.Join(",", subtipos);

                if (subaux != string.Empty)
                    LogDAO.GravaLogBanco(DateTime.Now, ulMatricula, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Permissões Atuais: " + subaux, Uteis.OPERACAO.Imprimiu.ToString());
                else
                    LogDAO.GravaLogBanco(DateTime.Now, ulMatricula, "Usuários", null, null, "Usuário: " + usuario.Nome + " Perfil: " + usuario.Perfil + " - CPF: " + usuario.CPF + " - Sem permissões Atuais!", Uteis.OPERACAO.Imprimiu.ToString());


                if (usuarioAutController.Atualizar(usuario, ulMatricula))
                {
                    UsuarioAutorizado usuarioAux = usuarioAutController.ObterPorMatricula(usuario.Matricula);

                    //P714 - Vai ser tabela de histórico e não pode delEtar nada/ só desativa :D
                    //Luciano 18/06/2020                        
                    usuarioAutController.DeletarSubtiposAssociados(usuarioAux, ulMatricula);

                    usuarioAutController.AssociarSubtipos(grupoSubtiposVR, usuarioAux, ulMatricula, "Atualizar");
                    usuarioAutController.AtualizarDataUltSol(txtCPF.Text, txtMatriculaACT.Text, usuarioAux.Usuario_ID.ToString(), "Atualização");
                    usuarioAutController.AtualizarDataUltSolBSOP(txtCPF.Text, usuarioAux.Usuario_ID.ToString(), "0");

                    //  P1461 - Associação das novas supervisões - Luara - 25/04/2025
                    usuarioAutController.AssociarSupervisoes(listEnvioSups, usuarioAux, ulMatricula);

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
            //C1126
            if ((nome == txtNomeACT.Text) &&
                 (cpf == txtCPF.Text) &&
                 (matricula == txtMatriculaACT.Text) &&
                 (gerencia == txtGerencia.Text) &&
                 (corredor == cblCorredores.SelectedIndex) &&
                 (ativo == chkAtivo.Checked) &&
                 (sup == cblSupervisoes.SelectedIndex)  )
                //true)
            {
                bool lsubs = false;
                int teste = -1;
                for (int i = 0; i < 7; i++)
                {
                    teste = cblSubtipos.Items[i].Selected ? 1 : 0;
                    if (teste == subs[i])
                    {
                        lsubs = true;
                        break;
                    }
                }

                if (lsubs)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não houve nenhuma alteração na página!' });", true);
                    return;
                }
            }
            else
            {
                nome = string.Empty;
                cpf = string.Empty;
                matricula = string.Empty;
                gerencia = string.Empty;
                corredor = 0;
                ativo = false;
                sup = 0;

                for (int i = 0; i < 7; i++)
                {
                    subs[i] = 0;
                }
            }

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

            if (usuarioAutController.JaExisteCPF(usuario.CPF) && (Matricula == "NOVO") )
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'CPF já existe!' });", true);
                return;
            }

            if (cblCorredores.SelectedItem == null)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione um corredor.' });", true);
                return;
            }
            else
            {
                usuario.ID_Corredor = int.Parse(cblCorredores.SelectedValue);//P1461
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
            txtCPF.Text = string.Empty;            
            //txtSenhaACT.Text = string.Empty;

            cblCorredores.SelectedIndex = 0;
            //P1461
            for (int i = 0; i < 7; i++)
            {
                cblCorredores.Items[i].Selected = false;
            }

            for (int i = 0; i < 7; i++)
            {                
                cblSubtipos.Items[i].Selected = false;
            }

            for (int i = 0; i < cblSupervisoes.Items.Count; i++)
            {
                cblSupervisoes.Items[i].Selected = false;
            }

            chkAtivo.Checked = false;
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

                chkAtivo.Checked = false;
                for (int i = 0; i < 7; i++)
                {

                    if (cblSubtipos.Items[i].Selected)
                    {
                        chkAtivo.Checked = true;
                        break;
                    }

                }
                
            }

            var corredores = pesquisa.ComboBoxCorredores();

            //  P1461 - Luara - Nova regra de supervisões par ausuários autorizados
            //  25/04/2025
            if (corredores.Count > 0)
            {
                //ddlCorredores.DataValueField = "ID";
                //ddlCorredores.DataTextField = "DESCRICAO";
                //ddlCorredores.DataSource = corredores;
                //ddlCorredores.DataBind();
                //ddlCorredores.Items.Insert(0, "Selecione um Corredor");

                //P1461
                cblCorredores.DataValueField = "ID";
                cblCorredores.DataTextField = "DESCRICAO";
                cblCorredores.DataSource = corredores;
                cblCorredores.DataBind();

                string corAtivos = string.Empty;

                for (int i = 0; i < cblCorredores.Items.Count; i++)
                {
                    if (cblCorredores.Items[i].Selected)
                    {
                        // Aqui você trabalha com o item selecionado
                        if(i>0)
                            corAtivos += "," + cblCorredores.Items[i].Value;
                        else
                            corAtivos += cblCorredores.Items[i].Value; ;
                        //string nomeCorredor = item.Text;
                    }
                }


                CarregarSupervisoes(corAtivos);
            }
           
        }

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

        public string VerificaSubtipo(string subtipo_id)
        {
            if (subtipo_id.Equals("1"))
            {
                return "US";
            }
            else if (subtipo_id.Equals("2"))
            {
                return "RL";
            }
            else if (subtipo_id.Equals("3"))
            {
                return "HT";
            }
            else if (subtipo_id.Equals("4"))
            {
                return "HL";
            }
            else if (subtipo_id.Equals("5"))
            {
                return "EE";
            }
            else if (subtipo_id.Equals("6"))
            {
                return "PP";
            }
            else if (subtipo_id.Equals("7"))
            {
                return "LDL";
            }
            else
            {
                return "";
            }

        }

        protected void cblCorredores_SelectedIndexChanged(object sender, EventArgs e)
        {
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

    }
}