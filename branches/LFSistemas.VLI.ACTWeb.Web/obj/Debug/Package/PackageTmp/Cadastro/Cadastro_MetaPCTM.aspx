<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_MetaPCTM.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_MetaPCTM" %>

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
                        <asp:Label runat="server" ID="lblMetaPCTMID" Visible="false" />
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

            if (document.getElementById("<%=ddlDadosRotas.ClientID%>").value == 'Selecione uma Rota!' || document.getElementById("<%=ddlDadosRotas.ClientID%>").value == '') {
                msg += " a Rota. \n";
                if (item.length > 0) item += ":<%=ddlDadosRotas.ClientID%>"; else item += "<%=ddlDadosRotas.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosMeta.ClientID%>").value == '') {
                msg += " a Meta. \n";
                if (item.length > 0) item += ":<%=txtDadosMeta.ClientID%>"; else item += "<%=txtDadosMeta.ClientID%>";
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
    <table style="width: 100%">
        <tr>
            <td style="width: 50%; padding: 5px; " colspan="2" >
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 27%;">
                                    <label for="matricula">Rota:</label>
                                    <asp:DropDownList ID="ddlDadosRotas" runat="server" CssClass="form-control" Width="98%" OnSelectedIndexChanged="ddlDadosRotas_SelectedIndexChanged" AutoPostBack="true" ToolTip="Informe a Rota a ser cadastrada. " />
                                </td>
                                <td style="width: 20%; padding: 5px;">
                                    <label for="matricula">Corredor:</label>
                                    <asp:TextBox runat="server" ID="txtDadosCorredor" CssClass="form-control" Width="98%" Enabled="false" Font-Bold="true" Style="font-size: 1.7em; color: rgb(204, 102, 51);" />
                                </td>
                                <td style="width: 20%; padding: 5px;">
                                    <label for="matricula">Prefixo:</label>
                                    <asp:TextBox runat="server" ID="txtDadosPrefixo" CssClass="form-control" Width="98%" Enabled="false" Font-Bold="true" Style="font-size: 1.7em; color: rgb(204, 102, 51);" />
                                </td>
                                <td style="width: 33%; padding: 5px;">

                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
            <td style="width: 10%; padding: 5px; ">
                <label for="matricula">Validade:</label>
                <asp:TextBox runat="server" ID="txtDadosValidade" CssClass="form-control" Width="98%" Enabled="false" Font-Bold="true" Style="font-size: 1.7em; color: rgb(55, 119, 180);" />
            </td>
        </tr>
        <tr>
            <td style="width: 50%; padding: 5px; ">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 50%">
                            <label for="matricula">Meta:</label>
                            <asp:TextBox runat="server" ID="txtDadosMeta" CssClass="form-control" Width="97%" onkeypress="return PermiteSomenteNumeros(event);" MaxLength="3" />
                        </td>
                        <td style="width: 10%">
                            <label for="nome">&nbsp;Ativo?:</label><br />
                            <asp:CheckBox runat="server" ID="chkAtivo" Text="Sim" Checked="true" CssClass="form-control" Width="98%" />
                        </td>
                        <td style="width: 330%"></td>
                    </tr>
                </table>
            </td>
            <td style="width: 40%; padding: 5px; "></td>
            <td style="width: 10%; padding: 5px; "></td>
        </tr>
        <tr>
        </tr>
        <tr>
            <td style="padding: 5px;"></td>
            <td style="padding: 5px;"></td>
            <td style="padding: 5px;"></td>
        </tr>
        <tr>
            <td colspan="3">
                <br />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkCancelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                &nbsp;&nbsp;
                <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClick="lnkExcluir_Click" OnClientClick="Confirm()" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                &nbsp;&nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
