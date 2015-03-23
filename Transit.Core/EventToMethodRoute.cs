using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Transit.Core
{

    public class EventToMethodRoute : EventRoute
    {

        private EventInfo _eventInfo;
        private MethodInfo _methodInfo;
        private Type _routeInType;
        private Type _routeOutType;


        private EventToMethodRoute() : base()
        {
        }

        private EventToMethodRoute(Component routeOutComponent, string eventName, Component routeInComponent, string methodName) : this(routeOutComponent, eventName, routeInComponent, methodName, null)
        {
        }

        private EventToMethodRoute(Component routeOutComponent, string eventName, Component routeInComponent, string methodName, RouteConverter routeConverter) : base(routeOutComponent, eventName, routeInComponent, methodName, routeConverter)
        {

            if (string.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            this._eventInfo = this.RouteOutComponent.GetType().GetEvent(this.RouteOut, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            this._methodInfo = this.RouteInComponent.GetType().GetMethod(this.RouteIn, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);

            if (this._eventInfo == null)
            {
                throw new ArgumentException("The event could not be found.", "eventName");
            }

            if (this._methodInfo == null)
            {
                throw new ArgumentException("The method could not be found.", "methodName");
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


        public static EventToMethodRoute Create<TRouteType>(Component routeOutComponent, string eventName, Component routeInComponent, Expression<Action<TRouteType>> methodExpression)
        {
            return EventToMethodRoute.Create(routeOutComponent, eventName, routeInComponent, methodExpression, null);
        }

        public static EventToMethodRoute Create<TRouteType>(Component routeOutComponent, EventInfo eventInfo, Component routeInComponent, Expression<Action<TRouteType>> methodExpression)
        {
            return EventToMethodRoute.Create(routeOutComponent, eventInfo, routeInComponent, methodExpression, null);
        }

        public static EventToMethodRoute Create<TRouteType>(Component routeOutComponent, EventInfo eventInfo, Component routeInComponent, Expression<Action<TRouteType>> methodExpression, RouteConverter routeConverter)
        {

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            return EventToMethodRoute.Create(routeOutComponent, eventInfo.Name, routeInComponent, methodExpression, routeConverter);

        }

        public static EventToMethodRoute Create<TRouteType>(Component routeOutComponent, string eventName, Component routeInComponent, Expression<Action<TRouteType>> methodExpression, RouteConverter routeConverter)
        {

            MethodCallExpression methodCallExpression = null;

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

            return new EventToMethodRoute(routeOutComponent, eventName, routeInComponent, methodCallExpression.Method.Name, routeConverter);

        }

        public override void RegisterRoute()
        {
            base.RegisterRoute(this._eventInfo, this._methodInfo);
        }

        #endregion

    }

}
