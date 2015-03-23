using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace Transit.Core
{
    
    public abstract class PropertyRoute : Route<string, string>, IWeakEventListener
    {

        internal PropertyRoute() : base()
        {
        }

        internal PropertyRoute(Component routeOutComponent, string propertyName, Component routeInComponent, string handlerName) : this(routeOutComponent, propertyName, routeInComponent, handlerName, null)
        {
        }

        internal PropertyRoute(Component routeOutComponent, string propertyName, Component routeInComponent, string handlerName, RouteConverter routeConverter) : base(routeOutComponent, propertyName, routeInComponent, handlerName, routeConverter)
        {
        }


        #region public

        public override abstract Type RouteInType { get; }
        public override abstract Type RouteOutType { get; }


        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "In: {0}-{1}, Out: {2}-{3}", this.RouteInComponent.Name, this.RouteInType, this.RouteOutComponent.Name, this.RouteOutType);
        }

        public override void UnregisterRoute()
        {

            PropertyChangedEventManager.RemoveListener(this.RouteOutComponent, this, this.RouteOut);
            base.UnregisterRoute();

        }

        #endregion

        #region protected

        protected abstract void OnPropertyRoutePropertyChanged(object sender, PropertyChangedEventArgs e);

        protected virtual void RegisterRoute(MethodInfo methodOutInfo, MethodInfo methodInInfo)
        {

            if (methodOutInfo == null)
            {
                throw new ArgumentNullException("methodOutInfo");
            }

            if (methodInInfo == null)
            {
                throw new ArgumentNullException("methodInInfo");
            }

            if (!ValidateMethodRouteOutAttribute(methodOutInfo))
            {
                throw new ArgumentException("The method or property must be marked with the RouteOutAttribute.", "methodOutInfo");
            }

            if (!ValidateMethodRouteInAttribute(methodInInfo))
            {
                throw new ArgumentException("The method or property must be marked with the RouteInAttribute.", "methodInInfo");
            }

            if (!ValidateMatchingTypes(methodOutInfo, methodInInfo))
            {
                throw new InvalidCastException("The property/method types do not match.");
            }

            PropertyChangedEventManager.AddListener(this.RouteOutComponent, this, this.RouteOut);
            this.SetIsRegistered(true);

        }

        protected virtual bool ValidateMatchingTypes(MethodInfo methodOutInfo, MethodInfo methodInInfo)
        {

            if (methodOutInfo == null)
            {
                throw new ArgumentNullException("methodOutInfo");
            }

            if (methodInInfo == null)
            {
                throw new ArgumentNullException("methodInInfo");
            }

            ParameterInfo[] parameters = methodOutInfo.GetParameters();
            Type methodOutType;

            if (parameters.Length == 0)
            {
                methodOutType = methodOutInfo.ReturnType;
            }
            else
            {
                methodOutType = parameters[0].ParameterType;
            }

            return this.RouteConverter != null ? this.RouteConverter.CanConvertFrom(methodOutType) : methodInInfo.GetParameters()[0].ParameterType.IsAssignableFrom(methodOutType);

        }

        #endregion

        #region IWeakEventListener

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {

            bool result = false;
        
            if (managerType == typeof(PropertyChangedEventManager))
            {
            
                OnPropertyRoutePropertyChanged(sender, (PropertyChangedEventArgs)e);
                result = true;

            }

            return result;

        }

        #endregion
 
    }

}
