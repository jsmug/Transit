using System;

namespace Transit.Core
{
    public abstract class RouteConverter
    {

        protected RouteConverter()
        {
        }


        #region public

        public abstract bool CanConvertFrom(Type fromType);
        public abstract object Convert(object from);

        #endregion

    }

}
