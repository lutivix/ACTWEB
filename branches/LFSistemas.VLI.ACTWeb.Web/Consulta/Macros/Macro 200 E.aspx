<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Macro 200 E.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro_200_E" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 811px;
        }
        .auto-style5 {
            width: 234px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table class="auto-style1">
            <tr>
                <td class="auto-style5">
                    <fieldset style="width: 395px">
                        <legend>Informações de licenciamento</legend>
                        <asp:CheckBox ID="CheckBox18" runat="server" />
                        <asp:Label ID="Label18" runat="server" Text="O modo permissivo foi acionado"></asp:Label>
                        <br />
                        <asp:CheckBox ID="CheckBox19" runat="server" />
                        Substituição de buffer de troca<br />
                        <asp:CheckBox ID="CheckBox20" runat="server" />
                        Cancelamento total do buffer de troca<br />
                        <asp:CheckBox ID="CheckBox21" runat="server" />
                        Cancelamento parcial do buffer de troca</fieldset></td>
                <td>
                    <fieldset style="width: 431px">
                        <legend>Parâmetro de aproximação máxima</legend>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        metros<br />
                    </fieldset><br />
                </td>
            </tr>
            <tr>
                <td class="auto-style5">
                    <fieldset style="width: 395px">
                        <legend>Pontos de troca</legend>
                        <asp:GridView ID="GridView1" runat="server" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                        </asp:GridView>
                    </fieldset></td>
                <td>
                    <fieldset style="width: 432px">
                        <legend>Palavra de comando</legend>
                        <asp:CheckBox ID="CheckBox22" runat="server" />
&nbsp;Aplicação de freio<br />
                        <asp:CheckBox ID="CheckBox23" runat="server" />
&nbsp;Liberação de freio<br />
                        <asp:CheckBox ID="CheckBox24" runat="server" />
&nbsp;Desabilitação das funções de supervisão<br />
                        <asp:CheckBox ID="CheckBox25" runat="server" />
&nbsp;Habilitação das funções de supervisão<br />
                        <asp:CheckBox ID="CheckBox26" runat="server" />
&nbsp;Solicitação do número de pontos de troca</fieldset></td>
            </tr>
            <tr>
                <td class="auto-style5">&nbsp;</td>
                <td>
                    <fieldset style="width: 435px">
                        <legend>Configuração das mensagens de retorno</legend>
                        <asp:CheckBox ID="CheckBox27" runat="server" />
                        &nbsp;Código de inicialização<br />
                        <asp:CheckBox ID="CheckBox28" runat="server" />
                        &nbsp;Código de erro de inicialização<br />
                        <asp:CheckBox ID="CheckBox29" runat="server" />
                        &nbsp;Comutação para permissivo<br />
                        <asp:CheckBox ID="CheckBox30" runat="server" />
                        &nbsp;Ocorrência de descarrilhamento<br />
                        <asp:CheckBox ID="CheckBox31" runat="server" />
                        &nbsp;Aplicou freio por ultrapassagem de velocidade limite<br />
                        <asp:CheckBox ID="CheckBox32" runat="server" />
                        &nbsp;Aplicou freio por invasão</fieldset></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
