<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayCache.ascx.cs" Inherits="DevBian.DemoWebSite.DisplayCache" %>

<asp:Panel ID="panelCache" runat="server">
    <asp:Label ID="labelCount" runat="server" />
    <asp:Repeater ID="repItems" runat="server">
        <HeaderTemplate>
            <table>
                <tr>
                    <th>Name</th>
                    <th>Value</th>
                </tr>
        </HeaderTemplate>
        <FooterTemplate></table></FooterTemplate>
        <ItemTemplate>
            <tr>
                <td><%# DataBinder.Eval(Container.DataItem, "Key") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "Value") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>
