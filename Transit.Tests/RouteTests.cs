using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Transit.Core;
using Transit.Tests.Components;
using Transit.Tests.Converters;
using Transit.Tests.Events;

namespace Transit.Tests
{

    [TestClass]
    public class RouteTests
    {

        public RouteTests()
        {
        }

        [TestClass]
        public class EventToMethodRouteTests
        {

            #region public

            [TestMethod]
            public void CanRegisterEventArgsRoute()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);

                EventToMethodRoute route = EventToMethodRoute.Create<TestEventArgs>(outComponent, "TestEventOut", inComponent, (x) => inComponent.TestEventArgsActionIn(x));
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");               
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.AreEqual(inComponent.IntInProperty, e.EventInteger);

            }

            [TestMethod]
            public void CanRegisterEventArgsRouteWithReferenceConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);
                TestEventArgsToStringRouteConverter converter = new TestEventArgsToStringRouteConverter();

                EventToMethodRoute route = EventToMethodRoute.Create<string>(outComponent, "TestEventOut", inComponent, (x) => inComponent.StringActionIn(x), converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.IsTrue(string.Compare(e.EventString, inComponent.StringInProperty, false) == 0);

            }

            [TestMethod]
            public void CanRegisterEventArgsRouteWithValueConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);
                TestEventArgsToIntRouteConverter converter = new TestEventArgsToIntRouteConverter();

                EventToMethodRoute route = EventToMethodRoute.Create<int>(outComponent, "TestEventOut", inComponent, (x) => inComponent.IntActionIn(x), converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.AreEqual(inComponent.IntInProperty, e.EventInteger);

            }

            [ClassInitialize()]
            public static void Initialize(TestContext testContext) 
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            #endregion

        }

        [TestClass]
        public class EventToPropertyRouteTests
        {

            #region public

            [TestMethod]
            public void CanRegisterEventArgsRoute()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);

                EventToPropertyRoute route = EventToPropertyRoute.Create(outComponent, "TestEventOut", inComponent, x => inComponent.TestEventArgsInProperty);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.AreEqual(inComponent.TestEventArgsInProperty.EventInteger, e.EventInteger);

            }

            [TestMethod]
            public void CanRegisterEventArgsRouteWithReferenceConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);
                TestEventArgsToStringRouteConverter converter = new TestEventArgsToStringRouteConverter();

                EventToPropertyRoute route = EventToPropertyRoute.Create(outComponent, "TestEventOut", inComponent, x => inComponent.StringInProperty, converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.IsTrue(string.Compare(e.EventString, inComponent.StringInProperty, false) == 0);

            }

            [TestMethod]
            public void CanRegisterEventArgsRouteWithValueConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                TestEventArgs e = new TestEventArgs("RouteOut", 100);
                TestEventArgsToIntRouteConverter converter = new TestEventArgsToIntRouteConverter();

                EventToPropertyRoute route = EventToPropertyRoute.Create(outComponent, "TestEventOut", inComponent, x => inComponent.IntInProperty, converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.RaiseTestEventOut(e);
                route.UnregisterRoute();

                Assert.AreEqual(inComponent.IntInProperty, e.EventInteger);

            }

            [ClassInitialize()]
            public static void Initialize(TestContext testContext)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            #endregion

        }

        [TestClass]
        public class PropertyToMethodRouteTests
        {

            #region public

            [TestMethod]
            public void CanRegisterPropertyRoute()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();

                PropertyToMethodRoute route = PropertyToMethodRoute.Create(outComponent, x => outComponent.IntOutProperty, inComponent, (x) => inComponent.IntActionIn(x));
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.IntOutProperty = 100;
                route.UnregisterRoute();

                Assert.AreEqual(outComponent.IntOutProperty, inComponent.IntInProperty);

            }

            [TestMethod]
            public void CanRegisterPropertyRouteWithReferenceConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                IntToStringRouteConverter converter = new IntToStringRouteConverter();

                PropertyToMethodRoute route = PropertyToMethodRoute.Create<string>(outComponent, "IntOutProperty", inComponent, (x) => inComponent.StringActionIn(x), converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.IntOutProperty = 100;
                route.UnregisterRoute();

                Assert.IsTrue(string.Compare(outComponent.IntOutProperty.ToString(), inComponent.StringInProperty, false) == 0);

            }

            [TestMethod]
            public void CanRegisterPropertyRouteWithValueConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                StringToIntRouteConverter converter = new StringToIntRouteConverter();

                PropertyToMethodRoute route = PropertyToMethodRoute.Create<int>(outComponent, "StringOutProperty", inComponent, (x) => inComponent.IntActionIn(x), converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.StringOutProperty = "100";
                route.UnregisterRoute();

                Assert.AreEqual(100, inComponent.IntInProperty);

            }

            [ClassInitialize()]
            public static void Initialize(TestContext testContext)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            #endregion

        }

        [TestClass]
        public class PropertyToPropertyRouteTests
        {

            #region public

            [TestMethod]
            public void CanRegisterPropertyRoute()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();

                PropertyToPropertyRoute route = PropertyToPropertyRoute.Create(outComponent, x => outComponent.IntOutProperty, inComponent, x => inComponent.IntInProperty);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.IntOutProperty = 100;
                route.UnregisterRoute();

                Assert.AreEqual(outComponent.IntOutProperty, inComponent.IntInProperty);

            }

            [TestMethod]
            public void CanRegisterPropertyRouteWithReferenceConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                IntToStringRouteConverter converter = new IntToStringRouteConverter();

                PropertyToPropertyRoute route = PropertyToPropertyRoute.Create<string>(outComponent, "IntOutProperty", inComponent, x => inComponent.StringInProperty, converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.IntOutProperty = 100;
                route.UnregisterRoute();

                Assert.IsTrue(string.Compare(outComponent.IntOutProperty.ToString(), inComponent.StringInProperty, false) == 0);

            }

            [TestMethod]
            public void CanRegisterPropertyRouteWithValueConverter()
            {

                OutComponent outComponent = new OutComponent();
                InComponent inComponent = new InComponent();
                StringToIntRouteConverter converter = new StringToIntRouteConverter();

                PropertyToPropertyRoute route = PropertyToPropertyRoute.Create<int>(outComponent, "StringOutProperty", inComponent, x => inComponent.IntInProperty, converter);
                route.RegisterRoute();

                if (!route.IsRegistered)
                {
                    Assert.Fail("Route not registered.");
                }

                outComponent.StringOutProperty = "100";
                route.UnregisterRoute();

                Assert.AreEqual(100, inComponent.IntInProperty);

            }

            [ClassInitialize()]
            public static void Initialize(TestContext testContext)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            #endregion

        }

    }

}
