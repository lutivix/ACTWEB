<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_Radios.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_Radios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/radio-b.png" />
                        <asp:Label ID="lblTitulo" runat="server" Text="Rádio" Font-Size="20px" Style="color: rgb(0, 100, 0);" CssClass="menu-item-icon menu-icon-radio" /></h2>
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

            if (document.getElementById("<%=txtDadosLoco.ClientID%>").value == '') {
                msg += " a loco. \n";
                if (item.length > 0) item += ":<%=txtDadosLoco.ClientID%>"; else item += "<%=txtDadosLoco.ClientID%>";
                retorno = false;
            }

            if (document.getElementById("<%=ddlDadosTipoLoco.ClientID%>").value == 'Selecione!' || document.getElementById("<%=ddlDadosTipoLoco.ClientID%>").value == '0') {
                msg += " o tipo da loco. \n";
                if (item.length > 0) item += ":<%=ddlDadosTipoLoco.ClientID%>"; else item += "<%=ddlDadosTipoLoco.ClientID%>";
                retorno = false;
            }

            if (document.getElementById("<%=ddlDadosCorredor.ClientID%>").value == 'Selecione!' || document.getElementById("<%=ddlDadosCorredor.ClientID%>").value == '0') {
                msg += " o corredor. \n";
                if (item.length > 0) item += ":<%=ddlDadosCorredor.ClientID%>"; else item += "<%=ddlDadosCorredor.ClientID%>";
                retorno = false;
            }

            if (document.getElementById("<%=ddlDadosSituacao.ClientID%>").value == 'Selecione!' || document.getElementById("<%=ddlDadosSituacao.ClientID%>").value == '0') {
                msg += " a situação. \n";
                if (item.length > 0) item += ":<%=ddlDadosSituacao.ClientID%>"; else item += "<%=ddlDadosSituacao.ClientID%>";
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
    <table class="nav-justified">
        <tr>
            <td style="width: 15%; padding: 5px;">
                <asp:Label runat="server" ID="lblControleRadioID" Visible="false" />
                <label for="nome">&nbsp;Trem:</label>
                <asp:TextBox runat="server" ID="txtDadosTrem" CssClass="form-control" Width="98%" ToolTip="Informe o trem." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Loco:</label>
                <asp:TextBox runat="server" ID="txtDadosLoco" CssClass="form-control" Width="98%" MaxLength="4" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Informe a locomotiva." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Tipo:</label>
                <asp:DropDownList runat="server" ID="ddlDadosTipoLoco" CssClass="form-control" Width="98%" ToolTip="Selecione o corredor." />
            </td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Corredor:</label>
                <asp:DropDownList runat="server" ID="ddlDadosCorredor" CssClass="form-control" Width="98%" ToolTip="Selecione o corredor." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Situação:</label>
                <asp:DropDownList runat="server" ID="ddlDadosSituacao" CssClass="form-control" Width="98%" ToolTip="Selecione a situação." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Considera:</label><br />
                <asp:CheckBox runat="server" ID="chkDadosConsidera" CssClass="form-control" Text="Sim" Width="68" Checked="true" ToolTip="Marque se for considerar." />
            </td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Rádio ID:</label>
                <asp:TextBox runat="server" ID="txtDadosRadioID" CssClass="form-control" Width="98%" MaxLength="4" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Informe o ID do rádio com no máximo 4 digitos numéricos."/>
            </td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Modelo de Cima:</label>
                <asp:TextBox runat="server" ID="txtDadosModeloAC" CssClass="form-control" Width="98%" MaxLength="10" ToolTip="Informe o modelo do rádio de cima." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Serial de Cima:</label>
                <asp:TextBox runat="server" ID="txtDadosSerialAC" CssClass="form-control" Width="98%" MaxLength="16" ToolTip="Informe o serial do rádio de cima" />
            </td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Modelo de Baixo:</label>
                <asp:TextBox runat="server" ID="txtDadosModeloAB" CssClass="form-control" Width="98%" MaxLength="10" ToolTip="Informe o modelo do rádio de baixo." />
            </td>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Serial de Baixo:</label>
                <asp:TextBox runat="server" ID="txtDadosSerialAB" CssClass="form-control" Width="98%" MaxLength="16" ToolTip="Informe o serial do rádio de baixo." />
            </td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;">
                <label for="nome">&nbsp;Ativo:</label><br />
                <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" Text="Sim" Width="68" ToolTip="Marque se estiver ativo." />
            </td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 15%; padding: 5px;"></td>
            <td style="width: 55%; padding: 5px;"></td>
        </tr>
        <tr>
            <td style="width: 15%; padding: 5px;" colspan="3" >
                <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna à tela de consulta."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClick="lnkExcluir_Click" OnClientClick="Confirm()" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-default" ToolTip="Limpa dados do formulário."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
