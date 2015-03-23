using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Transit.Core
{

    public class OldEventToPropertyRoute<TEventArgs, TPropertyExpression> : Route<Delegate, Expression<Func<Hub, TEventArgs>>> where TEventArgs : EventArgs
    {

        private Action<TEventArgs> _propertyAction;


        private OldEventToPropertyRoute() : base(null, null, null, null)
        {
        }

        public OldEventToPropertyRoute(Hub routeOutHub, Delegate eventHandler, Hub routeInHub, Expression<Func<Hub, TEventArgs>> propertyExpression) : base(routeOutHub, eventHandler, routeInHub, propertyExpression)
        {

            if (!eventHandler.GetType().IsDefined(typeof(RouteOutAttribute), false))
            {
                throw new ArgumentException("Invalid Route Out");
            }

            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression == null)
            {

            }

            if (memberExpression.Member.MemberType != MemberTypes.Property)
            {

            }

            if (!((PropertyInfo)memberExpression.Member).IsDefined(typeof(RouteOutAttribute), false))
            {
                throw new ArgumentException("Invalid Route In");
            }

            this._propertyAction = (Action<TEventArgs>)Delegate.CreateDelegate(typeof(Action<TEventArgs>), routeInHub, ((PropertyInfo)memberExpression.Member).GetSetMethod());
            AttachEvent();

        }


        #region public

        public override Type RouteInType 
        { 
            
            get
            {
                return typeof(TEventArgs);
            }

        }

        public override Type RouteOutType 
        { 
            
            get
            {
                return typeof(TEventArgs);
            }
 
        }

        #endregion

        #region private

        private void AttachEvent()
        {
            base.RouteOut(OnEvent);
        }

        private void DetachEvent()
        {
            //base.RouteOut -= OnEvent;
        }

        private void OnEvent(object sender, TEventArgs e)
        {
            this._propertyAction(e);
        }

        #endregion

    }

}
