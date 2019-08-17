<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DevBian.DemoWebSite.Default" %>

<%@ Register Src="~/DisplayCache.ascx" TagName="DisplayCache" TagPrefix="ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="label1" runat="server" />
            <br />
            <asp:Button ID="button1" runat="server" Text="Post back" />
            <asp:Button ID="button2" runat="server" Text="Post back with deep copy" />
        </div>
        <ascx:DisplayCache ID="dc" runat="server" />
    </form>
</body>
</html>
