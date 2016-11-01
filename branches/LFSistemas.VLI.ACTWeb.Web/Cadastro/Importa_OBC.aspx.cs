using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.UI;

namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Importa_OBC : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        int total = 0;
        int lidos = 0;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text    = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text    = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text    = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();
            }
        }

        protected void lnkImportar_Click(object sender, EventArgs e)
        {
            Importa();
        }
        public void Importa()
        {
            if (fupPlanilha.PostedFile.FileName != string.Empty)
            {
                try
                {
                    fupPlanilha.SaveAs(Server.MapPath("../upload/" + fupPlanilha.FileName));

                    string caminho = Server.MapPath("../upload/" + fupPlanilha.FileName);

                    string strFileType = System.IO.Path.GetExtension(caminho.ToLower());
                    string sSourceConstr = String.Empty;

                    

                    if (strFileType.Trim() == ".xls")
                    {
                        sSourceConstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + caminho + "; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        sSourceConstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source =" + caminho + "; Extended Properties=\"Excel 12.0; HDR=Yes; IMEX=2\"";
                    }

                    OleDbConnection oconn = new OleDbConnection(sSourceConstr);


                    OleDbCommand ocmd = new OleDbCommand("select LOCO, CORREDOR, FROTA from [Plan1$]", oconn);
                    oconn.Open();

                    OleDbDataReader odr = ocmd.ExecuteReader();

                    OleDbDataAdapter da = new OleDbDataAdapter("select LOCO, CORREDOR, FROTA from [Plan1$]", oconn);

                    DataTable dados = new DataTable();

                    da.Fill(dados);

                    List<Informacao_OBC> itens = new List<Informacao_OBC>();

                    while (odr.Read())
                    {
                        var obc = new Informacao_OBC();
                        obc.Loco = Valid(odr, 0);
                        obc.Corredor = Valid(odr, 1);
                        obc.Frota = Valid(odr, 2);

                        itens.Add(obc);

                        lidos++;
                        lblTotalLidos.Text = lidos.ToString();
                    }

                    if (Insert(itens, lblUsuarioMatricula.Text))
                        lblTotalImportados.Text = itens.Count.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione um arquivo.' });", true);
        }
        protected string Valid(OleDbDataReader myreader, int stval)//if any columns are 
        //found null then they are replaced by zero
        {
            object val = myreader[stval];
            if (val != DBNull.Value)
                return val.ToString();
            else
                return Convert.ToString(0);
        }

        public bool Insert(List<Informacao_OBC> itens, string usuarioLogado)
        {
            bool retorno = false;

            var item = new OBCController();

            if (item.InsereInformacaoOBC(itens, usuarioLogado))
                retorno = true;

            return retorno;
        }
    }
}