using System;
using System.Collections.Generic;

namespace BrawlLib.Modeling.Triangle_Converter.Deque
{
    public partial class Deque<T>
    {
        #region Enumerator Class

        [Serializable()]
        private class Enumerator : IEnumerator<T>
        {
            private readonly Deque<T> owner;

            private Node currentNode;

            private T current;

            private bool moveResult;

            private readonly long version;

            // A value indicating whether the enumerator has been disposed.
            private bool disposed;

            public Enumerator(Deque<T> owner)
            {
                this.owner = owner;
                currentNode = owner.front;
                version = owner.version;
            }

            #region IEnumerator Members

            public void Reset()
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                if (version != owner.version)
                {
                    throw new InvalidOperationException(
                        "The Deque was modified after the enumerator was created.");
                }

                #endregion

                currentNode = owner.front;
                moveResult = false;
            }

            public object Current
            {
                get
                {
                    #region Require

                    if (disposed)
                    {
                        throw new ObjectDisposedException(GetType().Name);
                    }

                    if (!moveResult)
                    {
                        throw new InvalidOperationException(
                            "The enumerator is positioned before the first " +
                            "element of the Deque or after the last element.");
                    }

                    #endregion

                    return current;
                }
            }

            public bool MoveNext()
            {
                #region Require

                if (disposed)
                {
                    throw new ObjectDisposedException(GetType().Name);
                }

                if (version != owner.version)
                {
                    throw new InvalidOperationException(
                        "The Deque was modified after the enumerator was created.");
                }

                #endregion

                if (currentNode != null)
                {
                    current = currentNode.Value;
                    currentNode = currentNode.Next;

                    moveResult = true;
                }
                else
                {
                    moveResult = false;
                }

                return moveResult;
            }

            #endregion

            #region IEnumerator<T> Members

            T IEnumerator<T>.Current
            {
                get
                {
                    #region Require

                    if (disposed)
                    {
                        throw new ObjectDisposedException(GetType().Name);
                    }

                    if (!moveResult)
                    {
                        throw new InvalidOperationException(
                            "The enumerator is positioned before the first " +
                            "element of the Deque or after the last element.");
                    }

                    #endregion

                    return current;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                disposed = true;
            }

            #endregion
        }

        #endregion
    }
}