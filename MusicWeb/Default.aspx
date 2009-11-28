<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MusicWeb._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        歌名：<asp:TextBox ID="txtMusicName" runat="server"></asp:TextBox>
        <asp:Button ID="btnSearch"
            runat="server" Text="Search" onclick="btnSearch_Click" />
    
    <asp:GridView ID="grdMain" runat="server">
    </asp:GridView>
    
    </div>
    </form>
    </body>
</html>
