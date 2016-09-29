<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popupTeste.aspx.cs" Inherits="LFSistemas.VLI.ACTWeb.Web.popupTeste" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ACTWEB</title>
    <style type="text/css">
        #UpdatePanel1, #UpdatePanel2, #UpdateProgress1 {
            border-right: gray 1px solid;
            border-top: gray 1px solid;
            border-left: gray 1px solid;
            border-bottom: gray 1px solid;
        }

        #UpdatePanel1, #UpdatePanel2 {
            width: 200px;
            height: 200px;
            position: relative;
            float: left;
            margin-left: 10px;
            margin-top: 10px;
        }

        #UpdateProgress1 {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            z-index: 9999;
            position: absolute;
            background-color: whitesmoke;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <script type="text/javascript">
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_initializeRequest(InitializeRequest);
            function InitializeRequest(sender, args) {
                if (prm.get_isInAsyncPostBack()) {
                    args.set_cancel(true);
                }
            }
            function AbortPostBack() {
                if (prm.get_isInAsyncPostBack()) {
                    prm.abortPostBack();
                }
            }
        </script>
        <div>
            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <%=DateTime.Now.ToString() %>
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="Refresh Panel" OnClick="Button_Click" />
                    <br />
                    Clicking the button while an asynchronous postback is in progress will
                    cancel the new postback. New postbacks are only allowed if one is not
                    already in progress.
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    Update in progress...
                <input type="button" value="stop" onclick="AbortPostBack()" />
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </form>
</body>
</html>
