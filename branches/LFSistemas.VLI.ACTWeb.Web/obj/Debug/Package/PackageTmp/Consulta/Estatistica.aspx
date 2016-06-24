<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Estatistica.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Estatistica" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    ESTATÍSTICA DE ACESSO
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <div class="row">
        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
            <label>Total acessos </label>
            <asp:TextBox ID="TextBoxTotalAcessos" runat="server" CssClass="form-control"></asp:TextBox>
        </div>
        <br />
  </div>

    <br />
    <div class="row">
        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
            <label>Aplicação </label>

            <table border="1" style="width: 100%">
                <tr>
                    <td>ACTApoio</td>
                    <td>5133616</td>
                </tr>
                <tr>
                    <td>ACTPainel</td>
                    <td>1106698</td>
                </tr>
                <tr>
                    <td>Macro61</td>
                    <td>82482</td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
            <label>Nível </label>

            <table border="2" style="width: 100%">
                <tr>
                    <td>Painel</td>
                    <td>11714</td>
                </tr>
                <tr>
                    <td>Apoio</td>
                    <td>560248</td>
                </tr>
                <tr>
                    <td>Op VP</td>
                    <td>100314</td>
                </tr>
            </table>
        </div>
    </div>

    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" CellPadding="3">

        <Columns>
            <asp:BoundField DataField="Matricula" HeaderText="Usuário" ItemStyle-Width="10em">
                <ItemStyle Width="10em"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-Width="20em">
                <ItemStyle Width="20em"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField DataField="Acessos" HeaderText="Acessos" ItemStyle-Width="5em">
                <ItemStyle Width="5em"></ItemStyle>
            </asp:BoundField>
        </Columns>
        <FooterStyle BackColor="White" ForeColor="#000066" />
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
        <RowStyle ForeColor="#000066" />
        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
        <SortedAscendingCellStyle BackColor="#F1F1F1" />
        <SortedAscendingHeaderStyle BackColor="#007DBB" />
        <SortedDescendingCellStyle BackColor="#CAC9C9" />
        <SortedDescendingHeaderStyle BackColor="#00547E" />
    </asp:GridView>

    <style>
        table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
        }

        th, td {
            padding: 5px;
        }
    </style>

</asp:Content>
