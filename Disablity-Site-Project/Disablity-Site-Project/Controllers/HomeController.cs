using Disablity_Site_Project.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Disablity_Site_Project.helpingClasses;
using Disablity_Site_Project.Models;

namespace Disablity_Site_Project.Controllers
{
    public class HomeController : Controller
    {
        GeneralPurpose gp = new GeneralPurpose();
        DatabaseEntities db = new DatabaseEntities();
        private string GetRealtiveTime(DateTime dateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;
            var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "one year ago" : years + " years ago";
            }
        }
        public ActionResult ViewJobs(string title="", string msg="",string color="")
        {
            if (gp.validateUser() == null)
            {
                ViewBag.button = "ابدأ حريتك";
            }
            else
            {
                ViewBag.button = "Logout";
                if (gp.validateUser().Role == 1)
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Buyer";
                }
                else
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Seller";
                }
            }
            ViewBag.msg = msg;
            ViewBag.color = color;
            ViewBag.title = title;
            List<JobOffer> jobOffers = new JobOfferBL().GetActiveJobOffersList(db).ToList();
            var model = TempData["list"] as List<JobOffer>;
            if (model != null)
            {
                if (model.Count > 0)
                {
                    jobOffers = model;
                }
            }
            List<JobOfferDTO> jobs = new List<JobOfferDTO>();
            foreach(JobOffer item in jobOffers)
            {
                jobs.Add(new JobOfferDTO()
                {
                    Id=item.Id,
                    Title=item.Title,
                    JobLocation=item.JobLocation,
                    JobType=item.JobType,
                    Description=item.Description,
                    Image=item.Image,
                    CreatedAt=GetRealtiveTime(item.CreatedAt.Value),
                    Budget=item.Budget
                    
                });
            }
            ViewBag.skills = new SkillBL().GetActiveSkillsList(db);
            return View(jobs);
        }
        public ActionResult SearchJobs(string Title="")
        {
            List<JobOffer> list = new JobOfferBL().GetActiveJobOffersList(db).Where(x => x.Title.ToLower().Contains(Title.ToLower())||Title.ToLower().Contains(x.Title.ToLower())).ToList();
            TempData["list"] = list.ToList();
            return RedirectToAction("ViewJobs", new { title = Title });
        }
        public ActionResult TrainingPortal(string msg="",string color="")
        {
            if (gp.validateUser() == null)
            {
                ViewBag.button = "ابدأ حريتك";
            }
            else
            {
                ViewBag.button = "Logout";
                if (gp.validateUser().Role == 1)
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Buyer";
                }
                else
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Seller";
                }
            }
          
            ViewBag.msg = msg;
            ViewBag.color = color;
            //ViewBag.title = gigTitle;
            return View();
        }
        public ActionResult Consultation(string msg = "", string color = "")
        {
            if (gp.validateUser() == null)
            {
                ViewBag.button = "ابدأ حريتك";
            }
            else
            {
                ViewBag.button = "Logout";
                if (gp.validateUser().Role == 1)
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Buyer";
                }
                else
                {
                    ViewBag.profile = new UserBL().GetActiveUserById(gp.validateUser().Id, db).ProfilePicture;
                    ViewBag.dash = "Seller";
                }
            }

            ViewBag.msg = msg;
            ViewBag.color = color;
            //ViewBag.title = gigTitle;
            return View();
        }
    }
}