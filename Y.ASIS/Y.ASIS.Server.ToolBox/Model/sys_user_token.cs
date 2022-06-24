namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_user_token")]
    public partial class sys_user_token
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long user_id { get; set; }

        [Required]
        [StringLength(100)]
        public string token { get; set; }

        public DateTime? expire_time { get; set; }

        public DateTime? update_time { get; set; }
    }
}
