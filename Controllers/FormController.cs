using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Web;
using System.Net;
using System.Net.Mail;


namespace ScopeIndia.Controllers
{
    public class FormController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RegisterForm()
        {
            return View();
        }
        [HttpPost]
        public IActionResult RegisterForm(IFormFile myFile)
        {

            try { 
            string file_name=myFile.FileName;
            file_name = Path.GetFileName(file_name);
            string Upload_folder = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot//Uploads");
            if(!Directory.Exists(Upload_folder))
            {
                Directory.CreateDirectory(Upload_folder);
            }
            string upload_path=Path.Combine(Upload_folder,file_name);
            if(System.IO.File.Exists(upload_path))
            {
                ViewBag.Uploadstatus += file_name + "Already Exists";
                Random file_number=new Random();
                file_name=file_number.Next().ToString() + file_name;
                upload_path = Path.Combine(Upload_folder, file_name);

            }
            else
            {
                ViewBag.Uploadstatus += file_name + "Uploaded Successfully\n";
                var Uploadstream=  new FileStream(upload_path,FileMode.Create);
                myFile.CopyTo(Uploadstream);
            }
            //string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    file.CopyTo(stream);
            //}
            string Fullname = Request.Form["Name"];
            string Dateofbirth = Request.Form["Dateofbirth"];
            string Gender = Request.Form["Gender"];
            string Qualification = Request.Form["Qualification"];
            string Courses = Request.Form["Courses"];
            string Mobile = Request.Form["Mobile"];
            string Email = Request.Form["Email"];
            string Guardian = Request.Form["Guardian"];
            string Time = Request.Form["Time"];
            string Branch = Request.Form["Branch"];
            string GuardianName = Request.Form["GuardianName"];
            string Occupation = Request.Form["Occupation"];
            string Classtime = Request.Form["ClassTime"];
            string Addresses = Request.Form["Addresses"];
            string Country = Request.Form["Country"];
            string States = Request.Form["States"];
            string City = Request.Form["City"];
            string Pincode = Request.Form["Pincode"];

            using (SqlConnection con=new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
               // string dateQuery = "Insert into Students(Name,Gender,Contact,Email)values(@Name,@Gender,@Contact,@Email)";
                string dataQuery = "Insert into Students(FULLNAME,DATEOFBIRTH,GENDER,QUALIFICATION,COURSES,MOBILE,EMAIL,GUARDIANCONTACT,CLASSMODE,BRANCH,GUARDIANNAME,GUARDIANOCCUPATION,CLASSTIMING,ADDRESSES,COUNTRY,STATES,CITY,PINCODE,IMAGES)VALUES(@Fullname,@Dateofbirth,@Gender,@Qualification,@Courses,@Mobile,@Email,@Guardian,@Time,@Branch,@GuardianName,@Occupation,@Classtime,@Addresses,@Country,@States,@City,@Pincode,@Images)";
                using (SqlCommand cmd=new SqlCommand(dataQuery,con))
                {
                    cmd.Parameters.AddWithValue("@Fullname", Fullname);
                    cmd.Parameters.AddWithValue("@Dateofbirth", Dateofbirth);
                    cmd.Parameters.AddWithValue("@Gender", Gender);
                    cmd.Parameters.AddWithValue("@Qualification", Qualification);
                    cmd.Parameters.AddWithValue("@Courses", Courses);
                    cmd.Parameters.AddWithValue("@Mobile", Mobile);
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Guardian", Guardian);
                    cmd.Parameters.AddWithValue("@Time", Time);
                    cmd.Parameters.AddWithValue("@Branch", Branch);
                    cmd.Parameters.AddWithValue("@GuardianName", GuardianName);
                    cmd.Parameters.AddWithValue("@Occupation", Occupation);
                    cmd.Parameters.AddWithValue("@Classtime", Classtime);
                    cmd.Parameters.AddWithValue("@Addresses", Addresses);
                    cmd.Parameters.AddWithValue("@Country", Country);
                    cmd.Parameters.AddWithValue("@States", States);
                    cmd.Parameters.AddWithValue("@City", City);
                    cmd.Parameters.AddWithValue("@Pincode", Pincode);
                    cmd.Parameters.AddWithValue("@Images", "/Uploads/" + file_name);

                    cmd.ExecuteNonQuery();
                }

               }
            }
            catch (Exception ex)
            {
                ViewBag.UploadStatus += $"Unable to upload files {ex.Message} ";
            }
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(object data)
        {
            string Name = Request.Form["name"];
            string Email = Request.Form["email"];
            string Contact = Request.Form["contact"];
            string Subject = Request.Form["Subject"];
            string Message = Request.Form["message"];

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("anandkumarj95@gmail.com");
            mail.To.Add(new MailAddress("anandkumarj95@gmail.com"));
            mail.Subject = "Student Enquiry Details";
            mail.IsBodyHtml = true;
            mail.Body = $"<h1>{Name}</h1>\n<h2>{Email}</h2>\n<h3>{Contact}</h3>\n<h4>{Subject}</h4>\n<p>{Message}</p>";
            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("anandkumarj95@gmail.com", "ioas cleh rmpu zmbw");
            smtpClient.EnableSsl = true;
            smtpClient.Send(mail);
            return View();
        }

    }
}
