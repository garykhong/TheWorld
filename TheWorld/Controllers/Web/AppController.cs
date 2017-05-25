using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        public MailService MailService { get; private set; }
        public IConfigurationRoot Config { get; private set; }
        public IWorldRepository Repository { get; private set; }
        public ILogger<AppController> Logger { get; private set; }

        public AppController(MailService mailService, IConfigurationRoot config, 
            IWorldRepository repository,
            ILogger<AppController> logger)
        {
            this.MailService = mailService;
            this.Config = config;
            this.Repository = repository;
            this.Logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            try
            {
                var data = this.Repository.GetAllTrips();
                return View(data);
            }
            catch(Exception ex)
            {
                Logger.LogError($"Failed to get trips in Index page: {ex.Message}");
                return Redirect("/error");
            }            
        }

        public IActionResult Contact()
        {            
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if(model.Email.Contains("aol.com"))
            {
                ModelState.AddModelError("", "We don't support AOL addresses");
            }
            if(ModelState.IsValid)
            {
                MailService.SendMail(Config["MailSettings:ToAddress"], model.Email, "from the world", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Message Sent";
            }
            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
