<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Importa_OBC.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Importa_OBC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Importa OBC" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <div class="row">
        <div class="well">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Planilha:&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:FileUpload ID="fupPlanilha" runat="server" AllowMultiple="false" accept="application/vnd.ms-excel" BorderStyle="None" CssClass="form-control" />

                    </td>

                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:LinkButton ID="lnkImportar" runat="server" CssClass="btn btn-info" OnClick="lnkImportar_Click"><i class="fa fa-table"></i>&nbsp;Importar</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="well">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Total Lidos:&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:Label ID="lblTotalLidos" runat="server" /></td>
                </tr>
                <tr>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Total Importado:&nbsp;&nbsp;</td>
                    <td style="width: 90%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                        <asp:Label ID="lblTotalImportados" runat="server" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
