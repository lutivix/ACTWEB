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
            <label for="cpf">CPF:</label>
            <asp:TextBox runat="server" ID="txtCPF" CssClass="form-control" OnTextChanged="txtCPF_TextChanged" onkeypress="return PermiteSomenteNumeros(event);" MaxLength="11" AutoPostBack="true" />
        </div>
    </div>
    <div class="row">
        <div class="form-group col-sm-6">

            <label for="gerencia">Gerência:</label>
            <asp:TextBox runat="server" ID="txtGerencia" CssClass="form-control"/>

            
        </div>

        <div class="form-group col-sm-6">

            <label for="supervisao">Supervisão:</label>
            <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control"/>

            
        </div>

        <div class="form-group col-sm-6">

            <label for="empresa">Empresa:</label>
            <asp:TextBox runat="server" ID="TextBox2" CssClass="form-control"/>

            
        </div>

        <div class="form-group col-sm-1">
            <label for="permiteldl">Permite:</label><br />

            <asp:CheckBoxList ID="checkboxListPermissoes" runat="server" CssClass="form-control" Checked="false" Width="80px" height="50px">
                <asp:ListItem Text="LDL" Value="1"></asp:ListItem>
                <asp:ListItem Text="BS" Value="2"></asp:ListItem>
            </asp:CheckBoxList>    

        </div>   

        <div class="form-group col-sm-1">
               <td style="width: 30%" rowspan="2">
                            Subtipos<label for="perfil">:</label>
                            <asp:Panel runat="server" Width="80px" Height="127px" ScrollBars="Vertical" CssClass="form-control">
                                <asp:CheckBoxList runat="server" ID="cblSubtipos" />
                            </asp:Panel>
                        </td>   
        </div> 
        <div class="form-group col-sm-1">
                                <label for="matricula">Corredor:</label>
                                    <asp:CheckBoxList runat="server" ID="cblCorredor" CssClass="form-control" SelectionMode="multiple" Width="160" Height="127" AutoPostBack="true">
                                        <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="6" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="3" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="2" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="5" />
                                        <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="4" />
                                    </asp:CheckBoxList>
        </div> 
                
    </div>
    
    <div class="row" style="margin-left: 02px;">
        <asp:Button ID="ButtonSalvar" type="button" CssClass="btn btn-success" runat="server" Text="Salvar" OnClick="ButtonSalvar_Click" OnClientClick="javascript:return validaFormulario();" />
        <asp:Button ID="ButtonCancelar" type="button" CssClass="btn btn-primary" runat="server" Text="Cancelar" OnClick="ButtonCancelar_Click" />
        <asp:Button ID="btnExcluir" type="button" CssClass="btn btn-danger" runat="server" Text="Excluir" OnClick="btnExcluir_Click" Visible="false" />
    </div>
    <script type="text/javascript" src="/js/mascara.js"></script>

</asp:Content>