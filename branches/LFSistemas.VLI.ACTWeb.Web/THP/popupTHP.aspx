<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTHP.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.THP.popupTHP" %>

<!DOCTYPE html>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll; background-color: none !important; 
    scrollbar-face-color: rgb(000, 000, 000) !important;
    scrollbar-shadow-color: rgb(000, 000, 000) !important;
    scrollbar-3dlight-color: rgb(000, 000, 000) !important;
    scrollbar-arrow-color: rgb(000, 000, 000) !important;
    scrollbar-base-color: rgb(000, 000, 000) !important;
    scrollbar-track-color: rgb(000, 000, 000) !important;
    scrollbar-darkshadow-color: rgb(000, 000, 000) !important;
    scrollbar-highlight-color: rgb(000, 000, 000) !important;
    scrollbar-shadow-color: rgb(000, 000, 000) !important; ">
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
    <style>

        .tfonte {
            font-size: 16pt;
        }

        .status-branco {    
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(000, 000, 000);   /* Preto */
        }
        .status-branco:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status2-branco {
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(040, 040, 040);   /* Preto */
        }
        .status2-branco:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status-azul {
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(0, 0, 255);       /* Azul */
        }
        .status-azul:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status2-azul {
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(0, 0, 200);       /* Azul */
        }
        .status2-azul:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status-amarelo {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 000);   /* Amarelo */
        }
        .status-amarelo:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status2-amarelo {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(200, 200, 000);   /* Amarelo */
        }
        .status2-amarelo:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status-vermelho {
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(255, 000, 000);   /* Vermelho */
        }
        .status-vermelho:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }

        .status2-vermelho {
            color: rgb(255, 255, 255);              /* Branco */
            background-color: rgb(200, 000, 000);   /* Vermelho */
        }
        .status2-vermelho:hover {
            color: rgb(000, 000, 000);              /* Preto */
            background-color: rgb(255, 255, 255);   /* Branco */
        }


        .pisca-azul {
            background-image: url(../../img/azul.gif) !important;
            background-size: 30px;
            /*background-repeat: no-repeat;*/
        }
        .pisca-amarelo {
            background-image: url(../../img/amarelo.gif) !important;
            background-size: 30px;
            /*background-repeat: no-repeat;*/
        }

        .pisca-vermelho {
            background-image: url(../../img/vermelho.gif) !important;
            background-size: 30px;
            /*background-repeat: no-repeat;*/
        }

        #relogio {
            color: rgb(000, 100, 000);
            font-size: 3em;
            font-weight: 900;
        }
    </style>
    <script>
        function relogio() {
            var data = new Date();
            var horas = data.getHours();
            var minutos = data.getMinutes();
            var segundos = data.getSeconds();

            if (horas < 10) horas = "0" + horas;
            if (minutos < 10) minutos = "0" + minutos;
            if (segundos < 10) segundos = "0" + segundos;

            document.getElementById("relogio").innerHTML = horas + ":" + minutos + ":" + segundos;
        }

        window.setInterval("relogio()", 1000);
    </script>
</head>
    
<body style="background: rgb(000, 000, 000);" >
    <form id="form1" runat="server">
        <asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
        <div class="well-lg" style="background-color: rgb(000, 000, 000); padding-left: 15px; padding-right: 15px;">
            <div class="content-form">
                <table class="nav-justified">
                    <tr>
                        <td style="width: 50%; text-align: left;">
                            <div class="alert alert-success">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 50%">
                                            <h2>
                                                <asp:Label ID="Label1" runat="server" Text="Monitoramento de THP" Font-Size="50px" Style="color: rgb(000, 100, 000);" /></h2>
                                        </td>
                                        <td style="width: 50%; text-align: right;">
                                            <div id="relogio"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td style="width: 05%; text-align: left; font-size: 12px;"></td>
                        <td style="width: 10%; text-align: left; font-size: 12px;">
                            <img src="../../img/vermelho.gif" alt="Smiley face" height="30" />
                            <asp:Label ID="Label2" runat="server" Text="Maior que 3 hs." Font-Size="30px" Style="color: rgb(255, 255, 255);" />
                        </td>
                        <td style="width: 10%; text-align: left; font-size: 12px;">
                            <img src="../../img/amarelo.gif" alt="Smiley face" height="30" />
                            <asp:Label ID="Label3" runat="server" Text="Maior que 1.5 hs." Font-Size="30px" Style="color: rgb(255, 255, 255);" />
                        </td>
                        <td style="width: 10%; text-align: left; font-size: 12px;">
                            <img src="../../img/azul.gif" alt="Smiley face" height="30" />
                            <asp:Label ID="Label4" runat="server" Text="Maior que 30 Minutos." Font-Size="30px" Style="color: rgb(255, 255, 255);" />
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
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 07%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkCodigo_OS" Text="Código OS" /></th>
                                                                <th style="width: 05%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkPrefixo" Text="Prefixo" /></th>
                                                                <th style="width: 05%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkLocal" Text="Local" /></th>
                                                                <th style="width: 05%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkTempo" Text="Tempo" /></th>
                                                                <th style="width: 05%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkTempoTotal" Text="Tempo Total" /></th>
                                                                <th style="width: 05%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkCodigo" Text="Cod." /></th>
                                                                <th style="width: 33%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkMotivo" Text="Motivo" /></th>
                                                                <th style="width: 10%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkCorredor" Text="Corredor" /></th>
                                                                <th style="width: 10%; text-align: center; font-size: 19pt; border: 0px; border-right: 0.2px solid rgb(000, 072, 089);"><asp:LinkButton runat="server" ID="lnkGrupo" Text="Grupo" /></th>
                                                                <th style="width: 03%; text-align: center; font-size: 19pt; border: 0px;"></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr class="status-<%# Eval("Cor") %>">
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Codigo_OS") %>"><%# Eval("Codigo_OS")%> </td>
                                                        <td style="width: 07%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Prefixo") %>"><%# Eval("Prefixo")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Local") %>"><%# Eval("Local")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Tempo") %>"><%# Eval("Tempo")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("TempoTotal") %>"><%# Eval("TempoTotal")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Codigo") %>"><%# Eval("Codigo")%> </td>
                                                        <td style="width: 33%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Motivo")%>"><%# Eval("Motivo")%> </td>
                                                        <td style="width: 10%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%> </td>
                                                        <td style="width: 10%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Grupo") %>"><%# Eval("Grupo")%> </td>
                                                        <td class="pisca-<%# Eval("Cor") %>" style="width: 03%; text-align: left; font-size: 17pt; border: 0px;"></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="status2-<%# Eval("Cor") %>">
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Codigo_OS") %>"><%# Eval("Codigo_OS")%> </td>
                                                        <td style="width: 07%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Prefixo") %>"><%# Eval("Prefixo")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Local") %>"><%# Eval("Local")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Tempo") %>"><%# Eval("Tempo")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("TempoTotal") %>"><%# Eval("TempoTotal")%> </td>
                                                        <td style="width: 05%; text-align: center; font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Codigo") %>"><%# Eval("Codigo")%> </td>
                                                        <td style="width: 33%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Motivo")%>"><%# Eval("Motivo")%> </td>
                                                        <td style="width: 10%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%> </td>
                                                        <td style="width: 10%; text-align: left;   font-size: 17pt; border: 0px; border-right: 0.2px solid rgb(160, 160, 160);" title="<%# Eval("Grupo") %>"><%# Eval("Grupo")%> </td>
                                                        <td class="pisca-<%# Eval("Cor") %>" style="width: 03%; text-align: left; font-size: 17pt; border: 0px;"></td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                            </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            <hr style="color: rgb(255, 255, 255); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="20" Style="color: rgb(255, 255, 255);" />
                                            <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="20" Style="color: rgb(255, 255, 255);" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left;">
                                            <br />

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
            <div class="footer-lf-popup" style="background-color: rgb(000, 000, 000);">
                <span>desenvolvido por </span>
                <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </div>
    </form>
</body>
</html>
