namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_captcha")]
    public partial class sys_captcha
    {
        [Key]
        public Guid uuid { get; set; }

        [Required]
        [StringLength(6)]
        public string code { get; set; }

        public DateTime? expire_time { get; set; }
    }
}
