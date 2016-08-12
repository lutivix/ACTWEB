<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_MetaTempo.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_MetaTempo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <%--<asp:Image runat="server" ImageUrl="/img/radio-b.png" />--%>
                        <asp:Label ID="lblTitulo" runat="server" Text="Meta Tempo" Font-Size="20px" Style="color: rgb(0, 100, 0);" CssClass="menu-item-icon menu-icon-radio" /></h2>
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
        function validaFormulario() {
            var retorno = true;
            var msg = "Gentileza informar os dados abaixo: \n";
            var item = [];

            if (document.getElementById("<%=txtDadosHoras.ClientID%>").value == '') {
                msg += " Horas. \n";
                if (item.length > 0) item += ":<%=txtDadosHoras.ClientID%>"; else item += "<%=txtDadosHoras.ClientID%>";
                retorno = false;
            }

            if (document.getElementById("<%=txtDadosHoras.ClientID%>").value == '') {
                msg += " Horas. \n";
                if (item.length > 0) item += ":<%=txtDadosHoras.ClientID%>"; else item += "<%=txtDadosHoras.ClientID%>";
                retorno = false;
            }


            if (retorno == false) {
                BootstrapDialog.show({ title: 'ATENÇÃO!', message: msg });
                var ind = item.split(":");
                if (ind.length > 0)
                    document.getElementById(ind[0]).focus();
            }

            return retorno;
        }

        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja realmente excluir o restrições selecionado?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }

    </script>
    <style>
        .uppercase {
            text-transform: uppercase;
        }
    </style>
    <div class="well well-sm">
        <table style="width: 100%;">
            <tr>
                <td style="width: 10%;">
                    <label for="hora_inicio">Localidade:</label>
                    <asp:TextBox ID="TextBox1" Text="ZBL" runat="server" Width="90%" CssClass="form-control" MaxLength="3" onkeypress="return PermiteSomenteNumeros(event);" />
                </td>
                <td style="width: 10%;">
                    <label for="hora_inicio">Tempo (min):</label>
                    <asp:TextBox ID="txtDadosHoras" Text="0" runat="server" Width="90%" CssClass="form-control" MaxLength="3" onkeypress="return PermiteSomenteNumeros(event);" />
                </td>
                <td style="width: 62%;" />
            </tr>
            <tr>
                <td colspan="3">
                    <br />
                </td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="lnkSalvar">
                        <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" ToolTip="Grava os dados informados no banco."  OnClientClick="javascript:return validaFormulario();"><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" ToolTip="Cancela a operação e retorna para a lista de abreviaturas." ><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" ToolTip="Apaga o registro do banco."  OnClientClick="Confirm()"><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" ToolTip="Limpa dados do formulário." ><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

