﻿using Y.ASIS.Models.Enums;

using Y.ASIS.Common.MVVMFoundation;
namespace Y.ASIS.App.Models
{
    /// <summary>
    /// 动车
    /// </summary>
    public class Train : NotifyObjectBase
    {
        private string no;
        /// <summary>
        /// 车号
        /// </summary>
        public string No
        {
            get { return no; }
            set { SetProperty(ref no, value); }
        }

        private PantographState leftPantograph;
        /// <summary>
        /// 左受电弓状态
        /// </summary>
        public PantographState LeftPantograph
        {
            get { return leftPantograph; }
            set { SetProperty(ref leftPantograph, value); }
        }

        private PantographState rightPantograph;
        /// <summary>
        /// 右受电弓状态
        /// </summary>
        public PantographState RightPantograph
        {
            get { return rightPantograph; }
            set { SetProperty(ref rightPantograph, value); }
        }

        private string state;
        /// <summary>
        /// 状态
        /// </summary>
        public string State
        {
            get { return state; }

            set
            {

                //state = value;
                //OnPropertyChanged(nameof(State));

                SetProperty(ref state, value);

            }
        }


        public override bool Equals(object obj)
        {
            if (!(obj is Train train))
            {
                return false;
            }

            return No == train.no
                && LeftPantograph == train.leftPantograph
                && RightPantograph == train.RightPantograph;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}
