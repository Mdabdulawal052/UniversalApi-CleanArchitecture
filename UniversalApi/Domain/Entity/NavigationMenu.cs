using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class NavigationMenu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("ParentNavigationMenu")]
        public int? ParentMenuId { get; set; }

        //public virtual NavigationMenu ParentNavigationMenu { get; set; }

        public string? ControllerName { get; set; }

        public string? ActionUrl { get; set; }

        public string Url { get; set; }
        public string NavIcon { get; set; }

        public int DisplayOrder { get; set; }

        [NotMapped]
        public bool Permitted { get; set; }

        public bool Visible { get; set; }
        public int? ModuleId { get; set; }

    }
}
