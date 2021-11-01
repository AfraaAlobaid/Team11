using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Disablity_Site_Project.DAL
{
    public class JobOfferDAL
    {
        public List<JobOffer> GetActiveJobOffersList(DatabaseEntities de)
        {
            return de.JobOffers.Where(x => x.IsActive == 1).ToList();
        }
        public JobOffer GeteActiveJobOfferById(int _Id, DatabaseEntities de)
        {
            return de.JobOffers.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }
        public bool AddJobOffer(JobOffer _JobOffer, DatabaseEntities de)
        {
            try
            {
                de.JobOffers.Add(_JobOffer);
                de.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        public bool UpdateJobOffer(JobOffer _JobOffer, DatabaseEntities de)
        {
            try
            {
                de.Entry(_JobOffer).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteJobOffer(int _Id, DatabaseEntities de)
        {
            try
            {
                JobOffer JobOffer = de.JobOffers.Where(x => x.Id == _Id).FirstOrDefault(/*x => x.IsActive == 1*/);
                JobOffer.IsActive = 0;
                de.Entry(JobOffer).State = System.Data.Entity.EntityState.Modified;
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