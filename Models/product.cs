namespace CNWEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("product")]
    public partial class product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public product()
        {
            bill_detail = new HashSet<bill_detail>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string name { get; set; }

        public int quantity { get; set; }

        public double price { get; set; }

        public double width { get; set; }

        public double height { get; set; }

        [StringLength(500)]
        public string detail { get; set; }

        [Required]
        [StringLength(200)]
        public string image_main { get; set; }

        [StringLength(200)]
        public string image1 { get; set; }

        [StringLength(200)]
        public string image2 { get; set; }

        [StringLength(200)]
        public string image3 { get; set; }

        public int id_category { get; set; }

        public int id_manufacturer { get; set; }

        public int id_unit { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bill_detail> bill_detail { get; set; }

        public virtual category category { get; set; }

        public virtual manufacturer manufacturer { get; set; }

        public virtual unit unit { get; set; }
    }
}
