using System;
using Projecktor.Domain.Entites;

namespace Projecktor.WebUI.Models
{
    public enum ActionEnum
    {
        Like = 0,
        Reblog = 1,
        Followed = 2
    }

    public class ActivityViewModel
    {
        public int ActId { get; set; }
        public User From { get; set; }
        public DateTime Date { get; set; }
        public ActionEnum Action { get; set; }
        public int PostId { get; set; }
        public int SourceId { get; set; }
    }
}