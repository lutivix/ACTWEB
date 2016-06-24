<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Responsavel_LDL.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Cadastro.Responsavel_LDL" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <title>ACTWEB - Responsável LDL</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">

    <link rel="stylesheet" type="text/css" href="../js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="../css/main.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui.css" />

    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>

    <link rel="grupo vli" href="logo-vli.ico">
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
            if (document.getElementById("<%=txtNome.ClientID%>").value == '') {
                msg += " o nome do responsável. \n";
                if (item.length > 0) item += ":<%=txtNome.ClientID%>"; else item += "<%=txtNome.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtMatricula.ClientID%>").value == '') {
                msg += " a matrícula do responsável. \n";
                if (item.length > 0) item += ":<%=txtMatricula.ClientID%>"; else item += "<%=txtMatricula.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtSenha.ClientID%>").value == '') {
                msg += " a senha do responsável. \n";
                if (item.length > 0) item += ":<%=txtSenha.ClientID%>"; else item += "<%=txtSenha.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlCargo.ClientID%>").value == '0' || document.getElementById("<%=ddlCargo.ClientID%>").value == '') {
                msg += " o cargo do responsável. \n";
                if (item.length > 0) item += ":<%=ddlCargo.ClientID%>"; else item += "<%=ddlCargo.ClientID%>";
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
</head>
<body onkeydown="tecla()">
    <form id="formResponsavel" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: center;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50%; text-align: left;">
                        <asp:Label ID="Label1" runat="server" Text="Cadastro de Responsável" CssClass="label" Font-Size="20px" ForeColor="Blue" title="Interdições LDL" />
                    </td>
                    <td style="width: 50%; text-align: right;">
                        <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                            <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" ForeColor="Gray" />,&nbsp;
                            <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" ForeColor="Gray" />&nbsp;
                            <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" ForeColor="Gray" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="well well-sm" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">Nome:&nbsp;&nbsp;</td>
                    <td style="width: 20%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; " colspan="4"><asp:TextBox runat="server" ID="txtNome" CssClass="form-control" /> </td>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                </tr>
                <tr>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">Matricula:&nbsp;&nbsp;</td>
                    <td style="width: 20%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "><asp:TextBox runat="server" ID="txtMatricula" CssClass="form-control" /> </td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">Senha:&nbsp;&nbsp;</td>
                    <td style="width: 20%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "><asp:TextBox runat="server" ID="txtSenha" CssClass="form-control" /></td>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                </tr>
                <tr>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">Cargo:&nbsp;&nbsp;</td>
                    <td style="width: 20%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">
                        <asp:DropDownList ID="ddlCargo" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Selecione" Value="0" />
                            <asp:ListItem Text="Administrador" Value="administrador" />
                            <asp:ListItem Text="Despachador" Value="despachador" />
                            <asp:ListItem Text="Engenheiro" Value="engenheiro" />
                            <asp:ListItem Text="HelpDesk" Value="HelpDesk" />
                            <asp:ListItem Text="Supervisor" Value="supervisor" />
                        </asp:DropDownList>                    
                    </td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; ">LDL:&nbsp;&nbsp;</td>
                    <td style="width: 20%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px; "><asp:CheckBox runat="server" ID="chkLDL"  /></td>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                </tr>
                <tr>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                    <td style="width: 20%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px; " colspan="4">
                        <asp:LinkButton ID="lnkNovoResponsavel" runat="server" CssClass="btn btn-success" OnClick="lnkNovoResponsavel_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Salvar responsável no banco."><i class="fa fa-floppy-o"></i>&nbsp;Salvar</asp:LinkButton>
                    </td>
                    <td style="width: 5%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px; "></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
