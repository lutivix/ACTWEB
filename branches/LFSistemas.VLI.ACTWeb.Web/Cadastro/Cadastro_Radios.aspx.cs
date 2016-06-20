using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Cadastro_Radios : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Id { get; set; }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());
            Id = string.Format("{0}", ViewState["id"] = Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper());


            if (!Page.IsPostBack)
            {
                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                lblControleRadioID.Text = Id = Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper();

                CarregaCombos();
                ControlarBarraComandos(Id, ulPerfil);
                if (Id != string.Empty)
                    CarregaDados(Id);
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            Radios dados = new Radios();
            var salvar = new RadiosController();

            dados.Atualizacao = DateTime.Now;
            dados.Trem = txtDadosTrem.Text.Length > 0 ? txtDadosTrem.Text : null;
            dados.Loco = txtDadosLoco.Text.Length > 0 ? txtDadosLoco.Text : null;
            dados.Tipo_Loco_ID = double.Parse(ddlDadosTipoLoco.SelectedItem.Value);
            dados.Tipo_Loco = ddlDadosTipoLoco.SelectedItem.Text;
            dados.Corredor_ID = double.Parse(ddlDadosCorredor.SelectedItem.Value);
            dados.Corredor = ddlDadosCorredor.SelectedItem.Text;
            dados.Situacao_ID = double.Parse(ddlDadosSituacao.SelectedItem.Value);
            dados.Situacao = ddlDadosSituacao.SelectedItem.Text;
            dados.Considera_SN = chkDadosConsidera.Checked ? "S" : "N";
            dados.Radio_ID = txtDadosRadioID.Text.Length > 0 ? txtDadosRadioID.Text : null;
            dados.Modelo_Radio_Acima = txtDadosModeloAC.Text.Length > 0 ? txtDadosModeloAC.Text : null;
            dados.Serial_Radio_Acima = txtDadosSerialAC.Text.Length > 0 ? txtDadosSerialAC.Text : null;
            dados.Modelo_Radio_Abaixo = txtDadosModeloAB.Text.Length > 0 ? txtDadosModeloAB.Text : null;
            dados.Serial_Radio_Abaixo = txtDadosSerialAB.Text.Length > 0 ? txtDadosSerialAB.Text : null;
            dados.Ativo_SN = chkAtivo.Checked ? "S" : "N";

            if (lblControleRadioID.Text.Length > 0)
            {
                dados.Radios_ID = double.Parse(lblControleRadioID.Text);

                if (salvar.Salvar(dados, lblUsuarioMatricula.Text))
                {
                    Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaRadios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                }
            }
            else
            {
                if (!JaExiste(dados.Loco))
                {
                    if (salvar.Salvar(dados, lblUsuarioMatricula.Text))
                    {
                        Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaRadios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Já existe um registro no banco com a loco: " + dados.Loco + "' });", true);
            }
        }
        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaRadios.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }
        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            var excluir = new RadiosController();

            if (lblControleRadioID.Text.Length > 0)
            {
                if (excluir.Excluir(lblControleRadioID.Text, lblUsuarioMatricula.Text))
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro exluido com sucesso por " + lblUsuarioLogado.Text + ".' });", true);
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível exluir o registro.' });", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi licalizado o identificador do registro.' });", true);
        }

        #endregion
        #endregion

        public void CarregaCombos()
        {
            var pesquisa = new ComboBoxController();

            var corredores = pesquisa.ComboBoxCorredores();
            if (corredores.Count > 0)
            {
                ddlDadosCorredor.DataValueField = "ID";
                ddlDadosCorredor.DataTextField = "DESCRICAO";
                ddlDadosCorredor.DataSource = corredores;
                ddlDadosCorredor.DataBind();
                ddlDadosCorredor.Items.Insert(0, "Selecione!");
                ddlDadosCorredor.SelectedIndex = 0;
            }

            var situacoes = pesquisa.ComboBoxSituacaoControleRadios();
            if (situacoes.Count > 0)
            {
                ddlDadosSituacao.DataValueField = "ID";
                ddlDadosSituacao.DataTextField = "DESCRICAO";
                ddlDadosSituacao.DataSource = situacoes;
                ddlDadosSituacao.DataBind();
                ddlDadosSituacao.Items.Insert(0, "Selecione!");
                ddlDadosSituacao.SelectedIndex = 0;
            }

            var tipo_locomotivas = pesquisa.ComboBoxTipoLocomotivas();
            if (situacoes.Count > 0)
            {
                ddlDadosTipoLoco.DataValueField = "ID";
                ddlDadosTipoLoco.DataTextField = "DESCRICAO";
                ddlDadosTipoLoco.DataSource = tipo_locomotivas;
                ddlDadosTipoLoco.DataBind();
                ddlDadosTipoLoco.Items.Insert(0, "Selecione!");
                ddlDadosTipoLoco.SelectedIndex = 0;
            }
        }

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void CarregaDados(string id)
        {
            var pesquisar = new RadiosController();

            var item = pesquisar.ObterPorId(int.Parse(id));

            txtDadosTrem.Text = item.Trem != null ? item.Trem : string.Empty;
            txtDadosLoco.Text = item.Loco != null ? item.Loco : string.Empty;
            ddlDadosTipoLoco.SelectedValue = item.Tipo_Loco_ID.ToString() != null ? item.Tipo_Loco_ID.ToString() : "0";
            ddlDadosCorredor.SelectedValue = item.Corredor_ID.ToString() != null ? item.Corredor_ID.ToString() : "0";
            ddlDadosSituacao.SelectedValue = item.Situacao_ID.ToString() != null ? item.Situacao_ID.ToString() : "0";
            chkDadosConsidera.Checked = item.Considera_SN == "S" ? true : false;
            txtDadosRadioID.Text = item.Radio_ID != null ? item.Radio_ID.ToString() : string.Empty;
            txtDadosModeloAC.Text = item.Modelo_Radio_Acima != null ? item.Modelo_Radio_Acima : string.Empty;
            txtDadosSerialAC.Text = item.Serial_Radio_Acima != null ? item.Serial_Radio_Acima : string.Empty;
            txtDadosModeloAB.Text = item.Modelo_Radio_Abaixo != null ? item.Modelo_Radio_Abaixo : string.Empty;
            txtDadosSerialAB.Text = item.Serial_Radio_Abaixo != null ? item.Serial_Radio_Abaixo : string.Empty;
            chkAtivo.Checked = item.Ativo_SN == "S" ? true : false;
        }

        protected bool JaExiste(string loco)
        {
            bool Retorno = false;
            var pesquisar = new RadiosController();
            var itens = pesquisar.ObterRadiosPorFiltro(new Radios()
            {
                Loco = loco,
            }, lblUsuarioPerfil.Text);

            if (itens.Count > 0)
                Retorno = true;

            return Retorno;
        }

        protected void Limpar()
        {
            //txtArquivo.Text =
            //txtDescricao.Text = string.Empty;
            //chkAtivo.Checked = true;
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void ControlarBarraComandos(string status, string perfil)
        {
            if (status == string.Empty && perfil == "ADM - TEL")
            {
                lblTitulo.Text = "Rádio - Cadastrando.";
                chkAtivo.Checked = true;
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = false;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkLimpar.Enabled = true;
                lnkLimpar.CssClass = "btn btn-default";
                txtDadosTrem.Enabled = false;
                txtDadosLoco.Enabled = true;
                ddlDadosTipoLoco.Enabled = true;
            }
            else if (status != string.Empty && perfil == "ADM - TEL")
            {
                lblTitulo.Text = "Rádio - Alterando.";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger";
                lnkLimpar.Enabled = false;
                lnkLimpar.CssClass = "btn btn-default disabled";
                txtDadosTrem.Enabled = false;
                txtDadosLoco.Enabled = false;
                ddlDadosTipoLoco.Enabled = false;
            }
            else
            {
                txtDadosTrem.Enabled =
                txtDadosLoco.Enabled =
                ddlDadosTipoLoco.Enabled =
                ddlDadosCorredor.Enabled =
                ddlDadosSituacao.Enabled =
                chkDadosConsidera.Enabled =
                txtDadosRadioID.Enabled =
                txtDadosModeloAC.Enabled =
                txtDadosSerialAC.Enabled =
                txtDadosModeloAB.Enabled =
                txtDadosSerialAB.Enabled =
                chkAtivo.Enabled = false;
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success disabled";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkLimpar.Enabled = false;
                lnkLimpar.CssClass = "btn btn-default disabled";
            }
        }

        #endregion

    }
}