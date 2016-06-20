<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Desenvolvimento.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Desenvolvimento" %>


    <asp:Content ID="Content3" ContentPlaceHolderID="ContentPageTitle" runat="server">
        <table class="nav-justified">
        <tr>
            <td style="width: 70%; text-align: left;">
                <asp:Label ID="Label1" runat="server" Text="Página em Desenvolvimento" CssClass="label" Font-Size="20px" ForeColor="Blue" />
            </td>
            <td style="width: 30%; text-align: right;">
                <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" ForeColor="Gray" />&nbsp;
                <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" ForeColor="Gray" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ContentMain" runat="server">

    <center>
    <a href="/Default.aspx" >     
        
        <h3>Clique na imagem para voltar</h3><br />
        <img src="/Imagens/vli.jpg" alt="Locomotiva VLI" width="50%">   
        

    </a>
</center>
</asp:Content>