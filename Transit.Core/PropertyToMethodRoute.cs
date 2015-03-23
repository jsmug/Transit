using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Transit.Core
{
    
    public class PropertyToMethodRoute : PropertyRoute
    {

        private MethodInfo _methodInfo;
        private MethodInfo _propertyOutInfo;
        private Type _routeInType;
        private Type _routeOutType;


        private PropertyToMethodRoute() : base()
        {
        }

        private PropertyToMethodRoute(Component routeOutComponent, string propertyOutName, Component routeInComponent, string methodName): this(routeOutComponent, propertyOutName, routeInComponent, methodName, null)
        {
        }

        private PropertyToMethodRoute(Component routeOutComponent, string propertyOutName, Component routeInComponent, string methodName, RouteConverter routeConverter) : base(routeOutComponent, propertyOutName, routeInComponent, methodName, routeConverter)
        {

            if (string.IsNullOrWhiteSpace(propertyOutName))
            {
                throw new ArgumentNullException("propertyOutName");
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            this._propertyOutInfo = this.RouteOutComponent.GetType().GetProperty(this.RouteOut, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetGetMethod();
            this._methodInfo = this.RouteInComponent.GetType().GetMethod(this.RouteIn, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

            if (this._propertyOutInfo == null)
            {
                throw new ArgumentException("The property could not be found.", "propertyOutName");
            }

            if (this._methodInfo == null)
            {
                throw new ArgumentException("The in method could not be found.", "methodName");
            }

            this._routeInType = this._methodInfo.GetParameters()[0].ParameterType;
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


        public static PropertyToMethodRoute Create<TRouteType>(Component routeOutComponent, Expression<Func<Component, TRouteType>> propertyExpresson, Component routeInComponent, Expression<Action<TRouteType>> methodExpression)
        {
            return PropertyToMethodRoute.Create(routeOutComponent, propertyExpresson, routeInComponent, methodExpression, null);
        }

        public static PropertyToMethodRoute Create<TRouteOutType, TRouteInType>(Component routeOutComponent, Expression<Func<Component, TRouteOutType>> propertyExpresson, Component routeInComponent, Expression<Action<TRouteInType>> methodExpression, RouteConverter routeConverter)
        {

            MemberExpression memberExpression = null;
            MethodCallExpression methodCallExpression = null;

            if (propertyExpresson == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            if (methodExpression == null)
            {
                throw new ArgumentNullException("methodExpression");
            }

            memberExpression = propertyExpresson.Body as MemberExpression;
            methodCallExpression = methodExpression.Body as MethodCallExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("The property expression is not valid. Could not get MemberExpression.", "propertyExpression");
            }

            if (methodCallExpression == null)
            {
                throw new ArgumentException("The method expression is not valid. Could not get MethodCallExpression.", "methodExpression");
            }

            if (memberExpression.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The propertyExpression is not a valid type. Type found: {0}.", memberExpression.Member.MemberType), "propertyExpression");
            }

            if (methodCallExpression.Method.MemberType != MemberTypes.Method)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The methodExpression is not a valid type. Type found: {0}.", methodCallExpression.Method.MemberType), "methodExpression");
            }

            return new PropertyToMethodRoute(routeOutComponent, memberExpression.Member.Name, routeInComponent, methodCallExpression.Method.Name, routeConverter);

        }

        public static PropertyToMethodRoute Create<TRouteType>(Component routeOutComponent, string propertyName, Component routeInComponent, Expression<Action<TRouteType>> methodExpression)
        {
            return PropertyToMethodRoute.Create(routeOutComponent, propertyName, routeInComponent, methodExpression, null);
        }

        public static PropertyToMethodRoute Create<TRouteType>(Component routeOutComponent, string propertyName, Component routeInComponent, Expression<Action<TRouteType>> methodExpression, RouteConverter routeConverter)
        {

            MethodCallExpression methodCallExpression = null;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            if (methodExpression == null)
            {
                throw new ArgumentNullException("methodExpression");
            }

            methodCallExpression = methodExpression.Body as MethodCallExpression;

            if (methodCallExpression == null)
            {
                throw new ArgumentException("The method expression is not valid. Could not get MethodCallExpression.", "methodExpression");
            }

            if (methodCallExpression.Method.MemberType != MemberTypes.Method)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The methodExpression is not a valid type. Type found: {0}.", methodCallExpression.Method.MemberType), "methodExpression");
            }

            return new PropertyToMethodRoute(routeOutComponent, propertyName, routeInComponent, methodCallExpression.Method.Name, routeConverter);

        }


        public override void RegisterRoute()
        {
            base.RegisterRoute(this._propertyOutInfo, this._methodInfo);
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
            
            this._methodInfo.Invoke(this.RouteInComponent, new object[] { value });
        
        }

        #endregion

    }

}
