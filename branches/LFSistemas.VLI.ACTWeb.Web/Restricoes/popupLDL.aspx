<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupLDL.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.popupLDL" %>

<!DOCTYPE html>
<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<!--<![endif]-->
<head>
    <title>ACTWEB - Interdições LDL</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">

    <link rel="stylesheet" type="text/css" href="../js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="../css/main.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="../css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="../css/bootstrap-modal.min.css" />

    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
    <script type="text/javascript" src="../js/jbootstrap-modal.min.js"></script>

    <link rel="grupo vli" href="logo-vli.ico">

    <script type="text/javascript">
        function tecla() {

            if (window.event.keyCode == 120) {
                document.getElementById('link1').click();
            }
            if (window.event.keyCode == 119) {
                document.getElementById('link2').click();
            }
        }
        function getRadioButtonSelectedTelefone() {
            txtDadosTelefone = document.getElementById('<%=txtDadosTelefone.ClientID %>');
            txtDadosMacro = document.getElementById('<%=txtDadosMacro.ClientID %>');

            document.getElementById('<%=txtDadosTelefone.ClientID %>').value = '';
            document.getElementById('<%=txtDadosMacro.ClientID %>').value = '';
            document.getElementById('<%=txtPrefixo.ClientID %>').value = '';
            txtDadosTelefone.disabled = false;
            txtDadosMacro.disabled = true;
            lblPrefixo.disabled = true;
            txtPrefixo.disabled = true;
            document.getElementById("<%=txtDadosTelefone.ClientID%>").focus();
        }
        function getRadioButtonSelectedRadio() {
            txtDadosTelefone = document.getElementById('<%=txtDadosTelefone.ClientID %>');
            txtDadosMacro = document.getElementById('<%=txtDadosMacro.ClientID %>');

            document.getElementById('<%=txtDadosTelefone.ClientID %>').value = '';
            document.getElementById('<%=txtDadosMacro.ClientID %>').value = '';
            document.getElementById('<%=txtPrefixo.ClientID %>').value = '';
            txtDadosTelefone.disabled = true;
            txtDadosMacro.disabled = true;
            lblPrefixo.disabled = true;
            txtPrefixo.disabled = true;
        }
        function getRadioButtonSelectedMacro() {
            txtDadosTelefone = document.getElementById('<%=txtDadosTelefone.ClientID %>');
            txtDadosMacro = document.getElementById('<%=txtDadosMacro.ClientID %>');
            txtPrefixo = document.getElementById('<%=txtPrefixo.ClientID %>');
            lblPrefixo = document.getElementById('<%=lblPrefixo.ClientID %>');

            document.getElementById('<%=txtDadosTelefone.ClientID %>').value = '';
            document.getElementById('<%=txtDadosMacro.ClientID %>').value = '';
            document.getElementById('<%=txtPrefixo.ClientID %>').value = '';
                
            txtDadosMacro.disabled = false;
            txtDadosTelefone.disabled = true;
            lblPrefixo.disabled = false;
            txtPrefixo.disabled = false;

          
            document.getElementById("<%=txtDadosMacro.ClientID%>").focus();
        }

        function validaFormulario() {
            var retorno = true;
            var msg = "Informe";
            var item = [];
            if (document.getElementById("<%=ddlDadosSecao.ClientID%>").value == '0' || document.getElementById("<%=ddlDadosSecao.ClientID%>").value == '') {
                msg += " a seção. \n";
                if (item.length > 0) item += ":<%=ddlDadosSecao.ClientID%>"; else item += "<%=ddlDadosSecao.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlDadosTipoDaInterdicao.ClientID%>").value == '0' || document.getElementById("<%=ddlDadosTipoDaInterdicao.ClientID%>").value == '') {
                msg += " o tipo de interdição. \n";
                if (item.length > 0) item += ":<%=ddlDadosTipoDaInterdicao.ClientID%>"; else item += "<%=ddlDadosTipoDaInterdicao.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlDadosTipoDaManutencao.ClientID%>").value == '0' || document.getElementById("<%=ddlDadosTipoDaManutencao.ClientID%>").value == '') {
                msg += " o tipo de manutenção. \n";
                if (item.length > 0) item += ":<%=ddlDadosTipoDaManutencao.ClientID%>"; else item += "<%=ddlDadosTipoDaManutencao.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=ddlDadosMotivo.ClientID%>").value == '0' || document.getElementById("<%=ddlDadosMotivo.ClientID%>").value == '') {
                msg += " o motivo da LDL. \n";
                if (item.length > 0) item += ":<%=ddlDadosMotivo.ClientID%>"; else item += "<%=ddlDadosMotivo.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosResponsavel.ClientID%>").value == '') {
                msg += " CPF do Responsável. \n";
                if (item.length > 0) item += ":<%=txtDadosResponsavel.ClientID%>"; else item += "<%=txtDadosResponsavel.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosDuracaoSolicitada.ClientID%>").value == '') {
                msg += " a Duração. \n";
                if (item.length > 0) item += ":<%=txtDadosDuracaoSolicitada.ClientID%>"; else item += "<%=txtDadosDuracaoSolicitada.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosKm.ClientID%>").value == '') {
                msg += " o Km. \n";
                if (item.length > 0) item += ":<%=txtDadosKm.ClientID%>"; else item += "<%=txtDadosKm.ClientID%>";
                retorno = false;
            }
            if (document.getElementById("<%=txtDadosResponsavel.ClientID%>").value == '') {
                msg += " o responsável. \n";
                if (item.length > 0) item += ":<%=txtDadosResponsavel.ClientID%>"; else item += "<%=txtDadosResponsavel.ClientID%>";
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
        function ismaxlength(obj) {
            var mlength = 38;
            if (obj.getAttribute && obj.value.length > mlength) {
                alert('Valor máximo de caracteres 38.');
                obj.value = obj.value.substring(0, mlength)
            }
        }

        //function soufoda()
        {
            // We want the form above to be there still because it houses our buttons, but it also means our
            // code still works without js.  With js disabled the "hide" never runs so the form stays visible
            // and functioning
            //$("#warning").hide();

            var ret = '<%=this.retirando %>';
            if (ret == "True")
            {
                var id = '<%=this.id_aut %>';
                var id2 = '<%=this.sb %>';
                var person = prompt("CONFIRME O Nº DE AUTORIZAÇÃO (SB EM CAIXA ALTA + Nº autorização)", "")
                if (person == id2 + id) {
                    $.ajax({
                        type: "POST",
                        url: "popupLDL.aspx/DeleteRestriction",
                        data: "{id:'" + id2 + id + "'}",
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            // if you want something to happen after the ajax call then
                            // code it here
                            document.getElementById('lnkRetirar').click();
                            alert("Deleção da LDL " + person + " foi solicitada!");
                        }
                    });
                }
                else
                {
                    // If you want to run a server-function when the user cancels then just
                    // do an ajax call here as above, likewise you can put general js here too
                    alert("Deleção de LDL abortada!");
                }
            }
        }

    </script>
</head>
<body onkeydown="tecla()">
    <form id="formInterdicao" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
        <style>
            .situacao-A {
                color: black;
                background-color: rgb(240,230,140); /* A - ARetirar | Khaki | */
            }

                .situacao-A:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-S {
                color: black;
                background-color: rgb(255,255,255); /* S - Solicitada |  | */
            }

                .situacao-S:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-C {
                color: black;
                background-color: rgb(144,238,144); /* C - Confirmada | LightGreen | */
            }

                .situacao-C:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-N {
                color: white;
                background-color: rgb(255,0,0); /* N - Negada | Tomato | */
            }

                .situacao-N:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-R {
                color: black;
                background-color: rgb(192,192,192); /* R - Retirada | Silver | */
            }

                .situacao-R:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-X {
                color: black;
                background-color: rgb(218,112,214); /* X - Cancelada pelo Solicitante | Orchid | */
            }

                .situacao-X:hover {
                    color: black;
                    background-color: white;
                }

            .situacao-E {
                color: black;
                background-color: grey; /* X - Cancelada pelo Solicitante | Orchid | */
            }

                .situacao-E:hover {
                    color: grey;
                    background-color: black;
                }

            .grid {
                width: 100%;
                height: 270px;
                overflow: scroll;
                overflow-x: hidden;
            }

            .linkbutton_enable {
            }
            .auto-style1 {
                width: 10%;
                height: 20px;
            }
            .auto-style2 {
                width: 65%;
                height: 20px;
            }
        </style>
        <div style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%; text-align: left;">
            <table style="width: 100%">
                <tr>
                    <td style="width: 79%;">
                        <div class="alert alert-success">
                            <h2>
                                <asp:Label ID="Label1" runat="server" Text="Solicitações de LDL" Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
        </div>
        <%--[ FORMULÁRIO PARA SOLICITAÇÃO DE LDL ]--%>
        <div class="well well-sm" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <div class="form-group">
                <div class="page-header sub-content-header">
                    <a id="link2" data-toggle="collapse" title="Restrição F8" data-parent="#macros" href="macros#Interdicao" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Criar Interdição</b> <i class="fa fa-ban"></i></a>
                </div>
            </div>
            <div id="Interdicao">
                <table class="nav-justified" style="width: 100%">
                    
                    <!--LINHA DA AUTORIZAÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Autorização&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtAutorizacao" runat="server" CssClass="form-control" Enabled="false" Text="" />
                        </td>
                        <td style="width: 40%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                    </tr>

                    <!--LINHA DA SITUAÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Situação&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:DropDownList ID="ddlDadosTipoDaSituacao" runat="server" CssClass="form-control" Enabled="false" />
                        </td>
                        <td style="width: 20%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Data&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosDataAtual" runat="server" CssClass="form-control" Enabled="false" Text="" />
                        </td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:Label runat="server" ID="lblIdentificador" Visible="false" />
                        </td>
                    </tr>

                    <!--LINHA DA SEÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Seção:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:DropDownList ID="ddlDadosSecao" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDadosSecao_SelectedIndexChanged" AutoPostBack="true" />
                        </td>
                        <td style="width: 20%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblMensagem" Text="&nbsp;&nbsp;" Font-Size="14.5px" ForeColor="Red" /></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Tipo da Interdição:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:DropDownList ID="ddlDadosTipoDaInterdicao" runat="server" CssClass="form-control" />
                        </td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>

                    <!--LINHA DA DURAÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Duração (Min.):&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosDuracaoSolicitada" runat="server" CssClass="form-control" />
                        </td>
                        <td style="width: 20%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Tipo da Manutenção:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:DropDownList ID="ddlDadosTipoDaManutencao" runat="server" CssClass="form-control" />
                        </td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>

                    <!--LINHA DA CIRCULAÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Circulação:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:DropDownList ID="ddlDadosTipoDaCirculacao" runat="server" CssClass="form-control" Enabled="false" />
                        </td>
                        <td style="width: 40%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Km:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosKm" runat="server" CssClass="form-control" />
                        </td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>

                    <!--LINHA DO OPERADOR CVV-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Operador CCV:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosOperadorCCV" runat="server" CssClass="form-control" Enabled="false" MaxLength="11" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>

                        <td style="width: 25%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" >
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <table style="width: 100%;">
                                        <tr>                                            
                                            <td style="width: 40%; vertical-align: middle; text-align: left; padding: 1px; color: rgb(0, 72, 89);">&nbsp;&nbsp;
                                                <asp:Label runat="server" ID="lblOperadorCCV_Nome" Font-Size="12" Font-Bold="true" />
                                            </td>
                                             <td style="width: 20%; vertical-align: middle; text-align: left; padding: 1px; color: rgb(0, 72, 89); >
											                         	<asp:Label runat="server" ID="lblCanalCom" Font-Size="12" Font-Bold="true" />
											                         		&nbsp;&nbsp;
											                       </td>											                      
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                                                                   
                        <!-- ACHO QUE O ELBERTH DUPLICO.. O QUE TÁ ACIMA TAVA NA LINHA DE BAIXO (CPF)
                        <td style="width: 20%; vertical-align: middle; text-align: left; padding: 1px; color: rgb(0, 72, 89);">MARCA TÍTULO&nbsp;&nbsp;<asp:Label runat="server" ID="Label2" Font-Size="12" Font-Bold="true" />
                        </td>
                        -->                        
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>                                                
                    </tr>

                    <!--LINHA DO CPF RESPONSÁVEL-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">CPF Responsável:&nbsp;&nbsp;</td>
                        <td style="width: 15%; padding: 1px;">
                            <asp:TextBox ID="txtDadosResponsavel" runat="server" CssClass="form-control" OnTextChanged="txtDadosResponsavel_TextChanged" AutoPostBack="true" />
                        </td>

                        <td style="width: 15%; vertical-align: bottom; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" ><!--</td>                           
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;" colspan="2">-->
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 5%; vertical-align: middle; text-align: left;" >Telefone Responsável:&nbsp;&nbsp;</td>
                                    <td style="width: 20%; vertical-align: middle; text-align: left;" >
                                        <asp:TextBox ID="txtTelefoneResponsavel" runat="server" MaxLength="11" CssClass="form-control" />
                                    </td>
                                    <td style="width: 35%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" >
                                        <asp:UpdatePanel runat="server">
                                            <ContentTemplate>
                                                <table style="width: 100%;">
                                                    <tr>                                            
                                                        <td style="width: 20%; vertical-align: middle; text-align: left; padding: 1px; color: rgb(0, 72, 89);">&nbsp;&nbsp;<asp:Label runat="server" ID="lblResponsavel_Nome" Font-Size="12" Font-Bold="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>  
                                </tr>
                            </table>
                        </td>
                        
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px; padding-left:20px;">
                            <asp:RadioButton ID="rdDadosTelefone" runat="server"  Text="Telefone:" GroupName="rdContato" TextAlign="Left" Checked="true" onchange="getRadioButtonSelectedTelefone();" />&nbsp;&nbsp;
                        </td>

                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosTelefone" MaxLength="11" runat="server" CssClass="form-control" />
                        </td>                                                                    

                        <td style="width: 15%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>                          
                    </tr>

                    <!--LINHA DOS EQUIPAMENTOS-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Equipamentos:&nbsp;&nbsp;</td>
                        <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosEquipamentos" runat="server" CssClass="form-control" />
                        </td>
                        <td style="width: 15%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;"></td>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px; padding-left:20px; ">
                            <asp:RadioButton ID="rdDadosRadio" runat="server" Text="Rádio:" GroupName="rdContato" TextAlign="Left" onchange="getRadioButtonSelectedRadio();" />&nbsp;&nbsp;
                        </td>
                                              
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>

                    <!--LINHA DO MOTIVO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Motivo:&nbsp;&nbsp;</td>
                        <td style="width: 35%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" colspan="2">
                            <asp:DropDownList ID="ddlDadosMotivo" runat="server" CssClass="form-control" />
                        </td>

                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px; padding-left:20px;">
                            <asp:RadioButton ID="rdDadosMacro" runat="server" Text="Macro:" GroupName="rdContato" TextAlign="Left" onchange="getRadioButtonSelectedMacro();" />&nbsp;&nbsp;
                        </td> 

                         <td style="width: 15%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtDadosMacro" runat="server" CssClass="form-control" Enabled="false" />
                        </td>
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>

                    <!--LINHA DA OBSERVAÇÃO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">Observação:&nbsp;&nbsp;</td>
                        <td style="width: 65%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" colspan="2">
                            <asp:TextBox ID="txtDadosObsercacao" runat="server" CssClass="form-control" MaxLength="38" onkeyup="return ismaxlength(this);" />
                        </td>

                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;  padding-left:20px; ">
                                <asp:Label runat="server" ID="lblPrefixo" Font-Size="9" Font-Bold="true" /></td>
                        	
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="txtPrefixo" MaxLength="4" runat="server" CssClass="form-control" />
                        </td>  
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>


                     <!-- Nova linha contendo informação da cauda - P707 e talvez prefixo, se decidirem mudar-->
                     <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                        <td style="width: 65%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" colspan="2">                            
                        </td>

                        <td style="width: 10%; vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;  padding-left:20px; ">
                            <asp:Label runat="server" ID="lbCauda" Font-Size="9" Font-Bold="true" />
                        </td>
                        	
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <asp:TextBox ID="tbCauda" MaxLength="6" runat="server" CssClass="form-control" />
                        </td>  
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">&nbsp;&nbsp;</td>
                    </tr>
                   
                    <!--LINHA EM BRANCO DE ESPAÇANENTO-->
                    <tr>
                        <td style="width: 10%; vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;">
                            <br />
                        </td>
                    </tr>

                    <!--BOTÕES-->
                    <tr>
                        <td style="vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" class="auto-style1">&nbsp;&nbsp;</td>
                        <td style="vertical-align: middle; text-align: left; margin-top: 10px; margin-bottom: 10px; padding: 1px;" colspan="4" class="auto-style2">
                            <asp:LinkButton ID="lnkCriar" runat="server" OnClick="lnkCriar_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Solicitar criação de interdição de LDL."><i class="fa fa-plus"></i>&nbsp;Criar Interdição</asp:LinkButton>
                            &nbsp;&nbsp;
                                    <%--<asp:LinkButton ID="lnkRetirar" runat="server" OnClick="soufoda()" ToolTip="Solicitar remoção de interdição de LDL."><i class="fa fa-minus"></i>&nbsp;Retirar Interdição</asp:LinkButton>--%>
                                    <asp:LinkButton ID="lnkRetirar" runat="server" OnClick="lnkRetirar_Click" ToolTip="Solicitar remoção de interdição de LDL."><i class="fa fa-minus"></i>&nbsp;Retirar Interdição</asp:LinkButton>
                             <%--<asp:LinkButton runat="server" ID="lnkParadaImediata" OnClick="lnkParadaImediata_OnClick">
                                        <span class="menu-item-icon"><i class="fa fa-envelope"></i></span>
                                        <span class="menu-item-label" title="Parada Imediata">Parada Imediata</span>
                                    </asp:LinkButton>--%>
                                    <asp:LinkButton ID="lnkAtualizarLista" runat="server" OnClick="lnkAtualizarLista_Click" ToolTip="Atualiza as informações do grid abaixo."><i class="fa fa-table"></i>&nbsp;Atualizar Lista</asp:LinkButton>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkLImpar" runat="server" OnClick="lnkLImpar_Click" ToolTip="Limpa os dados do formulário acima."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkNovoResponsavel" runat="server" OnClick="lnkNovoResponsavel_Click" ToolTip="Cadastra um novo responsável no bando de dados."><i class="fa fa-file-o"></i>&nbsp;Novo Responsável</asp:LinkButton>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkAtualizarCPF" runat="server" OnClick="lnkAtualizarCPF_Click" ToolTip="Atualizar CPF da LDL."><i class="fa fa-file-o"></i>&nbsp;Atualizar CPF</asp:LinkButton>
                            &nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkCancelar" runat="server" OnClick="lnkCancelar_Click" ToolTip="Cancela a operação."><i class="fa fa-times"></i>&nbsp;Cancelar</asp:LinkButton>
                            &nbsp;&nbsp;
                        </td>
                        <td style="vertical-align: middle; text-align: right; margin-top: 10px; margin-bottom: 10px; padding: 1px;" class="auto-style1">&nbsp;&nbsp;</td>
                    </tr>
                </table>
            </div>
            <div>



            



            </div>
        </div>
        <%--[ FILTRO DE PESQUISA ]--%>
        <div class="well well-sm" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <div class="form-group">
                <div class="page-header sub-content-header">
                    <a id="link1" data-toggle="collapse" title="Filtros F9" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                </div>
            </div>
            <div id="filtros" class="collapse">
                <table style="width: 100%; ">
                    <tr>
                        <td style="width: 5%; padding: 1px;">
                            <label for="grupo">Autorização:</label><br />
                            <asp:TextBox runat="server" ID="txtFiltroAutorizacao" Width="95%" CssClass="form-control" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 15%; padding: 1px;">
                            <label for="grupo">Situação:</label><br />
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="ddlFiltroTipoDaSituacao" runat="server" Width="95%" CssClass="form-control" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td style="width: 15%; padding: 1px;">
                            <label for="grupo">Seção:</label><br />
                            <asp:DropDownList runat="server" ID="ddlFiltroSecao" Width="95%" CssClass="form-control" />
                        </td>
                        <td style="width: 10%; padding: 1px;">
                            <label for="data_inicio">Data Inicial:</label>
                            <asp:TextBox ID="txtDataInicial" runat="server" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td style="width: 10%; padding: 1px;">
                            <label for="hora_inicio">Data Final:</label>
                            <asp:TextBox ID="txtDataFinal" runat="server" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                        </td>
                        <td rowspan="2" style="width: 170px; padding: 1px;">
                            <label for="data_fim">Corredor:</label>
                            <br />
                            <asp:CheckBoxList AutoPostBack="true" OnSelectedIndexChanged="clbCorredorLDL_SelectedIndexChanged" runat="server" ID="clbCorredorLDL" Rows="6" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="117">
                                <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="Baixada" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="Centro Leste" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="Centro Norte" />
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="Centro Sudeste" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="Minas Bahia" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="Minas Rio" />
                            </asp:CheckBoxList>
                        </td>
                        <td style="width: 35%; padding: 1px;"></td>

                    </tr>
                    <tr>
                        <td style="width: 5%; padding: 1px;">
                            <label for="grupo">Km:</label><br />
                            <asp:TextBox runat="server" ID="txtFiltroKm" Width="95%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" />
                        </td>
                        <td style="width: 50%; padding: 1px;" colspan="4">
                            <label for="grupo">Observação:</label><br />
                            <asp:TextBox runat="server" ID="txtFiltroObservacao" Width="99%" CssClass="form-control" />
                        </td>

                        <td style="width: 45%; padding: 1px;"></td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 100%; padding: 1px;" colspan="5">
                            <asp:Button ID="bntFiltroPesquisar" runat="server" OnClick="bntFiltroPesquisar_Click" CssClass="btn btn-primary" Text="Pesquisar" />
                            <asp:Button ID="bntFiltroLimpar" runat="server" OnClick="bntFiltroLimpar_Click" class="btn btn-primary" Text="Limpar" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="well well-lg" style="margin-top: 1%; margin-left: 4%; margin-right: 4%; margin-bottom: 1%;">
            <table class="nav-justified" style="width: 100%">
                <tr>

                    <td>
                        <asp:Panel ID="Panel1" runat="server" CssClass="grid">
                            <asp:Repeater ID="rptListaInterdicoes" runat="server">
                                <HeaderTemplate>
                                    <table class="table table-hover table-curved pro-table" id="macros">
                                        <thead>
                                            <tr>
                                                <th style="background-color: #fff; width: 02%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#"><i class="fa fa-search-plus"></a></th>
                                                <th style="background-color: #fff; width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkLdl" runat="server" OnClick="lnkLdl_Click">Autorização</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkMotivo" runat="server" OnClick="lnkMotivo_Click">Motivo</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkSituacao" runat="server" OnClick="lnkSituacao_Click">Situação</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 07%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkSecao" runat="server" OnClick="lnkSecao_Click">Seção</asp:LinkButton></th>
                                                <th style="background-color: #fff; width: 07%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkManutencao" runat="server" OnClick="lnkManutencao_Click">Manutenção</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 07%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkData" runat="server" OnClick="lnkData_Click">Data</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 05%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkKM" runat="server" OnClick="lnkKM_Click">KM</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 20%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkObservacao" runat="server" OnClick="lnkObservacao_Click">Observação</asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 10%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkDuracaoSolicitada" runat="server" OnClick="lnkDuracaoSolicitada_OnClick">Solicitado <Font size="2">(hh:mm)</Font></asp:LinkButton>
                                                </th>
                                                <th style="background-color: #fff; width: 10%; text-align: center; font-size: 12pt;">
                                                    <asp:LinkButton ID="lnkDuracaoAutorizada" runat="server" OnClick="lnkDuracaoAutorizada_OnClick">Autorizado <Font size="2">(hh:mm)</Font> </asp:LinkButton>
                                                </th>

                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr style="font-size: 9px; margin-top: 1px; cursor: pointer;" class="situacao-<%# Eval("Situacao_Nome").ToString().Substring(0,1) %>">
                                        <td style="width: 02%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                            <asp:LinkButton ID="lnkEdite" runat="server" OnClick="lnkEdite_Click" CommandArgument='<%# Eval("Solicitacao_ID_ACTWEB") %>'><i class="fa fa-search-plus"></i></asp:LinkButton>
                                        </td>
                                        <td style="width: 05%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Interdicao_Motivo") %>"><%# Eval("Interdicao_Motivo") %></td>
                                        <td style="width: 05%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Motivo_Desc") %>"><%# Eval("Motivo_Desc") %></td>
                                        <td style="width: 05%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Situacao_Nome") %>"><%# Eval("Situacao_Nome") %></td>
                                        <td style="width: 07%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Secao_Nome") %>"><%# Eval("Secao_Nome") %></td>
                                        <td style="width: 07%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo_Manutencao_Nome") %>"><%# Eval("Tipo_Manutencao_Nome") %></td>
                                        <td style="width: 07%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Data") %>"><%# Eval("Data") %></td>
                                        <td style="width: 05%; text-align: right; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Km") %>"><%# Eval("Km") %></td>
                                        <td style="width: 25%; text-align: left; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Observacao") %>"><%# Eval("Observacao") %></td>
                                        <td style="width: 10%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Duracao_Solicitada") %>"><%# double.Parse(Eval("Duracao_Solicitada").ToString()) != 0 ? string.Format("{0:d2}:{1:d2}", (int)TimeSpan.FromMinutes(double.Parse(Eval("Duracao_Solicitada").ToString())).TotalHours, (int)TimeSpan.FromMinutes(double.Parse(Eval("Duracao_Solicitada").ToString())).Minutes) : ""%> </td>
                                        <td style="width: 10%; text-align: center;" title="<%# Eval("Duracao_Autorizada") %>"><%# double.Parse(Eval("Duracao_Autorizada").ToString()) != 0 ? string.Format("{0:d2}:{1:d2}", (int)TimeSpan.FromMinutes(double.Parse(Eval("Duracao_Autorizada").ToString())).TotalHours, (int)TimeSpan.FromMinutes(double.Parse(Eval("Duracao_Autorizada").ToString())).Minutes) : ""%> </td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </tbody>
        </table>
                                </FooterTemplate>
                            </asp:Repeater>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <br />
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsolutions.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
        <%--        </asp:Panel>--%>            
    </form>

</body>
</html>
