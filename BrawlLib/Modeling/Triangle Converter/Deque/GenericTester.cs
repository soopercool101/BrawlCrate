using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BrawlLib.Modeling.Triangle_Converter.Deque
{
    public class GenericTester
    {
        private const int ElementCount = 100;

        public static void TestDeque(Deque<int> deque)
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

        private static void PopulateDequePushFront(Deque<int> deque)
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

        private static void PopulateDequePushBack(Deque<int> deque)
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

        private static void TestPopFront(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for (int i = 0; i < ElementCount; i++)
            {
                j = deque.PopFront();

                Debug.Assert(j == i);
            }

            Debug.Assert(deque.Count == 0);
        }

        private static void TestPopBack(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int j;

            for (int i = 0; i < ElementCount; i++)
            {
                j = deque.PopBack();

                Debug.Assert(j == ElementCount - 1 - i);
            }

            Debug.Assert(deque.Count == 0);
        }

        private static void TestContains(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            for (int i = 0; i < deque.Count; i++)
            {
                Debug.Assert(deque.Contains(i));
            }

            Debug.Assert(!deque.Contains(ElementCount));
        }

        private static void TestCopyTo(Deque<int> deque)
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

        private static void TestToArray(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            int[] array = deque.ToArray();
            int i = 0;

            foreach (int item in deque)
            {
                Debug.Assert(item.Equals(array[i]));
                i++;
            }
        }

        private static void TestClone(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            Deque<int> deque2 = (Deque<int>) deque.Clone();

            Debug.Assert(deque.Count == deque2.Count);

            IEnumerator<int> d2 = deque2.GetEnumerator();

            d2.MoveNext();

            foreach (int item in deque)
            {
                Debug.Assert(item.Equals(d2.Current));

                d2.MoveNext();
            }
        }

        private static void TestEnumerator(Deque<int> deque)
        {
            deque.Clear();

            PopulateDequePushBack(deque);

            IEnumerator<int> e = deque.GetEnumerator();

            try
            {
                int item = e.Current;

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                foreach (int item in deque)
                {
                    Debug.Assert(e.MoveNext());
                }

                Debug.Assert(!e.MoveNext());

                int o = e.Current;

                Debug.Fail("Exception failed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                e.Reset();

                foreach (int item in deque)
                {
                    Debug.Assert(e.MoveNext());
                }

                Debug.Assert(!e.MoveNext());

                int o = e.Current;

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