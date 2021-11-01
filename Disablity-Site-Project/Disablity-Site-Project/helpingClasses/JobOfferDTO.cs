using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.helpingClasses
{
    public class JobOfferDTO
    {
        public int Id { get; set; }
        public string EncId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Budget { get; set; }
        public string RequiredExp { get; set; }
        public string JobType { get; set; }
        public Nullable<int> BuyerId { get; set; }
        public string JobLocation { get; set; }
        public string Image { get; set; }
        public string AttachedFile { get; set; }
        public string  CreatedAt { get; set; }

    }
}