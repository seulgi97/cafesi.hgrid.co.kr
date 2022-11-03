using System;
using System.Web.UI;
using System.Web.Security;
using System.Data;

public partial class Account_Logout : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Session.Abandon();
        Response.Redirect("/Account/Login.aspx");
    }
    
}
