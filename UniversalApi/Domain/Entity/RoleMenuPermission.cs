using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class RoleMenuPermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("NavigationMenuId")]
        public int NavigationMenuId { get; set; }

        public NavigationMenu NavigationMenu { get; set; }
    }
}
