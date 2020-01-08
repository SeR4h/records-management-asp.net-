using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace RecordsTransferMgtSystem.Models
{
    public class projectFile
    {


        [JsonProperty("projectID")]
        public int projectID { get; set; }
         [Display(Name = "File Name")]
         [JsonProperty("fileTitle")]
        public string fileTitle { get; set; }
         [Display(Name = "Company")]
         [JsonProperty("company")]
        public string company { get; set; }
         [Display(Name = "SrNo")]
         [JsonProperty("SrNo")]
        public string SrNo { get; set; }
         [Display(Name = "RefNo")]
         [JsonProperty("ReferenceNo")]
        public string ReferenceNo{ get; set; }
         [Display(Name = "Copies Needed")]
         [JsonProperty("numberOfCopies")]
        public int numberOfCopies { get; set; }
        
    }
}