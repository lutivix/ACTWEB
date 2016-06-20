using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Relatorios
{
    public partial class CCO : System.Web.UI.Page
    {
        public Relatorio_CCO item { get; set; }
        public List<Operador> ListaOperadores { get; set; }
        public List<PostoTrabalho> ListaPostoTrabalho { get; set; }
        public string matriculas { get; set; }
        public string psttrabalho { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

            lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
            lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
            lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

            if (!IsPostBack)
            {
                txtDataInicial.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtDataFinal.Text = DateTime.Now.ToShortDateString();
                txtHoraInicial.Text = DateTime.Now.ToShortTimeString();
                txtHoraFinal.Text = DateTime.Now.ToShortTimeString();

                var operadorController = new OperadorController();
                var postotrabalhoController = new PostoTrabalhoController();

                ListaOperadores = operadorController.ObterTodos();
                ListaPostoTrabalho = postotrabalhoController.ObterTodos();

                rptListaOperadores.DataSource = ListaOperadores;
                rptListaOperadores.DataBind();

                rptListaPostoTrabalho.DataSource = ListaPostoTrabalho;
                rptListaPostoTrabalho.DataBind();

                pnlRepiter.Visible = false;
            }
        }

        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            var relatorio_ccoController = new Relatorio_ccoController();

            List<Relatorio_CCO> lista = new List<Relatorio_CCO>();

            List<string> matriculas = new List<string>();
            List<string> operadores = new List<string>();
            List<string> postotrabalho = new List<string>();

            int qtdeOperadores = 0;
            int qtdePostoTrabalho = 0;
            int contadorNormal = 0;
            int contadorPostoTrabalho = 0;

            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaOperadores.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField1 = (HiddenField)rptListaOperadores.Items[i].FindControl("HiddenField1");

                string[] operador = HiddenField1.Value.Split(':');

                //Pegando o CheckBox dentro do repeater
                CheckBox chkOperador = (CheckBox)rptListaOperadores.Items[i].FindControl("chkOperador");

                //Verificar se foi selecionado
                if (chkOperador.Checked)
                {
                    matriculas.Insert(qtdeOperadores, "'" + operador[0] + "'");
                    operadores.Insert(qtdeOperadores, operador[1]);
                    qtdeOperadores++;
                }
            }

            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaPostoTrabalho.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField2 = (HiddenField)rptListaPostoTrabalho.Items[i].FindControl("HiddenField2");

                //Pegando o CheckBox dentro do repeater
                CheckBox chkPostoTrabalho = (CheckBox)rptListaPostoTrabalho.Items[i].FindControl("chkPostoTrabalho");

                //Verificar se foi selecionado
                if (chkPostoTrabalho.Checked)
                {
                    postotrabalho.Insert(qtdePostoTrabalho, HiddenField2.Value);
                    qtdePostoTrabalho++;
                }
            }

            DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
            DateTime horaFinal = txtHoraFinal.Text != string.Empty ? DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text)) : DateTime.Now;

            if (postotrabalho.Count <= 0)
            {
                for (int i = 0; i < operadores.Count; i++) // Por operador
                {
                    item = relatorio_ccoController.ObterPorOperador(new Entities.FiltroRelatorio_CCO()
                    {
                        Matricula = matriculas[i],
                        Operador = operadores[i],
                        DataInicial = horaInicio,
                        DataFinal = horaFinal
                    });
                    if (item.QuantidadeMediaCaracteresMacro0 != null || item.QuantidadeMediaLicencaHora != null ||
                        item.QuantidadeMediaTrensHoraTrabalho != null || item.TempoMedioLicenciamentoMacro14 != null ||
                        item.TempoMedioRespostaEntradaVia != null || item.TempoRespostaMacro9 != null || item.THP != null)
                    {
                        lista.Insert(contadorNormal, item);
                        contadorNormal++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < operadores.Count; i++) // Por posto de trabalho
                {
                    for (int j = 0; j < postotrabalho.Count; j++)
                    {
                        item = relatorio_ccoController.ObterPorPostoTrabalho(new Entities.FiltroRelatorio_CCO()
                        {
                            Matricula = matriculas[i],
                            Operador = operadores[i],
                            DataInicial = horaInicio,
                            DataFinal = horaFinal,
                            PostoTrabalho = postotrabalho[j],

                        });
                        if (item.QuantidadeMediaCaracteresMacro0 != null || item.QuantidadeMediaLicencaHora != null ||
                            item.QuantidadeMediaTrensHoraTrabalho != null || item.TempoMedioLicenciamentoMacro14 != null ||
                            item.TempoMedioRespostaEntradaVia != null || item.TempoRespostaMacro9 != null || item.THP != null)
                        {
                            lista.Insert(contadorPostoTrabalho, item);
                            contadorPostoTrabalho++;
                        }
                    }
                }
            }

            if (lista.Count > 0)
            {
                pnlRepiter.Visible = true;
                rptRelatorio_CCO.DataSource = lista.OrderBy(o => o.DataInicial).OrderBy(o => o.Operador);
                rptRelatorio_CCO.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'A consulta não retornou informações' });", true);
        }

        protected string FormataHora(string hora)
        {
            string Retorno = hora;

            if (hora.Length == 1)
            {
                Retorno = "0" + hora + ":00";
                txtHoraInicial.Text = Retorno;
            }
            if (hora.Length == 2)
            {
                Retorno = hora + ":00";
                txtHoraInicial.Text = Retorno;
            }
            if (hora.Length == 3)
            {
                Retorno = hora + "00";
                txtHoraFinal.Text = Retorno;
            }
            if (hora.Length == 4)
            {
                Retorno = hora + "0";
                txtHoraFinal.Text = Retorno;
            }

            return Retorno;
        }

        protected void btnGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var relatorio_ccoController = new Relatorio_ccoController();

            List<Relatorio_CCO> lista = new List<Relatorio_CCO>();

            List<string> matriculas = new List<string>();
            List<string> operadores = new List<string>();
            List<string> postotrabalho = new List<string>();

            int qtdeOperadores = 0;
            int qtdePostoTrabalho = 0;
            int contadorNormal = 0;
            int contadorPostoTrabalho = 0;

            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaOperadores.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField1 = (HiddenField)rptListaOperadores.Items[i].FindControl("HiddenField1");

                string[] operador = HiddenField1.Value.Split(':');

                //Pegando o CheckBox dentro do repeater
                CheckBox chkOperador = (CheckBox)rptListaOperadores.Items[i].FindControl("chkOperador");

                //Verificar se foi selecionado
                if (chkOperador.Checked)
                {
                    matriculas.Insert(qtdeOperadores, "'" + operador[0] + "'");
                    operadores.Insert(qtdeOperadores, operador[1]);
                    qtdeOperadores++;
                }
            }

            //Pegar todos os itens do repeater
            for (int i = 0; i < rptListaPostoTrabalho.Items.Count; i++)
            {
                //Pegando o HiddenField dentro do repeater
                HiddenField HiddenField2 = (HiddenField)rptListaPostoTrabalho.Items[i].FindControl("HiddenField2");

                //Pegando o CheckBox dentro do repeater
                CheckBox chkPostoTrabalho = (CheckBox)rptListaPostoTrabalho.Items[i].FindControl("chkPostoTrabalho");

                //Verificar se foi selecionado
                if (chkPostoTrabalho.Checked)
                {
                    postotrabalho.Insert(qtdePostoTrabalho, HiddenField2.Value);
                    qtdePostoTrabalho++;
                }
            }

            DateTime horaInicio = txtHoraInicial.Text != string.Empty ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicial.Text)) : DateTime.Now;
            DateTime horaFinal = txtHoraFinal.Text != string.Empty ? DateTime.Parse(txtDataFinal.Text + " " + FormataHora(txtHoraFinal.Text)) : DateTime.Now;

            if (postotrabalho.Count <= 0)
            {
                for (int i = 0; i < operadores.Count; i++) // Por operador
                {
                    item = relatorio_ccoController.ObterPorOperador(new Entities.FiltroRelatorio_CCO()
                    {
                        Matricula = matriculas[i],
                        Operador = operadores[i],
                        DataInicial = horaInicio,
                        DataFinal = horaFinal
                    });
                    if (item.QuantidadeMediaCaracteresMacro0 != null || item.QuantidadeMediaLicencaHora != null ||
                        item.QuantidadeMediaTrensHoraTrabalho != null || item.TempoMedioLicenciamentoMacro14 != null ||
                        item.TempoMedioRespostaEntradaVia != null || item.TempoRespostaMacro9 != null || item.THP != null)
                    {
                        lista.Insert(contadorNormal, item);
                        contadorNormal++;
                    }
                }
            }
            else
            {
                for (int i = 0; i < operadores.Count; i++) // Por posto de trabalho
                {
                    for (int j = 0; j < postotrabalho.Count; j++)
                    {
                        item = relatorio_ccoController.ObterPorPostoTrabalho(new Entities.FiltroRelatorio_CCO()
                        {
                            Matricula = matriculas[i],
                            Operador = operadores[i],
                            DataInicial = horaInicio,
                            DataFinal = horaFinal,
                            PostoTrabalho = postotrabalho[j],

                        });
                        if (item.QuantidadeMediaCaracteresMacro0 != null || item.QuantidadeMediaLicencaHora != null ||
                            item.QuantidadeMediaTrensHoraTrabalho != null || item.TempoMedioLicenciamentoMacro14 != null ||
                            item.TempoMedioRespostaEntradaVia != null || item.TempoRespostaMacro9 != null || item.THP != null)
                        {
                            lista.Insert(contadorPostoTrabalho, item);
                            contadorPostoTrabalho++;
                        }
                    }
                }
            }

            lista.OrderBy(o => o.DataInicial).OrderBy(o => o.Operador);

            if (lista.Count > 0)
            {
                if (postotrabalho.Count <= 0) // Sem posto de trabalho
                {
                    sb.AppendLine("OPERADOR;DATA INICIAL;DATA FINAL;TMP. MD. RESPOSTA MACRO 9;TMP. MD. LICENCIAMENTO;TMP. MD. RESP. ENTRADA VIA;QTDE MD. CARACTERES MACRO 0");
                    foreach (var item in lista)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0));
                    }
                }
                else // Com posto de trabalho
                {
                    sb.AppendLine("OPERADOR;DATA INICIAL;DATA FINAL;TMP. MD. RESPOSTA MACRO 9;TMP. MD. LICENCIAMENTO;TMP. MD. RESP. ENTRADA VIA;QTDE MD. CARACTERES MACRO 0;POSTO TRABALHO");
                    foreach (var item in lista)
                    {
                        if (item.PostoTrabalho == "1")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Amarelo"));
                        else if (item.PostoTrabalho == "2")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Vermelho"));
                        else if (item.PostoTrabalho == "3")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Verde"));
                        else if (item.PostoTrabalho == "4")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Azul"));
                        else if (item.PostoTrabalho == "5")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Branco"));
                        else if (item.PostoTrabalho == "6")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Branco T"));
                        else if (item.PostoTrabalho == "7")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Laranja"));
                        else if (item.PostoTrabalho == "8")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Azul Ceu"));
                        else if (item.PostoTrabalho == "9")
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, "Vinho"));
                        else
                            sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7}", item.Operador, item.DataInicial, item.DataFinal, item.TempoRespostaMacro9, item.TempoMedioLicenciamentoMacro14, item.TempoMedioRespostaEntradaVia, item.QuantidadeMediaCaracteresMacro0, ""));
                    }
                }

                Response.Clear();
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.AddHeader("content-disposition", "attachment; filename=relatorio_CCO.csv");
                Response.Write(sb.ToString());
                Response.End();
            }
            else
                Response.Write("<script> alert('A consulta não retornou informações'); </script>");
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDataInicial.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            txtDataFinal.Text = DateTime.Now.ToShortDateString();
            txtHoraInicial.Text = DateTime.Now.ToShortTimeString();
            txtHoraFinal.Text = DateTime.Now.ToShortTimeString();

            var operadorController = new OperadorController();
            var postotrabalhoController = new PostoTrabalhoController();

            ListaOperadores = operadorController.ObterTodos();
            ListaPostoTrabalho = postotrabalhoController.ObterTodos();

            rptListaOperadores.DataSource = ListaOperadores;
            rptListaOperadores.DataBind();

            rptListaPostoTrabalho.DataSource = ListaPostoTrabalho;
            rptListaPostoTrabalho.DataBind();

            pnlRepiter.Visible = false;
        }

    }
}