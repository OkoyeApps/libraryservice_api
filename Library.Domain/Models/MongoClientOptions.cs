using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Models
{
    public class MongoClientOptions
    {
        public string ConnectionString { get; set; }
        public string DataBaseName { get; set; }
    }
}
