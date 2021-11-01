using Disablity_Site_Project.DAL;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.BL
{
    public class JobOfferBL
    {
        public List<JobOffer> GetActiveJobOffersList(DatabaseEntities de)
        {
            return new JobOfferDAL().GetActiveJobOffersList(de);
        }

        public JobOffer GetActiveJobOfferById(int _Id, DatabaseEntities de)
        {
            return new JobOfferDAL().GeteActiveJobOfferById(_Id, de);
        }

        public bool AddJobOffer(JobOffer j, DatabaseEntities de)
        {
            if (j.Budget==""||j.Title == "" || j.Description == ""/* || j.Budget == ""*/ || j.RequiredExp == "" || j.AttachedFile == ""||j.JobType==""||j.JobLocation=="" )
            {
                return false;
            }
            else
            {
                return new JobOfferDAL().AddJobOffer(j, de);
            }
        }

        public bool UpdateJobOffer(JobOffer j, DatabaseEntities de)
        {
            if (j.Budget == "" || j.Title == "" || j.Description == ""/* || j.Budget == ""*/ || j.RequiredExp == "" || j.AttachedFile == "" || j.JobType == "" || j.JobLocation == "")
            {
                return false;
            }
            else
            {
                return new JobOfferDAL().UpdateJobOffer(j, de);
            }
        }

        public bool DeleteJobOffer(int _id, DatabaseEntities de)
        {
            return new JobOfferDAL().DeleteJobOffer(_id, de);
        }
    }
}