using System;
using Transit.Core;

namespace SimpleConsole
{
    
    public class SubscriberComponent : Component
    {

        public SubscriberComponent() : base("Count")
        {
        }


        #region public

        [RouteIn]
        public void Increment(int value)
        {
            Console.WriteLine(value);
        }

        #endregion

    }

}
