using System;

namespace Transit.Core.Internal
{

    internal class RouteData<TRouteIn, TRouteOut, TRouteData>
    {

        private TRouteData _data;
        private Route<TRouteIn, TRouteOut> _route;
        

        private RouteData()
        {
        }

        internal RouteData(Route<TRouteIn, TRouteOut> route, TRouteData data)
        {

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            this._data = data;
            this._route = route;

        }

        #region public

        public TRouteData Data
        {

            get
            {
                return this._data;
            }

        }

        public Route<TRouteIn, TRouteOut> Route
        {

            get
            {
                return this._route;
            }

        }

        #endregion

    }

}
