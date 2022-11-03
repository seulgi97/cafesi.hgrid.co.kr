<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true" CodeFile="매출상세조회.aspx.cs" Inherits="Menu_매출상세조회" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">

</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        &nbsp; 매출상세조회
        <asp:TextBox ID="tbexcel" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>
        <asp:TextBox ID="선택_총판코드" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="선택_대리점코드" runat="server" Visible="false"></asp:TextBox>
        <asp:Label ID="선택_자판기코드" runat="server" Text="" Visible="false"></asp:Label>
        결제일시 : <asp:TextBox ID="tb시작일" runat="server" SkinID="tb1" Width="80"></asp:TextBox> ~ <asp:TextBox ID="tb종료일" runat="server" SkinID="tb1" Width="80"></asp:TextBox>
        <ajaxToolkit:CalendarExtender ID="cd1" runat="server" TargetControlID="tb시작일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
        <ajaxToolkit:CalendarExtender ID="cd2" runat="server" TargetControlID="tb종료일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
        <asp:Button ID="btnToday" runat="server" Text="오늘" CssClass="btn_gray" OnClick="btnToday_Click" />
        <asp:Button ID="btnMonth" runat="server" Text="이번달" CssClass="btn_gray"  OnClick="btnMonth_Click" />
        <asp:Button ID="btnPrevMonth" runat="server" Text="지난달" CssClass="btn_gray"  OnClick="btnPrevMonth_Click" />
        <asp:Button ID="bt새로고침" runat="server" Text="조회" CssClass="btn_green" OnClick="bt새로고침_Click" />
    </h2>
    <div id="layout">
        <table>
            <tr>
                <td style="vertical-align:top">
                    <div style="padding:5px">
                        <span style="font-size:15px; font-weight:bold">* 총판 ( <asp:Label ID="lb총판수" runat="server" Text=""></asp:Label> 건 ) </span>
                    </div>
                    <asp:GridView ID="gv총판" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv총판_RowCommand"
                        AllowPaging="false" EmptyDataText="총판 없음"
                        width="280px">
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:TemplateField HeaderText="총판명 [대리점/자판기]" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    [ <asp:Label ID="Label2" runat="server" Text='<%# Eval("branch_cnt") %>'></asp:Label> / 
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("machine_cnt") %>'></asp:Label> ]
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="결제금액" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_Select1"  Text='<%# Eval("pay_cnt") + "건" %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="bt_Select2"  Text='<%# Eval("pay_amount", "{0:#,##0}") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
                </td>
                <td style="vertical-align:top" runat="server" visible="false" id="td대리점">
                    <div style="padding:5px">
                        <span style="font-size:15px; font-weight:bold">* <asp:Label ID="lb선택총판명" runat="server" Text=""></asp:Label> (<asp:Label ID="lb선택총판idx" runat="server" Text=""></asp:Label>) </span>
                    </div>
                    <asp:GridView ID="gv대리점" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                        CellPadding="4" SkinID="GridView1" DataKeyNames="idx"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv대리점_RowCommand"
                        AllowPaging="false" EmptyDataText="대리점 없음"  
                        width="320px">
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:TemplateField HeaderText="대리점명 [ 자판기 ]" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    [ <asp:Label ID="Label1" runat="server" Text='<%# Eval("machine_cnt") %>'></asp:Label>  ] 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="결제금액" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_Select1"  Text='<%# Eval("pay_cnt") + "건" %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="bt_Select2"  Text='<%# Eval("pay_amount", "{0:#,##0}") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>
                </td>
                <td style="vertical-align:top" runat="server" visible="false" id="td자판기">
                    <div style="padding:5px">
                        <span style="font-size:15px; font-weight:bold">* <asp:Label ID="lb선택대리점명" runat="server" Text=""></asp:Label> (<asp:Label ID="lb선택대리점코드" runat="server" Text=""></asp:Label>) </span>
                    </div>
                    <asp:GridView ID="gv단말기리스트" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center"
                        AllowPaging="false" EmptyDataText="단말기 없음" OnRowDataBound="gv단말기리스트_RowDataBound"
                            width="700px"  OnRowCommand="gv단말기리스트_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ROWNUM" HeaderText="No" />
                            <asp:TemplateField HeaderText="설치장소 - 자판기번호">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="Label1" Text='<%# Eval("설치장소") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  /> - <asp:LinkButton runat="server" id="LinkButton1"  Text='<%# Eval("idx") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField >
                                <HeaderTemplate> <div id="container">통신 <div id="networkcircle" class="circle">?</div> </div>
                                    <div id = "networkhelp">
                                        각 아이콘에 마우스를 오버하면, 마지막 보고시간이 표시됩니다.
                                        <table>
                                            <tr><td><div class="normal">정상</div></td>
                                                <td>30분 이내에 보고가 있는 경우(정상)</td>
                                            </tr>
                                            <tr><td><div class="reqchk">점검요청</div></td>
                                                <td>30분 초과하고 6일 이내의 보고가 있는 경우(자판기를 점검하세요)</td>
                                            </tr>
                                            <tr><td><div class="nolonguse">장기미사용</div></td>
                                                <td>6일이 지난 보고</td>
                                            </tr>
                                            <tr><td><div class="nohistory">이력없음</div></td>
                                                <td>보고이력이 없는 경우</td>
                                            </tr>
                                        </table>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="machine_net" runat="server" Text='<%# Eval("통신상태") %>' Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>          
                            <asp:TemplateField  >
                                 <HeaderTemplate><div id="container">랙 <div id="rackcircle" class="circle">?</div></div>
                                       <div id = "rackhelp">
                                        각 아이콘에 마우스를 오버하면, 상세상태가 표시됩니다.
                                        <table>
                                            <tr><td><div class="normal">정상</div></td>
                                                <td>랙, 컵투출기, 출빙기 상태 모두 정상동작</td>
                                            </tr>
                                            <tr><td><div class="reqchk">점검요청</div></td>
                                                <td>각 랙과 컵투출기, 출빙기에 1개 이상의 점검이슈가 있는 경우 출력 <br />(자판기를 점검하세요)</td>
                                            </tr>
                                            <tr><td><div class="nouse">사용안함</div></td>
                                                <td>사용중인 랙이 없는 경우</td>
                                            </tr>
                                        </table>
                                    </div>
                                 </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="machine_stat" runat="server" Text='<%# Eval("machine_stat") %>' Visible="false"></asp:Label>                                   
                                </ItemTemplate>
                            </asp:TemplateField>
                           <%-- 총 결제건수 컬럼 추가--%>
                               <asp:TemplateField HeaderText="총 결제건수" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_Select1"  Text='<%# Eval("pay_cnt") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="카드결제" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>                                   
                                    <asp:LinkButton runat="server" id="bt_Select2"  Text='<%# Eval("pay_amount", "{0:#,##0}") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderText="쿠폰결제" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>                                   
                                    <asp:LinkButton runat="server" id="bt_Select3"  Text='<%# Eval("coupon_amount", "{0:#,##0}") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="총 결제금액" ItemStyle-HorizontalAlign="right">
                                <ItemTemplate>                                   
                                    <asp:LinkButton runat="server" id="bt_Select4"  Text='<%# Eval("total_amount", "{0:#,##0}") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="설치일" HeaderText="설치일" />
                            <asp:BoundField DataField="관리자연락처" HeaderText="관리연락처" />
                        </Columns>
                    </asp:GridView>
                    <asp:GridView ID="gv매출내역" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center"
                        ShowFooter="false" FooterStyle-HorizontalAlign="Right"
                        AllowPaging="false" EmptyDataText="매출내역 없음" width="700px">
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:BoundField DataField="machine_idx" HeaderText="MID" />
                            <asp:TemplateField HeaderText="설치장소">
                                <ItemTemplate>
                                    <asp:Label ID="machine_idx" runat="server" Text='<%# Eval("install_spot") %>' ToolTip='<%# Eval("machine_idx") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="pay_date" HeaderText="결제일시"/>
                            <asp:BoundField DataField="pay_type" HeaderText="구분"  />
                            <asp:BoundField DataField="pay_tid" HeaderText="TID"   />
                            <asp:BoundField DataField="pay_amount" DataFormatString="{0:#,##0}" HeaderText="카드결제" ItemStyle-HorizontalAlign="right" />
                            <asp:BoundField DataField="coupon_amount" DataFormatString="{0:#,##0}" HeaderText="쿠폰결제" ItemStyle-HorizontalAlign="right" />
                            <asp:BoundField DataField="total_amount" DataFormatString="{0:#,##0}"  HeaderText="총 결제금액" ItemStyle-HorizontalAlign="right"/>
                        </Columns>
                    </asp:GridView>
                </td>               
            </tr>
            <%-- 엑셀 다운로드 버튼 --%>
            <tr>
                <td></td>
                <td></td>
                <td align="right"  style="height:28px;">               
                <asp:LinkButton ID="btnFooter" runat="server" CausesValidation="false" CssClass="excel" OnClick="bt엑셀다운로드_Click" Visible="false">
                <asp:Image ID="imgExcel" runat="server" ImageUrl="../images/excel_logo.png" Width="20" Height="20" /> 다운로드
                </asp:LinkButton> 
                </td>
            </tr>
        </table>
    </div>
  </asp:Content>
