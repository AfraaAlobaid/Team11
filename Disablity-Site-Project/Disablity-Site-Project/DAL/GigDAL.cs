using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.DAL
{
    public class GigDAL
    {
        public List<Gig> GetActiveGigsList(DatabaseEntities de)
        {
            return de.Gigs.Where(x => x.IsActive == 1).ToList();
        }
        public Gig GeteActiveGigById(int _Id, DatabaseEntities de)
        {
            return de.Gigs.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }
        public bool AddGig(Gig _Gig, DatabaseEntities de)
        {
            try
            {
                de.Gigs.Add(_Gig);
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateGig(Gig _Gig, DatabaseEntities de)
        {
            try
            {
                de.Entry(_Gig).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteGig(int _Id, DatabaseEntities de)
        {
            try
            {
                Gig Gig = de.Gigs.Where(x => x.Id == _Id).FirstOrDefault(/*x => x.IsActive == 1*/);
                Gig.IsActive = 0;
                de.Entry(Gig).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}