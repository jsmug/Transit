using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using Transit.Core;
using Transit.Tests.Components;
using Transit.Tests.Events;

namespace Transit.Tests
{

    [TestClass]
    public class PackageTests
    {

        public PackageTests()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }


        [TestMethod]
        public void CanSetPackageName()
        {

            string name = "Package1";
            Package package = new Package(name);

            Assert.IsTrue(string.Compare(name, package.Name, false) == 0);

        }

        [TestMethod]
        public void CanUnregisterAllRoutes()
        {

            Package pkg = new Package();
            OutComponent outComponent = new OutComponent();
            InComponent inComponent = new InComponent();
            EventToMethodRoute eRoute = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
            PropertyToMethodRoute pRoute = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));

            pkg.RegisterEventRoute(eRoute);
            pkg.RegisterPropertyRoute(pRoute);

            pkg.UnregisterAll();

            Assert.IsTrue(!pkg.RegisteredEventRoutes.Any() & !pkg.RegisteredPropertyRoutes.Any());

        }

        [TestMethod]
        public void CanUnregisterEventRoute()
        {

            Package pkg = new Package();
            OutComponent outComponent = new OutComponent();
            InComponent inComponent = new InComponent();
            EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));

            pkg.RegisterEventRoute(route);
            pkg.UnregisterEventRoute(route);

            Assert.IsTrue(!pkg.RegisteredEventRoutes.Any());

        }

        [TestMethod]
        public void CanUnregisterPropertyRoute()
        {

            Package pkg = new Package();
            OutComponent outComponent = new OutComponent();
            InComponent inComponent = new InComponent();
            PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));

            pkg.RegisterPropertyRoute(route);
            pkg.UnregisterPropertyRoute(route);

            Assert.IsTrue(!pkg.RegisteredPropertyRoutes.Any());

        }

        [TestMethod]
        public void DuplicateEventRoutesDisalllowed()
        {

            Package pkg = new Package();
            OutComponent outComponent = new OutComponent();
            InComponent inComponent = new InComponent();
            EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));

            pkg.RegisterEventRoute(route);
            pkg.RegisterEventRoute(route);

            Assert.AreEqual(pkg.RegisteredEventRoutes.Count(), 1);

            pkg.UnregisterEventRoute(route);

        }

        [TestMethod]
        public void DuplicatePropertyRoutesDisalllowed()
        {

            Package pkg = new Package();
            OutComponent outComponent = new OutComponent();
            InComponent inComponent = new InComponent();
            PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));

            pkg.RegisterPropertyRoute(route);
            pkg.RegisterPropertyRoute(route);

            Assert.AreEqual(pkg.RegisteredPropertyRoutes.Count(), 1);

            pkg.UnregisterPropertyRoute(route);

        }

        [TestMethod]
        public void HasDefaultName()
        {

            Package package = new Package();
            Assert.IsTrue(package.Name.Length > 0);

        }


        #region Components Test

        [TestClass]
        public class ComponentsTest
        {

            public ComponentsTest()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }


            [TestMethod]
            public void ComponentsRegistration()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
                
                pkg.RegisterEventRoute(route);
                Assert.AreEqual<int>(2, pkg.Components.Count());

                pkg.UnregisterEventRoute(route);
                Assert.AreEqual<int>(0, pkg.Components.Count());

            }

        }
    
        #endregion

        #region Route Type Tests

        [TestClass]
        public class EventRouteTests
        {

            public EventRouteTests()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }


            #region public

            [TestMethod]
            public void CanRegisterEventToMethodRoute()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));

                pkg.RegisterEventRoute(route);
                Assert.IsTrue(pkg.RegisteredEventRoutes.Any());

                pkg.UnregisterEventRoute(route);

            }

            [TestMethod]
            public void CanRegisterEventToPropertyRoute()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToPropertyRoute route = EventToPropertyRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, x => inComponent.TestEventArgsInProperty);

                pkg.RegisterEventRoute(route);
                Assert.IsTrue(pkg.RegisteredEventRoutes.Any());

                pkg.UnregisterEventRoute(route);

            }

            #endregion

        }

        [TestClass]
        public class PropertyRouteTests
        {

            public PropertyRouteTests()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }


            #region public

            [TestMethod]
            public void CanRegisterPropertyToMethodRoute()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));

                pkg.RegisterPropertyRoute(route);
                Assert.IsTrue(pkg.RegisteredPropertyRoutes.Any());

                pkg.UnregisterPropertyRoute(route);

            }

            [TestMethod]
            public void CanRegisterPropertyToPropertyRoute()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToPropertyRoute route = PropertyToPropertyRoute.Create(outComponent, x => outComponent.StringOutProperty, inComponent, x => inComponent.StringInProperty);

                pkg.RegisterPropertyRoute(route);
                Assert.IsTrue(pkg.RegisteredPropertyRoutes.Any());

                pkg.UnregisterPropertyRoute(route);

            }

            #endregion

        }

        #endregion

        #region Event Tests

        [TestClass]
        public class EventsTest
        {

            public EventsTest()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }


            [TestMethod]
            public void CanAttachToEventRouteRegistered()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
                bool caught = false;

                pkg.EventRouteRegistered += (s, e) =>
                {
                    caught = e.EventRoute == route;
                };

                pkg.RegisterEventRoute(route);
                pkg.UnregisterEventRoute(route);

                Assert.IsTrue(caught);

            }

            [TestMethod]
            public void CanAttachToEventRouteUnregistered()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
                bool caught = false;

                pkg.EventRouteUnregistered += (s, e) =>
                {
                    caught = e.EventRoute == route;
                };

                pkg.RegisterEventRoute(route);
                pkg.UnregisterEventRoute(route);

                Assert.IsTrue(caught);

            }

            [TestMethod]
            public void CanAttachToPropertyRouteRegistered()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));
                bool caught = false;

                pkg.PropertyRouteRegistered += (s, e) =>
                {
                    caught = e.PropertyRoute == route;
                };

                pkg.RegisterPropertyRoute(route);
                pkg.UnregisterPropertyRoute(route);

                Assert.IsTrue(caught);

            }

            [TestMethod]
            public void CanAttachToPropertyRouteUnregistered()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));
                bool caught = false;

                pkg.PropertyRouteUnregistered += (s, e) =>
                {
                    caught = e.PropertyRoute == route;
                };

                pkg.RegisterPropertyRoute(route);
                pkg.UnregisterPropertyRoute(route);

                Assert.IsTrue(caught);

            }

            [TestMethod]
            public void CanCancelEventRouteRegistration()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));

                pkg.EventRoutePreRegister += (s, e) =>
                {
                    e.Cancel = true;
                };

                pkg.RegisterEventRoute(route);
                Assert.IsTrue(!pkg.RegisteredEventRoutes.Any());

            }

            [TestMethod]
            public void CanCancelEventRouteUnregistration()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
                bool shouldCancel = true;

                pkg.EventRoutePreUnregister += (s, e) =>
                {

                    e.Cancel = shouldCancel;
                    shouldCancel = false;

                };

                pkg.RegisterEventRoute(route);
                pkg.UnregisterEventRoute(route);
                Assert.IsTrue(pkg.RegisteredEventRoutes.Any());
                pkg.UnregisterEventRoute(route);

            }

            [TestMethod]
            public void CanCancelPropertyRouteRegistration()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));

                pkg.PropertyRoutePreRegister += (s, e) =>
                {
                    e.Cancel = true;
                };

                pkg.RegisterPropertyRoute(route);
                Assert.IsTrue(!pkg.RegisteredPropertyRoutes.Any());

            }

            [TestMethod]
            public void CanCancelPropertyRouteUnregistration()
            {

                Package pkg = new Package();
                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, x => outComponent.StringOutProperty, inComponent, (x) => inComponent.StringActionIn(x));
                bool shouldCancel = true;

                pkg.PropertyRoutePreUnregister += (s, e) =>
                {

                    e.Cancel = shouldCancel;
                    shouldCancel = false;

                };

                pkg.RegisterPropertyRoute(route);
                pkg.UnregisterPropertyRoute(route);
                Assert.IsTrue(pkg.RegisteredPropertyRoutes.Any());
                pkg.UnregisterPropertyRoute(route);

            }

        }

        #endregion

    }

}
