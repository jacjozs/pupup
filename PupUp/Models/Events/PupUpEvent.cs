using PupUp.Models.Identity;
using System;

namespace PupUp.Models.Events
{
    public class PupUpEvent
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public PupUpUser User { get; set; }
        public DateTime Time { get; set; }
        public string Desc { get; set; }
    }
}
