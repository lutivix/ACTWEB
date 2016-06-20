<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Macro200.ascx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro200" %>
<div class="panel panel-default" style="margin: 10px; height: 100%">
    <div class="panel-heading">
        <h3 class="panel-title">Macro
            <asp:Literal ID="LabelNumeroMacro" runat="server">200</asp:Literal></h3>
    </div>
    <div class="panel-body">
        <asp:Label ID="LabelMascara" runat="server"></asp:Label>
        <br />
        <HR size=6>
        <div class="row">
            <div class="col-xs-4">
                <strong>Loco: </strong>
            </div>
            <div class="col-xs-8">
               <asp:Label ID="LabelLocomotiva" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>MCT: </strong>
            </div>
            <div class="col-xs-8">
                <asp:Label ID="LabelMct" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Horario: </strong>
            </div>
            <div class="col-xs-8">
               <asp:Label ID="LabelHorario" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Trem: </strong>
            </div>
            <div class="col-xs-8">
                <asp:Label ID="LabelTrem" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Origem: </strong>
            </div>
            <div class="col-xs-8">
                 <asp:Label ID="LabelOrigem" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Destino: </strong>
            </div>
            <div class="col-xs-8">
                 <asp:Label ID="LabelDestino" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Tamanho: </strong>
            </div>
            <div class="col-xs-8">
            <asp:Label ID="LabelTamanho" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Peso: </strong>
            </div>
            <div class="col-xs-8">
                 <asp:Label ID="LabelPeso" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Versao OBC: </strong>
            </div>
            <div class="col-xs-8">
                 <asp:Label ID="LabelObc" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-4">
                <strong>Mapa: </strong>
            </div>
            <div class="col-xs-8">
                 <asp:Label ID="LabelMapa" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</div>
