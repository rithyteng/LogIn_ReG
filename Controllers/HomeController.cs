using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using login_and_reg.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;



namespace login_and_reg.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        // here we can "inject" our context service into the constructor
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("home")]
        public IActionResult Home(){
            return View("Home");
        }
        [HttpGet("LogingIn")]
        public IActionResult Stuff(){
            System.Console.WriteLine("Testing102");
            string testing = HttpContext.Session.GetString("Email");
            if(testing !=null){
                return View("LogIn");
            }
            else{
                return View("Index");
            }
            

        }
        [HttpPost("LogIn")]
        public IActionResult LogIn(Login thisuser){

        if(ModelState.IsValid)
        {
            // If inital ModelState is valid, query for a user with provided email
            var userInDb = dbContext.User.FirstOrDefault(u => u.Email == thisuser.Email);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "Invalid Email/Password");
                return View("Home");
            }
            
            // Initialize hasher object
            var hasher = new PasswordHasher<Login>();
            
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(thisuser, userInDb.Password, thisuser.Password);
            
            // result can be compared to 0 for failure
            if(result == 0)
            {
                ModelState.AddModelError("Password","Invalid Email/Passwordz");
                return View("Home");
                // handle failure (this should be similar to how "existing email" is handled)
            }
            else{
                HttpContext.Session.SetString("Email",thisuser.Email);
                
                return RedirectToAction ("Stuff");
            }
        }
        return View("Home");
        }
        [HttpPost("Register")]
        public IActionResult Register(User alluser){
            if(ModelState.IsValid){

                if(dbContext.User.Any(u => u.Email == alluser.Email))
        {
            // Manually add a ModelState error to the Email field, with provided
            // error message
            ModelState.AddModelError("Email", "Email already in use!");
            return View("Index");
            
            // You may consider returning to the View at this point
        }
         // Initializing a PasswordHasher object, providing our User class as its
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                alluser.Password = Hasher.HashPassword(alluser, alluser.Password);
                //Save your user object to the database
                dbContext.Add(alluser);
                dbContext.SaveChanges();

                return View("Sucess");
                
            }
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
