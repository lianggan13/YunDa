namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_user")]
    public partial class sys_user
    {
        [Key]
        public long user_id { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        [StringLength(255)]
        public string nickname { get; set; }

        [StringLength(100)]
        public string password { get; set; }

        [StringLength(50)]
        public string remote_id { get; set; }

        [StringLength(20)]
        public string salt { get; set; }

        [StringLength(50)]
        public string idcard { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(100)]
        public string mobile { get; set; }

        public sbyte? status { get; set; }

        public long? create_user_id { get; set; }

        public DateTime? create_time { get; set; }

        [StringLength(50)]
        public string staff_code { get; set; }
    }
}
