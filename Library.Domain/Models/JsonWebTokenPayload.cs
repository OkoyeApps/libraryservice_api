using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
   public  class JsonWebTokenPayload
    {
        public string Subject { get; set; }
        public long Expires { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}
