<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupRelatorioRestricoesVigentes.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.popupRelatorioRestricoesVigentes" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Relatório Restrições Vigentes</title>

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
</head>
<body>
    <form id="form1" runat="server">
        <asp:Panel runat="server" >
            <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 79%;">
                            <div class="alert alert-success">
                                <h2>
                                    <asp:Label ID="lblTitulo" runat="server" Text="Restrições" Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
            <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
                <div class="row">
                    <div class="form-group col-md-12">
                        <asp:Button ID="bntGerarExcel" CssClass="btn btn-default" runat="server" Text="Gerar Excel" OnClick="bntGerarExcel_Click" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-xs-12 table-responsive">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server">
                                        <asp:Repeater ID="rptListaRestricoesVigentes" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-hover table-curved pro-table " id="macros">
                                                    <thead>
                                                        <tr style="vertical-align: bottom;">
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Tipo</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Elemento</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Data</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Velocidade</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Km Inicial</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Km Final</a></th>
                                                            <th style="background-color: white; text-align: center; font-size: 12pt;"><a href="#">Observação</a></th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr style="font-size: 9px;" class="situacao-<%# Eval("Situacao")%>">
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo_Restricao")%>"><%# Eval("Tipo_Restricao")%> </td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Secao_Elemento")%>"><%# Eval("Secao_Elemento")%> </td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Data_Inicial")%>"><%# Eval ("Data_Inicial")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Velocidade")%>"><%# Eval ("Velocidade")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Km_Inicial")%>"><%# Eval ("Km_Inicial")%></td>
                                                    <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="><%# Eval ("Km_Final")%>"><%# Eval ("Km_Final")%></td>
                                                    <td style="text-align: left;" title="><%# Eval ("Observacao")%>"><%# Eval ("Observacao")%></td>
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
                </div>
            </div>
            <div style="float: right; margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
                <span>desenvolvido por </span>
                <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </asp:Panel>
    </form>
</body>
</html>
