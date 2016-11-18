<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupRestricoes.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.popupRestricoes" %>

<!DOCTYPE html>
<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<!--<![endif]-->
<head>
    <title>ACTWEB - Restrições</title>
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
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>

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

        function getDropdownListSelectedText() {
            var DropdownList = document.getElementById('<%=ddlDadosTipoRestricao.ClientID %>');
            var SelectedText = DropdownList.options[DropdownList.selectedIndex].text.substring(0, 2);

            ddlDadosSecoes = document.getElementById('<%=ddlDadosSecoes.ClientID %>');
            ddlDadosTipoRestricao = document.getElementById('<%=ddlDadosTipoRestricao.ClientID %>');
            ddlDadosSubTipoVR = document.getElementById('<%=ddlDadosSubTipoVR.ClientID %>');
            txtDadosDataInicial = document.getElementById('<%=txtDadosDataInicial.ClientID %>');
            txtDadosHoraInicial = document.getElementById('<%=txtDadosHoraInicial.ClientID %>');
            txtDadosDataFinal = document.getElementById('<%=txtDadosDataFinal.ClientID %>');
            txtDadosKm_Inicio = document.getElementById('<%=txtDadosKm_Inicio.ClientID %>');
            txtDadosKm_Final = document.getElementById('<%=txtDadosKm_Final.ClientID %>');
            txtDadosResponsavel = document.getElementById('<%=txtDadosResponsavel.ClientID %>');
            txtDadosObs = document.getElementById('<%=txtDadosObs.ClientID %>');
            txtDadosDuracao = document.getElementById('<%=txtDadosDuracao.ClientID %>');
            txtDadosVelocidade = document.getElementById('<%=txtDadosVelocidade.ClientID %>');
            lnkRemoverRestricao = document.getElementById('<%=lnkRemoverRestricao.ClientID %>');

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1;

            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd
            }
            if (mm < 10) {
                mm = '0' + mm
            }
            var today = dd + '/' + mm + '/' + yyyy;

            var intervalo = new Date();
            var hour = intervalo.getHours();
            var minute = intervalo.getMinutes();

            if (hour < 10) {
                hour = '0' + hour
            }
            if (minute < 10) {
                minute = '0' + minute
            }

            var tempo = hour + ":" + minute

            if (SelectedText == '038') {
                lnkRemoverRestricao.disabled = true;
            }


            if (SelectedText == 'VR' || SelectedText == 'IF') {
                ddlDadosSecoes.disabled = false;
                ddlDadosTipoRestricao.disabled = false;
                ddlDadosSubTipoVR.disabled = false;
                txtDadosDataInicial.disabled = false;
                txtDadosHoraInicial.disabled = false;
                txtDadosDataFinal.disabled = false;
                txtDadosHoraFinal.disabled = false;
                txtDadosKm_Inicio.disabled = false;
                txtDadosKm_Final.disabled = false;
                txtDadosResponsavel.disabled = false;
                txtDadosObs.disabled = false;

                if (SelectedText == 'VR') {
                    document.getElementById('<%=txtDadosVelocidade.ClientID %>').value = 'VR';
                    document.getElementById('<%=txtDadosDataInicial.ClientID %>').value = today;
                    document.getElementById('<%=txtDadosHoraInicial.ClientID %>').value = tempo;
                    document.getElementById('<%=txtDadosDataFinal.ClientID %>').value = today;
                }
                else {
                    document.getElementById('<%=txtDadosVelocidade.ClientID %>').value = 'IF';
                    document.getElementById('<%=txtDadosDataInicial.ClientID %>').value = today;
                    document.getElementById('<%=txtDadosHoraInicial.ClientID %>').value = tempo;
                    document.getElementById('<%=txtDadosDataFinal.ClientID %>').value = today;
                }
                txtDadosDuracao.disabled = true;
                txtDadosVelocidade.disabled = true;

                document.getElementById('<%=txtDadosDuracao.ClientID %>').value = '';
            }
            else {
                ddlDadosSecoes.disabled = false;
                ddlDadosTipoRestricao.disabled = false;
                txtDadosDuracao.disabled = false;
                txtDadosKm_Inicio.disabled = false;
                txtDadosKm_Final.disabled = false;
                txtDadosVelocidade.disabled = false;
                txtDadosObs.Enabled = false;

                ddlDadosSubTipoVR.disabled = true;
                txtDadosDataInicial.disabled = true;
                txtDadosHoraInicial.disabled = true;
                txtDadosDataFinal.disabled = true;
                txtDadosHoraFinal.disabled = true;
                txtDadosResponsavel.disabled = true;

                document.getElementById('<%=txtDadosDataInicial.ClientID %>').value = '';
                document.getElementById('<%=txtDadosHoraInicial.ClientID %>').value = '';
                document.getElementById('<%=txtDadosDataFinal.ClientID %>').value = '';
                document.getElementById('<%=txtDadosHoraFinal.ClientID %>').value = '';
                document.getElementById('<%=txtDadosVelocidade.ClientID %>').value = '';
                document.getElementById('<%=txtDadosResponsavel.ClientID %>').value = '';
                document.getElementById('<%=ddlDadosSubTipoVR.ClientID %>').value = '';
                document.getElementById('<%=ddlDadosSubTipoVR.ClientID %>').textContent = '';
            }
        }
        function editaRestricao(invoker) {
            var inputElements = document.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }

        function validaFormulario() {
            var retorno = true;
            var msg = "O(s) campo(s) abaixo é(são) obrigatório(s), gentileza preencher o(s) mesmo(s).  \n\n";
            var item = [];


            ddlDadosSecoes = document.getElementById('<%=ddlDadosSecoes.ClientID %>').value;
            ddlDadosTipoRestricao = document.getElementById('<%=ddlDadosTipoRestricao.ClientID %>').value;
            ddlDadosSubTipoVR = document.getElementById('<%=ddlDadosSubTipoVR.ClientID %>').value;
            txtDadosDataInicial = document.getElementById('<%=txtDadosDataInicial.ClientID %>').value;
            txtDadosHoraInicial = document.getElementById('<%=txtDadosHoraInicial.ClientID %>').value;
            txtDadosDataFinal = document.getElementById('<%=txtDadosDataFinal.ClientID %>').value;
            txtDadosHoraFinal = document.getElementById('<%=txtDadosHoraFinal.ClientID %>').value;
            txtDadosKm_Inicio = document.getElementById('<%=txtDadosKm_Inicio.ClientID %>').value;
            txtDadosKm_Final = document.getElementById('<%=txtDadosKm_Final.ClientID %>').value;
            txtDadosResponsavel = document.getElementById('<%=txtDadosResponsavel.ClientID %>').value;
            txtDadosObs = document.getElementById('<%=txtDadosObs.ClientID %>').value;
            txtDadosDuracao = document.getElementById('<%=txtDadosDuracao.ClientID %>').value;
            txtDadosVelocidade = document.getElementById('<%=txtDadosVelocidade.ClientID %>').value;

            if (ddlDadosTipoRestricao == '26' || ddlDadosTipoRestricao == '27') {

                if (ddlDadosSecoes == 'Selecione' || ddlDadosSecoes == '0') {
                    msg += "SEÇÃO; \n";
                    item += "ddlDadosSecoes";
                    retorno = false;
                }
                if (ddlDadosSubTipoVR == 'Selecione' || ddlDadosSubTipoVR == '0') {
                    msg += "SUBTIPO VR; \n";
                    if (item.length > 0) item += ":ddlDadosSubTipoVR"; else item += "ddlDadosSubTipoVR";
                    retorno = false;
                }
                if (txtDadosDataInicial == '') {
                    msg += "DATA INICIAL; \n";
                    if (item.length > 0) item += ":txtDadosDataInicial"; else item += "txtDadosDataInicial";
                    retorno = false;
                }
                if (txtDadosHoraInicial == '') {
                    msg += "HORA INICIAL; \n";
                    if (item.length > 0) item += ":txtDadosHoraInicial"; else item += "txtDadosHoraInicial";
                    retorno = false;
                }
                if (txtDadosDataFinal == '') {
                    msg += "DATA FINAL; \n";
                    if (item.length > 0) item += ":txtDadosDataFinal"; else item += "txtDadosDataFinal";
                    retorno = false;
                }
                if (txtDadosHoraFinal == '') {
                    msg += "HORA FINAL; \n";
                    if (item.length > 0) item += ":txtDadosHoraFinal"; else item += "txtDadosHoraFinal";
                    retorno = false;
                }
                if (txtDadosKm_Inicio == '') {
                    msg += "KM INICIAL; \n";
                    if (item.length > 0) item += ":txtDadosKm_Inicio"; else item += "txtDadosKm_Inicio";
                    retorno = false;
                }
                if (txtDadosKm_Final == '') {
                    msg += "KM FINAL; \n";
                    if (item.length > 0) item += ":txtDadosKm_Final"; else item += "txtDadosKm_Final";
                    retorno = false;
                }
                if (txtDadosResponsavel == '') {
                    msg += "RESPONSÁVEL; \n";
                    if (item.length > 0) item += ":txtDadosResponsavel"; else item += "txtDadosResponsavel";
                    retorno = false;
                }
                if (txtDadosObs == '') {
                    msg += "OBSERVAÇÃO; \n";
                    if (item.length > 0) item += ":txtDadosObs"; else item += "txtDadosObs";
                    retorno = false;
                }
            }
            else {
                if (ddlDadosSecoes == 'Selecione' || ddlDadosSecoes == '0') {
                    msg += "SEÇÃO; \n";
                    item += "ddlDadosSecoes";
                    retorno = false;
                }
                if (ddlDadosTipoRestricao == 'Selecione' || ddlDadosTipoRestricao == '0') {
                    msg += "TIPO DA RESTRIÇÃO; \n";
                    if (item.length > 0) item += ":ddlDadosTipoRestricao"; else item += "ddlDadosTipoRestricao";
                    retorno = false;
                }
                if (txtDadosDuracao == '') {
                    msg += "DURAÇÃO; \n";
                    if (item.length > 0) item += ":txtDadosDuracao"; else item += "txtDadosDuracao";
                    retorno = false;
                }
                if (txtDadosKm_Inicio == '') {
                    msg += "KM INICIAL; \n";
                    if (item.length > 0) item += ":txtDadosKm_Inicio"; else item += "txtDadosKm_Inicio";
                    retorno = false;
                }
                if (txtDadosKm_Final == '') {
                    msg += "KM FINAL; \n";
                    if (item.length > 0) item += ":txtDadosKm_Final"; else item += "txtDadosKm_Final";
                    retorno = false;
                }
                if (txtDadosVelocidade == '') {
                    msg += "VELOCIDADE; \n";
                    if (item.length > 0) item += ":txtDadosVelocidade"; else item += "txtDadosVelocidade";
                    retorno = false;
                }
                if (txtDadosObs == '') {
                    msg += "OBSERVAÇÃO; \n";
                    if (item.length > 0) item += ":txtDadosObs"; else item += "txtDadosObs";
                    retorno = false;
                }
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
            if (confirm("Deseja realmente remover as restrições selecionadas?")) {
                confirm_value.value = "true";
            } else {
                confirm_value.value = "false";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
</head>
<body onkeydown="tecla()">
    <form id="formRestricoes" runat="server">
        <style>
            .tipo-CC {
                color: black;
            }
            .tipo-PP {
                color: black;
            }
            .tipo-PC {
                color: rgb(46,139,87);
            }
            .situacao-E {
                color: black;
                background-color: white;
            }

            .situacao-P {
                color: black;
                background-color: yellow;
            }

            .cabeca {
                color: blue;
                font-family: 'Arial Rounded MT';
                font-size: 8px;
                font-weight: bold;
            }

            .cabeca {
                vertical-align: text-top;
                margin: 10px 0px 10px 0px;
            }

            .linha {
                vertical-align: text-top;
            }

            .Processando {
                width: 100%;
                height: 100%;
                top: 0;
                left: 0;
                position: absolute;
                background-color: whitesmoke;
                filter: alpha(opacity=80);
                opacity: 0.8;
            }

            .Texto_Processando {
                position: absolute;
                top: 50%;
                left: 50%;
                margin-top: -50px;
                margin-left: -50px;
            }
        </style>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel runat="server" Style="margin-top: 1%; margin-left: 4%; margin-right: 4%;">
            <div>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 79%;">
                            <div class="alert alert-success">
                                <h2>
                                    <asp:Label ID="lblTitulo" runat="server" Text="Restrições" Font-Size="20px" Style="color: rgb(0, 100, 0);" />&nbsp;</h2>
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
            <div>
                <table style="width: 100%; background-color: red;">
                    <tr>
                        <td style="width: 100%; font-size: 25pt; text-align: center">
                            <asp:Label runat="server" Text="As restrições do tipo VR serão retiradas automaticamente de acordo com a data e hora da programação! Fique Atento ao preecher os campos!" Font-Size="20" ForeColor="White" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="well well-sm" style="margin-bottom: 1%;">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <a id="link2" data-toggle="collapse" title="Restrição F8" data-parent="#macros" href="macros#Restricao" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Criar Restrição</b> <i class="fa fa-search"></i></a>
                    </div>
                </div>
                <div id="Restricao">
                    <table class="nav-justified" style="width: 100%">
                        <tr>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Seção&nbsp;&nbsp;</td>
                            <td style="text-align: left; vertical-align: central;" colspan="3">
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 40%;">
                                                    <asp:DropDownList ID="ddlDadosSecoes" runat="server" CssClass="form-control" Width="100%" OnSelectedIndexChanged="ddlDadosSecoes_SelectedIndexChanged" AutoPostBack="true" ToolTip="Informe a Seção onde será colocada a restrição. " />
                                                </td>
                                                <td style="vertical-align: top; padding-top: 10px;">&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label runat="server" ID="lblMensagem" Text="&nbsp;&nbsp;" Font-Size="14.5px" ForeColor="Red" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Tipo da Restrição&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:DropDownList ID="ddlDadosTipoRestricao" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDadosTipoRestricao_SelectedIndexChanged" AutoPostBack="true" Width="100%" ToolTip="Informe o tipo de restrição." />
                            </td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                        </tr>
                        <tr>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Duração (Min.)&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosDuracao" Width="100%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" MaxLength="5" ToolTip="Informe a duração da restrição." /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Subtipo VR&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:DropDownList runat="server" ID="ddlDadosSubTipoVR" Width="100%" CssClass="form-control" ToolTip="Informe o subtipo da restrição." /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                        </tr>
                        <tr style="vertical-align: top;">
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Data Inicio&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosDataInicial" Width="100%" CssClass="form-control" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Informe a data inicial da restrição." MaxLength="10" /></td>
                            <td style="width: 07%; text-align: right; vertical-align: top; padding-top: 10px;">Hora Inicio&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosHoraInicial" Width="50%" CssClass="form-control" onKeyUp="formatar(this, '##:##')" onkeypress="return fnValidaNroDoisPontos(event);" ToolTip="Informe a hora inicial da restrição." MaxLength="5" /></td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Data Final&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosDataFinal" Width="100%" CssClass="form-control" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" ToolTip="Informe a data final da restrição." MaxLength="10" /></td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Hora Final&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosHoraFinal" Width="50%" CssClass="form-control" onKeyUp="formatar(this, '##:##')" onkeypress="return fnValidaNroDoisPontos(event);" ToolTip="Informe a hora final da restrição." MaxLength="5" /></td>
                        </tr>
                        <tr>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Km Inicio&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosKm_Inicio" Width="100%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" MaxLength="8" ToolTip="Informe o KM inicial da restrição." /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Km Final&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosKm_Final" Width="100%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" MaxLength="8" ToolTip="Informe o KM final da restrição." /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                        </tr>
                        <tr>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Velocidade&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosVelocidade" Width="100%" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" MaxLength="7" ToolTip="Informe a velocidade máxima permitida na seção." /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Responsável&nbsp;&nbsp;</td>
                            <td style="width: 15%; text-align: left; vertical-align: central;">
                                <asp:TextBox runat="server" ID="txtDadosResponsavel" Width="100%" CssClass="form-control" ToolTip="Informe o responsável pela restrição" MaxLength="10" /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                        </tr>
                        <tr>
                            <td style="width: 10%; text-align: right; vertical-align: top; padding-top: 10px;">Observação&nbsp;&nbsp;</td>
                            <td style="text-align: left; vertical-align: central;" colspan="5">
                                <asp:TextBox runat="server" ID="txtDadosObs" Width="100%" CssClass="form-control" ToolTip="Informe as observações necessárias sobre a restrição a ser colocada na seção." MaxLength="36" /></td>
                            <td style="width: 07%; text-align: right; vertical-align: central;">&nbsp;</td>
                            <td style="width: 10%; text-align: left; vertical-align: central;"></td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                    <table class="nav-justified" style="width: 100%; text-align: center;">
                        <tr>
                            <td>
                                <asp:LinkButton runat="server" ID="lnkCriarRestricao" CssClass="btn btn-success" Text="Criar" OnClick="lnkCriarRestricao_Click" OnClientClick="javascript:return validaFormulario();" ToolTip="Envia uma solicitação de criação de restrição para o ACT."><i class="fa fa-plus"></i>&nbsp;Criar</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkProrrogarDataFinal" CssClass="btn btn-success" Text="Prorrogar Data Final" ToolTip="Prorroga data final de restrições."><i class="fa fa-calendar"></i>&nbsp;Prorrogar Data Final</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkDadosLimpar" CssClass="btn btn-success" OnClick="lnkDadosLimpar_Click" ToolTip="Limpa dados do formulário de criação de restrição."><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkAtualizarLista" CssClass="btn btn-default" OnClick="lnkAtualizarLista_Click" ToolTip="Atualiza lista de restrições."><i class="fa fa-refresh"></i>&nbsp;Atualizar Lista</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkRestricoesPorData" CssClass="btn btn-info" Text="Restrições por Data" OnClick="lnkRestricoesPorData_Click" ToolTip="Emite relatório de restrições por intervalo de datas."><i class="fa fa-calendar"></i>&nbsp;Restrições por Data</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkRestricoesVigentes" CssClass="btn btn-info" Text="Restrições Vigentes" OnClick="lnkRestricoesVigentes_Click" ToolTip="Emite relatório das restrições vigentes."><i class="fa fa-retweet"></i>&nbsp;Restrições Vigentes</asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton runat="server" ID="lnkRestricoesDeTemperatura" CssClass="btn btn-info" Text="Restrições de Temperatura" OnClick="lnkRestricoesDeTemperatura_Click" ToolTip="Emite relatório de restrições de temperatura."><i class="fa fa-fire"></i>&nbsp;Restrições de Temperatura</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="well well-sm" style="margin-bottom: 1%;">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <a id="link1" data-toggle="collapse" title="Filtros F9" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                    </div>
                </div>
                <div id="filtros" class="collapse">
                    <table>
                        <tr>
                            <td style="width: 160px;">
                                <label for="grupo">Seção:</label><br />
                                <asp:DropDownList runat="server" ID="ddlFiltroSB" Width="150" CssClass="form-control" ToolTip="Informe a seção a ser filtrada." />
                            </td>
                            <td style="width: 160px;">
                                <label for="grupo">Nº Restrição:</label><br />
                                <asp:TextBox runat="server" ID="txtFiltroNumeroRestricao" Width="150" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" ToolTip="Informe o número da restrição a ser filtrada." />
                            </td>
                            <td style="width: 120px;">
                                <label for="grupo">Km Inicial:</label><br />
                                <asp:TextBox runat="server" ID="txtFiltroKm_Inicial" Width="110" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" ToolTip="Informe o KM inicial a ser filtrado." />
                            </td>
                            <td style="width: 120px;">
                                <label for="grupo">Km Final:</label><br />
                                <asp:TextBox runat="server" ID="txtFiltroKm_Final" Width="110" CssClass="form-control" onkeypress="return fnValidaNroVirgula(event);" ToolTip="Informe o KM final a ser filtrado." />
                            </td>
                            <td style="width: 210px;">
                                <label for="grupo">Observação:</label><br />
                                <asp:TextBox runat="server" ID="txtFiltroObs" Width="200" CssClass="form-control" ToolTip="Informe toda ou parte da observação que pretende filtrar." />
                            </td>
                            <td style="width: 160px;">
                                <label for="grupo">Tipo Restrição:</label><br />
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlFiltroTipo" runat="server" CssClass="form-control" Width="150" ToolTip="Informe o tipo da restrição a ser filtrada." />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px;">
                                <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" Text="Pesquisar" OnClick="lnkFiltroPesquisar_Click" ToolTip="Pesquisa restrições conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>

                            </td>
                            <td style="width: 160px;">
                                <asp:LinkButton runat="server" ID="lnkFiltroLimpar" CssClass="btn btn-primary" Text="Limpar" OnClick="lnkFiltroLimpar_Click" ToolTip="Limpa dados do filtro de pesquisa e atualiza lista de restrições." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
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
            <div class="row" style="margin-top: 1%; margin-bottom: 1%;">
                <div class="form-group col-xs-12 table-responsive">
                    <table class="nav-justified" style="width: 100%">
                        <tr>
                            <td>
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Visible="true" Enabled="true" Height="300px">
                                    <asp:Repeater ID="rptListaRestricoes" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 2%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><span><i class="fa fa-check-square-o"></i></span></th>
                                                        <th style="width: 2%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><span><i class="fa fa-search-plus"></i></span></th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">Nº REST.</a></th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">TIPO</a></th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">SUBTIPO</a></th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">SB</a></th>
                                                        <th style="width: 12%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">DATA INICIAL</a></th>
                                                        <th style="width: 12%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">DATA FINAL</a></th>
                                                        <th style="width: 7%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">VELOCIDADE</a></th>
                                                        <th style="width: 7%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">KM INCIAL</a></th>
                                                        <th style="width: 5%; text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);"><a href="#">KM FINAL</a></th>
                                                        <th style="width: 50%; text-align: center; font-size: 12pt; "><a href="#">OBS</a></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; margin-top: 15px;" class="situacao-<%# Eval("Situacao")%> ">
                                                <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" visible='<%# Eval("Tipo_Restricao").ToString() != "038" ? true : false %>'>
                                                    <div>
                                                        <asp:HiddenField ID="HiddenField1" Value='<%# Eval("Tipo") +":"+ Eval("ProgramadaID") +":"+ Eval("CirculacaoID") +":"+ Eval("Secao_Elemento") +":"+ Eval("Tipo_Restricao") %>' runat="server" />
                                                        <asp:CheckBox runat="server" ID="chkRestricao" ToolTip="Seleciona a restrição atual." />
                                                    </div>
                                                </td>
                                                <td style="width: 2%; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                    <asp:LinkButton ID="lnkEdite" runat="server" OnClick="lnkEdite_Click" CommandArgument='<%# Eval("Tipo") +":"+ Eval("ProgramadaID") +":"+ Eval("CirculacaoID") +":"+ Eval("Situacao") %>' ToolTip="Exibe os dados da restrição selecionada no formulário acima."><i class="fa fa-search-plus"></i></asp:LinkButton>
                                                </td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 5%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("RestricaoID")%>"><%# Eval("RestricaoID")%> </td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 5%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo_Restricao")%>"><%# Eval("Tipo_Restricao")%> </td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 5%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("SubTipo_VR")%>"><%# Eval("SubTipo_VR")%> </td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 5%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Secao_Elemento")%>"><%# Eval("Secao_Elemento")%> </td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 12%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Data_Inicial") %>"><%# Eval ("Data_Inicial")%></td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 12%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Data_Final")%>"><%# Eval ("Data_Final")%></td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 7%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Velocidade")%>"><%# Eval ("Velocidade")%></td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 7%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Km_Inicial")%>"><%# Eval ("Km_Inicial")%></td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 5%; text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Km_Final")%>"><%# Eval ("Km_Final")%></td>
                                                <td class="tipo-<%# Eval("Tipo")%>" style="width: 50%; text-align: left; " title="<%# Eval ("Observacao")%>"><%# Eval ("Observacao")%></td>
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
            </div>
            <div class="row" style="margin-top: 1%; margin-bottom: 1%;">
                <table style="width: 100%; text-align: left;">
                    <tr>
                        <td>&nbsp;&nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkRemoverRestricao" CssClass="btn btn-success" Text="Remover Restições Selecionadas" OnClientClick="Confirm()" OnClick="lnkRemoverRestricao_Click" ToolTip="Remove restrições selecionadas."><i class="fa fa-minus-circle"></i>&nbsp;Remover Restições Selecionadas</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkRemoverRonda" CssClass="btn btn-success" Text="Remover Ronda" OnClick="lnkRemoverRonda_Click" ToolTip="Remove restrição de ronda."><i class="fa fa-chain-broken"></i>&nbsp;Remover Ronda</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton runat="server" ID="lnkAtualizaLista" CssClass="btn btn-default" OnClick="lnkAtualizarLista_Click" ToolTip="Atualiza lista de restrições."><i class="fa fa-refresh"></i>&nbsp;Atualizar Lista</asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <br />
            </div>
        </asp:Panel>
        <br />
        <div class="footer-lf-popup">
            <span>desenvolvido por </span>
            <a href="http://lfsistemas.net.br/" target="_blank" class="lfslogo-popup"></a>
        </div>
    </form>
</body>
</html>
