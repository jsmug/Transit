using System;
using Transit.Core;

namespace MultipleSubscriberConsole
{

    public class MethodSubscriberComponent : Component
    {

        public MethodSubscriberComponent() : base("Method Subscriber")
        {
        }


        #region public

        [RouteIn]
        public void ReceiveInterval(EventArgs e)
        {
            Console.WriteLine(string.Format("{0} received Interval Event {1}",  this.Name, DateTimeOffset.Now.ToString("hh:mm:ss")));
        }

        #endregion

    }

}
