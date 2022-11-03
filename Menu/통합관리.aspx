<%@ Page Title="홈 페이지" Language="C#" MasterPageFile="~/Master/Site.master" AutoEventWireup="true"
    CodeFile="통합관리.aspx.cs" Inherits="Menu_총판대리점관리" Theme="Main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    .LabelWithStyle {
        background : #4c4c4c;
    }
</style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div id="div_총판등록" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 총판(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 총판명 : </td>
                            <td> <asp:TextBox ID="총판등록_총판명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 사업자번호 : </td>
                            <td>
                                <asp:TextBox ID="총판등록_사업자번호" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 대표자명 : </td>
                            <td>
                                <asp:TextBox ID="총판등록_대표자명" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 전화번호 :</td>
                            <td>
                                 <asp:TextBox ID="총판등록_전화번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰번호 :</td>
                            <td>
                                 <asp:TextBox ID="총판등록_휴대폰번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 주소 :  </td>
                            <td>
                                <asp:TextBox ID="총판등록_주소" runat="server" SkinID="tb1" MaxLength="100" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="총판등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt총판등록완료" runat="server" OnClick="bt총판등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt총판등록닫기" runat="server" OnClick="bt총판등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_총판수정" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 총판(수정)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> IDX : </td>
                            <td><asp:TextBox ID="총판수정_코드" runat="server" SkinID="tb1" MaxLength="2" Width="40px" ReadOnly="true" BackColor="LightGray"></asp:TextBox> * 총판코드는 수정이 불가능합니다
                        </tr>
                        <tr>
                            <td> 총판명 : </td>
                            <td> <asp:TextBox ID="총판수정_총판명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 사업자번호 : </td>
                            <td>
                                <asp:TextBox ID="총판수정_사업자번호" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 대표자명 : </td>
                            <td>
                                <asp:TextBox ID="총판수정_대표자명" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 전화번호 :</td>
                            <td>
                                 <asp:TextBox ID="총판수정_전화번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰번호 :</td>
                            <td>
                                 <asp:TextBox ID="총판수정_휴대폰번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 주소 :  </td>
                            <td>
                                <asp:TextBox ID="총판수정_주소" runat="server" SkinID="tb1" MaxLength="100" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="총판수정_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <!-- 배너등록버튼 추가 -->
                     <asp:Button ID="bt총판배너" runat="server" OnClick="bt총판배너등록완료_Click" CausesValidation="false" Text="배너" SkinID="bt4" />
                    <asp:Button ID="bt총판수정완료" runat="server" OnClick="bt총판수정완료_Click" CausesValidation="false" Text="수정" SkinID="bt1" />
                    <asp:Button ID="bt총판삭제완료" runat="server" OnClick="bt총판삭제완료_Click" CausesValidation="false" Text="삭제" SkinID="bt3" OnClientClick="return confirm('삭제하시겠습니까? 하위 데이터가 있는경우 삭제되지 않을 수 있습니다. '); " />
                    <asp:Button ID="bt총판수정닫기" runat="server" OnClick="bt총판수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <!-- 배너정보 등록 리스트  --> 
    <div id="div_배너관리" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;z-index:3" >
        <table cellpadding="0" cellspacing="0" style="width:350px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 배너관리
                </td>
            </tr>
            <tr>
                <td style="text-align:right;"><asp:DropDownList runat="server" id="bannersrchDropDownLst" OnSelectedIndexChanged="bannersrchDropDownLst_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList></td>
            </tr>
            <tr style="background:#FFFFA4;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                       <tr>
                        <td>
                            <asp:HiddenField ID="hCompanyIdx" runat="server" />
                            <asp:HiddenField ID="hAgencyIdx" runat="server" />
                            <asp:HiddenField ID="hBranchIdx" runat="server" />
                            <asp:HiddenField ID="hMachineIdx" runat="server" />

                            <asp:GridView ID="gv배너리스트" runat="server"
                                AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="kid"
                                CellPadding="4" SkinID="GridView1"
                                RowStyle-HorizontalAlign="Center" 
                                OnRowCommand="gv배너리스트_RowCommand"
                                ShowHeaderWhenEmpty="true"
                                AllowPaging="true" 
                                width="400px">
                                <Columns>
                                    <asp:BoundField DataField="rownum" HeaderText="No" />
                                    <asp:BoundField DataField="btype" HeaderText="구분" />                                    
                                    <asp:TemplateField  HeaderText="제목" ItemStyle-HorizontalAlign="center">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" id="bt1"  Text='<%# Eval("title") %>' CommandName="Select" CommandArgument='<%# Eval("kid") %>'  />
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:BoundField DataField="timeinterval" HeaderText="동작시간" />
                                    <asp:BoundField DataField="opdate" HeaderText="노출기간" />
                                    <asp:BoundField DataField="useyn" HeaderText="게시" />
                                </Columns>
                                <EmptyDataTemplate>                                    
                                        <asp:Label ID="lblEmpty" runat="server"  Text="조회된 데이터가 없습니다." style="color:black;font-weight:bold"></asp:Label>                                    
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                       </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="총판배너등록결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="btnBannerRegistration" runat="server" OnClick="btnBannerRegistration_Click" CausesValidation="false" Text="배너" SkinID="bt1" />
                    <asp:Button ID="btnCloseBannerRegistration" runat="server" OnClick="btnCloseBannerRegistration_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <!-- 배너등록 창 -->
    <div id="div_bannerRegistration" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;z-index:4" >
        <table cellpadding="0" cellspacing="0" style="width:600px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 배너등록
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                          <tr>
                            <td></td>
                            <td>                            
                                <asp:HiddenField ID="hCompanyIdx2" runat="server" />
                                <asp:HiddenField ID="hAgencyIdx2" runat="server" />
                                <asp:HiddenField ID="hBranchIdx2" runat="server" />
                                <asp:HiddenField ID="hMachineIdx2" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td> 구분 : </td>
                            <td>                                
                                <asp:RadioButton runat="server" ID="rdoHome" OnCheckedChanged="rdoHome_CheckedChanged" AutoPostBack="true" GroupName="grpBannerType" Text="홈광고" />
                                <asp:RadioButton runat="server" ID="rdoBand" OnCheckedChanged="rdoBand_CheckedChanged" AutoPostBack="true" GroupName="grpBannerType" Text="띠배너" />
                            </td>
                        </tr>
                        <tr>
                            <td> 제목 : </td>
                            <td> <asp:TextBox ID="txtBannerTitle" runat="server" SkinID="tb1" MaxLength="100" Width="250px" ></asp:TextBox>
                                <asp:Label ID="lblKid" runat="server" Visible="false" MaxLength="100" Width="250px" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td> 동작시간 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="bannerOptimeDropDownList"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 노출기간 : </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="bannerStartDate" runat="server" SkinID="tb1" MaxLength="10" Width="90px" /> ~ <asp:TextBox ID="bannerEndDate" runat="server" SkinID="tb1" MaxLength="10" Width="90px" />
                                </div>
                                    <ajaxToolkit:CalendarExtender ID="bCalendarStart" runat="server" TargetControlID="bannerStartDate" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                                    <ajaxToolkit:CalendarExtender ID="bCalendarEnd" runat="server" TargetControlID="bannerEndDate" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td> 게시여부 :</td>
                            <td>
                                <asp:RadioButton runat="server" ID="rdoUseY" GroupName="grpBannerYn" Text="Y" />
                                <asp:RadioButton runat="server" ID="rdoUseN" GroupName="grpBannerYn" Text="N" />
                            </td>
                        </tr>
                        <tr>
                            <td> 이미지 등록 :</td>
                            <td>
                               <%-- 홈광고 이미지 업로드  --%>
                                <div id="divHomeBannerUpload" runat="server" visible="false" >
                                    <table>
                                        <tr>
                                            <td>홈광고 이미지규격: 1080x1920px png</td><td></td><td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="txtHomebannerUpload" runat="server" accept=".png" />                                                       
                                                       <%-- <asp:Button ID="btnSaveHomeImage" runat="server" OnClick="btnSaveHomeImage_Click"
                                                        Text="Upload" />--%>
                                                    </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSaveBannerImage" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>                                        
                                            <td>
                                                <asp:Label ID="lblHomeBannerUpload" runat="server" SkinID="lb1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <%-- 띠배너 이미지 업로드  --%>
                                <div id="divBandBannerUpload" runat="server" visible="false" >
                                    <table>
                                        <tr>
                                            <td>띠배너 이미지규격: 1080x116px png</td><td></td><td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="txtBandBannerUpload" runat="server" />
                                                     <%--   <asp:Button ID="btnSaveBandImage" runat="server" OnClick="btnBandBannerFile_Click" 
                                                        Text="Upload" />--%>
                                                    </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSaveBannerImage" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>                                        
                                            <td>
                                                <asp:Label ID="lblBannerUpload" runat="server" SkinID="lb1"></asp:Label>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td>띠배너 이미지규격2: 1080x1920px png</td><td></td><td></td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:FileUpload ID="txtBandBannerUpload2" runat="server" />
                                                      <%--  <asp:Button ID="btnSaveBandImage2" runat="server" OnClick="btnBandBannerFile2_Click" 
                                                        Text="Upload" />--%>
                                                    </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnSaveBannerImage" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>                                        
                                            <td>
                                                <asp:Label ID="lblBannerUpload2" runat="server" SkinID="lb1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="3" style="text-align:center;">
                    <div runat="server" id="배너등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="btnSaveBannerImage" runat="server" OnClick="btnSavBannerFile_Click" CausesValidation="false" Text="등록" SkinID="bt1" />
                    <asp:Button ID="btnModifyBannerImage" runat="server" Visible="false" OnClick="btnModifyBannerImage_Click" CausesValidation="false" Text="수정" SkinID="bt3" />
                    <asp:Button ID="btnDeleteBannerImage" runat="server" Visible="false" OnClick="btnDeleteBannerImage_Click" CausesValidation="false" Text="삭제" SkinID="bt4" />
                    <asp:Button ID="btnCloseBanner" runat="server" OnClick="btnCloseBanner_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_대리점등록" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 대리점(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 총판 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="대리점등록_총판코드"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 대리점명 : </td>
                            <td> <asp:TextBox ID="대리점등록_대리점명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 사업자번호 : </td>
                            <td>
                                <asp:TextBox ID="대리점등록_사업자번호" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 대표자명 : </td>
                            <td>
                                <asp:TextBox ID="대리점등록_대표자명" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 전화번호 :</td>
                            <td>
                                 <asp:TextBox ID="대리점등록_전화번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰번호 :</td>
                            <td>
                                 <asp:TextBox ID="대리점등록_휴대폰번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 주소 :  </td>
                            <td>
                                <asp:TextBox ID="대리점등록_주소" runat="server" SkinID="tb1" MaxLength="100" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="대리점등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt대리점등록완료" runat="server" OnClick="bt대리점등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt대리점등록닫기" runat="server" OnClick="bt대리점등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_대리점수정" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 대리점(수정/삭제)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 총판 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="대리점수정_총판코드"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> IDX : </td>
                            <td><asp:TextBox ID="대리점수정_코드" runat="server" SkinID="tb1" MaxLength="2" Width="40px" ReadOnly="true"></asp:TextBox> 
                                *코드는 수정이 불가능합니다.
                                <asp:HiddenField ID="hCompanyIdx3" runat="server" />
                                <asp:HiddenField ID="hAgencyIdx3" runat="server" />
                                <asp:HiddenField ID="hBranchIdx3" runat="server" />
                                <asp:HiddenField ID="hMachineIdx3" runat="server" />
                        </tr>
                        <tr>
                            <td> 대리점명 : </td>
                            <td> <asp:TextBox ID="대리점수정_대리점명" runat="server" SkinID="tb1" MaxLength="100" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 사업자번호 : </td>
                            <td>
                                <asp:TextBox ID="대리점수정_사업자번호" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 대표자명 : </td>
                            <td>
                                <asp:TextBox ID="대리점수정_대표자명" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 전화번호 :</td>
                            <td>
                                 <asp:TextBox ID="대리점수정_전화번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰번호 :</td>
                            <td>
                                 <asp:TextBox ID="대리점수정_휴대폰번호" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 주소 :  </td>
                            <td>
                                <asp:TextBox ID="대리점수정_주소" runat="server" SkinID="tb1" MaxLength="100" Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="div_대리점수정결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                      <!-- 배너등록버튼 추가 -->
                     <asp:Button ID="bt대리점배너등록" runat="server" OnClick="bt대리점배너조회_Click" CausesValidation="false" Text="배너" SkinID="bt4" />
                    <asp:Button ID="bt대리점삭제완료" runat="server" OnClick="bt대리점삭제완료_Click" CausesValidation="false" Text="삭제" SkinID="bt2" OnClientClick="return confirm('정말 삭제하시겠습니까?');" />
                    <asp:Button ID="bt대리점수정완료" runat="server" OnClick="bt대리점수정완료_Click" CausesValidation="false" Text="수정" SkinID="bt1" />
                    <asp:Button ID="bt대리점수정닫기" runat="server" OnClick="bt대리점수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    
    <div id="div_사용자등록" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 사용자(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 아이디 : </td>
                            <td><asp:TextBox ID="사용자등록_아이디" runat="server" SkinID="tb1" MaxLength="10" Width="150px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 이름 : </td>
                            <td> <asp:TextBox ID="사용자등록_이름" runat="server" SkinID="tb1" MaxLength="20" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 일반전화 : </td>
                            <td>
                                <asp:TextBox ID="사용자등록_일반전화" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰 : </td>
                            <td>
                                <asp:TextBox ID="사용자등록_휴대폰" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속 :</td>
                            <td>
                                 <asp:TextBox ID="사용자등록_소속" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 총판권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_총판선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 대리점권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_대리점선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 자판기권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자등록_자판기선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="사용자등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt사용자등록완료" runat="server" OnClick="bt사용자등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt사용자등록닫기" runat="server" OnClick="bt사용자등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_사용자수정" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 사용자(수정/삭제)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 아이디 : </td>
                            <td>
                                <asp:Label ID="사용자수정_아이디" runat="server" Text=""></asp:Label>
                        </tr>
                        <tr>
                            <td> 이름 : </td>
                            <td> <asp:TextBox ID="사용자수정_이름" runat="server" SkinID="tb1" MaxLength="20" Width="150px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 일반전화 : </td>
                            <td>
                                <asp:TextBox ID="사용자수정_일반전화" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 휴대폰 : </td>
                            <td>
                                <asp:TextBox ID="사용자수정_휴대폰" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속 :</td>
                            <td>
                                 <asp:TextBox ID="사용자수정_소속" runat="server" SkinID="tb1" MaxLength="14" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 총판권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_총판선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 대리점권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_대리점선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 자판기권한 :</td>
                            <td>
                                <asp:DropDownList ID="사용자수정_자판기선택" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="사용자수정_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt사용자삭제" runat="server" OnClick="bt사용자삭제_Click" CausesValidation="false" Text="삭제하기" SkinID="bt3" OnClientClick="return confirm('사용자를 삭제하시겠습니까?'); " />
                    <asp:Button ID="bt사용자수정완료" runat="server" OnClick="bt사용자수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt사용자수정닫기" runat="server" OnClick="bt사용자수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    
    <div id="div_자판기등록" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 자판기 신규 등록
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 관리자 연락처 : </td>
                            <td> <asp:TextBox ID="자판기등록_관리자연락처" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 자판기타입 : </td>
                            <td>
                                <asp:DropDownList ID="자판기등록_자판기타입" runat="server" SkinID="dd1" AutoPostBack="true">
                                    <asp:ListItem Value="s" Text="구형(Stand)"></asp:ListItem>
                                    <asp:ListItem Value="m" Text="구형(Mini)"></asp:ListItem>
                                    <asp:ListItem Value="z" Text="신형(Stand)"></asp:ListItem>
                                    <asp:ListItem Value="p" Text="신형(Mini)"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> RACK용량 :</td>
                            <td>
                                 <asp:TextBox ID="자판기등록_랙수" runat="server" SkinID="tb1" MaxLength="2" Width="40px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 설치장소 :</td>
                            <td>
                                 <asp:TextBox ID="자판기등록_설치장소" runat="server" SkinID="tb1" MaxLength="200" Width="200px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 설치일 :</td>
                            <td>
                                 <asp:TextBox ID="자판기등록_설치일" runat="server" SkinID="tb1" MaxLength="10" Width="80px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="cd2" runat="server" TargetControlID="자판기등록_설치일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속총판 :</td>
                            <td>
                                <asp:DropDownList ID="자판기등록_총판" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속대리점 :</td>
                            <td>
                                <asp:DropDownList ID="자판기등록_대리점" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제구분 :</td>
                            <td>
                                 <asp:DropDownList ID="자판기등록_결제구분" runat="server" SkinID="dd1">
                                     <asp:ListItem Value="VAN" Text="VAN"></asp:ListItem>
                                     <asp:ListItem Value="PG" Text="PG"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제업체 :</td>
                            <td>
                                 <asp:DropDownList ID="자판기등록_결제업체" runat="server" SkinID="dd1">
                                     <asp:ListItem Value="KICC" Text="KICC"></asp:ListItem>
                                     <asp:ListItem Value="NICE" Text="NICE"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제코드 :</td>
                            <td>
                                 <asp:TextBox ID="자판기등록_결제코드" runat="server" SkinID="tb1" MaxLength="15" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td align="center" colspan="2">
                    <div runat="server" id="div_자판기등록결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt자판기등록완료" runat="server" OnClick="bt자판기등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt자판기등록닫기" runat="server" OnClick="bt자판기등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_자판기수정" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 자판기 수정 <asp:Label ID="자판기수정_자판기번호" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 관리자 연락처 : </td>
                            <td> <asp:TextBox ID="자판기수정_관리자연락처" runat="server" SkinID="tb1" MaxLength="12" Width="100px"></asp:TextBox> </td>
                        </tr>
                        <tr>
                            <td> 자판기타입 : </td>
                            <td>
                                <asp:DropDownList ID="자판기수정_자판기타입" runat="server" SkinID="dd1" AutoPostBack="true">
                                    <asp:ListItem Value="s" Text="구형(Stand)"></asp:ListItem>
                                    <asp:ListItem Value="m" Text="구형(Mini)"></asp:ListItem>
                                    <asp:ListItem Value="z" Text="신형(Stand)"></asp:ListItem>
                                    <asp:ListItem Value="p" Text="신형(Mini)"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> RACK용량 :</td>
                            <td>
                                 <asp:TextBox ID="자판기수정_랙수" runat="server" SkinID="tb1" MaxLength="2" Width="40px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 설치장소 :</td>
                            <td>
                                 <asp:TextBox ID="자판기수정_설치장소" runat="server" SkinID="tb1" MaxLength="200" Width="200px"></asp:TextBox>
                            </td>
                        </tr>                        
                        <tr>
                            <td> 설치일 :</td>
                            <td>
                                 <asp:TextBox ID="자판기수정_설치일" runat="server" SkinID="tb1" MaxLength="10" Width="80px"></asp:TextBox>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="자판기수정_설치일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속총판 :</td>
                            <td>
                                <asp:DropDownList ID="자판기수정_총판" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 소속대리점 :</td>
                            <td>
                                <asp:DropDownList ID="자판기수정_대리점" runat="server" SkinID="dd1" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제구분 :</td>
                            <td>
                                 <asp:DropDownList ID="자판기수정_결제구분" runat="server" SkinID="dd1">
                                     <asp:ListItem Value="VAN" Text="VAN"></asp:ListItem>
                                     <asp:ListItem Value="PG" Text="PG"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제업체 :</td>
                            <td>
                                 <asp:DropDownList ID="자판기수정_결제업체" runat="server" SkinID="dd1">
                                     <asp:ListItem Value="KICC" Text="KICC"></asp:ListItem>
                                     <asp:ListItem Value="NICE" Text="NICE"></asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 결제코드 :</td>
                            <td>
                                 <asp:TextBox ID="자판기수정_결제코드" runat="server" SkinID="tb1" MaxLength="15" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td align="center" colspan="2">
                    <div runat="server" id="div_자판기수정결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt자판기수정완료" runat="server" OnClick="bt자판기수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt자판기수정닫기" runat="server" OnClick="bt자판기수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
    
    <div id="div_자판기로그" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:1500px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 자판기로그 <asp:Label ID="lb로그자판기번호" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 조회기간 </td>
                            <td><asp:TextBox ID="자판기로그_시작일" runat="server" SkinID="tb1" MaxLength="10" Width="80px"></asp:TextBox> ~ <asp:TextBox ID="자판기로그_종료일" runat="server" SkinID="tb1" MaxLength="10" Width="80px"></asp:TextBox> 

                                
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="자판기로그_시작일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="자판기로그_종료일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>

                              <asp:DropDownList ID="자판기로그_구분" runat="server" SkinID="dd1" AutoPostBack="true" OnSelectedIndexChanged="자판기로그_구분_SelectedIndexChanged">
                                  <asp:ListItem Value="" Text="전체조회"></asp:ListItem>
                                  <asp:ListItem Value="1" Text="1버전체크"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="2상품조회"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="3결제보고"></asp:ListItem>
                                  <asp:ListItem Value="4" Text="4쿠폰조회"></asp:ListItem>
                                  <asp:ListItem Value="5" Text="5쿠폰사용"></asp:ListItem>
                                  <asp:ListItem Value="6" Text="6쿠폰사용취소"></asp:ListItem>
                                  <asp:ListItem Value="7" Text="7자판기번호조회"></asp:ListItem>
                                  <asp:ListItem Value="8" Text="8자판기정보수정"></asp:ListItem>
                                  <asp:ListItem Value="9" Text="9상태보고"></asp:ListItem>
                                  <asp:ListItem Value="10" Text="10라이브체크"></asp:ListItem>
                                  <asp:ListItem Value="11" Text="11문열림보고"></asp:ListItem>
                                  <asp:ListItem Value="12" Text="12결제취소보고"></asp:ListItem>
                              </asp:DropDownList>
                              <asp:Button ID="bt자판기로그검색" runat="server" Text="검색" SkinID="bt1" OnClick="bt자판기로그검색_Click" />
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="float:left; height:540px;">
                                    <asp:GridView ID="gv_자판기로그" runat="server"
                                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                                        CellPadding="4" SkinID="GridView1" DataKeyNames="idx"
                                        RowStyle-HorizontalAlign="Center" Width="450"
                                        AllowPaging="true" EmptyDataText="로그 없음"  
                                         PageSize="20" OnPageIndexChanging="gv_자판기로그_PageIndexChanging"
                                         OnRowCommand="gv_자판기로그_RowCommand"
                                        >
                                        <Columns>
                                            <asp:BoundField DataField="idx" HeaderText="idx" />
                                            <asp:BoundField DataField="etype" HeaderText="etype" />
                                            <asp:BoundField DataField="구분" HeaderText="구분" />
                                            <asp:BoundField DataField="clientip" HeaderText="ip" />
                                            <asp:BoundField DataField="regdate" HeaderText="regdate" />
                                            <asp:TemplateField HeaderText="보기" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" id="bt_set"  Text="보기" CommandName="Select" CommandArgument='<%# Eval("idx") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        </asp:GridView>
                                </div>
                                <div style="float:left; height:540px;">
                                    * 받은 데이터 <br />
                                    <asp:TextBox ID="tb_getdata" runat="server" Text="" TextMode="MultiLine" Width="500" Rows="15"></asp:TextBox><br />
                                    * 결과 데이터 <br />
                                    <asp:TextBox ID="tb_senddata" runat="server" Text="" TextMode="MultiLine" Width="500" Rows="15"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="Div2" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt자판기로그닫기" runat="server" OnClick="bt자판기로그닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
    

    <div id="div_자판기메뉴" runat="server" visible="false" style="position:absolute;top:0px;left:30px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:600px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 자판기 개별 메뉴 설정 ( <asp:Label ID="lb자판기메뉴관리_자판기번호" runat="server" Text=""></asp:Label> )

                    <asp:TextBox ID="선택_브랜드코드" runat="server" Visible="true"></asp:TextBox>
                    <asp:TextBox ID="선택_상품코드" runat="server" Visible="true"></asp:TextBox>
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td colspan="3"> 메뉴 가져오기 : <asp:TextBox ID="자판기메뉴_가져오기_자판기번호" runat="server" SkinID="tb1" MaxLength="10" Width="80px"></asp:TextBox> 의 메뉴를   <asp:Button ID="bt자판기메뉴_가져오기" runat="server" Text="가져오기" SkinID="bt1" OnClick="bt자판기메뉴_가져오기_Click" OnClientClick="return confirm('기존 메뉴 정보가 모두 삭제되고 해당 자판기의 메뉴를 전체 가져옵니다. 계속하시겠습니까?')" />
                              <asp:Button ID="bt기본상품추가하기" runat="server" Text="기본상품추가" SkinID="bt2" OnClick="bt기본상품추가하기_Click" OnClientClick="return confirm('기존 메뉴 정보가 모두 삭제되고 기본메뉴와 기본커피캡슐 상품이 추가됩니다. 계속하시겠습니까?')" />

                        </tr>
                        <tr>
                            <td style="height:25px;background-color:#e6e6e6;text-align:center;width:200px;">메뉴 관리</td>
                            <td style="height:25px;background-color:#e6e6e6;text-align:center;width:200px;">상품 관리</td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top;">
                                <div style="width:100%;text-align:right;"> 
                                    <asp:Label ID="lb자판기메뉴관리_선택메뉴" runat="server" Text=""></asp:Label>
                                    <asp:Button ID="bt메뉴추가" runat="server" Text="메뉴추가" SkinID="bt1" OnClick="bt메뉴추가_Click" />
                                </div>
                                <asp:GridView ID="gv메뉴" runat="server"
                                    AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                                    CellPadding="4" SkinID="GridView1"
                                    RowStyle-HorizontalAlign="Center"
                                    AllowPaging="false" EmptyDataText="메뉴 없음" OnRowCommand="gv메뉴_RowCommand"  
                                    width="250px"
                                    >
                                    <Columns>
                                        <asp:TemplateField HeaderText="메뉴명" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>' ToolTip='<%# Eval("idx") %>'  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="dno" HeaderText="순서" />
                                        <asp:BoundField DataField="상품수" HeaderText="상품수" />
                                        <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt_up"  Text="삭제" CommandName="menu_del" CommandArgument='<%# Eval("idx") %>' OnClientClick="return confirm('메뉴를 삭제하시겠습니까? 메뉴에 등록된 상품도 모두 삭제됩니다.')"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>

                            </td>
                            <td style="vertical-align:top;">
                                <div style="width:100%;text-align:right;"> 
                                    <asp:DropDownList ID="dd메뉴_브랜드선택" runat="server" OnSelectedIndexChanged="dd브랜드선택_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    <asp:Button ID="bt상품추가" runat="server" Text="상품추가" SkinID="bt1"  OnClick="bt상품추가_Click" />
                                </div>
                                <asp:GridView ID="gv상품" runat="server"
                                    AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                                    CellPadding="4" SkinID="GridView1"
                                    RowStyle-HorizontalAlign="Center" OnRowCommand="gv상품_RowCommand"
                                    AllowPaging="false" EmptyDataText="메뉴 없음"  
                                    width="350px"
                                    >
                                    <Columns>
                                        <asp:TemplateField HeaderText="이미지" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                 <asp:Image ID="Image1" runat="server" Width="50px" ImageUrl='<%# "data:image/png;base64,"+Eval("imagebase64_s") %>' BackColor="White" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="브랜드명" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("브랜드명") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="상품명" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt3"  Text='<%# Eval("상품명") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="투출" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt4"  Text='<%# Eval("pr_type") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="center">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="menu_up" CommandArgument='<%# Eval("idx") %>' Visible="true" />
                                                <asp:LinkButton runat="server" id="bt_del"  Text="삭제" CommandName="menu_del" CommandArgument='<%# Eval("idx") %>' OnClientClick="return confirm('해당 상품을 삭제하시겠습니까?')"  />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="div_메뉴관리_안내" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt자판기메뉴관리닫기" runat="server" OnClick="bt자판기메뉴관리닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>

    <div id="div_상품등록" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 상품(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 브랜드명 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_브랜드" AutoPostBack="true" OnSelectedIndexChanged="상품등록_브랜드_SelectedIndexChanged"></asp:DropDownList>
                                <asp:TextBox ID="상품등록_브랜드명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                                
                            </td>
                        </tr>
                        <tr>
                            <td> 상품명 : </td>
                            <td><asp:TextBox ID="상품등록_상품명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 투출타입 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_투출타입">
                                    <asp:ListItem Value="투출"></asp:ListItem>
                                    <asp:ListItem Value="전시"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 단위 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품등록_단위">
                                    <asp:ListItem Value="pcs" Text="낱개"></asp:ListItem>
                                    <asp:ListItem Value="box" Text="박스"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 비고 : </td>
                            <td>
                                <asp:TextBox ID="상품등록_비고" runat="server" SkinID="tb1" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 이미지 :</td>
                            <td>
                                <asp:FileUpload ID="상품등록_이미지" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td><div>(이미지 권장사이즈: 160px x 160px .png)</div></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="상품등록_결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>                 
                    <asp:Button ID="bt상품등록완료" runat="server" OnClick="bt상품등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt상품등록닫기" runat="server" OnClick="bt상품등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_상품수정" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 상품(수정/삭제)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 상품코드 : </td>
                            <td>
                                <asp:Label ID="상품수정_상품코드" runat="server" Text=""></asp:Label> 
                            </td>
                        </tr>
                        <tr>
                            <td> 브랜드 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_브랜드코드"></asp:DropDownList>
                            </td>
                        </tr>
                        
                        <tr>
                            <td> 상품명 : </td>
                            <td><asp:TextBox ID="상품수정_상품명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                        </tr>
                        <tr>
                            <td> 투출타입 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_투출타입">
                                    <asp:ListItem Value="투출"></asp:ListItem>
                                    <asp:ListItem Value="전시"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 단위 : </td>
                            <td>
                                <asp:DropDownList runat="server" id="상품수정_단위">
                                    <asp:ListItem Value="pcs" Text="낱개"></asp:ListItem>
                                    <asp:ListItem Value="box" Text="박스"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td> 비고 : </td>
                            <td>
                                <asp:TextBox ID="상품수정_비고" runat="server" SkinID="tb1" MaxLength="100" Width="100px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td> 이미지 :</td>
                            <td>
                                <asp:Image ID="상품수정_현재이미지" runat="server" /><br />
                                <asp:FileUpload ID="상품수정_이미지" runat="server" Width="200px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="div_상품수정결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt상품수정완료" runat="server" OnClick="bt상품수정완료_Click" CausesValidation="false" Text="수정하기" SkinID="bt1" />
                    <asp:Button ID="bt상품수정닫기" runat="server" OnClick="bt상품수정닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
    
    <div id="div_메뉴추가" runat="server" visible="false" style="position:absolute;top:0px;left:330px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="width:400px;">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 메뉴(신규등록)
                </td>
            </tr>
            <tr style="background:#FFFFA4;height:25px;">
                <td style="padding-left:10px;background:#151521;" colspan="2">
                    <table>
                        <tr>
                            <td> 메뉴명 : </td>
                            <td>
                                <asp:TextBox ID="메뉴추가_메뉴명" runat="server" SkinID="tb1" MaxLength="30" Width="200px"></asp:TextBox> 
                                
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="div_메뉴등록결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt메뉴등록완료" runat="server" OnClick="bt메뉴등록완료_Click" CausesValidation="false" Text="등록하기" SkinID="bt1" />
                    <asp:Button ID="bt메뉴등록닫기" runat="server" OnClick="bt메뉴등록닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <div id="div_쿠폰관리" runat="server" visible="false" style="position:absolute;top:0px;left:130px; border:5px solid #aaa;background:#151521;padding:5px 5px 5px 5px;box-shadow: 3px 3px 3px gray;border:1px solid gray;" >
        <table cellpadding="0" cellspacing="0" style="">
            <tr style="background:#151521;height:45px;">
                <td align="center" colspan="2">
                    * 쿠폰관리
                </td>
            </tr>
            <tr>
                <td style="padding-left:10px;background:#151521;vertical-align:top;">
                    <table style="width:300px;">
                        <tr>
                            <td> * 쿠폰발행 </td>
                        </tr>
                        <tr>
                            <td>
                                <div><asp:TextBox ID="쿠폰발행_만료일" runat="server" SkinID="tb1" MaxLength="10" Width="90px"></asp:TextBox> 일까지 사용가능한</div>
                                <div><asp:TextBox ID="쿠폰발행_발행금액" runat="server" SkinID="tb1" MaxLength="4" Width="50px"></asp:TextBox> 원짜리 쿠폰</div>
                                <div><asp:TextBox ID="쿠폰발행_발행숫자" runat="server" SkinID="tb1" MaxLength="4" Width="50px"></asp:TextBox> 개 를 
                                발행처리 합니다. 해당 쿠폰은 </div>
                                <div><asp:TextBox ID="쿠폰발행_최소구매금액" runat="server" SkinID="tb1" MaxLength="4" Width="50px"></asp:TextBox> 원 이상 구매시 사용가능합니다.</div>
                                <asp:Button ID="bt쿠폰발행" runat="server" Text="쿠폰발행" OnClientClick="return confirm('쿠폰을 발행하시겠습니까?')" SkinID="bt1" OnClick="bt쿠폰발행_Click"/>
                                
                                <ajaxToolkit:CalendarExtender ID="cd1" runat="server" TargetControlID="쿠폰발행_만료일" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align:top;width:300px; background-color:#151521;">
                    <table>
                        <tr>
                            <td> 쿠폰요약 </td>
                        </tr>
                        <tr>
                            <td>
                                <div style=""> 발행수 : <asp:Label ID="쿠폰_발행수" runat="server" Text="0"></asp:Label> 건 ( <asp:Label ID="쿠폰_발행금액" runat="server" Text="0"></asp:Label> 원 ) </div>
                                <div style=""> 사용수 : <asp:Label ID="쿠폰_사용수" runat="server" Text="0"></asp:Label> 건 ( <asp:Label ID="쿠폰_사용금액" runat="server" Text="0"></asp:Label> 원 ) </div>
                                <div style=""> 미사용 : <asp:Label ID="쿠폰_미사용" runat="server" Text="0"></asp:Label> 건 ( <asp:Label ID="쿠폰_미사용금액" runat="server" Text="0"></asp:Label> 원 ) </div>
                                <div style=""> 미사용종료 : <asp:Label ID="쿠폰_미사용종료" runat="server" Text="0"></asp:Label> 건 (<asp:Label ID="쿠폰_미사용종료금액" runat="server" Text="0"></asp:Label> 원) </div>

                                <div>
                                    
                                    <asp:GridView ID="gv쿠폰리스트" runat="server"
                                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="쿠폰번호"
                                        CellPadding="4" SkinID="GridView1"
                                        RowStyle-HorizontalAlign="Center"
                                        AllowPaging="false" EmptyDataText="쿠폰 없음"
                                        width="580px" OnRowCommand="gv쿠폰리스트_RowCommand"
                                        >
                                        <Columns>
                                            <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" id="bt1"  Text='<%# Eval("rownum") %>' CommandName="Select" CommandArgument='<%# Eval("쿠폰번호") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="발행금액" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("발행금액") %>' CommandName="Select" CommandArgument='<%# Eval("쿠폰번호") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="최소구매금액" HeaderText="최소구매액" />
                                            <asp:BoundField DataField="만료일" HeaderText="만료일" />
                                            <asp:BoundField DataField="만료여부" HeaderText="만료" />
                                            <asp:BoundField DataField="useyn" HeaderText="사용여부" />
                                            <asp:BoundField DataField="발행일" HeaderText="발행일" />
                                            <asp:BoundField DataField="발행수량" HeaderText="발행수량" />
                                        </Columns>
                                        </asp:GridView>

                                    <asp:GridView ID="gv쿠폰상세리스트" runat="server"
                                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                                        CellPadding="4" SkinID="GridView1"
                                        RowStyle-HorizontalAlign="Center"
                                        AllowPaging="false" EmptyDataText="쿠폰 없음"
                                        width="580px"
                                        >
                                        <Columns>
                                            <asp:TemplateField HeaderText="No" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" id="bt1"  Text='<%# Eval("rownum") %>' CommandName="Select" CommandArgument='<%# Eval("쿠폰번호") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="쿠폰번호" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("쿠폰번호") %>' CommandName="Select" CommandArgument='<%# Eval("쿠폰번호") %>'  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="최소구매금액" HeaderText="최소구매액" />
                                            <asp:BoundField DataField="만료일" HeaderText="만료일" />
                                            <asp:BoundField DataField="만료여부" HeaderText="만료" />
                                            <asp:BoundField DataField="useyn" HeaderText="사용여부" />
                                            <asp:BoundField DataField="발행일" HeaderText="발행일" />
                                        </Columns>
                                        </asp:GridView>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="background:#151521;height:25px;">
                <td colspan="2" style="text-align:center;">
                    <div runat="server" id="div_쿠폰결과" style="padding:5px; font-size:14px; color:red; font-weight:bold;"></div>
                    <asp:Button ID="bt쿠폰닫기" runat="server" OnClick="bt쿠폰닫기_Click" CausesValidation="false" Text="닫기" SkinID="bt2" />
                </td>
            </tr>
        </table>
    </div>
      
    <h2>
        &nbsp; 통합관리
        <asp:TextBox ID="tbexcel" runat="server" SkinID="tb1" Visible="false"></asp:TextBox>

        <asp:TextBox ID="선택_총판코드" runat="server" Visible="false"></asp:TextBox>
        <asp:TextBox ID="선택_대리점코드" runat="server" Visible="false"></asp:TextBox>
    </h2>
    <div class="formlayout">
        <table>
            <tr>
                <td style="vertical-align:top">
                    <div style="padding:5px">

                        <span style="font-size:15px; font-weight:bold">* 총판 ( <asp:Label ID="lb총판수" runat="server" Text=""></asp:Label> 건 ) </span> <asp:Button ID="bt총판등록" runat="server" Text="총판등록" SkinID="bt2" OnClick="bt총판등록_Click" />
                        <asp:Button ID="bt새로고침" runat="server" Text="새로고침" SkinID="bt1" OnClick="bt새로고침_Click" />

                    </div>
                    <asp:GridView ID="gv총판" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv총판_RowCommand"
                        AllowPaging="false" EmptyDataText="총판 없음"
                        width="280px"
                        >
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:TemplateField HeaderText="총판명 [대리점/자판기/계정]" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    [ <asp:Label ID="Label2" runat="server" Text='<%# Eval("branch_cnt") %>'></asp:Label> / 
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("machine_cnt") %>'></asp:Label> / 
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("member_cnt") %>'></asp:Label> ]
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="agency_up" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>

                </td>
                <td style="vertical-align:top" runat="server" visible="false" id="td대리점">
                    <div style="padding:5px">
                        <span style="font-size:15px; font-weight:bold">* <asp:Label ID="lb선택총판명" runat="server" Text=""></asp:Label> (<asp:Label ID="lb선택총판idx" runat="server" Text=""></asp:Label>) </span><asp:Button ID="bt대리점등록" runat="server" Text="대리점등록" SkinID="bt2" OnClick="bt대리점등록_Click" />
                    </div>
                    <asp:GridView ID="gv대리점" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True"
                        CellPadding="4" SkinID="GridView1" DataKeyNames="idx"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv대리점_RowCommand"
                        AllowPaging="false" EmptyDataText="대리점 없음"  
                        width="320px"
                        >
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:TemplateField HeaderText="대리점명 [ 자판기/계정 ]" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("name") %>' CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                    [ <asp:Label ID="Label1" runat="server" Text='<%# Eval("machine_cnt") %>'></asp:Label> / 
                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("member_cnt") %>'></asp:Label> ] 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="branch_up" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        </asp:GridView>

                </td>
                <td style="vertical-align:top" runat="server" visible="false" id="td사용자">

                    <div style="padding:5px">
                        <span style="font-size:15px; font-weight:bold">* <asp:Label ID="lb선택명" runat="server" Text=""></asp:Label>  의 사용자 </span>
                        <asp:Button ID="bt사용자등록" runat="server" Text="사용자등록" SkinID="bt2" OnClick="bt사용자등록_Click" />
                    </div>
                    <asp:GridView ID="gv사용자" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center" OnRowCommand="gv사용자_RowCommand"
                        AllowPaging="false" EmptyDataText="사용자 없음"  
                        width="500px"
                        >
                        <Columns>
                            <asp:BoundField DataField="usrorg" HeaderText="소속" />
                            <asp:TemplateField HeaderText="아이디" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt2"  Text='<%# Eval("userid") %>' CommandName="Select" CommandArgument='<%# Eval("userid") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="usernm" HeaderText="이름" />
                            <asp:BoundField DataField="usrtel" HeaderText="연락처" />
                            <asp:BoundField DataField="mphone" HeaderText="핸드폰" />
                            <asp:BoundField DataField="active" HeaderText="사용" />
                            <asp:BoundField DataField="lst_logintime" HeaderText="마지막접속" />
                            <asp:TemplateField HeaderText="수정" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="bt_up"  Text="수정" CommandName="Select" CommandArgument='<%# Eval("userid") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br /><br />

                    <asp:Button ID="bt자판기등록" runat="server" Text="자판기등록" SkinID="bt2" OnClick="bt자판기등록_Click" />

                    <asp:GridView ID="gv단말기리스트" runat="server"
                        AutoGenerateColumns="false" GridLines="None" EnableTheming="True" DataKeyNames="idx"
                        CellPadding="4" SkinID="GridView1"
                        RowStyle-HorizontalAlign="Center"
                        AllowPaging="false" EmptyDataText="단말기 없음" OnRowDataBound="gv단말기리스트_RowDataBound"
                            width="700px"          OnRowCommand="gv단말기리스트_RowCommand"   
                        >
                        <Columns>
                            <asp:BoundField DataField="rownum" HeaderText="No" />
                            <asp:BoundField DataField="idx" HeaderText="자판기코드" />
                            <asp:TemplateField HeaderText="설치장소">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("설치장소") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div id="container">통신 <div id="networkcircle" class="circle">?</div> </div>
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
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div id="container">랙 <div id="rackcircle" class="circle">?</div></div>
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

                            <asp:BoundField DataField="설치일" HeaderText="설치일" />
                            <asp:BoundField DataField="관리자연락처" HeaderText="관리연락처" />
                            <asp:TemplateField HeaderText="상세">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" id="lbt0" Text="배너" CommandName="banner" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="lbt1" Text="메뉴" CommandName="menu" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="lbt2" Text="로그" CommandName="log" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="lbt4" Text="쿠폰" CommandName="coupon" CommandArgument='<%# Eval("idx") %>'  />
                                    <asp:LinkButton runat="server" id="lbt3" Text="수정" CommandName="Select" CommandArgument='<%# Eval("idx") %>'  />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </td>
            </tr>
        </table>

    </div>
            
  </asp:Content>
