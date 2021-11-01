using Disablity_Site_Project.DAL;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.BL
{
    public class GigBL
    {
        public List<Gig> GetActiveGigsList(DatabaseEntities de)
        {
            return new GigDAL().GetActiveGigsList(de);
        }

        public Gig GetActiveGigById(int _Id, DatabaseEntities de)
        {
            return new GigDAL().GeteActiveGigById(_Id, de);
        }

        public bool AddGig(Gig j, DatabaseEntities de)
        {
            if (j.Title == "" || j.description == "" || j.Price == "" || j.Image == "")
            {
                return false;
            }
            else
            {
                return new GigDAL().AddGig(j, de);
            }
        }

        public bool UpdateGig(Gig j, DatabaseEntities de)
        {
            if (j.Title == "" || j.description == "" || j.Price == "" || j.Image == "")
            {
                return false;
            }
            else
            {
                return new GigDAL().UpdateGig(j, de);
            }
        }

        public bool DeleteGig(int _id, DatabaseEntities de)
        {
            return new GigDAL().DeleteGig(_id, de);
        }
    }
}