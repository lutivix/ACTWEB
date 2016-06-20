using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Cadastro_Downloads : System.Web.UI.Page
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
                lblDownlodasID.Text = Id = Uteis.Descriptografar(Request.QueryString["di"].ToString(), "a#3G6**@").ToUpper();

                Limpar();

                ControlarBarraComandos(Id);
                if (Id != string.Empty)
                    CarregaDados(Id);

            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        protected void lnkSalvar_Click(object sender, EventArgs e)
        {
            string filepath = MapPath(@"/download/");

            try
            {
                if (fupArquivo.FileName != string.Empty)
                {
                    if (File.Exists(filepath +  Path.GetFileName(fupArquivo.FileName)))
                        File.Delete(filepath +  Path.GetFileName(fupArquivo.FileName));

                    fupArquivo.SaveAs(filepath +  Path.GetFileName(fupArquivo.FileName));
                }

                var downloads = new DownloadsController();

                Downloads dwl = new Downloads();

                if (lblDownlodasID.Text.Length > 0)
                    dwl.Downloads_ID = double.Parse(ViewState["id"].ToString());
                else
                    dwl.Downloads_ID = null;

                dwl.Modulo_do_Sistema = 1;
                dwl.Arquivo = txtArquivo.Text.Length > 0 ? txtArquivo.Text : null;
                dwl.Descricao = txtDescricao.Text.Length > 0 ? txtDescricao.Text : null;
                if (txtVersao.Text.Length > 0)
                    dwl.Versao = decimal.Parse(txtVersao.Text.Trim());
                dwl.Previsao_Atualizacao = txtPrevisao.Text.Length > 0 ? txtPrevisao.Text : null;
                dwl.Atualizacao = DateTime.Now;
                dwl.Liberado_SN = chkLiberadoDownload.Checked ? "S" : "N";


                if (dwl.Downloads_ID != null) // Alterando um registro existente
                {
                    dwl.Ativo_SN = chkAtivo.Checked ? "S" : "N";

                    if (downloads.ObterDownloadsPorFiltro(dwl, "tela_cadastro", null).Count == 0)
                    {
                        if (downloads.Salvar(dwl, ulMatricula))
                        {
                            Response.Write("<script>alert('Registro alterado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Altere o registro antes de Salvá-lo.' });", true);
                }
                else // Gravando um registro novo no banco
                {
                    if (downloads.ObterDownloadsPorFiltro(dwl, "tela_cadastro", null).Count == 0) // Se o registro não existir no banco salva o registro novo
                    {
                        dwl.Ativo_SN = chkAtivo.Checked ? "S" : "N";

                        if (downloads.Salvar(dwl, ulMatricula))
                        {
                            Response.Write("<script>alert('Registro gravado com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível gravar o registro no sistema.' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro já existe no sistema.' });", true);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected void lnkCalncelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@").ToString());
        }

        protected void lnkExcluir_Click(object sender, EventArgs e)
        {
            if (bool.Parse(Request.Form["confirm_value"]))
            {
                string filepath = MapPath("/download/" + Path.GetFileName(txtArquivo.Text));

                try
                {
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);

                        var downloads = new DownloadsController();

                        if (downloads.Excluir(double.Parse(lblDownlodasID.Text), lblUsuarioMatricula.Text))
                        {
                            Response.Write("<script>alert('Registro excluido com sucesso! Por " + ulMatricula + " - " + ulPerfil + "'); window.location='/Consulta/ConsultaDownloads.aspx?lu=" + Uteis.Criptografar(ulNome.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(ulMatricula.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(ulPerfil.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(ulMaleta.ToLower(), "a#3G6**@") + "'</script>");
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Não foi possível exluir o registro, tente novamente mais tarde ou entre em contato com o administrador do sistema.' });", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Arquivo não localizado no sistema.' });", true);

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }
            }
        }

        protected void lnkLimpar_Click(object sender, EventArgs e)
        {
            Limpar();
        }


        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void CarregaDados(string id)
        {
            var pesquisar = new DownloadsController();

            var item = pesquisar.ObterDownloadsPorId(int.Parse(id));

            txtArquivo.Text = item.Arquivo != null ? item.Arquivo : string.Empty;
            txtDescricao.Text = item.Descricao != null ? item.Descricao : string.Empty;
            txtVersao.Text = item.Versao != null ? item.Versao.Value.ToString() : string.Empty;
            txtPrevisao.Text = item.Previsao_Atualizacao != null ? item.Previsao_Atualizacao : string.Empty;
            chkLiberadoDownload.Checked = item.Liberado_SN == "Sim" ? true : false;
            chkAtivo.Checked = item.Ativo_SN == "Sim" ? true : false;

        }

        protected void Limpar()
        {
            txtArquivo.Text =
            txtDescricao.Text =
            txtPrevisao.Text =
            txtVersao.Text = string.Empty;
            chkLiberadoDownload.Checked = false;
            chkAtivo.Checked = true;
        }

        #endregion

        #region [ MÉTODOS DE CONTROLE DO FORMULÁRIO ]

        protected void ControlarBarraComandos(string status)
        {
            if (status == string.Empty)
            {
                lblTitulo.Text = "Cadastro de Downloads";
                chkAtivo.Checked = true;
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = false;
                lnkExcluir.CssClass = "btn btn-danger disabled";
                lnkLimpar.Enabled = true;
                lnkLimpar.CssClass = "btn btn-default";
                txtPrevisao.Text = "Sem previsão";
            }
            else
            {
                lblTitulo.Text = "Alteração de Abreviatura";
                lnkSalvar.Enabled = true;
                lnkSalvar.CssClass = "btn btn-success";
                lnkCalncelar.Enabled = true;
                lnkCalncelar.CssClass = "btn btn-info";
                lnkExcluir.Enabled = true;
                lnkExcluir.CssClass = "btn btn-danger";
                lnkLimpar.Enabled = false;
                lnkLimpar.CssClass = "btn btn-default disabled";
            }
        }

        #endregion
    }
}