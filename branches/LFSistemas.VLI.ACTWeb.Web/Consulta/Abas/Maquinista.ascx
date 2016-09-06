<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Maquinista.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Abas.Maquinista" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script type="text/javascript">
    function tecla() {
        if (window.event.keyCode == 27) {
            this.window.close();
        }
    }

    function validaFormulario() {
        var retorno = true;
        var msg = "Informe";
        var item = [];
        var confirm_value = document.createElement("INPUT");
        confirm_value.type = "hidden";
        confirm_value.name = "confirm_value";


        if (retorno == false) {
            BootstrapDialog.show({ title: 'ATENÇÃO!', message: msg });
            var ind = item.split(":");
            if (ind.length > 0)
                document.getElementById(ind[0]).focus();
        }
        else {

            document.forms[0].appendChild(confirm_value);
        }
        return retorno;
    }

</script>
<style type="text/css">
    .search {
        background: url(/img/key-a.png) no-repeat;
        background-position-y: center;
        padding-left: 20px;
    }

        .search:hover {
            background: url(/img/key-b.png) no-repeat;
            background-position-y: center;
            padding-left: 20px;
        }

        .search:focus {
            background: url(/img/key-b.png) no-repeat;
            background-position-y: center;
            padding-left: 20px;
        }
</style>
<div class="well" aria-autocomplete="none">
    <div class="row">
        <table style="width: 100%; padding-left: 15px;">
            <tr>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Matricula:</label>
                    <asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" Enabled="true" />
                    <asp:TextBox runat="server" ID="txtid" CssClass="form-control" Visible="false" Enabled="false" />
                </td>
                <td style="width: 30%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Nome:</label>
                    <asp:TextBox runat="server" ID="txtNome" CssClass="form-control" Enabled="true" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Sede:</label>
                    <asp:TextBox runat="server" ID="txtSede" CssClass="form-control" Enabled="true" />
                </td>

                <td style="width: 40%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;"></td>
            </tr>
        </table>

        <table style="width: 100%;  padding-left: 15px;">
            <tr>
                <td style="width: 50%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: bottom;">
                    <div>
                        <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClientClick="javascript:return validaFormulario();" OnClick="lnkSalvar_Click" ><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" ToolTip="Apaga o registro do banco." OnClientClick="javascript:return validaFormulario();" OnClick="lnkExcluir_Click" ><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>

    </div>
</div>
