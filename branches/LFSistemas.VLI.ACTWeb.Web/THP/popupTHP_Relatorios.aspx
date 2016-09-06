<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTHP_Relatorios.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.THP.popupTHP_Relatorios" %>

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

    <script type="text/javascript">
        $(document).keydown(function (e) {
            if (e.which == 120) {
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

        function tecla() {
            if (window.event.keyCode == 27) {
                this.window.close();
            }
        }

        $(function () {
            $("#dvAccordian").accordion({
                autoHeight: false,
                active: false,
                collapsible: true
            });
        });

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            prm.add_endRequest(EndRequest);
            $("#dvAccordian").accordion({
                autoHeight: false,
                active: false,
                collapsible: true,
            });
        });

        function InitializeRequest(sender, args) {
        }

        function EndRequest(sender, args) {
            $("#dvAccordian").accordion({
                autoHeight: false,
                active: false,
                collapsible: true,
            });
        }

        function checkData(sender, args) {

            var filtro_classe           = $('#txtFiltroClasse').val();
            var filtro_os               = $('#txtFiltroOS').val();
            var filtro_prefixo          = $('#txtFiltroPrefixo').val();
            var filtro_sb               = $('#txtFiltroSB').val();
            var filtro_corredores_id    = '';
            var filtro_trechos_id       = '';
            var filtro_rotas_id         = '';
            var filtro_subrotas_id      = '';
            var filtro_grupos_id        = '';
            var filtro_motivos_id       = '';

            var data = new Date();
            data.setDate(data.getDate() + 29);
            var ddd = data.getDate();
            var mmm = data.getMonth();
            var yyy = data.getFullYear();

            if (ddd < 10) { ddd = '0' + ddd }
            if (mmm < 10) { mmm = '0' + mmm }
            var agoraDe = ddd + "/" + mmm + "/" + yyy;

            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth();
            var yy = today.getFullYear();

            if (dd < 10) { dd = '0' + dd }
            if (mm < 10) { mm = '0' + mm }
            var agoraAte = dd + "/" + mm + "/" + yy;

            var dataDe = $("#txtFiltroDataDe").val().split("/");
            var dataAte = $("#txtFiltroDataAte").val().split("/");
            var data1 = new Date(dataDe[2] + "/" + dataDe[1] + "/" + dataDe[0]);
            var data2 = new Date(dataAte[2] + "/" + dataAte[1] + "/" + dataAte[0]);

            var intervalo = Math.abs((data2 - data1) / (1000 * 60 * 60 * 24));

            $("[id*=cblCorredores] input:checked").each(function () {
                filtro_corredores_id += $(this).val();
            });
            $("[id*=cblRotas] input:checked").each(function () {
                filtro_rotas_id += $(this).val();
            });
            $("[id*=cblSubRotas] input:checked").each(function () {
                filtro_subrotas_id += $(this).val();
            });
            $("[id*=cblGrupos] input:checked").each(function () {
                filtro_grupos_id += $(this).val();
            });
            $("[id*=cblMotivos] input:checked").each(function () {
                filtro_motivos_id += $(this).val();
            });


            if (filtro_classe           == ''
                && filtro_os            == ''
                && filtro_prefixo       == ''
                && filtro_sb            == ''
                && filtro_corredores_id == ''
                && filtro_rotas_id      == ''
                && filtro_subrotas_id   == ''
                && filtro_grupos_id     == ''
                && filtro_motivos_id    == '') {

                if (intervalo > 3) {
                    alert("Para filtrar um intervalo de dadas maior que 2 dias, é obrigatório selecionar pelo menos 1 filtro na pesquisa!");
                    $('#txtFiltroDataDe').focus();
                }
            }
            else {
                if (intervalo > 30) {
                    alert("O intervalo entre as datas não pode ser superior a 30 dias!");
                    $('#txtFiltroDataDe').focus();
                }
            }
        }

        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
            if (e.which == 13) {
                $('#lnkPesquisar').click();
                document.getElementById('lnkPesquisar').click();
            }
        });

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
            z-index: 9999;
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
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="36000" EnableScriptGlobalization="true" />
        <div style="margin: 20px;">
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
                    <asp:UpdatePanel runat="server" ID="upFiltro">
                        <ContentTemplate>
                            <table style="width: 100%; padding-left: 1em; padding-right: 1em;">
                                <tr>
                                    <td style="width: 20%;" colspan="2">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 40%">
                                                    <label for="De">De:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroDataDe" CssClass="form-control" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                                                    <asp:CalendarExtender runat="server" ID="ceFiltroDataDe" TargetControlID="txtFiltroDataDe" OnClientDateSelectionChanged="checkData" />
                                                </td>
                                                <td style="width: 40%">
                                                    <label for="Ate">Até:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroDataAte" CssClass="form-control" Width="95%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" onkeypress="return PermiteSomenteNumeros(event);" />
                                                    <asp:CalendarExtender runat="server" ID="ceFiltroDataAte" TargetControlID="txtFiltroDataAte" OnClientDateSelectionChanged="checkData" />
                                                </td>
                                                <td style="width: 20%; padding-top: 1em;">
                                                    <label for="Inicio">Classe:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroClasse" CssClass="form-control" Width="95%" />
                                                </td>
                                            </tr>
                                        </table>
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
                                </tr>
                                <tr>
                                    <td style="width: 20%;" colspan="2">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 40%; padding-top: 1em;">
                                                    <label for="Fim">OS:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroOS" CssClass="form-control" Width="95%" />
                                                </td>
                                                <td style="width: 40%; padding-top: 1em;" colspan="1">
                                                    <label for="Inicio">Prefixo:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroPrefixo" CssClass="form-control" Width="95%" />
                                                </td>
                                                <td style="width: 20%; padding-top: 1em;" colspan="1">
                                                    <label for="Inicio">SB:</label>
                                                    <asp:TextBox runat="server" ID="txtFiltroSB" CssClass="form-control" Width="95%" />
                                                </td>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upFiltro">
                        <ProgressTemplate>
                            <div class="Processando">
                                <table class="Texto_Processando">
                                    <tr>
                                        <td>
                                            <asp:Image runat="server" ImageUrl="~/img/process.gif" Width="50" />
                                        </td>
                                        <td>
                                            <asp:Label runat="server" Text="Processando..." />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <asp:UpdatePanel runat="server" ID="upDados" ValidateRequestMode="Enabled" RenderMode="Block" UpdateMode="Always">
                    <ContentTemplate>
                        <div runat="server" id="dvDados" class="row" style="margin-left: 1%; margin-right: 1%; margin-bottom: 1%;">
                            <div style="margin-top: 20px;">
                                <table style="width: 1800px; font-size: 0.8em;">
                                    <tr>
                                        <td>
                                            <table style="margin: 0; padding: 0;">
                                                <tr>
                                                    <td style="width: 091px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Data</td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Corredor</td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Rota</td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">SubRota</td>
                                                    <td style="width: 071px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Classe</td>
                                                    <td style="width: 091px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">OS</td>
                                                    <td style="width: 081px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Prefixo</td>
                                                    <td style="width: 150px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Grupo</td>
                                                    <td style="width: 150px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Motivo</td>
                                                    <td style="width: 070px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">SB</td>
                                                    <td style="width: 100px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Hr Início</td>
                                                    <td style="width: 100px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Hr Final</td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">
                                                        <%--THP (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="border: 0px; color: white;" colspan="2">THP (min)</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">
                                                        <%--TTP (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="border: 0px; color: white;" colspan="2">TTP (min)</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(250, 250, 250);">
                                                        <%--THM (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="border: 0px; color: white;" colspan="2">THM (min)</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; font-size: 1.5em; text-align: center;">
                                                        <%--TTT (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="border: 0px; color: white;" colspan="2">TTT (min)</td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 020px; height: 25px; background-color: rgb(255, 255, 255); color: white; font-size: 1.5em; text-align: center;"></td>
                                                </tr>
                                            </table>
                                            <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray; scrollbar-arrow-color: rgb(0, 72, 89);">
                                                <div id="dvAccordian">
                                                    <asp:Repeater ID="repAccordian" runat="server">
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table style="margin: 0; padding: 0;">
                                                                <tr>
                                                                    <td style="width: 091px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Data") %>"><%# Eval("Data").ToString().Substring(0, 10) %> </td>
                                                                    <td style="width: 122px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Corredor") %>"><%# Eval("Corredor")%> </td>
                                                                    <td style="width: 122px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Rota") %>"><%# Eval("Rota") %> </td>
                                                                    <td style="width: 122px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("SubRota") %>"><%# Eval("SubRota") %> </td>
                                                                    <td style="width: 071px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Classe") %>"><%# Eval("Classe")%> </td>
                                                                    <td style="width: 091px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("OS") %>"><%# Eval("OS") %> </td>
                                                                    <td style="width: 081px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Prefixo") %>"><%# Eval("Prefixo") %> </td>
                                                                    <td style="width: 150px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Grupo") %>"><%# Eval("Grupo") %> </td>
                                                                    <td style="width: 150px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Motivo") %>"><%# Eval("Motivo") %> </td>
                                                                    <td style="width: 070px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("SB") %>"><%# Eval("SB") %> </td>
                                                                    <td style="width: 100px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Data_Ini") %>"><%# Eval("Data_Ini") %> </td>
                                                                    <td style="width: 100px; border-right: 1px solid rgb(210, 210, 210); text-align: center;" title="<%# Eval("Data_Fim") %>"><%# Eval("Data_Fim") %> </td>
                                                                    <td style="width: 130px; border-right: 1px solid rgb(210, 210, 210); text-align: center;">
                                                                        <%--THP (min)--%>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_THP_Meta") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_THP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_THP_Meta").ToString()))) : "" %>' />
                                                                                </td>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_THP_Real") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_THP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_THP_Real").ToString()))) : "" %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 130px; border-right: 1px solid rgb(210, 210, 210); text-align: center;">
                                                                        <%--TTP (min)--%>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_TTP_Meta") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_TTP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_TTP_Meta").ToString()))) : "" %>' />
                                                                                </td>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_TTP_Real") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_TTP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_TTP_Real").ToString()))) : "" %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 130px; border-right: 1px solid rgb(210, 210, 210); text-align: center;">
                                                                        <%--THM (min)--%>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_THM_Meta") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_THM_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_THM_Meta").ToString()))) : "" %>' />
                                                                                </td>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_THM_Real") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_THM_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_THM_Real").ToString()))) : "" %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 130px; border-right: 1px solid rgb(210, 210, 210); text-align: center;">
                                                                        <%--TTT (min)--%>
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_TTT_Meta") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_TTT_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_TTT_Meta").ToString()))) : "" %>' />
                                                                                </td>
                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TOT_TTT_Real") %> minuto(s)">
                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TOT_TTT_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TOT_TTT_Real").ToString()))) : "" %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <div>
                                                                <table style="width: 100%; margin: 0px; padding: 0px;">
                                                                    <tr>
                                                                        <td style="height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Data</td>
                                                                        <td style="width: 100px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Corredor</td>
                                                                        <td style="width: 070px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Rota</td>
                                                                        <td style="width: 070px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">SubRota</td>
                                                                        <td style="width: 050px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Classe</td>
                                                                        <td style="width: 070px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">OS</td>
                                                                        <td style="width: 070px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Prefixo</td>
                                                                        <td style="width: 130px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Grupo</td>
                                                                        <td style="width: 130px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Motivo</td>
                                                                        <td style="width: 070px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">SB</td>
                                                                        <td style="width: 130px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Hr Início</td>
                                                                        <td style="width: 130px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">Hr Final</td>
                                                                        <td style="width: 150px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">
                                                                            <%--THP (min)--%>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="border: 0px; color: white;" colspan="2">THP (min)</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 150px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">
                                                                            <%--TTP (min)--%>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="border: 0px; color: white;" colspan="2">TTP (min)</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 150px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: bottom;">
                                                                            <%--THM (min)--%>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="border: 0px; color: white;" colspan="2">THM (min)</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 150px; height: 20px; background-color: rgb(55, 119, 188); color: white; text-align: center; vertical-align: bottom;">
                                                                            <%--TTT (min)--%>
                                                                            <table style="width: 100%">
                                                                                <tr>
                                                                                    <td style="border: 0px; color: white;" colspan="2">TTT (min)</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Meta</td>
                                                                                    <td style="width: 50%; border: 0px; color: white;">Real</td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <td style="width: 17px; height: 20px; background-color: rgb(255, 255, 255); color: white; text-align: center; vertical-align: bottom;"></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Panel runat="server" ScrollBars="Vertical" Height='<%# DataBinder.Eval(Container.DataItem, "Dados").ToString().Count() *3 %>'>
                                                                    <asp:Repeater ID="repeaterDados" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Dados") %>'>
                                                                        <HeaderTemplate>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table style="width: 100%; margin: 0px; padding: 0px;" class="table table-hover table-curved pro-table">
                                                                                <tr style="background-color: rgb(220, 220, 220);">
                                                                                    <td style="text-align: center;" title="<%# Eval("Data") %>"><%# Eval("Data").ToString().Substring(0, 10) %> </td>
                                                                                    <td style="width: 100px; text-align: center;" title="<%# Eval("Corredor") %>"><%# !string.IsNullOrEmpty(Eval("Corredor").ToString()) ? Eval("Corredor") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("Rota") %>"><%# !string.IsNullOrEmpty(Eval("Rota").ToString()) ? Eval("Rota") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("SubRota") %>"><%# !string.IsNullOrEmpty(Eval("SubRota").ToString()) ? Eval("SubRota") : string.Format("        ") %> </td>
                                                                                    <td style="width: 050px; text-align: center;" title="<%# Eval("Classe") %>"><%# !string.IsNullOrEmpty(Eval("Classe").ToString()) ? Eval("Classe") : string.Format(" ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("OS") %>"><%# !string.IsNullOrEmpty(Eval("OS").ToString()) ? Eval("OS") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("Prefixo") %>"><%# !string.IsNullOrEmpty(Eval("Prefixo").ToString()) ? Eval("Prefixo") : string.Format("    ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Grupo") %>"><%# !string.IsNullOrEmpty(Eval("Grupo").ToString()) ? Eval("Grupo") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Motivo") %>"><%# !string.IsNullOrEmpty(Eval("Motivo").ToString()) ? Eval("Motivo") : string.Format("                    ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("SB") %>"><%# !string.IsNullOrEmpty(Eval("SB").ToString()) ? Eval("SB") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Data_Ini") %>"><%# !string.IsNullOrEmpty(Eval("Data_Ini").ToString()) ? Eval("Data_Ini") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Data_Fim") %>"><%# !string.IsNullOrEmpty(Eval("Data_Fim").ToString()) ? Eval("Data_Fim") : string.Format("        ") %> </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--THP (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THP_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THP_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THP_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THP_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--TTP (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTP_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTP_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTP_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTP_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--THM (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THM_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THM_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THM_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THM_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THM_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THM_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--TTT (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTT_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTT_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTT_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTT_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTT_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTT_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <table style="margin: 0px; padding: 0px;" class="table table-hover table-curved pro-table">
                                                                                <tr style="background-color: rgb(190, 190, 190);">
                                                                                    <td style="text-align: center;" title="<%# Eval("Data") %>"><%# Eval("Data").ToString().Substring(0, 10) %> </td>
                                                                                    <td style="width: 100px; text-align: center;" title="<%# Eval("Corredor") %>"><%# !string.IsNullOrEmpty(Eval("Corredor").ToString()) ? Eval("Corredor") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("Rota") %>"><%# !string.IsNullOrEmpty(Eval("Rota").ToString()) ? Eval("Rota") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("SubRota") %>"><%# !string.IsNullOrEmpty(Eval("SubRota").ToString()) ? Eval("SubRota") : string.Format("        ") %> </td>
                                                                                    <td style="width: 050px; text-align: center;" title="<%# Eval("Classe") %>"><%# !string.IsNullOrEmpty(Eval("Classe").ToString()) ? Eval("Classe") : string.Format(" ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("OS") %>"><%# !string.IsNullOrEmpty(Eval("OS").ToString()) ? Eval("OS") : string.Format("        ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("Prefixo") %>"><%# !string.IsNullOrEmpty(Eval("Prefixo").ToString()) ? Eval("Prefixo") : string.Format("    ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Grupo") %>"><%# !string.IsNullOrEmpty(Eval("Grupo").ToString()) ? Eval("Grupo") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Motivo") %>"><%# !string.IsNullOrEmpty(Eval("Motivo").ToString()) ? Eval("Motivo") : string.Format("                    ") %> </td>
                                                                                    <td style="width: 070px; text-align: center;" title="<%# Eval("SB") %>"><%# !string.IsNullOrEmpty(Eval("SB").ToString()) ? Eval("SB") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Data_Ini") %>"><%# !string.IsNullOrEmpty(Eval("Data_Ini").ToString()) ? Eval("Data_Ini") : string.Format("        ") %> </td>
                                                                                    <td style="width: 130px; text-align: center;" title="<%# Eval("Data_Fim") %>"><%# !string.IsNullOrEmpty(Eval("Data_Fim").ToString()) ? Eval("Data_Fim") : string.Format("        ") %> </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--THP (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THP_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THP_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THP_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THP_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--TTP (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTP_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTP_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTP_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTP_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTP_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTP_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--THM (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THM_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THM_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THM_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("THM_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("THM_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("THM_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    <td style="width: 150px; text-align: center;">
                                                                                        <%--TTT (min)--%>
                                                                                        <table style="width: 100%">
                                                                                            <tr>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTT_Meta") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTT_Meta").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTT_Meta").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                                <td style="width: 50%; border: 0px;" title="<%# Eval("TTT_Real") %> minuto(s)">
                                                                                                    <asp:Label runat="server" Text='<%# double.Parse(Eval("TTT_Real").ToString()) != 0 ? string.Format("{0}", TimeSpan.FromSeconds(double.Parse(Eval("TTT_Real").ToString()))) : "        " %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </AlternatingItemTemplate>
                                                                        <FooterTemplate>
                                                                        </FooterTemplate>
                                                                    </asp:Repeater>
                                                                </asp:Panel>
                                                            </div>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                        </FooterTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </asp:Panel>
                                            <table style="margin: 0; padding: 0;">
                                                <tr>
                                                    <td style="width: 091px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 122px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 071px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 091px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 081px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 150px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 150px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 070px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 100px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: bottom;"></td>
                                                    <td style="width: 100px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: right; border-right: 1px solid rgb(250, 250, 250); vertical-align: middle;">MÉDIA:  </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: middle;">
                                                        <%--THP (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">THP:</td>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">
                                                                    <asp:Label runat="server" ID="lblAVG_THP_Real" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: middle;">
                                                        <%--TTP (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">TTP:</td>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">
                                                                    <asp:Label runat="server" ID="lblAVG_TTP_Real" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; border-right: 1px solid rgb(250, 250, 250); vertical-align: middle;">
                                                        <%--THM (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">THM:</td>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">
                                                                    <asp:Label runat="server" ID="lblAVG_THM_Real" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 130px; height: 25px; background-color: rgb(0, 72, 89); color: white; text-align: center; vertical-align: middle;">
                                                        <%--TTT (min)--%>
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">TTT:</td>
                                                                <td style="width: 50%; border: 0px; color: white; text-align: center; padding-left: 10px; padding-right: 10px;">
                                                                    <asp:Label runat="server" ID="lblAVG_TTT_Real" /></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width: 020px; height: 25px; background-color: rgb(255, 255, 255); color: white; text-align: center;"></td>
                                                </tr>
                                            </table>
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
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="upDados">
                    <ProgressTemplate>
                        <div class="Processando">
                            <table class="Texto_Processando">
                                <tr>
                                    <td>
                                        <asp:Image runat="server" ImageUrl="~/img/process.gif" Width="50" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Text="Processando..." />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
    </form>
</body>
</html>