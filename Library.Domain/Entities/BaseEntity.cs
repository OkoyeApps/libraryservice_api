using Library.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Text;


namespace Library.Domain.Entities
{
    public class BaseEntity
    {
        protected DateTime now = DateTime.UtcNow;
        public BaseEntity()
        {
            this.CreatedAt = now.ToTimestamp();
        }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
