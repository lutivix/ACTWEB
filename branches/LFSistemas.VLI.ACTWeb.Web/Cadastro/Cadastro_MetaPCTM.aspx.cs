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
    public partial class Cadastro_MetaPCTM : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Meta_PCTM> itens { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Flag { get; set; }
        public string Id { get; set; }

        #endregion

        #region [ EVENTOS ]

        #region [ MÉTODOS DE PÁGINA ]
        protected void Page_Load(object sender, EventArgs e)
        {
            ulNome = string.Format("{0}", ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper());
            ulMatricula = string.Format("{0}", ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper());
            ulPerfil = string.Format("{0}", ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper());
            ulMaleta = string.Format("{0}", ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper());

            if (Request.QueryString["lu"] != null) ViewState["ulNome"] = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mu"] != null) ViewState["uMatricula"] = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["pu"] != null) ViewState["uPerfil"] = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["mm"] != null) ViewState["ulMaleta"] = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
            if (Request.QueryString["id"] != null) ViewState["Id"] = Id = Uteis.Descriptografar(Request.QueryString["id"].ToString(), "a#3G6**@").ToUpper();
            Flag = Request.QueryString["flag"].ToString().ToLower();

            if (!Page.IsPostBack)
            {
                ulNome = ViewState["ulNome"].ToString();
                ulMatricula = ViewState["uMatricula"].ToString();
                ulPerfil = ViewState["uPerfil"].ToString();
                ulMaleta = ViewState["ulMaleta"].ToString();

                txtDadosValidade.Text = Uteis.ObterUltimoDiaDoMes().ToShortDateString();

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                if (Request.QueryString["id"] != null) lblMetaPCTMID.Text = Uteis.Descriptografar(Request.QueryString["id"].ToString(), "a#3G6**@").ToUpper();

                ControlarBarraComandos(Flag);
                ComboDadosRotas();
                if (Id != null && Flag != "n")
                    CarregaDadosNoFormulario(Id);
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            var metaController = new MetaPCTMController();

            Meta_PCTM meta = new Meta_PCTM();

            if (lblMetaPCTMID.Text.Length > 0)
            {
                meta.Meta_ID = double.Parse(lblMetaPCTMID.Text);
                meta.Validade = DateTime.Parse(txtDadosValidade.Text + " 23:59:59");
            }
            else
            {
                meta.Meta_ID = null;
                meta.Validade = Uteis.ObterUltimoDiaDoMes();
            }


            meta.Rota_ID = double.Parse(ddlDadosRotas.SelectedItem.Value);
            meta.Corredor_Nome = txtDadosCorredor.Text.Length > 0 ? txtDadosCorredor.Text : null;
            meta.Tipos_Trens = txtDadosPrefixo.Text.Length > 0 ? txtDadosPrefixo.Text : null;
            meta.Rota_Nome = ddlDadosRotas.SelectedItem.Text != "Selecione uma Rota!" ? ddlDadosRotas.SelectedItem.Text : null;
            meta.Publicacao = DateTime.Now;
            meta.Meta = double.Parse(txtDadosMeta.Text);
            meta.Ativo_SN = chkAtivo.Checked ? "S" : "N";

            if (meta.Meta_ID != null) // Alterando um registro existente
            {
                var existe = metaController.ObterMeta_PCTMPorFiltro(meta, "update");

                if (existe.Count <= 0) // Se NÃO existir uma meta com validade maior a que está passando deixa ativar a meta
                {
                    if (metaController.Salvar(meta, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível alterar o registro no sistema. Já existe uma meta para essa rota com validade superior a informada.' });", true);
            }
            else // Gravando um registro novo no banco
            {
                if (metaController.ObterMeta_PCTMPorFiltro(meta, "novo").Count == 0) // Se o registro não existir no banco salva o registro novo
                {
                    if (metaController.Salvar(meta, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
            }
        }
        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (lblMetaPCTMID.Text.Length > 0)
                {
                    var metaController = new MetaPCTMController();
                    double ID = double.Parse(lblMetaPCTMID.Text);

                    if (metaController.ApagarMeta_PCTMPorID(ID))
                    {
                        Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível excluir o registro no banco.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não é possível excluir um registro que ainda não foi cadastrado no banco.' });", true);
            }
        }
        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultarMetaPCTM.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        #endregion

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        private void CarregaDadosNoFormulario(string id)
        {
            var meta = new MetaPCTMController();


            var dados = meta.ObterMetaPCTMPorID(double.Parse(id));
            if (dados != null)
            {
                ddlDadosRotas.SelectedValue = dados.Rota_ID.ToString();

                txtDadosCorredor.Text = dados.Corredor_Nome;
                txtDadosPrefixo.Text = dados.Tipos_Trens;
                txtDadosValidade.Text = dados.Validade.Value.ToShortDateString();
                txtDadosMeta.Text = dados.Meta.ToString();
                chkAtivo.Checked = dados.Ativo_SN == "Sim" ? true : false;
            }
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        public void ComboDadosRotas()
        {
            var metaController = new MetaPCTMController();
            ddlDadosRotas.DataValueField = "Id";
            ddlDadosRotas.DataTextField = "Descricao";
            ddlDadosRotas.DataSource = metaController.ObterComboRotas();
            ddlDadosRotas.DataBind();
            ddlDadosRotas.Items.Insert(0, "Selecione uma Rota!");
            ddlDadosRotas.SelectedIndex = 0;
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]
        protected void ControlarBarraComandos(string status)
        {
            switch (status)
            {
                case "n":
                    lblTitulo.Text = "Cadastro de Metas do Painel de Concentração de Trens da Malha - VL!";

                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = false;
                    lnkExcluir.CssClass = "btn btn-danger disabled";

                    break;
                case "a":
                    lblTitulo.Text = "Alteração de Metas do Painel de Concentração de Trens da Malha - VL!";
                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = true;
                    lnkExcluir.CssClass = "btn btn-danger";
                    break;
                default:
                    break;
            }
        }
        protected void ddlDadosRotas_SelectedIndexChanged(object sender, EventArgs e)
        {

            var rotaController = new RotasController();
            if (ddlDadosRotas.SelectedItem.Value != "Selecione uma Rota!")
            {
                var rota = rotaController.ObterRotasPorID(double.Parse(ddlDadosRotas.SelectedItem.Value));
                if (rota != null)
                {
                    txtDadosCorredor.Text = rota.Corredor;
                    txtDadosPrefixo.Text = rota.Prefixo;
                }
                else
                {
                    txtDadosCorredor.Text = string.Format(" ");
                    txtDadosPrefixo.Text = string.Format(" ");
                }
            }
            else
            {
                txtDadosCorredor.Text = string.Format(" ");
                txtDadosPrefixo.Text = string.Format(" ");
            }
            ddlDadosRotas.Focus();
        }

        #endregion

    }
}