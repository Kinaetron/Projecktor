using System;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Models
{
    public class ActivityViewModel
    {
        public User From { get; set; }
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public int PostId { get; set; }
    }
}