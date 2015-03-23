using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Transit.Core
{
    
    public class EventToPropertyRoute : EventRoute
    {

        private EventInfo _eventInfo;
        private MethodInfo _methodInfo;
        private Type _routeInType;
        private Type _routeOutType;


        private EventToPropertyRoute() : base()
        {
        }

        private EventToPropertyRoute(Component routeOutComponent, string eventName, Component routeInComponent, string propertyName) : this(routeOutComponent, eventName, routeInComponent, propertyName, null)
        {
        }

        private EventToPropertyRoute(Component routeOutComponent, string eventName, Component routeInComponent, string propertyName, RouteConverter routeConverter) : base(routeOutComponent, eventName, routeInComponent, propertyName, routeConverter)
        {

            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            this._eventInfo = this.RouteOutComponent.GetType().GetEvent(this.RouteOut, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            this._methodInfo = this.RouteInComponent.GetType().GetProperty(this.RouteIn, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static).GetSetMethod(true);

            if (this._eventInfo == null)
            {
                throw new ArgumentException("The event could not be found.", "eventName");
            }

            if (this._methodInfo == null)
            {
                throw new ArgumentException("The property could not be found.", "propertyName");
            }

            this._routeInType = this._methodInfo.GetParameters()[0].ParameterType;
            this._routeOutType = this._eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;

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


        public static EventToPropertyRoute Create<TRouteType>(Component routeOutComponent, string eventName, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyExpression)
        {
            return EventToPropertyRoute.Create(routeOutComponent, eventName, routeInComponent, propertyExpression, null);
        }

        public static EventToPropertyRoute Create<TRouteType>(Component routeOutComponent, string eventName, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyExpression, RouteConverter routeConverter)
        {

            MemberExpression propertyMember = null;

            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            propertyMember = propertyExpression.Body as MemberExpression;

            if (propertyMember == null)
            {
                throw new ArgumentException("The property expression is not valid. Could not get MemberExpression.", "propertyExpression");
            }

            if (propertyMember.Member.MemberType != MemberTypes.Property)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentCulture, "The propertyExpression is not a valid type. Type found: {0}.", propertyMember.Member.MemberType), "propertyExpression");
            }

            return new EventToPropertyRoute(routeOutComponent, eventName, routeInComponent, propertyMember.Member.Name, routeConverter);

        }

        public static EventToPropertyRoute Create<TRouteType>(Component routeOutComponent, EventInfo eventInfo, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyExpression)
        {
            return EventToPropertyRoute.Create(routeOutComponent, eventInfo, routeInComponent, propertyExpression, null);
        }

        public static EventToPropertyRoute Create<TRouteType>(Component routeOutComponent, EventInfo eventInfo, Component routeInComponent, Expression<Func<Component, TRouteType>> propertyExpression, RouteConverter routeConverter)
        {

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            return EventToPropertyRoute.Create(routeOutComponent, eventInfo.Name, routeInComponent, propertyExpression, routeConverter);

        }

        public override void RegisterRoute()
        {
            base.RegisterRoute(this._eventInfo, this._methodInfo);
        }

        #endregion

    }

}
