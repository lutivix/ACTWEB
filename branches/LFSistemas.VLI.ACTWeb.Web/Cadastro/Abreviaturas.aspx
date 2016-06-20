<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Abreviaturas.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Abreviaturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
            var msg = "Informe";
            var item = [];

            if (document.getElementById("<%=txtDadosExtenso.ClientID%>").value == '') {
                msg += " o Extenso. \n";
                item += "<%=txtDadosExtenso.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosAbreviado.ClientID%>").value == '') {
                msg += " o Abreviado. \n";
                if (item.length > 0) item += ":<%=txtDadosAbreviado.ClientID%>"; else item += "<%=txtDadosAbreviado.ClientID%>";
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
    <div class="row">
        <div class="well">
            <table style="width: 100%">
                <tr>
                    <td> 
                        <div class="form-group col-sm-12">
                            <label for="nome">Extenso:</label>
                            <asp:TextBox runat="server" ID="txtDadosExtenso" CssClass="form-control" />
                        </div>
                    </td>
                    <td>
                        <div class="form-group col-sm-12">
                            <label for="nome">Abreviado:</label>
                            <asp:TextBox runat="server" ID="txtDadosAbreviado" CssClass="form-control" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group col-sm-1">
                            <label for="nome">Ativo:</label>
                            <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblAbreviaturaID" Visible="false" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="form-group col-sm-12">
                            <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="Confirm()" OnClick="lnkExcluir_Click" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa dados do formulário."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            &nbsp;&nbsp;
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
