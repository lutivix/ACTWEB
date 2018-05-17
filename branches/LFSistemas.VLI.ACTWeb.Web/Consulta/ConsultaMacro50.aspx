<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultaMacro50.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultaMacro501" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>Consulta de Macro 50</title>
    <%--<meta http-equiv="refresh" content="20" />--%>

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>

    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>

    <link rel="grupo vli" href="logo-vli.ico">

    <script type="text/javascript">
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
            if (window.event.keyCode == 40) {
                var myHeight = document.getElementById('<%=pnlGrid.ClientID %>');

                var altura = myHeight.clientHeight;
                if (altura <= 850)
                    document.getElementById('<%=pnlGrid.ClientID %>').style.height = altura + 20 + 'px';
            }
            if (window.event.keyCode == 38) {
                var myHeight = document.getElementById('<%=pnlGrid.ClientID %>');

                var altura = myHeight.clientHeight;
                if (altura >= 300)
                    document.getElementById('<%=pnlGrid.ClientID %>').style.height = altura - 20 + 'px';
            }
            if (window.event.keyCode == 39) {
                var mywidth = document.getElementById('<%=pnlGrid.ClientID %>');

                var largura = mywidth.clientWidth;
                if (largura <= 1800)
                    document.getElementById('<%=pnlGrid.ClientID %>').style.width = largura + 40 + 'px';
            }
            if (window.event.keyCode == 37) {
                var mywidth = document.getElementById('<%=pnlGrid.ClientID %>');

                var largura = mywidth.clientWidth;
                if (largura >= 800)
                    document.getElementById('<%=pnlGrid.ClientID %>').style.width = largura - 20 + 'px';
            }
        }
    </script>
</head>
<body onkeydown="tecla()">

    <form id="form1" runat="server">
        <asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>


        <script>
            $(function () {
                $("#<%= txtDataInicial.ClientID %>").datepicker({
                    buttonText: "Data inicial",
                    showOn: "button",
                    showButtonPanel: true,
                    changeMonth: true,
                    changeYear: true,
                    buttonImage: "../img/calendario.gif",
                    buttonImageOnly: true,

                    closeText: 'Fechar',
                    prevText: '&#x3C;Anterior',
                    nextText: 'Próximo&#x3E;',
                    currentText: 'Hoje',
                    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                    dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                    dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                    weekHeader: 'Sm',
                    dateFormat: 'dd/mm/yy',
                    firstDay: 0,
                    isRTL: false,
                    showMonthAfterYear: false,
                    yearSuffix: ''
                });
            });
        </script>
        <style>
            .tipo-E {
                color: green;
                background-color: white;
            }

            .tipo-R {
                color: black;
                background-color: white;
            }

            .macro-19 {
                color: black;
                background-color: red;
            }

            .leitura-T {
                background-color: white;
            }

            .leitura-F {
                background-color: yellow;
            }

            .leitura-R {
                background-color: lightblue;
            }

            .cabeca {
                color: blue;
                font-family: 'Arial Rounded MT';
                font-size: 8px;
                font-weight: bold;
            }

            .cabeca {
                vertical-align: text-top;
                margin: 10px 0px 10px 0px;
            }

            .linha {
                vertical-align: text-top;
            }

            .Processando {
                width: 100%;
                height: 100%;
                top: 0;
                left: 0;
                position: absolute;
                background-color: whitesmoke;
                filter: alpha(opacity=80);
                opacity: 0.8;
            }

            .Texto_Processando {
                position: absolute;
                top: 50%;
                left: 50%;
                margin-top: -50px;
                margin-left: -50px;
            }
        </style>

        <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table class="nav-justified">
                <tr>
                    <td style="width: 79%; text-align: left;">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="MCT: 7000" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                        </div>
                    </td>
                    <td style="width: 1%; text-align: left;"></td>
                    <td style="width: 20%; text-align: center;">
                        <div class="alert alert-info">
                            <h2>
                                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />,&nbsp;
                                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />,&nbsp;
                                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" />&nbsp;
                                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" Style="color: rgb(0, 72, 89);" Visible="false" />
                            </h2>
                        </div>
                    </td>
                </tr>
            </table>
            <table class="nav-justified">
                <tr style="display: block;">
                    <td>
                        <asp:LinkButton runat="server" ID="lnkMacro50" OnClick="lnkMacro50_Click" class="btn btn-success" Width="400px">
                            <span class="menu-item-label">Enviar Macro 50</span>
                        </asp:LinkButton>
                    </td>
                    <td style="padding-left: 20px;">
                        <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                    </td>
                </tr>
                <tr style="display: block;">
                    <td colspan="1">
                        <label for="data_fim">Corredor:</label>
                        <br />
                        <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="120">
                            <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="Baixada" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="Centro Leste" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="Centro Norte" />
                            <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="Centro Sudeste" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="Minas Bahia" />
                            <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="Minas Rio" />
                        </asp:CheckBoxList>
                    </td>
                    <td colspan="1">
                        <label for="matricula">Trem:</label>
                        <asp:TextBox runat="server" ID="txtFiltroTrem" CssClass="form-control" Width="130px" ToolTip="Separe os trens desejados com vírgulas. Ex.: C001, M642, E050" />
                        <br />
                        <label for="matricula">Data Inicial:</label>
                        <asp:TextBox ID="txtDataInicial" runat="server" Width="130px" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                    </td>
                    <td style="padding-top: 10px;">
                        <label for="matricula">Loco:</label>
                        <asp:TextBox runat="server" ID="txtFiltroLoco" CssClass="form-control" Width="130px" ToolTip="Separe as locomotivas desejadas com vírgulas. Ex.: 2904, 0962, 7007" />
                        <br />
                        <label for="hora_inicio">Hora:</label>
                        <asp:TextBox ID="txtHoraInicio" runat="server" Width="130px" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                    </td>
                    <td colspan="1">
                        <label for="matricula">Código OS:</label>
                        <asp:TextBox runat="server" ID="txtFiltroCodOS" CssClass="form-control" Width="130px" ToolTip="Separe as locomotivas desejadas com vírgulas. Ex.: 2904, 0962, 7007" />
                        <br />
                        <label for="data_fim">Mais Hora(s):</label>
                        <br />
                        <asp:DropDownList runat="server" Width="130px" Height="33px" ID="ddlMais" CssClass="form-control">
                            <asp:ListItem Text="01" Value="1" />
                            <asp:ListItem Text="02" Value="2" />
                            <asp:ListItem Text="03" Value="3" />
                            <asp:ListItem Text="04" Value="4" />
                            <asp:ListItem Text="05" Value="5" />
                            <asp:ListItem Text="06" Value="6" />
                            <asp:ListItem Text="07" Value="7" />
                            <asp:ListItem Text="08" Value="8" />
                            <asp:ListItem Text="09" Value="9" />
                            <asp:ListItem Text="10" Value="10" />
                            <asp:ListItem Text="11" Value="11" />
                            <asp:ListItem Text="12" Value="12" />
                            <asp:ListItem Text="13" Value="13" />
                            <asp:ListItem Text="14" Value="14" />
                            <asp:ListItem Text="15" Value="15" />
                            <asp:ListItem Text="16" Value="16" />
                            <asp:ListItem Text="17" Value="17" />
                            <asp:ListItem Text="18" Value="18" />
                            <asp:ListItem Text="19" Value="19" />
                            <asp:ListItem Text="20" Value="20" />
                            <asp:ListItem Text="21" Value="21" />
                            <asp:ListItem Text="22" Value="22" />
                            <asp:ListItem Text="23" Value="23" />
                            <asp:ListItem Text="24" Value="24" />
                        </asp:DropDownList>
                    </td>
                    <td colspan="1">
                        <label for="matricula">Prefixo Trem:</label>
                        <asp:TextBox runat="server" ID="txtFiltroPrefTrem" CssClass="form-control" Width="130px" ToolTip="Separe as locomotivas desejadas com vírgulas. Ex.: 2904, 0962, 7007" />
                        <br />
                        <label for="data_fim">Direção:</label>
                        <br />
                        <asp:RadioButton ID="rdParaFrente" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para frente" />
                        <br />
                        <asp:RadioButton ID="rdTras" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Trás" Checked="true" />
                    </td>
                    <td style="width: 10%;">&nbsp;
                            
                    </td>
                    <td style="width: 15%"></td>
                    <td style="width: 10%;"></td>
                    <td style="width: 10%;"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
            <table class="nav-justified">
                <tr>
                    <td colspan="3" style="width: 100%;">
                        <div>
                            <div class="row">
                                <asp:Panel ID="pnlGrid" runat="server" Visible="true" Enabled="true">
                                    <asp:Repeater ID="RepeaterMacro50" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkRE" runat="server" OnClick="lnkRE_Click">R/E</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkLoco" runat="server" OnClick="lnkLoco_Click">Loco</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkTrem" runat="server" OnClick="lnkTrem_Click">Trem</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkCodOS" runat="server" OnClick="lnkCodOS_Click">Cod. OS</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkHorario" runat="server" OnClick="lnkHorario_Click">Horário</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkMacro" runat="server" OnClick="lnkMacro_Click">Macro</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                            <asp:LinkButton ID="lnkTexto" runat="server" OnClick="lnkTexto_Click">Texto</asp:LinkButton></th>
                                                        <th style="text-align: center; font-size: 12pt;">
                                                            <asp:LinkButton ID="lnkCorredor" runat="server" OnClick="lnkCorredor_Click">Corredor</asp:LinkButton></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; cursor: pointer" class="macro-<%# Eval ("NumeroMacro")%> tipo-<%# Eval("Tipo")%> leitura-<%# Eval("Leitura") %>" onclick="javascript:window.open('/Consulta/popupMacro50.aspx?identificador_lda=<%# Eval("ID") %>&tipo=<%# Eval("Tipo")%>&macro=<%# Eval("NumeroMacro")%>&texto=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(Eval("Texto").ToString(), "a#3G6**@")  %>&locomotiva=<%# Eval("Locomotiva") %>&mct=<%# Eval("MCT") %>&codigoos=<%# Eval("CodigoOS") %>&horario=<%# Eval("Horario") %>&trem=<%# Eval("Trem") %>&identificador_tag_lda=<%# Eval("Leitura_ID") %>&tag_leitura=<%# Eval("Leitura") %>&lu=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(lblUsuarioLogado.Text.ToLower(), "a#3G6**@") %>&mu=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(lblUsuarioMatricula.Text.ToLower(), "a#3G6**@") %>&pu=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(lblUsuarioPerfil.Text.ToLower(), "a#3G6**@") %>&mm=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(lblUsuarioMaleta.Text.ToLower(), "a#3G6**@") %>', '<%# Eval("NumeroMacro")%>', 'width=850, height=650, resizable=not top=00 scrollbars=yes')">
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%> </td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva")%>"><%# Eval("Locomotiva")%> </td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%> </td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("CodigoOS")%>"><%# Eval ("CodigoOS")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Horario")%>"><%# Eval ("Horario")%></td>
                                                <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("NumeroMacro")%>"><%# Eval ("NumeroMacro")%></td>
                                                <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);">
                                                    <div style="width: 600px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                                                </td>
                                                <td style="text-align: center;" title="<%# Eval ("Corredor")%>"><%# Eval ("Corredor")%></td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="padding-top: 10px;">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkPrimeiraPagina" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                    &nbsp; 
                                    <asp:LinkButton ID="lnkPaginaAnterior" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                    &nbsp; Itens por página: &nbsp;
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="30" Value="30" />
                                        <asp:ListItem Text="40" Value="40" />
                                        <asp:ListItem Text="50" Value="50" />
                                        <asp:ListItem Text="100" Value="100" Selected="True" />
                                        <asp:ListItem Text="200" Value="200" />
                                        <asp:ListItem Text="300" Value="300" />
                                        <asp:ListItem Text="400" Value="400" />
                                        <asp:ListItem Text="500" Value="500" />
                                        <asp:ListItem Text="1000" Value="1000" />
                                    </asp:DropDownList>
                                    &nbsp;
                                    <asp:LinkButton ID="lnkProximaPagina" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                    &nbsp; 
                                    <asp:LinkButton ID="lnkUltimaPagina" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                </td>

                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="14" style="text-align: left;">
                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
