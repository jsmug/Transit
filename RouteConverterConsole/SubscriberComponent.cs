using System;
using Transit.Core;

namespace RouteConverterConsole
{

    public class SubscriberComponent : Component
    {

        public SubscriberComponent() : base("Subscriber")
        {
            this.DateTime = DateTimeOffset.Now.Subtract(new TimeSpan(30, 0, 0, 0));
        }


        #region public

        [RouteIn]
        public DateTimeOffset DateTime { get; set; }
        
        #endregion

    }

}
