<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="UsuariosAutorizados.aspx.cs"   
    Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.UsuariosAutorizados" %>

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
   
    <p id="LabelMensagem" runat="server" class="bg-success">Registro adicionado com sucesso!</p>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="nome">Nome:</label>
            <asp:TextBox runat="server" ID="txtNomeACT" CssClass="form-control" MaxLength="30" />
        </div>
        <div class="form-group col-sm-1">
            <label for="email">Ativo:</label><br />
            <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" Checked="true" Width="40px" />
        </div> 
    </div>
    <div class="row">
        <div class="form-group col-sm-6">
            <label for="matricula">Matrícula:</label>
            <asp:TextBox runat="server" ID="txtMatriculaACT" CssClass="form-control" MaxLength="10" OnTextChanged="txtMatriculaACT_TextChanged" AutoPostBack="true" />
        </div>
        <div class="form-group col-sm-6">
            <label for="cpf">CPF:</label>
            <asp:TextBox runat="server" ID="txtCPF" CssClass="form-control"  onkeypress="return PermiteSomenteNumeros(event);" MaxLength="11" OnTextChanged="txtCPF_TextChanged" AutoPostBack="true" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">

            <label for="gerencia">Gerência:</label>
            <asp:TextBox runat="server" ID="txtGerencia" CssClass="form-control" />


        </div>
        <div class="form-group col-sm-6">

            <label for="supervisao">Supervisão:</label>
            <asp:TextBox runat="server" ID="txtSupervisao" CssClass="form-control" />


        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">

            <label for="empresa">Empresa:</label>
            <asp:TextBox runat="server" ID="txtEmpresa" CssClass="form-control" />


        </div>
        <div class="form-group col-sm-1">
            <td style="width: 30%" rowspan="2">
                <label for="subtipos">Subtipos:</label><br />
                <asp:Panel runat="server" Width="80px" Height="147px" ScrollBars="Vertical" CssClass="form-control">
                    <asp:CheckBoxList runat="server" ID="cblSubtipos" OnSelectedIndexChanged="cblSubtipos_SelectedIndexChanged" AutoPostBack="true" />
                </asp:Panel>
            </td>
        </div>
        <!--<div class="form-group col-sm-1">
            <label for="permiteldl">Permite:</label><br />

            <asp:DropDownList ID="ddlPermissoes" runat="server" CssClass="form-control" Checked="false" Width="80px" height="30px" Visible="false">
                                        <asp:ListItem Text="Sim" Value="0" />
                                        <asp:ListItem Text="Não" Value="1" />
            </asp:DropDownList>    

        </div>-->
        <div class="form-group col-sm-2 ">
            <label for="corredores">Corredores:</label>
            <asp:Panel runat="server" Width="170px" Height="137px" ScrollBars="Vertical" CssClass="form-control">
                <asp:CheckBoxList runat="server" ID="cblCorredores" OnSelectedIndexChanged="cblCorredores_SelectedIndexChanged" AutoPostBack="true" />
            </asp:Panel>
        </div>

        <div class="form-group col-sm-2">
            <td style="width: 30%" rowspan="2">
                <label for="supervisoes">Supervisões:</label><br />
                <asp:Panel runat="server" Width="200px" Height="137px" ScrollBars="Vertical" CssClass="form-control">
                    <asp:CheckBoxList runat="server" ID="cblSupervisoes" OnSelectedIndexChanged="cblSupervisoes_SelectedIndexChanged" AutoPostBack="true" />
                </asp:Panel>
            </td>
        </div>
    </div>



    <div class="row" style="margin-left: 02px;">
        <asp:Button ID="ButtonSalvar" type="button" CssClass="btn btn-success" runat="server" Text="Salvar" OnClick="ButtonSalvar_Click" OnClientClick="javascript:return validaFormulario();" />
        <asp:Button ID="ButtonCancelar" type="button" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClick="ButtonCancelar_Click" />
        <asp:Button ID="btnExcluir" type="button" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="btnExcluir_Click" Visible="false" />
        <asp:Button ID="ButtonSalvarECriarOutro" type="button" CssClass="btn btn-success" runat="server" Text="Salvar e criar outro usuário" OnClick="ButtonSalvarECriarOutro_Click" OnClientClick="javascript:return validaFormulario();" />
    </div>
    <script type="text/javascript" src="/js/mascara.js"></script>

</asp:Content>