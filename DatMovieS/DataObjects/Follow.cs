using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DatMovieS.DataObjects
{
    public class Follow:EntityData
    {
        public string userId { get; set; }
        public string followerId { get; set; }
    }
}