using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Y.ASIS.Common.MVVMFoundation;
using Y.ASIS.Models.Enums;
namespace Y.ASIS.App.Models
{
    public class SafeConfirm : NotifyObjectBase
    {
        public string Name { get; set; }

        public List<SafeCondition> Conditions { get; set; }
    }

    public class SafeCondition : NotifyObjectBase
    {
        public int Index { get; set; }

        private string text;
        public string Text
        {
            get { return text; }
            set { SetProperty(ref text, value); }
        }

        private VideoType type;
        public VideoType Type
        {
            get { return type; }
            set { SetProperty(ref type, value); }
        }

        public List<int> SafeValues { get; set; }

        private List<int> recognizeValues;
        [JsonIgnore]
        public List<int> RecognizeValues
        {
            get { return recognizeValues; }
            set
            {
                if (recognizeValues != value)
                {
                    recognizeValues = value;
                    OnPropertyChanged(nameof(RecognizeValues));
                    OnPropertyChanged(nameof(IsSafe));
                }
            }
        }


        [JsonIgnore]
        public bool? IsSafe
        {
            get
            {
                if (RecognizeValues == null)
                {
                    return null;
                }
                return RecognizeValues.Any(i => SafeValues?.Contains(i) == true);
            }
        }
    }
}
