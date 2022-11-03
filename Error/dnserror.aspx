<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="dnserror.aspx.cs" Inherits="Error_dnserror"  EnableEventValidation="false" %>
<!DOCTYPE HTML>
<html>

    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>이 페이지를 표시할 수 없습니다.</title>
        <style>
            .login_tb { width:80%; border: 1px solid #aaa; font-family:맑은 고딕; font-weight:bold; font-size:14px; color:#aaa; padding:10px; }
            .login_bt { width:80%; border-radius:4px; background-color:#aaa; border: 0; font-family:맑은 고딕; font-weight:bold; font-size:14px; color:#fff; padding:10px; }
            .login_bt2 { width:80%; border-radius:4px; background-color:#002e7d; border: 0; font-family:맑은 고딕; font-weight:bold; font-size:14px; color:#fff; padding:10px; }
        </style>
    </head>

    <body style="padding:0; margin:0">
        <div id="contentContainer" style="width:100%;">
            <div id="mainTitle" style="width:100%; height:100px; background-color:#aaa;  color:#fff; font-family:맑은 고딕; font-size:2.5em; padding-top:50px;">
                <span style="padding-left:50px"> 이 페이지를 표시할 수 없습니다.</span>
            </div>
            <div >
                <ul id="cantDisplayTasks" class="tasks" style="font-family:맑은 고딕; font-size:15px; line-height:30px; padding-left:70px;">
                    <li id="Li1"> <span style="font-size:20px; color:Red; font-weight:bold"><%=su.GetIP(this.Page) %></span> 에서 접속하셨습니다. </li>
                    <li id="Li2">등록된 IP가 아닐경우 해당 사이트에 접속할 수 없습니다.</li>
                    <li id="task1-1">정상적으로 사용하다 이 페이지를 모든 이용자가 보게되면 사용하는 IP가 변경된것으로 보입니다.</li>
                    <li id="task1-3">영업 담당자에게 해당 IP등록을 요청해 주시기 바랍니다.</li>
                </ul>
            </div>
            <br />
            <div style="padding-left:50px; width:10em;"><input type="button" onclick="location.href='https://cs.smart36.co.kr';" value="연결 재시도" class="login_bt" /> </div>
        </div>
    </body>
</html>
