<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="RelatoriosAlarmes.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.RelatoriosAlarmes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <asp:Image runat="server" ImageUrl="/img/relatorio-b.png" />
                        <asp:Label ID="Label1" runat="server" Text="Relatório de Alarmes" Font-Size="20px" Style="color: rgb(0, 100, 0);" /></h2>
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
    <!--<asp:Timer ID="Temporizador" runat="server" OnTick="Temporizador_Tick" Interval="60000" />-->
    <script type="text/javascript">
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('<%=lnkPesquisar.ClientID %>').click();
                document.getElementById('<%=lnkPesquisar.ClientID %>').click();
                e.preventDefault();
            }
        });
    </script>
    <style>
        .ativo-S {
            color: rgb(0, 72, 89);
        }

        .ativo-N {
            color: rgb(204, 102, 51);
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
    <div class="well well-sm">
        <div class="page-header sub-content-header">
            <%--<h2>Filtros de Pesquisa</h2>--%>
            <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
        </div>
        <div id="filtros">
            <table style="width: 1200px; padding-left: 1em; padding-right: 1em;">
                    <tr>
                    <td style="position: relative; width: 8%">
                        <label for="data_inicio">Data:</label>
                        <asp:TextBox ID="txtDataInicio" runat="server" onblur="validaData(this,this.value)" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);" />
                        <asp:CalendarExtender runat="server" ID="cetxtDataInicio" TargetControlID="txtDataInicio" />
                    </td>
                    <td style="width: 8%">
                         <label for="hora_inicio">Hora:</label>
                         <asp:TextBox ID="txtHoraInicio" runat="server" onKeyUp="formatar(this, '##:##')" CssClass="form-control" MaxLength="5" onkeypress="return fnValidaNroDoisPontos(event);" />
                    </td>
                    <td style="width: 5%;">
                         <label for="data_fim">Mais Hora(s):</label>
                         <br />
                         <asp:DropDownList runat="server" ID="ddlMais" CssClass="form-control">
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
                    <td style="width: 7%">
                         <label for="data_fim">Direção:</label>
                         <br />
                         <asp:RadioButton ID="rdParaFrente" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para frente" />
                         <br />
                         <asp:RadioButton ID="rdTras" runat="server" GroupName="Espaco" Text="&nbsp;&nbsp;Para Trás" Checked="true" />
                    </td>
                    <td style="width: 12%">
                        <label for="perfil">Corredores:</label>
                        <asp:Panel runat="server" Height="110" CssClass="form-control" ScrollBars="Vertical">
                            <asp:CheckBoxList runat="server" ID="cblDadosCorredores" />
                        </asp:Panel>
                    </td>
                    <td style="width: 8%">
                        <label for="perfil">Estações:</label>
                        <asp:Panel runat="server" Height="110" CssClass="form-control" style="" ScrollBars="Vertical">
                            <asp:CheckBoxList runat="server" ID="cblEstacoes" />
                        </asp:Panel>
                    </td>
                    <td style="width: 17%">
                        <label for="perfil">Status:</label>
                        <asp:Panel runat="server" Height="110" ScrollBars="Vertical" CssClass="form-control">
                            <asp:CheckBoxList runat="server" ID="cblStatus" />
                        </asp:Panel>
                    </td>
                    <td style="width: 7%">
                        <label for="perfil">Tipo de Alarme:</label>
                        <asp:Panel runat="server" Height="110" CssClass="form-control" ScrollBars="Vertical">
                            <asp:CheckBoxList runat="server" ID="cblTipoAlarme" />
                        </asp:Panel>
                    </td>
                    <td style="width: 15%">
                    </td>
                    
                    </tr>
                <tr>
                    <td style="width: 100%; padding-top: 10px;" colspan="5">
                        <asp:LinkButton runat="server" ID="lnkPesquisar" CssClass="btn btn-success" OnClick="lnkPesquisar_Click" ToolTip="Pesquisa palavra conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                        <asp:LinkButton runat="server" ID="lnkLimpar" CssClass="btn btn-primary" OnClick="lnkLimpar_Click" ToolTip="Limpa os filtros de pesquisa." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                        <asp:LinkButton ID="lnkExcel" runat="server" CssClass="btn btn-default" OnClick="lnkExcel_Click" ToolTip="Exporta registros para o Excel." Width="150"><i class="fa fa-table"></i>&nbsp;Gerar Excel</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-12 table-responsive">
            <table class="nav-justified">
                <tr>
                    <td>
                        <asp:Repeater ID="RepeaterItens" runat="server">
                            <HeaderTemplate>
                                <table class="table table-hover table-curved pro-table">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkCorredor" OnClick="lnkCorredor_Click" Text="Corredor" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkEstacao" OnClick="lnkEstacao_Click" Text="Estação" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkDscEst" OnClick="lnkDscEst_Click" Text="Descrição Estação" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkStatus" OnClick="lnkStatus_Click" Text="Status Alarme" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkParametros" OnClick="lnkParametros_Click" Text="Parâmetros" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkDtIni" OnClick="lnkDtIni_Click" Text="Data Início" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkReconhecido" OnClick="lnkReconhecido_Click" Text="Reconhecido" ForeColor="White" /></th>
                                            <th style="width: 10%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188); border-right: 1px solid rgb(0, 72, 89);">
                                                <asp:LinkButton runat="server" ID="lnkDtFim" OnClick="lnkDtFim_Click" Text="Data Fim" ForeColor="White" /></th>
                                            <th style="width: 30%; text-align: center; vertical-align: middle; font-size: 12pt; background-color: rgb(55, 119, 188);">
                                                <asp:LinkButton runat="server" ID="lnkDscAlarme" OnClick="lnkDscAlarme_Click" Text="Descrição Alarme" ForeColor="White" /></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr class="linha1" style="cursor: pointer;">
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor")%>"><%# Eval("Corredor")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao")%>"><%# Eval("Estacao")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao_Estacao")%>"><%# Eval("Descricao_Estacao")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Status_Alarme")%>"><%# Eval("Status_Alarme")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Parametros")%>"><%# Eval("Parametros")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataINI")%>"><%# Eval("dataINI")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataREC")%>"><%# Eval("dataREC")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataFIM")%>"><%# Eval("dataFIM")%> </td>
                                    <td style="width: 30%; text-align: center; vertical-align: middle;" title="<%# Eval("Descricao_Alarme")%>"><%# Eval("Descricao_Alarme")%> </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="linha2" style="cursor: pointer;">
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Corredor")%>"><%# Eval("Corredor")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Estacao")%>"><%# Eval("Estacao")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Descricao_Estacao")%>"><%# Eval("Descricao_Estacao")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Status_Alarme")%>"><%# Eval("Status_Alarme")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("Parametros")%>"><%# Eval("Parametros")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataINI")%>"><%# Eval("dataINI")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataREC")%>"><%# Eval("dataREC")%> </td>
                                    <td style="width: 10%; text-align: center; vertical-align: middle; border-right: 1px solid rgb(0, 72, 89);" title="<%# Eval("dataFIM")%>"><%# Eval("dataFIM")%> </td>
                                    <td style="width: 30%; text-align: center; vertical-align: middle;" title="<%# Eval("Descricao_Alarme")%>"><%# Eval("Descricao_Alarme")%> </td>
                                </tr>
                            </AlternatingItemTemplate>
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
                                    <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" Width="80" CssClass="form-control-single" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="30" Value="30" />
                                        <asp:ListItem Text="40" Value="40" />
                                        <asp:ListItem Text="50" Value="50" Selected="True" />
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
                                <td colspan="14" style="text-align: left; color: rgb(0, 72, 89);">
                                    <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                                    <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                                    <asp:Label runat="server" ID="Label2" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="14" style="text-align: left; color: rgb(0, 72, 89);">
                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                    </td>
                </tr>
            </table>

        </div>
    </div>
</asp:Content>
