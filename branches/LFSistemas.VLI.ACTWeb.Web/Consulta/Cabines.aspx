<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Cabines.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Cabines" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cabines</title>
     <style>
         

            
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <td style="width: 25%" rowspan="2">
            <label for="perfil">Cabines:</label>
                 <asp:CheckBoxList runat="server" ID="cblCabines" CssClass="form-control" SelectionMode="multiple" Width="250" Height="200" OnSelectedIndexChanged="cblCabines_SelectedIndexChanged" AutoPostBack="true" >
                            </asp:CheckBoxList>
            <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
        </td>
      
    </div>
    </form>
</body>
</html>
