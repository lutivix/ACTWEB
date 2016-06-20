<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="THP.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Abas.THP" %>

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
        
        if (document.getElementById("<%=rdMotivo.ClientID%>").checked) {

            if ( document.getElementById("<%=ddlMotivoNovo.ClientID%>").value == '' || document.getElementById("<%=ddlMotivoNovo.ClientID%>").value == 'Selecione!') {
                msg += " o Motivo. \n";
                item += "<%=ddlMotivoNovo.ClientID%>";
                retorno = false;
            }
            if ( document.getElementById("<%=txtMotivoSenha.ClientID%>").value == '') {
                msg += " a Senha. \n";
                if (item.length > 0) item += ":<%=txtMotivoSenha.ClientID%>"; else item += "<%=txtMotivoSenha.ClientID%>";
                retorno = false;
            }
        }
        
        if (document.getElementById("<%=rdParada.ClientID%>").checked)
        {
            if (document.getElementById("<%=txtParadaSenha.ClientID%>").value == '') {
                msg += " a Senha. \n";
                if (item.length > 0) item += ":<%=txtParadaSenha.ClientID%>"; else item += "<%=txtParadaSenha.ClientID%>";
                retorno = false;
            }
        }
        if (retorno == false) {
            BootstrapDialog.show({ title: 'ATENÇÃO!', message: msg });
            var ind = item.split(":");
            if (ind.length > 0)
                document.getElementById(ind[0]).focus();
        }
        else {

            if (document.getElementById("<%=rdMotivo.ClientID%>").checked) {

                if (confirm("Deseja realmente mudar o motivo da parada?")) {
                    confirm_value.value = "true";
                } else {
                    confirm_value.value = "false";
                }
            }
            else {
                if (confirm("Deseja realmente encerrar a parada?")) {
                    confirm_value.value = "true";
                } else {
                    confirm_value.value = "false";
                }
            }
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
        <table style="width: 100%;">
            <tr>
                <td style="width: 10%; padding-top: 10px; padding-left: 20px; vertical-align: middle;">
                    <div class="form-control" style="width: 25%;">
                        <asp:RadioButton runat="server" ID="rdMotivo" GroupName="gpTHP" Text="Mudar motivo" OnCheckedChanged="rdMotivo_CheckedChanged" AutoPostBack="true" Checked="true" />
                        <asp:RadioButton runat="server" ID="rdParada" GroupName="gpTHP" Text="Encerrar parada" OnCheckedChanged="rdParada_CheckedChanged" AutoPostBack="true" />
                    </div>
                </td>
            </tr>
        </table>
        <table style="width: 100%; padding-left: 15px;">
            <tr>
                <td style="width: 10%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Id:</label>
                    <asp:TextBox runat="server" ID="txtDadosID" CssClass="form-control" Enabled="false" />
                    <asp:Label runat="server" ID="lblSenhaUsuario" Visible="false" />
                </td>
                <td style="width: 10%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Trem:</label>
                    <asp:TextBox runat="server" ID="txtDadosTrem" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Tempo:</label>
                    <asp:TextBox runat="server" ID="txtDadosTempo" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Local:</label>
                    <asp:TextBox runat="server" ID="txtDadosLocal" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Grupo:</label>
                    <asp:TextBox runat="server" ID="txtDadosGrupo" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                    <label>Corredor:</label>
                    <asp:TextBox runat="server" ID="txtDadosCorredor" CssClass="form-control" Enabled="false" />
                </td>
                <td style="width: 55%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;"></td>
            </tr>
            <tr>
                <td style="width: 100%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;" colspan="6">
                    <label>Motivo:</label>
                    <asp:Label runat="server" ID="lblDadosCodigoMotivo" Visible="false" />
                    <asp:TextBox runat="server" ID="txtDadosMotivo" CssClass="form-control" Enabled="false" />
                </td>
            </tr>
        </table>
        <div runat="server" id="dvMotivo" aria-autocomplete="none" >
            <table style="width: 100%;  padding-left: 15px;">
                <tr>
                    <td style="width: 60%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                        <label>Novo Motivo:</label>
                        <asp:DropDownList runat="server" ID="ddlMotivoNovo" CssClass="form-control"  />
                    </td>
                    <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;" >
                        <label>Senha:</label>
                        <asp:TextBox ID="txtMotivoSenha" runat="server" TextMode="Password" CssClass="form-control search" BackColor="White" AutoCompleteType="Disabled" ></asp:TextBox>
                    </td>
                    <td style="width: 30%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;"></td>
                </tr>
            </table>
        </div>
        <div runat="server" id="dvParada" visible="false">
            <table style="width: 100%;  padding-left: 15px;">
                <tr>
                    <td style="width: 15%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;">
                        <label>Senha:</label>
                        <asp:TextBox ID="txtParadaSenha" runat="server" TextMode="Password" CssClass="form-control search" BackColor="White" AutoCompleteType="Disabled" ></asp:TextBox>
                    </td>
                    <td style="width: 90%; padding-top: 10px; padding-left: 10px; padding-right: 10px; vertical-align: middle;"></td>
                </tr>
            </table>
        </div>
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
