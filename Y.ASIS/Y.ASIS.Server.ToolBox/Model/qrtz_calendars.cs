namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.qrtz_calendars")]
    public partial class qrtz_calendars
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(120)]
        public string SCHED_NAME { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string CALENDAR_NAME { get; set; }

        [Column(TypeName = "blob")]
        [Required]
        public byte[] CALENDAR { get; set; }
    }
}
