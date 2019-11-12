<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupRelatorioRestricoesPorData.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.popupRelatorioRestricoesPorData" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Relatório Restrições por Data</title>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <%--<link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />--%>
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>

    <link rel="grupo vli" href="logo-vli.ico">

    <script type="text/javascript">
        $(function () {
            $("#<%= txtFiltroDataInicial.ClientID %>").datepicker({
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
            $("#<%= txtFiltroDataFinal.ClientID %>").datepicker({
                buttonText: "Data final",
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
            $('#<%= txtFiltroHoraInicial.ClientID %>').timepicker({
                buttonText: "Hora inicial",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar'
            });
            $('#<%= txtFiltroHoraFinal.ClientID %>').timepicker({
                buttonText: "Hora final",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar'
            });
        });

        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });
    </script>
    <style>
        .Processando {
            position: absolute;
            height: 270px;
            width: 314px;
            top: 290px;
            z-index: 1;
            padding-left: 452px;
            padding-right: 13px;
            left: -389px;
            margin-left: 40%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel runat="server" >
            <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 79%;">
                            <div class="alert alert-success">
                                <h2>
                                    <asp:Label ID="lblTitulo" runat="server" Text="Relatório de Restrições de Circulação por Data" Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
            </div>
            <div class="row" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
                    <div class="well">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%;">
                                    <label for="grupo">Restrições de:</label>
                                    <asp:TextBox runat="server" ID="txtFiltroDataInicial" Width="95%" CssClass="form-control" onKeyUp="formatar(this, '##/##/####')" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Clique no calendário abaixo para inserir a data inicial." />
                                </td>
                                <td style="width: 10%;">
                                    <label for="grupo">hs</label>
                                    <asp:TextBox runat="server" ID="txtFiltroHoraInicial" Width="95%" CssClass="form-control" onKeyUp="formatar(this, '##:##')" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" ToolTip="Clique no relógio abaixo para inserir a hora inicial." />
                                </td>
                                <td style="width: 20%;">
                                    <label for="grupo">até:</label>
                                    <asp:TextBox runat="server" ID="txtFiltroDataFinal" Width="95%" CssClass="form-control" onKeyUp="formatar(this, '##/##/####')" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Clique no calendário abaixo para inserir a data final." />

                                </td>
                                <td style="width: 10%;">
                                    <label for="grupo">hs</label>
                                    <asp:TextBox runat="server" ID="txtFiltroHoraFinal" Width="95%" CssClass="form-control" onKeyUp="formatar(this, '##:##')" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" ToolTip="Clique no relógio abaixo para inserir a hora final." />
                                </td>
                                <td style="width: 40%;"></td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <br />
                                    <asp:Button ID="btnGerarRelatorio" runat="server" CssClass="btn btn-success" OnClick="btnGerarRelatorio_Click" Text="Gerar Relatório" />
                                    &nbsp;&nbsp;
                                            <asp:Button ID="btnAtualiza" runat="server" class="btn btn-default" OnClick="btnAtualiza_Click" Text="Atualiza" />
                                    &nbsp;&nbsp;
                                            <asp:Button ID="btnLimpar" runat="server" class="btn btn-info" OnClick="btnLimpar_Click" Text="Limpar" />
                                </td>
                            </tr>
                        </table>
                    </div>
            </div>
            <div class="row" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
                <asp:Panel runat="server" ID="pnlRelatorio" Visible="false" Width="100%">
                    <div class="form-group ">
                        <asp:Button ID="bntGerarExcel" CssClass="btn btn-default" runat="server" Text="Gerar Excel" OnClick="bntGerarExcel_Click" />
                    </div>
                    <div class="form-group col-xs-12 table-responsive">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" >
                                        <asp:Repeater ID="rptListaRestricoesVigentes" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-hover table-curved pro-table " id="macros">
                                                    <thead>
                                                        <tr style="vertical-align: bottom;">
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Tipo</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Elemento</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Data Inicial</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Data Final</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Velocidade</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Km Inicial</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Km Final</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; "><a href="#">Observação</a></th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr style="font-size: 09px;" class="situacao-<%# Eval("Situacao")%>">
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo_Restricao")%>"><%# Eval("Tipo_Restricao")%> </td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Secao_Elemento")%>"><%# Eval("Secao_Elemento")%> </td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Data_Inicial")%>"><%# Eval ("Data_Inicial")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Data_Final")%>"><%# Eval ("Data_Final")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Velocidade")%>"><%# Eval ("Velocidade")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Km_Inicial")%>"><%# Eval ("Km_Inicial")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Km_Final")%>"><%# Eval ("Km_Final")%></td>
                                                    <td style="text-align: left; " title="><%# Eval ("Observacao")%>"><%# Eval ("Observacao")%></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                    </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </div>
            <div style="float: right; margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
                <span>desenvolvido por </span>
                <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
