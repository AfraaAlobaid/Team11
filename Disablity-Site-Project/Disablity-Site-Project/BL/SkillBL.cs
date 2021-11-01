using Disablity_Site_Project.DAL;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.BL
{
    public class SkillBL
    {
        public List<Skill> GetActiveSkillsList(DatabaseEntities de)
        {
            return new SkillDAL().GetActiveSkillsList(de);
        }

        public Skill GetActiveSkillById(int _Id, DatabaseEntities de)
        {
            return new SkillDAL().GeteActiveSkillById(_Id, de);
        }

        public bool AddSkill(Skill _Skill, DatabaseEntities de)
        {
            if (_Skill.Name == "" ||_Skill.JobId==null)
            {
                return false;
            }
            else
            {
                return new SkillDAL().AddSkill(_Skill, de);
            }
        }

        public bool UpdateSkill(Skill _Skill, DatabaseEntities de)
        {
            if (_Skill.Name == "" || _Skill.JobId == null)
            {
                return false;
            }
            else
            {
                return new SkillDAL().UpdateSkill(_Skill, de);
            }
        }

        public bool DeleteSkill(int _id, DatabaseEntities de)
        {
            return new SkillDAL().DeleteSkill(_id, de);
        }
    }
}