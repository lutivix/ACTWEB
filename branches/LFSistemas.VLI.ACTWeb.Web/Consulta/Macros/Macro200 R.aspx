<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Macro200 R.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Consulta.Macros.Macro2001" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 849px;
        }
        .auto-style2 {
            width: 342px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="width: 901px">
    
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;MACRO 200<br />
         <table class="auto-style1">
             <tr>
                 <td class="auto-style2">
    
         <fieldset style="width: 334px; margin-bottom: 0px;">
         <legend>Inicialização do MCI/OBC:</legend>
             <asp:CheckBox ID="CheckBox12" runat="server"/>
             <asp:Label ID="Label12" runat="server" Text="O MCI/OBC foi inicializado."></asp:Label>
         </fieldset></td>
                 <td>
    
         <fieldset style="width: 395px">
         <legend>Modo permissivo</legend>
             <asp:CheckBox ID="CheckBox18" runat="server"/>
             <asp:Label ID="Label18" runat="server" Text="O modo permissivo foi acionado"></asp:Label>
         </fieldset></td>
             </tr>
             <tr>
                 <td class="auto-style2"><fieldset style="width: 337px">
         <legend>Erro de inicialização do MCI</legend>
             <asp:CheckBox ID="CheckBox13" runat="server"/>
            <asp:Label ID="Label13" runat="server" Text="Codigo do erro:"></asp:Label>
            <asp:TextBox ID="TextBox4" runat="server" Width="100px"></asp:TextBox>
         </fieldset></td>
                 <td>
    
         <fieldset style="width: 395px">
         <legend>Informações de sincronismo</legend>
             <asp:CheckBox ID="CheckBox19" runat="server"/>
             <asp:Label ID="Label19" runat="server" Text="Atualização normal"></asp:Label>
             <br />
             <asp:CheckBox ID="CheckBox20" runat="server"/>
             <asp:Label ID="Label20" runat="server" Text="Resposta a uma solicitação do CCO"></asp:Label>
             <br />
             <asp:CheckBox ID="CheckBox21" runat="server"/>
             <asp:Label ID="Label21" runat="server" Text="Ultrapassagem do número de ponto de troca"></asp:Label>
             <br />
             <asp:CheckBox ID="CheckBox22" runat="server"/>
             <asp:Label ID="Label22" runat="server" Text="Descarte do último ponto de troca - Buffer vazio"></asp:Label>
             <br />
             <asp:CheckBox ID="CheckBox23" runat="server"/>
             <asp:Label ID="Label23" runat="server" Text="Penalização por excesso de velocidade"></asp:Label>
             <br />

         </fieldset></td>
             </tr>
             <tr>
                 <td class="auto-style2"><fieldset style="width: 338px">
         <legend>Aplicação de freio</legend>
             <asp:CheckBox ID="CheckBox14" runat="server"/>
            <asp:Label ID="Label14" runat="server" Text="Emêrgencia pelo EOT"></asp:Label>
              <br />
              <asp:CheckBox ID="CheckBox15" runat="server"/>
            <asp:Label ID="Label15" runat="server" Text="Excesso de velocidade"></asp:Label>
              <br />
              <asp:CheckBox ID="CheckBox16" runat="server"/>
            <asp:Label ID="Label16" runat="server" Text="Invasão de limite de licenciamento"></asp:Label>
              <br />
              <asp:CheckBox ID="CheckBox17" runat="server"/>
            <asp:Label ID="Label17" runat="server" Text="Descarrilamento DDC:"></asp:Label>
            <asp:TextBox ID="TextBox5" runat="server" Width="80px"></asp:TextBox>

           
         </fieldset></td>
                 <td>
    
         <fieldset style="width: 395px">
         <legend>Primeiro ponto pendente ou último ponto descartado</legend>Latitude:
             <asp:TextBox ID="TextBox6" runat="server" Width="90px"></asp:TextBox>
&nbsp;&nbsp; Longitude:
             <asp:TextBox ID="TextBox7" runat="server" Width="90px"></asp:TextBox>
         </fieldset></td>
             </tr>
         </table>
    
    </div>
    </form>
</body>
</html>
