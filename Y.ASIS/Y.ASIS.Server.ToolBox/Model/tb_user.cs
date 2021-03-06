namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.tb_user")]
    public partial class tb_user
    {
        [Key]
        public long user_id { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(20)]
        public string mobile { get; set; }

        [StringLength(64)]
        public string password { get; set; }

        public DateTime? create_time { get; set; }
    }
}
