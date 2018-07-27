using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HtmlAgilityPack;

using BorsaProjesi.Models;
using System.Text;
using Newtonsoft.Json;

namespace BorsaProjesi.Controllers
{
    public class HomeController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            string link = "https://www.doviz.com/";

            Uri url = new Uri(link);

            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string html = client.DownloadString(url);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            var secilenhtml = @"/html/body/header/div/div[1]/div/ul";

            StringBuilder st = new StringBuilder();

            var secilenHtmlList = document.DocumentNode.SelectNodes(secilenhtml);

            var verilerList = new List<Icerik>();

            foreach (var items in secilenHtmlList)
            {
                foreach (var innerItem in items.SelectNodes("li"))
                {
                    var veri = new Icerik();

                    foreach (var item in innerItem.SelectNodes("a//span"))
                    {

                        var classValue = item.Attributes["class"] == null ? null : item.Attributes["class"].Value;

                        if (classValue == "menu-row1")
                        {
                            st.AppendLine(item.InnerText);
                            veri.isim = item.InnerText;
                        }
                        else if (classValue == "menu-row2")
                        {
                            st.AppendLine(item.InnerText);
                            veri.deger = item.InnerText;

                        }

                        else if (classValue == "menu-row3")
                        {
                            st.AppendLine(item.InnerText);
                            veri.yuzde = item.InnerText;
                        }
                    }
                    verilerList.Add(veri);
                }
            }
            return Json(verilerList);
        }


    }
}
