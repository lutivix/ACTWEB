<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnviarMacro200.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.EnviarMacro2001" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ACTWEB - Envio de Macro 200</title>

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
        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }
    </script>
</head>
<body onkeydown="tecla()">
    <form id="form1" runat="server">
        <script>
            function abrirhelp() {
                window.open("../Ajuda/Help02.html", "Ajuda", "status=no, toolbar=no, scrollbars=yes, resizable=yes, location=no, width=800, height=600, menubar=no");
            }

            function ContChar(campo, maximo) {
                var x = parseInt(campo.value.length);
                if (x >= maximo) {
                    alert("Ultrapassou a quantidade máxima de: " + maximo + " caracteres!");
                    event.keyCode = 0;
                    event.returnValue = false;
                } else {
                    if ((event.keyCode >= 32 && event.keyCode <= 64) || (event.keyCode >= 97 && event.keyCode <= 122) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode == 231) || (event.keyCode == 199)) {
                        if ((event.keyCode >= 97) && (event.keyCode <= 122)) {
                            return event.keyCode -= 32;
                        } else return
                    } else {
                        alert("Caracter não permitido!");
                        event.keyCode = 0;
                    }
                }
            }
        </script>
        <div style="margin: 3%;">
            <table class="nav-justified">
                <tr>
                    <td style="width: 69%; text-align: left;">
                        <div class="alert alert-success">
                            <h2><asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="lblTitulo" runat="server" Text="Macro 200" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
                        </div>
                    </td>
                    <td style="width: 1%; text-align: left;"></td>
                    <td style="width: 30%; text-align: center;">
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
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; text-align: left;">
                        <label for="data_fim">Trens:</label>
                        <br />
                        <asp:DropDownList runat="server" ID="ddlTrens" CssClass="form-control" OnSelectedIndexChanged="ddlTrens_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione o Trem" Width="95%" />
                    </td>
                    <td style="width: 50%; text-align: left;">
                        <label for="data_fim">MCT:</label>
                        <br />
                        <asp:DropDownList runat="server" ID="ddlMcts" CssClass="form-control" OnSelectedIndexChanged="ddlMcts_SelectedIndexChanged" AutoPostBack="true" ToolTip="Selecione o Mcts" Width="95%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; text-align: right;">
                        <asp:RadioButton ID="rdHabilitaSV" runat="server" Font-Size="22px" GroupName="Macro200" Text="Hab. Supervisão de Velocidade&nbsp;&nbsp;" CssClass="checkbox-inline" />
                    </td>
                    <td style="width: 50%; text-align: left;">
                        <asp:RadioButton ID="rdDesabilitaSV" runat="server" Font-Size="22px" GroupName="Macro200" Text="Desab. Supervisão de Velocidade" CssClass="checkbox-inline" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 50%; text-align: right;">
                        <asp:RadioButton ID="rdHabilitaTP" runat="server" Font-Size="22px" GroupName="Macro200" Text="Hab. Temporização Permissiva&nbsp;&nbsp;" CssClass="checkbox-inline" />
                    </td>
                    <td style="width: 50%; text-align: left;">
                        <asp:RadioButton ID="rdDesabilitaTP" runat="server" Font-Size="22px" GroupName="Macro200" Text="Desab. Temporização Permissiva" CssClass="checkbox-inline" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <asp:Button ID="bntEnviar" CssClass="btn btn-success" runat="server" Text="Enviar" OnClick="bntEnviar_Click" Width="30%" />
                        &nbsp;&nbsp;
                        <asp:Button ID="btnLimpar" CssClass="btn btn-primary" runat="server" Text="Limpar" OnClick="btnLimpar_Click" Width="30%" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
