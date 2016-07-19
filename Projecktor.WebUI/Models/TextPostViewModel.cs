using System;
using Projecktor.Domain.Entites;


namespace Projecktor.WebUI.Models
{
    public class TextPostViewModel
    {
        public DateTime TimePosted { get; set; }
        public TextPost TextPost { get; set; } 
    }
}