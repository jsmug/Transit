using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Transit.Core.Common;
using Transit.Core.Events;

namespace Transit.Core
{
    
    public class Package : ObservableObject
    {

        #region constants

        private const string DefaultPackageName = "Package";

        #endregion


        private readonly ReadOnlyObservableCollection<Component> _components; 
        private readonly ObservableCollection<Component> _componentsBuffer = new ObservableCollection<Component>();
        private readonly List<EventRoute> _eventRoutes = new List<EventRoute>();
        private string _name;
        private readonly List<PropertyRoute> _propertyRoutes = new List<PropertyRoute>();


        public event EventHandler<EventRoutePreRegistrationEventArgs> EventRoutePreRegister = delegate { };
        public event EventHandler<EventRouteRegistrationEventArgs> EventRouteRegistered = delegate { };
        public event EventHandler<EventRoutePreRegistrationEventArgs> EventRoutePreUnregister = delegate { };
        public event EventHandler<EventRouteRegistrationEventArgs> EventRouteUnregistered = delegate { };
        public event EventHandler<PropertyRoutePreRegistrationEventArgs> PropertyRoutePreRegister = delegate { };
        public event EventHandler<PropertyRouteRegistrationEventArgs> PropertyRouteRegistered = delegate { };
        public event EventHandler<PropertyRoutePreRegistrationEventArgs> PropertyRoutePreUnregister = delegate { };
        public event EventHandler<PropertyRouteRegistrationEventArgs> PropertyRouteUnregistered = delegate { };


        public Package() : this(DefaultPackageName)
        {

            this._components = new ReadOnlyObservableCollection<Component>(this._componentsBuffer);

        }

        public Package(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            this._components = new ReadOnlyObservableCollection<Component>(this._componentsBuffer);
            this._name = name;

        }


        #region public

        public ReadOnlyObservableCollection<Component> Components
        {

            get
            {
                return this._components;
            }

        }

        public string Name
        {

            get
            {
                return this._name;
            }

            set
            {
                SetValue(ref this._name, value, x => this.Name);
            }

        }

        public IEnumerable<EventRoute> RegisteredEventRoutes
        {

            get
            {
                return this._eventRoutes.Skip(0);
            }

        }

        public IEnumerable<PropertyRoute> RegisteredPropertyRoutes
        {

            get
            {
                return this._propertyRoutes.Skip(0);
            }

        }


        public void RegisterEventRoute(EventRoute route)
        {

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (!this._eventRoutes.Contains(route))
            {

                EventRoutePreRegistrationEventArgs e = new EventRoutePreRegistrationEventArgs(route);
                EventRoutePreRegister(this, e);

                if (!e.Cancel)
                {

                    if (!route.IsRegistered)
                    {
                        route.RegisterRoute();
                    }

                    if (route.IsRegistered)
                    {

                        this._eventRoutes.Add(route);
                        EventRouteRegistered(this, new EventRouteRegistrationEventArgs(route));

                    }

                    AddRouteComponents(route);

                }

            }

        }

        public void RegisterPropertyRoute(PropertyRoute route)
        {

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (!this._propertyRoutes.Contains(route))
            {

                PropertyRoutePreRegistrationEventArgs e = new PropertyRoutePreRegistrationEventArgs(route);
                PropertyRoutePreRegister(this, e);

                if (!e.Cancel)
                {

                    if (!route.IsRegistered)
                    {
                        route.RegisterRoute();
                    }

                    if (route.IsRegistered)
                    {

                        this._propertyRoutes.Add(route);
                        PropertyRouteRegistered(this, new PropertyRouteRegistrationEventArgs(route));

                    }

                    AddRouteComponents(route);

                }

            }

        }

        public void UnregisterAll()
        {

            this._componentsBuffer.Clear();
            this._eventRoutes.ForEach(x => UnregisterEventRoute(x));
            this._propertyRoutes.ForEach(x => UnregisterPropertyRoute(x));

        }

        public void UnregisterEventRoute(EventRoute route)
        {

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (this._eventRoutes.Contains(route))
            {

                EventRoutePreRegistrationEventArgs e = new EventRoutePreRegistrationEventArgs(route);
                EventRoutePreUnregister(this, e);

                if (!e.Cancel)
                {

                    if (route.IsRegistered)
                    {
                        route.UnregisterRoute();
                    }

                    if (!route.IsRegistered)
                    {

                        this._eventRoutes.Remove(route);
                        EventRouteUnregistered(this, new EventRouteRegistrationEventArgs(route));

                    }

                    RemoveRouteComponents(route);

                }

            }

        }

        public void UnregisterPropertyRoute(PropertyRoute route)
        {

            if (route == null)
            {
                throw new ArgumentNullException("route");
            }

            if (this._propertyRoutes.Contains(route))
            {

                PropertyRoutePreRegistrationEventArgs e = new PropertyRoutePreRegistrationEventArgs(route);
                PropertyRoutePreUnregister(this, e);

                if (!e.Cancel)
                {

                    if (route.IsRegistered)
                    {
                        route.UnregisterRoute();
                    }

                    if (!route.IsRegistered)
                    {

                        this._propertyRoutes.Remove(route);
                        PropertyRouteUnregistered(this, new PropertyRouteRegistrationEventArgs(route));

                    }

                    RemoveRouteComponents(route);

                }

            }

        }

        #endregion

        #region private

        private void AddRouteComponents(Route<string, string> route)
        {

            if (!this._componentsBuffer.Contains(route.RouteInComponent))
            {
                this._componentsBuffer.Add(route.RouteInComponent);
            }

            if (!this._componentsBuffer.Contains(route.RouteOutComponent))
            {
                this._componentsBuffer.Add(route.RouteOutComponent);
            }

        }

        private void RemoveRouteComponents(Route<string, string> route)
        {

            Component inComponent = route.RouteInComponent;
            Component outComponent = route.RouteOutComponent;
            bool foundExistingInComponent = this._propertyRoutes.Where(x => x.RouteInComponent == inComponent).Any();
            bool foundExistingOutComponent = this._propertyRoutes.Where(x => x.RouteOutComponent == outComponent).Any();

            if (!foundExistingInComponent)
            {

                foundExistingInComponent = this._eventRoutes.Where(x => x.RouteInComponent == inComponent).Any();

                if (!foundExistingInComponent)
                {
                    this._componentsBuffer.Remove(inComponent);
                }

            }

            if (!foundExistingOutComponent)
            {

                foundExistingOutComponent = this._eventRoutes.Where(x => x.RouteOutComponent == outComponent).Any();

                if (!foundExistingOutComponent)
                {
                    this._componentsBuffer.Remove(outComponent);
                }

            }

        }

        #endregion

    }

}
