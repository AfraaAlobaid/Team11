using Disablity_Site_Project.BL;
using Disablity_Site_Project.helpingClasses;
using Disablity_Site_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Disablity_Site_Project.Controllers
{
    public class BuyerController : Controller
    {
        DatabaseEntities db = new DatabaseEntities();
        GeneralPurpose gp = new GeneralPurpose();
        private bool isLogedIn()
        {
            if (gp.validateUser() != null)
            {
                if (gp.validateUser().Role == 1)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        public ActionResult Dashboard()
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            return View();
        }
        #region ManageJobOffers
        public ActionResult AddJobOffer(string msg = "", string color = "")
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                ViewBag.msg = msg;
                ViewBag.color = color;
                return View();
            }
        }
        //public ActionResult Viewcandidates(string email = "", string time = "", string msg = "", string color = "black")
        //{
        //    var passDate = DateTime.ParseExact(time, "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        //    var currDate = DateTime.ParseExact(DateTime.Now.Date.ToString(), "M/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
        //    if (passDate != currDate)
        //    {
        //        return RedirectToAction("Login", "Auth", new { msg = "Link expired, Please try again!", color = "red" });
        //    }
        //    ViewBag.email =StringCypher.Base64Decode( email);
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult GetCandidatesList(string email = "")
        //{ 
        //    List<Candidate> clist = new CandidateBL().GetActiveCandidatesList(db);
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        //    string sortDirection = Request["order[0][dir]"];
        //    if (sortColumnName != "" && sortColumnName != null)
        //    {
        //    if (sortDirection == "asc")
        //        {
        //            clist = clist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //        }
        //        else
        //        {
        //            clist = clist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        clist = clist.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
        //                x.Email.ToLower().Contains(searchValue.ToLower()) ||
        //                x.Contact.ToLower().Contains(searchValue.ToLower()) ||
        //                (x.Address.ToLower().Contains(searchValue.ToLower()) || x.Qualification.ToLower().Contains(searchValue.ToLower()) || x.ProfessionalExperience.ToLower().Contains(searchValue.ToLower()) || x.ExpectedSalary.ToLower().Contains(searchValue.ToLower()) )).ToList();
        //    }
        //    int totalrows = clist.Count();
        //    int totalrowsafterfilterinig = clist.Count();
        //    clist = clist.Skip(start).Take(length).ToList();
        //    List<CandidateDTO> cdto = new List<CandidateDTO>();
        //    foreach (Candidate c in clist)
        //    {
        //        CandidateDTO obj = new CandidateDTO()
        //        {
        //            Id = c.Id,
        //            Name = c.Name,
        //            Email = c.Email,
        //            Contact = c.Contact,
        //            Address = c.Address,
        //            Qualification = c.Qualification,
        //            ProfessionalExperience = c.ProfessionalExperience,
        //            ExpectedSalary = c.ExpectedSalary
        //        };

        //        cdto.Add(obj);
        //    }
        //    return Json(new { data = cdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        //}
      
        [HttpPost]
        public ActionResult PostAddJobOffer(JobOffer j)
        {
            if ((!isLogedIn()))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (Request.Files["Image"] == null || Request.Files["AttachedFile"] == null || j.Budget == "" || j.Title == "" || j.Description == "" /*|| j.Budget == ""*/ || j.RequiredExp == "" || j.AttachedFile == "" || j.JobLocation == "" || j.JobType == "")
            {
                return RedirectToAction("AddJobOffer", new { msg = "Fill all the fieldds first", color = "red" });
            }
            HttpPostedFileBase httpPostedFile = Request.Files["AttachedFile"];
            string fileName = System.IO.Path.GetFileName(httpPostedFile.FileName);
            string extName = System.IO.Path.GetExtension(httpPostedFile.FileName);
            if (extName.ToLower().ToLower() == ".pdf")
            {
                string filePath = "../Content/assets/Files/" + fileName;
                httpPostedFile.SaveAs(Server.MapPath(filePath));
                j.AttachedFile = filePath;
            }
            else
            {
                return RedirectToAction("AddJobOffer", new { msg = "Only pdf files are allowed", color = "red" });
            }
            HttpPostedFileBase image = Request.Files["Image"];
            string imageName = System.IO.Path.GetFileName(image.FileName);
            string imageExtension = System.IO.Path.GetExtension(image.FileName);
            if (imageExtension.ToLower().ToLower() == ".jpg" || imageExtension.ToLower().ToLower() == ".png")
            {
                string filePath = "~/Content/assets/images/" + imageName;
                image.SaveAs(Server.MapPath(filePath));
                j.Image = imageName;
            }
            else
            {
                return RedirectToAction("AddGig", new { msg = "Only .jpg & .png  files are allowed", color = "red" });
            }
            j.IsActive = 1;
            j.CreatedAt = DateTime.Now;
            j.BuyerId = gp.validateUser().Id;
            if (new JobOfferBL().AddJobOffer(j, db))
            {
                List<String> skillsRequired = Request.Form["RequiredSkills"].Split(',').ToList();
                foreach (string item in skillsRequired)
                {
                    new SkillBL().AddSkill(new Skill()
                    {
                        Name = item,
                        JobId = j.Id,
                        IsActive = 1,
                        CreatedAt= DateTime.Now
                    }, db) ;
                }
                return RedirectToAction("AddJobOffer", new { msg = "Record Added successfully", color = "green" });
            }
            return RedirectToAction("AddJobOffer", new { msg = "Record connot be added to database", color = "red" });
        }
        public ActionResult ViewJobOffers(string msg = "", string color = "")
        {
            if (!isLogedIn())
            {
                if (gp.validateUser() != null)
                {
                    return RedirectToAction("Login", "Auth");
                }
            }
            ViewBag.msg = msg;
            ViewBag.color = color;
            return View();
        }
        [HttpPost]
        public ActionResult GetJobOffersList(string JobOfferTitle = "")
        {
            List<JobOffer> jlist = jlist = new JobOfferBL().GetActiveJobOffersList(db).Where(x => x.BuyerId.Value == gp.validateUser().Id).OrderByDescending(x => x.Id).ToList();
            if (JobOfferTitle != "")
            {
                jlist = jlist.Where(x => x.Title.ToLower().Contains(JobOfferTitle.ToLower())).ToList();
            }
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];
            if (sortColumnName != "" && sortColumnName != null)
            {
                if (sortDirection == "asc")
                {
                    jlist = jlist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                }
                else
                {
                    jlist = jlist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
                }
            }
            if (!string.IsNullOrEmpty(searchValue))
            {
                jlist = jlist.Where(x => x.Title.ToLower().Contains(searchValue.ToLower()) ||
                        x.Description.ToLower().Contains(searchValue.ToLower()) ||
                        x.RequiredExp.ToLower().Contains(searchValue.ToLower()) ||
                        x.Budget.ToLower().Contains(searchValue.ToLower())).ToList();
            }
            int totalrows = jlist.Count();
            int totalrowsafterfilterinig = jlist.Count();
            jlist = jlist.Skip(start).Take(length).ToList();
            List<JobOfferDTO> jdto = new List<JobOfferDTO>();
            foreach (JobOffer j in jlist)
            {
                JobOfferDTO jobOffer = new JobOfferDTO
                {
                    Id=j.Id,
                    EncId=StringCypher.Base64Encode(j.Id.ToString()),
                    Title = j.Title,
                    Description = j.Description,
                    Budget = j.Budget,
                    RequiredExp = j.RequiredExp,
                    JobType=j.JobType
                };
                jdto.Add(jobOffer);
            }
            return Json(new { data = jdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostUpdateJobOffer(JobOffer JobOffer)
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            JobOffer j = new JobOfferBL().GetActiveJobOfferById(JobOffer.Id, db);
            j.Title = JobOffer.Title;
            j.Description = JobOffer.Description;
            j.RequiredExp = JobOffer.RequiredExp;
            j.Budget = JobOffer.Budget;
            bool chkJobOffer = new JobOfferBL().UpdateJobOffer(j, db);
            if (chkJobOffer)
            {
                return RedirectToAction("ViewJobOffers", new { msg = "JobOffer updated successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("ViewJobOffers", new { msg = "Somethings' wrong", color = "red" });
            }
        }
        [HttpPost]
        public ActionResult JobOfferById(string id)
        {
            JobOffer j = new JobOfferBL().GetActiveJobOfferById(int.Parse(id), db);
            JobOffer obj = new JobOffer()
            {
                Id = j.Id,
                Title = j.Title,
                Description = j.Description,
                RequiredExp = j.RequiredExp,
                Budget = j.Budget,
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteJobOffer(int id)
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (new JobOfferBL().DeleteJobOffer(id, db))
            {
                
                return RedirectToAction("ViewJobOffers", new { msg = "Record deleted successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("ViewJobOffers", new { msg = "Somethings' wrong", color = "red" });
            }
        }
        //public ActionResult ViewCandidates(string id)
        //{
        //    if (!isLogedIn())
        //    {
        //        return RedirectToAction("Login", "Auth");
        //    }
        //    ViewBag.JobOfferId = StringCypher.Base64Decode(id);
        //    ViewBag.title = new JobOfferBL().GetActiveJobOfferById(int.Parse(StringCypher.Base64Decode(id)), db).Title;
        //    return View();
        //}
        //public ActionResult GetCandidatesList(int JobOfferId)
        //{
        //    List<Candidate_JobOffer> candidate_JobOffers = new Candidate_JobOfferBL().GetActiveCandidate_JobOffersList(db).Where(x => x.JobOfferId == JobOfferId).ToList();
        //    List<Candidate> clist = new List<Candidate>();
        //    foreach (Candidate_JobOffer candidate_JobOffer in candidate_JobOffers)
        //    {
        //        clist.Add(new CandidateBL().GetActiveCandidateById(candidate_JobOffer.CandidateId, db));
        //    }
        //    //List<Candidate> clist = new CandidateBL().GetActiveCandidatesList(db).OrderByDescending(x => x.Id).ToList();
        //    //if (JobOfferTitle != "")
        //    //{
        //    //    clist = clist.Where(x => x.JobOffer.Title.ToLower().Contains(JobOfferTitle.ToLower())).ToList();
        //    //}
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        //    string sortDirection = Request["order[0][dir]"];
        //    if (sortColumnName != "" && sortColumnName != null)
        //    {
        //        if (sortColumnName == "CandidateId")
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                clist = clist.OrderByDescending(x => x.Id).ToList();
        //            }
        //            else
        //            {
        //                clist = clist.OrderBy(x => x.Id).ToList();
        //            }
        //        }
        //        else
        //       if (sortDirection == "asc")
        //        {
        //            clist = clist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //        }
        //        else
        //        {
        //            clist = clist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        clist = clist.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
        //                x.Email.ToLower().Contains(searchValue.ToLower()) ||
        //                x.Contact.ToLower().Contains(searchValue.ToLower()) ||
        //                (x.Address.ToLower().Contains(searchValue.ToLower()) || x.Qualification.ToLower().Contains(searchValue.ToLower()) || x.Experience1.ToLower().Contains(searchValue.ToLower()) || x.ExpectedSalary.ToLower().Contains(searchValue.ToLower()))).ToList();
        //    }
        //    int totalrows = clist.Count();
        //    int totalrowsafterfilterinig = clist.Count();
        //    clist = clist.Skip(start).Take(length).ToList();

        //    List<CandidateDTO> cdto = new List<CandidateDTO>();
        //    foreach (Candidate c in clist)
        //    {
        //        CandidateDTO obj = new CandidateDTO()
        //        {
        //            Id = c.Id,
        //            EncId = StringCypher.Base64Encode(c.Id.ToString()),
        //            Name = c.Name,
        //            Email = c.Email,
        //            Contact = c.Contact,
        //            Address = c.Address,
        //            //Qualification=c.Qualification, 
        //            //ProfessionalExperience=c.ProfessionalExperience, 
        //            //ExpectedSalary=c.ExpectedSalary, 
        //            ////JobOfferTitle=c.JobOffer.Title 
        //        };

        //        cdto.Add(obj);
        //    }
        //    return Json(new { data = cdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult GetFileView(string _Id)
        {
            JobOffer file = new JobOfferBL().GetActiveJobOfferById(int.Parse( _Id),db);
            JobOffer obj = new JobOffer
            {
                AttachedFile = file.AttachedFile
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public FileResult ViewDocument(string id)
        {
            string path=  new JobOfferBL().GetActiveJobOfferById(int.Parse(StringCypher.Base64Decode(id)), db).AttachedFile;
            string ReportURL = $"{path}";
            byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
            return File(FileBytes, "application/pdf");
        }
        #endregion
        public ActionResult ScheduleMeeting(DateTime dateTime,string title="",int? sellerid=null)
        {
            if (dateTime == null)
            {
                return RedirectToAction("SearchGigs", new {gigTitle=title, msg = "Please select a valid date first!", color = "red" });

            }
            if (gp.validateUser()!=null)
            {
                if (gp.validateUser().Role == 1)
                {
                    string BaseUrl = string.Format("{0}://{1}{2}", HttpContext.Request.Url.Scheme, HttpContext.Request.Url.Authority, "/");
                    MailSender.SendMeetingSchaduledEmail(gp.validateUser().Email, BaseUrl, new UserBL().GetActiveUserById(sellerid.Value, db).Email, dateTime);
                    MailSender.SendMeetingSchaduledEmail(new UserBL().GetActiveUserById(sellerid.Value, db).Email, BaseUrl, gp.validateUser().Email, dateTime);
                    return RedirectToAction("SearchGigs", new { gigTitle = title, msg = $"Meeting scheduled  Successfuly! An email is sent to the Seller {new UserBL().GetActiveUserById(sellerid.Value, db).Email}", color = "green" });

                }
                else
                {
                    return RedirectToAction("SearchGigs", new { gigTitle = title, msg = "You have to login as Buyer first , to schadule Meeting!", color = "red" });

                }
            }
            else
            {
                return RedirectToAction("SearchGigs", new { gigTitle = title, msg = "You have to login as Buyer first , to schadule Meeting!", color = "red" });
            }
            return View();
        }
        public ActionResult SearchGigs(string gigTitle="",string msg="",string color="")
        {
            if (gp.validateUser() == null)
            {
                ViewBag.button = "Login";
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
            if (gigTitle == "")
            {
                return View(new List<Gig>());
            }
            ViewBag.msg = msg;
            ViewBag.color = color;
            ViewBag.title = gigTitle;
            List<Gig> gigList= new GigBL().GetActiveGigsList(db).Where(x =>gigTitle.ToLower().Contains( x.Title.ToLower())||x.Title.ToLower().Contains(gigTitle.ToLower())).ToList();
            return View(gigList);
        }
    }
}