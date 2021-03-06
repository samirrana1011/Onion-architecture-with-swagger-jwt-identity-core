using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OA_DataAccess
{
   public class Note : BaseEntity
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Tag { get; set; }

        [Display(AutoGenerateField = false)]
        public string UserID { get; set; }

       
    }
}
