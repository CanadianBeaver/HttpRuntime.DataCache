<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DevBian.DemoWebSite.Default" %>

<%@ Register Src="~/DisplayCache.ascx" TagName="DisplayCache" TagPrefix="ascx" %>
<%@ Register Src="~/ClearCacheButton.ascx" TagName="ClearCacheButton" TagPrefix="ascx" %>
<%@ Register Src="~/PopulateCacheButton.ascx" TagName="PopulateCacheButton" TagPrefix="ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        button, input[type=submit] {
            background-color: #0094ff;
            border-radius: 3px;
            color: #ffd800;
            border: 1px solid #0026ff;
            padding: 6px 9px 6px 9px;
        }
        button:disabled, input[type=submit]:disabled {
            background-color: transparent;
            border-radius: 3px;
            color: gray;
            border: 1px solid gray;
            padding: 6px 9px 6px 9px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="button1" runat="server" Text="Increment ID in cache" />
            <asp:Button ID="button2" runat="server" Text="Increment ID with deep copy" />
            <ascx:ClearCacheButton runat="server" />
            <ascx:PopulateCacheButton runat="server" />
        </div>
        <ascx:DisplayCache runat="server" />
    </form>
</body>
</html>
