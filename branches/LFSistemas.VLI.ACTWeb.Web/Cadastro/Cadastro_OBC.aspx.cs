using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Cadastro_OBC : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Display> itens { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }
        public string Flag { get; set; }
        public string Id { get; set; }

        #endregion
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

                lblUsuarioLogado.Text = ulNome.Length > 12 ? ulNome.Substring(0, 12).ToUpper() : ulNome;
                lblUsuarioMatricula.Text = ulMatricula = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = ulPerfil = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = ulMaleta = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
                if (Request.QueryString["id"] != null) lblOBCID.Text = Uteis.Descriptografar(Request.QueryString["id"].ToString(), "a#3G6**@").ToUpper();
                txtDadosDataAtual.Text = DateTime.Now.ToShortDateString();
                ControlarBarraComandos(Flag);
                if (Id != null && Flag != "n")
                    CarregaDadosNoFormulario(Id);
            }

        }
        protected void lnkSalvar_Click1(object sender, EventArgs e)
        {
            string filepath = MapPath("/download/");
            HttpFileCollection uploadedFiles = Request.Files;

            for (int i = 0; i < uploadedFiles.Count; i++)
            {
                HttpPostedFile userPostedFile = uploadedFiles[i];

                try
                {
                    if (userPostedFile.ContentLength > 0)
                    {
                        Label1.Text += "<u>File #" + (i + 1) +
                           "</u><br>";
                        Label1.Text += "File Content Type: " +
                           userPostedFile.ContentType + "<br>";
                        Label1.Text += "File Size: " +
                           userPostedFile.ContentLength + "kb<br>";
                        Label1.Text += "File Name: " +
                           userPostedFile.FileName + "<br>";

                        userPostedFile.SaveAs(filepath + "\\" +
                           System.IO.Path.GetFileName(userPostedFile.FileName));
                    }
                }
                catch (Exception)
                {
                    Response.Write("<script>alert('Arquivo gravado com sucesso mas com erro no upload')</script>");

                }
            }

            var menu = new OBCController();

            OBC men = new OBC();

            if (lblOBCID.Text.Length > 0)
                men.Obc_ID = double.Parse(lblOBCID.Text);
            else

                men.Obc_ID = null;

            men.Versao_Firm = decimal.Parse(txtFirmwere.Text);
            men.Versao_Mapa = double.Parse(txtMapa.Text);
            men.Atualizacao_Firm = txtPrvF.Text.ToUpper();
            men.Atualizacao_Mapa = txtPrvM.Text.ToUpper();
            men.Data_Atualizacao = txtDadosDataAtual.Text;
            men.Ativo_SN = chkAtivo.Checked ? "S" : "N";


            #region [ ALTERANDO UM REGISTRO EXISTENTE ]

            if (men.Obc_ID != null) // Alterando um registro existente
            {
                int count = 0;
                double? id = null;
                var dados = menu.ObterTodos(null, null, null);

                for (int i = 0; i < dados.Count; i++)
                {
                    if (dados[i].Ativo_SN == "Sim")
                    {
                        count++;
                        id = dados[i].Obc_ID;
                    }
                }
                if (count == 1 && men.Obc_ID == id && men.Ativo_SN == "N")
                {
                    // Se existir apenas um ativo no banco e for o mesmo que estou alterando posso alterar qualquer informação menos desativar o mesmo.
                    // alerta
                    Response.Write("<script>alert('Operação não realizada pois e obrigatorio possuir um OBC ativo')</script>");

                }
                else
                {

                    if (menu.SalvarInformação(men, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaOBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
            }
            #endregion

            #region [ GRAVANDO UM REGISTRO NOVO ]
            else // Gravando um registro novo no banco
            {
                if (menu.ObterOBCPorFiltro(men, null).Count == 0) // Se o registro não existir no banco salva o registro novo
                {
                    if (menu.SalvarInformação(men, ulMatricula))
                    {
                        Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaOBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
            }
            #endregion
        }

        private void CarregaDadosNoFormulario(string Men)
        {
            var OBC = new OBCController();


            var dados = OBC.ObterOBCPorID(int.Parse(Men));
            if (dados != null)
            {
                txtFirmwere.Text = dados.Versao_Firm.Value.ToString();
                txtMapa.Text = dados.Versao_Mapa.Value.ToString();
                txtPrvF.Text = dados.Atualizacao_Mapa;
                txtPrvM.Text = dados.Atualizacao_Mapa;
                txtDadosDataAtual.Text = dados.Data_Atualizacao;
                chkAtivo.Checked = dados.Ativo_SN == "S" ? true : false;
            }
        }
        protected void ControlarBarraComandos(string status)
        {
            switch (status)
            {
                case "n":
                    lblTitulo.Text = "Cadastro OBC";

                    lnkSalvar.Enabled = true;
                    lnkSalvar.CssClass = "btn btn-success";
                    lnkCancelar.Enabled = true;
                    lnkCancelar.CssClass = "btn btn-info";
                    lnkExcluir.Enabled = false;
                    lnkExcluir.CssClass = "btn btn-danger disabled";
                    break;
                case "a":
                    
                    lblTitulo.Text = "Alteração de OBC";
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
        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                if (lblOBCID.Text.Length > 0)
                {
                    var obc = new OBCController();
                    var men = new OBC();

                    men.Obc_ID = double.Parse(lblOBCID.Text);
                    men.Ativo_SN = chkAtivo.Checked ? "S" : "N";

                    int count = 0;
                    double? id = null;
                    var dados = obc.ObterTodos(null, null, null);

                    for (int i = 0; i < dados.Count; i++)
                    {
                        if (dados[i].Ativo_SN == "Sim")
                        {
                            count++;
                            id = dados[i].Obc_ID;
                        }
                    }
                    if (count == 1 && men.Obc_ID == id)
                    {
                        // Se existir apenas um ativo no banco e for o mesmo que estou alterando posso alterar qualquer informação menos desativar o mesmo.
                        // alerta
                        Response.Write("<script>alert('Operação não realizada pois e obrigatorio possuir um OBC ativo')</script>");
                    }

                    else
                    {
                        if (obc.ApagarOBCPorID(double.Parse(lblOBCID.Text)))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaOBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível excluir o registro no banco.' });", true);
                    }
                }
            }
        }
        protected void lnkCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaOBC.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());

        }

    }
}





