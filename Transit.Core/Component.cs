using System;
using Transit.Core.Common;

namespace Transit.Core
{
    
    public class Component : ObservableObject, IEquatable<Component>
    {


        #region constants

        private const int IncrementHashValue = 23;
        private const int InitialHashValue = 17;

        #endregion


        private readonly Guid _id = Guid.NewGuid();
        private string _name;


        private Component()
        {
        }

        protected Component(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            this._name = name;

        }


        #region public

        public Guid Id
        {

            get
            {
                return this._id;
            }

        }

        public string Name
        {

            get
            {
                return this._name;
            }

        }


        public override bool Equals(object obj)
        {
            return Equals(obj as Component);
        }

        public bool Equals(Component other)
        {
            return other == null ? false : ReferenceEquals(this, other) ? true : this._id.Equals(other.Id);
        }

        public override int GetHashCode()
        {

            unchecked
            {

                int hash = Component.InitialHashValue;
                hash = hash * Component.IncrementHashValue + this._id.GetHashCode();

                return hash;

            }

        }

        #endregion

    }

}
