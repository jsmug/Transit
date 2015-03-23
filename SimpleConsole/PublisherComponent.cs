using Transit.Core;

namespace SimpleConsole
{

    public class PublisherComponent : Component
    {

        private int _count;


        public PublisherComponent() : base("Counter")
        {
            this._count = -1;
        }


        #region public

        [RouteOut]
        public int Count
        {

            get
            {
                return this._count;
            }

            set
            {
                SetValue(ref this._count, value, x => this.Count);
            }

        }

        #endregion

    }

}
