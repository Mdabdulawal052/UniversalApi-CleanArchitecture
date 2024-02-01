using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }

        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}
