using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Web;

namespace WebApplication2
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //show article.json 
            // 取得 App_Data\article.json 的絕對路徑
            string jsonPath = Server.MapPath("~/App_Data/article.json");
            // 讀取檔案內容
            string jsonContent = File.ReadAllText(jsonPath);
            // 輸出到網頁（Content-Type 設為 application/json）
            Response.ContentType = "application/json";
            Response.Write(jsonContent);
            Response.End();
        }
    }
}