namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_user_role")]
    public partial class sys_user_role
    {
        public long id { get; set; }

        public long? user_id { get; set; }

        public long? role_id { get; set; }
    }
}
