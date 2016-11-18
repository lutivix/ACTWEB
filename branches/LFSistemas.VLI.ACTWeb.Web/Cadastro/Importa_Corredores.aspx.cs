using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;


namespace LFSistemas.VLI.ACTWeb.Web.Cadastro
{
    public partial class Importa_Corredores : System.Web.UI.Page
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

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                CarregaCombos(null);
            }
        }

        protected void lnkImportar_Click(object sender, EventArgs e)
        {
            Importa();
        }


        [System.Web.Services.WebMethod]
        public static string GetText()
        {
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(1000);
            }
            return "All finished!";
        }






        public void Importa()
        {

            string caminho = string.Empty;
            double Atualizados = 0;
            CultureInfo elGR = CultureInfo.CreateSpecificCulture("el-GR");

            lblTotalLidos.Text = lblTotalImportados.Text = string.Empty;

            if (fupPlanilha.PostedFile.FileName != string.Empty)
            {
                try
                {

                    if (fupPlanilha.HasFile)  //if file uploaded
                    {
                        if (checkFileType(fupPlanilha.FileName))  //Check for file types
                        {
                            if (File.Exists(Server.MapPath("../upload/" + fupPlanilha.FileName)))
                                File.Delete(Server.MapPath("../upload/" + fupPlanilha.FileName));

                            fupPlanilha.SaveAs(Server.MapPath("../upload/" + fupPlanilha.FileName));
                            caminho = Server.MapPath("../upload/" + fupPlanilha.FileName);
                        }
                        else
                            Response.Write("<script language =Javascript> alert('O formato do arquivo não é válido, selecione um arquivo do tipo: xls ou xlsx.');</script>");
                    }

                    List<double> selecionados = new List<double>();

                    int qtdeRotas = 0;
                    //Pegar todos os itens do repeater
                    for (int i = 0; i < rptListaItens.Items.Count; i++)
                    {
                        //Pegando o HiddenField dentro do repeater
                        HiddenField HiddenField1 = (HiddenField)rptListaItens.Items[i].FindControl("HiddenField1");

                        double equipamento = double.Parse(HiddenField1.Value);

                        //Pegando o CheckBox dentro do repeater
                        CheckBox chkRota = (CheckBox)rptListaItens.Items[i].FindControl("chkEquipamento");

                        //Verificar se foi selecionado
                        if (chkRota.Checked)
                        {
                            selecionados.Add(equipamento);
                        }
                    }

                    var trechos = string.Join(",", selecionados);

                    string strFileType = System.IO.Path.GetExtension(caminho.ToLower());
                    string sSourceConstr = String.Empty;

                    if (strFileType.Trim() == ".xls")
                    {
                        sSourceConstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source =" + caminho + "; Extended Properties=\"Excel 8.0; HDR=Yes; IMEX=2\"";
                    }
                    else if (strFileType.Trim() == ".xlsx")
                    {
                        sSourceConstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + caminho + ";Extended Properties='Excel 12.0 Xml;HDR=YES';";
                    }

                    OleDbConnection conexao = new OleDbConnection(sSourceConstr);

                    OleDbDataAdapter adapter = new OleDbDataAdapter(@"SELECT CORREDOR, LATITUDE, LONGITUDE, KM, VELOCIDADE_ASC, VELOCIDADE_DESC, NOME_SB, TRECHO FROM [Plan1$] WHERE TRECHO IN (" + trechos + ")", conexao);
                    DataSet ds = new DataSet();


                    conexao.Open();
                    adapter.Fill(ds);

                    List<Importa_Corredor> itens = new List<Importa_Corredor>();

                    foreach (DataRow linha in ds.Tables[0].Rows)
                    {
                        Importa_Corredor item = new Importa_Corredor();

                        item.Corredor = linha["CORREDOR"].ToString();
                        item.Latitude = linha["LATITUDE"].ToString() != "-" ? linha["LATITUDE"].ToString() : null;
                        item.Longitude = linha["LONGITUDE"].ToString() != "-" ? linha["LONGITUDE"].ToString() : null;
                        item.KM = linha["KM"].ToString() != "-" ? linha["KM"].ToString() : null;
                        item.Velocidade_Asc = linha["VELOCIDADE_ASC"].ToString() != "-" ? linha["VELOCIDADE_ASC"].ToString() : null;
                        item.Velocidade_Desc = linha["VELOCIDADE_DESC"].ToString() != "-" ? linha["VELOCIDADE_DESC"].ToString() : null;
                        item.Secao = linha["NOME_SB"].ToString();
                        item.Trecho = linha["TRECHO"].ToString();

                        itens.Add(item);
                    }


                    if (!string.IsNullOrEmpty(trechos))
                    {
                        var acao = new ImportacaoController();

                        lblTotalLidos.Text = string.Format("{0:0,0}", itens.Count);
                        lblTotalImportados.Text = string.Format("{0:0,0}", acao.Importa_Corredores(itens, trechos, lblUsuarioMatricula.Text));

                        CarregaCombos(null);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Selecione pelo menos um trecho.' });", true);

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

        private bool checkFileType(string fileName)
        {
            string fileExt = Path.GetExtension(fupPlanilha.FileName);

            switch (fileExt.ToLower())
            {
                case ".xls":
                    return true;
                case ".xlsx":
                    return true;
                default:
                    return false;
            }
        }
        private string createDir()
        {
            string caminho = string.Empty;
            DirectoryInfo myDir = new DirectoryInfo(MapPath("../upload/"));
            myDir.Create();

            //Now save file
            fupPlanilha.SaveAs(Server.MapPath("../upload/" + fupPlanilha.FileName));

            caminho = Server.MapPath("../upload/" + fupPlanilha.FileName);

            Response.Write("<script language =Javascript> alert('File Uploaded Successfully,Click Show Images');</script>");

            return caminho;
        }


        #region [ CARREGA COMBOS ]

        protected void CarregaCombos(string origem)
        {
            var pesquisa = new ComboBoxController();

            var trechos = pesquisa.ComboBoxTrechos();
            if (trechos.Count > 0)
            {
                rptListaItens.DataSource = trechos;
                rptListaItens.DataBind();
            }
        }

        #endregion

    }
}