<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Displays.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Displays" %>

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
            if (document.getElementById("<%=txtMensagem.ClientID%>").value == '') {
                msg += " a Mensagem. \n";
                item += "<%=txtMensagem.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtData.ClientID%>").value == '') {
                msg += " a Data. \n";
                if (item.length > 0) item += ":<%=txtData.ClientID%>"; else item += "<%=txtData.ClientID%>";
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

        $(function () {
            $("#<%= txtData.ClientID %>").datepicker({
                showOn: "button",
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                buttonImage: "../img/calendario.gif",
                buttonImageOnly: true,

                closeText: 'Fechar',
                prevText: '&#x3C;Anterior',
                nextText: 'Próximo&#x3E;',
                currentText: 'Hoje',
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 0,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            });
        });

    </script>
    <div class="row">
        <div class="well">
            <table style="width: 100%">
                <tr>
                    <td style="width: 90%">
                        <label for="nome">Mensagem:</label>
                        <asp:TextBox runat="server" ID="txtMensagem" CssClass="form-control" Width="98%" />
                    </td>
                    <td style="width: 90%">
                        <label for="nome">Data para Ativação:</label>
                        <asp:TextBox runat="server" ID="txtData" CssClass="form-control" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 3%">
                            <label for="nome">Ativo:</label>
                            <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label runat="server" ID="lblDisplayID" Visible="false" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                            <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkCancelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="Confirm()" OnClick="lnkExcluir_Click" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
