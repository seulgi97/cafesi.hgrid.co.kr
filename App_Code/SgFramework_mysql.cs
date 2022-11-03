using System;
using System.Collections.Generic;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using System.Net;
using System.Security.Cryptography;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Collections;
using System.Net.Json;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Net.Mail;
using System.Drawing.Drawing2D;
using System.Web.UI.HtmlControls;

/// <summary>
/// SgFramework의 요약 설명입니다.
/// </summary>

namespace SgFramework_mysql
{
    public class SgUtil
    {
        ConnectionStringSettings cts = ConfigurationManager.ConnectionStrings["MyDBConnectionString"];
        private String connStr = "";
        public MySqlConnection TranConn;
        public MySqlCommand TranCmd;
        public bool TranResult = false;
        public String TranErrorMsg = "";

        public static string img_pr_path = "/image/pr/";

        public SgUtil()
        {
            connStr = cts.ConnectionString.ToString();
        }

        public string Base64Encoding(string str)
        {
            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        public string Base64Decoding(string str)
        {
            byte[] decbuff = Convert.FromBase64String(str);
            return System.Text.Encoding.UTF8.GetString(decbuff);
        }

        #region sg_int 
        public int sg_int(string str)
        {
            int retVal = 0;
            try
            {
                retVal = Convert.ToInt32(str);
            }
            catch (Exception ex) { }
            return retVal;
        }
        #endregion

        #region binding : DetailsView에 리스트 출력
        public int binding(DetailsView obj, String sql)
        {
            int retVal = 0;
            try
            {
                bool isAllowPaging = obj.AllowPaging;
                SqlDataSource SqlDataSource1 = new SqlDataSource(connStr, sql);
                obj.DataSource = SqlDataSource1;
                obj.DataBind();

                obj.AllowPaging = false;
                retVal = obj.Rows.Count;
                obj.AllowPaging = isAllowPaging;

                SqlDataSource1.Dispose();
                SqlDataSource1 = null;
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : GridView에 리스트 출력
        public int binding(GridView obj, String sql)
        {
            int retVal = 0;
            try
            {
                bool isAllowPaging = obj.AllowPaging;

                MySqlDataAdapter SqlDataSource1 = new MySqlDataAdapter(sql, connStr);
                DataSet riverDataSet = new DataSet();
                SqlDataSource1.Fill(riverDataSet);

                obj.DataSource = riverDataSet.Tables[0];
                obj.DataBind();

                obj.AllowPaging = false;
                retVal = obj.Rows.Count;
                obj.AllowPaging = isAllowPaging;

                SqlDataSource1.Dispose();
                SqlDataSource1 = null;
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : ListView에 리스트 출력
        public int binding(ListView obj, String sql)
        {
            int retVal = 0;
            try
            {
                SqlDataSource SqlDataSource1 = new SqlDataSource(connStr, sql);
                obj.DataSource = SqlDataSource1;
                obj.DataBind();

                retVal = obj.Items.Count;

                SqlDataSource1.Dispose();
                SqlDataSource1 = null;
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : ListBox에 리스트 출력
        public int binding(ListBox obj, String sql)
        {
            int retVal = 0;
            try
            {
                SqlDataSource SqlDataSource1 = new SqlDataSource(connStr, sql);
                obj.DataSource = SqlDataSource1;
                obj.DataBind();

                retVal = obj.Items.Count;

                SqlDataSource1.Dispose();
                SqlDataSource1 = null;
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : DataList에 리스트 출력
        public int binding(DataList obj, String sql)
        {
            int retVal = 0;
            try
            {
                SqlDataSource SqlDataSource1 = new SqlDataSource(connStr, sql);
                SqlDataSource1.SelectCommand = sql;
                obj.DataSource = SqlDataSource1;
                obj.DataBind();

                retVal = 1;

                SqlDataSource1.Dispose();
                SqlDataSource1 = null;


            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : RadioButtonList 에 리스트 출력 ( 쿼리에서 첫번째 필드가 텍스트, 두번째필드가 Value )

        public int binding(RadioButtonList obj, String sql)
        {
            return binding(obj, sql, "", true);
        }

        public int binding(RadioButtonList obj, String sql, String 빈값)
        {
            return binding(obj, sql, 빈값, true);
        }

        public int binding(RadioButtonList obj, String sql, String 빈값, bool DefaultChecked)
        {
            int retVal = 0;
            try
            {
                String setVal = obj.SelectedValue;
                obj.Items.Clear();

                if (빈값 != "")
                {
                    obj.Items.Add(new ListItem(빈값, ""));
                }

                DataView dv = SqlDvQuery(sql);

                for (int i = 0; i < dv.Count; i++)
                {
                    obj.Items.Add(new ListItem(dv[i][0].ToString(), dv[i][1].ToString()));
                }
                if (DefaultChecked)
                {
                    if (setVal != "" || dv.Count == 1)
                    {
                        setRadioButtonList(obj, setVal);
                    }
                }

                retVal = dv.Count;

                dv.Dispose();
                dv = null;


            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region binding : CheckBoxList 에 리스트 출력 ( 쿼리에서 첫번째 필드가 텍스트, 두번째필드가 Value )

        public int binding(CheckBoxList obj, String sql)
        {
            return binding(obj, sql, "");
        }

        public int binding(CheckBoxList obj, String sql, String 빈값)
        {
            int retVal = 0;

            obj.Items.Clear();

            if (빈값 != "")
            {
                obj.Items.Add(new ListItem(빈값, ""));
            }

            DataView dv = SqlDvQuery(sql);

            for (int i = 0; i < dv.Count; i++)
            {
                obj.Items.Add(new ListItem(dv[i][0].ToString(), dv[i][1].ToString()));
            }

            retVal = dv.Count;

            dv.Dispose();
            dv = null;


            return retVal;
        }

        #endregion

        #region binding : DropDownList 에 리스트 출력 ( 쿼리에서 첫번째 필드가 텍스트, 두번째필드가 Value )

        public int binding(DropDownList obj, String sql)
        {
            int retVal = 0;

            try
            {
                retVal = binding(obj, sql, "");
            }
            catch (Exception ex)
            {

            }
            return retVal;
        }

        public int binding(DropDownList obj, String sql, String 빈값)
        {
            int retVal = 0;
            String setVal = "";

            try
            {
                setVal = obj.SelectedValue;
            }
            catch (Exception ex2)
            {

            }

            try
            {


                obj.Items.Clear();

                if (빈값 != "")
                {
                    obj.Items.Add(new ListItem(빈값, ""));
                }

                DataView dv = SqlDvQuery(sql);
                for (int i = 0; i < dv.Count; i++)
                {
                    obj.Items.Add(new ListItem(dv[i][0].ToString(), dv[i][1].ToString()));
                }
                if (setVal != "" || dv.Count == 1)
                {
                    setDropDownList(obj, setVal);
                }

                retVal = dv.Count;

                dv.Dispose();
                dv = null;

            }
            catch (Exception ex)
            {

            }
            return retVal;
        }
        #endregion

        #region 현재 사용자의 IP 주소 가져오기
        public String GetIP(Page 페이지)
        {
            String retVal = "";

            try
            {
                retVal = 페이지.Request.UserHostAddress;
            }
            catch (Exception ex) { }

            return retVal;
        }
        #endregion

        #region 현재 도메인가져오기 https://aaa.com
        public String GetDomain(Page 페이지)
        {
            String retVal = "";
            String ServerPort = 페이지.Request.ServerVariables["SERVER_PORT"].ToString();
            String HTTP_HOST = 페이지.Request.ServerVariables["HTTP_HOST"].ToString();
            String PathInfo = 페이지.Request.ServerVariables["PATH_INFO"].ToString();
            String QueryString = 페이지.Request.ServerVariables["QUERY_STRING"].ToString();
            String HTTPSYN = 페이지.Request.ServerVariables["HTTPS"].ToString();

            try
            {
                if (HTTPSYN == "on")
                {
                    retVal = "https://" + HTTP_HOST;
                }
                else
                {
                    retVal = "http://" + HTTP_HOST;
                }

            }
            catch (Exception ex) { }

            return retVal;
        }
        #endregion

        #region 현재 URL전체 가져오기 https://aaa.com/smdi/test.aspx?321234=123&te=1324
        public String GetFullURL(Page 페이지)
        {
            String retVal = "";
            String ServerPort = 페이지.Request.ServerVariables["SERVER_PORT"].ToString();
            String HTTP_HOST = 페이지.Request.ServerVariables["HTTP_HOST"].ToString();
            String PathInfo = 페이지.Request.ServerVariables["PATH_INFO"].ToString();
            String QueryString = 페이지.Request.ServerVariables["QUERY_STRING"].ToString();
            String HTTPSYN = 페이지.Request.ServerVariables["HTTPS"].ToString();

            try
            {
                if (HTTPSYN == "on")
                {
                    retVal = "https://" + HTTP_HOST + PathInfo + "?" + QueryString;
                }
                else
                {
                    retVal = "http://" + HTTP_HOST + PathInfo + "?" + QueryString;
                }

            }
            catch (Exception ex) { }

            return retVal;
        }
        #endregion

        #region Req : Request 처리

        public String Req(Page 페이지, String 이름)
        {
            String retVal = "";

            retVal = Req(페이지, 이름, 0, "");

            return retVal;
        }

        public String Req(Page 페이지, String 이름, int 처리)
        {
            String retVal = "";

            retVal = Req(페이지, 이름, 처리, "");

            return retVal;
        }

        public String Req(Page 페이지, String 이름, String 기본값)
        {
            String retVal = "";

            retVal = Req(페이지, 이름, 기본값);

            return retVal;
        }

        public String Req(Page 페이지, String 이름, int 처리, String 기본값)
        {
            String retVal = "";

            try
            {

                retVal = 페이지.Request[이름].ToString();

                if (처리 == 1)
                {
                    retVal = retVal.Trim();
                }

                if (처리 == 2)
                {
                    retVal = retVal.Replace("'", "''");
                }

                if (처리 == 3)
                {
                    retVal = retVal.Replace("'", "''").Trim();
                }
            }
            catch (Exception ex)
            {

            }

            return retVal;
        }

        #endregion

        #region ReqC : Request Cookie

        public String ReqC(Page 페이지, String 이름)
        {
            String retVal = "";

            retVal = ReqC(페이지, 이름, false);

            return retVal;
        }

        public String ReqC(Page 페이지, String 이름, bool URL디코딩여부)
        {
            String retVal = "";

            try
            {
                String 복호화 = Decrypt_AES(페이지.Request.Cookies[이름].Value);
                String 날짜 = 복호화.Substring(Decrypt_AES(페이지.Request.Cookies[이름].Value).Length - (17 + 16), 17);
                String 생성ip = 복호화.Substring(Decrypt_AES(페이지.Request.Cookies[이름].Value).Length - (16), 16);

                String 복호IP = AddByteZero(GetIP(페이지).Replace(".", ""), 16);
                if (생성ip == 복호IP)
                {
                    retVal = 복호화.Substring(0, Decrypt_AES(페이지.Request.Cookies[이름].Value).Length - (17 + 16));
                }
                else
                {
                    페이지.Response.Cookies[이름].Value = "";
                }

            }
            catch (Exception ex)
            {
                페이지.Response.Cookies[이름].Value = "";
            }

            return retVal;
        }
        #endregion

        #region ResC : Response Cookie

        public void ResC(Page 페이지, String 이름, String 값)
        {
            ResC(페이지, 이름, 값, false);
        }

        public void ResC(Page 페이지, String 이름, String 값, bool URL디코딩여부)
        {
            try
            {
                String ip = AddByteZero(GetIP(페이지).Replace(".", ""), 16);
                페이지.Response.Cookies[이름].Value = Encrypt_AES(값 + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ip);
                if (값.Trim() == "")
                {
                    페이지.Response.Cookies[이름].Value = "";
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region ReqS : Request Session

        public String ReqS(Page 페이지, String 이름)
        {
            String retVal = "";

            retVal = ReqS(페이지, 이름, false);

            return retVal;
        }

        public String ReqS(Page 페이지, String 이름, bool URL디코딩여부)
        {
            String retVal = "";

            try
            {
                retVal = 페이지.Session[이름].ToString();

                if (URL디코딩여부)
                {
                    retVal = HttpUtility.UrlDecode(retVal);
                }
            }
            catch (Exception ex)
            {

            }

            return retVal;
        }
        #endregion

        #region ResS : Response Session

        public void ResS(Page 페이지, String 이름, String 값)
        {
            ResS(페이지, 이름, 값, false);
        }

        public void ResS(Page 페이지, String 이름, String 값, bool URL디코딩여부)
        {

            try
            {
                if (URL디코딩여부)
                {
                    페이지.Session[이름] = HttpUtility.UrlDecode(값);
                }
                else
                {
                    페이지.Session[이름] = 값;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region sc : javascript 실행
        public void sc(Page currentPage, string strScript)
        {
            currentPage.ClientScript.RegisterStartupScript(currentPage.GetType(), "sc", strScript, true);
        }
        #endregion

        #region GetAccessNum : 핸드폰번호를 기준으로 인증번호 추출 , 결과 : 인증번호
        public String GetAccessNum(String hp)
        {
            String retVal = "";

            try
            {
                Int64 tmp = Convert.ToInt64(hp.Replace("-", ""));
                Int64 tmp2 = Convert.ToInt64(DateTime.Now.ToString("MMdd"));
                Int64 tmp3 = Convert.ToInt64(9425964);
                Int64 tmp4 = Convert.ToInt64(tmp * tmp2 * tmp3);
                String tmp5 = tmp4.ToString().Replace("-", "").Substring(0, 6);

                retVal = tmp5;

            }
            catch (Exception ex)
            {
                retVal = "생성오류";
            }

            return retVal;
        }
        #endregion

        public String FcEditReplace(String org)
        {
            String ret = "";

            ret = org.Replace("<br>", "");

            return ret;
        }

        #region ProcIn : 프로시져 Input 값 생성 , 결과 : SqlParameter
        public SqlParameter ProcIn(String Name, SqlDbType t, String Value)
        {
            SqlParameter pin = new SqlParameter(Name, t);
            pin.Value = Value;
            pin.Direction = ParameterDirection.Input;
            return pin;
        }
        #endregion

        #region ProcOut : 프로시져 Output 값 생성 , 결과 : SqlParameter
        public SqlParameter ProcOut(String Name, SqlDbType t)
        {
            SqlParameter pout = new SqlParameter(Name, t);
            pout.Direction = ParameterDirection.Output;
            return pout;
        }
        #endregion

        #region CntBytesLen : 문자내역 bytes로 계산
        public int CntBytesLen(String tmp)
        {
            int retVal = 0;
            byte[] tmp2 = Encoding.Default.GetBytes(tmp);
            retVal = tmp2.Length;

            return retVal;
        }
        #endregion

        #region 휴대폰 번호 0101234567 010-0123-4567로 변경하는 것
        public string BarHP(String strHP)
        {
            String retVal = ConvertHP(strHP);
            try
            {
                retVal = retVal.Replace("-", "").Trim();

                if (retVal.Length == 11)
                {
                    retVal = retVal.Substring(0, 3) + "-" + retVal.Substring(3, 4) + "-" + retVal.Substring(7, 4);
                }
                else if (retVal.Length == 12)
                {
                    retVal = retVal.Substring(0, 4) + "-" + retVal.Substring(4, 4) + "-" + retVal.Substring(8, 4);
                }
                else if (retVal.Length == 10)
                {
                    retVal = retVal.Substring(0, 3) + "-" + retVal.Substring(3, 3) + "-" + retVal.Substring(6, 4);
                }
                else if (retVal.Length == 9)
                {
                    retVal = retVal.Substring(0, 2) + "-" + retVal.Substring(2, 3) + "-" + retVal.Substring(5, 4);
                }
                else
                {
                    retVal = retVal.Substring(0, 2) + "-" + retVal.Substring(2, 3) + "-" + retVal.Substring(5, (retVal.Length - 5));
                }

            }
            catch (Exception ex) { }
            return retVal;

        }
        #endregion

        public String ConvertDateTime(Object obj)
        {
            String retVal = "";

            try
            {
                retVal = ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
            }
            catch (Exception ex) { }

            return retVal;
        }

        public String setPercent(int Total, int cnt)
        {
            Double retVal = 0.0;
            String retStr = "0";
            try
            {
                retVal = Convert.ToDouble(cnt) / Convert.ToDouble(Total) * 100.0;
                if (retVal > 0)
                {
                    retStr = retVal.ToString().Substring(0, 4);
                }
                else
                {
                    retStr = "0";
                }
            }
            catch (Exception ex)
            {

            }
            return retStr;
        }

        #region getSelectedIndex
        public String getSelectedIndex(DropDownList dd)
        {
            String retVal = "";
            try
            {
                retVal = dd.SelectedIndex.ToString();
            }
            catch (Exception ex)
            {
            }
            return retVal;
        }
        #endregion

        #region getSelectedValue
        public String getSelectedValue(DropDownList dd)
        {
            String retVal = "";
            try
            {
                retVal = dd.SelectedValue;
            }
            catch (Exception ex)
            {
            }
            return retVal;
        }

        public String getSelectedValue(GridView gv)
        {
            String retVal = "";
            try
            {
                retVal = gv.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
            }
            return retVal;
        }
        #endregion

        #region getSelectedText
        public String getSelectedText(DropDownList dd)
        {
            String retVal = "";
            try
            {
                retVal = dd.SelectedItem.Text;
            }
            catch (Exception ex)
            {
            }
            return retVal;
        }
        #endregion

        #region setRadioButtonList : RadioButtonList 에 값 셋팅할때 사용
        public void setRadioButtonList(RadioButtonList rb, String SelectedValue)
        {
            setRadioButtonList(rb, SelectedValue, true);
        }

        public void setRadioButtonList(RadioButtonList rb, String SelectedValue, bool CheckDefaultValue)
        {
            try
            {
                rb.SelectedValue = SelectedValue;
            }
            catch (Exception ex)
            {
                if (CheckDefaultValue)
                {
                    if (rb.Items.Count > 0)
                    {
                        rb.SelectedIndex = 0;
                    }
                }
            }
        }

        public void setRadioButtonList(RadioButtonList rb, int SelectedValue)
        {
            try
            {
                rb.SelectedIndex = SelectedValue;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region setDropDownList : DropDownList 에 값 셋팅할때 사용
        public void setDropDownList(DropDownList dd, String SelectedValue)
        {
            try
            {
                dd.SelectedValue = SelectedValue;
            }
            catch (Exception ex)
            {
                if (dd.Items.Count > 0)
                {
                    dd.SelectedIndex = 0;
                }
            }
        }
        public void setDropDownList(DropDownList dd, int SelectedIndex)
        {
            try
            {
                dd.SelectedIndex = SelectedIndex;
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region isImageName : 파일명에서 확장자가 이미지일경우 true
        public bool isImageName(String Fname)
        {
            bool retVal = false;

            try
            {
                String[] tmp = Fname.Split('.');
                if (tmp.Length == 0)
                {
                    retVal = false;
                }
                else if (tmp.Length >= 1)
                {
                    String tmp2 = tmp[tmp.Length - 1];
                    if (tmp2.ToLower() == "jpg" || tmp2.ToLower() == "gif" || tmp2.ToLower() == "tiff" || tmp2.ToLower() == "tif" || tmp2.ToLower() == "png" || tmp2.ToLower() == "bmp")
                    {
                        retVal = true;
                    }
                    else
                    {
                        retVal = false;
                    }
                }
                else
                {
                    retVal = false;
                }

            }
            catch (Exception ex)
            {

            }

            return retVal;
        }
        #endregion

        #region DB값 치환처리 함수
        public String sg_db_query(String str)
        {
            return sg_db_query(str, false);
        }

        public String sg_db_query(String str, bool istite)
        {
            String ret_val = "";

            try
            {
                if (istite)
                {
                    ret_val = str.Replace("'", "''").Replace("%", "").Replace(";", "");
                }
                else
                {
                    ret_val = str.Replace("'", "''");
                }
            }
            catch (Exception ex)
            {

            }

            return ret_val;
        }
        #endregion

        #region isDate : 날짜 여부
        public bool isDate(String str)
        {
            bool retVal = false;

            try
            {
                if (str.Length == 10)
                {
                    DateTime dt = Convert.ToDateTime(str);
                }
                else if (str.Length == 19)
                {
                    DateTime dt = Convert.ToDateTime(str);
                }
                else
                {
                    String tmp = str.Substring(0, 4) + "-" + str.Substring(4, 2) + "-" + str.Substring(6, 2);
                    DateTime dt = Convert.ToDateTime(tmp);
                }

                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;
        }
        #endregion

        #region isNumeric : 숫자여부 확인
        public bool isNumeric(String str)
        {
            bool retVal = false;

            try
            {
                long dt = Convert.ToInt64(str);

                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;
        }
        #endregion

        #region IsNumeric : 숫자여부 확인
        public bool IsNumeric(string s)
        {
            bool retVal = false;

            try
            {
                long dt = Convert.ToInt64(s);

                retVal = true;
            }
            catch (Exception ex)
            {
                retVal = false;
            }

            return retVal;
        }
        #endregion

        #region IsEnglish :영문체크 영어만 사용되었으면 true 아니면 false
        public bool IsEnglish(string letter)
        {
            bool IsCheck = true;
            Regex engRegex = new Regex(@"[a-zA-Z]");
            Boolean ismatch = engRegex.IsMatch(letter);
            if (!ismatch)
            {
                IsCheck = false;
            }
            return IsCheck;
        }
        #endregion

        #region IsEnglishNumber :영문/숫자 체크 영어랑숫자만 사용되었으면 true 아니면 false
        public bool IsEnglishNumber(string letter)
        {
            bool IsCheck = true;
            Regex engRegex = new Regex(@"[a-zA-Z]");
            Boolean ismatch = engRegex.IsMatch(letter);
            Regex numRegex = new Regex(@"[0-9]");
            Boolean ismatchNum = numRegex.IsMatch(letter);

            if (!ismatch && !ismatchNum)
            {
                IsCheck = false;
            }
            return IsCheck;
        }
        #endregion

        #region SqlNoneQuery : 결과 bool
        public bool SqlNoneQuery(string sql)
        {
            return SqlNoneQuery2(sql);
        }
        #endregion

        #region SqlNoneQuery2 : 결과 bool
        public bool SqlNoneQuery2(string sql)
        {
            bool retVal = false;
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {

                connection.Open();
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.ExecuteNonQuery();
                connection.Close();
                retVal = true;

            } // End using

            return retVal;
        }
        #endregion

        #region SqlNoneQuery3 : 결과 bool 단, 타임아웃을 4분으로 지연하여 장기 쿼리시 사용 ( 취급주의!!! )
        public bool SqlNoneQuery3(string sql)
        {
            bool retVal = false;
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.CommandTimeout = 240;
                    command.ExecuteNonQuery();
                    connection.Close();
                    retVal = true;
                }
                catch (Exception ex)
                {

                }

            } // End using

            return retVal;
        }
        #endregion

        #region SqlDvQuery : 결과 DataView
        public DataView SqlDvQuery(string sql)
        {
            DataView dv = null;
            MySqlDataAdapter sda = new MySqlDataAdapter(sql, connStr);
            DataSet ds = new DataSet();
            sda.Fill(ds);

            dv = new DataView(ds.Tables[0]);

            return dv;
        }
        #endregion

        #region SqlFieldQuery : 쿼리의 최초 첫번째 필드를 리턴
        public string SqlFieldQuery(string sql)
        {
            string retVal = "";
            DataView dv = null;

            using (MySqlConnection connection = new MySqlConnection(connStr))
            {

                connection.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(sql, connection);
                MySqlCommandBuilder cb = new MySqlCommandBuilder(da);
                da.Fill(dt);
                dv = new DataView(dt);

                if (dv != null && dv.Count > 0)
                {
                    retVal = dv.Table.Rows[0][0].ToString();
                }

                connection.Close();


            } // End using

            return retVal;
        }
        #endregion

        public void LoadFile(System.Web.UI.WebControls.PlaceHolder holder, string path)
        {
            //localPath = 실제경로 
            string localPath = HttpContext.Current.Server.MapPath(path);
            string contents = "";
            try
            {
                contents = LoadTextFile(localPath);
            }
            catch
            {
            }

            System.Web.UI.LiteralControl control =
                new System.Web.UI.LiteralControl(contents);
            holder.Controls.Add(control);
        }

        public string LoadTextFile(string strPath)
        {
            byte[] byteBuf = LoadBinFile(strPath);
            return Encoding.UTF8.GetString(byteBuf);
        }

        public byte[] LoadBinFile(string strPath)
        {
            FileStream fsStream = File.OpenRead(strPath);
            long nLength = (new FileInfo(strPath).Length);
            byte[] byteBuf = new Byte[nLength];
            fsStream.Read(byteBuf, 0, byteBuf.Length);
            fsStream.Close();

            return byteBuf;
        }

        #region FileUploadName : 파일업로드시 파일명 생성하기 - 동일 파일이 이미 존재할 경우 파일명(+count)로 생성
        public string FileUploadName(String dirPath, String fileN)
        {
            string fileName = fileN;

            if (fileN.Length > 0)
            {
                int indexOfDot = fileName.LastIndexOf(".");
                string strName = fileName.Substring(0, indexOfDot);
                string strExt = fileName.Substring(indexOfDot);

                bool bExist = true;
                int fileCount = 0;

                string dirMapPath = string.Empty;

                while (bExist)
                {
                    dirMapPath = dirPath;
                    string pathCombine = System.IO.Path.Combine(dirMapPath, fileName);

                    if (System.IO.File.Exists(pathCombine))
                    {
                        fileCount++;
                        fileName = strName + "(" + fileCount + ")" + strExt;
                    }
                    else
                    {
                        bExist = false;
                    }
                }
            }

            return fileName;

        }
        #endregion

        #region rs : 그리드뷰에서 필드명으로 해당 필드의 위치 가져오기
        public int rs(GridView gv, String fldName)
        {
            int retval = 0;

            try
            {
                for (int i = 0; i < gv.Columns.Count; i++)
                {
                    if (gv.Columns[i].HeaderText.ToLower() == fldName.ToLower())
                    {
                        retval = i;
                        break;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return retval;
        }
        #endregion

        #region patchnull : null 문자 패치 ( 통신데이터에서 ASCII 코드가 00 인것을 공백으로 치환 )
        public String patchnull(String tmp)
        {
            String retVal = "";

            retVal = HttpUtility.UrlEncode(tmp).Replace("%00", "");

            return retVal;
        }
        #endregion

        #region FileExt : 파일확장자 가져오기 함수
        public String FileExt(String fname)
        {
            String retVal = "";

            try
            {
                retVal = fname.Substring(fname.LastIndexOf('.') + 1).ToLower();
            }
            catch (Exception ex)
            {

            }

            return retVal;
        }
        #endregion

        #region sghttp : POST 방식 기본 UTF-8
        public String sghttp(String strURL)
        {
            return sghttp(strURL, "UTF-8");
        }
        public String sghttp(String strURL, String Encode)
        {

            String ret_result = "-9999";

            try
            {

                byte[] tmp = Encoding.Default.GetBytes(strURL);

                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(strURL); // 객체를 생성한다.
                hwr.Method = "POST"; // 포스트 방식으로 전달
                hwr.ContentType = @"application/x-www-form-urlencoded";
                hwr.ContentLength = tmp.Length;

                Stream sendStream = hwr.GetRequestStream(); // sendStream 을 생성한다.
                sendStream.Write(tmp, 0, tmp.Length); // 데이터를 전송한다.
                sendStream.Close();              // sendStream 을 종료한다.

                HttpWebResponse hwrp = (HttpWebResponse)hwr.GetResponse();
                Stream strm = hwrp.GetResponseStream();
                StreamReader sr = new StreamReader(strm, Encoding.GetEncoding(Encode));
                string html = "";
                while (sr.Peek() > -1)
                {
                    html += sr.ReadLine();
                }
                int seq = -5964;
                try
                {
                    ret_result = html;
                }
                catch (Exception ex)
                {
                }
                sr.Close();
                strm.Close();
            }
            catch (Exception ex)
            {
                ret_result = "-5000" + ex.Message.ToString(); // 네트워크 오류 및 방화벽 등록 체크 
            }

            return ret_result; //ret_result;
        }
        #endregion

        #region sghttpget : http 통신 GET방식을 사용할 경우 기본 EUC-KR
        public String sghttpget(String strURL)
        {
            return sghttpget(strURL, "UTF-8");
        }

        public String sghttpget(String strURL, String Encode)
        {
            String ret_result = "-9999";

            try
            {

                byte[] tmp = Encoding.Default.GetBytes(strURL);

                HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(strURL); // 객체를 생성한다.
                hwr.Method = "GET"; // GET 방식으로 전달
                hwr.ContentType = "text/xml";

                HttpWebResponse hwrp = (HttpWebResponse)hwr.GetResponse();
                Stream strm = hwrp.GetResponseStream();
                StreamReader sr = new StreamReader(strm, Encoding.GetEncoding(Encode));
                string html = "";
                html += sr.ReadToEnd();
                int seq = -5964;
                try
                {
                    ret_result = html;
                }
                catch (Exception ex)
                {
                }
                sr.Close();
                strm.Close();
            }
            catch (Exception ex)
            {
                ret_result = "-5000" + ex.Message.ToString(); // 네트워크 오류 및 방화벽 등록 체크 
            }

            return ret_result; //ret_result;
        }
        #endregion

        #region sghttp_post : POST 값 전송
        public string sghttp_post(string url, Dictionary<string, string> postParameters)
        {
            string postData = "";
            string pageContent = "";

            try
            {

                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }

                HttpWebRequest myHttpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                myHttpWebRequest.Method = "POST";

                byte[] data = Encoding.Default.GetBytes(postData);

                myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
                myHttpWebRequest.ContentLength = data.Length;

                Stream requestStream = myHttpWebRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();

                Stream responseStream = myHttpWebResponse.GetResponseStream();

                StreamReader myStreamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));

                pageContent = myStreamReader.ReadToEnd();

                myStreamReader.Close();
                responseStream.Close();

                myHttpWebResponse.Close();
            }
            catch (Exception ex)
            {
                pageContent = "[Error]" + ex.Message.ToString();
            }

            return pageContent;
        }
        #endregion

        #region Encrypt_DES : Decrypt_DES : DES 8bit 암복호화 함수
        public String Encrypt_DES(String strKey)
        {
            return Encrypt_DES(strKey, "gtm56km4");
        }
        // 용도 : 간단히 URL 파라미터 보안등에 사용
        public String Encrypt_DES(String strKey, String EnKey)
        {
            String strReturn = "";

            if (strKey != null && strKey != "")
            {
                byte[] pbyteKey;
                pbyteKey = ASCIIEncoding.ASCII.GetBytes(EnKey);

                DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
                desCSP.Mode = CipherMode.ECB;
                desCSP.Padding = PaddingMode.PKCS7;
                desCSP.Key = pbyteKey;
                desCSP.IV = pbyteKey;

                MemoryStream ms = new MemoryStream();
                CryptoStream cryptStream = new CryptoStream(ms, desCSP.CreateEncryptor(), CryptoStreamMode.Write);
                byte[] data = Encoding.UTF8.GetBytes(strKey.ToCharArray());
                cryptStream.Write(data, 0, data.Length);
                cryptStream.FlushFinalBlock();

                strReturn = Convert.ToBase64String(ms.ToArray());
                cryptStream = null;
                ms = null;
                desCSP = null;
            }

            return strReturn;
        }
        public String Decrypt_DES(String strKey)
        {
            return Decrypt_DES(strKey, "gtm56km4");
        }

        public String Decrypt_DES(String strKey, String DeKey)
        {
            String strReturn = "";

            if (strKey != null && strKey != "")
            {

                byte[] pbyteKey;
                pbyteKey = ASCIIEncoding.ASCII.GetBytes(DeKey);

                DESCryptoServiceProvider desCSP = new DESCryptoServiceProvider();
                desCSP.Mode = CipherMode.ECB;
                desCSP.Padding = PaddingMode.PKCS7;
                desCSP.Key = pbyteKey;
                desCSP.IV = pbyteKey;
                MemoryStream ms = new MemoryStream();
                CryptoStream cryptStream = new CryptoStream(ms, desCSP.CreateDecryptor(), CryptoStreamMode.Write);
                strKey = strKey.Replace(" ", "+");
                byte[] data = Convert.FromBase64String(strKey);
                cryptStream.Write(data, 0, data.Length);
                cryptStream.FlushFinalBlock();

                strReturn = patchnull(Encoding.UTF8.GetString(ms.GetBuffer()));

                cryptStream = null;
                ms = null;
                desCSP = null;

            }

            return strReturn;
        }

        #endregion

        #region Encrypt_SHA : SHA256Bit 암호화 함수
        // 용도 : 비밀번호용. 복호화가 필요 없이 분실시 새로 생성해야 하는 경우 사용
        public string Encrypt_SHA(string Data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }
        #endregion

        #region Encrypt_AES : Decrypt_AES : AES 256Bit 암복호화 함수
        // 용도 : 주민번호, 신용카드번호 등 복호화 해야 하는 경우 처리
        public string Encrypt_AES(string InputText)
        {
            return Encrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
        }

        public string Encrypt_AES(string Input, string key)
        {
            String Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Convert.ToBase64String(xBuff);
            }

            return Output;
        }

        public string Decrypt_AES(string InputText)
        {
            return Decrypt_AES(InputText, "gtm56km412#$%inb5040sr!@#$%&^&(I");
        }

        public string Decrypt_AES(string Input, string key)
        {
            String Output = "";

            if (Input != "")
            {
                RijndaelManaged aes = new RijndaelManaged();
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                var decrypt = aes.CreateDecryptor();
                byte[] xBuff = null;
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Convert.FromBase64String(Input);
                        cs.Write(xXml, 0, xXml.Length);
                    }

                    xBuff = ms.ToArray();
                }

                Output = Encoding.UTF8.GetString(xBuff);
            }

            return patchnull(Output);
        }

        public String AESEncrypt128(String Input, String key)
        {

            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            //RijndaelCipher.Padding = PaddingMode.None;
            //RijndaelCipher.Mode = CipherMode.ECB;
            //RijndaelCipher.Padding = PaddingMode.PKCS7;

            byte[] PlainText = System.Text.Encoding.Default.GetBytes(Input);
            byte[] Salt = Encoding.Default.GetBytes(key.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(PlainText, 0, PlainText.Length);
            cryptoStream.FlushFinalBlock();

            byte[] CipherBytes = memoryStream.ToArray();

            memoryStream.Close();
            cryptoStream.Close();

            StringBuilder sb = new StringBuilder();
            foreach (byte b in CipherBytes)
            {
                sb.Append(b.ToString("X2"));
            }

            string EncryptedData = sb.ToString().ToUpper();

            return EncryptedData;
        }

        //AE_S128 복호화
        public String AESDecrypt128(String Input, String key)
        {
            Dictionary<string, byte> hexindex = new Dictionary<string, byte>();
            for (int i = 0; i <= 255; i++)
            {
                hexindex.Add(i.ToString("X2"), (byte)i);
            }

            List<byte> hexres = new List<byte>();
            for (int i = 0; i < Input.Length; i += 2)
                hexres.Add(hexindex[Input.Substring(i, 2)]);

            byte[] EncryptedData = hexres.ToArray();

            RijndaelManaged RijndaelCipher = new RijndaelManaged();
            RijndaelCipher.Padding = PaddingMode.None;

            byte[] Salt = Encoding.Default.GetBytes(key.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            byte[] PlainText = new byte[EncryptedData.Length];

            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string DecryptedData = Encoding.ASCII.GetString(PlainText, 0, DecryptedCount);

            return DecryptedData;
        }


        #endregion

        #region RemoveHTML : HTML제거

        public string RemoveHTML(String src)
        {
            return Regex.Replace(src, @"[<][a-z|A-Z|/](.|\n)*?[>]", "");

        }

        #endregion

        #region 강력한 비밀번호 체크
        public bool StrongCheckPw(String pw)
        {
            bool retVal = false;
            Regex reg1 = new Regex("\\d"); // 숫자찾기
            Regex reg2 = new Regex("[a-z]|[A-Z]"); // 영문문자찾기
            Regex reg3 = new Regex("\\W"); // 특수문자찾기

            if (pw.Length > 7 && pw.Length < 31 && reg1.IsMatch(pw) && reg2.IsMatch(pw) && reg3.IsMatch(pw))
            {
                retVal = true;
            }
            return retVal;
        }
        #endregion

        #region  페이지 권한 체크
        public void PermitPage(Page P, String PageNo)
        {
            String UserID = AuthUser.gUserID.ToString();

            String AdminCnt = SqlFieldQuery("select count(*) from smart36db.dbo.직원2 where ID='" + UserID + "' and 구분='4' and 거래처id='1001' ");
            String cnt = SqlFieldQuery("select count(*) from smart36db.dbo.t_메뉴 where id='" + PageNo + "' and 공통메뉴여부='Y' ");

            // 공통 메뉴가 아닐경우
            if (cnt == "0")
            {
                // 관리자가 아닐경우
                if (AdminCnt == "0")
                {
                    String sql = "select count(*) from smart36db.dbo.t_메뉴권한 where 직원ID='" + UserID + "' and 메뉴ID='" + PageNo + "' ";
                    String PageChk = SqlFieldQuery(sql);
                    if (PageChk == "0")
                    {
                        P.Response.Redirect("/Menu/Denied.aspx?id=" + PageNo + "&UserID=" + UserID);
                        P.Response.End();
                    }
                }

            }
        }

        public bool PermitCheck(Page P, String PageNo)
        {
            bool retVal = false;
            String UserID = AuthUser.gUserID.ToString();

            String cnt = SqlFieldQuery("select count(*) from smart36db.dbo.t_메뉴 with (NOLOCK) where id='" + PageNo + "' and 공통메뉴여부='Y' ");
            // 공통 메뉴일 경우 권한체크 안함
            if (cnt == "0")
            {

                String PageChk = SqlFieldQuery("select count(*) from smart36db.dbo.t_메뉴권한 with (NOLOCK) where 직원ID='" + UserID + "' and 메뉴ID='" + PageNo + "' ");
                if (PageChk == "0")
                {
                    retVal = false;
                }
                else
                {
                    retVal = true;
                }

                //String AdminCnt = SqlFieldQuery("select count(*) from smart36db.dbo.직원2 where ID='" + UserID + "' and 구분='4' ");
                //if (AdminCnt != "0")
                //{
                //    retVal = true;
                //}
            }
            else
            {
                retVal = true;
            }

            return retVal;
        }


        public string PermitMail(Page P, String MailNo)
        {
            String sender = "";
            DataView dv = SqlDvQuery("select 이메일, 이름 from smart36db.dbo.t_자동메일권한 with(nolock) where 삭제일시 is null and 메일id='" + MailNo + "' order by 이름");

            if (dv != null && dv.Count > 0)
            {
                for (int i = 0; dv.Count > i; i++)
                {
                    if (i == 0)
                    {
                        sender += dv[i]["이메일"].ToString() + "^" + dv[i]["이름"].ToString();
                    }
                    else
                    {
                        sender += "|" + dv[i]["이메일"].ToString() + "^" + dv[i]["이름"].ToString();
                    }
                }
            }

            return sender;
        }
        #endregion

        #region FileDelete : 파일삭제 , 정상 , 실패
        public String FileDelete(String strPath)
        {
            String RetVal = "정상";

            try
            {
                File.Delete(strPath);
            }
            catch (Exception ex)
            {
                RetVal = "실패 | " + ex.Message.ToString();
            }

            return RetVal;
        }
        #endregion

        #region ConvertHP(String hp)
        public String ConvertHP(String hp)
        {
            String RetVal = "";
            String php = hp.Replace("-", "").Trim();
            try
            {
                // 01088671000 : 11
                // 0168761000 : 10
                int hplen = php.Length;

                if (hplen == 11)
                {
                    RetVal = php.Substring(0, 3) + "-" + php.Substring(3, 4) + "-" + php.Substring(7, 4);
                }
                else if (hplen == 12)
                {
                    RetVal = php.Substring(0, 4) + "-" + php.Substring(4, 4) + "-" + php.Substring(8, 4);
                }
                else if (hplen == 10)
                {
                    RetVal = php.Substring(0, 3) + "-" + php.Substring(3, 3) + "-" + php.Substring(6, 4);
                }
                else
                {
                    RetVal = hp;
                }

            }
            catch (Exception ex)
            {
                RetVal = hp;
            }

            return RetVal;
        }
        #endregion

        #region Convert계좌(String Num)
        public String Convert계좌(String Num)
        {
            String RetVal = "";

            try
            {
                // 01088671000 : 11
                // 0168761000 : 10
                int Numlen = Num.Length;

                if (Numlen < 9)
                {
                    RetVal = Num.Substring(0, 4) + "-" + Num.Substring(4, Numlen - 4);
                }

                if (Numlen > 8 && Numlen < 13)
                {
                    RetVal = Num.Substring(0, 4) + "-" + Num.Substring(4, 4) + "-" + Num.Substring(8, Numlen - 8);
                }

                if (Numlen > 12 && Numlen < 17)
                {
                    RetVal = Num.Substring(0, 4) + "-" + Num.Substring(4, 4) + "-" + Num.Substring(8, 4) + "-" + Num.Substring(12, Numlen - 12);
                }

            }
            catch (Exception ex)
            {
                RetVal = Num;
            }

            return RetVal;
        }
        #endregion

        #region "Tiff 이미지 공용변수"
        private const string TIFF_CODEC = "image/tiff";
        private const string TIFF_FILE_EXTENSION = ".tiff";
        private const long ENCODING_SCHEME = (long)EncoderValue.CompressionCCITT4;
        #endregion

        #region TiffSplit(string fileName, string destFolder) : TIFF 이미지 나누어 저장하기
        public void TiffSplit(string fileName, string destFolder)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(fileName);
            ImageCodecInfo codecInfo = GetCodecInfo(TIFF_CODEC);

            FrameDimension frameDim = new FrameDimension(image.FrameDimensionsList[0]);
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, ENCODING_SCHEME);

            for (int i = 0; i < image.GetFrameCount(frameDim); i++)
            {
                image.SelectActiveFrame(frameDim, i);

                string fileNameWOExt = Path.GetFileNameWithoutExtension(fileName);
                string newFileName = string.Concat(fileNameWOExt, "_", (i + 1).ToString(), TIFF_FILE_EXTENSION);

                string folder = Path.Combine(Path.GetDirectoryName(fileName), destFolder);
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                image.Save(Path.Combine(folder, newFileName), codecInfo, encoderParams);
            }
        }
        #endregion

        #region TiffMerge(List<string> fileNames, string outputFileName) : TIFF 이미지 합치기
        public void TiffMerge(List<string> fileNames, string outputFileName)
        {
            ImageCodecInfo codecInfo = GetCodecInfo(TIFF_CODEC);

            EncoderParameters encoderParams = new EncoderParameters(2);
            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
            encoderParams.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.Compression, ENCODING_SCHEME);

            System.Drawing.Image image = System.Drawing.Image.FromFile(fileNames[0]);

            if (!Directory.Exists(Path.GetDirectoryName(outputFileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(outputFileName));
            }

            image.Save(outputFileName, codecInfo, encoderParams);

            encoderParams.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
            FrameDimension frameDim = new FrameDimension(image.FrameDimensionsList[0]);

            for (int i = 1; i < image.GetFrameCount(frameDim); i++)
            {
                image.SelectActiveFrame(frameDim, i);
                image.SaveAdd(image, encoderParams);
            }

            System.Drawing.Image[] images = new System.Drawing.Image[fileNames.Count - 1];
            for (int i = 1; i < fileNames.Count; i++)
            {
                images[i - 1] = System.Drawing.Image.FromFile(fileNames[i]);
                frameDim = new FrameDimension(images[i - 1].FrameDimensionsList[0]);

                for (int j = 0; j < images[i - 1].GetFrameCount(frameDim); j++)
                {
                    images[i - 1].SelectActiveFrame(frameDim, j);
                    image.SaveAdd(images[i - 1], encoderParams);
                }
            }

            image.Dispose();
            image = null;

        }
        #endregion

        #region "private functions"
        private ImageCodecInfo GetCodecInfo(string codec)
        {
            ImageCodecInfo codecInfo = null;

            foreach (ImageCodecInfo info in ImageCodecInfo.GetImageEncoders())
            {
                if (info.MimeType == codec)
                {
                    codecInfo = info;
                    break;
                }
            }

            return codecInfo;
        }
        #endregion

        #region 주민번호/외국인번호 유효성 체크  : 주민번호유효성체크(String 주민번호13자리)
        public bool 주민번호유효성체크(string s_rrn) // 유효성검사. 사용법RRNCheck("8201011234567");
        {
            if (rrncheck(s_rrn) || fgncheck(s_rrn))
            {
                return true;
            }
            return false;
        }

        public bool rrncheck(string s_rrn) // 주민등록번호유효성검사.
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(s_rrn.Substring(i, 1)) * ((i % 8) + 2);
            }

            if (((11 - (sum % 11)) % 10) == int.Parse(s_rrn.Substring(12, 1)))
            {
                return true;
            }
            return false;
        }

        public bool fgncheck(string s_rrn) // 외국인등록번호유효성검사.
        {
            int sum = 0;
            if (int.Parse(s_rrn.Substring(7, 2)) % 2 != 0)
            {
                return false;
            }

            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(s_rrn.Substring(i, 1)) * ((i % 8) + 2);
            }

            if ((((11 - (sum % 11)) % 10 + 2) % 10) == int.Parse(s_rrn.Substring(12, 1)))
            {
                return true;
            }

            return false;
        }
        #endregion

        #region 이메일전송


        /// <summary>
        /// 메일 발송하기
        /// </summary>
        /// <param name="pFromEmail">보내는 사람 이메일 주소</param>
        /// <param name="pFromName">보내는 사람 이름</param>
        /// <param name="pToEmailNameList">받는 사람 리스트 (이메일^이름|이메일^이름| 형식)</param>
        /// <param name="pTitle">메일 제목</param>
        /// <param name="pContents">메일 내용</param>
        /// <param name="pAttachPath">첨부파일 경로</param>
        /// <param name="pAttachName">첨부파일 이름</param>
        /// <returns>string</returns>
        /// 
        public string MailSender(string pToEmailNameList, string pTitle, string pContents, string pAttachPath, string pAttachName)
        {
            String pFromEmail = "system@smart36.co.kr";
            String pFromName = "시스템";
            String pFromPassword = "tltmxpa12#$%";

            return MailSender(pToEmailNameList, pTitle, pContents, pAttachPath, pAttachName, pFromEmail, pFromName, pFromPassword);
        }

        public string MailSender(string pToEmailNameList, string pTitle, string pContents, string pAttachPath, string pAttachName, String pFromEmail, String pFromName, String pFromPassword)
        {
            String retVal = "";

            try
            {
                MailAddress _FromAddr = new MailAddress(pFromEmail, pFromName);
                MailAddressCollection _ToAddrList = new MailAddressCollection();

                char[] _sperator1 = new char[] { '|' };
                char[] _sperator2 = new char[] { '^' };

                string[] _arrToUser = pToEmailNameList.Split(_sperator1, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < _arrToUser.Length; i++)
                {
                    string[] _arrToEmailName = _arrToUser[i].Split(_sperator2, StringSplitOptions.RemoveEmptyEntries);
                    _ToAddrList.Add(new MailAddress(_arrToEmailName[0], _arrToEmailName[1]));
                }

                IEnumerator _ie = _ToAddrList.GetEnumerator();
                MailMessage _Message = new MailMessage();
                _Message.From = _FromAddr;

                while (_ie.MoveNext())
                {
                    _Message.To.Add((MailAddress)_ie.Current);
                }

                _Message.Subject = pTitle;
                _Message.Body = pContents;
                _Message.IsBodyHtml = true;

                if (pAttachName != null && pAttachName != "")
                {
                    var _Attachment = new Attachment(pAttachPath + pAttachName);
                    _Attachment.Name = pAttachName;
                    _Message.Attachments.Add(_Attachment);
                }

                SmtpClient _SClient = new SmtpClient("smtp.worksmobile.com");
                _SClient.Port = 587;
                _SClient.EnableSsl = true;
                _SClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                _SClient.Credentials = new System.Net.NetworkCredential(pFromEmail, pFromPassword);
                _SClient.Send(_Message);

                retVal = "SUCCESS";
            }
            catch (Exception ex)
            {
                retVal = ex.Message.ToString() + ex.StackTrace + ex.Source;
            }

            return retVal;

        }

        /// <summary>
        /// 메일 수신자
        /// </summary>
        /// <param name="pDS">받는 사람 데이터셋</param>
        /// <returns>받는 사람 리스트 (이메일^이름|이메일^이름| 형식)</returns>
        public string MailReceiver(DataSet pDS)
        {
            string _Emails = string.Empty;

            if (pDS != null && pDS.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in pDS.Tables[0].Rows)
                {
                    _Emails = dr["email"].ToString().Trim() + "^" + dr["user_name"].ToString().Trim() + "|";
                }
            }

            return _Emails;
        }
        #endregion

        #region POST 파일 전송 함수
        public String LottePostFileSend(String PostURL, String FileName, String floc, String cid)
        {
            // Read file data
            FileStream fs = new FileStream(floc, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", FileName);
            postParameters.Add("cid", cid);
            postParameters.Add("rfile", new FileParameter(data, FileName, "application/" + FileExt(FileName)));

            // Create request and receive response
            string postURL = PostURL;
            string userAgent = "Someone";
            HttpWebResponse webResponse = MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            return fullResponse;
        }

        public String PostFileSend(String PostURL, String FileDir, String FileName, String SaveDir)
        {
            // Read file data
            FileStream fs = new FileStream(FileDir + FileName, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", FileName);
            postParameters.Add("fileformat", FileExt(FileName));
            postParameters.Add("rfile", new FileParameter(data, FileName, "application/" + FileExt(FileName)));

            // Create request and receive response
            string postURL = PostURL + "?loc=" + SaveDir + "&size=" + data.Length.ToString();
            string userAgent = "Someone";
            HttpWebResponse webResponse = MultipartFormDataPost(postURL, userAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();

            return fullResponse;
        }

        private static readonly Encoding encoding = Encoding.UTF8;

        public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        {
            string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());
            string contentType = "multipart/form-data; boundary=" + formDataBoundary;

            byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

            return PostForm(postUrl, userAgent, contentType, formData);
        }

        private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        {
            HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

            if (request == null)
            {
                throw new NullReferenceException("request is not a http request");
            }

            // Set up the request properties.
            request.Method = "POST";
            request.ContentType = contentType;
            request.UserAgent = userAgent;
            request.CookieContainer = new CookieContainer();
            request.ContentLength = formData.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(formData, 0, formData.Length);
                requestStream.Close();
            }

            return request.GetResponse() as HttpWebResponse;
        }

        private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        {
            Stream formDataStream = new System.IO.MemoryStream();
            bool needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;

                if (param.Value is FileParameter)
                {
                    FileParameter fileToUpload = (FileParameter)param.Value;

                    // Add just the first part of this param, since we will write the file data directly to the Stream
                    string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
                        boundary,
                        param.Key,
                        fileToUpload.FileName ?? param.Key,
                        fileToUpload.ContentType ?? "application/octet-stream");

                    formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                    // Write the file data directly to the Stream, rather than serializing it to a string.
                    formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                }
                else
                {
                    string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                        boundary,
                        param.Key,
                        param.Value);
                    formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                }
            }

            // Add the end of the request.  Start with a newline
            string footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            byte[] formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        public class FileParameter
        {
            public byte[] File { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
            public FileParameter(byte[] file) : this(file, null) { }
            public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
            public FileParameter(byte[] file, string filename, string contenttype)
            {
                File = file;
                FileName = filename;
                ContentType = contenttype;
            }
        }
        #endregion

        #region 랜덤문자열을 만들어주는 함수
        public String randomtext(int strLen)
        {
            int rnum = 0;
            int i, j;
            String ranStr = null;

            System.Random ranNum = new System.Random();

            for (i = 0; i <= strLen; i++)
            {
                for (j = 0; j <= 122; j++)
                {
                    rnum = ranNum.Next(48, 123);
                    if (rnum >= 48 && rnum <= 122 && (rnum <= 57 || rnum >= 65) && (rnum <= 90 || rnum >= 97))
                    {
                        break;
                    }
                }

                ranStr += Convert.ToChar(rnum);
            }

            return ranStr;
        }
        #endregion

        #region 미성년자여부체크
        public bool 미성년자여부(string 주민번호) // 유효성검사. 사용법RRNCheck("8201011234567");
        {
            bool retVal = false;

            String 생년월일 = 주민번호.Substring(0, 6);
            String 구분 = 주민번호.Substring(6, 1);

            if (구분 == "9" || 구분 == "0")
            {
                생년월일 = "18" + 생년월일;
            }
            else if (구분 == "1" || 구분 == "2" || 구분 == "5" || 구분 == "6")
            {
                생년월일 = "19" + 생년월일;
            }
            else if (구분 == "3" || 구분 == "4" || 구분 == "7" || 구분 == "8")
            {
                생년월일 = "20" + 생년월일;
            }

            생년월일 = 생년월일.Substring(0, 4) + "-" + 생년월일.Substring(4, 2) + "-" + 생년월일.Substring(6, 2);

            String 현재년월 = DateTime.Now.ToString("yyyy-MM-dd");

            Int32 나이 = Convert.ToInt32(현재년월.Substring(0, 4)) - Convert.ToInt32(생년월일.Substring(0, 4)) + 1;

            나이--;

            if (나이 < 19)
            {
                retVal = true;
            }

            return retVal;
        }
        #endregion

        #region 통화표시 화폐단위 FormatNumber
        public String FormatNumber(string 통화) // 유효성검사. 사용법RRNCheck("8201011234567");
        {
            String retVal = 통화;
            try
            {
                retVal = String.Format("{0:#,##0}", Convert.ToInt64(통화));
            }
            catch (Exception ex)
            {

            }

            return retVal;

        }
        #endregion

        #region 문자발송 함수

        public string XmlHttpRequest(string urlString, string xmlContent, string EncodeName)
        {
            String DefaultEncode = "UTF-8";
            if (EncodeName != "")
            {
                DefaultEncode = EncodeName;
            }
            string response = null;
            System.Net.HttpWebRequest httpWebRequest = null;    //Declare an HTTP-specific implementation of the WebRequest class.
            System.Net.HttpWebResponse httpWebResponse = null;  //Declare an HTTP-specific implementation of the WebResponse class

            //Creates an HttpWebRequest for the specified URL.
            httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(urlString);

            try
            {
                byte[] bytes;
                bytes = System.Text.Encoding.GetEncoding(DefaultEncode).GetBytes(xmlContent);

                //Set HttpWebRequest properties
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Headers.Add("x-waple-authorization", "MzY2Ni0xNDUxODY5NDI3OTQ3LTgxNDRiMjZkLTRjYzAtNDYyYy04NGIyLTZkNGNjMDM2MmMyNQ==");

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    //Writes a sequence of bytes to the current stream 
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();//Close stream
                }

                //Sends the HttpWebRequest, and waits for a response.
                httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    //Get response stream into StreamReader
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                            response = reader.ReadToEnd();
                    }
                }
                httpWebResponse.Close();//Close HttpWebResponse
            }
            catch (System.Net.WebException we)
            {   //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                httpWebResponse.Close();
                //Release objects
                httpWebResponse = null;
                httpWebRequest = null;
            }
            return response;
        }

        public String SendSMS(String NumFrom, String NumTo, String Mesg, String UserID)
        {
            return SendSMS(NumFrom, NumTo, Mesg, UserID, "", "");
        }
        public String SendSMS(String NumFrom, String NumTo, String Mesg, String UserID, String orderNum)
        {
            return SendSMS(NumFrom, NumTo, Mesg, UserID, orderNum, "");
        }

        public String SendSMS(String NumFrom, String NumTo, String Mesg, String UserID, String orderNum, String date_reserv)
        {
            String sms_user_id = "smart36";
            String sms_user_pw = "tmakxm36!@#$";
            String sms_send_lev = "";
            String groupid = "0";  // 개별
            String sql = "";
            String Result = "";

            if (orderNum != "")
            {
                if (SqlFieldQuery("select count(*) from n방송주문 where 주문번호='" + orderNum + "'") == "0")
                {
                    Result = "존재하지않는 주문번호입니다";
                }
            }

            String strParam = "";

            String url = "http://api.smart36.co.kr:5555/SMS/Send.aspx";
            strParam = "?sms_user_id=" + Encrypt_DES(sms_user_id) + "&sms_user_pw=" + Encrypt_DES(sms_user_pw) + "&sms_num_from=" + Encrypt_DES(NumFrom) + "&sms_num_to=" + Encrypt_DES(NumTo) + "&sms_mesg=" + Encrypt_DES(Mesg) + "&date_reserv=" + Encrypt_DES(date_reserv) + "&sms_send_lev=" + Encrypt_DES(sms_send_lev) + "&orderNum=" + Encrypt_DES(orderNum) + "&UserID=" + UserID + "&groupid=" + groupid;

            String RetVal = "";

            RetVal = sghttpget(url + strParam);

            String str결과코드 = "";
            String str결과내역 = "";

            String[] result = RetVal.Split('|');

            str결과코드 = result[0].ToString();
            str결과내역 = result[1].ToString();

            if (str결과코드 == "0")
            {
                Result = "정상발송";

                if (orderNum != "")
                {
                    sql = "insert into n해피콜상담내역 (주문번호, 상태, 상담내역, 상담사id) ";
                    sql += " values ('" + orderNum + "', '문자발송', '발신번호 : " + ConvertHP(NumFrom) + "<br>수신번호 : " + ConvertHP(NumTo) + "<br>" + Mesg + " (" + DateTime.Now.ToString("yyyy-MM-dd- HH:mm:ss") + ")', '" + UserID + "')";

                    SqlNoneQuery2(sql);
                }
            }
            else
            {
                Result = str결과내역;
            }

            return Result;
        }



        public String SetSMS(String NumFrom, String DESC, String UserID)
        {

            String StrParams = "";
            String Result = "";
            String 설명 = sg_db_query(DESC);

            StrParams = "sendnumber=" + NumFrom + "&comment=" + UserID + ":" + 설명;
            Result = XmlHttpRequest("http://api.openapi.io/ppurio/1/sendnumber/save/smart362", StrParams, "UTF-8");

            SqlNoneQuery2("INSERT INTO [smart36db].[dbo].[t_문자발송_발송번호](num_from,num_desc,user_id) values('" + NumFrom + "','" + DESC + "','" + UserID + "')");

            return Result;

        }


        public String SendKMS(String NumFrom, String NumTo, String Mesg, String UserID, String STYPE, String PARAMS)
        {
            String sms_user_id = "smart36";
            String sms_user_pw = "tmakxm36!@#$";
            String date_reserv = "";
            String sms_send_lev = "";
            String groupid = "0";  // 개별
            String orderNum = "";
            String Result = "";


            String strParam = "";

            String url = "http://api.smart36.co.kr:5555/SMS/KSend.aspx";
            strParam = "?&stype=" + STYPE + "&sms_user_id=" + Encrypt_DES(sms_user_id) + "&sms_user_pw=" + Encrypt_DES(sms_user_pw) + "&sms_num_from=" + Encrypt_DES(NumFrom) + "&sms_num_to=" + Encrypt_DES(NumTo) + "&sms_mesg=" + Encrypt_DES(Mesg) + "&date_reserv=" + Encrypt_DES(date_reserv) + "&sms_send_lev=" + Encrypt_DES(sms_send_lev) + "&orderNum=" + Encrypt_DES(orderNum) + "&UserID=" + UserID + "&groupid=" + groupid + "&sms_params=" + Encrypt_DES(PARAMS);

            String RetVal = "";

            RetVal = sghttpget(url + strParam);

            String str결과코드 = "";
            String str결과내역 = "";

            String[] result = RetVal.Split('|');

            str결과코드 = result[0].ToString();
            str결과내역 = result[1].ToString();

            if (str결과코드 == "0")
            {
                Result = "정상발송";
            }
            else
            {
                Result = str결과내역;
            }

            return Result;
        }
        #endregion

        #region String ByteSubstring(String Data,int StartIdx, int byteLength)
        public String ByteSubstring(String Data, int StartIdx, int byteLength)
        {
            String retVal = "";

            byte[] byteTEMP = Encoding.Default.GetBytes(Data);

            retVal = Encoding.Default.GetString(byteTEMP, StartIdx, byteLength);

            return retVal;
        }
        #endregion

        #region String ByteSubstring(String Data,int StartIdx, int byteLength)
        public Single sSng(String Data)
        {
            Single retVal = 0;
            try
            {
                retVal = Convert.ToSingle(Data.Trim());
            }
            catch (Exception ex)
            {

            }


            return retVal;
        }
        #endregion

        #region 일자 0 치환
        private string strLenth(int i)
        {
            return i.ToString().Length < 2 ? "0" + i.ToString() : i.ToString();
        }
        #endregion

        #region AddByteBlank
        public String AddByteBlank(String Val, int len)
        {
            String retVal = Val;

            if (CntBytesLen(retVal) < len)
            {
                int addBlank = len - CntBytesLen(retVal);
                for (int i = 0; i < addBlank; i++)
                {
                    retVal += " ";
                }
            }

            return retVal;
        }
        #endregion

        #region AddByteZero
        public String AddByteZero(String Val, int len)
        {
            String retVal = Val;

            if (CntBytesLen(retVal) < len)
            {
                int addBlank = len - CntBytesLen(retVal);
                for (int i = 0; i < addBlank; i++)
                {
                    retVal = "0" + retVal;
                }
            }

            return retVal;
        }
        #endregion

        #region FTP 파일존재 여부 

        public bool FtpFileExists(String strFtpAddress, String strFtpId, String strFtpPwd, String FileName)
        {
            bool IsExists = true;

            FtpWebRequest reqFTP = null;
            FtpWebResponse respFTP = null;

            try
            {

                UriBuilder URI = new UriBuilder(strFtpAddress + FileName);

                URI.Scheme = "ftp";

                reqFTP = (FtpWebRequest)WebRequest.Create(URI.Uri);
                reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                respFTP = (FtpWebResponse)reqFTP.GetResponse();
                if (respFTP.StatusCode == System.Net.FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    IsExists = false;
                }

            }
            catch
            {
                IsExists = false;
            }
            finally
            {
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }
            return IsExists;
        }

        #endregion

        #region FTP 파일삭제 
        public bool FtpFileDelete(String strFtpAddress, String strFtpId, String strFtpPwd, string FileName)
        {
            bool isOk = true;

            FtpWebRequest reqFTP = null;
            FtpWebResponse respFTP = null;
            try
            {

                UriBuilder URI = new UriBuilder(strFtpAddress + FileName);
                URI.Scheme = "ftp";
                reqFTP = (FtpWebRequest)WebRequest.Create(URI.Uri);
                reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                respFTP = (FtpWebResponse)reqFTP.GetResponse();

                reqFTP = null;
                respFTP = null;
            }

            catch (Exception ex)
            {
                isOk = false;
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }
            return isOk;
        }

        #endregion

        #region FTP 파일다운로드 
        public bool FtpFileDownload(String strFtpAddress, String strFtpId, String strFtpPwd, string ftpFileName, String SaveFileName)
        {
            bool isOk = true;

            UriBuilder URI = new UriBuilder(strFtpAddress + ftpFileName);
            URI.Scheme = "ftp";

            FtpWebRequest reqFTP = (FtpWebRequest)WebRequest.Create(URI.Uri);
            reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd);
            reqFTP.KeepAlive = false;
            reqFTP.UseBinary = true;
            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
            reqFTP.UsePassive = true;


            FtpWebResponse respFTP = (FtpWebResponse)reqFTP.GetResponse();

            Stream strm = respFTP.GetResponseStream();
            FileStream writeStream = null;

            try
            {
                writeStream = new FileStream(SaveFileName, FileMode.Create);
                int Length = 2048;

                Byte[] buffer = new Byte[Length];
                int bytesRead = strm.Read(buffer, 0, Length);

                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = strm.Read(buffer, 0, Length);
                }
                strm.Close();
                writeStream.Close();

                reqFTP = null;
                respFTP = null;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (writeStream != null)
                {
                    writeStream.Close();
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }

            return isOk;
        }

        #endregion

        #region FTP 파일업로드

        public bool FtpFileUpload(String strFtpAddress, String strFtpId, String strFtpPwd, string FileName, string saveFileName)
        {
            bool isOk = true;
            //업로드할 파일 
            FileInfo fi = new FileInfo(FileName);
            string strFileName = saveFileName;
            string strFileFullName = fi.FullName;


            FtpWebRequest reqFTP;

            UriBuilder URI = new UriBuilder(strFtpAddress + strFileName);
            URI.Scheme = "ftp";

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(URI.Uri);
            reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd); // 아이디 비밀번호 삽입
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;      // 바이너리로 전송함
            reqFTP.UsePassive = true;

            int bufferLength = 2048;
            byte[] buff = new byte[bufferLength];

            int contentLen;
            Stream strm = null;
            FileStream fs = null;
            try
            {
                fs = fi.OpenRead();

                strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, bufferLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, bufferLength);
                }
                strm.Close();
                fs.Close();

                reqFTP = null;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (fi != null)
                {
                    fi = null;
                }

            }

            return isOk;
        }

        public bool FtpFileUpload(String strFtpAddress, String strFtpId, String strFtpPwd, string FileName)
        {
            bool isOk = true;
            //업로드할 파일 
            FileInfo fi = new FileInfo(FileName);
            string strFileName = fi.Name;
            string strFileFullName = fi.FullName;


            FtpWebRequest reqFTP;

            UriBuilder URI = new UriBuilder(strFtpAddress + strFileName);
            URI.Scheme = "ftp";

            reqFTP = (FtpWebRequest)FtpWebRequest.Create(URI.Uri);
            reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd); // 아이디 비밀번호 삽입
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;      // 바이너리로 전송함
            reqFTP.UsePassive = true;

            int bufferLength = 2048;
            byte[] buff = new byte[bufferLength];

            int contentLen;
            Stream strm = null;
            FileStream fs = null;
            try
            {
                fs = fi.OpenRead();

                strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, bufferLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, bufferLength);
                }
                strm.Close();
                fs.Close();

                reqFTP = null;
            }
            catch (Exception ex)
            {
                isOk = false;
                throw new Exception(ex.Message.ToString());
            }
            finally
            {
                if (strm != null)
                {
                    strm.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                }
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (fi != null)
                {
                    fi = null;
                }

            }

            return isOk;
        }

        #endregion

        #region FTP 디렉토리 생성

        public bool FtpCreateDirectory(String strFtpAddress, String strFtpId, String strFtpPwd, string DirectoryName)
        {
            bool isOk = true;
            FtpWebRequest reqFTP = null;
            FtpWebResponse respFTP = null;

            strFtpAddress += DirectoryName;

            try
            {
                UriBuilder URI = new UriBuilder(strFtpAddress);
                URI.Scheme = "ftp";

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(URI.Uri);
                reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd); // 아이디 비밀번호 삽입
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

                respFTP = (FtpWebResponse)reqFTP.GetResponse();

                if (respFTP.StatusCode == System.Net.FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    isOk = false;
                }
            }
            catch
            {
                isOk = false;
            }
            finally
            {
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }

            return isOk;
        }

        #endregion

        #region FTP 파일리스트가져오기

        public String FtpFileList(String strFtpAddress, String strFtpId, String strFtpPwd, String strList)
        {
            bool IsExists = true;

            String[] List;

            FtpWebRequest reqFTP = null;
            FtpWebResponse respFTP = null;

            try
            {

                UriBuilder URI = new UriBuilder(strFtpAddress);

                URI.Scheme = "ftp";

                reqFTP = (FtpWebRequest)WebRequest.Create(URI.Uri);
                reqFTP.Credentials = new NetworkCredential(strFtpId, strFtpPwd);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                respFTP = (FtpWebResponse)reqFTP.GetResponse();
                if (respFTP.StatusCode == System.Net.FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    IsExists = false;
                }
                StreamReader reader = new StreamReader(respFTP.GetResponseStream(), System.Text.Encoding.Default);
                strList = reader.ReadToEnd();

            }
            catch
            {
                IsExists = false;
            }
            finally
            {
                if (reqFTP != null)
                {
                    reqFTP = null;
                }
                if (respFTP != null)
                {
                    respFTP = null;
                }
            }

            return strList;
        }

        #endregion

        #region 만 나이를 리턴한다. 0보다 작을경우 변환 오류처리 해야함
        public int Age(String BirthyyyyMMdd)
        {
            int retVal = -999;
            String Birth = BirthyyyyMMdd.Replace("-", "").Replace("/", "").Replace(".", "").Trim();

            try
            {
                if (Birth.Length != 8)
                {
                    return -1;
                }

                if (!isNumeric(Birth))
                {
                    return -2;
                }

                DateTime dt = Convert.ToDateTime(Birth.Substring(0, 4) + "-" + Birth.Substring(4, 2) + "-" + Birth.Substring(6, 2));
                DateTime now = DateTime.Now;

                int iAge = 0;
                int by = dt.Year;
                int bm = dt.Month;
                int bd = dt.Day;

                int ny = now.Year;
                int nm = now.Month;
                int nd = now.Day;

                if (bm < nm)
                {
                    iAge = ny - by;
                }
                else if (bm == nm)
                {
                    if (bd <= nd)
                    {
                        iAge = ny - by;
                    }
                    else
                    {
                        iAge = ny - by - 1;
                    }

                }
                else
                {
                    iAge = ny - by - 1;
                }

                retVal = iAge;
            }
            catch (Exception ex)
            {
                retVal = -888;
            }

            return retVal;
        }
        #endregion

        #region 날짜에 대한 요일을 가져온다
        public String getWeek(String d)
        {
            return getWeek(Convert.ToDateTime(d));
        }

        public String getWeek(DateTime d)
        {
            String retVal = "";

            switch (d.DayOfWeek.ToString("d"))
            {
                case "0": retVal = "일"; break;
                case "1": retVal = "월"; break;
                case "2": retVal = "화"; break;
                case "3": retVal = "수"; break;
                case "4": retVal = "목"; break;
                case "5": retVal = "금"; break;
                case "6": retVal = "토"; break;
            }

            return retVal;
        }
        #endregion

        #region 공휴일
        public String getHoliday(String d)
        {
            String retVal = "";

            retVal = SqlFieldQuery("select 사유 from smart36db.dbo.tbl_공휴일 where 공휴일='" + d + "' ");

            return retVal;
        }
        #endregion

        #region 이미지처리 함수 

        #region BitmapToBase64

        public String BitmapToBase64(Bitmap img)
        {
            var ms = new System.IO.MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            var base64String = Convert.ToBase64String(byteImage);
            return base64String;
        }

        #endregion

        #region Base64ToImage 
        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String); MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Bitmap image = new Bitmap(ms, true);
            image.MakeTransparent(Color.White);
            return image;
        }
        #endregion

        #region ImageToBase64

        public string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }


        #endregion

        #region ResizeImage
        public Bitmap ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        #endregion

        #endregion

        #region 이미지코덱정보 가져오기
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
        #endregion

        #region 한글만 사용되었는지 체크 ( 한글만 있으면 True , 다른게 있으면 false )

        public bool isOnlyHangle(String txt)
        {
            bool retval = false;

            string str = @"^[가-힣]*$";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);
            retval = rex.IsMatch(txt);

            return retval;

        }
        #endregion

        #region 문자열에 한글이 포함되어 있으면 true 없으면 false
        public bool isHangul(string s)
        {
            char[] charArr = s.ToCharArray();

            foreach (char c in charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 특수문자가 포함되면 true 없으면 false 
        public bool isSptext(string txt)
        {
            bool retval = false;

            string str = @"[`~!\#$%^&*\()\=+|\\/:;?""<>']";
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(str);

            retval = rex.IsMatch(txt);

            if (txt.Contains("[") == true || txt.Contains("]") == true || txt.Contains("-") == true
                || txt.Contains("_") == true || txt.Contains("@") == true)
            {
                retval = true;
            }

            return retval;
        }
        #endregion

        #region 길이 split
        public String[] lensplit(string txt, int cutlen)
        {
            String[] retVal = null;

            if (txt == "" || txt.Length < cutlen)
            {
                retVal = null;
            }

            if (txt.Length == cutlen)
            {
                retVal = new string[1];
                retVal[0] = txt;
            }

            if (txt.Length > cutlen)
            {
                retVal = new string[txt.Length / cutlen];

                for (int i = 0; i < txt.Length / cutlen; i++)
                {
                    retVal[i] = txt.Substring(i * cutlen, cutlen);
                }
            }
            return retVal;
        }
        #endregion

        #region 가격정보로 빈 투출정보 생성 pay_get_out(String 상태정보)
        public String pay_get_out(String 상태정보)
        {
            String retval = "";

            // UM0080UH0210UQ0130UT0240UK0100EK0170US0100EC0110UJ0120
            // EA000200UR000200UN000200US000240XU000140UM000240UI000150EK110220XT000230
            상태정보 = 상태정보.Substring(4);
            String[] tmp = lensplit(상태정보, 6);
            for (int i = 0; i < tmp.Length; i++)
            {
                retval += tmp[i].ToString().Substring(0, 2)+"00"+ tmp[i].ToString().Substring(2);

            }

            return retval;
        }
        #endregion

        #region 투출정보 총건수 total_out(String 투출정보)
        public int total_out(String 투출정보)
        {
            int retval = 0;

            String[] tmp = lensplit(투출정보, 8);
            for (int i = 0; i < tmp.Length; i++)
            {
                int 투출건수 = sg_int(tmp[i].ToString().Substring(3, 1));

                retval = retval + 투출건수;

            }

            return retval;
        }
        #endregion

        #region 투출정보 건수 수정 change_out(String 투출정보, int 순번, int 판매건수)
        public String change_out(String 투출정보, int 순번, int 판매건수)
        {
            String retval = "";

            String[] tmp = lensplit(투출정보, 8);
            for (int i = 0; i < tmp.Length; i++)
            {
                if (순번 == i)
                {
                    int 잔여재고 = sg_int(tmp[i].ToString().Substring(4, 3));

                    tmp[i] = tmp[i].ToString().Substring(0, 2) + 판매건수.ToString() + 판매건수.ToString() + tmp[i].ToString().Substring(4, 3) + "0";
                }
            }

            retval = String.Join("", tmp);

            return retval;
        }
        #endregion

        #region 투출정보 매칭 change_out(String 투출정보, String 가격정보, int 검색가격)
        public String find_out(String 투출정보, String 가격정보, int 검색가격)
        {
            String retval = 투출정보;

            String[] tmp투출 = lensplit(투출정보, 8);
            String[] tmp가격 = lensplit(가격정보, 7);

            bool 찾음 = false;

            #region 가격정보 = 검색가격 이 일치하는경우 1건
            for (int i = 0; i < tmp가격.Length; i++)
            {
                if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격)
                {
                    retval = change_out(retval, i, 1);
                    찾음 = true;
                    break;
                }
            }
            #endregion

            #region 가격정보 = 검색가격/2 이 일치하는경우 동일2건 결제
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격 / 2)
                    {
                        retval = change_out(retval, i, 2);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 가격정보 = 검색가격/3 이 일치하는경우 동일3건 결제
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격 / 3)
                    {
                        retval = change_out(retval, i, 3);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 가격정보 = 검색가격/4 이 일치하는경우 동일4건 결제
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격 / 4)
                    {
                        retval = change_out(retval, i, 4);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 가격정보 = 검색가격/5 이 일치하는경우 동일5건 결제
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격 / 5)
                    {
                        retval = change_out(retval, i, 5);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 가격정보 = 검색가격/6 이 일치하는경우 동일6건 결제
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) == 검색가격 / 6)
                    {
                        retval = change_out(retval, i, 6);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 1번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[0].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 0, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 2번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[1].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 1, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 3번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[2].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 2, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 4번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[3].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 3, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 5번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[4].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 4, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 6번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[5].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 5, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 7번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[6].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 6, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 8번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[7].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 7, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            #region 검색가격 - 9번째 가격을 빼고 찾는경우 2건결제시 첫번째가 포함되는경우
            if (찾음 == false)
            {
                for (int i = 0; i < tmp가격.Length; i++)
                {
                    if (sg_int(tmp가격[i].Substring(2, 5)) + sg_int(tmp가격[8].Substring(2, 5)) == 검색가격)
                    {
                        retval = change_out(retval, 8, 1);
                        retval = change_out(retval, i, 1);
                        찾음 = true;
                        break;
                    }
                }
            }
            #endregion

            return retval;
        }
        #endregion

        #region 알림메세지 팝업

        public void sg_alert(Page p , String 메세지)
        {
            ((Label)p.Master.FindControl("lb_alert")).Text = 메세지;
            ((HtmlGenericControl)p.Master.FindControl("div_alert_bg")).Visible = true;
            ((HtmlGenericControl)p.Master.FindControl("div_alert")).Visible = true;
        }
        public void sg_alert_c(Page p)
        {
            ((Label)p.Master.FindControl("lb_alert")).Text = "";
            ((HtmlGenericControl)p.Master.FindControl("div_alert_bg")).Visible = false;
            ((HtmlGenericControl)p.Master.FindControl("div_alert")).Visible = false;
        }

        #endregion

    }
}