using System;
using Transit.Core;

namespace RouteConverterConsole
{
    
    public class StringToDateTimeRouteConverter : RouteConverter
    {

        public StringToDateTimeRouteConverter() : base()
        {
        }


        #region public

        public override bool CanConvertFrom(Type fromType)
        {
            return fromType == typeof(string);
        }

        public override object Convert(object from)
        {

            DateTimeOffset date;
            return DateTimeOffset.TryParse(from.ToString(), out date) ? date : DateTimeOffset.MinValue;

        }

        #endregion

    }

}
