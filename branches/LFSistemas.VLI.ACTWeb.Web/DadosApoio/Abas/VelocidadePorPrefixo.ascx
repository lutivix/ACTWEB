<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VelocidadePorPrefixo.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.DadosApoio.Abas.VelocidadePorPrefixo" %>

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
        var teste = document.getElementById("<%=txtVelocidade.ClientID%>").value;
         
        if (document.getElementById("<%=lbSecao.ClientID%>")) {
            if (document.getElementById("<%=lbSecao.ClientID%>").value == 'Selecione!' || document.getElementById("<%=lbSecao.ClientID%>").value == '') {
                msg += " a SB. \n";
                if (item.length > 0) item += ":<%=lbSecao.ClientID%>"; else item += "<%=lbSecao.ClientID%>";
                retorno = false;
            }
        } 
       
       <%-- document.getElementById("<%=txtPrefixo.ClientID%>").value == '' || --%>
        if (document.getElementById("<%=txtPrefixo.ClientID%>").innerHTML.length < 4)
             {
                     msg += " o Prefixo corretamente. Ex: A001\n";
                     if (item.length > 0) item += ":<%=txtPrefixo.ClientID%>"; else item += "<%=txtPrefixo.ClientID%>";
                     retorno = false;
             }
        

        if (document.getElementById("<%=txtVelocidade.ClientID%>").value == '') 
        {
            msg += " a Velocidade. \n"; 
            if (item.length > 0) item += ":<%=txtVelocidade.ClientID%>"; else item += "<%=txtVelocidade.ClientID%>";
            retorno = false;
        }

        if (retorno == false)
        {
                BootstrapDialog.show({ title: 'ATENÇÃO!', message: msg });

                var ind = item.split(":");
                if (ind.length > 0)
                    document.getElementById(ind[0]).focus();
        }

            return retorno;
        }

        function ConfirmS() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja realmente enviar a velocidade para o prefixo na SB?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }
        function ConfirmE() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("Deseja realmente retirar a velocidade do prefixo na SB?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }
     

</script>
<style type="text/css">

</style>

<div class="well" aria-autocomplete="none">
    <div class="row">
        <table style="width: 100%; margin: 10px;">
            <tr>
                <td style="width: 20%; padding-top: 05px;" colspan="2">
                    <label for="Prefixo">SB:</label>
                    <%--<asp:DropDownList ID="ddlSecao" runat="server" CssClass="form-control" Width="98%" />--%> 
                    <asp:ListBox ID="lbSecao" runat="server" CssClass="form-control" Width="98%" SelectionMode="Multiple" Visible="true"></asp:ListBox> 
                    <asp:TextBox ID="txtSb"   runat="server" CssClass="form-control" Width="97%" />
                </td>
            </tr>
            <tr>
                <td style="width: 10%; padding-top: 05px;">
                    <label for="Prefixo">Prefixo:</label>
                    <asp:TextBox runat="server" ID="txtPrefixo" CssClass="form-control" Width="97%" style="text-transform:uppercase;"/>
                </td>
                <td style="width: 10%; padding-top: 05px;">
                    <label for="Velocidade">Velocidade:</label>
                    <asp:TextBox runat="server" ID="txtVelocidade" CssClass="form-control" Width="97%" />
                </td>
                <td style="width: 80%; padding-top: 05px;"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>
                <td style="width: 100%; padding-top: 10px;" colspan="4">
                    <div class="btn-group btn-group-lg hidden-xs">
                        <div class="btn-group btn-group-lg">
                            <asp:LinkButton runat="server" ID="lnkCancelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCancelar_OnClick"><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
                        </div>
                        <div class="btn-group btn-group-lg">
                            <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Excluir" OnClientClick="ConfirmE();" OnClick="lnkExcluir_OnClick"><i class="fa fa-eraser"></i>&nbsp;Excluir</asp:LinkButton>
                        </div>
                        <div class="btn-group btn-group-lg">
                            <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClientClick="javascript:return validaFormulario();" OnClick="lnkSalvar_OnClick"><i class="fa fa-plus"></i>&nbsp;Salvar</asp:LinkButton>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
