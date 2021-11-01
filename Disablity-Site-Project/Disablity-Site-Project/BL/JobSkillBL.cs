using Disablity_Site_Project.DAL;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.BL
{
    public class JobSkillBL
    {
        public List<JobSkill> GetActiveJobSkillsList(DatabaseEntities de)
        {
            return new JobSkillDAL().GetActiveJobSkillsList(de);
        }

        public JobSkill GetActiveJobSkillById(int _Id, DatabaseEntities de)
        {
            return new JobSkillDAL().GeteActiveJobSkillById(_Id, de);
        }

        public bool AddJobSkill(JobSkill _JobSkill, DatabaseEntities de)
        {
            if (_JobSkill.JobId== null||_JobSkill.SkillId==null)
            {
                return false;
            }
            else
            {
                return new JobSkillDAL().AddJobSkill(_JobSkill, de);
            }
        }

        public bool UpdateJobSkill(JobSkill _JobSkill, DatabaseEntities de)
        {
            if (_JobSkill.JobId == null || _JobSkill.SkillId == null)
            {
                return false;
            }
            else
            {
                return new JobSkillDAL().UpdateJobSkill(_JobSkill, de);
            }
        }

        public bool DeleteJobSkill(int _id, DatabaseEntities de)
        {
            return new JobSkillDAL().DeleteJobSkill(_id, de);
        }
    }
}