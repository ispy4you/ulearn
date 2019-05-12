using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace rocket_bot
{
    public class Channel<T> where T : class
    {
        public List<T> Chan;

        public Channel()
        {
            Chan = new List<T>();
        }

        /// <summary>
        /// Возвращает элемент по индексу или null, если такого элемента нет.
        /// При присвоении удаляет все элементы после.
        /// Если индекс в точности равен размеру коллекции, работает как Append.
        /// </summary>
        public T this[int index]
        {
            get
            {
                lock (Chan)
                {
                    if (Chan.Count() <= index) return null;
                    return Chan[index];
                }
            }
            set
            {
            }
        }

        /// <summary>
        /// Возвращает последний элемент или null, если такого элемента нет
        /// </summary>
        public T LastItem()
        {
            lock (Chan)
            {
                try
                {
                    Chan.Last();
                }
                catch (InvalidOperationException)
                {
                    return null;
                }
                return Chan.Last();
            }
        }

        /// <summary>
        /// Добавляет item в конец только если lastItem является последним элементом
        /// </summary>
        public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
        {
            lock (Chan)
            {
                if (Chan.Count() == 0) return;
                if (Equals(Chan.Last(), knownLastItem))
                    Chan.Add(item);
            }
        }

        /// <summary>
        /// Возвращает количество элементов в коллекции
        /// </summary>
        public int Count
        {
            get
            {
                lock (Chan)
                {
                    return Chan.Count();
                }
            }
        }
    }
}