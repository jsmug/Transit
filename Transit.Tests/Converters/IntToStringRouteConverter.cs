using System;
using Transit.Core;

namespace Transit.Tests.Converters
{

    public class IntToStringRouteConverter : RouteConverter
    {

        public IntToStringRouteConverter() : base()
        {
        }


        #region public

        public override bool CanConvertFrom(Type fromType)
        {
            return fromType == typeof(int);
        }

        public override object Convert(object from)
        {
            return from.ToString();
        }

        #endregion

    }

}
