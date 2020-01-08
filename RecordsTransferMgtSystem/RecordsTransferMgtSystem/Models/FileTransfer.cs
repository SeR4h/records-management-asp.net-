using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecordsTransferMgtSystem.Models
{
    public class FileTransfer
    {
        [Required]
        [Display(Name="Project")]
        public string projectName { get; set; }
            [Display(Name = "Directorate")]
        public string directorate { get; set; }
            [Display(Name = "Department")]
        public string department { get; set; }
            [Display(Name = "Station")]
        public string station { get; set; }
        [Display(Name = "Transfer Date")]
        public DateTime dateOfTransfer { get; set; }
        [Display(Name ="Box Number")]
        public string boxNumber { get; set; }
            [Display(Name = "Project")]
        public int projectID { get; set; }
            public string status { get; set; }
            
      
    }
}