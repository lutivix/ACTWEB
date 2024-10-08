using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class Macro : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Macro> itens { get; set; }
        public List<Entities.Macro> zerado { get; set; }
        public string corredores { get; set; }
        public string ulNome { get; set; }
        public string ulMatricula { get; set; }
        public string ulPerfil { get; set; }
        public string ulMaleta { get; set; }

        public int UserListCount { get; set; }
        public int NowViewing
        {
            get
            {
                object obj = ViewState["_NowViewing"];
                if (obj == null)
                    return 0;
                else
                    return (int)obj;
            }
            set
            {
                this.ViewState["_NowViewing"] = value;
            }
        }
        public enum Navigation
        {
            None,
            Primeira,
            Proxima,
            Anterior,
            Ultima,
            Pager,
            Sorting
        }

        #endregion

        #region [ EVENTOS DE PÁGINA ]

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Pesquisar(null, Navigation.None);
                ViewState["ordenacao"] = "ASC";
            }
        }

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]

        
        protected void ButtonPesquisar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNumeroLocomotiva.Text) && string.IsNullOrEmpty(txtNumeroTrem.Text) && string.IsNullOrEmpty(txtCodigoOS.Text) && string.IsNullOrEmpty(txtNumeroMacro.Text))
            {
                if (int.Parse(ddlMais.SelectedValue) > 4)
                    Response.Write("<script>alert('A busca esta limitada em 4 horas, para pesquisar em um intervalo maior de tempo escolha macro, loco ou trem !');</script>");
                else
                {
                    if (rdJuntas.Checked)
                        CarregarMacrosJuntas(null, Navigation.None);
                    if (rdRecebidas.Checked)
                        CarregarMacrosRecebidas(null, Navigation.None);
                    if (rdEnviadas.Checked)
                        CarregarMacrosEnviadas(null, Navigation.None);
                }
            }
            else
            {
                if (rdJuntas.Checked)
                    CarregarMacrosJuntas(null, Navigation.None);
                if (rdRecebidas.Checked)
                    CarregarMacrosRecebidas(null, Navigation.None);
                if (rdEnviadas.Checked)
                    CarregarMacrosEnviadas(null, Navigation.None);
            }

        }
        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            txtDataInicio.Text = dataIni.ToShortDateString();
            txtHoraInicio.Text = dataIni.ToShortTimeString();

            // limpa todos os campos da tela
            txtNumeroLocomotiva.Text =
            txtNumeroTrem.Text =
            txtNumeroMacro.Text =
            txtCodigoOS.Text =
            txtMotivo.Text =
            txtExpressao.Text = string.Empty;
            rdTras.Checked = rdJuntas.Checked = true;
            ddlMais.SelectedIndex = 0;
            clbCorredor.ClearSelection();


            if (rdJuntas.Checked)
                this.CarregarMacrosJuntas(null, Navigation.None);
            if (rdEnviadas.Checked)
                this.CarregarMacrosEnviadas(null, Navigation.None);
            if (rdRecebidas.Checked)
                this.CarregarMacrosRecebidas(null, Navigation.None);
        }
        protected void ButtonGerarExcel_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            var macroController = new MacroController();

            if (rdJuntas.Checked)
            {
                #region [ GERA EXCEL MACRO JUNTAS ]

                string result = string.Empty;
                int Espaco = 0;
                DateTime horaFim = DateTime.Now;
                pnlJuntas.Visible = true;
                pnlEnviadas.Visible = pnlRecebidas.Visible = false;

                if (!string.IsNullOrEmpty(txtMotivo.Text))
                {
                    string[] m = txtMotivo.Text.Split(',');
                    string[] a = { };

                    for (int i = 0; i < m.Length; i++)
                    {
                        if (m[i].Length == 1)
                        {
                            Array.Resize(ref a, a.Length + 1);

                            a[i] = "'0" + m[i] + "'";
                            m[i] = "'_" + m[i] + "'";
                        }
                        else
                        {
                            Array.Resize(ref a, a.Length + 1);
                            m[i] = "'" + m[i] + "'";
                        }
                    }

                    var y = m.Concat(a).Where(w => w != null);

                    result = String.Join(result + ',', y);
                }
                else
                    result = txtMotivo.Text != string.Empty ? txtMotivo.Text : string.Empty;

                if (txtHoraInicio.Text.Length > 1)
                    if (int.Parse(txtHoraInicio.Text.Substring(0, 2)) > 24)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Hora inválida, favor digitar hora válida.' });", true);
                    }
                    else
                    {
                        if ((int.Parse(txtHoraInicio.Text.Substring(0, 2)) == 24))
                            txtHoraInicio.Text = "00:00";

                        if (rdParaFrente.Checked)
                        {
                            DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                            horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                            Espaco = 0;
                        }

                        else if (rdTras.Checked)
                        {
                            DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                            horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                            Espaco = 1;
                        }

                        var aux = new List<string>();
                        if (clbCorredor.Items.Count > 0)
                        {
                            for (int i = 0; i < clbCorredor.Items.Count; i++)
                            {
                                if (clbCorredor.Items[i].Selected)
                                {
                                    aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                                }
                            }
                            if (aux.Count <= 0)
                            {
                                aux.Add("'Baixada'");
                                aux.Add("'Centro Leste'");
                                aux.Add("'Centro Norte'");
                                aux.Add("'Centro Sudeste'");
                                aux.Add("'Minas Bahia'");
                                aux.Add("'Minas Rio'");
                                aux.Add("'-'");
                                aux.Add("' '");
                            }
                            else
                            {
                                aux.Add("'-'");
                                aux.Add("' '");
                            }

                            corredores = string.Join(",", aux);
                        }

                        itens = macroController.ObterTodos(new Entities.FiltroMacro()
                        {
                            NumeroLocomotiva = txtNumeroLocomotiva.Text,
                            NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                            NumeroMacro = txtNumeroMacro.Text,
                            CodigoOS = txtCodigoOS.Text,
                            DataInicio = DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)),
                            DataFim = horaFim,
                            Expressao = txtExpressao.Text,
                            Espaco = Espaco,
                            Motivo = result,
                            Corredores = corredores
                        });

                        if (itens.Count > 0)
                        {
                            sb.AppendLine("E/R;ID;HORÁRIO;LOCO;MACRO;TEXTO;LOCALIZAÇÃO;MCT;TREM;CODIGO OS;");


                            foreach (var macro in itens)
                            {
                                sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9}", macro.Tipo, macro.ID, macro.Horario, macro.Locomotiva, macro.NumeroMacro, Uteis.RetirarPontoeVirgula(macro.Texto), Uteis.RetirarPontoeVirgula(macro.Localizacao), macro.MCT, macro.Trem, macro.CodigoOS));
                            }
                        }
                        else
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                    }

                #endregion
            }
            else if (rdEnviadas.Checked)
            {
                #region [ GERA EXCEL MACRO ENVIADAS ]

                int Espaco = 0;
                DateTime horaFim = DateTime.Now;
                pnlEnviadas.Visible = true;
                pnlJuntas.Visible = pnlRecebidas.Visible = false;

                if (Page.IsPostBack)
                {
                    if (rdParaFrente.Checked)
                    {
                        DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                        horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                        Espaco = 0;
                    }

                    else if (rdTras.Checked)
                    {
                        DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                        horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                        Espaco = 1;
                    }
                }

                var itens = macroController.ObterEnviadas(new Entities.FiltroMacro()
                {
                    NumeroLocomotiva = txtNumeroLocomotiva.Text,
                    NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                    NumeroMacro = txtNumeroMacro.Text,
                    CodigoOS = txtCodigoOS.Text,
                    DataInicio = DateTime.Parse(txtDataInicio.Text + " " + txtHoraInicio.Text),
                    DataFim = horaFim,
                    Expressao = txtExpressao.Text,
                    Espaco = Espaco
                });

                if (itens.Count > 0)
                {
                    sb.AppendLine("E/R;LOCOMOTIVA;TREM;CODIGO OS;MCT;HORÁRIO;MACRO;TEXTO;CONFIRMAÇÃO;DIFERENÇA TEMPO;SITUAÇÃO");

                    foreach (var macro in itens)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", macro.Tipo, macro.Locomotiva, macro.Trem, macro.CodigoOS, macro.MCT, macro.Horario, macro.NumeroMacro, macro.Texto, macro.Confirmacao_Leitura, macro.Tempo_Decorrido, macro.Status));
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

                #endregion
            }
            else if (rdRecebidas.Checked)
            {
                #region [ GERA EXCEL MACRO RECEBIDAS ]

                string result = string.Empty;
                int Espaco = 0;
                DateTime horaFim = DateTime.Now;
                pnlRecebidas.Visible = true;
                pnlJuntas.Visible = pnlEnviadas.Visible = false;
                if (!string.IsNullOrEmpty(txtMotivo.Text))
                {
                    string[] m = txtMotivo.Text.Split(',');

                    for (int i = 0; i < m.Length; i++)
                    {
                        if (m[i].Length == 1)
                            m[i] = "'_" + m[i] + "'";
                        else
                            m[i] = "'" + m[i] + "'";
                    }

                    result = String.Join(result + ',', m);
                }
                else
                    result = txtMotivo.Text != string.Empty ? txtMotivo.Text : string.Empty;

                if (rdParaFrente.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                    Espaco = 0;
                }

                else if (rdTras.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                    Espaco = 1;
                }

                var aux = new List<string>();
                if (clbCorredor.Items.Count > 0)
                {
                    for (int i = 0; i < clbCorredor.Items.Count; i++)
                    {
                        if (clbCorredor.Items[i].Selected)
                        {
                            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                        }
                    }

                    if (aux.Count <= 0)
                    {
                        aux.Add("'Baixada'");
                        aux.Add("'Centro Leste'");
                        aux.Add("'Centro Norte'");
                        aux.Add("'Centro Sudeste'");
                        aux.Add("'Minas Bahia'");
                        aux.Add("'Minas Rio'");
                        aux.Add("'-'");
                        aux.Add("' '");
                    }
                    else
                    {
                        aux.Add("'-'");
                        aux.Add("' '");
                    }

                    corredores = string.Join(",", aux);
                }

                var itens = macroController.ObterRecebidas(new Entities.FiltroMacro()
                {
                    NumeroLocomotiva = txtNumeroLocomotiva.Text,
                    NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                    NumeroMacro = txtNumeroMacro.Text,
                    CodigoOS = txtCodigoOS.Text,
                    DataInicio = DateTime.Parse(txtDataInicio.Text + " " + txtHoraInicio.Text),
                    DataFim = horaFim,
                    Expressao = txtExpressao.Text,
                    Espaco = Espaco,
                    Motivo = result,
                    Corredores = corredores
                });

                if (itens.Count > 0)
                {
                    sb.AppendLine("E/R;LOCOMOTIVA;TREM;CODIGO OS;HORÁRIO;MACRO;TEXTO;LOCALIZAÇÃO;MCT;LATITUDE;LONGITUDE");

                    foreach (var macro in itens)
                    {
                        sb.AppendLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}", macro.Tipo, macro.Locomotiva, macro.Trem, macro.CodigoOS, macro.Horario, macro.NumeroMacro, Uteis.RetirarPontoeVirgula(macro.Texto), Uteis.RetirarPontoeVirgula(macro.Localizacao), macro.MCT, macro.Latitude, macro.Longitude));
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

                #endregion
            }


            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
            Response.AddHeader("content-disposition", "attachment; filename=macros.csv");
            Response.Write(sb.ToString());
            Response.End();
        }
        protected void ButtonAtualizarHora_Click(object sender, EventArgs e)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            txtDataInicio.Text = dataIni.ToShortDateString();
            txtHoraInicio.Text = dataIni.ToShortTimeString();

            if (rdJuntas.Checked)
                this.CarregarMacrosJuntas(null, Navigation.None);
            if (rdEnviadas.Checked)
                this.CarregarMacrosEnviadas(null, Navigation.None);
            if (rdRecebidas.Checked)
                this.CarregarMacrosRecebidas(null, Navigation.None);
        }

        #region [ ORDENAR POR COLUNAS - JUNTAS ]

        protected void lnkJuntasRE_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasPrefixo7D_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasCodOS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasHorario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasMacro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasTratado_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkJuntasCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosJuntas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosJuntas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        #endregion

        #region [ ORDENAR POR COLUNAS - ENVIADAS ]
        
        protected void lnkEnviadasRE_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasPrefixo7D_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasCodOS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasHorario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasMacro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasTempoEnvio_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("TEMPO_ENVIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("TEMPO_ENVIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasTratado_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasSituacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("SITUACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("SITUACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkEnviadasCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosEnviadas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosEnviadas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        #endregion

        #region [ ORDENAR POR COLUNAS - RECEBIDAS ]
        
        protected void lnkRecebidasRE_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasPrefixo7D_OnClick(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("PREFIXO7D " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasCodOS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasHorario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasMacro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasLocalizacao_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("LOCALIZACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("LOCALIZACAO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasMCT_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("MCT " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasLatitude_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("LATITUDE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("LATITUDE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasLongitude_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("LONGITUDE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("LONGITUDE " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasTratado_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("TRATADO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkRecebidasCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                CarregarMacrosRecebidas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                CarregarMacrosRecebidas("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        #endregion

        #region [ NAVEGAR NO REPITER ]

        protected void lnkJ_PrimeiraPagina_Click(object sender, EventArgs e)
        {
            PrimeiraPagina();
        }
        protected void lnkJ_PaginaAnterior_Click(object sender, EventArgs e)
        {
            PaginaAnterior();
        }
        protected void lnkJ_ProximaPagina_Click(object sender, EventArgs e)
        {
            ProximaPagina();
        }
        protected void lnkJ_UltimaPagina_Click(object sender, EventArgs e)
        {
            UltimaPagina();
        }
        protected void lnkE_PrimeiraPagina_Click(object sender, EventArgs e)
        {
            PrimeiraPagina();
        }
        protected void lnkE_PaginaAnterior_Click(object sender, EventArgs e)
        {
            PaginaAnterior();
        }
        protected void lnkE_ProximaPagina_Click(object sender, EventArgs e)
        {
            ProximaPagina();
        }
        protected void lnkE_UltimaPagia_Click(object sender, EventArgs e)
        {
            UltimaPagina();
        }
        protected void lnkR_UltimaPagia_Click(object sender, EventArgs e)
        {
            UltimaPagina();
        }
        protected void lnkR_ProximaPagina_Click(object sender, EventArgs e)
        {
            ProximaPagina();
        }
        protected void lnkR_PaginaAnterior_Click(object sender, EventArgs e)
        {
            PaginaAnterior();
        }
        protected void lnkR_PrimeiraPagina_Click(object sender, EventArgs e)
        {
            PrimeiraPagina();
        }

        protected void PaginaAnterior()
        {
            if (rdJuntas.Checked)
                CarregarMacrosJuntas(null, Navigation.Anterior);
            if (rdRecebidas.Checked)
                CarregarMacrosRecebidas(null, Navigation.Anterior);
            if (rdEnviadas.Checked)
                CarregarMacrosEnviadas(null, Navigation.Anterior);
        }
        protected void ProximaPagina()
        {
            if (rdJuntas.Checked)
                CarregarMacrosJuntas(null, Navigation.Proxima);
            if (rdRecebidas.Checked)
                CarregarMacrosRecebidas(null, Navigation.Proxima);
            if (rdEnviadas.Checked)
                CarregarMacrosEnviadas(null, Navigation.Proxima);
        }
        protected void PrimeiraPagina()
        {
            if (rdJuntas.Checked)
                CarregarMacrosJuntas(null, Navigation.Primeira);
            if (rdRecebidas.Checked)
                CarregarMacrosRecebidas(null, Navigation.Primeira);
            if (rdEnviadas.Checked)
                CarregarMacrosEnviadas(null, Navigation.Primeira);
        }
        protected void UltimaPagina()
        {
            //Fill repeater for Last event
            if (rdJuntas.Checked)
                CarregarMacrosJuntas(null, Navigation.Ultima);
            if (rdRecebidas.Checked)
                CarregarMacrosRecebidas(null, Navigation.Ultima);
            if (rdEnviadas.Checked)
                CarregarMacrosEnviadas(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;
            Pesquisar(null, Navigation.Anterior);
        }

        #endregion

        #endregion

        #endregion

        #region [ MÉTODOS DE SELEÇÃO DOS RADIOBUTTONS ]

        protected void rdJuntas_CheckedChanged(object sender, EventArgs e)
        {
            CarregarMacrosJuntas(null, Navigation.None);
        }
        protected void rdEnviadas_CheckedChanged(object sender, EventArgs e)
        {
            CarregarMacrosEnviadas(null, Navigation.None);
        }
        protected void rdRecebidas_CheckedChanged(object sender, EventArgs e)
        {
            CarregarMacrosRecebidas(null, Navigation.None);
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            var dataIni = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            var dataFim = DateTime.Parse(DateTime.Now.AddMinutes(-20).ToString("dd/MM/yyyy HH:mm"));
            txtDataInicio.Text = dataIni.ToShortDateString();
            txtHoraInicio.Text = dataIni.ToShortTimeString();

            corredores = "'Baixada', 'Centro Leste', 'Centro Norte', 'Centro Sudeste', 'Minas Bahia', 'Minas Rio'";

            pnlJuntas.Visible = true;
            pnlEnviadas.Visible = pnlRecebidas.Visible = false;

            string result = string.Empty;

            var macroController = new MacroController();
            itens = macroController.ObterTodos(new Entities.FiltroMacro()
            {
                NumeroLocomotiva = txtNumeroLocomotiva.Text,
                NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                NumeroMacro = txtNumeroMacro.Text,
                CodigoOS = txtCodigoOS.Text,
                DataInicio = dataIni,
                DataFim = dataFim,
                Expressao = txtExpressao.Text,
                Espaco = 1,
                Motivo = result,
                Corredores = corredores
            });

            if (itens.Count > 0)
            {

                switch (ordenacao)
                {
                    case "TIPO ASC":
                        itens = itens.OrderBy(o => o.Tipo).ToList();
                        break;
                    case "TIPO DESC":
                        itens = itens.OrderByDescending(o => o.Tipo).ToList();
                        break;
                    case "LOCO ASC":
                        itens = itens.OrderBy(o => o.Locomotiva).ToList();
                        break;
                    case "LOCO DESC":
                        itens = itens.OrderByDescending(o => o.Locomotiva).ToList();
                        break;
                    case "TREM ASC":
                        itens = itens.OrderBy(o => o.Trem).ToList();
                        break;
                    case "TREM DESC":
                        itens = itens.OrderByDescending(o => o.Trem).ToList();
                        break;
                    case "COD_OS ASC":
                        itens = itens.OrderBy(o => o.CodigoOS).ToList();
                        break;
                    case "COD_OS DESC":
                        itens = itens.OrderByDescending(o => o.CodigoOS).ToList();
                        break;
                    case "HORARIO ASC":
                        itens = itens.OrderBy(o => o.Horario).ToList();
                        break;
                    case "HORARIO DESC":
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                    case "MACRO ASC":
                        itens = itens.OrderBy(o => o.NumeroMacro).ToList();
                        break;
                    case "MACRO DESC":
                        itens = itens.OrderByDescending(o => o.NumeroMacro).ToList();
                        break;
                    case "TEXTO ASC":
                        itens = itens.OrderBy(o => o.Texto).ToList();
                        break;
                    case "TEXTO DESC":
                        itens = itens.OrderByDescending(o => o.Texto).ToList();
                        break;
                    case "TRATADO ASC":
                        itens = itens.OrderBy(o => o.Tratado).ToList();
                        break;
                    case "TRATADO DESC":
                        itens = itens.OrderByDescending(o => o.Tratado).ToList();
                        break;
                    case "CORREDOR ASC":
                        itens = itens.OrderBy(o => o.Corredor).ToList();
                        break;
                    case "CORREDOR DESC":
                        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                        break;
                    default:
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                }

                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = itens;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlJ_ItensPorPagina.SelectedValue);

                switch (navigation)
                {
                    case Navigation.Proxima:
                        NowViewing++;
                        break;
                    case Navigation.Anterior:
                        NowViewing--;
                        break;
                    case Navigation.Ultima:
                        NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(ddlJ_ItensPorPagina.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblJ_PaginaAte.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkJ_PaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkJ_ProximaPagina.Enabled = !objPds.IsLastPage;
                lnkJ_PrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkJ_UltimaPagina.Enabled = !objPds.IsLastPage;

                this.RepeaterJuntas.DataSource = objPds;
                this.RepeaterJuntas.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
        }
        protected void CarregarMacrosJuntas(string ordenacao, Navigation navigation)
        {
            string result = string.Empty;
            int Espaco = 0;
            DateTime horaFim = DateTime.Now;
            pnlJuntas.Visible = true;
            pnlEnviadas.Visible = pnlRecebidas.Visible = false;

            if (!string.IsNullOrEmpty(txtMotivo.Text))
            {
                string[] m = txtMotivo.Text.Split(',');
                string[] a = { };

                for (int i = 0; i < m.Length; i++)
                {
                    if (m[i].Length == 1)
                    {
                        Array.Resize(ref a, a.Length + 1);

                        a[i] = "'0" + m[i] + "'";
                        m[i] = "'_" + m[i] + "'";
                    }
                    else
                    {
                        Array.Resize(ref a, a.Length + 1);
                        m[i] = "'" + m[i] + "'";
                    }
                }

                var y = m.Concat(a).Where(w => w != null);

                result = String.Join(result + ',', y);
            }
            else
                result = txtMotivo.Text != string.Empty ? txtMotivo.Text : string.Empty;

            if (txtHoraInicio.Text.Length > 1)
                if (int.Parse(txtHoraInicio.Text.Substring(0, 2)) > 24)
                {
                    Response.Write("<script>alert('Hora inválida, favor digitar hora válida');</script>");
                }
                else
                {
                    if ((int.Parse(txtHoraInicio.Text.Substring(0, 2)) == 24))
                        txtHoraInicio.Text = "00:00";

                    if (rdParaFrente.Checked)
                    {
                        DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                        horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                        Espaco = 0;
                    }

                    else if (rdTras.Checked)
                    {
                        DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                        horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                        Espaco = 1;
                    }

                    var aux = new List<string>();
                    if (clbCorredor.Items.Count > 0)
                    {
                        for (int i = 0; i < clbCorredor.Items.Count; i++)
                        {
                            if (clbCorredor.Items[i].Selected)
                            {
                                aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                            }
                        }
                        if (aux.Count <= 0)
                        {
                            aux.Add("'Baixada'");
                            aux.Add("'Centro Leste'");
                            aux.Add("'Centro Norte'");
                            aux.Add("'Centro Sudeste'");
                            aux.Add("'Minas Bahia'");
                            aux.Add("'Minas Rio'");
                            aux.Add("'-'");
                            aux.Add("' '");
                        }
                        else
                        {
                            aux.Add("'-'");
                            aux.Add("' '");
                        }

                        corredores = string.Join(",", aux);
                    }

                    var macroController = new MacroController();
                    itens = macroController.ObterTodos(new Entities.FiltroMacro()
                    {
                        NumeroLocomotiva = txtNumeroLocomotiva.Text,
                        NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                        NumeroMacro = txtNumeroMacro.Text,
                        CodigoOS = txtCodigoOS.Text,
                        DataInicio = DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)),
                        DataFim = horaFim,
                        Expressao = txtExpressao.Text,
                        Espaco = Espaco,
                        Motivo = result,
                        Corredores = corredores
                    });

                    if (itens.Count > 0)
                    {

                        switch (ordenacao)
                        {
                            case "TIPO ASC":
                                itens = itens.OrderBy(o => o.Tipo).ToList();
                                break;
                            case "TIPO DESC":
                                itens = itens.OrderByDescending(o => o.Tipo).ToList();
                                break;
                            case "LOCO ASC":
                                itens = itens.OrderBy(o => o.Locomotiva).ToList();
                                break;
                            case "LOCO DESC":
                                itens = itens.OrderByDescending(o => o.Locomotiva).ToList();
                                break;
                            case "TREM ASC":
                                itens = itens.OrderBy(o => o.Trem).ToList();
                                break;
                            case "TREM DESC":
                                itens = itens.OrderByDescending(o => o.Trem).ToList();
                                break;
                            case "PREFIXO7D ASC":
                                itens = itens.OrderBy(o => o.Prefixo7D).ToList();
                                break;
                            case "PREFIXO7D DESC":
                                itens = itens.OrderByDescending(o => o.Prefixo7D).ToList();
                                break;

                            case "COD_OS ASC":
                                itens = itens.OrderBy(o => o.CodigoOS).ToList();
                                break;
                            case "COD_OS DESC":
                                itens = itens.OrderByDescending(o => o.CodigoOS).ToList();
                                break;
                            case "HORARIO ASC":
                                itens = itens.OrderBy(o => o.Horario).ToList();
                                break;
                            case "HORARIO DESC":
                                itens = itens.OrderByDescending(o => o.Horario).ToList();
                                break;
                            case "MACRO ASC":
                                itens = itens.OrderBy(o => o.NumeroMacro).ToList();
                                break;
                            case "MACRO DESC":
                                itens = itens.OrderByDescending(o => o.NumeroMacro).ToList();
                                break;
                            case "TEXTO ASC":
                                itens = itens.OrderBy(o => o.Texto).ToList();
                                break;
                            case "TEXTO DESC":
                                itens = itens.OrderByDescending(o => o.Texto).ToList();
                                break;
                            case "TRATADO ASC":
                                itens = itens.OrderBy(o => o.Tratado).ToList();
                                break;
                            case "TRATADO DESC":
                                itens = itens.OrderByDescending(o => o.Tratado).ToList();
                                break;
                            case "CORREDOR ASC":
                                itens = itens.OrderBy(o => o.Corredor).ToList();
                                break;
                            case "CORREDOR DESC":
                                itens = itens.OrderByDescending(o => o.Corredor).ToList();
                                break;
                            default:
                                itens = itens.OrderByDescending(o => o.Horario).ToList();
                                break;
                        }

                        PagedDataSource objPds = new PagedDataSource();
                        objPds.DataSource = itens;
                        objPds.AllowPaging = true;
                        objPds.PageSize = int.Parse(ddlJ_ItensPorPagina.SelectedValue);

                        switch (navigation)
                        {
                            case Navigation.Proxima:
                                NowViewing++;
                                break;
                            case Navigation.Anterior:
                                NowViewing--;
                                break;
                            case Navigation.Ultima:
                                NowViewing = objPds.PageCount - 1;
                                break;
                            case Navigation.Pager:
                                if (int.Parse(ddlJ_ItensPorPagina.SelectedValue) >= objPds.PageCount)
                                    NowViewing = objPds.PageCount - 1;
                                break;
                            case Navigation.Sorting:
                                break;
                            default:
                                NowViewing = 0;
                                break;
                        }
                        objPds.CurrentPageIndex = NowViewing;
                        lblJ_PaginaAte.Text = "Página: " + (NowViewing + 1).ToString() + " de " +
                                              objPds.PageCount.ToString();
                        lnkJ_PaginaAnterior.Enabled = !objPds.IsFirstPage;
                        lnkJ_ProximaPagina.Enabled = !objPds.IsLastPage;
                        lnkJ_PrimeiraPagina.Enabled = !objPds.IsFirstPage;
                        lnkJ_UltimaPagina.Enabled = !objPds.IsLastPage;

                        this.RepeaterJuntas.DataSource = objPds;
                        this.RepeaterJuntas.DataBind();
                    }
                    else
                    {
                        this.RepeaterJuntas.DataSource = itens;
                        this.RepeaterJuntas.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!",
                            " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);
                    }

                    lblTotalJuntas.Text = string.Format("{0:0,0}", itens.Count);
                }
            else
            {

                if (rdParaFrente.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                    Espaco = 0;
                }

                else if (rdTras.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                    Espaco = 1;
                }

                var macroController = new MacroController();
                itens = macroController.ObterTodos(new Entities.FiltroMacro()
                {
                    NumeroLocomotiva = txtNumeroLocomotiva.Text,
                    NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                    NumeroMacro = txtNumeroMacro.Text,
                    CodigoOS = txtCodigoOS.Text,
                    DataInicio = DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)),
                    DataFim = horaFim,
                    Espaco = Espaco,
                    Motivo = result,
                    Corredores = corredores
                });

                if (itens.Count > 0)
                {
                    switch (ordenacao)
                    {
                        case "TIPO ASC":
                            itens = itens.OrderBy(o => o.Tipo).ToList();
                            break;
                        case "TIPO DESC":
                            itens = itens.OrderByDescending(o => o.Tipo).ToList();
                            break;
                        case "LOCO ASC":
                            itens = itens.OrderBy(o => o.Locomotiva).ToList();
                            break;
                        case "LOCO DESC":
                            itens = itens.OrderByDescending(o => o.Locomotiva).ToList();
                            break;
                        case "TREM ASC":
                            itens = itens.OrderBy(o => o.Trem).ToList();
                            break;
                        case "TREM DESC":
                            itens = itens.OrderByDescending(o => o.Trem).ToList();
                            break;
                        case "COD_OS ASC":
                            itens = itens.OrderBy(o => o.CodigoOS).ToList();
                            break;
                        case "COD_OS DESC":
                            itens = itens.OrderByDescending(o => o.CodigoOS).ToList();
                            break;
                        case "HORARIO ASC":
                            itens = itens.OrderBy(o => o.Horario).ToList();
                            break;
                        case "HORARIO DESC":
                            itens = itens.OrderByDescending(o => o.Horario).ToList();
                            break;
                        case "MACRO ASC":
                            itens = itens.OrderBy(o => o.NumeroMacro).ToList();
                            break;
                        case "MACRO DESC":
                            itens = itens.OrderByDescending(o => o.NumeroMacro).ToList();
                            break;
                        case "TEXTO ASC":
                            itens = itens.OrderBy(o => o.Texto).ToList();
                            break;
                        case "TEXTO DESC":
                            itens = itens.OrderByDescending(o => o.Texto).ToList();
                            break;
                        case "TRATADO ASC":
                            itens = itens.OrderBy(o => o.Tratado).ToList();
                            break;
                        case "TRATADO DESC":
                            itens = itens.OrderByDescending(o => o.Tratado).ToList();
                            break;
                        case "CORREDOR ASC":
                            itens = itens.OrderBy(o => o.Corredor).ToList();
                            break;
                        case "CORREDOR DESC":
                            itens = itens.OrderByDescending(o => o.Corredor).ToList();
                            break;
                        default:
                            itens = itens.OrderByDescending(o => o.Horario).ToList();
                            break;
                    }

                    PagedDataSource objPds = new PagedDataSource();
                    objPds.DataSource = itens;
                    objPds.AllowPaging = true;
                    objPds.PageSize = int.Parse(ddlJ_ItensPorPagina.SelectedValue);

                    switch (navigation)
                    {
                        case Navigation.Proxima:
                            NowViewing++;
                            break;
                        case Navigation.Anterior:
                            NowViewing--;
                            break;
                        case Navigation.Ultima:
                            NowViewing = objPds.PageCount - 1;
                            break;
                        case Navigation.Pager:
                            if (int.Parse(ddlJ_ItensPorPagina.SelectedValue) >= objPds.PageCount)
                                NowViewing = objPds.PageCount - 1;
                            break;
                        case Navigation.Sorting:
                            break;
                        default:
                            NowViewing = 0;
                            break;
                    }
                    objPds.CurrentPageIndex = NowViewing;
                    lblJ_PaginaAte.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                    lnkJ_PaginaAnterior.Enabled = !objPds.IsFirstPage;
                    lnkJ_ProximaPagina.Enabled = !objPds.IsLastPage;
                    lnkJ_PrimeiraPagina.Enabled = !objPds.IsFirstPage;
                    lnkJ_UltimaPagina.Enabled = !objPds.IsLastPage;

                    this.RepeaterJuntas.DataSource = objPds;
                    this.RepeaterJuntas.DataBind();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

                lblTotalJuntas.Text = string.Format("{0:0,0}", itens.Count);
            }
        }
        protected void CarregarMacrosEnviadas(string ordenacao, Navigation navigation)
        {
            int Espaco = 0;
            DateTime horaFim = DateTime.Now;
            pnlEnviadas.Visible = true;
            pnlJuntas.Visible = pnlRecebidas.Visible = false;

            if (Page.IsPostBack)
            {
                if (rdParaFrente.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                    Espaco = 0;
                }

                else if (rdTras.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                    Espaco = 1;
                }
            }

            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }
                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }

            var macroController = new MacroController();
            var itens = macroController.ObterEnviadas(new Entities.FiltroMacro()
            {
                NumeroLocomotiva = txtNumeroLocomotiva.Text,
                NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                NumeroMacro = txtNumeroMacro.Text,
                CodigoOS = txtCodigoOS.Text,
                DataInicio = DateTime.Parse(txtDataInicio.Text + " " + txtHoraInicio.Text),
                DataFim = horaFim,
                Expressao = txtExpressao.Text,
                Espaco = Espaco,
                Corredores = corredores
            });

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "TIPO ASC":
                        itens = itens.OrderBy(o => o.Tipo).ToList();
                        break;
                    case "TIPO DESC":
                        itens = itens.OrderByDescending(o => o.Tipo).ToList();
                        break;
                    case "LOCO ASC":
                        itens = itens.OrderBy(o => o.Locomotiva).ToList();
                        break;
                    case "LOCO DESC":
                        itens = itens.OrderByDescending(o => o.Locomotiva).ToList();
                        break;
                    case "TREM ASC":
                        itens = itens.OrderBy(o => o.Trem).ToList();
                        break;
                    case "TREM DESC":
                        itens = itens.OrderByDescending(o => o.Trem).ToList();
                        break;
                    case "PREFIXO7D ASC":
                        itens = itens.OrderBy(o => o.Prefixo7D).ToList();
                        break;
                    case "PREFIXO7D DESC":
                        itens = itens.OrderByDescending(o => o.Prefixo7D).ToList();
                        break;
                    case "COD_OS ASC":
                        itens = itens.OrderBy(o => o.CodigoOS).ToList();
                        break;
                    case "COD_OS DESC":
                        itens = itens.OrderByDescending(o => o.CodigoOS).ToList();
                        break;
                    case "HORARIO ASC":
                        itens = itens.OrderBy(o => o.Horario).ToList();
                        break;
                    case "HORARIO DESC":
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                    case "MACRO ASC":
                        itens = itens.OrderBy(o => o.NumeroMacro).ToList();
                        break;
                    case "MACRO DESC":
                        itens = itens.OrderByDescending(o => o.NumeroMacro).ToList();
                        break;
                    case "TEXTO ASC":
                        itens = itens.OrderBy(o => o.Texto).ToList();
                        break;
                    case "TEXTO DESC":
                        itens = itens.OrderByDescending(o => o.Texto).ToList();
                        break;
                    case "TEMPO_ENVIO ASC":
                        itens = itens.OrderBy(o => o.Tempo_Decorrido).ToList();
                        break;
                    case "TEMPO_ENVIO DESC":
                        itens = itens.OrderByDescending(o => o.Tempo_Decorrido).ToList();
                        break;
                    case "TRATADO ASC":
                        itens = itens.OrderBy(o => o.Tratado).ToList();
                        break;
                    case "TRATADO DESC":
                        itens = itens.OrderByDescending(o => o.Tratado).ToList();
                        break;
                    case "SITUACAO ASC":
                        itens = itens.OrderBy(o => o.Status).ToList();
                        break;
                    case "SITUACAO DESC":
                        itens = itens.OrderByDescending(o => o.Status).ToList();
                        break;
                    default:
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                }

                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = itens;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlE_ItensPorPagina.SelectedValue);

                switch (navigation)
                {
                    case Navigation.Proxima:
                        NowViewing++;
                        break;
                    case Navigation.Anterior:
                        NowViewing--;
                        break;
                    case Navigation.Ultima:
                        NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(ddlE_ItensPorPagina.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblE_PaginaAte.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkE_PaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkE_ProximaPagina.Enabled = !objPds.IsLastPage;
                lnkE_PrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkE_UltimaPagina.Enabled = !objPds.IsLastPage;

                this.RepeaterEnviadas.DataSource = objPds;
                this.RepeaterEnviadas.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotalEnviadas.Text = string.Format("{0:0,0}", itens.Count);
        }
        protected void CarregarMacrosRecebidas(string ordenacao, Navigation navigation)
        {
            string result = string.Empty;
            int Espaco = 0;
            DateTime horaFim = DateTime.Now;
            pnlRecebidas.Visible = true;
            pnlJuntas.Visible = pnlEnviadas.Visible = false;
            if (!string.IsNullOrEmpty(txtMotivo.Text))
            {
                string[] m = txtMotivo.Text.Split(',');

                for (int i = 0; i < m.Length; i++)
                {
                    if (m[i].Length == 1)
                        m[i] = "'_" + m[i] + "'";
                    else
                        m[i] = "'" + m[i] + "'";
                }

                result = String.Join(result + ',', m);
            }
            else
                result = txtMotivo.Text != string.Empty ? txtMotivo.Text : string.Empty;

            if (Page.IsPostBack)
            {
                if (rdParaFrente.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
                    Espaco = 0;
                }

                else if (rdTras.Checked)
                {
                    DateTime horaInicio = txtHoraInicio.Text != string.Empty ? DateTime.Parse(txtDataInicio.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
                    horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
                    Espaco = 1;
                }
            }

            var aux = new List<string>();
            if (clbCorredor.Items.Count > 0)
            {
                for (int i = 0; i < clbCorredor.Items.Count; i++)
                {
                    if (clbCorredor.Items[i].Selected)
                    {
                        aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                    }
                }

                if (aux.Count <= 0)
                {
                    aux.Add("'Baixada'");
                    aux.Add("'Centro Leste'");
                    aux.Add("'Centro Norte'");
                    aux.Add("'Centro Sudeste'");
                    aux.Add("'Minas Bahia'");
                    aux.Add("'Minas Rio'");
                    aux.Add("'-'");
                    aux.Add("' '");
                }
                else
                {
                    aux.Add("'-'");
                    aux.Add("' '");
                }

                corredores = string.Join(",", aux);
            }

            var macroController = new MacroController();
            var itens = macroController.ObterRecebidas(new Entities.FiltroMacro()
            {
                NumeroLocomotiva = txtNumeroLocomotiva.Text,
                NumeroTrem = txtNumeroTrem.Text.ToUpper(),
                NumeroMacro = txtNumeroMacro.Text,
                CodigoOS = txtCodigoOS.Text,
                DataInicio = DateTime.Parse(txtDataInicio.Text + " " + txtHoraInicio.Text),
                DataFim = horaFim,
                Expressao = txtExpressao.Text,
                Espaco = Espaco,
                Motivo = result,
                Corredores = corredores
            });

            if (itens.Count > 0)
            {
                switch (ordenacao)
                {
                    case "TIPO ASC":
                        itens = itens.OrderBy(o => o.Tipo).ToList();
                        break;
                    case "TIPO DESC":
                        itens = itens.OrderByDescending(o => o.Tipo).ToList();
                        break;
                    case "LOCO ASC":
                        itens = itens.OrderBy(o => o.Locomotiva).ToList();
                        break;
                    case "LOCO DESC":
                        itens = itens.OrderByDescending(o => o.Locomotiva).ToList();
                        break;
                    case "TREM ASC":
                        itens = itens.OrderBy(o => o.Trem).ToList();
                        break;
                    case "TREM DESC":
                        itens = itens.OrderByDescending(o => o.Trem).ToList();
                        break;
                    case "PREFIXO7D ASC":
                        itens = itens.OrderBy(o => o.Prefixo7D).ToList();
                        break;
                    case "PREFIXO7D DESC":
                        itens = itens.OrderByDescending(o => o.Prefixo7D).ToList();
                        break;
                    case "COD_OS ASC":
                        itens = itens.OrderBy(o => o.CodigoOS).ToList();
                        break;
                    case "COD_OS DESC":
                        itens = itens.OrderByDescending(o => o.CodigoOS).ToList();
                        break;
                    case "HORARIO ASC":
                        itens = itens.OrderBy(o => o.Horario).ToList();
                        break;
                    case "HORARIO DESC":
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                    case "MACRO ASC":
                        itens = itens.OrderBy(o => o.NumeroMacro).ToList();
                        break;
                    case "MACRO DESC":
                        itens = itens.OrderByDescending(o => o.NumeroMacro).ToList();
                        break;
                    case "TEXTO ASC":
                        itens = itens.OrderBy(o => o.Texto).ToList();
                        break;
                    case "TEXTO DESC":
                        itens = itens.OrderByDescending(o => o.Texto).ToList();
                        break;
                    case "LOCALIZACAO ASC":
                        itens = itens.OrderBy(o => o.Localizacao).ToList();
                        break;
                    case "LOCALIZACAO DESC":
                        itens = itens.OrderByDescending(o => o.Localizacao).ToList();
                        break;
                    case "MCT ASC":
                        itens = itens.OrderBy(o => o.MCT).ToList();
                        break;
                    case "MCT DESC":
                        itens = itens.OrderByDescending(o => o.MCT).ToList();
                        break;
                    case "LATITUDE ASC":
                        itens = itens.OrderBy(o => o.Latitude).ToList();
                        break;
                    case "LATITUDE DESC":
                        itens = itens.OrderByDescending(o => o.Latitude).ToList();
                        break;
                    case "LONGITUDE ASC":
                        itens = itens.OrderBy(o => o.Longitude).ToList();
                        break;
                    case "LONGITUDE DESC":
                        itens = itens.OrderByDescending(o => o.Longitude).ToList();
                        break;
                    case "TRATADO ASC":
                        itens = itens.OrderBy(o => o.Tratado).ToList();
                        break;
                    case "TRATADO DESC":
                        itens = itens.OrderByDescending(o => o.Tratado).ToList();
                        break;
                    case "CORREDOR ASC":
                        itens = itens.OrderBy(o => o.Corredor).ToList();
                        break;
                    case "CORREDOR DESC":
                        itens = itens.OrderByDescending(o => o.Corredor).ToList();
                        break;
                    default:
                        itens = itens.OrderByDescending(o => o.Horario).ToList();
                        break;
                }

                PagedDataSource objPds = new PagedDataSource();
                objPds.DataSource = itens;
                objPds.AllowPaging = true;
                objPds.PageSize = int.Parse(ddlR_ItensPorPagina.SelectedValue);

                switch (navigation)
                {
                    case Navigation.Proxima:
                        NowViewing++;
                        break;
                    case Navigation.Anterior:
                        NowViewing--;
                        break;
                    case Navigation.Ultima:
                        NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Pager:
                        if (int.Parse(ddlR_ItensPorPagina.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblR_PaginaAte.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkR_PaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkR_ProximaPagina.Enabled = !objPds.IsLastPage;
                lnkR_PrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkR_UltimaPagina.Enabled = !objPds.IsLastPage;


                this.RepeaterRecebidas.DataSource = objPds;
                this.RepeaterRecebidas.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotalRecebidas.Text = string.Format("{0:0,0}", itens.Count);
        }

        #endregion

        #region [ MÉTODOS DE APOIO ]

        void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Fill repeater for Pager event
            Pesquisar(null, Navigation.Pager);
        }
        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            ddlJ_ItensPorPagina.SelectedIndexChanged += new EventHandler(ddlPageSize_SelectedIndexChanged);
            ddlE_ItensPorPagina.SelectedIndexChanged += new EventHandler(ddlPageSize_SelectedIndexChanged);
        }
        protected string FormataHora(string hora)
        {
            string Retorno = hora;


            if (hora.Length == 1)
            {
                Retorno = "0" + hora + ":00";
                txtHoraInicio.Text = Retorno;
            }
            if (hora.Length == 2)
            {
                Retorno = hora + ":00";
                txtHoraInicio.Text = Retorno;
            }
            if (hora.Length == 3)
            {
                Retorno = hora + "00";
                txtHoraInicio.Text = Retorno;
            }
            if (hora.Length == 4)
            {
                Retorno = hora + "0";
                txtHoraInicio.Text = Retorno;
            }

            return Retorno;
        }

        #endregion

        protected void lnkCanal_Click(object sender, EventArgs e)
        {

        }

        protected void lnkEnviadasCanal_Click(object sender, EventArgs e)
        {

        }

        protected void lnkRecebidasCanal_Click(object sender, EventArgs e)
        {

        }
    }
}