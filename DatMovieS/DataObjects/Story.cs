using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatMovieS.DataObjects
{
    public class Story:EntityData
    {
        public string creatorId { get; set; } 
        public int likes { get; set; }
        public string firstClipId { get; set; }
        public string description { get; set; }
    }
}