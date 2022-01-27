<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2><asp:Label ID="lblTitulo" runat="server" Text="Consulta de Usuário" Font-Size="20px" style="color: rgb(0, 100, 0);" /></h2>
                </div>
            </td>
            <td style="width: 1%; text-align: left;"></td>
            <td style="width: 20%; text-align: center;">
                <div class="alert alert-info">
                    <h2>
                        <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                        <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                        <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />&nbsp;
                        <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" Visible="false" />
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
            if (document.getElementById("<%=txtNome.ClientID%>").value == '') {
                msg += " o nome do usuário. \n";
                item += "<%=txtNome.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtMatricula.ClientID%>").value == '') {
                msg += " a matrícula do usuário. \n";
                if (item.length > 0) item += ":<%=txtMatricula.ClientID%>"; else item += "<%=txtMatricula.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtSenha.ClientID%>").value == '') {
                msg += " a senha do usuário. \n";
                if (item.length > 0) item += ":<%=txtSenha.ClientID%>"; else item += "<%=txtSenha.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlPerfil.ClientID%>").value == 'Selecione' || document.getElementById("<%=ddlPerfil.ClientID%>").value == '') {
                msg += " o perfil do usuário. \n";
                if (item.length > 0) item += ":<%=ddlPerfil.ClientID%>"; else item += "<%=ddlPerfil.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtEmail.ClientID%>").value == '') {
                msg += " o e-mail do usuário. \n";
                if (item.length > 0) item += ":<%=txtEmail.ClientID%>"; else item += "<%=txtEmail.ClientID%>";
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
    </script>
    <p id="LabelMensagem" runat="server" class="bg-success">Registro adicionado com sucesso!</p>
    <div class="row">
        <div class="form-group col-sm-12">
            <label for="nome">Nome:</label>
            <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" MaxLength="70" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="matricula">Matrícula:</label>
            <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" MaxLength="30" />
        </div>
        <div class="form-group col-sm-6">
            <label for="senha">Senha:</label>
            <asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" TextMode="Password" MaxLength="70" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="nivel" id ="labelPerfil">Perfil:</label>
            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-control" DataTextField="Nome" DataValueField="Id" />
        </div>
        <div class="form-group col-sm-6">
            <label for="maleta"id="labelMaleta">Maleta:</label>
            <asp:TextBox runat="server" ID="txtMaleta" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);" MaxLength="5" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-12">
            <label for="email" id="labelEmail">E-mail:</label>
            <asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" onblur="validateEmail(this);" MaxLength="70" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-1">
            <label for="email" id="labelAtivo">Ativo:</label><br />
            <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" Checked="true" Width="40" />
        </div>
    </div>
    <div class="row">
        <asp:Button ID="ButtonSalvar" type="button" CssClass="btn btn-success" runat="server" Text="Salvar" OnClick="ButtonSalvar_Click" OnClientClick="javascript:return validaFormulario();" />
        <asp:Button ID="ButtonCancelar" type="button" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClick="ButtonCancelar_Click" />
        <asp:Button ID="btnExcluir" type="button" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="btnExcluir_Click" Visible="false" />
    </div>
    <script type="text/javascript" src="/js/mascara.js"></script>

</asp:Content>
