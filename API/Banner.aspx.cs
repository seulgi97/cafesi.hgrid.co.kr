using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Json;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Collections;

public partial class API_Banner : System.Web.UI.Page
{
    SgFramework.SgUtil su = new SgFramework.SgUtil();
    protected string sql = string.Empty;
    protected JsonObjectCollection j = null;
    protected JsonObjectCollection getData = null;
    protected JsonTextParser parser = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        String ip = su.GetIP(this.Page);
        String body = su.getRawBody(this.Page);
        String retVal = "";
        String eType = "";
        String machine_idx = "";
        String log_idx = "";
        String tmp = "";
        String clientip = su.GetIP(this.Page);

        j = new JsonObjectCollection();
        parser = new JsonTextParser();

        #region eType = 0 서버체크
        if (body == "")
        {
            j.Add(new JsonStringValue("RETCODE", "0"));
            j.Add(new JsonStringValue("RETMSG", "정상"));
            j.Add(new JsonStringValue("eType", "0"));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        #region body 체크 
        if (body == "")
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-4"));
            j.Add(new JsonStringValue("RETMSG", "body 값이 없습니다"));

            Response.Write(j.ToString());
            return;
        }

        try
        {
            getData = (JsonObjectCollection)parser.Parse(body);

        }
        catch (Exception ex)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-3"));
            j.Add(new JsonStringValue("RETMSG", ex.Message.ToString() + "-" + body));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        body = body.Replace(Environment.NewLine, "").Replace("\\", "");
        eType = su.sg_db_query((String)getData["eType"].GetValue());
        //machine_idx = su.Decrypt_AES(su.sg_db_query((String)getData["mid"].GetValue())).Trim();
        machine_idx = su.sg_db_query((String)getData["mid"].GetValue()).Trim();

        #region eType 체크 
        if (eType == "" || su.isNumeric(eType) == false)
        {
            j.Clear();

            j.Add(new JsonStringValue("RETCODE", "-1"));
            j.Add(new JsonStringValue("RETMSG", "eType 값은 숫자로 입력해야 합니다"));

            Response.Write(j.ToString());
            return;
        }
        #endregion

        var rtnmsg = string.Empty;
        switch (eType)
        {
            case "1": // 배너정보 조회
                rtnmsg = GetBannerInfo(machine_idx, eType);
                break;
            default:
                j.Clear();
                j.Add(new JsonStringValue("RETCODE", "-99"));
                j.Add(new JsonStringValue("RETMSG", "해당 eType은 지원하지 않습니다"));
                j.Add(new JsonStringValue("eType", eType));

                rtnmsg = j.ToString();
                break;
        }

        Response.Write(rtnmsg);
    }

    #region Get Banner Information
    /// <summary>
    /// 장비번호로 홈광고 배너이미지(1) & 띠베너이미지 (2)    
    /// </summary>
    /// <param name="machineIdx"></param>
    /// <returns></returns>
    private string GetBannerInfo(string midx, string etype)
    {
        try
        {
            j.Clear();

            sql = @"
                    declare @company_idx int
                    declare @agency_idx int
                    declare @branch_idx int
                    declare @machine_idx int
                    
                    -- 장비에서 대리점정보 획득 
                    select @company_idx = company_idx
                          ,@agency_idx  = agency_idx
                          ,@branch_idx  = branch_idx
                          ,@machine_idx = idx
                    from dbo.machine with(nolock)
                    where idx = {0} -- by machineIdx
                      
                    -- Banner정보 테이블에서 장비Id로 해당 배너이미지 조회
                    select '장비별' as ttype, company_idx, agency_idx, branch_idx, machine_idx
                    , title, startdate, enddate, imgurl, imgurl2, timeinterval
                    from dbo.tbl_banner with(nolock)
                    where machine_idx = @machine_idx
                    and useyn = 'Y'
                    union 

                    -- 지점배너정보 
                    select '지점' as ttype, company_idx, agency_idx, branch_idx, machine_idx
                    , title, startdate, enddate, imgurl, imgurl2, timeinterval
                    from dbo.tbl_banner with(nolock)
                    where company_idx = @company_idx
                    and agency_idx  = @agency_idx
                    and branch_idx  = @branch_idx 
                    and machine_idx = 0
                    and useyn = 'Y'

                    union 

                    -- 총판배너정보 
                    select '총판' as ttype, company_idx, agency_idx, branch_idx, machine_idx
                    , title, startdate, enddate, imgurl, imgurl2, timeinterval
                    from dbo.tbl_banner with(nolock)
                    where company_idx = @company_idx
                    and agency_idx  = @agency_idx
                    and branch_idx  = 0
                    and machine_idx = 0
                    and useyn = 'Y'
                    ";

            DataView dv = su.SqlDvQuery( string.Format(sql, midx));

            if(dv == null || dv.Count == 0)
            {
                j.Add(new JsonStringValue("RETCODE", "-1"));
                j.Add(new JsonStringValue("RETMSG", "조회된 자료가 없습니다"));
                j.Add(new JsonStringValue("eType", etype));
                return j.ToString();
            } 
            else // 배너정보 조회
            {
                JsonArrayCollection arr = new JsonArrayCollection();
                for(int i=0; i<dv.Count; i++)
                {
                    JsonObjectCollection joc = new JsonObjectCollection();

                    joc.Add(new JsonStringValue("ttype", dv[i]["ttype"].ToString()));      // 배너구분
                    joc.Add(new JsonStringValue("companyidx", dv[i]["company_idx"].ToString()));
                    joc.Add(new JsonStringValue("agencyidx", dv[i]["agency_idx"].ToString()));
                    joc.Add(new JsonStringValue("branchidx", dv[i]["branch_idx"].ToString()));
                    joc.Add(new JsonStringValue("machineidx", dv[i]["machine_idx"].ToString()));
                    joc.Add(new JsonStringValue("title",    dv[i]["title"].ToString()));      // 홈배너, 띠배너 제목 
                    joc.Add(new JsonStringValue("timeinterval", dv[i]["timeinterval"].ToString()));
                    joc.Add(new JsonStringValue("imgurl",   dv[i]["imgurl"].ToString()));
                    joc.Add(new JsonStringValue("imgurl2",  dv[i]["imgurl2"].ToString()));    // 띠배너 이미지

                    arr.Add(joc);
                }

                return arr.ToString();
            }
        }
        catch (Exception ex)
        {
            j.Add(new JsonStringValue("RETCODE", "-2"));
            j.Add(new JsonStringValue("RETMSG", ex.Message));
            j.Add(new JsonStringValue("eType", etype));
            return j.ToString();
        }
    }
    #endregion
}