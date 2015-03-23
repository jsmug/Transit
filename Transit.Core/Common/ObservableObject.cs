using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using Transit.Core.ExtensionMethods;

namespace Transit.Core.Common
{
    
    public abstract class ObservableObject : INotifyPropertyChanged, ISuspendable
    {

        private readonly object _suspendLock = new object();
        private bool _suspendNotifications;


        public event PropertyChangedEventHandler PropertyChanged;


        protected ObservableObject()
        {
        }


        #region public

        public void Resume()
        {

            lock (this._suspendLock)
            {
                this._suspendNotifications = false;
            }

        }

        public void Suspend()
        {

            lock (this._suspendLock)
            {
                this._suspendNotifications = true;
            }

        }

        #endregion

        #region protected

        protected virtual void OnPropertyChanged<TExpression>(Expression<Func<ObservableObject, TExpression>> propertyExpression)
        {

            if (propertyExpression == null)
            {
                throw new ArgumentNullException("propertyExpression");
            }

            OnPropertyChanged(this.GetPropertySymbol(propertyExpression));

        }

        protected virtual void OnPropertyChanged(string propertyName)
        {

            lock (this._suspendLock)
            {

                if (!this._suspendNotifications && this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }

            }

        }

        protected bool SetValue<T>(ref T property, T value, Expression<Func<ObservableObject, T>> propertyExpression)
        {
            return SetValue(ref property, value, propertyExpression, false);
        }

        protected bool SetValue<T>(ref T property, T value, Expression<Func<ObservableObject, T>> propertyExpression, bool force)
        {

            bool set = false;

            if (!EqualityComparer<T>.Default.Equals(property, value) | force)
            {

                property = value;
                OnPropertyChanged(propertyExpression);

                set = true;

            }

            return set;

        }

        #endregion

    }

}
