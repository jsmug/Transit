using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;

namespace Transit.Core.Internal
{

    internal sealed class PropertyObserver<TPropertySource> : IWeakEventListener where TPropertySource : INotifyPropertyChanged
    {

        private readonly WeakReference _propertySource;
        private readonly IDictionary<string, Action<TPropertySource>> _propertySourceToHandlerMap = new Dictionary<string, Action<TPropertySource>>();


        private PropertyObserver()
        {
        }

        internal PropertyObserver(TPropertySource propertySource)
        {

            if (propertySource == null)
            {
                throw new ArgumentNullException("propertySource");
            }

            this._propertySource = new WeakReference(propertySource);

        }


        #region public

        public PropertyObserver<TPropertySource> RegisterHandler(Expression<Func<TPropertySource, object>> expression, Action<TPropertySource> handler)
        {

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }

            string propertyName = this.GetPropertyName(expression);
            TPropertySource propertySource;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("The expression did not provide a property name.", "expression");
            }

            propertySource = this.GetPropertySource();

            if (propertySource != null)
            {

                this._propertySourceToHandlerMap[propertyName] = handler;
                PropertyChangedEventManager.AddListener(propertySource, this, propertyName);

            }

            return this;

        }

        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> expression)
        {

            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            string propertyName = this.GetPropertyName(expression);
            TPropertySource propertySource;

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentException("The expression did not provide a property name.", "expression");
            }

            propertySource = this.GetPropertySource();

            if (propertySource != null)
            {

                if (this._propertySourceToHandlerMap.ContainsKey(propertyName))
                {

                    this._propertySourceToHandlerMap.Remove(propertyName);
                    PropertyChangedEventManager.RemoveListener(propertySource, this, propertyName);

                }

            }

            return this;
        }

        #endregion

        #region IWeakEventListener

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {

            string propertyName = default(string);
            TPropertySource propertySource;
            Action<TPropertySource> handler = null;
            List<Action<TPropertySource>> actions = null;
            bool result = false;

            if (managerType == typeof(PropertyChangedEventManager))
            {

                propertyName = ((PropertyChangedEventArgs)e).PropertyName;
                propertySource = (TPropertySource)sender;

                if (string.IsNullOrWhiteSpace(propertyName))
                {

                    actions = this._propertySourceToHandlerMap.Values.ToList();

                    foreach (Action<TPropertySource> action in actions)
                    {
                        action(propertySource);
                    }

                    result = true;

                }
                else
                {

                    if (this._propertySourceToHandlerMap.TryGetValue(propertyName, out handler))
                    {

                        handler(propertySource);
                        result = true;

                    }

                }

            }

            return result;

        }

        #endregion

        #region private

        private string GetPropertyName(Expression<Func<TPropertySource, object>> expression)
        {

            LambdaExpression lambda = expression as LambdaExpression;
            MemberExpression memberExpression = null;
            UnaryExpression unaryExpression = null;
            string propertyName = default(string);

            if (lambda.Body is UnaryExpression)
            {
                unaryExpression = lambda.Body as UnaryExpression;
                memberExpression = unaryExpression.Operand as MemberExpression;

            }
            else
            {
                memberExpression = lambda.Body as MemberExpression;
            }

            if (memberExpression != null)
            {
                propertyName = memberExpression.Member.Name;
            }

            return propertyName;

        }

        private TPropertySource GetPropertySource()
        {

            TPropertySource source;

            try
            {
                source = (TPropertySource)this._propertySource.Target;
            }
            catch
            {
                source = default(TPropertySource);
            }

            return source;

        }

        #endregion

    }

}
