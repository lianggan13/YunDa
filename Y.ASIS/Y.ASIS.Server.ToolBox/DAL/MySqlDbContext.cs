using System.Data.Entity;
using Y.ASIS.Server.ToolBox.Model;

namespace Y.ASIS.Server.ToolBox.DAL
{
    public partial class MySqlDbContext : DbContext
    {
        public MySqlDbContext()
            : base("name=MySqlDbContext")
        {
        }

        public virtual DbSet<qrtz_blob_triggers> qrtz_blob_triggers { get; set; }
        public virtual DbSet<qrtz_calendars> qrtz_calendars { get; set; }
        public virtual DbSet<qrtz_cron_triggers> qrtz_cron_triggers { get; set; }
        public virtual DbSet<qrtz_fired_triggers> qrtz_fired_triggers { get; set; }
        public virtual DbSet<qrtz_job_details> qrtz_job_details { get; set; }
        public virtual DbSet<qrtz_locks> qrtz_locks { get; set; }
        public virtual DbSet<qrtz_paused_trigger_grps> qrtz_paused_trigger_grps { get; set; }
        public virtual DbSet<qrtz_scheduler_state> qrtz_scheduler_state { get; set; }
        public virtual DbSet<qrtz_simple_triggers> qrtz_simple_triggers { get; set; }
        public virtual DbSet<qrtz_simprop_triggers> qrtz_simprop_triggers { get; set; }
        public virtual DbSet<qrtz_triggers> qrtz_triggers { get; set; }
        public virtual DbSet<schedule_job> schedule_job { get; set; }
        public virtual DbSet<schedule_job_log> schedule_job_log { get; set; }
        public virtual DbSet<sys_captcha> sys_captcha { get; set; }
        public virtual DbSet<sys_config> sys_config { get; set; }
        public virtual DbSet<sys_fingervalidate> sys_fingervalidate { get; set; }
        public virtual DbSet<sys_log> sys_log { get; set; }
        public virtual DbSet<sys_menu> sys_menu { get; set; }
        public virtual DbSet<sys_oss> sys_oss { get; set; }
        public virtual DbSet<sys_request_log> sys_request_log { get; set; }
        public virtual DbSet<sys_role> sys_role { get; set; }
        public virtual DbSet<sys_role_menu> sys_role_menu { get; set; }
        public virtual DbSet<sys_user> sys_user { get; set; }
        public virtual DbSet<sys_user_role> sys_user_role { get; set; }
        public virtual DbSet<sys_user_token> sys_user_token { get; set; }
        public virtual DbSet<sys_userface> sys_userface { get; set; }
        public virtual DbSet<sys_userfinger> sys_userfinger { get; set; }
        public virtual DbSet<t_tools_cabinet> t_tools_cabinet { get; set; }
        public virtual DbSet<t_tools_info> t_tools_info { get; set; }
        public virtual DbSet<t_tools_user> t_tools_user { get; set; }
        public virtual DbSet<tb_user> tb_user { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<qrtz_blob_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_blob_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_blob_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_calendars>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_calendars>()
                .Property(e => e.CALENDAR_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_cron_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_cron_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_cron_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_cron_triggers>()
                .Property(e => e.CRON_EXPRESSION)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_cron_triggers>()
                .Property(e => e.TIME_ZONE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.ENTRY_ID)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.INSTANCE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.STATE)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.JOB_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.JOB_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.IS_NONCONCURRENT)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_fired_triggers>()
                .Property(e => e.REQUESTS_RECOVERY)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.JOB_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.JOB_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.JOB_CLASS_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.IS_DURABLE)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.IS_NONCONCURRENT)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.IS_UPDATE_DATA)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .Property(e => e.REQUESTS_RECOVERY)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_job_details>()
                .HasMany(e => e.qrtz_triggers)
                .WithRequired(e => e.qrtz_job_details)
                .HasForeignKey(e => new { e.SCHED_NAME, e.JOB_NAME, e.JOB_GROUP })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<qrtz_locks>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_locks>()
                .Property(e => e.LOCK_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_paused_trigger_grps>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_paused_trigger_grps>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_scheduler_state>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_scheduler_state>()
                .Property(e => e.INSTANCE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simple_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simple_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simple_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.STR_PROP_1)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.STR_PROP_2)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.STR_PROP_3)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.BOOL_PROP_1)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_simprop_triggers>()
                .Property(e => e.BOOL_PROP_2)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.SCHED_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.TRIGGER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.TRIGGER_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.JOB_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.JOB_GROUP)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.TRIGGER_STATE)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.TRIGGER_TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .Property(e => e.CALENDAR_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<qrtz_triggers>()
                .HasOptional(e => e.qrtz_blob_triggers)
                .WithRequired(e => e.qrtz_triggers);

            modelBuilder.Entity<qrtz_triggers>()
                .HasOptional(e => e.qrtz_cron_triggers)
                .WithRequired(e => e.qrtz_triggers);

            modelBuilder.Entity<qrtz_triggers>()
                .HasOptional(e => e.qrtz_simple_triggers)
                .WithRequired(e => e.qrtz_triggers);

            modelBuilder.Entity<qrtz_triggers>()
                .HasOptional(e => e.qrtz_simprop_triggers)
                .WithRequired(e => e.qrtz_triggers);

            modelBuilder.Entity<schedule_job>()
                .Property(e => e.bean_name)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job>()
                .Property(e => e.method_name)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job>()
                .Property(e => e._params)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job>()
                .Property(e => e.cron_expression)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job>()
                .Property(e => e.remark)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job_log>()
                .Property(e => e.bean_name)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job_log>()
                .Property(e => e.method_name)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job_log>()
                .Property(e => e._params)
                .IsUnicode(false);

            modelBuilder.Entity<schedule_job_log>()
                .Property(e => e.error)
                .IsUnicode(false);

            modelBuilder.Entity<sys_captcha>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<sys_config>()
                .Property(e => e.param_key)
                .IsUnicode(false);

            modelBuilder.Entity<sys_config>()
                .Property(e => e.param_value)
                .IsUnicode(false);

            modelBuilder.Entity<sys_config>()
                .Property(e => e.remark)
                .IsUnicode(false);

            modelBuilder.Entity<sys_fingervalidate>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_fingervalidate>()
                .Property(e => e.code)
                .IsUnicode(false);

            modelBuilder.Entity<sys_fingervalidate>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<sys_fingervalidate>()
                .Property(e => e.roleid)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log>()
                .Property(e => e.api_name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log>()
                .Property(e => e.info)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log>()
                .Property(e => e._params)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<sys_log>()
                .Property(e => e.create_date)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.perms)
                .IsUnicode(false);

            modelBuilder.Entity<sys_menu>()
                .Property(e => e.icon)
                .IsUnicode(false);

            modelBuilder.Entity<sys_oss>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.API_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.INFO)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.PARAMS)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.RESULT_INFO)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.TIME)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.CREATE_TIME)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.RESULT_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<sys_request_log>()
                .Property(e => e.UPDATE_TIME)
                .IsUnicode(false);

            modelBuilder.Entity<sys_role>()
                .Property(e => e.role_name)
                .IsUnicode(false);

            modelBuilder.Entity<sys_role>()
                .Property(e => e.remark)
                .IsUnicode(false);

            modelBuilder.Entity<sys_role>()
                .Property(e => e.type)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.nickname)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.remote_id)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.salt)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.idcard)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.mobile)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user>()
                .Property(e => e.staff_code)
                .IsUnicode(false);

            modelBuilder.Entity<sys_user_token>()
                .Property(e => e.token)
                .IsUnicode(false);

            modelBuilder.Entity<sys_userface>()
                .Property(e => e.userid)
                .IsUnicode(false);

            modelBuilder.Entity<sys_userface>()
                .Property(e => e.face)
                .IsUnicode(false);

            modelBuilder.Entity<sys_userfinger>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<sys_userfinger>()
                .Property(e => e.FINGER)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_cabinet>()
                .Property(e => e.REMOTE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_cabinet>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_cabinet>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_cabinet>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_cabinet>()
                .Property(e => e.TOKEN)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.TYPE)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.RFID)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.STATUS)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.CABINET_ID)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_info>()
                .Property(e => e.REMOTE_ID)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_user>()
                .Property(e => e.TOOLS_ID)
                .IsUnicode(false);

            modelBuilder.Entity<t_tools_user>()
                .Property(e => e.USER_ID)
                .IsUnicode(false);

            modelBuilder.Entity<tb_user>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tb_user>()
                .Property(e => e.mobile)
                .IsUnicode(false);

            modelBuilder.Entity<tb_user>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
