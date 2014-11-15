using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

using olx.api.Helpers;
using olx.api.Models;

namespace olx.api.Controllers
{

    public class AdController : ApiController
    {

        [ActionName("All")]
        [HttpGet]
        public object All()
        {
            DateTime startTime = DateTime.Now;
            List<Ad> ads = new List<Ad>();
            var db = new Database();
            using (var result = db.View("vw_allAds"))
            {
                while (result.Read())
                {

                    ads.Add(new Ad
                    {
                        Id = result.GetInt32(0),
                        UniqueId = result.GetString(1),
                        Title = result.GetString(2),
                        Location = result.GetString(3),
                        AdId = result.GetString(4),
                        Price = result.GetString(5),
                        Seller = result.GetString(6),
                        SellerContact = result.GetString(7),
                        Description = result.GetString(8),
                        Type = result.GetString(9),
                        HasPictures = result.GetBoolean(10),
                        AddedOn = result.GetDateTime(11),
                        Url = result.GetString(12)
                    });
                }

            }
            db.Dispose();

            return new
            {
                TimeStamp = (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds),
                TimeConsumed = (DateTime.Now - startTime).TotalMilliseconds,
                TotalRecords = ads.Count,
                Status = "Success",
                Result = ads.OrderBy(x => x.Id).ToList()
            };
        }

        [ActionName("Get")]
        [HttpGet]
        public object Get(int iterator, int top = 100)
        {
            const string viewName = "vw_allAds";

            DateTime startTime = DateTime.Now;
            List<Ad> ads = new List<Ad>();
            var db = new Database();
            using (var result = db.Pagination(viewName, iterator, top))
            {
                while (result.Read())
                {

                    ads.Add(new Ad
                    {
                        Id = result.GetInt32(0),
                        UniqueId = result.GetString(1),
                        Title = result.GetString(2),
                        Location = result.GetString(3),
                        AdId = result.GetString(4),
                        Price = result.GetString(5),
                        Seller = result.GetString(6),
                        SellerContact = result.GetString(7),
                        Description = result.GetString(8),
                        Type = result.GetString(9),
                        HasPictures = result.GetBoolean(10),
                        AddedOn = result.GetDateTime(11),
                        Url = result.GetString(12)
                    });
                }

            }
            db.Dispose();

            return new
            {
                TimeStamp = (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds),
                TimeConsumed = (DateTime.Now - startTime).TotalMilliseconds,
                TotalRecords = ads.Count,
                Status = "Success",
                Result = ads.OrderBy(x => x.Id).ToList()
            };

        }

        [ActionName("Images")]
        [HttpGet]
        public object Images(string uniqueId)
        {
            DateTime startTime = DateTime.Now;
            const string guidPattern = @"\b[A-F0-9]{8}(?:-[A-F0-9]{4}){3}-[A-F0-9]{12}\b";
            Regex regex = new Regex(guidPattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(uniqueId))
            {

                List<string> images = new List<string>();
                var db = new Database();
                using (var result = db.ViewWithCondition("vw_imgs", string.Format("uniqueId = '{0}' and url not like '%nophoto%'", uniqueId)))
                {
                    while (result.Read())
                    {
                        images.Add(result.GetString(2));
                    }

                }
                db.Dispose();


                return new
                {
                    TimeStamp = (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds),
                    TimeConsumed = (DateTime.Now - startTime).TotalMilliseconds,
                    TotalRecords = images.Count,
                    Status = "Success",
                    Result = new { UniqueId = uniqueId, Images = images }
                };
            }

            return new
            {
                TimeStamp = (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds),
                TimeConsumed = (DateTime.Now - startTime).TotalMilliseconds,
                Status = "Error",
                Result = "Invalid Unique Id"
            };
        }

        [ActionName("Count")]
        [HttpGet]
        public object Count()
        {
            DateTime startTime = DateTime.Now;
            const string viewName = "vw_allAds";
            List<string> images = new List<string>();
            var db = new Database();
            var result = db.ExecuteScalar(string.Format("SELECT COUNT(id) FROM {0}", viewName));
            db.Dispose();

            return new
            {
                TimeStamp = (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds),
                TimeConsumed = (DateTime.Now - startTime).TotalMilliseconds,
                Status = "Success",
                Result = new { Count = result }
            };

        }
    }

}
