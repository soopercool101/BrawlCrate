namespace System.Collections.Generic
{
    public partial class Deque<T>
    {
        #region Enumerator Class

        [Serializable]
        private class Enumerator : IEnumerator<T>
        {
            private readonly Deque<T> owner;

            private readonly long version;

            private T current;

            private Node currentNode;

            // A value indicating whether the enumerator has been disposed.
            private bool disposed;

            private bool moveResult;

            public Enumerator(Deque<T> owner)
            {
                this.owner = owner;
                currentNode = owner.front;
                version = owner.version;
            }

            #region IEnumerator<T> Members

            T IEnumerator<T>.Current
            {
                get
                {
                    #region Require

                    if (disposed)
                        throw new ObjectDisposedException(GetType().Name);
                    if (!moveResult)
                        throw new InvalidOperationException(
                            "The enumerator is positioned before the first " +
                            "element of the Deque or after the last element.");

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

            #region IEnumerator Members

            public void Reset()
            {
                #region Require

                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);
                if (version != owner.version)
                    throw new InvalidOperationException(
                        "The Deque was modified after the enumerator was created.");

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
                        throw new ObjectDisposedException(GetType().Name);
                    if (!moveResult)
                        throw new InvalidOperationException(
                            "The enumerator is positioned before the first " +
                            "element of the Deque or after the last element.");

                    #endregion

                    return current;
                }
            }

            public bool MoveNext()
            {
                #region Require

                if (disposed)
                    throw new ObjectDisposedException(GetType().Name);
                if (version != owner.version)
                    throw new InvalidOperationException(
                        "The Deque was modified after the enumerator was created.");

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
        }

        #endregion
    }
}