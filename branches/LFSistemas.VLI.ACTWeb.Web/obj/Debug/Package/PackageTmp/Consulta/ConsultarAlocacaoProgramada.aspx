<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="ConsultarAlocacaoProgramada.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.ConsultarAlocacaoProgramada" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    <table class="nav-justified">
        <tr>
            <td style="width: 79%; text-align: left;">
                <div class="alert alert-success">
                    <h2>
                        <%--<asp:Image runat="server" ImageUrl="/img/radio-b.png" />--%>
                        <asp:Label ID="Label1" runat="server" Text="Alocação Programada" Font-Size="20px" Style="color: rgb(0, 100, 0);" CssClass="menu-item-icon menu-icon-radio" /></h2>
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
    <script type="text/javascript">
        $(document).keydown(function (e) {
            if (e.which == 120) {
                $('#link1').click();
                document.getElementById('link1').click();
                e.preventDefault();
            }
        });

        $(function () {
            $("#dvAccordian").accordion({
                collapsible: true,
                active: false,
                clearStyle: true

            });
            $('#dvAccordian .ui-accordion-content').show();
        });


    </script>
    <style>
        .ativo-S {
            color: rgb(0, 72, 89);
        }

        .ativo-N {
            color: rgb(204, 102, 51);
        }

        .linha1 {
            color: rgb(0, 0, 0);
            background-color: rgb(255, 255,255);
        }

            .linha1:hover {
                color: rgb(0, 0, 0);
                background-color: rgb(255, 255,255);
            }

        .linha2 {
            color: rgb(0, 0, 0);
            background-color: rgb(250, 250, 250);
        }

            .linha2:hover {
                color: rgb(0, 0, 0);
                background-color: rgb(255, 255,255);
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

        .auto-style1 {
            width: 82%;
        }
    </style>
    <div class="row">
        <div class="well well-sm">
            <asp:Label runat="server" ID="ldltexto" />
            <div class="page-header sub-content-header">
                <%--<h2>Filtros de Pesquisa</h2>--%>
                <a id="link1" data-toggle="collapse" title="Filtros" data-parent="#macros" href="macros#filtros" style="margin-left: 3px; font-size: 15px" accesskey="F9"><b>Filtros</b> <i class="fa fa-search"></i></a>
            </div>
            <div id="filtros" style="margin-top: 1%; margin-left: 1%; margin-right: 1%; margin-bottom: 1%; text-align: left;">
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 170px;">
                            <label for="data_fim">Corredor:</label>
                            <br />
                            <asp:CheckBoxList runat="server" ID="clbCorredor" Rows="7" CssClass="form-control" SelectionMode="Multiple" Width="160" Height="50" >
                                <asp:ListItem Text="&nbsp;&nbsp;Centro Leste" Value="1" Selected="True" />
                                <asp:ListItem Text="&nbsp;&nbsp;Minas Rio" Value="2" Selected="True" />
                            </asp:CheckBoxList>
                        </td>
                    </tr>

                    <tr>
                        <td colspan="3">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:LinkButton runat="server" ID="lnkFiltroPesquisar" CssClass="btn btn-primary" Text="Pesquisar" OnClick="lnkFiltroPesquisar_Click" ToolTip="Pesquisa conforme filtro informado." Width="150"><i class="fa fa-search"></i>&nbsp;Pesquisar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkFiltroLimpar" CssClass="btn btn-primary" Text="Limpar" ToolTip="Limpa dados do filtro de pesquisa e atualiza lista." Width="150"><i class="fa fa-long-arrow-left"></i>&nbsp;Limpar</asp:LinkButton>
                            <asp:LinkButton runat="server" ID="lnkFiltroNovo" CssClass="btn btn-success" Text="Novo" OnClick="lnkFiltroNovo_Click" ToolTip="Cadastra novo." Width="150"><i class="fa fa-plus"></i>&nbsp;Novo</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="page-header sub-content-header">
            <h3>Resultados da Pesquisa</h3>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-12 table-responsive">
            <table class="nav-justified">
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="pnlRepiter" ScrollBars="Vertical" Height="380" Style="scrollbar-arrow-color: white; scrollbar-face-color: gray;">
                            <div id="dvAccordian" class="panel-collapse collapse in" style="width: 100%; ">
                                <asp:Repeater ID="RepeaterItens" runat="server">
                                    <HeaderTemplate>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="cursor: pointer; width: 100%; background-color: rgb(55, 119, 188); border: 1px solid rgb(0, 72, 89); color: white;">
                                            <tr onclick="javascript:window.location='/Cadastro/Cadastro_AlocacaoProgramada.aspx?di=<%# LFSistemas.VLI.ACTWeb.Entities.Uteis.Criptografar(Eval("Corredor_ID").ToString(), "a#3G6**@") %>'">
                                                <th style="width: 600px; text-align: center; font-size: 14pt;"><%#Eval("Corredor") %></th>
                                                <th style="width: 100px; text-align: center; font-size: 14pt;">Meta</th>
                                            </tr>
                                        </table>
                                        <div>
                                            <asp:Panel runat="server" ScrollBars="Vertical" Height='<%# DataBinder.Eval(Container.DataItem, "Localidades").ToString().Count()  %>' Width="100%">
                                                <asp:Repeater ID="repPctm" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "Localidades") %>' >
                                                    <HeaderTemplate>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <table>
                                                            <tr style="background: #eee;">
                                                                <td style="width: 600px; height: 20px; padding-left: 10px; padding-right: 10px; text-align: left; "><%#Eval("Local") %></td>
                                                                <td style="width: 100px; height: 20px; padding-left: 10px; padding-right: 10px; text-align: center; "><%#Eval("Meta") %></td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <table >
                                                            <tr>
                                                                <td style="width: 600px; height: 20px; padding-left: 10px; padding-right: 10px; text-align: left; "><%#Eval("Local") %></td>
                                                                <td style="width: 100px; height: 20px; padding-left: 10px; padding-right: 10px; text-align: center; "><%#Eval("Meta") %></td>
                                                            </tr>
                                                        </table>
                                                    </AlternatingItemTemplate>
                                                </asp:Repeater>
                                            </asp:Panel>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="14" style="text-align: left;">
                        <hr style="color: rgb(0, 72, 89); padding: 0px 5px 0px 5px;" />
                        <asp:Label runat="server" Text="Registros: " Font-Bold="true" Font-Size="12" Style="color: rgb(153, 153, 153);" />
                        <asp:Label runat="server" ID="lblTotal" Font-Bold="true" Font-Size="12" Style="color: rgb(0, 72, 89);" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
