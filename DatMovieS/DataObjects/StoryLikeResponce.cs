using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DatMovieS.DataObjects
{
    public class LikeResponce
    {
        public bool doIlike { get; set; }
        public int totalLikes { get; set; }
    }
}