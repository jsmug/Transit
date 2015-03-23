using System;
using Transit.Core;
using Transit.Tests.Events;

namespace Transit.Tests.Converters
{

    public class TestEventArgsToIntRouteConverter : RouteConverter
    {

        public TestEventArgsToIntRouteConverter() : base()
        {
        }


        #region public

        public override bool CanConvertFrom(Type fromType)
        {
            return fromType == typeof(TestEventArgs);
        }

        public override object Convert(object from)
        {
            return ((TestEventArgs)from).EventInteger;
        }

        #endregion

    }

}
