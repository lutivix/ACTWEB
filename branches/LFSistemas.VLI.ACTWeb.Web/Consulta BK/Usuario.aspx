<%@ Page Title="" Language="C#" MasterPageFile="~/ACTWEB.Master" AutoEventWireup="true" CodeBehind="Usuario.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Usuario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPageTitle" runat="server">
    Consulta Usuario
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" runat="server">
    <div class="page-header sub-content-header">
            <h2>Filtros de Pesquisa</h2>
            <a data-toggle="collapse" data-parent="#macros" href="macros#filtros" style="margin-left: 3px;"><i class="fa fa-sort"></i></a>
        </div>

    <div id="filtros" class="collapse">
        <div class="well list-filters">
            <div class="row">
                <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                    <label for="grupo">Nível</label>
                    <select id="grupo" name="grupo" class="form-control">
                        <option value="">Exibir todos</option>
                        <option value="1">Apoio</option>
                        <option value="2">Administrador</option>
                    </select>
                </div>
                <div class="form-group col-lg-4 col-md-6 col-sm-6 col-xs-12">
                    <label for="matricula">Matrícula</label>
                    <input type="text" id="matricula" name="matricula" class="form-control">
                </div>
                <div class="form-group col-lg-8 col-md-12 col-sm-12 col-xs-12">
                    <label for="nome">Nome</label>
                    <input type="text" id="nome" name="nome" class="form-control">
                </div>
                <div class="form-group col-lg-4 ajustar-lg col-md-12 col-sm-12 col-xs-12 text-align-right">
                    <button type="button" class="btn btn-link pro-btn inline-button ajustar-lg">Limpar</button>
                    <asp:Button ID="ButtonPesquisar" CssClass="btn btn-primary pro-btn inline-button ajustar-lg" Text="Pesquisar" runat="server" OnClick="ButtonPesquisar_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="form-group">
        <div class="page-header sub-content-header">
            <h2>Resultados da Pesquisa</h2>
            <a class="btn btn-default" href="/Cadastro/Usuario.aspx" title="Adicionar Novo">Adicionar Novo
        </a>
        </div>
    </div>
    <div class="row">
        <div class="form-group col-xs-12 table-responsive">
            <asp:Repeater ID="RepeaterItens" runat="server">
                <HeaderTemplate>
                    <table class="table table-hover table-curved pro-table">
                        <thead>
                            <tr>
                                <th><a href="#">Nome</a></th>
                                <th><a href="#">Matricula</a></th>
                                <th><a href="#">Nivel Acesso</a></th>
                                <th><a href="#">Codigo Maleta</a></th>
                                <th><a href="#">Email</a></th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("Nome")%> </td>
                        <td><%# Eval("Matricula")%> </td>
                        <td><%# Eval("NivelAcessoId")%> </td>
                        <td><%# Eval("CodigoMaleta")%> </td>
                        <td><%# Eval("Email")%> </td>
                        <td>
                            <a href="/Cadastro/Usuario.aspx?id=<%# Eval("Id")%>" class="btn btn-default">
                                <i class="fa fa-search"></i>
                            </a>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
            </table>
                </FooterTemplate>
            </asp:Repeater>

        </div>
    </div>
</asp:Content>
