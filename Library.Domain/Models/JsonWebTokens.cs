using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public long Expires { get; set; }
        public string Id { get; set; }
        public dynamic[] Claims { get; set; }
    }
}
