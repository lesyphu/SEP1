using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SEPApplication1
{
    public class LoginData
    {
        public string Id { get; set; }
        public string Secret { get; set; }
    }
    public class LoginResult
    {
        public int Code { get; set; }
        public LoginData Data { get; set; }
        public string Message { get; set; }
    }
    public class CoursesData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
    }
    public class GetCoursesResult
    {
        public int  Code { get; set; }
        public CoursesData[] Data { get; set; }
        public string Message { get; set; }
    }

    public class API
    {
       
        public LoginResult Login(string  username, string password)
        {
            using (var client = new WebClient())
            {
                var form = new NameValueCollection();
                form["Username"] = username;
                form["Password"] = password;
                var result = client.UploadValues("http://cntttest.vanlanguni.edu.vn:8080/CMU/SEPAPI/SEP21/Login", "POST", form);
                var json = Encoding.UTF8.GetString(result);
                return JsonConvert.DeserializeObject<LoginResult>(json);
            }
        }
        public GetCoursesResult GetCourses (string lecturerID)
        {
             using (var client = new WebClient())
             {
                var json = client.DownloadString("http://cntttest.vanlanguni.edu.vn:8080/CMU/SEPAPI/SEP21/"+ "/GetCourses?lecturerID="+ lecturerID);
                return JsonConvert.DeserializeObject<GetCoursesResult>(json);
             }
        }
    }
}