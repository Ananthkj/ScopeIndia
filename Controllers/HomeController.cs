using Microsoft.AspNetCore.Mvc;
using ScopeIndia.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.VisualBasic;




namespace ScopeIndia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        CookieOptions options = new CookieOptions();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var cookieValue = HttpContext.Request.Cookies["cookie"];
            if (cookieValue==null)
            {
                return RedirectToAction("LoginPage");
            };
            return View();
        }

        public IActionResult Courses()
        {
            return View();
        }
        public IActionResult RegistrationForm()
        {
            return View();
        }
        public IActionResult LoginPage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LoginPage(object data)
        {
            string Username = Request.Form["username"];
            string Password = Request.Form["password"];
            //var check = "false";
            string setcookie = Request.Form["setcookie"];
            HttpContext.Session.SetString("email", Username);
            //bool islogin = false;

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string dataQuery = "select * from Students where EMAIL=@Username AND PASSWORD=@Password";
                using (SqlCommand cmd = new SqlCommand(dataQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Username", Username);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //islogin = true;
                            //Cookies
                            this.options.Expires = DateTimeOffset.Now.AddMinutes(1);
                            //HttpContext.Response.Cookies.Delete("authorised");
                            if (setcookie == "true")
                            {
                                HttpContext.Response.Cookies.Append("cookie", "true", options);

                            }

                            using (SqlConnection con2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                            {  
                                con2.Open();
                                string dataQuery2 = "UPDATE Students SET ISLOGIN='1' WHERE EMAIL=@Username";
                                 using (SqlConnection con3 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                                   {
                                     con3.Open();
                                       string dataQuery3 = "select ISLOGIN from Students where  EMAIL=@Username";
                                         using (SqlCommand cmd3 = new SqlCommand(dataQuery3, con3))
                                          {
                                            cmd3.Parameters.AddWithValue ("@Username", Username);
                                            if (reader.HasRows)
                                            {

                                                using (SqlDataReader reader3 = cmd3.ExecuteReader())
                                                {
                                                  if (reader3.Read())
                                                    {
                                                        int value = reader3.GetInt32(reader3.GetOrdinal("ISLOGIN"));
                                                    if (value == 1)
                                                    {
                                                        return RedirectToAction("Dashboard");
                                                    }
                                                    else
                                                    {
                                                        using (SqlCommand cmd2 = new SqlCommand(dataQuery2, con2))
                                                        {
                                                            cmd2.Parameters.AddWithValue("@Username", Username);

                                                            cmd2.ExecuteNonQuery();
                                                            return RedirectToAction("Newpassword");
                                                        }
                                                    }
                                                    }
                                                    else
                                                    {
                                                        return RedirectToAction("Privacy");
                                                    }
                                                }
                                            }else {

                                                return RedirectToAction("Privacy");
                                            }
                                            }

                                                }
                                            }

                                                
                                        }                                      
                        else
                        {
                            ViewBag.Message = "Your Credentials is not correct";
                            return View();
                        }
                    }

                }
            }
            ViewBag.Message = "Your Credentials is not correct";

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("cookie");
            return RedirectToAction("Index");

        }


        public IActionResult FirstTimePage()
        {

            return View();
        }
        public IActionResult Forgetpassword()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Forgetpassword(object data)
        {
            string Email= Request.Form["email"];
            Random random = new Random();
            int randomNum=random.Next(1000, 9999);  

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string query = "SELECT * FROM Students WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Replace "user@example.com" with the email you want to search for
                    command.Parameters.AddWithValue("@Email", Email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MailMessage email = new MailMessage();
                            email.From = new MailAddress("anandkumarj95@gmail.com");
                            email.To.Add(new MailAddress(Email));
                            email.Subject = "Registed Password";
                            email.IsBodyHtml = false;
                            email.Body= $"{randomNum}";
                            System.Net.Mail.SmtpClient smtp= new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                            smtp.Credentials = new NetworkCredential("anandkumarj95@gmail.com", "gety kffr gmzg sqra");
                            smtp.EnableSsl = true;
                            smtp.Send(email);
                            reader.Close();

                            string query1 = "update Students set PASSWORD=@password where EMAIL=@email";
                            using (SqlCommand command1=new SqlCommand(query1,con))
                            {
                                command1.Parameters.AddWithValue("@password",randomNum);
                                command1.Parameters.AddWithValue("@email",Email);
                                command1.ExecuteNonQuery();

                            }
                            ViewBag.Message = "Mail has been send Succesfully";
                            //return RedirectToAction("LoginPage");
                        }
                        else
                        {
                            ViewBag.Message = "Invalid Email";
                            return View();
                        }
                    }
                }              
            }
            return View();

        }

        [HttpPost]
        public IActionResult FirstTimePage(object data)
        {
            string Email = Request.Form["Email"];
            Random random = new Random();

            int randomNumber1 = random.Next(1000, 9999);

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string query = "SELECT * FROM Students WHERE Email = @Email";

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // Replace "user@example.com" with the email you want to search for
                    command.Parameters.AddWithValue("@Email", Email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MailMessage email = new MailMessage();
                            email.From = new MailAddress("anandkumarj95@gmail.com");
                            email.To.Add(new MailAddress(Email));
                            email.Subject = "Register Id OTP";
                            email.IsBodyHtml = false;
                            email.Body = $"{randomNumber1}";
                            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
                            smtpClient.Credentials = new NetworkCredential("anandkumarj95@gmail.com", "ound jhud cnfr nvqt");
                            smtpClient.EnableSsl = true;
                            smtpClient.Send(email);
                            reader.Close();

                            string query1 = "UPDATE STUDENTS SET password=@password where Email = @Email";
                            using (SqlCommand command1 = new SqlCommand(query1, con))
                            {
                                command1.Parameters.AddWithValue("@Email", Email);
                                command1.Parameters.AddWithValue("@password", randomNumber1);
                                command1.ExecuteNonQuery();
                            }
                            ViewBag.Message = "Mail has been send Succesfully";
                            return RedirectToAction("Newpassword");
                        }

                        else
                        {
                            ViewBag.Message = "please register first";
                            return View();
                        }
                    }
                }
            }
        }
        public IActionResult Newpassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Newpassword(object data)
        {
            string DefaultPassword = Request.Form["Defaultpassword"];
            string NewPassword = Request.Form["Newpassword"];
            string ConfirmPassword = Request.Form["Confirmpassword"];
            string email=HttpContext.Session.GetString("email");
            if(NewPassword==ConfirmPassword)
            {
                using (SqlConnection con=new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                {
                    con.Open();
                    string dataQuery = "update Students set PASSWORD=@newpassword where EMAIL=@email";
                    using (SqlCommand cmd=new SqlCommand(dataQuery,con))
                    {
                        cmd.Parameters.AddWithValue("@newpassword",NewPassword);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.ExecuteNonQuery();
                        ViewBag.Message = "Pasword has been Changed";
                    }

                }
            }
            else if(DefaultPassword== "Keep Default Password")
            {
                RedirectToAction("Index");

            }
            else
            {
                ViewBag.Message("Entered Password Doesn't Match");
            }
            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        List<object> data =new List<object>();
        public IActionResult studentstable()
        {
           
            //List<string> list = new List<string>();
            string Email = HttpContext.Session.GetString("email");

            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string queryData = "select * from Students where Email=@Email";
                using (SqlCommand cmd=new SqlCommand(queryData,con))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    //cmd.ExecuteNonQuery();
                    using (SqlDataReader reader=cmd.ExecuteReader())
                    {
                       
                        while (reader.Read())
                        {                          
                            if (!reader.IsDBNull(reader.GetOrdinal("FULLNAME")))
                            {
                                string name = reader.GetString(reader.GetOrdinal("FULLNAME"));
                                string email= reader.GetString(reader.GetOrdinal("EMAIL"));
                                string DOB = reader.GetString(reader.GetOrdinal("DATEOFBIRTH"));
                                string gender = reader.GetString(reader.GetOrdinal("GENDER"));
                                string qualification = reader.GetString(reader.GetOrdinal("QUALIFICATION"));
                                string courses = reader.GetString(reader.GetOrdinal("COURSES"));
                                string mobile = reader.GetString(reader.GetOrdinal("MOBILE"));
                                string guardiancontact = reader.GetString(reader.GetOrdinal("GUARDIANCONTACT"));
                                string classmode = reader.GetString(reader.GetOrdinal("CLASSMODE"));
                                string branch = reader.GetString(reader.GetOrdinal("BRANCH"));
                                string guardianname = reader.GetString(reader.GetOrdinal("GUARDIANNAME"));
                                string guardianoccupation = reader.GetString(reader.GetOrdinal("GUARDIANOCCUPATION"));
                                string classtiming = reader.GetString(reader.GetOrdinal("CLASSTIMING"));
                                string address = reader.GetString(reader.GetOrdinal("ADDRESSES"));
                                string country = reader.GetString(reader.GetOrdinal("COUNTRY"));
                                string state = reader.GetString(reader.GetOrdinal("STATES"));
                                string city = reader.GetString(reader.GetOrdinal("CITY"));
                                string pincode = reader.GetString(reader.GetOrdinal("PINCODE"));
                                string image = reader.GetString(reader.GetOrdinal("images"));
                                ViewBag.Name = name;
                                ViewBag.Email = email;
                                ViewBag.dob = DOB;
                                ViewBag.gender = gender;
                                ViewBag.Qualification= qualification;
                                ViewBag.courses=courses;
                                ViewBag.mobile= mobile;
                                ViewBag.guardiancontact=guardiancontact;
                                ViewBag.classmode= classmode;   
                                ViewBag.branch= branch; 
                                ViewBag.guardianname= guardianname;
                                ViewBag.guardianoccupation=guardianoccupation;
                                ViewBag.classtiming= classtiming;
                                ViewBag.address= address;
                                ViewBag.country= country;
                                ViewBag.state= state;
                                ViewBag.city= city;
                                ViewBag.pincode= pincode;
                                ViewBag.image= image;   
                                data.Add(new {name=name});
                            }
                        }

                    }
                }
                //using (SqlConnection con2 = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
                //{
                //    con2.Open();
                //    string queryData2 = "select IMAGES from Students where Email=@Email";
                //    using (SqlCommand cmd = new SqlCommand(queryData2, con2))
                //    {
                //        cmd.Parameters.AddWithValue("@Email", Email);
                //        cmd.ExecuteNonQuery();
                //        using (SqlDataReader reader = cmd.ExecuteReader())
                //        {
                //            while(reader.Read())
                //            {
                //                if(!reader.IsDBNull(reader.GetOrdinal("IMAGES")))
                //                {
                //                    string images =reader.GetString( reader.GetOrdinal("IMAGES"));
                //                    ViewBag.Image=images;
                //                }
                              
                //            }
                //        }
                            
                //    }

                //}

            }
            return View();
        }

        public IActionResult Editmenu()
        {
            string Email = HttpContext.Session.GetString("email");
            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                con.Open();
                string queryData = "select * from Students where Email=@Email";
                using (SqlCommand cmd = new SqlCommand(queryData, con))
                {
                    cmd.Parameters.AddWithValue("@Email", Email);
                    //cmd.ExecuteNonQuery();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {                          
                            if (!reader.IsDBNull(reader.GetOrdinal("FULLNAME")))
                            {
                                string name = reader.GetString(reader.GetOrdinal("FULLNAME"));
                                string email = reader.GetString(reader.GetOrdinal("EMAIL"));
                                string DOB = reader.GetString(reader.GetOrdinal("DATEOFBIRTH"));
                                string gender = reader.GetString(reader.GetOrdinal("GENDER"));
                                string qualification = reader.GetString(reader.GetOrdinal("QUALIFICATION"));
                                string courses = reader.GetString(reader.GetOrdinal("COURSES"));
                                string mobile = reader.GetString(reader.GetOrdinal("MOBILE"));
                                string guardiancontact = reader.GetString(reader.GetOrdinal("GUARDIANCONTACT"));
                                string classmode = reader.GetString(reader.GetOrdinal("CLASSMODE"));
                                string branch = reader.GetString(reader.GetOrdinal("BRANCH"));
                                string guardianname = reader.GetString(reader.GetOrdinal("GUARDIANNAME"));
                                string guardianoccupation = reader.GetString(reader.GetOrdinal("GUARDIANOCCUPATION"));
                                string classtiming = reader.GetString(reader.GetOrdinal("CLASSTIMING"));
                                string address = reader.GetString(reader.GetOrdinal("ADDRESSES"));
                                string country = reader.GetString(reader.GetOrdinal("COUNTRY"));
                                string state = reader.GetString(reader.GetOrdinal("STATES"));
                                string city = reader.GetString(reader.GetOrdinal("CITY"));
                                string pincode = reader.GetString(reader.GetOrdinal("PINCODE"));
                                ViewBag.Name = name;
                                ViewBag.Email = email;
                                ViewBag.dob = DOB;
                                ViewBag.gender = gender;
                                ViewBag.Qualification = qualification;
                                ViewBag.courses = courses;
                                ViewBag.mobile = mobile;
                                ViewBag.guardiancontact = guardiancontact;
                                ViewBag.classmode = classmode;
                                ViewBag.branch = branch;
                                ViewBag.guardianname = guardianname;
                                ViewBag.guardianoccupation = guardianoccupation;
                                ViewBag.classtiming = classtiming;
                                ViewBag.address = address;
                                ViewBag.country = country;
                                ViewBag.state = state;
                                ViewBag.city = city;
                                ViewBag.pincode = pincode;
                               
                            }
                        }

                    }
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Editmenu(object data)
        {
            

            string name = Request.Form["fname"];
            string dob = Request.Form["dob"];
            string gender = Request.Form["gender"];
            string Qualification = Request.Form["Qualification"];
            string course = Request.Form["course"];
            string mobile = Request.Form["mobile"];
            string email = Request.Form["email"];
            string guardian = Request.Form["guardian"];
            string classmode = Request.Form["classmode"];
            string location = Request.Form["location"]; 
            string guardianname = Request.Form["guardianname"];
            string guardianoccupation = Request.Form["guardianoccupation"];
            string timing = Request.Form["timing"];
            string address = Request.Form["address"];
            string country = Request.Form["country"];
            string state = Request.Form["state"];
            string city = Request.Form["city"];
            string pincode = Request.Form["pincode"];


            using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Scope;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"))
            {
                string Log_Email = HttpContext.Session.GetString("email");
                con.Open();
                string dataQuery = "update Students set FULLNAME=@Name,DATEOFBIRTH=@DOB,GENDER=@Gender,QUALIFICATION=@Qualification,COURSES=@Course,MOBILE=@Mobile,EMAIL=@email,GUARDIANCONTACT=@Guardian,CLASSMODE=@Classmode,BRANCH=@Location,GUARDIANNAME=@Guardianname,CLASSTIMING=@Timing,ADDRESSES=@Address,COUNTRY=@Country,STATES=@State,CITY=@City,PINCODE=@Pincode WHERE EMAIL=@Log_Email";
                using (SqlCommand cmd=new SqlCommand(dataQuery,con))
                {
                    cmd.Parameters.AddWithValue("@Log_Email", Log_Email);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@DOB", dob);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Qualification", Qualification);
                    cmd.Parameters.AddWithValue("@Course", course);
                    cmd.Parameters.AddWithValue("@Mobile", mobile);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@Guardian", guardian);
                    cmd.Parameters.AddWithValue("@Classmode", classmode);
                    cmd.Parameters.AddWithValue("@Location", location);
                    cmd.Parameters.AddWithValue("@Guardianname", guardianname);
                    cmd.Parameters.AddWithValue("@Timing", timing);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Country", country);
                    cmd.Parameters.AddWithValue("@State", state);
                    cmd.Parameters.AddWithValue("@City", city);
                    cmd.Parameters.AddWithValue("@Pincode", pincode);
                    cmd.ExecuteNonQuery();

                }


                  
            }

            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
