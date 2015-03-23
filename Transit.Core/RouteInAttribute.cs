using System;

namespace Transit.Core
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class RouteInAttribute : Attribute 
    {

        public RouteInAttribute() : base()
        {
        }

    }

}
