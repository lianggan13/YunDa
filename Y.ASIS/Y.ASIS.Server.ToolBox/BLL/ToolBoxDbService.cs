using System.Data;
using System.Linq;
using Y.ASIS.Server.ToolBox.DAL;

namespace Y.ASIS.Server.ToolBox.BLL
{
    public class ToolBoxDbService
    {
        private MySqlDbContext context = new MySqlDbContext();
        public bool UserHasExsit(string remoteId)
        {
            var user = context.sys_user.FirstOrDefault(u => u.remote_id == remoteId);
            return user != null;
        }

        public bool UserFaceHasExsit(string remoteId)
        {
            var user = context.sys_user.FirstOrDefault(u => u.remote_id == remoteId);
            if (user == null)
                return false;

            var face = context.sys_userface.FirstOrDefault(f => f.userid == user.user_id.ToString());
            return face != null;
        }

        public bool CheckDbOpened()
        {
            return context.Database.Connection.State == ConnectionState.Open;
        }
    }
}
