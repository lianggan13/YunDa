namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_userface")]
    public partial class sys_userface
    {
        public int id { get; set; }

        [StringLength(50)]
        public string userid { get; set; }

        [StringLength(50)]
        public string face { get; set; }
    }
}
