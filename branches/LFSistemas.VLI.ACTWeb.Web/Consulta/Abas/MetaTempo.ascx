<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MetaTempo.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Abas.MetaTempo" %>

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
                    <label>Corredor:</label>
                    <asp:TextBox runat="server" ID="txtDadosCorredor" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 35%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Localidade:</label>
                    <asp:TextBox runat="server" ID="txtDadosLocal" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Tempo (min):</label>
                    <asp:TextBox runat="server" ID="txtDadosTempo" CssClass="form-control" Enabled="true" onkeypress="return PermiteSomenteNumeros(event);"/>
                </td>

                <td style="width: 25%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;"></td>
            </tr>
        </table>

        <table style="width: 100%;  padding-left: 15px;">
            <tr>
                <td style="width: 50%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: bottom;">
                    <div>
                        <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClientClick="javascript:return validaFormulario();" OnClick="lnkSalvar_Click" ><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkCalncelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCalncelar_Click" ToolTip="Cancela a operação."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                    </div>
                </td>
            </tr>
        </table>

    </div>
</div>
