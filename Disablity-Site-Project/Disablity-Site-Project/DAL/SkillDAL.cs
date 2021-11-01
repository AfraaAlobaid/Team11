using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.DAL
{
    public class SkillDAL
    {
        public List<Skill> GetActiveSkillsList(DatabaseEntities de)
        {
            return de.Skills.Where(x => x.IsActive == 1).ToList();
        }
        public Skill GeteActiveSkillById(int _Id, DatabaseEntities de)
        {
            return de.Skills.Where(x => x.Id == _Id).FirstOrDefault(x => x.IsActive == 1);
        }
        public bool AddSkill(Skill _Skill, DatabaseEntities de)
        {
            try
            {
                de.Skills.Add(_Skill);
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateSkill(Skill _Skill, DatabaseEntities de)
        {
            try
            {
                de.Entry(_Skill).State = System.Data.Entity.EntityState.Modified;
                de.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteSkill(int _Id, DatabaseEntities de)
        {
            try
            {
                Skill Skill = de.Skills.Where(x => x.Id == _Id).FirstOrDefault(/*x => x.IsActive == 1*/);
                Skill.IsActive = 0;
                de.Entry(Skill).State = System.Data.Entity.EntityState.Modified;
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