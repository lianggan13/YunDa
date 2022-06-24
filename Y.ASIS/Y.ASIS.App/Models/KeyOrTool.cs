using Y.ASIS.Common.MVVMFoundation;

namespace Y.ASIS.App.Models
{
    public class KeyOrTool : EnumerableObject
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private int trackId;
        public int TrackId
        {
            get { return trackId; }
            set { SetProperty(ref trackId, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }


        private bool @using;
        public bool Using
        {
            get { return @using; }
            set
            {
                SetProperty(ref @using, value);
                OnPropertyChanged("Using");
            }
        }

    }

    public class KeyAndToolInfo : NotifyObjectBase
    {
        private int keyNow;
        public int KeyNow
        {
            get { return keyNow; }
            set { SetProperty(ref keyNow, value); }
        }

        private int keyHasLent;
        public int KeyHasLent
        {
            get { return keyHasLent; }
            set { SetProperty(ref keyHasLent, value); }
        }

        private int toolNow;
        public int ToolNow
        {
            get { return toolNow; }
            set { SetProperty(ref toolNow, value); }
        }

        private int toolHasLent;
        public int ToolHasLent
        {
            get { return toolHasLent; }
            set { SetProperty(ref toolHasLent, value); }
        }
    }
}
