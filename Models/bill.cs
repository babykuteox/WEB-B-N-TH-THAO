namespace CNWEB.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("bill")]
    public partial class bill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public bill()
        {
            bill_detail = new HashSet<bill_detail>();
        }

        public int id { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_create { get; set; }

        public double total_price { get; set; }

        [Required]
        [StringLength(500)]
        public string address { get; set; }

        [StringLength(500)]
        public string note { get; set; }

        public int state { get; set; }

        public int id_customer { get; set; }

        public virtual customer customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bill_detail> bill_detail { get; set; }
    }
}
