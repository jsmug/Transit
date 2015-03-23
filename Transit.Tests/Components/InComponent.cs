using Transit.Core;
using Transit.Tests.Events;

namespace Transit.Tests.Components
{
    
    public class InComponent : Component
    {

        public InComponent() : this("InComponent")
        {
        }

        public InComponent(string name) : base(name)
        {
        }


        #region public

        [RouteIn]
        public int IntInProperty { get; set; }

        [RouteIn]
        public string StringInProperty { get; set; }

        [RouteIn]
        public TestEventArgs TestEventArgsInProperty { get; set; }


        [RouteIn]
        public void IntActionIn(int input)
        {

            this.IntInProperty = input;
            System.Diagnostics.Trace.WriteLine(string.Format("IntAction was called with value: {0}", input));

        }

        [RouteIn]
        public void StringActionIn(string input)
        {

            this.StringInProperty = input;
            System.Diagnostics.Trace.WriteLine(string.Format("StringAction was called with value: {0}", input));

        }

        [RouteIn]
        public void TestEventArgsActionIn(TestEventArgs input)
        {

            this.IntInProperty = input.EventInteger;
            this.StringInProperty = input.EventString;

            System.Diagnostics.Trace.WriteLine(string.Format("TestEventArgsAction was called with value: {0}", input));

        }

        #endregion

    }

}
