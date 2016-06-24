<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTermometros.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.popupTermometros" %>

<% @ Register assembly = "AjaxControlToolkit" namespace = "AjaxControlToolkit" tagprefix = "asp"%>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />
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
    <style>
        .critico-S {
            color: rgb(204, 102, 51);
        }

        .critico-N {
            color: rgb(000, 000, 000);
        }
        .status-NORMAL { 
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status-FALHA { 
            color: rgb(000, 000, 000);             /* Preto */
            background-color: rgb(192, 192, 192);  /* Cinza */
        }

        .status-RESTRIÇÃO { 
            color: rgb(000, 000, 000);             /* Preto */
            background-color: rgb(255, 255, 000);  /* Amarelo */
        }

        .status-RONDA { 
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(255 , 000, 000);  /* Vermelho */
        }

        .status-INTERDIÇÃO { 
            color: rgb(255, 255, 255);             /* Branco */
            background-color: rgb(138, 043, 226);  /* Violeta */
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager  ID ="ToolkitScriptManager1"  runat ="server" />
        <div class="well-lg" style="background-color: rgb(255, 255, 255); padding-left: 15px; padding-right: 15px;">
            <div class="content-form">
                <table class="nav-justified">
                    <tr>
                        <td style="width: 100%; text-align: left;">
                            <div class="alert alert-success">
                                <h2>
                                    <asp:Label ID="Label1" runat="server" Text="Monitoramento de Temperatura - Termômetros" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel runat="server" ID="upRegistros">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterItens" runat="server">
                                                <HeaderTemplate>
                                                    <table >
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 100px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkID" OnClick="lnkCorredor_Click" Text="ID" ForeColor="White" /></th>
                                                                <th style="width: 100px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkCorredor" OnClick="lnkCorredor_Click" Text="Corredor" ForeColor="White" /></th>
                                                                <th style="width: 100px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro" OnClick="lnkTermometro_Click" Text="Termômetro" ForeColor="White"/></th>
                                                                <th style="width: 200px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTrecho" OnClick="lnkTrecho_Click" Text="Trecho" ForeColor="White"/></th>
                                                                <th style="width: 120px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura" OnClick="lnkTemperatura_Click" Text="Temperatura" ForeColor="White"/></th>
                                                                <th style="width: 150px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkUltimaLeitura" OnClick="lnkUltimaLeitura_Click" Text="Ultima Leitura" ForeColor="White"/></th>
                                                                <th style="width: 100px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkStatus" OnClick="lnkStatus_Click" Text="Status" ForeColor="White"/></th>
                                                                <th style="width: 100px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkCritico" OnClick="lnkCritico_Click" Text="Critico" ForeColor="White"/></th>
                                                                <th style="width: 400px; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkOcorrencia" OnClick="lnkOcorrencia_Click" Text="Ocorrência" ForeColor="White"/></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table >
                                                            <tr class="status-<%# Eval("Status").ToString() %>">
                                                                <td style="width: 100px; height: 20px; text-align: right; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Termometro_ID") %></td>
                                                                <td style="width: 100px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Corredor") %></td>
                                                                <td style="width: 100px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                                <td style="width: 200px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Trecho") %></td>
                                                                <td style="width: 120px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                                <td style="width: 150px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                                <td style="width: 100px; height: 20px; text-align: left; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Status").ToString().ToUpper() %></td>
                                                                <td class="critico-<%#Eval("Critico").ToString().Substring(0,1) %>" style="width: 100px; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Critico") %></td>
                                                                <td style="width: 400px; height: 20px; text-align: left; padding: 0px 5px 0px 5px;"><%# Eval("Ocorrencia") %></td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                            </asp:Repeater>
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <br />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRegistros">
                    <ProgressTemplate>
                        <div class="Processando">
                            <table class="Texto_Processando">
                                <tr>
                                    <td>
                                        <asp:Image runat="server" ID="imgProcess" ImageUrl="~/img/process.gif" Width="50" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lblProcess" Text="Processando..." />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div class="footer-lf-popup">
                <span>desenvolvido por </span>
                <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </div>
    </form>
</body>
</html>
