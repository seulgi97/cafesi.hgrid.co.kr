using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Drawing;

public partial class Error_dnserror : System.Web.UI.Page
{

    public SgFramework.SgUtil su = new SgFramework.SgUtil();
    string AgencyID = "";
    string UserID = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //
    }

}