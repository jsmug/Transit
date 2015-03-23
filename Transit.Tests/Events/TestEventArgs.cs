using System;

namespace Transit.Tests.Events
{
    
    public class TestEventArgs : EventArgs
    {

        private int _eventInteger;
        private string _eventString;


        private TestEventArgs() : base()
        {
        }

        public TestEventArgs(string eventString, int eventInteger) : base()
        {

            if (string.IsNullOrWhiteSpace(eventString))
            {
                throw new ArgumentNullException("eventString");
            }

            this._eventInteger = eventInteger;
            this._eventString = eventString;

        }


        #region public

        public int EventInteger
        {

            get
            {
                return this._eventInteger;
            }

        }

        public string EventString
        {

            get
            {
                return this._eventString;
            }

        }

        #endregion

    }

}
