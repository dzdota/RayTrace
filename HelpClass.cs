using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yurchuk._2sem.laba._4
{
    class MyList<T> : List<T>
    {
        public event EventHandler OnAdd;
        internal new void Add(T item)
        {
            OnAdd?.Invoke(item, null);
            /*if (null != OnAdd)
            {
                OnAdd(item, null);
            }*/
            base.Add(item);
        }
    }
}
