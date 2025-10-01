using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Web;
using System.Text;

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
                    string jsonPath = Server.MapPath("~/App_Data/article.json");
                    if (File.Exists(jsonPath))
                    {
                        string content = File.ReadAllText(jsonPath, Encoding.UTF8);
                        Response.Write(content);
                    }
                    else

                        Response.Write("[]");

                }

                else if (method == "POST")
                {

                }

            }


            Response.End();
        }
    }
}