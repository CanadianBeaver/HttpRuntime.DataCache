<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayCache.ascx.cs" Inherits="DevBian.DemoWebSite.DisplayCache" %>

<asp:Panel ID="panelCache" runat="server" GroupingText="Cache">
    <asp:Repeater ID="repItems" runat="server">
        <HeaderTemplate>
            <table>
                <tr>
                    <th>&nbsp;</th>
                    <th>Name</th>
                    <th>Value</th>
                </tr>
        </HeaderTemplate>
        <FooterTemplate></table></FooterTemplate>
        <ItemTemplate>
            <tr>
                <td><%# DataBinder.Eval(Container.DataItem, "Index") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "Key") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "Value") %></td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</asp:Panel>

<style>
    #<%= panelCache.ClientID %> {
        margin-top: 12px;
        color: dimgray;
    }
    #<%= panelCache.ClientID %> fieldset {
        border: none;
        padding: 0px;
    }
    #<%= panelCache.ClientID %> fieldset legend {
        background-color: antiquewhite;
        width: 100%;
        padding: 9px;
        margin: 0px;
    }
    #<%= panelCache.ClientID %> fieldset table{
        border: none;
        width: 100%;
        margin: 0px;
    }
    #<%= panelCache.ClientID %> fieldset table th, #<%= panelCache.ClientID %> fieldset table td {
        background-color: ghostwhite;
        padding: 9px;
        text-align: left;
        font-weight: normal;
    }
    #<%= panelCache.ClientID %> fieldset table td:first-child {
        text-align: right;
    }
    #<%= panelCache.ClientID %> fieldset table td:last-child {
        background-color: floralwhite;
        width: 100%;
        padding: 9px;
    }
</style>
