<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Macro.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macro" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <style>
        .tipo-E {
            color: green;
            background-color: white;
        }

            .tipo-E:hover {
                color: black;
                background-color: white;
            }

        .tipo-R {
            color: black;
            background-color: white;
        }

        .macro-19 {
            color: white;
            background-color: red;
        }

            .macro-19:hover {
                color: black;
                background-color: white;
            }

        .macro-18 {
            color: white;
            background-color: black;
        }

            .macro-18:hover {
                color: black;
                background-color: white;
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
    <script type="text/javascript">
<%--        $(function () {
            $("#<%= txtDataInicio.ClientID %>").datepicker({
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
                yearSuffix: '',
                beforeShowDay: function (date) {
                    return [true, 'highlight', 'The custom title'];
                }
            });
            $('#<%= txtHoraInicio.ClientID %>').timepicker({
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
            });
        });--%>

        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });
        function abrirhelp() {
            window.open("../Ajuda/Help01.html", "Ajuda", "status=no, toolbar=no, scrollbars=yes, resizable=yes, location=no, width=800, height=600, menubar=no");
        }
        Sys.WebForms.PageRequestManager.getInstance()._origOnFormActiveElement = Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive;
        Sys.WebForms.PageRequestManager.getInstance()._onFormElementActive = function (element, offsetX, offsetY) {
            if (element.tagName.toUpperCase() === 'INPUT' && element.type === 'image') {
                offsetX = Math.floor(offsetX);
                offsetY = Math.floor(offsetY);
            }
            this._origOnFormActiveElement(element, offsetX, offsetY);
        };
    </script>
    <div class="well well-sm">
        <asp:Panel ID="Panel1" runat="server" DefaultButton="lnkPesquisar">
            <asp:Table ID="Table1" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <div class="form-group">
                            <div class="page-header sub-content-header">
                                <%--<h2>Filtros</h2>--%>
                                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                            </div>
                        </div>
                        <div id="filtros">
                            <div>
                                <table class="auto-style1">
                                    <tr>
                                        <td style="width: 170px;">
                                            <label for="locomotiva">Nº Locomotiva:</label>
                                            <asp:TextBox ID="txtNumeroLocomotiva" runat="server" Width="160" CssClass="form-control" ToolTip="Separe as locomotivas desejadas com vírgulas. Ex.: 2904, 0962, 7007" onkeypress="return fnValidaNroVirgula(event);" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="trem">Nº Trem:</label>
                                            <asp:TextBox ID="txtNumeroTrem" runat="server" Width="160" CssClass="form-control" ToolTip="Separe os trens desejados com vírgulas. Ex.: C001, M642, E050" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="macro">Nº Macro:</label>
                                            <asp:TextBox ID="txtNumeroMacro" runat="server" Width="160" CssClass="form-control" ToolTip="Separe as macros desejadas com víruglas. Ex.: 32, 35, 6, 9" onkeypress="return fnValidaNroVirgula(event);" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="codigoos">Código OS:</label>
                                            <asp:TextBox ID="txtCodigoOS" runat="server" Width="160" CssClass="form-control"></asp:TextBox>
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="motivo">Motivo:</label>
                                            <asp:TextBox ID="txtMotivo" runat="server" Width="160" CssClass="form-control" ToolTip="Separe os motivos desejados com víruglas. Ex.: 32, 35, 6, 9" onkeypress="return fnValidaNroVirgula(event);" />
                                        </td>
                                        <td rowspan="2" style="width: 170px;">
                                            <label for="data_fim">Corredor:</label>
                                            <br />
                                            <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="137">
                                                <asp:ListItem Text="&nbsp;&nbsp;Baixada" Value="Baixada" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="Centro Leste" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Norte" Value="Centro Norte" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Centro Sudeste" Value="Centro Sudeste" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Minas Bahia" Value="Minas Bahia" />
                                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="Minas Rio" />
                                            </asp:CheckBoxList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 170px;">
                                            <label for="data_inicio">Data:</label>
                                            <asp:TextBox ID="txtDataInicio" runat="server" Width="160" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                                            <asp:CalendarExtender runat="server" ID="cetxtDataInicio" TargetControlID="txtDataInicio" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="hora_inicio">Hora:</label>
                                            <asp:TextBox ID="txtHoraInicio" runat="server" Width="160" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="data_fim">Mais Hora(s):</label>
                                            <br />
                                            <asp:DropDownList runat="server" Width="160" ID="ddlMais" CssClass="form-control">
                                                <asp:ListItem Text="01" Value="1" />
                                                <asp:ListItem Text="02" Value="2" />
                                                <asp:ListItem Text="03" Value="3" />
                                                <asp:ListItem Text="04" Value="4" />
                                                <asp:ListItem Text="05" Value="5" />
                                                <asp:ListItem Text="06" Value="6" />
                                                <asp:ListItem Text="07" Value="7" />
                                                <asp:ListItem Text="08" Value="8" />
                                                <asp:ListItem Text="09" Value="9" />
                                                <asp:ListItem Text="10" Value="10" />
                                                <asp:ListItem Text="11" Value="11" />
                                                <asp:ListItem Text="12" Value="12" />
                                                <asp:ListItem Text="13" Value="13" />
                                                <asp:ListItem Text="14" Value="14" />
                                                <asp:ListItem Text="15" Value="15" />
                                                <asp:ListItem Text="16" Value="16" />
                                                <asp:ListItem Text="17" Value="17" />
                                                <asp:ListItem Text="18" Value="18" />
                                                <asp:ListItem Text="19" Value="19" />
                                                <asp:ListItem Text="20" Value="20" />
                                                <asp:ListItem Text="21" Value="21" />
                                                <asp:ListItem Text="22" Value="22" />
                                                <asp:ListItem Text="23" Value="23" />
                                                <asp:ListItem Text="24" Value="24" />
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="data_fim">Direção:</label>
                                            <br />
                                            <asp:RadioButton ID="rdParaFrente" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para frente" />
                                            <br />
                                            <asp:RadioButton ID="rdTras" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Trás" Checked="true" />
                                        </td>
                                        <td style="width: 170px;">
                                            <label for="motivo">Expressão:</label>
                                            <asp:TextBox ID="txtExpressao" runat="server" Width="160" CssClass="form-control" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="width: 100%">

                                            <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" OnClick="ButtonPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkLImpar" runat="server" CssClass="btn btn-primary" OnClick="btnLimpar_Click"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkAtualizarHora" runat="server" CssClass="btn btn-info" OnClick="ButtonAtualizarHora_Click"><i class="fa fa-clock-o"></i>&nbsp;Atualizar Hora</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkGerarExcel" runat="server" CssClass="btn btn-default" OnClick="ButtonGerarExcel_Click"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="SelectButton" runat="server" CssClass="btn btn-info" OnClientClick="abrirhelp()"><i class="fa fa-question-circle"></i>&nbsp;Ajuda</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <table>
                                    <tr>
                                        <td>
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </asp:TableCell>
                    <asp:TableCell>&nbsp;&nbsp;&nbsp;</asp:TableCell>
                    <asp:TableCell>
                        <div id="Div4" class="form-group" runat="server" visible="false">
                            <div class="page-header sub-content-header">
                                <a data-toggle="collapse" title="Avançado" data-parent="#macros" href="#avancado" style="margin-left: 3px; font-size: 15px" accesskey="9"><b>Avançado</b> <i class="fa fa-bars"></i></a>
                            </div>
                        </div>
                        <div id="avancado" class="collapse">
                            <div>
                                <div class="row">

                                    <asp:CheckBox ID="chkER" runat="server" Text="E/R" Checked="true" class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12" />
                                    <asp:CheckBox ID="chkLocomotiva" runat="server" Text="LOCOMOTIVA" Checked="true" class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12" />
                                    <asp:CheckBox ID="chkTrem" runat="server" Text="TREM" Checked="true" class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12" />

                                </div>
                            </div>
                        </div>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
    <asp:RadioButton ID="rdJuntas" runat="server" GroupName="Filtro" Text="&nbsp; Macros Juntas" OnCheckedChanged="rdJuntas_CheckedChanged" Checked="true" AutoPostBack="true" />
    &nbsp;&nbsp;
            <asp:RadioButton ID="rdEnviadas" runat="server" GroupName="Filtro" Text="&nbsp; Macros Enviadas" OnCheckedChanged="rdEnviadas_CheckedChanged" AutoPostBack="true" />
    &nbsp;&nbsp;
            <asp:RadioButton ID="rdRecebidas" runat="server" GroupName="Filtro" Text="&nbsp; Macros Recebidas" OnCheckedChanged="rdRecebidas_CheckedChanged" AutoPostBack="true" />
    <ul class="nav nav-tabs" role="tablist" id="myTab">
    </ul>
    <div class="tab-content">
        <asp:Panel runat="server" ID="pnlJuntas" Visible="true">
            <%--   MACROS JUNTAS   --%>
            <div class="tab-pane active" id="Div1">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <h3>Resultados da Pesquisa</h3>
                        <%--<a data-toggle="modal" data-target="#modal_macros" data-backdrop="static" style="margin-left: 3px;" href=""><i class="fa fa-question-circle"></i></a>--%>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="upRegistros">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterJuntas" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table" id="macros">
                                                        <thead>
                                                            <tr>
                                                                <% if (this.chkER.Checked)
                                                                   {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasRE" runat="server" OnClick="lnkJuntasRE_Click">R/E</asp:LinkButton>
                                                                </th>
                                                                <%} %>
                                                                <% if (this.chkLocomotiva.Checked)
                                                                   {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasLoco" runat="server" OnClick="lnkJuntasLoco_Click">Loco</asp:LinkButton>
                                                                </th>
                                                                <%} %>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasTrem" runat="server" OnClick="lnkJuntasTrem_Click">Trem</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasCodOS" runat="server" OnClick="lnkJuntasCodOS_Click">Cod. OS</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasHorario" runat="server" OnClick="lnkJuntasHorario_Click">Horário</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasMacro" runat="server" OnClick="lnkJuntasMacro_Click">Macro</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasTexto" runat="server" OnClick="lnkJuntasTexto_Click">Texto</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkJuntasTratado" runat="server" OnClick="lnkJuntasTratado_Click">Tratado</asp:LinkButton>
                                                                </th>
                                                                <th style="text-align: center; font-size: 12pt;">
                                                                    <asp:LinkButton ID="lnkJuntasCorredor" runat="server" OnClick="lnkJuntasCorredor_Click">Corredor</asp:LinkButton>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="cursor: pointer" class="macro-<%# Eval ("NumeroMacro")%> tipo-<%# Eval("Tipo")%>" onclick="javascript:window.open('/Consulta/MacroPopUp.aspx?tipo=<%# Eval("Tipo")%>&id=<%# Eval("ID")%>', '<%# Eval("ID")%>', 'width=710, height=600, resizable=yes top=00 scrollbars=yes')">
                                                        <% if (this.chkER.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%></td>
                                                        <%} %>
                                                        <% if (this.chkLocomotiva.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva")%>"><%# Eval("Locomotiva")%></td>
                                                        <%} %>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("CodigoOS")%>"><%# Eval ("CodigoOS")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Horario")%>"><%# Eval ("Horario")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("DescricaoMacro")%>"><%# Eval ("NumeroMacro")%></td>
                                                        <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);">
                                                            <div onclick="<%--$(this).css('text-overflow', 'inherit'); $(this).css('width', '100%');--%>" style="width: 600px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                                                        </td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tratado")%>"><%# Eval ("Tratado")%></td>
                                                        <td style="text-align: left;" title="<%# Eval ("Corredor")%>"><%# Eval ("Corredor")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                        </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblJ_PaginaAte" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkJ_PrimeiraPagina" runat="server" OnClick="lnkJ_PrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkJ_PaginaAnterior" runat="server" OnClick="lnkJ_PaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="ddlJ_ItensPorPagina" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" Selected="True" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lnkJ_ProximaPagina" runat="server" OnClick="lnkJ_ProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkJ_UltimaPagina" runat="server" OnClick="lnkJ_UltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalJuntas" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
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

        </asp:Panel>
        <asp:Panel runat="server" ID="pnlEnviadas" Visible="false">
            <div class="tab-pane" id="Div2">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <h3>Resultados da Pesquisa</h3>
                        <%--<a data-toggle="modal" data-target="#modal_macros" data-backdrop="static" style="margin-left: 3px;" href=""><i class="fa fa-question-circle"></i></a>--%>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterEnviadas" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table " id="macrosenviadas">
                                                        <thead>
                                                            <tr>
                                                                <% if (this.chkER.Checked)
                                                                   {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasRE" runat="server" OnClick="lnkEnviadasRE_Click">E/R</asp:LinkButton></th>
                                                                <%} %><% if (this.chkLocomotiva.Checked)
                                                                         {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasLoco" runat="server" OnClick="lnkEnviadasLoco_Click">Loco</asp:LinkButton></th>
                                                                <%} %>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasTrem" runat="server" OnClick="lnkEnviadasTrem_Click">Trem</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasCodOS" runat="server" OnClick="lnkEnviadasCodOS_Click">Cod. OS</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasHorario" runat="server" OnClick="lnkEnviadasHorario_Click">Horário</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasMacro" runat="server" OnClick="lnkEnviadasMacro_Click">Macro</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasTexto" runat="server" OnClick="lnkEnviadasTexto_Click">Texto</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasTempoEnvio" runat="server" OnClick="lnkEnviadasTempoEnvio_Click">Temp. Envio</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasTratado" runat="server" OnClick="lnkEnviadasTratado_Click">Tratado</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkEnviadasSituacao" runat="server" OnClick="lnkEnviadasSituacao_Click">Situação</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt;">
                                                                    <asp:LinkButton ID="lnkEnviadasCorredor" runat="server" OnClick="lnkEnviadasCorredor_Click">Corredor</asp:LinkButton>
                                                                </th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="cursor: pointer" class="macro-<%# Eval ("NumeroMacro")%> tipo-<%# Eval("Tipo")%>" onclick="javascript:window.open('/Consulta/MacroPopUp.aspx?tipo=<%# Eval("Tipo")%>&id=<%# Eval("ID")%>', '<%# Eval("ID")%>', 'width=710, height=600, resizable=yes top=00 scrollbars=yes')">
                                                        <% if (this.chkER.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%></td>
                                                        <%} %>
                                                        <% if (this.chkLocomotiva.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva")%>"><%# Eval("Locomotiva")%></td>
                                                        <%} %>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("CodigoOS")%>"><%# Eval ("CodigoOS")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Horario")%>"><%# Eval ("Horario")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("DescricaoMacro")%>"><%# Eval ("NumeroMacro")%></td>
                                                        <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);">
                                                            <div onclick="<%--$(this).css('text-overflow', 'inherit'); $(this).css('width', '100%');--%>" style="width: 450px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                                                        </td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tempo_Decorrido")%>"><%# Eval ("Tempo_Decorrido")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tratado")%>"><%# Eval ("Tratado")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Status")%>"><%# Eval ("Status")%></td>
                                                        <td style="text-align: left;" title="<%# Eval ("Corredor")%>"><%# Eval ("Corredor")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                            </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblE_PaginaAte" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkE_PrimeiraPagina" runat="server" OnClick="lnkE_PrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkE_PaginaAnterior" runat="server" OnClick="lnkE_PaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="ddlE_ItensPorPagina" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" Selected="True" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lnkE_ProximaPagina" runat="server" OnClick="lnkE_ProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkE_UltimaPagina" runat="server" OnClick="lnkE_UltimaPagia_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalEnviadas" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upRegistros">
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
        </asp:Panel>
        <asp:Panel runat="server" ID="pnlRecebidas" Visible="false">
            <div class="tab-pane" id="Div3">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <h3>Resultados da Pesquisa</h3>
                        <%--<a data-toggle="modal" data-target="#modal_macros" data-backdrop="static" style="margin-left: 3px;" href=""><i class="fa fa-question-circle"></i></a>--%>
                    </div>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterRecebidas" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table " id="macrosrecebidas">
                                                        <thead>
                                                            <tr>
                                                                <% if (this.chkER.Checked)
                                                                   {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasRE" runat="server" OnClick="lnkRecebidasRE_Click">R/E</asp:LinkButton></th>
                                                                <%} %><% if (this.chkLocomotiva.Checked)
                                                                         {%>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasLoco" runat="server" OnClick="lnkRecebidasLoco_Click">Loco</asp:LinkButton></th>
                                                                <%} %>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasTrem" runat="server" OnClick="lnkRecebidasTrem_Click">Trem</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasCodOS" runat="server" OnClick="lnkRecebidasCodOS_Click">Cod. OS</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasHorario" runat="server" OnClick="lnkRecebidasHorario_Click">Horário</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasMacro" runat="server" OnClick="lnkRecebidasMacro_Click">Macro</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasTexto" runat="server" OnClick="lnkRecebidasTexto_Click">Texto</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasLocalizacao" runat="server" OnClick="lnkRecebidasLocalizacao_Click">Localização</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasMCT" runat="server" OnClick="lnkRecebidasMCT_Click">MCT</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasLatitude" runat="server" OnClick="lnkRecebidasLatitude_Click">Latitude</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasLongitude" runat="server" OnClick="lnkRecebidasLongitude_Click">Longitude</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton ID="lnkRecebidasTratado" runat="server" OnClick="lnkRecebidasTratado_Click">Tratado</asp:LinkButton></th>
                                                                <th style="text-align: center; font-size: 12pt;">
                                                                    <asp:LinkButton ID="lnkRecebidasCorredor" runat="server" OnClick="lnkRecebidasCorredor_Click">Corredor</asp:LinkButton></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr style="cursor: pointer" class="macro-<%# Eval ("NumeroMacro")%> tipo-<%# Eval("Tipo")%>" onclick="javascript:window.open('/Consulta/MacroPopUp.aspx?tipo=<%# Eval("Tipo")%>&id=<%# Eval("ID")%>', '<%# Eval("ID")%>', 'width=710, height=600, resizable=yes top=00 scrollbars=yes')">
                                                        <% if (this.chkER.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Tipo")%>"><%# Eval("Tipo")%></td>
                                                        <%} %>
                                                        <% if (this.chkLocomotiva.Checked)
                                                           {%>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Locomotiva")%>"><%# Eval("Locomotiva")%> </td>
                                                        <%} %>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Trem")%>"><%# Eval("Trem")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("CodigoOS")%>"><%# Eval ("CodigoOS")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Horario")%>"><%# Eval ("Horario")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("DescricaoMacro")%>"><%# Eval ("NumeroMacro")%></td>
                                                        <td style="text-align: left; border-right: 1px solid rgb(0, 72, 89);">
                                                            <div onclick="<%--$(this).css('text-overflow', 'inherit'); $(this).css('width', '100%');--%>" style="width: 300px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                                                        </td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Localizacao")%>"><%# Eval ("Localizacao")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("MCT")%>"><%# Eval ("MCT")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Latitude")%>"><%# Eval ("Latitude")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Longitude")%>"><%# Eval ("Longitude")%></td>
                                                        <td style="text-align: center; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval ("Tratado")%>"><%# Eval ("Tratado")%></td>
                                                        <td style="text-align: left;" title="<%# Eval ("Corredor")%>"><%# Eval ("Corredor")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                            </table>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblR_PaginaAte" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkR_PrimeiraPagina" runat="server" OnClick="lnkR_PrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkR_PaginaAnterior" runat="server" OnClick="lnkR_PaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="ddlR_ItensPorPagina" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" Selected="True" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lnkR_ProximaPagina" runat="server" OnClick="lnkR_ProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkR_UltimaPagina" runat="server" OnClick="lnkR_UltimaPagia_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalRecebidas" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upRegistros">
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
        </asp:Panel>
    </div>
</asp:Content>
