using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace DatMovieS.DataObjects
{
    public class Clip:EntityData
    {
        public string videoLocalName { get; set; }
        public string creatorId { get; set; }
        public string storyId { get; set; }
        public int likes { get; set; }
        public string url { get; set; }
        public string column { get; set; }
        public string parentId { get; set; }

    }
}