using System;

namespace MyWebApp.Models
{
    public class DiscussionPost
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string ImagePath { get; set; }
        public DateTime PostedAt { get; set; }
    }
}