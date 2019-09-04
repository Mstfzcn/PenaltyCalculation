<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="PenaltyCalculation.View.HomePage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Penalty Calculation</title>
</head>
<body>
    <form id="form1" runat="server">
        <center>

            <h1>PENALTY CALCULATION</h1><br/><br/>

            <asp:Label ID="labelReceiptDate" runat="server" Text="Receipt Date :   "></asp:Label>
            <asp:TextBox ID="receiptDate" runat="server" placeholder="From" type="date"></asp:TextBox>

            <br/><br/>

            <asp:Label ID="labelDeliveryDate" runat="server" Text="Receipt Date :   "></asp:Label>
            <asp:TextBox ID="deliveryDate" runat="server" placeholder="From" type="date"></asp:TextBox>

            <br/><br/>
            
            <asp:Label ID="labelCountry" runat="server" Text="Country :   "></asp:Label>
            <asp:DropDownList ID="DDListCountry" runat="server" OnInit="DDListCountry_Init"></asp:DropDownList>

            <br/><br/>

            <asp:Button ID="buttonCalc" runat="server" Text="Calculate" OnClick="buttonCalc_Click"></asp:Button>

            <br/><br/>
        </center>
    </form>
</body>
</html>
