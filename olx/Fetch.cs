using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using olx.Helpers;
using olx.Models;

namespace olx
{
    class Fetch
    {
        private string _currentCategory;
        private int _currentPage;
        private int? _totalPages;
        private int? _totalRecords;
        private string _logText;
        private Database _db;
        public int? Begin(string category)
        {
            Console.WriteLine("Fetching /{0}", category);
            _currentCategory = category;
            _totalRecords = 0;
            _currentPage = 1;
            _db = new Database();
            Ad.DB = _db;

            ProcessUrl(Util.GetUrl(category));

            while (_currentPage <= _totalPages)
            {
                Console.WriteLine("Page {0}/{1}", _currentPage, _totalPages);
                ProcessPage(category, _currentPage);
                Console.WriteLine("Completed!");
                _currentPage++;
            }

            using (StreamWriter file = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + @"\Log.txt"))
            {
                file.Write(_logText);
            }

            return _totalRecords;
        }

        private void ProcessPage(string category, int page)
        {
            string categoryUrl = Util.GetUrl(category, page);
            try
            {
                var enumerable = ProcessUrl(categoryUrl);
                int iterator = 1;
                foreach (string url in enumerable)
                {
                    string link = url.Split('#')[0];
                    string guid = Guid.NewGuid().ToString();
                    var _params = new List<SqlParameter>
                    {
                        new SqlParameter {ParameterName = "@unique_id", Value = guid, SqlDbType = SqlDbType.VarChar},
                        new SqlParameter {ParameterName = "@url", Value = link, SqlDbType = SqlDbType.VarChar}
                    };

                    string result = _db.ExecuteProcedureScalar("usp_insertUrl", _params);
                    
                    if (result == null)
                        DoProcess(guid, link);
                  
                    DrawTextProgressBar(iterator, enumerable.Count);
                    iterator++;
                }

                _totalRecords += (iterator - 1);
            }
            catch (Exception ex)
            {
                _logText += GetLogMessage("Failed to fetch a page", categoryUrl, ex.Message, ex.StackTrace);
            }
        }
        private IList<string> ProcessUrl(string url)
        {
            var doc = new HtmlDocument();
            WebRequest request = WebRequest.Create(url);
            doc.Load(request.GetResponse().GetResponseStream());

            if (!_totalPages.HasValue)
                _totalPages = GetTotalPages(doc.DocumentNode);

            Regex regex = new Regex(pattern: "http://olx\\.com\\.pk/item/([^\"]*)");
            var allAnchors = doc.DocumentNode.Descendants("a")
                .Where(x => x.ParentNode.Name == "h3")
                .Select(a => a.GetAttributeValue("href", null))
                .Where(u => !String.IsNullOrEmpty(u) && regex.IsMatch(u));

            return allAnchors as IList<string> ?? allAnchors.ToList();
        }
        private bool DoProcess(string guid, string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Proxy = null;
            HtmlDocument document = new HtmlDocument();
            request.Method = "GET";

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    document.Load(stream);
                }
            }

            var offerRoot = document.DocumentNode.SelectSingleNode(Util.GetClassXPath("offerbody"));

            try
            {
                Ad ad = new Ad(offerRoot, _currentCategory, guid);
                return ad.Save();
            }
            catch (Exception ex)
            {
                _logText += GetLogMessage("Failed to fetch an entry", url, ex.Message, ex.StackTrace);
                return false;
            }

        }
        private static void DrawTextProgressBar(int progress, int total)
        {
            Console.CursorLeft = 0;
            Console.Write("[");
            Console.CursorLeft = 32;
            Console.Write("]");
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.CursorLeft = position++;
                Console.Write("=");
            }

            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("{0} of {1}   ", progress, total);
        }
        private static int GetTotalPages(HtmlNode documentNode)
        {
            var lastPage = documentNode
                .SelectSingleNode(Util.GetClassXPath("pager"))
                .LastChild
                .PreviousSibling
                .PreviousSibling
                .PreviousSibling
                .InnerText.Trim();
            return int.Parse(lastPage);
        }
        private static string GetLogMessage(string title, string url, string reason, string stacktrace)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("---------------------------");
            sb.Append(Environment.NewLine);
            sb.Append(title.ToUpper());
            sb.Append(Environment.NewLine);
            sb.Append("---------------------------");
            sb.Append(Environment.NewLine);
            sb.Append("URL:");
            sb.Append(Environment.NewLine);
            sb.Append("\t");
            sb.Append(url);
            sb.Append(Environment.NewLine);
            sb.Append("TIME:");
            sb.Append(Environment.NewLine);
            sb.Append("\t");
            sb.Append(DateTime.Now.ToLongTimeString());
            sb.Append(Environment.NewLine);
            sb.Append("REASON:");
            sb.Append(Environment.NewLine);
            sb.Append("\t");
            sb.Append(reason);
            sb.Append(Environment.NewLine);
            sb.Append("STACKTRACE:");
            sb.Append(Environment.NewLine);
            sb.Append("\t");
            sb.Append(stacktrace);
            sb.Append(Environment.NewLine);
            return sb.ToString();
        }
    }
}
