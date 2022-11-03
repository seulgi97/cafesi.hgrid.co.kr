/// <summary>
/// SgFramework의 요약 설명입니다.
/// </summary>

using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;

namespace SgFramework
{
    public class SgExcel
    {
        public SgExcel()
        {
            
        }
        SgFramework.SgUtil su = new SgFramework.SgUtil();
        public void sql2excel(Page p , string sql,string 다운로드파일명)
        {

            if (sql.Trim() == "")
            {
                return;
            }
            if (다운로드파일명.Trim() == "")
            {
                return;
            }

            String tempfilename = DateTime.Now.ToString("yyyyMMdd") + AuthUser.gUserID + ".xlsx";
            String path = @"e:\tempdel\";
            if (Directory.Exists(path) == false)
            {
                path = @"d:\tempdel\";
            }

            if(File.Exists(path+ tempfilename) == true)
            {
                File.Delete(path + tempfilename);
            }

            FileInfo fi = new FileInfo( path+ tempfilename);
            using (ExcelPackage pack = new ExcelPackage())
            {
                DataView dv = su.SqlDvQuery(sql);

                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("p1");
                ws.Cells["A1"].LoadFromDataTable(dv.ToTable(), true);
                pack.SaveAs(fi);

                FileDownload(path + tempfilename, 다운로드파일명);
            }

            fi = null;
                
            GC.Collect();
        }

        /// <summary>
        /// cafesi는 현재 C드라이만 존재해서 따로 분리함
        /// </summary>
        /// <param name="p"></param>
        /// <param name="sql"></param>
        /// <param name="다운로드파일명"></param>
        public void sqlToExcel(Page p, string sql, string 다운로드파일명)
        {

            if (sql.Trim() == "")
            {
                return;
            }
            if (다운로드파일명.Trim() == "")
            {
                return;
            }

            String tempfilename = DateTime.Now.ToString("yyyyMMdd") + AuthUser.gUserID + ".xlsx";
            String path = @"c:\tempdel\";

            if (File.Exists(path + tempfilename) == true)
            {
                File.Delete(path + tempfilename);
            }

            FileInfo fi = new FileInfo(path + tempfilename);
            using (ExcelPackage pack = new ExcelPackage())
            {
                DataView dv = su.SqlDvQuery(sql);

                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("p1");
                ws.Cells["A1"].LoadFromDataTable(dv.ToTable(), true);
                pack.SaveAs(fi);

                FileDownload(path + tempfilename, 다운로드파일명);
            }

            fi = null;

            GC.Collect();
        }


        #region 대용량 파일 다운로드 처리 

        public void FileDownload(string filePath, string fileName)
        {
            HttpContext Context = HttpContext.Current;

            // 요청한 파일이 없으면 에러를 일으킨다. 
            if (File.Exists(filePath)==true)
            {
                // 해더를 추가한다 
                Context.Response.Clear();
                Context.Response.AddHeader("Content-Type", "Application/Unknown");
                Context.Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString());
                // attachment header 가 들어가 있는 경우 확장자에 관계 없이 무조건 다운로드 창이 뜬다
                Context.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpContext.Current.Server.UrlEncode(fileName).Replace("+", "%20") + ".xlsx");

                // 버퍼를 나눠읽어서 쓰는 방식이나.. 한꺼번에 버퍼를 쓰는 방식은 문제가 발생한다. 
                // 문제가 있기 때문에 asp.net 2.0 이상에서는 TransmitFile 를 사용한다.
                Context.Response.TransmitFile(filePath);
            }
        }

        #endregion
    }
 


}