using System;

namespace olx.api.Models
{
    public class Ad
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string AdId { get; set; }
        public string Price { get; set; }
        public string Seller { get; set; }
        public string Description { get; set; }
        public string SellerContact { get; set; }
        public string Type { get; set; }
        public Boolean HasPictures { get; set; }
        public DateTime AddedOn { get; set; }
        public string Url { get; set; }
    }
}
