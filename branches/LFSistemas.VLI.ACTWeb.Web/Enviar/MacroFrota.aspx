<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="MacroFrota.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Enviar.MacroFrota" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/macro-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Macro pra Frota" Font-Size="20px" Style="color: rgb(0, 100, 0);" CssClass="menu-item-icon menu-icon-radio" /></h2>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <script type="text/javascript">
        function ContChar(campo, maximo) {
            var x = parseInt(campo.value.length + 1);
            if (x >= maximo) {
                alert("Ultrapassou a quantidade máxima de: " + maximo + " caracteres!");
                event.keyCode = 0;
                campo.value = campo.value.substring(0, maximo);
                //event.returnValue = false;
                return
            } else {
                if ((event.keyCode >= 32 && event.keyCode <= 64) || (event.keyCode >= 97 && event.keyCode <= 122) || (event.keyCode >= 65 && event.keyCode <= 90) || (event.keyCode == 231) || (event.keyCode == 199)) {
                    if ((event.keyCode >= 97) && (event.keyCode <= 122)) {
                        return event.keyCode -= 32;
                    } else return
                }
            }
        }

        $(function () {
            $("#<%=txtMensagem.ClientID%>").keyup(function () {
                var tamanho = $("#<%=txtMensagem.ClientID%>").val().length;
                $("#<%=lblQtdeCaracteres.ClientID%>").html(tamanho);
            });
        });
    </script>
    <table style="width: 100%;">
        <tr>
            <td style="width: 100%; text-align: center;" colspan="2">
                <asp:Label runat="server" Text="ABREVIE A MENSAGEM" Font-Size="15" ForeColor="Black" /><br />
                <asp:Label runat="server" Text="Evite o uso de cumprimentos e nomes.<br>Cada caracter digitado aumenta o preço da mensagem, máximo de " Font-Size="10" ForeColor="Red" />
                <asp:Label runat="server" Text="100" Font-Size="10" ForeColor="Blue" />
                <asp:Label runat="server" Text=" caracteres." Font-Size="10" ForeColor="red" />
            </td>
        </tr>
        <tr>
            <td style="width: 50%; text-align: left;">
                <label for="motivo">Mensagem:</label>
            </td>
            <td style="width: 50%; text-align: right;">
                <label for="matricula">Trens circulando:&nbsp;&nbsp;</label>
                <asp:Label runat="server" ID="lblTrensCirculando" Text="0"  ForeColor="Red"/>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtMensagem" runat="server" TextMode="MultiLine" CssClass="form-control" Height="160" MaxLength="190" onkeypress='ContChar(this, 100)' onkeyup='ContChar(this, 100)' Width="100%" />
            </td>
        </tr>
    </table>
    <table style="width: 100%;">
        <tr>
            <td>
                <label for="matricula">Caracteres:&nbsp;&nbsp;</label>
                <asp:Label runat="server" ID="lblQtdeCaracteres" Text="0" />
            </td>
            <td style="width: 50%; text-align: right;">
                <br />
                <asp:LinkButton runat="server" ID="lnkEnviar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkEnviar_Click" ToolTip="Envia a mensagem para os trens que estão circulando no momento"><i class="fa fa-envelope"></i>&nbsp;Enviar</asp:LinkButton>
            </td>
        </tr>
    </table>
</asp:Content>
