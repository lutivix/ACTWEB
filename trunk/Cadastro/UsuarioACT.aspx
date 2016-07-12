<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="UsuarioACT.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.UsuarioACT" %>

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
            if (document.getElementById("<%=txtNomeACT.ClientID%>").value == ''){
                msg += " o nome do usuário. \n";
                item += "<%=txtNomeACT.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtMatriculaACT.ClientID%>").value == ''){
                msg += " a matrícula do usuário. \n";
                if (item.length > 0) item += ":<%=txtMatriculaACT.ClientID%>"; else item += "<%=txtMatriculaACT.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtSenhaACT.ClientID%>").value == ''){
                msg += " a senha do usuário. \n";
                if (item.length > 0) item += ":<%=txtSenhaACT.ClientID%>"; else item += "<%=txtSenhaACT.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlPerfil.ClientID%>").value == 'Selecione!' || document.getElementById("<%=ddlPerfil.ClientID%>").value == ''){
                msg += " o tipo do operador. \n";
                if (item.length > 0) item += ":<%=ddlPerfil.ClientID%>"; else item += "<%=ddlPerfil.ClientID%>";
                retorno = false;
            }

            if (retorno == false){
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
            <asp:TextBox runat="server" ID="txtNomeACT" CssClass="form-control" MaxLength="30" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="matricula">Matrícula:</label>
            <asp:TextBox runat="server" ID="txtMatriculaACT" CssClass="form-control" MaxLength="10" OnTextChanged="txtMatriculaACT_TextChanged" AutoPostBack="true" />
        </div>
        <div class="form-group col-sm-6">
            <label for="senha">Senha:</label>
            <asp:TextBox runat="server" ID="txtSenhaACT" CssClass="form-control" TextMode="Password" MaxLength="8" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="cpf">CPF:</label>
            <asp:TextBox runat="server" ID="txtCPFACT" CssClass="form-control" OnTextChanged="txtCPFACT_TextChanged" onkeypress="return PermiteSomenteNumeros(event);" MaxLength="11" AutoPostBack="true" />
        </div>
        <div class="form-group col-sm-1">
            <label for="permiteldl">Permite LDL:</label><br />
            <asp:CheckBox runat="server" ID="chkPermiteLDLACT" CssClass="form-control" Checked="true" Width="120px" />
        </div>            
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="nivel">Perfil:</label>
            <asp:DropDownList ID="ddlPerfil" runat="server" CssClass="form-control" DataTextField="Nome" DataValueField="Id" />
        </div>
    </div>
    <div class="row" style="margin-left: 02px;">
        <asp:Button ID="ButtonSalvar" type="button" CssClass="btn btn-success" runat="server" Text="Salvar" OnClick="ButtonSalvar_Click" OnClientClick="javascript:return validaFormulario();" />
        <asp:Button ID="ButtonCancelar" type="button" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClick="ButtonCancelar_Click" />
        <asp:Button ID="btnExcluir" type="button" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="btnExcluir_Click" Visible="false" />
    </div>
    <script type="text/javascript" src="/js/mascara.js"></script>

</asp:Content>
