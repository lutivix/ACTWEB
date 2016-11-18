<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTrensOnline.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.popupTrensOnline" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="GMaps" Namespace="Subgurim.Controles" TagPrefix="cc1" %>

<!DOCTYPE html>


<html lang="pt-br" style="width: 100%; height: 100%;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
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
        $(document).keydown(function (e) {
            if (window.event.keyCode == 120) {
                document.getElementById('link1').click();
            }
            if (window.event.keyCode == 13) {
                document.getElementById('<%=lnkPesquisar.ClientID %>').click;
            }
        });
    </script>
    <style>
        .ativo-S {
            color: rgb(0, 72, 89);
        }

        .ativo-N {
            color: rgb(204, 102, 51);
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
</head>
<body style="margin: 1%;">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <asp:UpdatePanel runat="server" ID="upRegistros">
            <ContentTemplate>
                <div>
                    <table class="nav-justified">
                        <tr>
                            <td style="width: 79%; text-align: left;">
                                <div class="alert alert-success">
                                    <h2>
                                        <asp:Image runat="server" ImageUrl="/img/apoio-b.png" />
                                        <asp:Label ID="Label1" runat="server" Text="Consulta ultima posição do trem" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
                <div class="well">
                    <div class="page-header sub-content-header">
                        <%--<h2>Filtros de Pesquisa</h2>--%>
                        <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                    </div>
                    <div id="filtros" >
                        <table style="width: 100%; margin: 10px;">
                            <tr>
                                <td style="width: 20%; padding-top: 10px; vertical-align: bottom;">
                                    <label for="matricula">Trem:</label>
                                    <asp:TextBox runat="server" ID="txtFiltroTrem" CssClass="form-control" Width="98%" />
                                </td>
                                <td style="width: 80%; padding-top: 10px; vertical-align: bottom;">
                                    <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="125"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="125"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>&nbsp;&nbsp;
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <table>
                    <tr>
                        <td style="text-align: left;">
                            <br />
                            <asp:Label runat="server" Text="Trens: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                            <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                        </td>
                    </tr>
                </table>
                <div class="well">
                    <cc1:GMap ID="GMap1" runat="server" Width="100%" Height="680px" mapType="Normal"></cc1:GMap>
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
    </form>
</body>
</html>
