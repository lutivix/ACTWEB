using LFSistemas.VLI.ACTWeb.Controllers;
using LFSistemas.VLI.ACTWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace LFSistemas.VLI.ACTWeb.Web.Consulta
{
    public partial class ConsultaMacro501 : System.Web.UI.Page
    {
        #region [ PROPRIEDADES ]

        public List<Entities.Macro> itens { get; set; }
        public string corredores { get; set; }

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

            //!chamar nova página com filtros de cabines
            if (!Page.IsPostBack)
            {
              //  var a = Request.Querystring("cabines");
                string cabines = (Request.QueryString["cabines"]);
                var usuarioLogado = Uteis.Descriptografar(Request.QueryString["lu"].ToString(), "a#3G6**@").ToUpper();

                lblUsuarioLogado.Text = usuarioLogado.Length > 12 ? usuarioLogado.Substring(0, 12).ToUpper() : usuarioLogado;
                lblUsuarioMatricula.Text = Uteis.Descriptografar(Request.QueryString["mu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioPerfil.Text = Uteis.Descriptografar(Request.QueryString["pu"].ToString(), "a#3G6**@").ToUpper();
                lblUsuarioMaleta.Text = Uteis.Descriptografar(Request.QueryString["mm"].ToString(), "a#3G6**@").ToUpper();

                ViewState["ordenacao"] = "ASC";
                ViewState["corredor"] = "";

                var dataInicial = DateTime.Now;
               

                txtDataInicial.Text = dataInicial.ToShortDateString();
                txtHoraInicio.Text = dataInicial.ToShortTimeString();

                var dataFinal = txtHoraInicio;

                VerificaNovasMensagensComHoras();
            }
        }

        #endregion

        #region [ MÉTODOS DE CLICK DOS BOTÕES ]
        protected void lnkMacro50_Click(object sender, EventArgs e)
        {
            
            Response.Write("<script>window.open('/Consulta/EnviarMacro50.aspx?lu=" + Uteis.Criptografar(lblUsuarioLogado.Text.ToLower(), "a#3G6**@") + "&mu=" + Uteis.Criptografar(lblUsuarioMatricula.Text.ToLower(), "a#3G6**@") + "&pu=" + Uteis.Criptografar(lblUsuarioPerfil.Text.ToLower(), "a#3G6**@") + "&mm=" + Uteis.Criptografar(lblUsuarioMaleta.Text.ToLower(), "a#3G6**@") + "', '', 'width=680, height=570, scrollbars=yes, resusable=yes, status=no, toolbar=no, location=no, durectirues=no, top=0, left=0'); </script>");
            
        }
        protected void lnkRE_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TIPO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkLoco_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("LOCO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTrem_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TREM " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkCodOS_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("COD_OS " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkHorario_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("HORARIO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkMacro_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("MACRO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkTexto_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("TEXTO " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }
        protected void lnkCorredor_Click(object sender, EventArgs e)
        {
            var ordenacao = ViewState["ordenacao"].ToString();

            if (ordenacao == "ASC")
            {
                ViewState["ordenacao"] = "DESC";
                Pesquisar("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
            else
            {
                ViewState["ordenacao"] = "ASC";
                Pesquisar("CORREDOR " + ViewState["ordenacao"].ToString(), Navigation.None);
            }
        }

        protected void lnkPaginaAnterior_Click(object sender, EventArgs e)
        {
            //Fill repeater for Previous event
            Pesquisar(null, Navigation.Anterior);
        }
        protected void lnkProximaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Next event
            Pesquisar(null, Navigation.Proxima);
        }
        protected void lnkPrimeiraPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for First event
            Pesquisar(null, Navigation.Primeira);
        }
        protected void lnkUltimaPagina_Click(object sender, EventArgs e)
        {
            //Fill repeater for Last event
            Pesquisar(null, Navigation.Ultima);
        }
        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            ViewState["SortExpression"] = e.CommandName;
            Pesquisar(null, Navigation.Anterior);
        }

        #endregion

        #region [ MÉTODOS DE ACESSO A DADOS ]

        protected void VerificaNovasMensagensComHoras()
        {
            if (lblUsuarioMaleta.Text == "7000")
            {
                //var aux = new List<string>();
                //if (clbCorredor.Items.Count > 0)
                //{
                //    for (int i = 0; i < clbCorredor.Items.Count; i++)
                //    {
                //        if (clbCorredor.Items[i].Selected)
                //        {
                //            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                //        }
                //    }
                //    if (aux.Count <= 0)
                //    {
                //        aux.Add("'Baixada'");
                //        aux.Add("'Centro Leste'");
                //        aux.Add("'Centro Norte'");
                //        aux.Add("'Centro Sudeste'");
                //        aux.Add("'Minas Bahia'");
                //        aux.Add("'Minas Rio'");
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }
                //    else
                //    {
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }

                //    corredores = string.Join(",", aux);

                //    ViewState["corredor"] = corredores;
                //}

                var macroController = new MacroController();

                var qtde = macroController.ObterQtdeMacrosNaoLidas(corredores);
            
                if (qtde > 0)
                {
                    PesquisarComHoras(null, Navigation.None);

                    if (Request.Browser.Browser == "Firefox")
                    {

                        Response.Write("<script>window.open('/popup_Mensagem.aspx?ico=" + Uteis.Criptografar("!", "a#3G6**@") + "&tit=" + Uteis.Criptografar("ATENÇÃO", "a#3G6**@") + "&men=" + Uteis.Criptografar("Existe(m) " + qtde.ToString() +
                                        "   mensagem(ns) não lida(s)!", "a#3G6**@") + "','','width=500, height=180, scrollbars=yes, resizable=no, status=no, toolbar=no, location=no, durectirues=no,, left='+(screen.availWidth/2-235.5)+',top='+(screen.availHeight/2-136.5)+'');</script>");
                        

                    }
                    else
                        Response.Write("<script>alert('Existe(m) " + qtde.ToString() + " mensagem(ns) não lida(s)!');</script>");
                }
                else
                    PesquisarComHoras(null, Navigation.None);
            }
        }

        protected void VerificaNovasMensagens()
        {
            //if (lblUsuarioMaleta.Text == "7000")
            {
                //var aux = new List<string>();
                //if (clbCorredor.Items.Count > 0)
                //{
                //    for (int i = 0; i < clbCorredor.Items.Count; i++)
                //    {
                //        if (clbCorredor.Items[i].Selected)
                //        {
                //            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                //        }
                //    }
                //    if (aux.Count <= 0)
                //    {
                //        aux.Add("'Baixada'");
                //        aux.Add("'Centro Leste'");
                //        aux.Add("'Centro Norte'");
                //        aux.Add("'Centro Sudeste'");
                //        aux.Add("'Minas Bahia'");
                //        aux.Add("'Minas Rio'");
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }
                //    else
                //    {
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }

                //    corredores = string.Join(",", aux);

                //    ViewState["corredor"] = corredores;
                //}

                var macroController = new MacroController();

                var qtde = macroController.ObterQtdeMacrosNaoLidas(corredores);

                if (qtde > 0)
                {
                    Pesquisar(null, Navigation.None);

                    if (Request.Browser.Browser == "Firefox")
                    {

                        Response.Write("<script>window.open('/popup_Mensagem.aspx?ico=" + Uteis.Criptografar("!", "a#3G6**@") + "&tit=" + Uteis.Criptografar("ATENÇÃO", "a#3G6**@") + "&men=" + Uteis.Criptografar("Existe(m) " + qtde.ToString() +
                                        "   mensagem(ns) não lida(s)!", "a#3G6**@") + "','','width=500, height=180, scrollbars=yes, resizable=no, status=no, toolbar=no, location=no, durectirues=no,, left='+(screen.availWidth/2-235.5)+',top='+(screen.availHeight/2-136.5)+'');</script>");


                    }
                    else
                        Response.Write("<script>alert('Existe(m) " + qtde.ToString() + " mensagem(ns) não lida(s)!');</script>");
                }
                else
                    Pesquisar(null, Navigation.None);
            }
        }

        protected void lnkPesquisar_Click(object sender, EventArgs e)
        {       
            PesquisarBotao(null, Navigation.Pager);

        }

        protected void Pesquisar(string ordenacao, Navigation navigation)
        {
            {
                DateTime horaFim = DateTime.Now;

                //var aux = new List<string>();
                //if (clbCorredor.Items.Count > 0)
                //{
                //    for (int i = 0; i < clbCorredor.Items.Count; i++)
                //    {
                //        if (clbCorredor.Items[i].Selected)
                //        {
                //            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
                //        }
                //    }
                //    if (aux.Count <= 0)
                //    {
                //        aux.Add("'Baixada'");
                //        aux.Add("'Centro Leste'");
                //        aux.Add("'Centro Norte'");
                //        aux.Add("'Centro Sudeste'");
                //        aux.Add("'Minas Bahia'");
                //        aux.Add("'Minas Rio'");
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }
                //    else
                //    {
                //        aux.Add("'-'");
                //        aux.Add("' '");
                //    }

                //    corredores = string.Join(",", aux);

                //    ViewState["corredor"] = corredores;
                //}

                var macroController = new MacroController();
                var itens = macroController.ObterMacros50(new Entities.FiltroMacro()
                {

                    NumeroLocomotiva = string.Empty,
                    NumeroTrem = string.Empty,
                    CodigoOS = string.Empty,
                    DataInicio = null,
                    DataFim = null,
                    NumeroMacro = "50",
                    Corredores = ViewState["corredor"].ToString()

                }, "tela_consulta");

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
                    objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

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
                            if (int.Parse(ddlPageSize.SelectedValue) >= objPds.PageCount)
                                NowViewing = objPds.PageCount - 1;
                            break;
                        case Navigation.Sorting:
                            break;
                        default:
                            NowViewing = 0;
                            break;
                    }
                    objPds.CurrentPageIndex = NowViewing;
                    lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                    lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
                    lnkProximaPagina.Enabled = !objPds.IsLastPage;
                    lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
                    lnkUltimaPagina.Enabled = !objPds.IsLastPage;

                    this.RepeaterMacro50.DataSource = objPds;
                    this.RepeaterMacro50.DataBind();
                }
                else
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

                lblTotal.Text = string.Format("{0:0,0}", itens.Count);
            }
        }

        protected void PesquisarBotao(string ordenacao, Navigation navigation)
        {
            //DateTime horaFim = DateTime.Now;
            DateTime horaInicio = txtDataInicial.Text.Length > 0 ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
            DateTime horaFim = DateTime.Now.AddHours(-6);
            string corredores = null;

            //if ((int.Parse(txtHoraInicio.Text.Substring(0, 2)) == 24))
            // txtHoraInicio.Text = "00:00";

            if (rdParaFrente.Checked)
            {
                horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
            }

            else if (rdTras.Checked)
            {
                horaFim = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
               
            }

            //var aux = new List<string>();
            //if (clbCorredor.Items.Count > 0)
            //{
            //    for (int i = 0; i < clbCorredor.Items.Count; i++)
            //    {
            //        if (clbCorredor.Items[i].Selected)
            //        {
            //            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
            //        }
            //    }
            //    if (aux.Count <= 0)
            //    {
            //        aux.Add("'Baixada'");
            //        aux.Add("'Centro Leste'");
            //        aux.Add("'Centro Norte'");
            //        aux.Add("'Centro Sudeste'");
            //        aux.Add("'Minas Bahia'");
            //        aux.Add("'Minas Rio'");
            //        aux.Add("'-'");
            //        aux.Add("' '");
            //    }
            //    else
            //    {
            //        aux.Add("'-'");
            //        aux.Add("' '");
            //    }

            //    corredores = string.Join(",", aux);

            //    ViewState["corredor"] = corredores;
            //}

            var macroController = new MacroController();

            String cabines = (Request.QueryString["cabines"]);

            var itens = macroController.ObterMacros50PorCabines(new Entities.FiltroMacro()
            {
                NumeroLocomotiva = txtFiltroLoco.Text.Length > 0 ? txtFiltroLoco.Text : null,
                NumeroTrem = string.Empty,
                CodigoOS = txtFiltroCodOS.Text.Length > 0 ? txtFiltroCodOS.Text : null,
                DataInicio = horaInicio,
                DataFim = horaFim,
                NumeroMacro = "50",
                PrefixoTrem = txtFiltroPrefTrem.Text.Length > 0 ? txtFiltroPrefTrem.Text : null,
                cabines = cabines,
                Expressao = txtExpressao.Text.Length > 0 ? txtExpressao.Text : null,
                
            }, "tela_consulta");

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
                objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

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
                        if (int.Parse(ddlPageSize.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkProximaPagina.Enabled = !objPds.IsLastPage;
                lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkUltimaPagina.Enabled = !objPds.IsLastPage;

                this.RepeaterMacro50.DataSource = objPds;
                this.RepeaterMacro50.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }

        protected void PesquisarComHoras(string ordenacao, Navigation navigation)
        {
            //DateTime horaFim = DateTime.Now;
            DateTime horaInicio = DateTime.Now;
            DateTime horaFim = DateTime.Now.AddHours(-6);
            string corredores = null;

            //if ((int.Parse(txtHoraInicio.Text.Substring(0, 2)) == 24))
            // txtHoraInicio.Text = "00:00";

            //if (rdParaFrente.Checked)
            //{
            //    horaFim = horaInicio.AddHours(int.Parse(ddlMais.SelectedValue));
            //}

            //else if (rdTras.Checked)
            //{
            //    horaInicio = horaInicio.AddHours(-int.Parse(ddlMais.SelectedValue));
            //    horaFim = txtDataInicial.Text.Length > 0 ? DateTime.Parse(txtDataInicial.Text + " " + FormataHora(txtHoraInicio.Text)) : DateTime.Now;
            //}

            //var aux = new List<string>();
            //if (clbCorredor.Items.Count > 0)
            //{
            //    for (int i = 0; i < clbCorredor.Items.Count; i++)
            //    {
            //        if (clbCorredor.Items[i].Selected)
            //        {
            //            aux.Add(string.Format("'{0}'", clbCorredor.Items[i].Value));
            //        }
            //    }
            //    if (aux.Count <= 0)
            //    {
            //        aux.Add("'Baixada'");
            //        aux.Add("'Centro Leste'");
            //        aux.Add("'Centro Norte'");
            //        aux.Add("'Centro Sudeste'");
            //        aux.Add("'Minas Bahia'");
            //        aux.Add("'Minas Rio'");
            //        aux.Add("'-'");
            //        aux.Add("' '");
            //    }
            //    else
            //    {
            //        aux.Add("'-'");
            //        aux.Add("' '");
            //    }

            //    corredores = string.Join(",", aux);

            //    ViewState["corredor"] = corredores;
            //}

            var macroController = new MacroController();

            String cabines = (Request.QueryString["cabines"]);

            var itens = macroController.ObterMacros50PorCabines(new Entities.FiltroMacro()
            {
                NumeroLocomotiva = txtFiltroLoco.Text.Length > 0 ? txtFiltroLoco.Text : null,
                NumeroTrem = string.Empty,
                CodigoOS = txtFiltroCodOS.Text.Length > 0 ? txtFiltroCodOS.Text : null,
                DataInicio = horaInicio,
                DataFim = horaFim,
                NumeroMacro = "50",
                PrefixoTrem = txtFiltroPrefTrem.Text.Length > 0 ? txtFiltroPrefTrem.Text : null,
                cabines = cabines,
                Expressao = txtExpressao.Text.Length > 0 ? txtExpressao.Text : null

            }, "tela_consulta");

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
                objPds.PageSize = int.Parse(ddlPageSize.SelectedValue);

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
                        if (int.Parse(ddlPageSize.SelectedValue) >= objPds.PageCount)
                            NowViewing = objPds.PageCount - 1;
                        break;
                    case Navigation.Sorting:
                        break;
                    default:
                        NowViewing = 0;
                        break;
                }
                objPds.CurrentPageIndex = NowViewing;
                lblCurrentPage.Text = "Página: " + (NowViewing + 1).ToString() + " de " + objPds.PageCount.ToString();
                lnkPaginaAnterior.Enabled = !objPds.IsFirstPage;
                lnkProximaPagina.Enabled = !objPds.IsLastPage;
                lnkPrimeiraPagina.Enabled = !objPds.IsFirstPage;
                lnkUltimaPagina.Enabled = !objPds.IsLastPage;

                this.RepeaterMacro50.DataSource = objPds;
                this.RepeaterMacro50.DataBind();
            }
            else
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Atenção!", " BootstrapDialog.show({ title: 'ATENÇÃO!', message: 'Registro não localizado.' });", true);

            lblTotal.Text = string.Format("{0:0,0}", itens.Count);
        }


        #endregion

        protected void Temporizador_Tick(object sender, EventArgs e)
        {
           VerificaNovasMensagens();
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
        
    }
}