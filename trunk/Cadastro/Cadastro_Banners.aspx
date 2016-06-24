<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_Banners.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_Banners" %>

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

            if (document.getElementById("<%=txtArquivo.ClientID%>").value == '') {
                msg += " o Arquivo. \n";
                if (item.length > 0) item += ":<%=txtDescricao.ClientID%>"; else item += "<%=txtDescricao.ClientID%>";
                retorno = false;
            }

            if (document.getElementById("<%=txtDescricao.ClientID%>").value == '') {
                msg += " a Descrição. \n";
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


        function previewFile() {
            var preview = document.querySelector('#<%=imgArquivo.ClientID %>');
            var file = document.querySelector('#<%=fupArquivo.ClientID %>').files[0];
           
            var reader = new FileReader();

            reader.onloadend = function (e) {

                var image = new Image();
                image.src = e.target.result;

                image.onload = function () {
                    var height = this.height;
                    var width = this.width;
                    if ((height > 250 || width > 1200) || (height < 200 || width < 960)) {
                        alert("A imegem inserida está com " + width +" X " + height + " pixels, ou seja fora do padrão de 1200 x 250 pixels. Gentileza tentar outra imagem.");

                        preview.src = "";
                        document.querySelector('#<%=fupArquivo.ClientID %>').value = null;
                        document.getElementById('<%=txtArquivo.ClientID %>').value = null;
                        return false;
                    }
                    else
                    {
                        preview.src = reader.result;

                        document.getElementById('<%=txtArquivo.ClientID %>').value = null;
                        document.getElementById('<%=txtArquivo.ClientID %>').value = file.name.replace(/^.*?([^\\\/]*)$/, '$1');
                        document.getElementById("<%=txtDescricao.ClientID%>").focus();
                        document.querySelector('#<%=fupArquivo.ClientID %>').value = file;
                    }
                    return true;
                };
            }

            if (file) {
                reader.readAsDataURL(file);
            } else {
                preview.src = "";
            }
        }

    </script>
    <table class="nav-justified">
        <tr>
            <td colspan="2" style="width: 60%; vertical-align: top;">
                <label for="nome">&nbsp;Upload:</label>
                <asp:Label runat="server" ID="lblBannerID" Visible="false" />

                <%--<asp:FileUpload runat="server" ID="fupArquivo"  />--%>
                <input runat="server" id="fupArquivo" type="file" name="file" onchange="previewFile()" class="form-control" style="width: 98%" />
            </td>
            <td rowspan="3" style="width: 40%; vertical-align: top;">
                <label for="nome">&nbsp;</label>
                <div class="alert alert-danger" style="text-align: center;">
                    <asp:Label ID="Label1" runat="server" Text="A imagem precisa ter 1200 X 250 pixels" />
                </div>

                <asp:Image runat="server" ID="imgArquivo" CssClass="form-control" Width="98%" Height="150" />
            </td>
        </tr>
        <tr>
            <td style="width: 15%; vertical-align: top;">
                <label for="nome">&nbsp;Arquivo:</label>
                <asp:TextBox runat="server" ID="txtArquivo" Width="97%" CssClass="form-control" />
            </td>
            <td style="width: 25%; vertical-align: top;">
                <label for="nome">&nbsp;Descrição:</label>
                <asp:TextBox runat="server" ID="txtDescricao" Width="95%" CssClass="form-control" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="nome">&nbsp;Ativo?:</label><br />
                <asp:CheckBox runat="server" ID="chkAtivo" Text="Sim" Checked="true" CssClass="form-control" Width="70px" />
            </td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="Confirm()" OnClick="lnkExcluir_Click" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" OnClick="lnkLimpar_Click" ToolTip="Limpa dados do formulário."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                &nbsp;&nbsp;
            </td>
            <td>&nbsp;</td>
        </tr>
    </table>
</asp:Content>
