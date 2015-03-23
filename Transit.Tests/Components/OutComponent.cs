using System;
using Transit.Core;
using Transit.Tests.Events;

namespace Transit.Tests.Components
{

    public class OutComponent : Component
    {

        private int _intOut;
        private Random _random;
        private string _stringOut;


        [RouteOut]
        public event EventHandler<TestEventArgs> TestEventOut = delegate { };


        public OutComponent(): this("OutComponent")
        {
        }

        public OutComponent(string name) : base(name)
        {
            this._random = new Random(Environment.TickCount);
        }


        [RouteOut]
        public int IntOutProperty
        {

            get
            {
                return this._intOut;
            }

            set
            {
                SetValue(ref this._intOut, value, x => this.IntOutProperty);
            }

        }

        [RouteOut]
        public string StringOutProperty
        {

            get
            {
                return this._stringOut;
            }

            set
            {
                SetValue(ref this._stringOut, value, x => this.StringOutProperty);
            }

        }


        public void RaiseTestEventOut()
        {
            TestEventOut(this, new TestEventArgs("Event raised from FireTestEventOut.", this._random.Next()));
        }

        public void RaiseTestEventOut(TestEventArgs e)
        {
            TestEventOut(this, e);
        }

    }

}
