<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="CCO.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.CCO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table style="width: 100%">
    <tr>
        <td style="width: 79%;">
            <div class="alert alert-success">
                <h2><asp:Label ID="Label1" runat="server" Text="Relatório CCO" Font-Size="20px" style="color: rgb(0, 100, 0);" />&nbsp;</h2>
            </div>
        </td>
        <td style="width: 1%; text-align: left;"></td>
        <td style="width: 20%; text-align: center;">
            <div class="alert alert-info">
                <h2>
                    <asp:Label ID="lblUsuarioMatricula" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                    <asp:Label ID="lblUsuarioLogado" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />,&nbsp;
                    <asp:Label ID="lblUsuarioPerfil" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" />&nbsp;
                    <asp:Label ID="lblUsuarioMaleta" runat="server" Font-Size="12px" style="color: rgb(0, 72, 89);" Visible="false" />
                </h2>
            </div>
        </td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <link href="../css/jquery-ui-timepicker-addon.css" rel="stylesheet" />
    <script>
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });

        function selectAllOperador(invoker) {
            var divControll = document.getElementById('dvOperadores');
            var inputElements = divControll.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }

        function selectAllPostoTrabalho(invoker) {
            var divControll = document.getElementById('dvPostoTrabalho');
            var inputElements = divControll.getElementsByTagName('input');
            for (var i = 0; i < inputElements.length; i++) {
                var myElement = inputElements[i];
                if (myElement.type === "checkbox") {
                    myElement.checked = invoker.checked;
                }
            }
        }

        $(function () {
            $("#<%= txtDataInicial.ClientID %>").datepicker({
                buttonText: "Data inicial",
                showOn: "button",
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                buttonImage: "../img/calendario.gif",
                buttonImageOnly: true,

                closeText: 'Fechar',
                prevText: '&#x3C;Anterior',
                nextText: 'Próximo&#x3E;',
                currentText: 'Hoje',
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 0,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            });
            $("#<%= txtDataFinal.ClientID %>").datepicker({
                buttonText: "Data final",
                showOn: "button",
                showButtonPanel: true,
                changeMonth: true,
                changeYear: true,
                buttonImage: "../img/calendario.gif",
                buttonImageOnly: true,

                closeText: 'Fechar',
                prevText: '&#x3C;Anterior',
                nextText: 'Próximo&#x3E;',
                currentText: 'Hoje',
                monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
                monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
                dayNames: ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'],
                dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 0,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            });
            $('#<%= txtHoraInicial.ClientID %>').timepicker({
                buttonText: "Hora inicial",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar',
                timeFormat: 'HH:mm',
                amNames: ['a.m.', 'AM', 'A'],
                pmNames: ['p.m.', 'PM', 'P'],
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
            $('#<%= txtHoraFinal.ClientID %>').timepicker({
                buttonText: "Hora inicial",
                buttonImage: '../img/time.png',
                buttonImageOnly: true,
                showOn: 'button',
                timeOnlyTitle: 'Escolha a hora',
                timeText: 'Hora',
                hourText: 'Horas',
                minuteText: 'Minutos',
                secondText: 'Segundos',
                millisecText: 'Milissegundos',
                timezoneText: 'Fuso horário',
                currentText: 'Agora',
                closeText: 'Fechar',
                timeFormat: 'HH:mm',
                amNames: ['a.m.', 'AM', 'A'],
                pmNames: ['p.m.', 'PM', 'P'],
                pickDate: false,
                pickSeconds: false,
                pick12HourFormat: false
            });
        });

    </script>
    <style>
        .postoTrabalho-1 { /* Amarelo */
            color: black;
            background-color: #F0E68C;
        }
        .postoTrabalho-1:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-2 { /* Vermelho */
            color: black;
            background-color: #F08080;
        }
        .postoTrabalho-2:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-3 { /* Verde */
            color: black;
            background-color: #8FBC8F;
        }
        .postoTrabalho-3:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-4 { /* Azul */
            color: black;
            background-color: #ADD8E6;
        }
        .postoTrabalho-4:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-5 { /* Branco */
            color: black;
            background-color: #ffffff;
        }
        .postoTrabalho-5:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-6 { /* Branco T */
            color: black;
            background-color: #DCDCDC;
        }
        .postoTrabalho-6:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-7 { /* Laranja */
            color: black;
            background-color: #FFA500;
        }
        .postoTrabalho-7:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-8 { /* Azul Ceu */
            color: black;
            background-color: #AFEEEE;
        }
        .postoTrabalho-9:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .postoTrabalho-9 { /* Vinho */
            color: white;
            background-color: #800080;
        }
        .postoTrabalho-9:hover {
            color: black;
            background-color: rgb(0,0,0);
        }

        .Processando {
            margin-left: auto;
            margin-right: auto;
            width: 50em;
            text-align: center;
        }
        .ModalBackground {
            background-color: gray;
            filter: alpha(opacity=50);
            opacity: 0.50;
        }
    </style>
            
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros" >
            <div class="row">
                <table style="width: 95%; margin-left: 3%; margin-top: 1%;">
                    <tr>
                        <td style="width: 10%;">
                            <label for="matricula">Data Inicial:</label>
                            <asp:TextBox runat="server" ID="txtDataInicial" CssClass="form-control" MaxLength="10" Width="90%"></asp:TextBox>
                        </td>
                        <td style="width: 10%;">
                            <label for="matricula">Hora Inicial:</label>
                            <asp:TextBox runat="server" ID="txtHoraInicial" CssClass="form-control" MaxLength="10" Width="80%" />
                        </td>
                        <td style="width: 40%; vertical-align: top;" rowspan="2">
                            <div id="dvOperadores">
                                <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Visible="true" Enabled="true" Height="150px" Width="95%" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                                    <asp:Repeater ID="rptListaOperadores" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr style="position: absolute; vertical-align: bottom;">
                                                        <th style="margin-left: 000px; width: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">
                                                            <asp:CheckBox runat="server" ID="chkTodos" OnClick="selectAllOperador(this)" ToolTip="Seleciona Todos" /></th>
                                                        <th style="margin-left: 030px; width: 450px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">Operador</th>
                                                    </tr>
                                                    <tr>
                                                        <td style="height: 25px;"></td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; margin-top: 15px;">
                                                <td style="width: 005px; text-align: left;">
                                                    <div>
                                                        <asp:HiddenField ID="HiddenField1" Value=' <%# string.Format("{0}:{1}", Eval("Matricula"), Eval("Nome") ) %>' runat="server" />
                                                        <asp:CheckBox runat="server" ID="chkOperador" />
                                                    </div>
                                                </td>
                                                <td style="width: 350px; text-align: left;" title="<%# Eval("Nome") %>"><%# Eval("Nome")%> </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
    </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                </asp:Panel>
                            </div>
                        </td>
                        <td style="width: 20%; vertical-align: top;" rowspan="2">
                            <div id="dvPostoTrabalho">
                                <asp:Panel ID="Panel2" runat="server" ScrollBars="Vertical" Visible="true" Enabled="true" Height="150px" Width="95%" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                                    <asp:Repeater ID="rptListaPostoTrabalho" runat="server">
                                        <HeaderTemplate>
                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                <thead>
                                                    <tr style="position: absolute; vertical-align: bottom;">
                                                        <th style="margin-left: 000px; width: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">
                                                            <asp:CheckBox runat="server" ID="chkTodosPostoTrabalho" OnClick="selectAllPostoTrabalho(this)" ToolTip="Seleciona Todos" /></th>
                                                        <th style="margin-left: 030px; width: 210px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">Posto de Trabalho</th>


                                                        <th></th>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <br />
                                                            <br />
                                                        </td>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr style="font-size: 9px; margin-top: 15px;">
                                                <td style="width: 005px; text-align: left;">
                                                    <div>
                                                        <asp:HiddenField ID="HiddenField2" Value='<%# Eval("ID") %>' runat="server" />
                                                        <asp:CheckBox runat="server" ID="chkPostoTrabalho" />
                                                    </div>
                                                </td>
                                                <td style="width: 130px; text-align: left;" title="<%# Eval("Descricao")%>"><%# Eval("Descricao")%> </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
    </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                    <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">
                            <label for="matricula">Data Final:</label>
                            <asp:TextBox runat="server" ID="txtDataFinal" CssClass="form-control" MaxLength="10" Width="90%" />
                        </td>
                        <td style="width: 10%;">
                            <label for="matricula">Hora Final:</label>
                            <asp:TextBox runat="server" ID="txtHoraFinal" CssClass="form-control" MaxLength="10" Width="80%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 15px;"></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Button ID="btnPesquisar" CssClass="btn btn-success" Text="Pesquisar" runat="server" Width="15%" OnClick="btnPesquisar_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnLimpar" CssClass="btn btn-primary" Text="Limpar" runat="server" Width="15%" OnClick="btnLimpar_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btnGerarExcel" CssClass="btn btn-default" Text="Gerar Excel" runat="server" Width="15%" OnClick="btnGerarExcel_Click" />
                        </td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div style="margin-top: 2%">
        <table style="width: 100%;">
            <tr>
                <td style="width: 100%;">
                    <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Height="400" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                        <asp:Repeater ID="rptRelatorio_CCO" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover table-curved pro-table " id="macros">
                                    <thead>
                                        <tr style="vertical-align: top;">
                                            <th style="width: 25%; height: 25px; background-color: #4682B4; color: white;">Operador</th>
                                            <th style="width: 13%; height: 25px; background-color: #4682B4; color: white;">Data Inicial</th>
                                            <th style="width: 13%; height: 25px; background-color: #4682B4; color: white;">Data Final</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Tmp. Md. Resp. Macro 9</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Tmp. Md. Licenc.</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Tmp. Md. Resp. Entrada Via</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Qtde Md. Caract. Macro 0</th>
                                            <%--                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Qtde Md. Licença por Hora</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">THP</th>
                                            <th style="width: 07%; height: 25px; background-color: #4682B4; color: white;">Qtde Md. Trens Hora Trabalho</th>--%>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr style="font-size: 7.5px; height: 25px;" class="postoTrabalho-<%# Eval ("PostoTrabalho")%>">
                                    <td style="width: 25%; text-align: left;" title="<%# Eval("Operador").ToString().ToUpper() %>"><%# Eval("Operador")%> </td>
                                    <td style="width: 13%; text-align: left;" title="<%# Eval("DataInicial")%>"><%# Eval("DataInicial")%> </td>
                                    <td style="width: 13%; text-align: left;" title="<%# Eval("DataFinal")%>"><%# Eval("DataFinal")%> </td>
                                    <td style="width: 07%; text-align: left;" title="<%# Eval("TempoRespostaMacro9")%>"><%# Eval("TempoRespostaMacro9")%> </td>
                                    <td style="width: 07%; text-align: left;" title="<%# Eval("TempoMedioLicenciamentoMacro14")%>"><%# Eval("TempoMedioLicenciamentoMacro14")%> </td>
                                    <td style="width: 07%; text-align: left;" title="<%# Eval("TempoMedioRespostaEntradaVia")%>"><%# Eval("TempoMedioRespostaEntradaVia")%> </td>
                                    <td style="width: 07%; text-align: left;" title="<%# Eval("QuantidadeMediaCaracteresMacro0")%>"><%# Eval("QuantidadeMediaCaracteresMacro0")%> </td>
                                    <!--<td style="width: 07%; text-align: left;" title=""> </td>
                                    <td style="width: 07%; text-align: left;" title=""> </td>
                                    <td style="width: 07%; text-align: left;" title=""></td> -->
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                </tbody>
        </table>
                            </FooterTemplate>
                        </asp:Repeater>
                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
