using System;
using Transit.Core;

namespace RouteConverterConsole
{

    public class PublisherComponent : Component
    {

        private string _dateTimeString;


        public PublisherComponent() : base("Publisher")
        {
        }


        #region public

        [RouteOut]
        public string DateTime 
        {

            get
            {
                return this._dateTimeString;
            }

            set
            {
                SetValue(ref this._dateTimeString, value, x => this.DateTime);
            }
        
        }

        #endregion

    }

}
