namespace CNWEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("employee")]
    public partial class employee
    {
        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string name { get; set; }

        [Required]
        [StringLength(10)]
        public string gender { get; set; }

        public int phone { get; set; }

        [Required]
        [StringLength(200)]
        public string address { get; set; }

        public int id_account { get; set; }

        public virtual account account { get; set; }
    }
}
