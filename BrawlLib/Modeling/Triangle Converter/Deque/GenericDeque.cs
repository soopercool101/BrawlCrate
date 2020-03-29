#region License

/* Copyright (c) 2006 Leslie Sanford
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy 
 * of this software and associated documentation files (the "Software"), to 
 * deal in the Software without restriction, including without limitation the 
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or 
 * sell copies of the Software, and to permit persons to whom the Software is 
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

#endregion

#region Contact

/*
 * Leslie Sanford
 * Email: jabberdabber@hotmail.com
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace BrawlLib.Modeling.Triangle_Converter.Deque
{
    /// <summary>
    /// Represents a simple double-ended-queue collection of objects.
    /// </summary>
    [Serializable()]
    public partial class Deque<T> : ICollection, IEnumerable<T>, ICloneable
    {
        #region Deque Members

        #region Fields

        // The node at the front of the deque.
        public Node front;

        // The node at the back of the deque.
        public Node back;

        // The number of elements in the deque.
        private int count;

        // The version of the deque.
        private long version;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the Deque class.
        /// </summary>
        public Deque()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Deque class that contains 
        /// elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">
        /// The collection whose elements are copied to the new Deque.
        /// </param>
        public Deque(IEnumerable<T> collection)
        {
            #region Require

            if (collection == null)
            {
                throw new ArgumentNullException("col");
            }

            #endregion

            foreach (T item in collection)
            {
                PushBack(item);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets an item in the Deque.
        /// </summary>
        public T this[uint index]
        {
            get
            {
                if (index >= count || front == null)
                {
                    return default(T);
                }

                Node c = front;
                for (int i = 0; i < index; i++)
                {
                    c = front.Next;
                }

                return c.Value;
            }
            set
            {
                if (index >= count || front == null)
                {
                    return;
                }

                Node c = front;
                for (int i = 0; i < index; i++)
                {
                    c = front.Next;
                }

                c.Value = value;

                version++;
            }
        }

        /// <summary>
        /// Resizes the Deque.
        /// </summary>
        internal void Resize(uint Size, T defaultValue)
        {
            count = 0;
            back = null;

            if (Size <= 0)
            {
                front = null;
                return;
            }

            Node f = front;
            if (f == null)
            {
                f = front = new Node(defaultValue);
            }

            for (int i = 0; i < Size; i++)
            {
                if (f.Next == null)
                {
                    f.Next = new Node(defaultValue) {Previous = f};
                }

                f = f.Next;
                count++;
            }

            if (f != null)
            {
                back = f;
                f.Previous.Next = null;
            }

            version++;

            #region Invariant

            AssertValid();

            #endregion
        }

        /// <summary>
        /// Removes all objects from the Deque.
        /// </summary>
        public virtual void Clear()
        {
            count = 0;

            front = back = null;

            version++;

            #region Invariant

            AssertValid();

            #endregion
        }

        /// <summary>
        /// Determines whether or not an element is in the Deque.
        /// </summary>
        /// <param name="obj">
        /// The Object to locate in the Deque.
        /// </param>
        /// <returns>
        /// <b>true</b> if <i>obj</i> if found in the Deque; otherwise, 
        /// <b>false</b>.
        /// </returns>
        public virtual bool Contains(T obj)
        {
            foreach (T o in this)
            {
                if (EqualityComparer<T>.Default.Equals(o, obj))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Inserts an object at the front of the Deque.
        /// </summary>
        /// <param name="item">
        /// The object to push onto the deque;
        /// </param>
        public virtual void PushFront(T item)
        {
            // The new node to add to the front of the deque.
            Node newNode = new Node(item)
            {
                // Link the new node to the front node. The current front node at 
                // the front of the deque is now the second node in the deque.
                Next = front
            };

            // If the deque isn't empty.
            if (Count > 0)
            {
                // Link the current front to the new node.
                front.Previous = newNode;
            }

            // Make the new node the front of the deque.
            front = newNode;

            // Keep track of the number of elements in the deque.
            count++;

            // If this is the first element in the deque.
            if (Count == 1)
            {
                // The front and back nodes are the same.
                back = front;
            }

            version++;

            #region Invariant

            AssertValid();

            #endregion
        }

        /// <summary>
        /// Inserts an object at the back of the Deque.
        /// </summary>
        /// <param name="item">
        /// The object to push onto the deque;
        /// </param>
        public virtual void PushBack(T item)
        {
            // The new node to add to the back of the deque.
            Node newNode = new Node(item)
            {
                // Link the new node to the back node. The current back node at 
                // the back of the deque is now the second to the last node in the
                // deque.
                Previous = back
            };

            // If the deque is not empty.
            if (Count > 0)
            {
                // Link the current back node to the new node.
                back.Next = newNode;
            }

            // Make the new node the back of the deque.
            back = newNode;

            // Keep track of the number of elements in the deque.
            count++;

            // If this is the first element in the deque.
            if (Count == 1)
            {
                // The front and back nodes are the same.
                front = back;
            }

            version++;

            #region Invariant

            AssertValid();

            #endregion
        }

        /// <summary>
        /// Removes and returns the object at the front of the Deque.
        /// </summary>
        /// <returns>
        /// The object at the front of the Deque.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The Deque is empty.
        /// </exception>
        public virtual T PopFront()
        {
            #region Require

            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty.");
            }

            #endregion

            // Get the object at the front of the deque.
            T item = front.Value;

            // Move the front back one node.
            front = front.Next;

            // Keep track of the number of nodes in the deque.
            count--;

            // If the deque is not empty.
            if (Count > 0)
            {
                // Tie off the previous link in the front node.
                front.Previous = null;
            }
            // Else the deque is empty.
            else
            {
                // Indicate that there is no back node.
                back = null;
            }

            version++;

            #region Invariant

            AssertValid();

            #endregion

            return item;
        }

        /// <summary>
        /// Removes and returns the object at the back of the Deque.
        /// </summary>
        /// <returns>
        /// The object at the back of the Deque.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The Deque is empty.
        /// </exception>
        public virtual T PopBack()
        {
            #region Require

            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty.");
            }

            #endregion

            // Get the object at the back of the deque.
            T item = back.Value;

            // Move back node forward one node.
            back = back.Previous;

            // Keep track of the number of nodes in the deque.
            count--;

            // If the deque is not empty.
            if (Count > 0)
            {
                // Tie off the next link in the back node.
                back.Next = null;
            }
            // Else the deque is empty.
            else
            {
                // Indicate that there is no front node.
                front = null;
            }

            version++;

            #region Invariant

            AssertValid();

            #endregion

            return item;
        }

        /// <summary>
        /// Returns the object at the front of the Deque without removing it.
        /// </summary>
        /// <returns>
        /// The object at the front of the Deque.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The Deque is empty.
        /// </exception>
        public virtual T PeekFront()
        {
            #region Require

            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty.");
            }

            #endregion

            return front.Value;
        }

        /// <summary>
        /// Returns the object at the back of the Deque without removing it.
        /// </summary>
        /// <returns>
        /// The object at the back of the Deque.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The Deque is empty.
        /// </exception>
        public virtual T PeekBack()
        {
            #region Require

            if (Count == 0)
            {
                throw new InvalidOperationException("Deque is empty.");
            }

            #endregion

            return back.Value;
        }

        /// <summary>
        /// Copies the Deque to a new array.
        /// </summary>
        /// <returns>
        /// A new array containing copies of the elements of the Deque.
        /// </returns>
        public virtual T[] ToArray()
        {
            T[] array = new T[Count];
            int index = 0;

            foreach (T item in this)
            {
                array[index] = item;
                index++;
            }

            return array;
        }

        /// <summary>
        /// Returns a synchronized (thread-safe) wrapper for the Deque.
        /// </summary>
        /// <param name="deque">
        /// The Deque to synchronize.
        /// </param>
        /// <returns>
        /// A synchronized wrapper around the Deque.
        /// </returns>
        public static Deque<T> Synchronized(Deque<T> deque)
        {
            #region Require

            if (deque == null)
            {
                throw new ArgumentNullException("deque");
            }

            #endregion

            return new SynchronizedDeque(deque);
        }

        [Conditional("DEBUG")]
        private void AssertValid()
        {
            int n = 0;
            Node current = front;

            while (current != null)
            {
                n++;
                current = current.Next;
            }

            Debug.Assert(n == Count);

            if (Count > 0)
            {
                Debug.Assert(front != null && back != null, "Front/Back Null Test - Count > 0");

                Node f = front;
                Node b = back;

                while (f.Next != null && b.Previous != null)
                {
                    f = f.Next;
                    b = b.Previous;
                }

                Debug.Assert(f.Next == null && b.Previous == null, "Front/Back Termination Test");
                Debug.Assert(f == back && b == front, "Front/Back Equality Test");
            }
            else
            {
                Debug.Assert(front == null && back == null, "Front/Back Null Test - Count == 0");
            }
        }

        #endregion

        #endregion

        #region ICollection Members

        /// <summary>
        /// Gets a value indicating whether access to the Deque is synchronized 
        /// (thread-safe).
        /// </summary>
        public virtual bool IsSynchronized => false;

        /// <summary>
        /// Gets the number of elements contained in the Deque.
        /// </summary>
        public virtual int Count => count;

        /// <summary>
        /// Copies the Deque elements to an existing one-dimensional Array, 
        /// starting at the specified array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional Array that is the destination of the elements 
        /// copied from Deque. The Array must have zero-based indexing. 
        /// </param>
        /// <param name="index">
        /// The zero-based index in array at which copying begins. 
        /// </param>
        public virtual void CopyTo(Array array, int index)
        {
            #region Require

            if (array == null)
            {
                throw new ArgumentNullException("array");
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", index,
                    "Index is less than zero.");
            }

            if (array.Rank > 1)
            {
                throw new ArgumentException("Array is multidimensional.");
            }

            if (index >= array.Length)
            {
                throw new ArgumentException("Index is equal to or greater " +
                                            "than the length of array.");
            }

            if (Count > array.Length - index)
            {
                throw new ArgumentException(
                    "The number of elements in the source Deque is greater " +
                    "than the available space from index to the end of the " +
                    "destination array.");
            }

            #endregion

            int i = index;

            foreach (object obj in this)
            {
                array.SetValue(obj, i);
                i++;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the Deque.
        /// </summary>
        public virtual object SyncRoot => this;

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that can iterate through the Deque.
        /// </summary>
        /// <returns>
        /// An IEnumerator for the Deque.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        #region ICloneable Members

        /// <summary>
        /// Creates a shallow copy of the Deque.
        /// </summary>
        /// <returns>
        /// A shallow copy of the Deque.
        /// </returns>
        public virtual object Clone()
        {
            Deque<T> clone = new Deque<T>(this)
            {
                version = version
            };

            return clone;
        }

        #endregion

        #region IEnumerable<T> Members

        public virtual IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }

        #endregion

        internal void Sort(Comparison<T> comp)
        {
            Top:
            bool f = true;

            Node next = front, prev = front;
            while (next != null)
            {
                if (f)
                {
                    next = next.Next;
                    f = false;
                    continue;
                }

                int v = comp(prev.Value, next.Value);
                if (v > 0)
                {
                    next.Previous = prev.Previous;
                    prev.Previous = next;
                    prev.Next = next.Next;
                    next.Next = prev;
                    goto Top;
                }
            }
        }
    }
}