using System;
using RestSharp;
using RestSharp.Authenticators;
namespace Disablity_Site_Project.helpingClasses
{
    public class MailSender
    {
         
        public static bool SendForgotPasswordEmail(string email, string BaseUrl = "")
        {
            try
            {
                string MailBody = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                            "</div> </nav><center><div><h1 class='text-center'>Password Reset!</h1>" +
                            "<p class='text-center'> Simply click the button showing below to reset your password (Link will expire after date change): </p><br>" +
                            "<button style='background-color: rgb(0,174,239);'>" +
                                "<a href='" + BaseUrl + "Auth/ResetPassword?email=" + StringCypher.Base64Encode(email) + "&time=" + DateTime.Now.Date.ToString() + "' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a>" +
                            "</button>" +
                            "<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p></div></center>" +
                            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
                RestClient client = new RestClient();
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "2b1ce341848c6fc996e43321365755ec-aff8aa95-f4645c49");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "nodlays.co.uk", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "uzair.aslam02@gmail.com");
                request.AddParameter("to", email);
                request.AddParameter("subject", "Testing | Password Reset");
                request.AddParameter("html", MailBody);
                request.Method = Method.POST;
                string response = client.Execute(request).Content.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SendMeetingSchaduledEmail(string email, string BaseUrl = "",string email2="",DateTime? dateTime=null)
        {
            try
            {
                string MailBody = "<html><head></head><body><nav class='navbar navbar-default'><div class='container-fluid'>" +
                            "</div> </nav><center><div><h1 class='text-center'>Meeting Scheduled!</h1>" +
                            "<h3 class='text-center'> Your Meeting has been scheduled with "+email2 +" on "+dateTime.Value.ToString("dd/MM/yyyy")+" </h3><br>" +
                            //"<button style='background-color: rgb(0,174,239);'>" +
                            //    "<a href='" + BaseUrl + "Auth/ResetPassword?email=" + StringCypher.Base64Encode(email) + "&time=" + DateTime.Now.Date.ToString() + "' style='text-decoration:none;font-size:15px;color:white;'>Reset Password</a>" +
                            //"</button>" +
                            //"<p style='color:red;'>Link will not work in spam. Please move this mail into your inbox.</p></div></center>" +
                            "<script src = 'https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js' ></ script ></ body ></ html >";
                RestClient client = new RestClient(); 
                client.BaseUrl = new Uri("https://api.mailgun.net/v3");
                client.Authenticator = new HttpBasicAuthenticator("api", "key-ff43f744db29071a74f3106541e3b925");
                RestRequest request = new RestRequest();
                request.AddParameter("domain", "nodlays.co.uk", ParameterType.UrlSegment);
                request.Resource = "{domain}/messages";
                request.AddParameter("from", "uzair.aslam02@gmail.com");
                request.AddParameter("to", email);
                request.AddParameter("subject", "New meeting");
                request.AddParameter("html", MailBody);
                request.Method = Method.POST;
                string response = client.Execute(request).Content.ToString();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}