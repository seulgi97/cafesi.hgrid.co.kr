<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="월별매출현황.aspx.cs" Inherits="Menu_월별매출통계" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
	<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
		<ProgressTemplate>
			<div id="div_loading" style="position:absolute;top:0px;left:0px;padding-top:200px;width:100%;height:800px;background:#FFFFFF;text-align:center;"><img alt="" src="/images/loading.gif" /></div>
		</ProgressTemplate>
	</asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

    <h2>
        &nbsp;TOP
    </h2>

    <table style="border:0px; " class="tb2" cellspacing="0" cellpadding="0">
        <tr>
            <th>당월 판매TOP10</th>
            <th>1년간 판매TOP10</th>
        </tr>
        <tr>
            <td style="vertical-align:top">
                
                <asp:GridView ID="gv당월대리점" runat="server"
                        AutoGenerateColumns="true" GridLines="None" EnableTheming="True"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center"
                        AllowPaging="false" EmptyDataText=" 없음"              
                        >
                        <Columns>
                        </Columns>
                </asp:GridView>
            </td>
            <td style="vertical-align:top">
                
                <asp:GridView ID="gv년간대리점" runat="server"
                        AutoGenerateColumns="true" GridLines="None" EnableTheming="True"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center"
                        AllowPaging="false" EmptyDataText=" 없음"              
                        >
                        <Columns>
                        </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
<br />
    <h2>
        &nbsp; 월별매출통계
    </h2>
    <div style="border:1px solid #e6e6e6; padding:10px;">
        <div style="padding:5px"> 
            <asp:TextBox ID="tbexcel" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>
            <asp:DropDownList ID="dd업체명" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="bt새로고침_Click"></asp:DropDownList>
            <asp:DropDownList ID="dd매출년도" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="bt새로고침_Click"></asp:DropDownList>

            검색어 : <asp:TextBox ID="tb검색어" runat="server" SkinID="tb1"></asp:TextBox>

            <asp:Button ID="bt새로고침" runat="server" Text="검색" SkinID="bt1" OnClick="bt새로고침_Click" />
            <asp:Button ID="bt엑셀다운로드" runat="server" Text="엑셀다운" SkinID="bt2" OnClick="bt엑셀다운로드_Click" />

            [ <asp:Label ID="lb매출수" runat="server" Text="0"></asp:Label> 건 ]
        </div>
        <div style="padding:5px"> * 총판별 월별매출
            <asp:Button ID="bt총판엑셀" runat="server" Text="엑셀다운" SkinID="bt2" OnClick="bt총판엑셀_Click" />
            <asp:TextBox ID="hexcel1" runat="server" SkinID="tb1" Visible="false"></asp:TextBox> </div>

        <asp:GridView ID="gv총판월매출" runat="server"
                AutoGenerateColumns="true" GridLines="None" EnableTheming="True"
                Width="100%" CellPadding="4" SkinID="GridView1"
                RowStyle-HorizontalAlign="Center"
                AllowPaging="false" EmptyDataText="van_result  없음"              
                >
                <Columns>
                </Columns>
        </asp:GridView>
        
        <div style="padding:5px"> * 대리점별 월별매출 
            <asp:Button ID="bt대리점엑셀" runat="server" Text="엑셀다운" SkinID="bt2" OnClick="bt대리점엑셀_Click" />
            <asp:TextBox ID="hexcel2" runat="server" SkinID="tb1" Visible="false"></asp:TextBox> </div>
        <asp:GridView ID="gv대리점월매출" runat="server"
                AutoGenerateColumns="true" GridLines="None" EnableTheming="True"
                Width="100%" CellPadding="4" SkinID="GridView1"
                RowStyle-HorizontalAlign="Center"
                AllowPaging="false" EmptyDataText="van_result  없음"              
                >
                <Columns>
                </Columns>
        </asp:GridView>
        
        <div style="padding:5px"> * 자판기별 월별매출 
            <asp:Button ID="bt자판기엑셀" runat="server" Text="엑셀다운" SkinID="bt2" OnClick="bt자판기엑셀_Click"/>
            <asp:TextBox ID="hexcel3" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>  </div>
        <asp:GridView ID="gv월매출리스트" runat="server"
                AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                Width="100%" CellPadding="4" SkinID="GridView1"
                RowStyle-HorizontalAlign="Center" OnRowCommand="gv단말기리스트_RowCommand"
                AllowPaging="false" EmptyDataText="van_result  없음"              
                >
                <Columns>
                    <asp:BoundField DataField="총판명" HeaderText="총판명" />
                    <asp:BoundField DataField="대리점명" HeaderText="대리점명" />
                    <asp:BoundField DataField="자판기번호" HeaderText="자판기번호" />
                    <asp:BoundField DataField="1월" HeaderText="1월" />
                    <asp:BoundField DataField="1금액" HeaderText="금액" />
                    <asp:BoundField DataField="2월" HeaderText="2월" />
                    <asp:BoundField DataField="2금액" HeaderText="금액" />
                    <asp:BoundField DataField="3월" HeaderText="3월" />
                    <asp:BoundField DataField="3금액" HeaderText="금액" />
                    <asp:BoundField DataField="4월" HeaderText="4월" />
                    <asp:BoundField DataField="4금액" HeaderText="금액" />
                    <asp:BoundField DataField="5월" HeaderText="5월" />
                    <asp:BoundField DataField="5금액" HeaderText="금액" />
                    <asp:BoundField DataField="6월" HeaderText="6월" />
                    <asp:BoundField DataField="6금액" HeaderText="금액" />
                    <asp:BoundField DataField="7월" HeaderText="7월" />
                    <asp:BoundField DataField="7금액" HeaderText="금액" />
                    <asp:BoundField DataField="8월" HeaderText="8월" />
                    <asp:BoundField DataField="8금액" HeaderText="금액" />
                    <asp:BoundField DataField="9월" HeaderText="9월" />
                    <asp:BoundField DataField="9금액" HeaderText="금액" />
                    <asp:BoundField DataField="10월" HeaderText="10월" />
                    <asp:BoundField DataField="10금액" HeaderText="금액" />
                    <asp:BoundField DataField="11월" HeaderText="11월" />
                    <asp:BoundField DataField="11금액" HeaderText="금액" />
                    <asp:BoundField DataField="12월" HeaderText="12월" />
                    <asp:BoundField DataField="12금액" HeaderText="금액" />
                    <asp:BoundField DataField="총건수" HeaderText="총건수" />
                    <asp:BoundField DataField="총금액" HeaderText="총금액" />

                </Columns>
                </asp:GridView>
    </div>
            
        
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="bt엑셀다운로드" />
            <asp:PostBackTrigger ControlID="bt총판엑셀" />
            <asp:PostBackTrigger ControlID="bt대리점엑셀" />
            <asp:PostBackTrigger ControlID="bt자판기엑셀" />
        </Triggers>
    </asp:UpdatePanel>
  </asp:Content>
