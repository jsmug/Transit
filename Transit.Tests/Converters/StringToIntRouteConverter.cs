using System;
using Transit.Core;

namespace Transit.Tests.Converters
{

    public class StringToIntRouteConverter : RouteConverter
    {

        public StringToIntRouteConverter() : base()
        {
        }


        #region public

        public override bool CanConvertFrom(Type fromType)
        {
            return fromType == typeof(string);
        }

        public override object Convert(object from)
        {

            int converted;
            int.TryParse(from.ToString(), out converted);

            return converted;

        }

        #endregion

    }

}
