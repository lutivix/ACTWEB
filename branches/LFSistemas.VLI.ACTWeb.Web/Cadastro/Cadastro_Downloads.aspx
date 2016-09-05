<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_Downloads.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_Downloads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Downloads" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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

            if (document.getElementById("<%=txtDescricao.ClientID%>").value == '') {
                msg += " a Descrição do arquivo. \n";
                if (item.length > 0) item += ":<%=txtDescricao.ClientID%>"; else item += "<%=txtDescricao.ClientID%>";
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

        function myFunction() {

            var Arquivo = document.getElementById('<%=fupArquivo.ClientID %>');
            

            if (Arquivo.value != '') {
                document.getElementById('<%=txtArquivo.ClientID %>').value = Arquivo.value.replace(/^.*?([^\\\/]*)$/, '$1');;
                document.getElementById("<%=txtDescricao.ClientID%>").focus();
            }
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
    <table style="width: 100%">
        <tr>
            <td style="width: 100%; padding: 5px;" colspan="4">
                <label for="nome">&nbsp;Upload: - Lembrando que o arquivo não pode ultrapassar 50MB.</label>
                <asp:Label runat="server" ID="lblDownlodasID" Visible="false" />
                <asp:FileUpload ID="fupArquivo" runat="server" AllowMultiple="false" BorderStyle="None" CssClass="form-control" onchange="myFunction()" />
            </td>
        </tr>
        <tr>
            <td style="width: 20%; padding: 5px;">
                <label for="nome">&nbsp;Arquivo:</label>
                <asp:TextBox runat="server" ID="txtArquivo" Width="98%" CssClass="form-control" />
            </td>
            <td style="width: 40%; padding: 5px;">
                <label for="nome">&nbsp;Descrição:</label>
                <asp:TextBox runat="server" ID="txtDescricao" Width="98%" CssClass="form-control" />
            </td>
            <td style="width: 10%; padding: 5px;">
                <label for="nome">&nbsp;Versão:</label>
                <asp:TextBox runat="server" ID="txtVersao" Width="93%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" MaxLength="10" />
            </td>
            <td style="width: 20%; padding: 5px;">
                <label for="nome">&nbsp;Previsão de Atualização:</label>
                <asp:TextBox runat="server" ID="txtPrevisao" Width="98%" CssClass="form-control" />
            </td>
        </tr>
        <tr>
            <td style="padding: 5px;">
                <label for="nome">&nbsp;Download Liberado?:</label><br />
                <asp:CheckBox runat="server" ID="chkLiberadoDownload" Text="Sim" Checked="true" CssClass="form-control" Width="70px" />
            </td>
            <td style="padding: 5px;"></td>
            <td style="padding: 5px;" ></td>
            <td style="padding: 5px;" ></td>
        </tr>
        <tr>
            <td style="padding: 5px;">
                <label for="nome">&nbsp;Ativo?:</label><br />
                <asp:CheckBox runat="server" ID="chkAtivo" Text="Sim" Checked="true" CssClass="form-control" Width="70px" />
            </td>
            <td style="padding: 5px;"></td>
            <td style="padding: 5px;"></td>
            <td style="padding: 5px;"></td>
        </tr>
        <tr>
            <td colspan="4"><br /></td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="Confirm()" OnClick="lnkExcluir_Click" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa dados do formulário."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                &nbsp;&nbsp;

<%--                <asp:Button ID="ButtonSalvar" type="button" CssClass="btn btn-success" runat="server" Text="Salvar" OnClick="ButtonSalvar_Click" OnClientClick="javascript:return validaFormulario();" />
                <asp:Button ID="btnExcluir" type="button" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="btnExcluir_Click" OnClientClick="Confirm()" />
                <asp:Button ID="ButtonCancelar" type="button" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClick="ButtonCancelar_Click" />--%>
            </td>
        </tr>
    </table>
</asp:Content>
