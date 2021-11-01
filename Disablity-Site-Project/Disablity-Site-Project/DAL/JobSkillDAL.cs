using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.DAL
{
    public class JobSkillDAL
    {
        public List<JobSkill> GetActiveJobSkillsList(DatabaseEntities de)
        {
            return de.JobSkills.Where(x => x.IsActive == 1).ToList();
        }
        public JobSkill GeteActiveJobSkillById(int _Id, DatabaseEntities de)
        {
            return de.JobSkills.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }
        public bool AddJobSkill(JobSkill _JobSkill, DatabaseEntities de)
        {
            try
            {
                de.JobSkills.Add(_JobSkill);
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateJobSkill(JobSkill _JobSkill, DatabaseEntities de)
        {
            try
            {
                de.Entry(_JobSkill).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteJobSkill(int _Id, DatabaseEntities de)
        {
            try
            {
                JobSkill JobSkill = de.JobSkills.Where(x => x.Id == _Id).FirstOrDefault(/*x => x.IsActive == 1*/);
                JobSkill.IsActive = 0;
                de.Entry(JobSkill).State = System.Data.Entity.EntityState.Modified;
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