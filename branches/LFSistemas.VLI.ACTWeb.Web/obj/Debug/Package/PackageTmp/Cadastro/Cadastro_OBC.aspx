<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Cadastro_OBC.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Cadastro_OBC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="lblTitulo" runat="server" Text="Consulta de Frases do Display " Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
            if (document.getElementById("<%=txtFirmwere.ClientID%>").value == '') {
                msg += " Versao Firmwere. \n";
                item += "<%=txtFirmwere.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtMapa.ClientID%>").value == '') {
                msg += " a Versão Mapa  . \n";
                if (item.length > 0) item += ":<%=txtMapa.ClientID%>"; else item += "<%=txtMapa.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtPrvF.ClientID%>").value == '') {
                msg += " a Prv.Firmware  . \n";
                if (item.length > 0) item += ":<%=txtPrvF.ClientID%>"; else item += "<%=txtPrvF.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtPrvM.ClientID%>").value == '') {
                msg += " a Prv.Mapa  . \n";
                if (item.length > 0) item += ":<%=txtPrvM.ClientID%>"; else item += "<%=txtPrvM.ClientID%>";
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
    <table style="width: 100%">
                <tr>
                    <td style="width: 10%">
                        <label for="nome">Versao Firmwere:</label>
                        <asp:TextBox runat="server" ID="txtFirmwere" Width="91%" CssClass="form-control" onkeypress="return fnValidaNroVirgula (event) ;" />
                    </td>
                    <td style="width: 10%">
                        <label for="nome">Versão Mapa:</label>
                        <asp:TextBox runat="server" ID="txtMapa"  Width="91%" CssClass="form-control"  onkeypress="return fnValidaNroVirgula(event);" />
                    </td>
                    <td style="width: 10%">
                        <label for="nome">Prv.Firmware:</label>
                        <asp:TextBox runat="server" ID="txtPrvF"  Width="91%" CssClass="form-control" />
                    </td>
                    <td style="width: 10%">
                        <label for="nome">Prv.Mapa:</label>
                        <asp:TextBox runat="server" ID="txtPrvM"  Width="91%" CssClass="form-control"   />
                    </td>
                     <asp:TextBox runat="server" ID="txtDadosDataAtual" Width="10%"  CssClass="form-control" Visible="false" />
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="width: 4%">
                            <label for="nome">Ativo:</label>
                           <asp:CheckBox runat="server" ID="chkAtivo" CssClass="form-control" Width="40"  Checked="true"/>
                        </div>
                    </td>
                </tr>
                <tr>
                  <td colspan="2">
                        <asp:Label runat="server" ID="lblOBCID" Visible="false" />
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                            &nbsp;&nbsp;
                            &nbsp;&nbsp;
                    </td>
                </tr>          
   </table>
    <br />
    <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">ARQ.FIRMWARE + GR :&nbsp;&nbsp;</td>
    <td style="width: 90%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;"> 
    <asp:FileUpload ID ="FileUpload2" runat="server" AllowMultiple="false" accept="application/zip" BorderStyle="None" CssClass="form-control" />
    <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">ARQ.MAPA :&nbsp;&nbsp;</td>
    <td style="width: 90%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;"> 
    <asp:FileUpload ID ="FileUpload4" runat="server" AllowMultiple="false" accept=".lib" BorderStyle="None" CssClass="form-control" />
    <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">ARQ.LOGIN :&nbsp;&nbsp;</td>
    <td style="width: 90%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;"> 
    <asp:FileUpload ID ="FileUpload5" runat="server" AllowMultiple="false" accept=".crp" BorderStyle="None" CssClass="form-control" />
    <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">ARQ.HISTORICO VER.MAPA :&nbsp;&nbsp;</td>
    <td style="width: 90%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;"> 
    <asp:FileUpload ID ="FileUpload3" runat="server" AllowMultiple="false" accept="application/pdf" BorderStyle="None" CssClass="form-control" />
         <br />
         <asp:LinkButton runat="server" ID="lnkSalvar" CssClass="btn btn-success" Text="Salvar" OnClick="lnkSalvar_Click1" OnClientClick="javascript:return validaFormulario();" ToolTip="Grava os dados informados no banco."><i class="fa fa-floppy-o" ></i>&nbsp;Salvar</asp:LinkButton>
         <asp:LinkButton runat="server" ID="lnkCancelar" CssClass="btn btn-info" Text="Cancelar" OnClick="lnkCancelar_Click" ToolTip="Cancela a operação e retorna para a lista de abreviaturas."><i class="fa fa-sign-out"></i>&nbsp;Cancelar</asp:LinkButton>
         <asp:LinkButton runat="server" ID="lnkExcluir" CssClass="btn btn-danger" Text="Cancelar" OnClientClick="Confirm()" OnClick="lnkExcluir_Click" ToolTip="Apaga o registro do banco."><i class="fa fa-minus-circle"></i>&nbsp;Excluir</asp:LinkButton>
      
     <asp:Label ID="Label1" runat="server" Visible="false"></asp:Label>

     </table>
          
</asp:Content>

