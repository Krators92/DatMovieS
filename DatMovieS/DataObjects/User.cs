using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Runtime.Serialization;

namespace DatMovieS.DataObjects
{
    
    public class User: EntityData
    {
          
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public string Alias { get; set; }
        public string isFacebookUser { get; set; }
        public string AvatarImage { get; set; }
        

    }
}