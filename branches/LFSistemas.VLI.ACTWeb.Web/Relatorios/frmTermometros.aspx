<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="frmTermometros.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Relatorios.frmTermometros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Label ID="Label1" runat="server" Text="Relatório de Temperatura - Termômetros" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <script>
        function rbSelect() {
            var radiolist = document.getElementById('<%= rbRelatorio.ClientID %>');
            var radio = radiolist.getElementsByTagName("input");

            for (var x = 0; x < radio.length; x++) {
                if (radio[x].type === "radio" && radio[x].checked) {
                    var aux = radio[x].value;
                }
            }

            if (aux == 'RD03' || aux == 'RD04') {
                document.getElementById('<%= txtDataInicial.ClientID %>').disabled = true;
                document.getElementById('<%= txtHoraInicial.ClientID %>').disabled = true;
                document.getElementById('<%= ddlMais.ClientID %>').disabled = true;
                document.getElementById('<%= rdParaFrente.ClientID %>').disabled = true;
                document.getElementById('<%= rdParaTras.ClientID %>').disabled = true;
            }
            else {
                document.getElementById('<%= txtDataInicial.ClientID %>').disabled = false;
                document.getElementById('<%= txtHoraInicial.ClientID %>').disabled = false;
                document.getElementById('<%= ddlMais.ClientID %>').disabled = false;
                document.getElementById('<%= rdParaFrente.ClientID %>').disabled = false;
                document.getElementById('<%= rdParaTras.ClientID %>').disabled = false;
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
        });
    </script>
    <style>
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
    <asp:UpdatePanel runat="server" ID="upTermometros">
        <ContentTemplate>
            <div class="well well-sm">
                <div class="form-group">
                    <div class="page-header sub-content-header">
                        <%--<h2>Filtros</h2>--%>
                        <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
                    </div>
                </div>
                <div id="filtros">
                    <div>
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 30%;">
                                    <div style="text-align: left;">
                                        <asp:RadioButtonList runat="server" ID="rbRelatorio" RepeatDirection="Horizontal" CssClass="form-control" onclick="rbSelect()">
                                            <asp:ListItem Text="Histórico de Temperatura por Termômetros&nbsp;&nbsp;&nbsp;" Value="RD01" Selected="True" />
                                            <asp:ListItem Text="Histórico de Status de Termômetros&nbsp;&nbsp;&nbsp;" Value="RD02" />
                                            <asp:ListItem Text="Abrangência para Baixas Temperaturas&nbsp;&nbsp;&nbsp;" Value="RD03" />
                                            <asp:ListItem Text="Abrangência para Altas Temperaturas" Value="RD04" />
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 10px;">
                                    <asp:Panel runat="server" Width="100%" Height="150" CssClass="form-control">
                                        <table style="width: 100%;">
                                            <tr>
                                                <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Data Inicial:</label>
                                                    <asp:TextBox ID="txtDataInicial" runat="server" Width="90%" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                                                </td>
                                                <td style="width: 10%; vertical-align: top;">&nbsp;<label for="matricula">Hora Inicial:</label>
                                                    <asp:TextBox ID="txtHoraInicial" runat="server" Width="90%" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                                                </td>
                                                <td style="width: 10%; vertical-align: top;">&nbsp;<label for="data_fim">Mais Hora(s):</label>
                                                    <br />
                                                    <asp:DropDownList runat="server" ID="ddlMais" CssClass="form-control" Width="80%">
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
                                                <td style="width: 10%; vertical-align: top;">&nbsp;<label for="data_fim">Direção:</label>
                                                    <br />
                                                    <asp:RadioButton ID="rdParaFrente" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Frente" />
                                                    <br />
                                                    <asp:RadioButton ID="rdParaTras" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Trás" Checked="true" />
                                                </td>
                                                <td style="width: 30%; vertical-align: top;" rowspan="2">&nbsp;<label for="data_fim">Termômetro:</label>
                                                    <div>
                                                        <asp:Panel ID="Panel1" runat="server">
                                                            <asp:DropDownList runat="server" ID="ddlFiltroTermometros" CssClass="form-control" />
                                                            <%--<asp:Repeater ID="rptListaTermometros" runat="server">
                                                        <HeaderTemplate>
                                                            <table class="table table-hover table-curved pro-table " id="macros">
                                                                <thead>
                                                                    <tr style="position: absolute; vertical-align: bottom;">
                                                                        <th style="margin-left: 000px; width: 030px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white;">
                                                                            <asp:CheckBox runat="server" ID="chkTodos" OnClick="selectAllRotas(this)" ToolTip="Seleciona Todos" /></th>
                                                                        <th style="margin-left: 030px; width: 350px; height: 25px; position: absolute; z-index: auto; background-color: #4682B4; color: white; font-size: 1em;">Termômetros:</th>
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
                                                                        <asp:HiddenField ID="HiddenField1" Value=' <%# string.Format("{0}", Eval("Id") ) %>' runat="server" />
                                                                        <asp:CheckBox runat="server" ID="chkTermometro" />
                                                                    </div>
                                                                </td>
                                                                <td style="width: 350px; text-align: left;" title="<%# Eval("Descricao") %>"><%# Eval("Descricao")%> </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            </tbody>
    </table>
                                                        </FooterTemplate>
                                                    </asp:Repeater>--%>
                                                        </asp:Panel>
                                                    </div>
                                                </td>
                                                <td style="width: 30%; vertical-align: top;"></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%; vertical-align: bottom;" colspan="4">
                                                    <br />
                                                    <asp:LinkButton ID="lnkPesquisar" runat="server" CssClass="btn btn-success" OnClick="lnkPesquisar_Click"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                                                    &nbsp;&nbsp;<asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-primary"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                                                    &nbsp;&nbsp;<asp:LinkButton ID="lnkAtualizarHora" runat="server" CssClass="btn btn-info"><i class="fa fa-clock-o"></i>&nbsp;Atualizar Hora</asp:LinkButton>
                                                    &nbsp;&nbsp;<asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn btn-default"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <asp:Panel runat="server" ID="pnlRD01" Visible="false">
                <div class="alert alert-info" style="text-align: center;">
                    <h2>
                        <asp:Label ID="Label2" runat="server" Text="Relatorio de Histórico de Temperatura por Termômetros" Font-Size="20px" Style="color: rgb(055, 119, 188);" /></h2>
                </div>
                <asp:UpdatePanel runat="server" ID="upRD01">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterRD01" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <%--<% if (this.termometro_id < 1000)
                                                                   {%>--%>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro_1" Text="Termômetro" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura_1" Text="Temperatura" ForeColor="White" /></th>
                                                                <th style="width: 20%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkUltimaLeitura_1" Text="Ultima Leitura" ForeColor="White" /></th>
                                                                <%--<%} %>
                                                                <% if (this.termometro_id >= 1000)
                                                                   {%>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro_2" Text="Termômetro" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura_2" Text="Sensor 1" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura_3" Text="Sensor 2" ForeColor="White" /></th>
                                                                <th style="width: 20%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkUltimaLeitura_2" Text="Ultima Leitura" ForeColor="White" /></th>
                                                                <%} %>--%>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <%--<% if (this.termometro_id < 1000)
                                                               {%>--%>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 20%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <%--<%} %>
                                                            <% if (this.termometro_id >= 1000)
                                                               {%>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_2") %></td>
                                                            <td style="width: 20%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <%} %>--%>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <%--<% if (this.termometro_id < 1000)
                                                               {%>--%>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Temperatura_1")) %></td>
                                                            <td style="width: 20%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <%--<%} %>
                                                            <% if (this.termometro_id >= 1000)
                                                               {%>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Temperatura_1")) %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Temperatura_2")) %></td>
                                                            <td style="width: 20%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <%} %>--%>
                                                        </tr>
                                                    </table>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="lnkPrimeiraPagina" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkPaginaAnterior" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" Selected="True" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="lnkProximaPagina" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="lnkUltimaPagina" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalRD01" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />

                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRD01">
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
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlRD02" Visible="false">
                <div class="alert alert-info" style="text-align: center;">
                    <h2>
                        <asp:Label ID="Label3" runat="server" Text="Relatorio de Histórico de Status de Termômetros" Font-Size="20px" Style="color: rgb(055, 119, 188);" /></h2>
                </div>
                <asp:UpdatePanel runat="server" ID="upRD02">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterRD02" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro" Text="Termômetro" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkData" Text="Data" ForeColor="White" /></th>
                                                                <th style="width: 20%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkOcorrencia" Text="Ocorrencia" ForeColor="White" /></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <td style="width: 20%; height: 20px; text-align: left; padding: 0px 5px 0px 5px;"><%# Eval("Ocorrencia") %></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Leitura") %></td>
                                                            <td style="width: 20%; height: 20px; text-align: left; padding: 0px 5px 0px 5px;"><%# Eval("Ocorrencia") %></td>
                                                        </tr>
                                                    </table>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label6" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton4" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" Selected="True" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="LinkButton5" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton6" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalRD02" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />

                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upRD02">
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
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlRD03" Visible="false">
                <div class="alert alert-info" style="text-align: center;">
                    <h2>
                        <asp:Label ID="Label4" runat="server" Text="Relatorio de Abrangencia para Baixas Temperaruras" Font-Size="20px" Style="color: rgb(055, 119, 188);" /></h2>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterRD03" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro" Text="Termômetro" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkSecao" Text="Seção" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura" Text="Temperatura" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkVelocidade" Text="Velocidade" ForeColor="White" /></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Secao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Velocidade")) %></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Secao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Velocidade")) %></td>
                                                        </tr>
                                                    </table>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label7" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton7" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton8" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" Selected="True" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="LinkButton9" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton10" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalRD03" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />

                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="upRD02">
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
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlRD04" Visible="false">
                <div class="alert alert-info" style="text-align: center;">
                    <h2>
                        <asp:Label ID="Label5" runat="server" Text="Relatorio de Abrangencia para Altas Temperaruras" Font-Size="20px" Style="color: rgb(055, 119, 188);" /></h2>
                </div>
                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                    <ContentTemplate>
                        <div class="row">
                            <div class="form-group col-xs-12 table-responsive">
                                <table class="nav-justified">
                                    <tr>
                                        <td>
                                            <asp:Repeater ID="RepeaterRD04" runat="server">
                                                <HeaderTemplate>
                                                    <table class="table table-hover table-curved pro-table">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTermometro" Text="Termômetro" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkSecao" Text="Seção" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center; border-right: 1px solid rgb(0, 72, 89);">
                                                                    <asp:LinkButton runat="server" ID="lnkTemperatura" Text="Temperatura" ForeColor="White" /></th>
                                                                <th style="width: 10%; height: 20px; background-color: rgb(153, 153, 183); color: white; font-size: 1.5em; text-align: center;">
                                                                    <asp:LinkButton runat="server" ID="lnkVelocidade" Text="Velocidade" ForeColor="White" /></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Secao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Velocidade")) %></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <table class="table table-hover table-curved pro-table " style="width: 100%;">
                                                        <tr>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Estacao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Secao") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; border-right: 1px solid rgb(0, 72, 89); padding: 0px 5px 0px 5px;"><%# Eval("Temperatura_1") %></td>
                                                            <td style="width: 10%; height: 20px; text-align: center; padding: 0px 5px 0px 5px;"><%# string.Format("{0:0,0.00}", Eval("Velocidade")) %></td>
                                                        </tr>
                                                    </table>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="padding-top: 10px;">
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="Label8" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton11" runat="server" OnClick="lnkPrimeiraPagina_Click" ToolTip="Primeira página"><i class="fa fa-fast-backward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton12" runat="server" OnClick="lnkPaginaAnterior_Click" ToolTip="Página anterior"><i class="fa fa-backward"></i></asp:LinkButton>&nbsp;
                                                        &nbsp; Itens por página: &nbsp;
                                                        <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single">
                                                            <asp:ListItem Text="10" Value="10" />
                                                            <asp:ListItem Text="20" Value="20" />
                                                            <asp:ListItem Text="30" Value="30" Selected="True" />
                                                            <asp:ListItem Text="40" Value="40" />
                                                            <asp:ListItem Text="50" Value="50" />
                                                            <asp:ListItem Text="100" Value="100" />
                                                            <asp:ListItem Text="200" Value="200" />
                                                            <asp:ListItem Text="300" Value="300" />
                                                            <asp:ListItem Text="400" Value="400" />
                                                            <asp:ListItem Text="500" Value="500" />
                                                            <asp:ListItem Text="1000" Value="1000" />
                                                        </asp:DropDownList>
                                                        &nbsp;
                                                        <asp:LinkButton ID="LinkButton13" runat="server" OnClick="lnkProximaPagina_Click" ToolTip="Próxima página"><i class="fa fa-forward"></i></asp:LinkButton>
                                                        &nbsp; 
                                                        <asp:LinkButton ID="LinkButton14" runat="server" OnClick="lnkUltimaPagina_Click" ToolTip="Última página"><i class="fa fa-fast-forward"></i></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14" style="text-align: left;">
                                            <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                            <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                            <asp:Label runat="server" ID="lblTotalRD04" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                        </td>
                                    </tr>
                                </table>

                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="upRD02">
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress3" runat="server" AssociatedUpdatePanelID="upTermometros">
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
</asp:Content>
