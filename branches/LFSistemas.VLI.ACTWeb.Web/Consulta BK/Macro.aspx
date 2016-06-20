<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Macro.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    Listagem de Macros
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <style>
        .macro-19 {
            background-color: white;
            color: red;
        }

        .tipo-E {
            background-color: white;
            color: green;
        }
        .tipo-R {
            background-color: white;
            color: black;
        }

        .macro-19 {
            color: white;
            background-color:red
        }
    </style>

    <ul class="nav nav-tabs" role="tablist" id="myTab">
        <li class="active"><a href="#home" role="tab" data-toggle="tab">Filtros</a></li>
        <li><a href="#profile" role="tab" data-toggle="tab">Avançado</a></li>
    </ul>

    <div class="tab-content">
        <div class="tab-pane active" id="home">
            <div class="form-group">
                <div class="page-header sub-content-header">
                    <h2>Filtros de Pesquisa</h2>
                    <a data-toggle="collapse" data-parent="#macros" href="macros#filtros" style="margin-left: 3px;"><i class="fa fa-sort"></i></a>
                </div>
            </div>
            <div id="filtros" class="collapse">
                <div class="well list-filters">
                    <div class="row">
                        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
                            <label for="locomotiva">Nº Locomotiva</label>
                            <asp:TextBox ID="TextBoxNumeroLocomotiva" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
                            <label for="trem">Nº Trem</label>
                            <asp:TextBox ID="TextBoxNumeroTrem" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
                            <label for="macro">Nº Macro</label>
                            <asp:TextBox ID="TextBoxNumeroMacro" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-4 col-sm-5 col-xs-12">
                            <label for="codigoos">Código OS</label>
                            <asp:TextBox ID="TextBoxCodigoOS" runat="server" onkeypress="return PermiteSomenteNumeros(event);" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                            <label for="data_inicio">Data Inicio</label>
                            <asp:TextBox ID="TextBoxDataInicio" runat="server" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                            <label for="hora_inicio">Hora Inicio</label>
                            <asp:TextBox ID="TextBoxHoraInicio" runat="server" onKeyUp="formatar(this, '##:##:##')" CssClass="form-control" MaxLength="8" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                            <label for="data_fim">Data Fim</label>
                            <asp:TextBox ID="TextBoxDataFim" runat="server" onKeyUp="formatar(this, '##/##/####')" CssClass="form-control" MaxLength="10" onkeypress="return PermiteSomenteNumeros(event);"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                            <label for="hora_fim">Hora Fim</label>
                            <asp:TextBox ID="TextBoxHoraFim" runat="server" onKeyUp="formatar(this, '##:##:##')" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="form-group col-lg-4 col-md-12 col-sm-12 col-xs-12 text-align-right">
                            
                            <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-link pro-btn inline-button ajustar-lg" OnClick="LinkButton1_Click">Limpar</asp:LinkButton>
                            <asp:Button ID="ButtonPesquisar" CssClass="btn btn-primary pro-btn inline-button ajustar-lg" runat="server" Text="Pesquisar" OnClick="ButtonPesquisar_Click" />
                            <asp:Button ID="ButtonGerarExcel" CssClass="btn btn-success pro-btn inline-button ajustar-lg" runat="server" Text="Gerar Excel" OnClick="ButtonGerarExcel_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="tab-pane" id="profile">
            <div class="form-group">
                <div class="page-header sub-content-header">
                    <h2>Exibição de Campos</h2>
                </div>
            </div>
            <div class="well list-filters">
                <div class="row">
                    <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                        <asp:CheckBox ID="CheckER" runat="server" Text="E/R" Checked="true" />
                    </div>
                    <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                        <asp:CheckBox ID="CheckLocomotiva" runat="server" Text="LOCOMOTIVA" Checked="true" />
                    </div>
                    <div class="form-group col-lg-2 col-md-5 col-sm-5 col-xs-12">
                        <asp:CheckBox ID="CheckBoxTrem" runat="server" Text="TREM" Checked="true" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div class="form-group">
        <div class="page-header sub-content-header">
            <h2>Resultados da Pesquisa</h2>
            <a data-toggle="modal" data-target="#modal_macros" data-backdrop="static" style="margin-left: 3px;" href=""><i class="fa fa-question-circle"></i></a>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-12 table-responsive">
            <asp:Repeater ID="RepeaterItens" runat="server">
                <HeaderTemplate>
                    <table class="table table-hover table-curved pro-table " id="macros">
                        <thead>
                            <tr>
                                <% if (this.CheckER.Checked)
                                   {%>
                                <th><a href="#">E/R</a></th>
                                <%} %>
                                <% if (this.CheckLocomotiva.Checked)
                                   {%>
                                <th><a href="#">Locomotiva</a></th>
                                <%} %>
                                <th><a href="#">Trem</a></th>
                                <th><a href="#">Codigo OS</a></th>
                                <th><a href="#">Horário</a></th>
                                <th><a href="#">Macro</a></th>
                                <th><a href="#">Texto</a></th>
                                <th><a href="#">MCT</a></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr style="cursor: pointer" class="macro-<%# Eval ("NumeroMacro")%> tipo-<%# Eval("Tipo")%>" onclick="javascript:window.open('/Consulta/MacroPopUp.aspx?tipo=<%# Eval("Tipo")%>&id=<%# Eval("ID")%>', '', 'width=400, height=600')">
                        <% if (this.CheckER.Checked)
                           {%>
                        <td><%# Eval("Tipo")%> </td>
                        <%} %>
                        <% if (this.CheckLocomotiva.Checked)
                           {%>
                        <td><%# Eval("Locomotiva")%> </td>
                        <%} %>
                        <td><%# Eval("Trem")%> </td>
                        <td><%# Eval ("CodigoOS")%></td>
                        <td><%# Eval ("Horario")%></td>
                        <td title="<%# Eval ("DescricaoMacro")%>"><%# Eval ("NumeroMacro")%></td>
                        <td>
                            <div onclick="$(this).css('text-overflow', 'inherit'); $(this).css('width', '100%');" style="width: 150px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;" title="<%# Eval ("Texto")%>"><%# Eval ("Texto")%></div>
                        </td>
                        <td><%# Eval ("MCT")%></td>
                        <td></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
            </table>
                </FooterTemplate>
            </asp:Repeater>

        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('.button-show-details').click(function (evt) {
                evt.preventDefault();
                var elemento = $(this).parent().parent().next('tr');

                if (elemento.is(':visible')) {
                    elemento.fadeOut(500);
                } else {
                    elemento.fadeIn(500);
                }

            });
        });
    </script>

    <script type="text/javascript" src="/js/mascara.js"></script>

</asp:Content>
