using Newtonsoft.Json;
using Y.ASIS.Common.Models;
using Y.ASIS.Common.MVVMFoundation;
using Y.ASIS.Models.Enums;
namespace Y.ASIS.App.Models
{
    public class VideoStream : NotifyObjectBase, IVideoStream
    {
        private int id;
        public int ID
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Url { get; set; }

        public int Channel { get; set; }

        public string Ip { get; set; }

        public int Port { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Model { get; set; }

        public string Extension { get; set; }

        private VideoType type;
        public VideoType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        private bool playing;
        [JsonIgnore]
        public bool Playing
        {
            get { return playing; }
            set { SetProperty(ref playing, value); }
        }

        private SafeCondition conndition;
        public SafeCondition Condition
        {
            get { return conndition; }
            set
            {
                conndition = value;
                OnPropertyChanged(nameof(Condition));
                if (conndition != value)
                {
                    //Recognize();
                }
            }
        }


        //private void Recognize()
        //{
        //    if (conndition != null)
        //    {
        //        conndition.RecognizeValues = conndition.SafeValues;
        //    }
        //}
    }
}
