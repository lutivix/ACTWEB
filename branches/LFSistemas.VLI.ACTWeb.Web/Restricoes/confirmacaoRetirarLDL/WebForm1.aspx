<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.Restricoes.confirmacaoRetirarLDL.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="/js/mascara.js"></script>
    <script type="text/javascript" src="/js/myFunction.js"></script>
    <script type="text/javascript" src="/js/main.js"></script>
    <script type="text/javascript" src="/js/bootstrap-dialog.js"></script>
    <script type="text/javascript" src="/js/pro.js"></script>
    <script type="text/javascript" src="/js/jquery.dataTables.js"></script>
    <script type="text/javascript" src="/js/extensions/FixedHeader/js/dataTables.fixedHeader.js"></script>
    <script type="text/javascript" src="../js/jquery-1.8.2.js"></script>
    <script type="text/javascript" src="../js/jquery-ui.js"></script>
</head>
<body>
 <form id="form1" runat="server">
    <p>
        Press button to delete product <asp:Literal ID="LiteralID" runat="server" />.
    </p>
    <asp:Button ID="ButtonDelete" Text="Delete" OnClick="ButtonDelete_Click" runat="server" />
    <%-- <asp:Button ID="ButtonDelete" Text="Delete" OnClick="myFunction()" runat="server"/>--%>

    <asp:PlaceHolder ID="PlaceWarning" Visible="false" EnableViewState="false" runat="server">
    <div id="warning">
        <p>
            Are you sure?
        </p>
        <p>
            <asp:Button ID="ButtonYes" Text="Continue" OnClick="ButtonYes_Click" runat="server" />
            <asp:Button ID="ButtonNo" Text="Cancel" OnClick="ButtonNo_Click" runat="server" />
        </p>
    </div>

    <script type="text/javascript">
        // We want the form above to be there still because it houses our buttons, but it also means our
        // code still works without js.  With js disabled the "hide" never runs so the form stays visible
        // and functioning
        //$("#warning").hide();

        var id = '<%=this.id %>';
        var person = prompt("Are you sure?","")
        if (person == id) 
        {
            $.ajax({
                type: "POST",
                url: "WebForm1.aspx/DeleteProduct",
                data: "{id:'" + id + "'}",
                contentType: "application/json; charset=utf-8",
                success: function () {
                    // if you want something to happen after the ajax call then
                    // code it here
                    alert("Product deleted");
                }
            });
        }
        else {                   
            // If you want to run a server-function when the user cancels then just
            // do an ajax call here as above, likewise you can put general js here too
            alert("Deleção Abortada!");
        }       
    </script>

    </asp:PlaceHolder>
    </form>
</body>
</html>
