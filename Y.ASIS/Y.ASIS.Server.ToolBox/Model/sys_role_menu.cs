namespace Y.ASIS.Server.ToolBox.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cdyunda-toolsdb.sys_role_menu")]
    public partial class sys_role_menu
    {
        public long id { get; set; }

        public long? role_id { get; set; }

        public long? menu_id { get; set; }
    }
}
