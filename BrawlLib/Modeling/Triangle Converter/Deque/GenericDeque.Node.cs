using System;

namespace BrawlLib.Modeling.Triangle_Converter.Deque
{
    public partial class Deque<T>
    {
        #region Node Class

        // Represents a node in the deque.
        [Serializable()]
        public class Node
        {
            private T value;

            private Node previous;

            private Node next;

            public Node(T value)
            {
                this.value = value;
            }

            public T Value
            {
                get => value;
                set => this.value = value;
            }

            public Node Previous
            {
                get => previous;
                set => previous = value;
            }

            public Node Next
            {
                get => next;
                set => next = value;
            }
        }

        #endregion
    }
}