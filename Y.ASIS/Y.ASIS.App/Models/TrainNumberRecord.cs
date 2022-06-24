using System;

namespace Y.ASIS.App.Models
{
    class TrainNumberRecord : EnumerableObject
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        private string track;
        public string Track
        {
            get { return track; }
            set { SetProperty(ref track, value); }
        }

        private string trainNumber;
        public string TrainNumber
        {
            get { return trainNumber; }
            set { SetProperty(ref trainNumber, value); }
        }

        private DateTime time;
        public DateTime Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }
    }
}
