using System;
using Microsoft.AspNet.Identity.EntityFramework;


namespace WebApp.Models
{
    public class WebAppUser : IdentityUser
    {
        public DateTime FirstTrip { get; set; }
    }
}