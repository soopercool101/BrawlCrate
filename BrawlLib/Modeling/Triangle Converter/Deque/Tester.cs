using System;
using System.Collections;
using System.Diagnostics;

namespace BrawlLib.Modeling.Triangle_Converter.Deque
{
    internal class Tester
    {
        private const int ElementCount = 100;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            TestDeque(new Deque());
            TestDeque(Deque.Synchronized(new Deque()));

            GenericTester.TestDeque(new Deque<int>());
            GenericTester.TestDeque(Deque<int>.Synchronized(new Deque<int>()));
        }

        private static void TestDeque(Deque deque)
        {
            deque.Clear();
            Debug.Assert(deque.Count == 0);

            PopulateDequePushFront(deque);
            PopulateDequePushBack(deque);
            TestPopFront(deque);
            TestPopBack(deque);
            TestContains(deque);
            TestCopyTo(deque);
            TestToArray(deque);
            TestClone(deque);
            TestEnumerator(deque);
        }

        private static void PopulateDequePushFront(Deque deque)
        {
            deque.Clear();

            for (int i = 0; i < ElementCount; i++)
            {
                deque.PushFront(i);
            }

            Debug.Assert(deque.Count == ElementCount);

            int j = ElementCount - 1;

            foreach (int i in deque)
            {
                Debug.Assert(i == j);
                j--;
            }
        }

        private static void PopulateDequePushBack(Deque deque)
        {
            deque.Clear();

            for (int i = 0; i < ElementCount; i++)
            {
                deque.PushBack(i);
            }

            Debug.Assert(deque.Count == ElementCount);

            int j = 0;

            foreach (int i in deque)
            {
                Debug.Assert(i == j);
                j++;
            }
        }

        private static void TestPopFront(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for (int i = 0; i < ElementCount; i++)
            {
                j = (int) deque.PopFront();

                Debug.Assert(j == i);
            }

            Debug.Assert(deque.Count == 0);
        }

        private static void TestPopBack(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for (int i = 0; i < ElementCount; i++)
            {
                j = (int) deque.PopBack();

                Debug.Assert(j == ElementCount - 1 - i);
            }

            Debug.Assert(deque.Count == 0);
        }

        private static void TestContains(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            for (int i = 0; i < deque.Count; i++)
            {
                Debug.Assert(deque.Contains(i));
            }

            Debug.Assert(!deque.Contains(ElementCount));
        }

        private static void TestCopyTo(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int[] array = new int[deque.Count];

            deque.CopyTo(array, 0);

            foreach (int i in deque)
            {
                Debug.Assert(array[i] == i);
            }

            array = new int[deque.Count * 2];

            deque.CopyTo(array, deque.Count);

            foreach (int i in deque)
            {
                Debug.Assert(array[i + deque.Count] == i);
            }

            array = new int[deque.Count];

            try
            {
                deque.CopyTo(null, deque.Count);

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, -1);

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, deque.Count / 2);

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(array, deque.Count);

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.CopyTo(new int[10, 10], deque.Count);

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void TestToArray(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            object[] array = deque.ToArray();
            int i = 0;

            foreach (object obj in deque)
            {
                Debug.Assert(obj.Equals(array[i]));
                i++;
            }
        }

        private static void TestClone(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            Deque deque2 = (Deque) deque.Clone();

            Debug.Assert(deque.Count == deque2.Count);

            IEnumerator d2 = deque2.GetEnumerator();

            d2.MoveNext();

            foreach (object obj in deque)
            {
                Debug.Assert(obj.Equals(d2.Current));

                d2.MoveNext();
            }
        }

        private static void TestEnumerator(Deque deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            IEnumerator e = deque.GetEnumerator();

            try
            {
                object obj = e.Current;

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (object obj in deque)
                {
                    Debug.Assert(e.MoveNext());
                }

                Debug.Assert(!e.MoveNext());

                object o = e.Current;

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                e.Reset();

                foreach (object obj in deque)
                {
                    Debug.Assert(e.MoveNext());
                }

                Debug.Assert(!e.MoveNext());

                object o = e.Current;

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                deque.PushBack(deque.Count);

                e.Reset();

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                e.MoveNext();

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}