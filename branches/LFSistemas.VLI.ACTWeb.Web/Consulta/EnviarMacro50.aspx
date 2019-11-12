<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnviarMacro50.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.EnviarMacro501" %>

<% Session.Timeout = 60; %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head id="Head1" runat="server">
    <title>ACTWEB - Envio de Macro 50</title>

    <link href="../css/jquery.dataTables.css" rel="stylesheet" />
    <link href="../css/jquery.dataTables.min.css" rel="stylesheet" />
    <link href="../css/jquery.dataTables_themeroller.css" rel="stylesheet" />
    <link href="../css/main.css" rel="stylesheet" />
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
        <style>
            .label {
                color: blue;
                font-family: 'Arial Rounded MT';
                font-size: 20px;
                font-weight: bold;
            }
        </style>
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
        <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 69%;">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                                <asp:Label ID="Label2" runat="server" Text="Envio de Macro 50 " Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
                    <td style="width: 50%; text-align: left;">
                        <label for="data_fim">Trens:</label>
                        <br />
                        <asp:TextBox runat="server" ID="txtIdentificador_lda" Visible="false" />
                        <asp:TextBox runat="server" ID="txtIdentificador_tag_lda" Visible="false" />
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
                    <td colspan="2" style="width: 95%; text-align: center;">
                        <div style="width: 620px; text-align: center;">
                            <asp:Label ID="Label1" runat="server" Text="ATENÇÃO. ABREVIE A MENSAGEM" CssClass="label01" /><br />
                            <asp:Label ID="Label3" runat="server" Text="Evite o uso de cumprimentos e nomes<br>Cada caracter digitado aumenta o preço da mensagem" Font-Size="10" ForeColor="Red" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 95%; text-align: left;">
                        <label for="motivo">Mensagem:</label>
                        <br />
                        <asp:TextBox ID="txtMensagem" runat="server" TextMode="MultiLine" CssClass="form-control" Height="200" MaxLength="494" onkeypress='ContChar(this, 494)' Width="100%" onkeydown = "return (event.keyCode!=13);" />
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
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 100%; text-align: center;">
                        <asp:Label ID="Label4" runat="server" Text="Sua mensagem não pode ser abreviada? Consulte aqui o <a class='link' href='#' onclick='abrirhelp()'>Dicionário Ferroviário</a>" />
                    </td>
                </tr>
            </table>
            <div style="float: right;">
                <span>desenvolvido por </span>
                <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo"></a>
            </div>
        </div>
    </form>
</body>
</html>
