using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NAPASTUDENT.Models;
using NAPASTUDENT.Models.ViewModels;

namespace NAPASTUDENT.Controllers.Api
{
    public class AccountController : ApiController
    {
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController()
        {
            _context = new ApplicationDbContext();
            var roleStore = new UserStore<ApplicationUser>(_context);
            _userManager = new UserManager<ApplicationUser>(roleStore);
        }
        
    }

  
}
