<%@ Application Language="C#" %>
<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 응용 프로그램이 시작될 때 실행되는 코드입니다.

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  응용 프로그램이 종료될 때 실행되는 코드입니다.

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 처리되지 않은 오류가 발생할 때 실행되는 코드입니다.

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 새 세션이 시작할 때 실행되는 코드입니다.

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 세션이 끝날 때 실행되는 코드입니다. 
        // 참고: Session_End 이벤트는 Web.config 파일에서 sessionstate 모드가
        // InProc로 설정되어 있는 경우에만 발생합니다. 세션 모드가 StateServer 또는 SQLServer로 
        // 설정되어 있는 경우에는 이 이벤트가 발생하지 않습니다.

    }

    //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
    //{
    //    if (HttpContext.Current.User != null)
    //    {
    //        if (HttpContext.Current.User.Identity.IsAuthenticated)
    //        {
    //            if (HttpContext.Current.User.Identity is FormsIdentity)
    //            {
    //                FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
    //                FormsAuthenticationTicket ticket = id.Ticket;

    //                // Get the stored user-data, in this case, our roles (권한그룹ID)
    //                string userData = ticket.UserData;
    //                string[] arrUserData = userData.Split(':');
    //                string strGrp_IDs = arrUserData[2];
    //                string[] roles = strGrp_IDs.Split(',');
    //                HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(id, roles);

    //                //프로그램 사용로그 기록.
    //                Utils.write2_log("Access Authenticate", AuthUser.gCorpID.ToString() + "|" + AuthUser.gUserID.ToString());
    //            }
    //        }
    //    }
    //}

       
</script>
