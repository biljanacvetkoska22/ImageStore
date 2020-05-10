using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImageStore.Data;
using ImageStore.Services;
using ImageStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImageStore.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IImageRepository _repository;
        
        public AppController(IMailService mailService, IImageRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }
        public IActionResult Index()
        {
            var results = _repository.GetAllProducts();
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
           // throw new InvalidOperationException("whoa");

            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if(ModelState.IsValid)
            {
                //send email
                _mailService.SendMessage("Biljana@mandex.com", model.Subject, $"From: {model.Name} - {model.Email}  Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent.";
                ModelState.Clear();
            }
          
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }

       
        public IActionResult Shop()
        {
            

            return View();
        }
    }
}