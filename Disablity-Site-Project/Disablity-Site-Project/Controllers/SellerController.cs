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
    public class SellerController : Controller
    {
        DatabaseEntities db = new DatabaseEntities();
        GeneralPurpose gp = new GeneralPurpose();
        private bool isLogedIn()
        {
            if (gp.validateUser() != null)
            {
                if (gp.validateUser().Role == 2)
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
        #region ManageGigs
        public ActionResult AddGig(string msg = "", string color = "")
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
        [HttpPost]
        public ActionResult PostAddGig(Gig j)
        {
            if ((!isLogedIn()))
            {
                return RedirectToAction("Login", "Auth");
            }
            if (j.Title == "" || j.description == "" || j.Price == "" || j.Image == "" )
            {
                return RedirectToAction("AddGig", new { msg = "Fill all the fieldds first", color = "red" });
            }
            HttpPostedFileBase httpPostedFile = Request.Files["Image"];
            string fileName = System.IO.Path.GetFileName(httpPostedFile.FileName);
            string extName = System.IO.Path.GetExtension(httpPostedFile.FileName);
            if (extName.ToLower().ToLower() == ".jpg"|| extName.ToLower().ToLower() == ".png")
            {
                string filePath = "~/Content/assets/images/" + fileName;
                httpPostedFile.SaveAs(Server.MapPath(filePath));
                j.Image = fileName;
            }
            else
            {
                return RedirectToAction("AddGig", new { msg = "Only pdf files are allowed", color = "red" });
            }
            j.IsActive = 1;
            j.CreatedAt = DateTime.Now;
            j.SellerId = gp.validateUser().Id;
            if (new GigBL().AddGig(j, db))
            {
                return RedirectToAction("AddGig", new { msg = "Your gig has been added to the databse!", color = "green" });
            }
            return RedirectToAction("AddGig", new { msg = "Record has not been added successfully!", color = "red" });
        }
        public ActionResult ViewGigs(string msg = "", string color = "")
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
        public ActionResult GetGigsList(string GigTitle = "")
        {
            List<Gig> jlist = jlist = new GigBL().GetActiveGigsList(db).Where(x => x.SellerId.Value == gp.validateUser().Id).OrderByDescending(x => x.Id).ToList();
            if (GigTitle != "")
            {
                jlist = jlist.Where(x => x.Title.ToLower().Contains(GigTitle.ToLower())).ToList();
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
                        x.description.ToLower().Contains(searchValue.ToLower()) ||
                        x.Price.ToLower().Contains(searchValue.ToLower()) 
                        ).ToList();
            }
            int totalrows = jlist.Count();
            int totalrowsafterfilterinig = jlist.Count();
            jlist = jlist.Skip(start).Take(length).ToList();
            List<GigDTO> jdto = new List<GigDTO>();
            foreach (Gig j in jlist)
            {
                GigDTO Gig = new GigDTO
                {
                    Id = j.Id,
                    EncId = StringCypher.Base64Encode(j.Id.ToString()),
                    Title = j.Title,
                    Description = j.description,
                    Price = j.Price,
                };
                jdto.Add(Gig);
            }
            return Json(new { data = jdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PostUpdateGig(Gig Gig)
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            HttpPostedFileBase httpPostedFile = Request.Files["Image"];
            if (httpPostedFile.FileName != "")
            {
                string fileName = System.IO.Path.GetFileName(httpPostedFile.FileName);
                string extName = System.IO.Path.GetExtension(httpPostedFile.FileName);
                if (extName.ToLower().ToLower() == ".jpg" || extName.ToLower().ToLower() == ".png")
                {
                    string filePath = "../Content/assets/images/" + fileName;
                    httpPostedFile.SaveAs(Server.MapPath(filePath));
                    Gig.Image = fileName;
                    
                }
                else
                {
                    return RedirectToAction("AddGig", new { msg = "Only pdf files are allowed", color = "red" });
                }
            }
            Gig j = new GigBL().GetActiveGigById(Gig.Id, db);
            j.Title = Gig.Title;
            j.description = Gig.description;
            j.Price = Gig.Price;
            if (Gig.Image != null)
            {
                j.Image = Gig.Image;
            }
            bool chkGig = new GigBL().UpdateGig(j, db);
            if (chkGig)
            {
                return RedirectToAction("ViewGigs", new { msg = "Gig updated successfully!", color = "green" });
            }
            else
            {
                return RedirectToAction("ViewGigs", new { msg = "Gig has not been updated successfully!", color = "red" });
            }
        }
        [HttpPost]
        public ActionResult GigById(string id)
        {
            Gig j = new GigBL().GetActiveGigById(int.Parse(id), db);
            Gig obj = new Gig()
            {
                Id = j.Id,
                Title = j.Title,
                description = j.description,
                Image = "../Content/assets/images/"+j.Image,
                Price = j.Price,
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteGig(int id)
        {
            if (!isLogedIn())
            {
                return RedirectToAction("Login", "Auth");
            }
            if (new GigBL().DeleteGig(id, db))
            {

                return RedirectToAction("ViewGigs", new { msg = "Gig deleted successfully", color = "green" });
            }
            else
            {
                return RedirectToAction("ViewGigs", new { msg = "Gig has not been deleted sucessfully", color = "red" });
            }
        }
        ////public ActionResult ViewCandidates(string id)
        ////{
        ////    if (!isLogedIn())
        ////    {
        ////        return RedirectToAction("Login", "Auth");
        ////    }
        ////    ViewBag.GigId = StringCypher.Base64Decode(id);
        ////    ViewBag.title = new GigBL().GetActiveGigById(int.Parse(StringCypher.Base64Decode(id)), db).Title;
        ////    return View();
        ////}
        ////public ActionResult GetCandidatesList(int GigId)
        ////{
        ////    List<Candidate_Gig> candidate_Gigs = new Candidate_GigBL().GetActiveCandidate_GigsList(db).Where(x => x.GigId == GigId).ToList();
        ////    List<Candidate> clist = new List<Candidate>();
        ////    foreach (Candidate_Gig candidate_Gig in candidate_Gigs)
        ////    {
        ////        clist.Add(new CandidateBL().GetActiveCandidateById(candidate_Gig.CandidateId, db));
        ////    }
        ////    //List<Candidate> clist = new CandidateBL().GetActiveCandidatesList(db).OrderByDescending(x => x.Id).ToList();
        ////    //if (GigTitle != "")
        ////    //{
        ////    //    clist = clist.Where(x => x.Gig.Title.ToLower().Contains(GigTitle.ToLower())).ToList();
        ////    //}
        ////    int start = Convert.ToInt32(Request["start"]);
        ////    int length = Convert.ToInt32(Request["length"]);
        ////    string searchValue = Request["search[value]"];
        ////    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        ////    string sortDirection = Request["order[0][dir]"];
        ////    if (sortColumnName != "" && sortColumnName != null)
        ////    {
        ////        if (sortColumnName == "CandidateId")
        ////        {
        ////            if (sortDirection == "asc")
        ////            {
        ////                clist = clist.OrderByDescending(x => x.Id).ToList();
        ////            }
        ////            else
        ////            {
        ////                clist = clist.OrderBy(x => x.Id).ToList();
        ////            }
        ////        }
        ////        else
        ////       if (sortDirection == "asc")
        ////        {
        ////            clist = clist.OrderByDescending(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        ////        }
        ////        else
        ////        {
        ////            clist = clist.OrderBy(x => x.GetType().GetProperty(sortColumnName).GetValue(x)).ToList();
        ////        }
        ////    }
        ////    if (!string.IsNullOrEmpty(searchValue))
        ////    {
        ////        clist = clist.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()) ||
        ////                x.Email.ToLower().Contains(searchValue.ToLower()) ||
        ////                x.Contact.ToLower().Contains(searchValue.ToLower()) ||
        ////                (x.Address.ToLower().Contains(searchValue.ToLower()) || x.Qualification.ToLower().Contains(searchValue.ToLower()) || x.Experience1.ToLower().Contains(searchValue.ToLower()) || x.ExpectedSalary.ToLower().Contains(searchValue.ToLower()))).ToList();
        ////    }
        ////    int totalrows = clist.Count();
        ////    int totalrowsafterfilterinig = clist.Count();
        ////    clist = clist.Skip(start).Take(length).ToList();

        ////    List<CandidateDTO> cdto = new List<CandidateDTO>();
        ////    foreach (Candidate c in clist)
        ////    {
        ////        CandidateDTO obj = new CandidateDTO()
        ////        {
        ////            Id = c.Id,
        ////            EncId = StringCypher.Base64Encode(c.Id.ToString()),
        ////            Name = c.Name,
        ////            Email = c.Email,
        ////            Contact = c.Contact,
        ////            Address = c.Address,
        ////            //Qualification=c.Qualification, 
        ////            //ProfessionalExperience=c.ProfessionalExperience, 
        ////            //ExpectedSalary=c.ExpectedSalary, 
        ////            ////GigTitle=c.Gig.Title 
        ////        };

        ////        cdto.Add(obj);
        ////    }
        ////    return Json(new { data = cdto, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfilterinig }, JsonRequestBehavior.AllowGet);
        ////}

        //public FileResult ViewDocument(string id)
        //{
        //    string path = new GigBL().GetActiveGigById(int.Parse(StringCypher.Base64Decode(id)), db).AttachedFile;
        //    string ReportURL = $"{path}";
        //    byte[] FileBytes = System.IO.File.ReadAllBytes(ReportURL);
        //    return File(FileBytes, "application/pdf");
        //}
        #endregion
    }
}