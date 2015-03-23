using System;

namespace Transit.Core
{

    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Property | AttributeTargets.Delegate)]
    public sealed class RouteOutAttribute : Attribute 
    {

        public RouteOutAttribute() : base()
        {
        }

    }

}
