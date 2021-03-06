namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_role")]
    public partial class sys_role
    {
        [Key]
        public long role_id { get; set; }

        [StringLength(100)]
        public string role_name { get; set; }

        [StringLength(100)]
        public string remark { get; set; }

        public long? create_user_id { get; set; }

        public DateTime? create_time { get; set; }

        [Required]
        [StringLength(10)]
        public string type { get; set; }
    }
}
