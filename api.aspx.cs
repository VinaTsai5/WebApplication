using Newtonsoft.Json;
using System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication2
{
    public partial class api : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/json";

            string url = Page.RouteData.Values["url"] as string;
            string method = Request.HttpMethod;

            if (url == "article")
            {
                if (method == "GET")
                {
                    HandleGetArticles();
                }
                else if (method == "POST")
                {
                    HandleFormPostArticle();
                }
                else
                {
                    Response.StatusCode = 405; Response.Write("{\"error\":\"Method not allowed\"}");
                }
            }
            else
            {
                Response.StatusCode = 404; Response.Write("{\"error\":\"Not found\"}");
            }

            Response.End();
        }

        private void HandleGetArticles()
        {
            string jsonPath = Server.MapPath("~/App_Data/article.json");
            if (File.Exists(jsonPath))
                Response.Write(File.ReadAllText(jsonPath, Encoding.UTF8));
            else
                Response.Write("[]");
        }

        private void HandleFormPostArticle()
        {
            string title = Request.Form["title"]?.Trim();
            string content = Request.Form["content"]?.Trim();
            string date = Request.Form["date"]?.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(date))
            {
                string redirectUrl = "~/edit.html?error=" + HttpUtility.UrlEncode("標題、內容和日期為必填欄位");
                Response.Redirect(redirectUrl, false);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                return;
            }

            var newArticle = new
            {
                id = Guid.NewGuid().ToString(),
                title = title,
                content = content,
                date = date,
                image = "",
                tags = new string[0],
                createdAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            SaveArticle(newArticle);
            string successUrl = "~/edit.html?success=" + HttpUtility.UrlEncode("文章新增成功！");
            Response.Redirect(successUrl, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        private void SaveArticle(object newArticle)
        {
            string jsonPath = Server.MapPath("~/App_Data/article.json");
            List<object> articles = new List<object>();

            if (File.Exists(jsonPath))
            {
                try
                {
                    articles = JsonConvert.DeserializeObject<List<object>>(File.ReadAllText(jsonPath, Encoding.UTF8)) ?? new List<object>();
                }
                catch
                {
                    articles = new List<object>();
                }
            }

            articles.Insert(0, newArticle);
            string appDataPath = Server.MapPath("~/App_Data/");
            if (!Directory.Exists(appDataPath))
                Directory.CreateDirectory(appDataPath);
            File.WriteAllText(jsonPath, JsonConvert.SerializeObject(articles, Formatting.Indented), Encoding.UTF8);
        }
    }
}