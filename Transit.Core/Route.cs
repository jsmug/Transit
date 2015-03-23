using System;
using System.Reflection;
using Transit.Core.Common;

namespace Transit.Core
{

    public abstract class Route<TRouteOut, TRouteIn> : ObservableObject, IEquatable<Route<TRouteOut, TRouteIn>>
    {

        private bool _isRegistered;
        private RouteConverter _routeConverter;
        private TRouteIn _routeIn;
        private Component _routeInComponent;
        private TRouteOut _routeOut;
        private Component _routeOutComponent;


        internal Route()
        {
        }

        internal Route(Component routeOutComponent, TRouteOut routeOut, Component routeInComponent, TRouteIn routeIn) : this(routeOutComponent, routeOut, routeInComponent, routeIn, null)
        {
        }

        internal Route(Component routeOutComponent, TRouteOut routeOut, Component routeInComponent, TRouteIn routeIn, RouteConverter routeConverter)
        {

            if (routeOutComponent == null)
            {
                throw new ArgumentNullException("routeOutComponent");
            }

            if (routeOut == null)
            {
                throw new ArgumentNullException("routeOut");
            }

            if (routeInComponent == null)
            {
                throw new ArgumentNullException("routeInComponent");
            }

            if (routeIn == null)
            {
                throw new ArgumentNullException("routeIn");
            }

            this._routeConverter = routeConverter;
            this._routeIn = routeIn;
            this._routeInComponent = routeInComponent;
            this._routeOut = routeOut;
            this._routeOutComponent = routeOutComponent;

        }


        #region public

        public bool IsRegistered
        {

            get
            {
                return this._isRegistered;
            }

        }

        public RouteConverter RouteConverter
        {

            get
            {
                return this._routeConverter;
            }

        }

        public TRouteIn RouteIn
        {

            get
            {
                return this._routeIn;
            }

        }

        public Component RouteInComponent
        {

            get
            {
                return this._routeInComponent;
            }

        }

        public abstract Type RouteInType { get; }

        public TRouteOut RouteOut
        {

            get
            {
                return this._routeOut;
            }

        }

        public Component RouteOutComponent
        {

            get
            {
                return this._routeOutComponent;
            }

        }

        public abstract Type RouteOutType { get; }


        public override bool Equals(object obj)
        {
            return Equals(obj as Route<TRouteOut, TRouteIn>);
        }

        public bool Equals(Route<TRouteOut, TRouteIn> other)
        {
            return other == null ? false : this._routeIn.Equals(other.RouteIn) && this._routeInComponent.Equals(other.RouteInComponent) && this._routeOut.Equals(other.RouteOut) && this._routeOutComponent.Equals(other.RouteOutComponent);
        }

        public override int GetHashCode()
        {

            unchecked
            {

                int hash = 17;

                hash = hash * 23 + this._routeIn.GetHashCode();
                hash = hash * 23 + this._routeInComponent.GetHashCode();
                hash = hash * 23 + this._routeOut.GetHashCode();
                hash = hash * 23 + this._routeOutComponent.GetHashCode();

                return hash;

            }

        }

        public abstract void RegisterRoute();

        public virtual void UnregisterRoute()
        {

            this._isRegistered = false;
            return;

        }

        #endregion

        #region protected

        protected void SetRouteIn(TRouteIn routeIn)
        {

            if (routeIn == null)
            {
                throw new ArgumentNullException("routeIn");
            }

            this._routeIn = routeIn;
            this.OnPropertyChanged(x => this.RouteIn);

        }

        protected void SetRouteInComponent(Component component)
        {

            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            this._routeInComponent = component;
            this.OnPropertyChanged(x => this.RouteInComponent);

        }

        protected void SetRouteOut(TRouteOut routeOut)
        {

            if (routeOut == null)
            {
                throw new ArgumentNullException("routeOut");
            }

            this._routeOut = routeOut;
            this.OnPropertyChanged(x => this.RouteOut);

        }

        protected void SetRouteOutComponent(Component component)
        {

            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            this._routeOutComponent = component;
            this.OnPropertyChanged(x => this.RouteOutComponent);

        }

        protected virtual bool ValidateMethodRouteInAttribute(MethodInfo methodInfo)
        {

            PropertyInfo propertyInfo = this._routeInComponent.GetType().GetProperty(methodInfo.Name.Replace("set_", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            bool hasAttribute = false;

            if (propertyInfo == null)
            {
                propertyInfo = this._routeInComponent.GetType().GetProperty(methodInfo.Name.Replace("get_", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            }

            if (propertyInfo == null)
            {
                hasAttribute = Attribute.IsDefined(methodInfo, typeof(RouteInAttribute));
            }
            else
            {
                hasAttribute = Attribute.IsDefined(propertyInfo, typeof(RouteInAttribute));
            }

            return hasAttribute;

        }

        protected virtual bool ValidateMethodRouteOutAttribute(MethodInfo methodInfo)
        {

            PropertyInfo propertyInfo = this._routeOutComponent.GetType().GetProperty(methodInfo.Name.Replace("set_", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            bool hasAttribute = false;

            if (propertyInfo == null)
            {
                propertyInfo = this._routeOutComponent.GetType().GetProperty(methodInfo.Name.Replace("get_", ""), BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            }
                        
            if (propertyInfo == null)
            {
                hasAttribute = Attribute.IsDefined(methodInfo, typeof(RouteOutAttribute));
            }
            else
            {
                hasAttribute = Attribute.IsDefined(propertyInfo, typeof(RouteOutAttribute));
            }

            return hasAttribute;

        } 

        #endregion

        #region internal
        
        internal void SetIsRegistered(bool isRegistered)
        {
            this._isRegistered = isRegistered;
        }

        #endregion

    }

}
