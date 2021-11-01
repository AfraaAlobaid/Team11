using Disablity_Site_Project.DAL;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Disablity_Site_Project.BL
{
    public class UserBL
    {
        public List<User> GetActiveUsersList(DatabaseEntities de)
        {
            return new UserDAL().GetActiveUsersList(de);
        }

        public User GetActiveUserById(int _Id, DatabaseEntities de)
        {
            return new UserDAL().GeteActiveUserById(_Id, de);
        }

        public bool AddUser(User _user, DatabaseEntities de)
        {
            if (_user.Name == ""||_user.ProfilePicture==""||_user.Gender=="" || _user.Contact == "" || _user.Type == "" || _user.Role == null || _user.Email == "" || _user.Password == "" || _user.ConfirmPassword == ""||_user.ShortDescription==""||_user.Type=="")
            {
                return false;
            }
            else
            {
                return new UserDAL().AddUser(_user, de);
            }
        }

        public bool UpdateUser(User _user, DatabaseEntities de)
        {
            if (_user.Name == "" || _user.Contact == ""||_user.Type=="" || _user.Role == null || _user.Email == ""||_user.Password==""||_user.ConfirmPassword=="")
            {
                return false;
            }
            else
            {
                return new UserDAL().UpdateUser(_user, de);
            }
        }

        public bool DeleteUser(int _id, DatabaseEntities de)
        {
            return new UserDAL().DeleteUser(_id, de);
        }
    }
}