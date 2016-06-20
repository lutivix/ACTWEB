<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTHP_Relatorios.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.popupTHP_Relatorios" Async="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html lang="pt-br" style="width: 100%; height: 100%; overflow: scroll;">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <title>ACTWEB</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">

    <link rel="stylesheet" type="text/css" href="/js/extensions/FixedHeader/css/dataTables.fixedHeader.css" />
    <link rel="stylesheet" type="text/css" href="/fonts/fontawesome-webfont.ttf" />
    <link rel="stylesheet" type="text/css" href="/fonts/FontAwesome.eot" />

    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery.dataTables_themeroller.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui.css" />
    <link rel="stylesheet" type="text/css" href="/css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" type="text/css" href="/css/main.css" />

    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>

    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="/js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="/js/jquery-ui.js"></script>
    <script type="text/javascript" src="/js/jquery-ui-timepicker-addon.js"></script>
    <link rel="grupo vli" href="logo-vli.ico">
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="36000" />
        <script type="text/javascript">
            $(document).keydown(function (e) {
                if (e.which == 120) {
<%--                $('<%=lnkPesquisar.ClientID %>').click();
                document.getElementById('<%=lnkPesquisar.ClientID %>').click();--%>
                    e.preventDefault();
                }
            });

            function selectAllCorredores(invoker) {
                var divControll = document.getElementById('dvCorredores');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }

            function selectAllTrechos(invoker) {
                var divControll = document.getElementById('dvTrechos');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }

            function selectAllRotas(invoker) {
                var divControll = document.getElementById('dvRotas');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }

            function selectAllSubRotas(invoker) {
                var divControll = document.getElementById('dvSubRotas');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }
            function selectAllGrupos(invoker) {
                var divControll = document.getElementById('dvGrupos');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }
            function selectAllMotivos(invoker) {
                var divControll = document.getElementById('dvMotivos');
                var inputElements = divControll.getElementsByTagName('input');
                for (var i = 0; i < inputElements.length; i++) {
                    var myElement = inputElements[i];
                    if (myElement.type === "checkbox") {
                        myElement.checked = invoker.checked;
                    }
                }
            }

            function Get_Selected_Value() {
                var ControlRef = document.getElementById('<%= cblColunas.ClientID %>');
                var CheckBoxListArray = ControlRef.getElementsByTagName('input');
                var spanArray = ControlRef.getElementsByTagName('span');
                var conta = 0;

                for (var i = 0; i < CheckBoxListArray.length; i++) {
                    var checkBoxRef = CheckBoxListArray[i];

                    var labelArray = checkBoxRef.parentNode.getElementsByTagName('label');

                    if (checkBoxRef.checked == true) {
                        conta++;
                        if (labelArray.length > 0) {


                            if (labelArray[0].innerHTML == "Data") { document.getElementById('th_01').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Corredor") { document.getElementById('th_02').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Rota") { document.getElementById('th_03').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "SubRota") { document.getElementById('th_04').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Classe") { document.getElementById('th_05').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "OS") { document.getElementById('th_06').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Prefixo") { document.getElementById('th_07').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Grupo") { document.getElementById('th_08').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Motivo") { document.getElementById('th_09').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "SB") { document.getElementById('th_10').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "THP") { document.getElementById('th_11').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "TTP") { document.getElementById('th_12').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "THM") { document.getElementById('th_13').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração THP") { document.getElementById('th_14').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração TTP") { document.getElementById('th_15').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração THM") { document.getElementById('th_16').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "TTT") { document.getElementById('th_17').style.display = 'block'; }

                            if (labelArray[0].innerHTML == "Data") { document.getElementById('td_01').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Corredor") { document.getElementById('td_02').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Rota") { document.getElementById('td_03').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "SubRota") { document.getElementById('td_04').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Classe") { document.getElementById('td_05').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "OS") { document.getElementById('td_06').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Prefixo") { document.getElementById('td_07').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Grupo") { document.getElementById('td_08').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Motivo") { document.getElementById('td_09').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "SB") { document.getElementById('td_10').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "THP") { document.getElementById('td_11').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "TTP") { document.getElementById('td_12').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "THM") { document.getElementById('td_13').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração THP") { document.getElementById('td_14').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração TTP") { document.getElementById('td_15').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "Duração THM") { document.getElementById('td_16').style.display = 'block'; }
                            if (labelArray[0].innerHTML == "TTT") { document.getElementById('td_17').style.display = 'block'; }

                        }
                    }
                    else {
                        conta--;
                        if (labelArray.length > 0) {

                            if (labelArray[0].innerHTML == "Data") { document.getElementById('th_01').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Corredor") { document.getElementById('th_02').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Rota") { document.getElementById('th_03').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "SubRota") { document.getElementById('th_04').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Classe") { document.getElementById('th_05').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "OS") { document.getElementById('th_06').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Prefixo") { document.getElementById('th_07').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Grupo") { document.getElementById('th_08').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Motivo") { document.getElementById('th_09').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "SB") { document.getElementById('th_10').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "THP") { document.getElementById('th_11').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "TTP") { document.getElementById('th_12').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "THM") { document.getElementById('th_13').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração THP") { document.getElementById('th_14').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração TTP") { document.getElementById('th_15').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração THM") { document.getElementById('th_16').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "TTT") { document.getElementById('th_17').style.display = 'none'; }

                            if (labelArray[0].innerHTML == "Data") { document.getElementById('td_01').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Corredor") { document.getElementById('td_02').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Rota") { document.getElementById('td_03').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "SubRota") { document.getElementById('td_04').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Classe") { document.getElementById('td_05').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "OS") { document.getElementById('td_06').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Prefixo") { document.getElementById('td_07').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Grupo") { document.getElementById('td_08').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Motivo") { document.getElementById('td_09').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "SB") { document.getElementById('td_10').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "THP") { document.getElementById('td_11').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "TTP") { document.getElementById('td_12').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "THM") { document.getElementById('td_13').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração THP") { document.getElementById('td_14').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração TTP") { document.getElementById('td_15').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "Duração THM") { document.getElementById('td_16').style.display = 'none'; }
                            if (labelArray[0].innerHTML == "TTT") { document.getElementById('td_17').style.display = 'none'; }

                            //if (labelArray[0].innerHTML == "Data") { document.getElementById('td_01').style.display = 'none'; }

                        }
                    }
                }

                if (conta < 1) conta = 1;
                if (conta > 17) conta = 17;

                //document.getElementById('thTitulo').colSpan = conta;
            }

            function verificar(nome, quantidade) {
                saida = "Os checkboxes checados são:";
                // itera baseado na quantidade de elementos
                for (i = 0; i < quantidade; i++) {
                    // obtém cada elemento pelo id
                    checkBox = document.getElementById(nome + (i + 1));
                    // se o checkbox estiver marcado, adiciona mais uma linha na string de saida.
                    if (checkBox.checked) {
                        saida += "\n" + checkBox.value;
                    }
                }
                // mostra a saída
                alert(saida);
            }


            function toTop() {
                $('html, body').animate({
                    scrollTop: 0
                }, 1000, 'linear');
            }
        </script>
        <style>
            .status-branco {
                color: rgb(000, 000, 000); /* Preto */
                background-color: rgb(255, 255, 255); /* Branco */
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

            .cabecalho1 {
                width: 100%;
                text-align: justify;
                vertical-align: bottom;
            }

            .cabecalho2 {
                width: 100%;
                text-align: justify;
                vertical-align: bottom;
                margin: 0px;
            }

            .celula_pri {
                float: left;
                width: 5.00%;
                height: 50px;
                font-size: 10pt;
                text-align: center;
                vertical-align: bottom;
                color: white;
                background-color: rgb(55, 119, 188);
                border-left: 1px solid rgb(0, 72, 89);
                border-top: 1px solid rgb(0, 72, 89);
                border-right: 1px solid rgb(0, 72, 89);
            }

            .celula_cen {
                float: left;
                width: 5.00%;
                height: 50px;
                font-size: 10pt;
                text-align: center;
                vertical-align: bottom;
                color: white;
                background-color: rgb(55, 119, 188);
                border-left: 0.5px solid rgb(0, 72, 89);
                border-top: 1px solid rgb(0, 72, 89);
                border-right: 0.5px solid rgb(0, 72, 89);
            }

            .celula_ult {
                float: left;
                width: 5.00%;
                height: 50px;
                font-size: 10pt;
                text-align: center;
                vertical-align: middle;
                color: white;
                background-color: rgb(55, 119, 188);
                border-left: 1px solid rgb(0, 72, 89);
                border-top: 1px solid rgb(0, 72, 89);
                border-right: 1px solid rgb(0, 72, 89);
            }
        </style>
        <div style="margin: 20px;">
            <asp:UpdatePanel runat="server" ID="upRegistros">
                <ContentTemplate>
                    <table class="nav-justified">
                        <tr>
                            <td style="width: 79%; text-align: left;">
                                <div class="alert alert-success">
                                    <h2>
                                        <asp:Image runat="server" ImageUrl="/img/thp-b.png" />
                                        <asp:Label ID="Label1" runat="server" Text="Consulta de THP" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
                    <div class="well well-sm">
                        <div class="page-header sub-content-header">
                            <%--<h2>Filtros de Pesquisa</h2>--%>
                            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                        </div>
                        <div id="filtros">
                            <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                                <tr>
                                    <td style="width: 13.4%; padding-top: 1em;">
                                        <div runat="server" id="dvPeriodo1" visible="true">
                                            <table>
                                                <tr>
                                                    <td style="width: 50%">
                                                        <label for="De">De:</label>
                                                        <asp:TextBox runat="server" ID="txtFiltroDataDe" CssClass="form-control" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                                                    </td>
                                                    <td style="width: 50%">
                                                        <label for="Ate">Até:</label>
                                                        <asp:TextBox runat="server" ID="txtFiltroDataAte" CssClass="form-control" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td style="width: 6.6%; padding-top: 1em;">
                                        <label for="Inicio">Classe:</label>
                                        <asp:TextBox runat="server" ID="txtFiltroClasse" CssClass="form-control" Width="95%" />
                                    </td>
                                    <td style="width: 10%; padding-top: 1em;" rowspan="3">
                                        <label for="matricula">Corredores:</label>
                                        <asp:CheckBox runat="server" ID="chkCorredores" OnClick="selectAllCorredores(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkCorredores_CheckedChanged" AutoPostBack="true" />
                                        <div id="dvCorredores">
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblCorredores" SelectionMode="Multiple" OnSelectedIndexChanged="cblCorredores_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 13%; padding-top: 1em;" rowspan="3" hidden="hidden">
                                        <label for="matricula">Trechos:</label>
                                        <asp:CheckBox runat="server" ID="chkTrechos" OnClick="selectAllTrechos(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkTrechos_CheckedChanged" AutoPostBack="true" />
                                        <div id="dvTrechos">
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblTrechos" SelectionMode="Multiple" OnSelectedIndexChanged="cblTrechos_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 10%; padding-top: 1em;" rowspan="3">
                                        <label for="matricula">Rotas:</label>
                                        <asp:CheckBox runat="server" ID="chkRotas" OnClick="selectAllRotas(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkRotas_CheckedChanged" AutoPostBack="true" />
                                        <div id="dvRotas">
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblRotas" SelectionMode="Multiple" OnSelectedIndexChanged="cblRotas_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 10%; padding-top: 1em;" rowspan="3">
                                        <label for="matricula">SubRotas</label>
                                        <asp:CheckBox runat="server" ID="chkSubRotas" OnClick="selectAllSubRotas(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkSubRotas_CheckedChanged" AutoPostBack="true" />
                                        <div id="dvSubRotas">
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblSubRotas" SelectionMode="Multiple" OnSelectedIndexChanged="cblSubRotas_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 13%; padding-top: 1em;" rowspan="3">
                                        <label for="matricula">Grupos:</label>
                                        <asp:CheckBox runat="server" ID="chkGrupos" OnClick="selectAllGrupos(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkGrupos_CheckedChanged" AutoPostBack="true" />
                                        <div id="dvGrupos">
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblGrupos" SelectionMode="Multiple" OnSelectedIndexChanged="cblGrupos_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 20%; padding-top: 1em;" rowspan="3">
                                        <div id="dvMotivos">
                                            <label for="matricula">Motivos:</label>
                                            <asp:CheckBox runat="server" ID="chkMotivos" OnClick="selectAllMotivos(this)" ToolTip="Seleciona Todos" OnCheckedChanged="chkMotivos_CheckedChanged" AutoPostBack="true" />
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblMotivos" SelectionMode="Multiple" OnSelectedIndexChanged="cblMotivos_SelectedIndexChanged" AutoPostBack="true" />
                                            </asp:Panel>
                                        </div>
                                    </td>
                                    <td style="width: 10%; padding-top: 1em;" rowspan="3">
                                        <div id="dvColunas">
                                            <label for="matricula">Colunas:</label>
                                            <asp:CheckBox runat="server" ID="chkColunas" ToolTip="Seleciona Todos" />
                                            <asp:Panel runat="server" Width="95%" Height="140" ScrollBars="Vertical" CssClass="form-control">
                                                <asp:CheckBoxList runat="server" ID="cblColunas" SelectionMode="Multiple" OnSelectedIndexChanged="cblColunas_SelectedIndexChanged" AutoPostBack="true">
                                                    <asp:ListItem Text="Data" Value="Data" Selected="True" />
                                                    <asp:ListItem Text="Corredor" Value="Corredor" Selected="True" />
                                                    <asp:ListItem Text="Rota" Value="Rota" Selected="True" />
                                                    <asp:ListItem Text="SubRota" Value="SubRota" Selected="True" />
                                                    <asp:ListItem Text="Classe" Value="Classe" Selected="True" />
                                                    <asp:ListItem Text="OS" Value="OS" Selected="True" />
                                                    <asp:ListItem Text="Prefixo" Value="Prefixo" Selected="True" />
                                                    <asp:ListItem Text="Grupo" Value="Grupo" Selected="True" />
                                                    <asp:ListItem Text="Motivo" Value="Motivo" Selected="True" />
                                                    <asp:ListItem Text="SB" Value="SB" Selected="True" />
                                                    <asp:ListItem Text="THP" Value="THP" Selected="True" />
                                                    <asp:ListItem Text="TTP" Value="TTP" Selected="True" />
                                                    <asp:ListItem Text="THM" Value="THM" Selected="True" />
                                                </asp:CheckBoxList>
                                            </asp:Panel>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%;" colspan="2">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 33%; padding-top: 1em;">
                                                    <label for="Fim">OS:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroOS" CssClass="form-control" Width="95%" />
                                                </td>
                                                <td style="width: 33%; padding-top: 1em;" colspan="1">
                                                    <label for="Inicio">Prefixo:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroPrefixo" CssClass="form-control" Width="95%" />
                                                </td>
                                                <td style="width: 33%; padding-top: 1em;" colspan="1">
                                                    <label for="Inicio">SB:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroSB" CssClass="form-control" Width="95%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 20%;" colspan="2">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 33%; padding-top: 1em;">
                                                    <asp:RadioButton runat="server" ID="rbAnalitica" Text="Analítica" TextAlign="Right" Width="95%" CssClass="form-control" GroupName="gmTHP" Checked="true" OnCheckedChanged="rbAnalitica_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td style="width: 33%; padding-top: 1em;" colspan="1">
                                                    <asp:RadioButton runat="server" ID="rbConsolidada" Text="Consolidada" TextAlign="Right" Width="95%" CssClass="form-control" GroupName="gmTHP" OnCheckedChanged="rbConsolidada_CheckedChanged" AutoPostBack="true" />
                                                </td>
                                                <td style="width: 33%; padding-top: 1em;" colspan="1"></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 44.5%; padding-top: 1em; vertical-align: bottom;">
                                <tr>
                                    <td style="width: 50%; padding-top: 1em; vertical-align: bottom;">
                                        <label for="Inicio"></label>
                                        <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Busca as informações do banco de dados." Width="32%"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-info" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="31.5%"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkFiltrar" CssClass="btn btn-success" ToolTip="Filtra as informações" Width="32%" Visible="false"><i class="fa fa-search"></i>&nbsp;Filtrar</asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="lnkGeraExcel" CssClass="btn btn-default" OnClick="lnkGeraExcel_Click" ToolTip="Gera arquivo Excel" Width="31.5%"><i class="fa fa-plus"></i>&nbsp;Excel</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div runat="server" id="dvAnalitica" class="well well-sm" visible="true">
                        <div style="width: 100%;">
                            <table class="nav-justified">
                                <tr>
                                    <td style="width: 100%;">
                                        <asp:Repeater ID="RepeaterItensAnalitica" runat="server">
                                            <HeaderTemplate>
                                                <thead>
                                                    <div id="dvRepeaterItensAnalitica">
                                                        <table id="tbAnalitica" class="table table-hover table-curved pro-table" style="width: 100%;">
                                                            <tr style="background-color: #fff;">
                                                                <th id="th_01" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom; visibility: <%# Eval("Coluna_Data") %>;" title="Data">Data</th>
                                                                <th id="th_02" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Corredor">Corredor</th>
                                                                <th id="th_03" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Rota">Rota</th>
                                                                <th id="th_04" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="SubRota">SubRota</th>
                                                                <th id="th_05" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Classe">Classe</th>
                                                                <th id="th_06" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="OS">OS</th>
                                                                <th id="th_07" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Prefixo">Prefixo</th>
                                                                <th id="th_08" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Grupo">Grupo</th>
                                                                <th id="th_09" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Motivo">Motivo</th>
                                                                <th id="th_10" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="SB">SB</th>
                                                                <th id="th_11" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="border: 0px; background-color: rgb(55, 119, 188); color: white;" colspan="2">THP (mm)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Meta</td>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Real</td>
                                                                        </tr>
                                                                    </table>
                                                                </th>
                                                                <th id="th_12" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="border: 0px; background-color: rgb(55, 119, 188); color: white;" colspan="2">TTP (mm)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Meta</td>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Real</td>
                                                                        </tr>
                                                                    </table>
                                                                </th>
                                                                <th id="th_13" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="border: 0px; background-color: rgb(55, 119, 188); color: white;" colspan="2">THM (mm)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Meta</td>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Real</td>
                                                                        </tr>
                                                                    </table>
                                                                </th>
                                                                <th id="th_14" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Duracao_THP">Duração THP (mm) </th>
                                                                <th id="th_15" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Duracao_TTP">Duração TTP (mm) </th>
                                                                <th id="th_16" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;" title="Duracao_THM">Duração THM (mm) </th>
                                                                <th id="th_17" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; background-color: rgb(55, 119, 188); color: white; vertical-align: bottom;">
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td style="border: 0px; background-color: rgb(55, 119, 188); color: white;" colspan="2">TTT (mm)</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Meta</td>
                                                                            <td style="width: 50%; border: 0px; background-color: rgb(55, 119, 188); color: white;">Real</td>
                                                                        </tr>
                                                                    </table>
                                                                </th>
                                                            </tr>
                                                </thead>
                                                <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>

                                                <tr style="background-color: #fff;">
                                                    <td id="td_01" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); visibility: <%# Eval("Coluna_Data") %>;" title="<%# Eval("Apuracao").ToString().Substring(0,10) %>"><%# Eval("Apuracao").ToString().Substring(0,10) %></td>
                                                    <td id="td_02" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); visibility: <%# Eval("Coluna_Corredor") %>;" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%></td>
                                                    <td id="td_03" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); visibility: <%# Eval("Coluna_Rota") %>;" title="<%# Eval("Rota") %>"><%# Eval("Rota") %></td>
                                                    <td id="td_04" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); visibility: <%# Eval("Coluna_SubRota") %>;" title="<%# Eval("SubRota") %>"><%# Eval("SubRota") %></td>
                                                    <td id="td_05" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_Classe") %>;" title="<%# Eval("Classe") %>"><%# Eval("Classe")%></td>
                                                    <td id="td_06" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_OS") %>;" title="<%# Eval("OS") %>"><%# Eval("OS")%></td>
                                                    <td id="td_07" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_Prefixo") %>;" title="<%# Eval("Prefixo") %>"><%# Eval("Prefixo") %></td>
                                                    <td id="td_08" style="width: 05.00%; text-align: left; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_Grupo") %>;" title="<%# Eval("Grupo") %>"><%# Eval("Grupo")%></td>
                                                    <td id="td_09" style="width: 05.00%; text-align: left; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_Motivo") %>;" title="<%# Eval("Motivo") %>"><%# Eval("Motivo")%></td>
                                                    <td id="td_10" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_SB") %>;" title="<%# Eval("SB") %>"><%# Eval("SB")%></td>
                                                    <td id="td_11" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_THP") %>;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("THP_Meta") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("THP_Real") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="td_12" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_TTP") %>;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("TTP_Meta") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("TTP_Real") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="td_13" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle; visibility: <%# Eval("Coluna_THM") %>;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("THM_Meta") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# Eval("THM_Real") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td id="td_14" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); text-align: right; vertical-align: middle; visibility: <%# Eval("Coluna_Duracao_THP") %>; visibility: <%# Eval("zVisible") %>;" rowspan="<%# Eval("zRowspan")%>" title="<%# Eval("Duracao_THP") %> minuto(s)"> <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Duracao_THP").ToString()))) %> </td>
                                                    <td id="td_15" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); text-align: right; vertical-align: middle; visibility: <%# Eval("Coluna_Duracao_TTP") %>; visibility: <%# Eval("zVisible") %>;" rowspan="<%# Eval("zRowspan")%>" title="<%# Eval("Duracao_TTP") %> minuto(s)"> <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Duracao_TTP").ToString()))) %> </td>
                                                    <td id="td_16" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); text-align: right; vertical-align: middle; visibility: <%# Eval("Coluna_Duracao_THM") %>; visibility: <%# Eval("zVisible") %>;" rowspan="<%# Eval("zRowspan")%>" title="<%# Eval("Duracao_THM") %> minuto(s)"> <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Duracao_THM").ToString()))) %> </td>
                                                    <td id="td_17" style="width: 05.00%; text-align: center; padding-left: 05px; padding-right: 05px; text-align: right; vertical-align: middle; visibility: <%# Eval("Coluna_TTT") %>; visibility: <%# Eval("zVisible") %>;" rowspan="<%# Eval("zRowspan")%>">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 33%; border: 0px;  text-align: right;" title="<%# Eval("Total_M" )%> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_M").ToString()))) %>
                                                                </td>
                                                                <td style="width: 33%; border: 0px;  text-align: right;" title="<%# Eval("Total_R") %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_R").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                        </table>
                                                </div>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurrentPageAnalitica" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkPrimeiraPaginaAnalitica" runat="server" OnClick="lnkPrimeiraPaginaAnalitica_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                        &nbsp; 
                                        <asp:LinkButton ID="lnkPaginaAnteriorAnalitica" runat="server" OnClick="lnkPaginaAnteriorAnalitica_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                        &nbsp; Itens por página: &nbsp;
                                        <asp:DropDownList ID="ddlPageSizeAnalitica" runat="server" OnSelectedIndexChanged="ddlPageSizeAnalitica_SelectedIndexChanged" AutoPostBack="true" Width="100" CssClass="form-control-single" onchange="javascript:toTop();">
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;10" Value="10" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;20" Value="20" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;30" Value="30" Selected="True" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;40" Value="40" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;50" Value="50" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;100" Value="100" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;200" Value="200" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;300" Value="300" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;400" Value="400" />
                                            <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;500" Value="500" />
                                            <asp:ListItem Text="&nbsp;&nbsp;1.000" Value="1000" />
                                            <asp:ListItem Text="&nbsp;&nbsp;2.000" Value="2000" />
                                            <asp:ListItem Text="&nbsp;&nbsp;5.000" Value="5000" />
                                            <asp:ListItem Text="10.000" Value="10000" />
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:LinkButton ID="lnkProximaPaginaAnalitica" runat="server" OnClick="lnkProximaPaginaAnalitica_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                        &nbsp; 
                                        <asp:LinkButton ID="lnkUltimaPaginaAnalitica" runat="server" OnClick="lnkUltimaPaginaAnalitica_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <br />
                                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                        <asp:Label runat="server" ID="lblTotalAnalitica" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div runat="server" id="dvConsolida" class="well well-sm" visible="false">
                        <div style="width: 100%;">
                            <table class="nav-justified">
                                <tr>
                                    <td>
                                        <asp:Repeater ID="RepeaterItensConsolida" runat="server">
                                            <HeaderTemplate>
                                                <table class="table table-hover table-curved pro-table" style="width: 100%;">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkData" Text="Data" ForeColor="White" /></th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkCorredor" Text="Corredor" ForeColor="White" /></th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkClasse" Text="Classe" ForeColor="White" /></th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkRota" Text="Rota" ForeColor="White" /></th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <asp:LinkButton runat="server" ID="lnkSubRota" Text="SubRota" ForeColor="White" /></th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="border: 0px; background-color: rgb(55, 119, 0); color: white;" colspan="2">
                                                                            <h1>THP (h)</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Meta </td>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Real </td>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="border: 0px; background-color: rgb(55, 119, 0); color: white;" colspan="2">
                                                                            <h1>TTP (h)</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Meta </td>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Real </td>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0); border-right: 1px solid rgb(0, 72, 89);">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="border: 0px; background-color: rgb(55, 119, 0); color: white;" colspan="2">
                                                                            <h1>THM (h)</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Meta </td>
                                                                        <td style="width: 50%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Real </td>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                            <th style="width: 06%; text-align: center; font-size: 12pt; background-color: rgb(55, 119, 0);">
                                                                <table style="width: 100%;">
                                                                    <tr>
                                                                        <td style="border: 0px; background-color: rgb(55, 119, 0); color: white;" colspan="2">
                                                                            <h1>TTT (h)</h1>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 33%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Meta </td>
                                                                        <td style="width: 33%; border: 0px; font-size: 12pt; background-color: rgb(55, 119, 0); color: white;"> Real </td>
                                                                    </tr>
                                                                </table>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr style="background-color: #EEE;">
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Apuracao").ToString().Substring(0,10) %>"><%# Eval("Apuracao").ToString().Substring(0,10) %></td>
                                                    <td style="width: 06%; height: 20px; text-align: left; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Classe") %>"><%# Eval("Classe")%></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Rota") %>"><%# Eval("Rota") %></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("SubRota") %>"><%# Eval("SubRota") %></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                  <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THP_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THP_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("TTP_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("TTP_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THM_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THM_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("Total_M")) %>">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_M").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("Total_R")) %>">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_R").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr style="background-color: #FFF;">
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Apuracao").ToString().Substring(0,10) %>"><%# Eval("Apuracao").ToString().Substring(0,10) %></td>
                                                    <td style="width: 06%; height: 20px; text-align: left; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Classe") %>"><%# Eval("Classe")%></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("Rota") %>"><%# Eval("Rota") %></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;" title="<%# Eval("SubRota") %>"><%# Eval("SubRota") %></td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THP_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px; text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THP_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("TTP_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("TTP_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("TTP_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; border-right: 1px solid rgb(0, 72, 89); vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THM_Meta")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Meta").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("THM_Real")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("THM_Real").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 06%; height: 20px; text-align: center; padding-left: 05px; padding-right: 05px; vertical-align: middle;">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("Total_M")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_M").ToString()))) %>
                                                                </td>
                                                                <td style="width: 50%; border: 0px;  text-align: right;" title="<%# string.Format("{0:0,0}", Eval("Total_R")) %> minuto(s)">
                                                                    <%# string.Format("{0}", TimeSpan.FromMinutes(double.Parse(Eval("Total_R").ToString()))) %>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </AlternatingItemTemplate>
                                            <FooterTemplate>
                                                </tbody>
                                            </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCurrentPageConsolida" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkPrimeiraPaginaConsolida" runat="server" OnClick="lnkPrimeiraPaginaConsolida_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                        &nbsp; 
                                            <asp:LinkButton ID="lnkPaginaAnteriorConsolida" runat="server" OnClick="lnkPaginaAnteriorConsolida_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                            &nbsp; Itens por página: &nbsp;
                                            <asp:DropDownList ID="ddlPageSizeConsolida" runat="server" OnSelectedIndexChanged="ddlPageSizeConsolida_SelectedIndexChanged" AutoPostBack="true" Width="100" CssClass="form-control-single" onchange="javascript:toTop();">
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;10" Value="10" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;20" Value="20" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;30" Value="30" Selected="True" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;40" Value="40" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;50" Value="50" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;100" Value="100" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;200" Value="200" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;300" Value="300" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;400" Value="400" />
                                                <asp:ListItem Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;500" Value="500" />
                                                <asp:ListItem Text="&nbsp;&nbsp;1.000" Value="1000" />
                                                <asp:ListItem Text="&nbsp;&nbsp;2.000" Value="2000" />
                                                <asp:ListItem Text="&nbsp;&nbsp;5.000" Value="5000" />
                                                <asp:ListItem Text="10.000" Value="10000" />
                                            </asp:DropDownList>
                                        &nbsp;
                                            <asp:LinkButton ID="lnkProximaPaginaConsolida" runat="server" OnClick="lnkProximaPaginaConsolida_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                        &nbsp; 
                                            <asp:LinkButton ID="lnkUltimaPaginaConsolida" runat="server" OnClick="lnkUltimaPaginaConsolida_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <br />
                                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                        <asp:Label runat="server" ID="lblTotalConsolida" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRegistros">
                <ProgressTemplate>
                    <div class="Processando">
                        <table class="Texto_Processando">
                            <tr>
                                <td>
                                    <asp:Image runat="server" ID="imgProcess" ImageUrl="~/img/process.gif" Width="50" />
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblProcess" Text="Processando..." />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
</html>
