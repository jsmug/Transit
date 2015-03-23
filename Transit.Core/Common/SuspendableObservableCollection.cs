using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;

namespace Transit.Core.Common
{
    
    public class SuspendableObservableCollection<T> : ObservableCollection<T>, ISuspendable
    {

        private readonly TaskScheduler _context;
        private readonly object _suspendLock = new object();
        private bool _suspendNotifications;


        public SuspendableObservableCollection() : base()
        {

            if (SynchronizationContext.Current == null)
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            }

            this._context = TaskScheduler.FromCurrentSynchronizationContext();

        }

        public SuspendableObservableCollection(IEnumerable<T> collection) : base(collection)
        {
        }

        public SuspendableObservableCollection(IList<T> list) : base(list)
        {
        }


        #region public

        public void AddRange(IEnumerable<T> items)
        {

            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (T item in items)
            {
                this.Items.Add(item);
            }

        }

        public void RemoveRange(IEnumerable<T> items)
        {

            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            foreach (T item in items)
            {
                this.Items.Remove(item);
            }

        }

        public void Resume()
        {

            lock (this._suspendLock)
            {

                this._suspendNotifications = false;
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

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

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            using (BlockReentrancy())
            {

                if (!this._suspendNotifications)
                {
                    base.OnCollectionChanged(e);
                }

            }

        }

        #endregion

    }

}
