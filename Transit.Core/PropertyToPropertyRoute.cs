using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Transit.Core
{

    public class PropertyToPropertyRoute : PropertyRoute
    {

        private MethodInfo _propertyInInfo;
        private MethodInfo _propertyOutInfo;
        private Type _routeInType;
        private Type _routeOutType;


        private PropertyToPropertyRoute() : base()
        {
        }

        private PropertyToPropertyRoute(Component routeOutComponent, string propertyOutName, Component routeInComponent, string propertyInName) : this(routeOutComponent, propertyOutName, routeInComponent, propertyInName, null)
        {
        }

        private PropertyToPropertyRoute(Component routeOutComponent, string propertyOutName, Component routeInComponent, string propertyInName, RouteConverter routeConverter) : base(routeOutComponent, propertyOutName, routeInComponent, propertyInName, routeConverter)
        {

            if (string.IsNullOrWhiteSpace(propertyOutName))
            {
                throw new ArgumentNullException("propertyOutName");
            }

            if (string.IsNullOrWhiteSpace(propertyInName))
            {
                throw new ArgumentNullException("propertyInName");
            }

            this._propertyInInfo = this.RouteInComponent.GetType().GetProperty(this.RouteIn, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetSetMethod();
            this._propertyOutInfo = this.RouteOutComponent.GetType().GetProperty(this.RouteOut, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetGetMethod();

            if (this._propertyOutInfo == null)
            {
                throw new ArgumentException("The property could not be found.", "propertyOutName");
            }

            if (this._propertyInInfo == null)
            {
                throw new ArgumentException("The property could not be found.", "propertyInName");
            }

            this._routeInType = this._propertyInInfo.GetParameters()[0].ParameterType;
            this._routeOutType = this._propertyOutInfo.ReturnType;

        }


        #region public

        public override Type RouteInType
        {

            get
            {
                return this._routeInType;
            }

        }

        public override Type RouteOutType
        {

            get
            {
                return this._routeOutType;
            }

        }


        public static PropertyToPropertyRoute Create<TRouteType>(Component routeOutComponent, Expression<Func<Component, TRouteType>> propertyOutExpresson, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyInExpresson)
        {
            return PropertyToPropertyRoute.Create(routeOutComponent, propertyOutExpresson, routeInComponent, propertyInExpresson, null);
        }

        public static PropertyToPropertyRoute Create<TRouteOutType, TRouteInType>(Component routeOutComponent, Expression<Func<Component, TRouteOutType>> propertyOutExpresson, Component routeInComponent, Expression<Func<Component, TRouteInType>> propertyInExpresson, RouteConverter routeConverter)
        {

            MemberExpression memberInExpression = null;
            MemberExpression memberOutExpression = null;

            if (propertyOutExpresson == null)
            {
                throw new ArgumentNullException("propertyOutExpression");
            }

            if (propertyInExpresson == null)
            {
                throw new ArgumentNullException("propertyInExpression");
            }

            memberOutExpression = propertyOutExpresson.Body as MemberExpression;
            memberInExpression = propertyInExpresson.Body as MemberExpression;

            if (memberOutExpression == null)
            {
                throw new ArgumentException("The property expression is not valid. Could not get MemberExpression.", "propertyOutExpression");
            }

            if (memberInExpression == null)
            {
                throw new ArgumentException("The property expression is not valid. Could not get MemberExpression.", "propertyInExpression");
            }

            if (memberOutExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The propertyOutExpression is not a valid type. Type found: {0}.", memberOutExpression.Member.MemberType), "propertyOutExpression");
            }

            if (memberInExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The propertyInExpression is not a valid type. Type found: {0}.", memberInExpression.Member.MemberType), "propertyInExpression");
            }

            return new PropertyToPropertyRoute(routeOutComponent, memberOutExpression.Member.Name, routeInComponent, memberInExpression.Member.Name, routeConverter);

        }

        public static PropertyToPropertyRoute Create<TRouteType>(Component routeOutComponent, string propertyName, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyInExpresson)
        {
            return PropertyToPropertyRoute.Create(routeOutComponent, propertyName, routeInComponent, propertyInExpresson, null);
        }

        public static PropertyToPropertyRoute Create<TRouteType>(Component routeOutComponent, string propertyName, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyInExpresson, RouteConverter routeConverter)
        {

            MemberExpression memberInExpression = null;
            
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            if (propertyInExpresson == null)
            {
                throw new ArgumentNullException("propertyInExpression");
            }

            memberInExpression = propertyInExpresson.Body as MemberExpression;

            if (memberInExpression == null)
            {
                throw new ArgumentException("The property expression is not valid. Could not get MemberExpression.", "propertyInExpression");
            }

            if (memberInExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The propertyInExpression is not a valid type. Type found: {0}.", memberInExpression.Member.MemberType), "propertyInExpression");
            }

            return new PropertyToPropertyRoute(routeOutComponent, propertyName, routeInComponent, memberInExpression.Member.Name, routeConverter);

        }


        public override void RegisterRoute()
        {
            base.RegisterRoute(this._propertyOutInfo, this._propertyInInfo);
        }

        #endregion

        #region protected

        protected override void OnPropertyRoutePropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            object value = null;

            if (this.RouteConverter != null)
            {
                value = this.RouteConverter.Convert(this._propertyOutInfo.Invoke(this.RouteOutComponent, null));
            }
            else
            {
                value = this._propertyOutInfo.Invoke(this.RouteOutComponent, null);
            }

            this._propertyInInfo.Invoke(this.RouteInComponent, new object[] { value });

        }

        #endregion

    }

}
