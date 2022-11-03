<%@ Page Title="로그인" Language="C#" Theme="Main" MasterPageFile="~/Master/HomeSite.master" AutoEventWireup="true"
    CodeFile="Login.aspx.cs" Inherits="Account_Login" %>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
<div style="width:345px; margin:0 auto; padding-top:50px; text-align:center;">
    <span style="font-size:30px;">CafeSI </span><span style="font-size:20px;"> management</span>
</div>
<div style="width:345px; margin:0 auto; padding-top:20px;">
    <div style="background-color:#1E1E2D;  border-radius:0.475rem; padding-top:20px; height:350px; text-align:center;">
        <table style="border:0px;">
            <tr>
                <td style="padding-left:30px; width:50%; text-align:center;">

                    <table>
                        <tr>
                            <td style="height:80px;">
                                <div style=" width:100%; padding-bottom:15px; font-size:15px; font-family: 맑은 고딕; font-weight:bold; color:#fff; border-radius:0.475rem; ">
            로그인 정보를 입력해 주세요 <span style="font-size:11px; color:#666;"><br /> [ 접속 IP : <asp:Label ID="lb접속IP" runat="server" Text=""></asp:Label> ]</span>
        </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:50px;padding-bottom:20px;">
                                <div style="font-size:13px; font-weight:bold; text-align:left; margin-bottom:7px; width:90%;"> 아이디 </div>
                                <asp:TextBox ID="tbUserID" runat="server" MaxLength="20" CssClass="login_tb" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:50px;padding-bottom:20px;">
                                <div style="font-size:13px; font-weight:bold; text-align:left; margin-bottom:7px; width:90%;"> 비밀번호 </div>
                                <asp:TextBox ID="tbPassword" runat="server" CssClass="login_tb" TextMode="Password" MaxLength="30" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="height:50px;">
                                <asp:Button ID="LoginButton" runat="server" Text="로그인 하기" CssClass="login_bt2" ValidationGroup="LoginUserValidationGroup" onclick="LoginButton_Click" Width="270px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lb인증번호결과" runat="server" Text=""></asp:Label>
                                <asp:Label ID="lb로그인결과" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
        
    </div>
</div>

<br />

<div style="width:100%; text-align:center; font-size:10px; color:#aaa;">
    COPYRIGHT 2020 석천정보통신
</div>
 
</asp:Content>