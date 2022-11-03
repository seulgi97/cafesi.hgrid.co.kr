using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

/// <summary>
/// AuthUser의 요약 설명입니다.
/// </summary>
public class AuthUser
{
    /// <summary>
    /// 인증된 사용자 정보를 가져옴.
    /// </summary>
    /// <param name="">-</param>
    /// <returns>string[]</returns>
    public string[] GetUserData()
    {
        FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;

        // Get the stored user-data, in this case, our roles (권한그룹ID)
        string userData = ticket.UserData;
        string[] arrUserData = userData.Split(':');
        return arrUserData;
    }

    /// <summary>
    /// 유효한 사용자를 Auth 처리함 (쿠키처리)
    /// </summary>
    /// <param name="UserID">로그인ID</param>
    /// <param name="strUserData">사용자 정의데이타</param>
    /// <returns>void</returns>
    public void VerifyUser(string UserID, string strUserData)
    {
        //=============.net 인증처리=====================================================
        // Create a new ticket used for authentication
        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
         1, // Ticket version
         UserID, // Login ID associated with ticket
         DateTime.Now, // Date/time issued 
         DateTime.Now.AddMinutes(2880), // Date/time to expire
         true, // "true" for a persistent user cookie
         strUserData, // User-data, in this case the roles
         FormsAuthentication.FormsCookiePath);// Path cookie valid for

        // Encrypt the cookie using the machine key for secure transport
        string hash = FormsAuthentication.Encrypt(ticket);
        HttpCookie cookie = new HttpCookie(
         FormsAuthentication.FormsCookieName, // Name of auth cookie
         hash); // Hashed ticket

        // Set the cookie's expiration time to the tickets expiration time
        if (ticket.IsPersistent) cookie.Expires = ticket.Expiration;

        // Add the cookie to the list for outgoing response    
        HttpContext.Current.Response.Cookies.Add(cookie);
    }

    /// <summary>
    /// 정상로그인 여부를 체크함.
    /// </summary>
    /// <param name="">-</param>
    /// <returns>bool(true/false)</returns>
    public static bool IsAuthenticated
    {
        get
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                        return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// 사용자 LoginID (emp_no)
    /// </summary>
    public static string gUserID
    {
        get
        {
            if (AuthUser.IsAuthenticated)
            {
                FormsIdentity id =
                 (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                return ticket.Name;
            }
            else
                return null;
        }
    }

    /// <summary>
    /// 사용자명(emp_nm)
    /// </summary>
    public static string gUserName
    {
        get
        {
            if (AuthUser.IsAuthenticated)
            {
                AuthUser oAuth = new AuthUser();
                string[] arrTemp = oAuth.GetUserData();
                return arrTemp[0].ToString();
            }
            else
                return null;
        }
    }

    /// <summary>
    /// 사용자 구분
    /// </summary>
    public static string gUserGubn
    {
        get
        {
            if (AuthUser.IsAuthenticated)
            {
                AuthUser oAuth = new AuthUser();
                string[] arrTemp = oAuth.GetUserData();
                return arrTemp[1].ToString();
            }
            else
                return null;
        }
    }

    /// <summary>
    /// 거래처ID
    /// </summary>
    public static string gCorpID
    {
        get
        {
            if (AuthUser.IsAuthenticated)
            {
                AuthUser oAuth = new AuthUser();
                string[] arrTemp = oAuth.GetUserData();
                return arrTemp[2].ToString();
            }
            else
                return null;
        }
    }
}