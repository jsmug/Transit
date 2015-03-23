using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Transit.Core
{

    public abstract class EventRoute : Route<string, string>
    {

        private Delegate _eventDelegate;


        internal EventRoute() : base()
        {
        }

        internal EventRoute(Component routeOutComponent, string eventName, Component routeInComponent, string handlerName) : this(routeOutComponent, eventName, routeInComponent, handlerName, null)
        {
        }

        internal EventRoute(Component routeOutComponent, string eventName, Component routeInComponent, string handlerName, RouteConverter routeConverter): base(routeOutComponent, eventName, routeInComponent, handlerName, routeConverter)
        {
        }


        #region public

        public override abstract Type RouteInType { get; }
        public override abstract Type RouteOutType { get; }
       

        public override void UnregisterRoute()
        {

            EventInfo eventInfo = null;

            if (this._eventDelegate != null)
            {

                eventInfo = this.RouteOutComponent.GetType().GetEvent(this.RouteOut);
                eventInfo.RemoveEventHandler(this.RouteOutComponent, this._eventDelegate);

            }

            base.UnregisterRoute();

        }

        #endregion

        #region protected

        protected virtual void RegisterRoute(EventInfo eventInfo, MethodInfo methodInfo)
        {

            DynamicMethod handler = null;
            ILGenerator generator = null;
            List<Type> eventTypes = null;
            MethodInfo routeConverterInfo = null;
            Type eventType;
            Type methodParameterType;

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            if (!ValidateEventAttributes(eventInfo))
            {
                throw new ArgumentException("The event must be marked with the RouteOutAttribute.", "eventInfo");
            }

            if (!ValidateMethodRouteInAttribute(methodInfo))
            {
                throw new ArgumentException("The method or property must be marked with the RouteInAttribute.", "methodInfo");
            }

            if (!ValidateMatchingTypes(eventInfo, methodInfo))
            {
                throw new InvalidCastException("The event type and property type do not match.");
            }

            if (!IsValidEvent(eventInfo.EventHandlerType))
            {
                throw new ArgumentNullException("The event is not a MultiCastDelegate or is missing an Invoke method.", "eventInfo");
            }

            eventTypes = GetEventParameterTypes(eventInfo.EventHandlerType).ToList();
            eventTypes.Insert(0, this.RouteInComponent.GetType());
            handler = new DynamicMethod("", null, eventTypes.ToArray(), this.RouteInComponent.GetType());

            eventType = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;
            methodParameterType = methodInfo.GetParameters()[0].ParameterType;

            if (this.RouteConverter != null)
            {
                routeConverterInfo = this.RouteConverter.GetType().GetMethod("Convert", BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(object) }, null);
            }

            generator = handler.GetILGenerator();
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldarg_2);

            if (routeConverterInfo != null)
            {

                if (eventType.IsValueType)
                {
                    generator.Emit(OpCodes.Box, eventType);
                }

                generator.Emit(OpCodes.Call, routeConverterInfo);

                if (methodParameterType.IsValueType)
                {
                    generator.Emit(OpCodes.Unbox_Any, methodParameterType);
                }
                else
                {
                    generator.Emit(OpCodes.Castclass, methodParameterType);
                }
          
                generator.Emit(OpCodes.Starg, 2);
                generator.Emit(OpCodes.Ldarg_0);
                generator.Emit(OpCodes.Ldarg_2);
                   
            }

            generator.Emit(OpCodes.Call, methodInfo);
            generator.Emit(OpCodes.Ret);

            this._eventDelegate = handler.CreateDelegate(eventInfo.EventHandlerType, this.RouteInComponent);
            eventInfo.AddEventHandler(this.RouteOutComponent, this._eventDelegate);

            this.SetIsRegistered(true);

        }

        protected virtual IEnumerable<Type> GetEventParameterTypes(Type type)
        {

            ParameterInfo[] parameters = type.GetMethod("Invoke").GetParameters();
            return parameters.Select(x => x.ParameterType);

        }

        protected virtual bool IsValidEvent(Type type)
        {

            bool isValid = false;

            if (type.BaseType == typeof(MulticastDelegate))
            {
                isValid = type.GetMethod("Invoke") != null;
            }

            return isValid;

        }

        protected virtual bool ValidateEventAttributes(EventInfo eventInfo)
        {
            return Attribute.IsDefined(eventInfo, typeof(RouteOutAttribute));
        }

        protected virtual bool ValidateMatchingTypes(EventInfo eventInfo, MethodInfo methodInfo)
        {

            if (eventInfo == null)
            {
                throw new ArgumentNullException("eventInfo");
            }

            if (methodInfo == null)
            {
                throw new ArgumentNullException("methodInfo");
            }

            Type eventType = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;
            return this.RouteConverter != null ? this.RouteConverter.CanConvertFrom(eventType) : methodInfo.GetParameters()[0].ParameterType.IsAssignableFrom(eventType);

        }

        #endregion

    }

}
