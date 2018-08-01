using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using SEPApplication1.Models;

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
    public class GetMemberResult
    {
        public int Code { get; set; }
        public MemberData[] Data { get; set; }
        public string Message { get; set; }
    }

    public class PostMemberResult
    {
        public int Code { get; set; }
        public string message { get; set; }
    }


    public class MemberData
    {
        public string ID { get; set; }
        public string fullname { get; set; }
        public DateTime birthday { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    }
    public class StudentData
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public DateTime Birthday { get; set; }
        
    }
    public class GetStudentResult
    {
        public int Code { get; set; }
        public StudentData Data { get; set; }
        public string Message { get; set; }
    }
    public class SessionsData
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public string Info { get; set; }
    }
    public class GetSessionResult
    {
        public int Code { get; set; }
        public SessionsData[] Data { get; set; }
        public string Message { get; set; }
    }

    public class API
    {
        //string URL = "http://125.234.238.137:8080/CMU/SEPAPI/SEP21/";
        string URL = "http://cntttest.vanlanguni.edu.vn:8080/CMU/SEPAPI/SEP21/";
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
        public GetSessionResult GetSessions(string Course_id)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString("http://cntttest.vanlanguni.edu.vn:8080/CMU/SEPAPI/SEP21/" + "/GetSessions?Course_id=" + Course_id);
                return JsonConvert.DeserializeObject<GetSessionResult>(json);
            }
        }
        //GetMember
        public GetMemberResult GetMember(string courseID)
        {
            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;
                var json = client.DownloadString(URL + "/GetMembers?courseID=" + courseID);
                return JsonConvert.DeserializeObject<GetMemberResult>(json);
            }
        }
        SEPEntities db = new SEPEntities();
        //PostMember
        public PostMemberResult postmember(string uid, string secret, string course)
        {
            using (var client = new WebClient())
            {
                var form = new NameValueCollection();
                form["uid"] = uid;
                form["secret"] = secret;

                var datas = new
                {
                    course = course,
                    members = db.Members.Where(x => x.Course.Code == course).Select(b => b.Code).ToArray()
                };
                var predata = JsonConvert.SerializeObject(datas);
                form["data"] = predata;
                var result = client.UploadValues(URL + "SyncMembers", "POST", form);
                var json = Encoding.UTF8.GetString(result);
                return JsonConvert.DeserializeObject<PostMemberResult>(json);
            }
        }
        public GetStudentResult GetStudent(string code)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString("http://cntttest.vanlanguni.edu.vn:8080/CMU/SEPAPI/SEP21/" + "GetStudent?code=" + code);
                return JsonConvert.DeserializeObject<GetStudentResult>(json);

            }
        }
    }
}