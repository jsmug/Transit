using System;
using Transit.Core;

namespace MultipleSubscriberConsole
{

    public class PropertySubscriberComponent : Component
    {

        private EventArgs _eventArgs;


        public PropertySubscriberComponent() : base("Property Subscriber")
        {
        }


        #region public

        [RouteIn]
        public EventArgs ReceiveInterval
        {

            get
            {
                return this._eventArgs;
            }

            set
            {

                this._eventArgs = value;
                Console.WriteLine(string.Format("{0} received Interval Event {1}", this.Name, DateTimeOffset.Now.ToString("hh:mm:ss")));

            }

        }

        #endregion

    }

}
