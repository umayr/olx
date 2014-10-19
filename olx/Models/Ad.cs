using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using HtmlAgilityPack;
using olx.Helpers;

namespace olx.Models
{
    class Ad
    {
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string AdId { get; set; }
        public string Price { get; set; }
        public string Seller { get; set; }
        public string Description { get; set; }
        public string SellerContact { get; set; }
        public string Type { get; set; }
        public int HasPictures { get; set; }

        private string _guid;
        private IList<string> _pictureUrls;

        public static Database DB { get; set; }
        public Ad(HtmlNode root, string category, string guid)
        {
            _guid = guid;
            _pictureUrls = new List<string>();
            try
            {
                Title = (root.SelectSingleNode(Util.GetClassXPath("offerheadinner")))
                    .Element("h1").InnerText.Trim();

            }
            catch (NullReferenceException)
            {
                Title = "N/A";
            }

            try
            {
                Location = (root.SelectSingleNode(Util.GetClassXPath("show-map-link")))
                    .Element("strong").InnerText.Trim();
            }
            catch (NullReferenceException)
            {
                Location = "N/A";
            }
            try
            {
                //string adId = root.SelectSingleNode("\\p").InnerText.Trim();
                AdId = Util.LongBetween(555555555, 999999999)
                       .ToString(CultureInfo.InvariantCulture);

            }
            catch (NullReferenceException)
            {

                AdId = "N/A";
            }
            try
            {
                Price = root.SelectSingleNode(Util.GetClassXPath("pricelabel"))
                    .Element("strong").InnerText.Trim();

            }
            catch (NullReferenceException)
            {
                Price = "N/A";
            }

            try
            {

                Seller = (root.SelectSingleNode(Util.GetClassXPath("userdetails")))
                    .Element("span").FirstChild.InnerText.Trim();
            }
            catch (NullReferenceException)
            {
                Seller = "N/A";
            }
            try
            {
                SellerContact = (root.SelectSingleNode(Util.GetIdXPath("contact_methods")))
                    .SelectSingleNode(Util.GetClassXPath("contactitem"))
                    .Element("strong").InnerText.Trim();
            }
            catch (NullReferenceException)
            {
                SellerContact = "N/A";
            }

            try
            {
                Description = (root.SelectSingleNode(Util.GetIdXPath("textContent")))
                          .Element("p").InnerText.Trim();

            }
            catch (NullReferenceException)
            {
                Description = "N/A";
            }

            Type = category;

            HasPictures = _hasPictures(root);


        }

        public bool Save()
        {
            List<SqlParameter> param = new List<SqlParameter>
            {
                new SqlParameter("@unique_id", _guid),
                new SqlParameter("@title", Title),
                new SqlParameter("@location", Location),
                new SqlParameter("@ad_id", AdId),
                new SqlParameter("@price", Price),
                new SqlParameter("@seller", Seller),
                new SqlParameter("@seller_contact", SellerContact),
                new SqlParameter("@description", Description),
                new SqlParameter("@type", Type),
                new SqlParameter("@has_picture", HasPictures),
                new SqlParameter("@raw_price", int.Parse(new string(Price.AsEnumerable().Where(char.IsDigit).ToArray())))
            };

            foreach (List<SqlParameter> _params in _pictureUrls.Select(imgUrl => new List<SqlParameter>{
                new SqlParameter("@unique_id", _guid),
                new SqlParameter("@url", imgUrl)
            }))
            {
                DB.ExecuteProcedure("usp_insertImgUrl", _params);
            }
            return DB.ExecuteProcedure("usp_insertAd", param);

        }

        private int _hasPictures(HtmlNode root)
        {
            try
            {

                var imgs = root.Descendants("img").ToList();
                if (!imgs.Any())
                    return 0;
                foreach (var img in imgs)
                {
                    _pictureUrls.Add(img.Attributes["src"].Value.Trim());
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public readonly static string[] Categories = { "mobiles-tablets", "electronics-computers", "vehicles", "home-furniture", "kids-baby-products", "fashion-beauty", "books-sports-hobbies", "animals", "services", "jobs", "real-estate" };
        private static readonly CultureInfo CultureInfo = new CultureInfo("en-IN");

    }
}
