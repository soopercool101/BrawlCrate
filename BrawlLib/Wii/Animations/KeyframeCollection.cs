using BrawlLib.Internal;
using System;
using System.Collections.Generic;

namespace BrawlLib.Wii.Animations
{
    public interface IKeyframeSource
    {
        //KeyframeEntry GetKeyframe(int index, int arrayIndex);
        //KeyframeEntry SetKeyframe(int index, float value, params int[] arrays);
        //void RemoveKeyframe(int index, params int[] arrays);
        int FrameCount { get; }
        KeyframeArray[] KeyArrays { get; }
    }

    public class KeyframeCollection : IEnumerable<KeyframeArray>
    {
        public KeyframeArray[] _keyArrays;

        public KeyframeArray this[int index] => _keyArrays[index.Clamp(0, _keyArrays.Length - 1)];

        private bool _looped;

        public bool Loop
        {
            get => _looped;
            set
            {
                _looped = value;
                foreach (KeyframeArray a in _keyArrays)
                {
                    a.Loop = _looped;
                }
            }
        }

        private int _frameLimit;

        public int FrameLimit
        {
            get => _frameLimit;
            set
            {
                _frameLimit = value;
                foreach (KeyframeArray r in _keyArrays)
                {
                    r.FrameLimit = _frameLimit;
                }
            }
        }

        public KeyframeCollection(int arrayCount, int numFrames, params float[] defaultValues)
        {
            _frameLimit = numFrames;
            _keyArrays = new KeyframeArray[arrayCount];
            for (int i = 0; i < arrayCount; i++)
            {
                _keyArrays[i] = new KeyframeArray(numFrames, i < defaultValues.Length ? defaultValues[i] : 0);
            }
        }

        public float this[int index, params int[] arrays]
        {
            get => GetFrameValue(arrays[0], index);
            set
            {
                foreach (int i in arrays)
                {
                    _keyArrays[i].SetFrameValue(index, value);
                }
            }
        }

        public KeyframeEntry SetFrameValue(int arrayIndex, int frameIndex, float value, bool parsing = false)
        {
            return _keyArrays[arrayIndex].SetFrameValue(frameIndex, value, parsing);
        }

        public KeyframeEntry GetKeyframe(int arrayIndex, int index)
        {
            return _keyArrays[arrayIndex].GetKeyframe(index);
        }

        public float GetFrameValue(int arrayIndex, float index, bool returnOutValue = false)
        {
            return _keyArrays[arrayIndex].GetFrameValue(index, returnOutValue);
        }

        internal KeyframeEntry Remove(int arrayIndex, int index)
        {
            KeyframeEntry entry = null, root = _keyArrays[arrayIndex]._keyRoot;

            for (entry = root._next; entry != root && entry._index < index; entry = entry._next)
            {
                ;
            }

            if (entry._index == index)
            {
                entry.Remove();
                _keyArrays[arrayIndex]._keyCount--;
            }
            else
            {
                entry = null;
            }

            return entry;
        }

        public void Insert(int index, params int[] arrays)
        {
            KeyframeEntry entry = null, root;
            foreach (int x in arrays)
            {
                root = _keyArrays[x]._keyRoot;
                for (entry = root._prev; entry != root && entry._index >= index; entry = entry._prev)
                {
                    if (++entry._index >= _frameLimit)
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyArrays[x]._keyCount--;
                    }
                }
            }
        }

        public void Delete(int index, params int[] arrays)
        {
            KeyframeEntry entry = null, root;
            foreach (int x in arrays)
            {
                root = _keyArrays[x]._keyRoot;
                for (entry = root._prev; entry != root && entry._index >= index; entry = entry._prev)
                {
                    if (entry._index == index || --entry._index < 0)
                    {
                        entry = entry._next;
                        entry._prev.Remove();
                        _keyArrays[x]._keyCount--;
                    }
                }
            }
        }

        public int Clean()
        {
            int removed = 0;
            foreach (KeyframeArray arr in _keyArrays)
            {
                removed += arr.Clean();
            }

            return removed;
        }

        public bool Equals(KeyframeCollection obj)
        {
            if (!(obj is KeyframeCollection k))
            {
                return false;
            }

            if (k.ArrayCount != ArrayCount)
                return false;

            for (int i = 0; i < ArrayCount; i++)
            {
                if (!_keyArrays[i].Equals(k._keyArrays[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public int ArrayCount => _keyArrays.Length;

        public IEnumerator<KeyframeArray> GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _keyArrays.GetEnumerator();
        }
    }

    public class KeyframeEntry
    {
        public int _index;
        public KeyframeEntry _prev, _next;

        public float _value;
        public float _tangent;

        public bool Equals(KeyframeEntry obj)
        {
            return obj is KeyframeEntry entry && _index == entry._index && _value == entry._value && _tangent == entry._tangent;
        }

        public KeyframeEntry Second
        {
            get => _prev._index == _index ? _prev : _next._index == _index ? _next : null;
            set
            {
                KeyframeEntry second = Second;
                if (second == null)
                {
                    value._index = _index;
                    InsertAfter(value);
                }
                else
                {
                    second._value = value._value;
                    second._tangent = value._tangent;
                }
            }
        }

        public KeyframeEntry(int index, float value)
        {
            _index = index;
            _prev = _next = this;
            _value = value;
        }

        /// <summary>
        /// Inserts the provided entry before this one and relinks the previous and next entries.
        /// </summary>
        public void InsertBefore(KeyframeEntry entry)
        {
            _prev._next = entry;
            entry._prev = _prev;
            entry._next = this;
            _prev = entry;
        }

        /// <summary>
        /// Inserts the provided entry after this one and relinks the previous and next entries.
        /// </summary>
        public void InsertAfter(KeyframeEntry entry)
        {
            _next._prev = entry;
            entry._next = _next;
            entry._prev = this;
            _next = entry;
        }

        public void Remove()
        {
            _next._prev = _prev;
            _prev._next = _next;
        }

        public float Interpolate(float offset, float span, KeyframeEntry next, bool forceLinear = false)
        {
            //Return this value if no offset from this keyframe
            if (offset == 0)
            {
                return _value;
            }

            //Return next value if offset is to the next keyframe
            if (offset == span)
            {
                return next._value;
            }

            //Get the difference in values
            float diff = next._value - _value;

            //Calculate a percentage from this keyframe to the next
            float time = offset / span; //Normalized, 0 to 1

            bool prevDouble = _prev._index >= 0 && _prev._index == _index - 1;
            bool nextDouble = next._next._index >= 0 && next._next._index == next._index + 1;
            bool oneApart = _next._index == _index + 1;

            if (forceLinear)
            {
                return _value + diff * time;
            }

            float tan = _tangent;
            float nextTan = next._tangent;

            if (prevDouble || oneApart)
            {
                // Do nothing, as this doesn't seem to work properly //tan = (next._value - _value) / (next._index - _index);
            }

            if (nextDouble || oneApart)
            {
                // Do nothing, as this doesn't seem to work properly //nextTan = (next._value - _value) / (next._index - _index);
            }

            //Interpolate using a hermite curve
            float inv = time - 1.0f; //-1 to 0
            return _value
                   + offset * inv * (inv * tan + time * nextTan)
                   + time * time * (3.0f - 2.0f * time) * diff;
        }

        /// <summary>
        /// Returns an interpolated value between this keyframe and the next. 
        /// You can force linear calculation, but the Wii itself doesn't have anything like that.
        /// The Wii emulates linear interpolation using two keyframes across a range with the same tangent
        /// and then two keyframes on the same frame but with different tangents.
        /// </summary>
        public float Interpolate(float offset, bool forceLinear = false)
        {
            return Interpolate(offset, _next._index - _index, _next, forceLinear);
        }

        private const bool RoundTangent = true;
        private const int TangetDecimalPlaces = 3;

        public float GenerateTangent()
        {
            _tangent = 0.0f;
            if (Second != null)
            {
                if (_prev._index == _index)
                {
                    if (_next._index != -1)
                    {
                        //Generate only with the next keyframe
                        _tangent = (_value - _next._value) / (_index - _next._index);
                    }
                }
                else if (_next._index == _index)
                {
                    if (_prev._index != -1)
                    {
                        //Generate only with the previous keyframe
                        _tangent = (_value - _prev._value) / (_index - _prev._index);
                    }
                }
            }
            else
            {
                float weightCount = 0;
                if (_prev._index != -1)
                {
                    _tangent += (_value - _prev._value) / (_index - _prev._index);
                    weightCount++;
                }

                if (_next._index != -1)
                {
                    _tangent += (_next._value - _value) / (_next._index - _index);
                    weightCount++;
                }

                if (weightCount > 0)
                {
                    _tangent /= weightCount;
                }
            }

            if (RoundTangent)
            {
                _tangent = (float) Math.Round(_tangent, TangetDecimalPlaces);
            }

            return _tangent;
        }

        public override string ToString()
        {
            return $"Value={_value}";
        }
    }

    public class KeyframeArray
    {
        internal KeyframeEntry _keyRoot;
        internal int _keyCount;

        private bool _looped;

        public bool Loop
        {
            get => _looped;
            set => _looped = value;
        }

        internal int _frameLimit;

        public int FrameLimit
        {
            get => _frameLimit;
            set
            {
                _frameLimit = value;
                while (_keyRoot._prev._index >= value)
                {
                    _keyRoot._prev.Remove();
                    _keyCount--;
                }
            }
        }

        public float this[int index]
        {
            get => GetFrameValue(index);
            set => SetFrameValue(index, value);
        }

        public KeyframeArray(int limit, float defaultValue = 0)
        {
            _frameLimit = limit;
            _keyRoot = new KeyframeEntry(-1, defaultValue);
        }

        private const float _cleanDistance = 0.00001f;

        public int Clean()
        {
            int flag, res, removed = 0;
            KeyframeEntry entry;

            //Eliminate redundant values
            for (entry = _keyRoot._next._next; entry != _keyRoot; entry = entry._next)
            {
                flag = res = 0;

                if (entry._prev == _keyRoot)
                {
                    if (entry._next != _keyRoot)
                    {
                        flag = 1;
                    }
                }
                else if (entry._next == _keyRoot)
                {
                    flag = 2;
                }
                else
                {
                    flag = 3;
                }

                if ((flag & 1) != 0)
                {
                    res |= Math.Abs(entry._next._value - entry._value) <= _cleanDistance ? 1 : 0;
                }

                if ((flag & 2) != 0)
                {
                    res |= Math.Abs(entry._prev._value - entry._value) <= _cleanDistance ? 2 : 0;
                }

                if (flag == res && res != 0)
                {
                    entry = entry._prev;
                    entry._next.Remove();

                    entry.GenerateTangent();
                    entry._next.GenerateTangent();
                    entry._prev.GenerateTangent();

                    _keyCount--;
                    removed++;
                }
            }

            return removed;
        }

        public KeyframeEntry GetKeyframe(int index)
        {
            KeyframeEntry entry;
            for (entry = _keyRoot._next; entry != _keyRoot && entry._index < index; entry = entry._next)
            {
                ;
            }

            if (entry._index == index)
            {
                return entry;
            }

            return null;
        }

        public float GetFrameValue(float index, bool returnOutValue = false)
        {
            KeyframeEntry entry;

            if (index > _keyRoot._prev._index)
            {
                //If the frame is greater than the last keyframe's frame index
                if (_looped)
                {
                    float span = FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) +
                                 _keyRoot._next._index.Clamp(0, FrameLimit);
                    float offset =
                        index > _keyRoot._prev._index && index < FrameLimit
                            ? index - _keyRoot._prev._index
                            : FrameLimit - _keyRoot._prev._index + index;

                    return _keyRoot._prev.Interpolate(offset, span, _keyRoot._next);
                }

                return _keyRoot._prev._value;
            }

            if (index < _keyRoot._next._index)
            {
                //If the frame is less than the first keyframe's frame index
                if (_looped)
                {
                    float span = FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) +
                                 _keyRoot._next._index.Clamp(0, FrameLimit);
                    float offset =
                        index > _keyRoot._prev._index.Clamp(0, FrameLimit) && index < FrameLimit
                            ? index - _keyRoot._prev._index.Clamp(0, FrameLimit)
                            : FrameLimit - _keyRoot._prev._index.Clamp(0, FrameLimit) + index;

                    return _keyRoot._prev.Interpolate(offset, span, _keyRoot._next);
                }

                return _keyRoot._next._value;
            }

            //Find the entry just before the specified index
            for (entry = _keyRoot._next; //Get the first entry
                entry != _keyRoot &&     //Make sure it's not the root
                entry._index <= index;   //Its index must be less than or equal to the current index
                entry = entry._next)     //Get the next entry
            {
                if (entry._index == index)
                {
                    //The index is a keyframe
                    if (returnOutValue)
                    {
                        while (entry._next != null && entry._next._index == entry._index)
                        {
                            entry = entry._next;
                        }
                    }

                    return entry._value; //Return the value of the keyframe.
                }
            }

            //Frame lies between two keyframes. Interpolate between them
            return entry._prev.Interpolate(index - entry._prev._index);
        }

        public KeyframeEntry SetFrameValue(int index, float value, bool parsing = false)
        {
            KeyframeEntry entry = null;
            if (_keyRoot._prev == _keyRoot || _keyRoot._prev._index < index)
            {
                entry = _keyRoot;
            }
            else
            {
                for (entry = _keyRoot._next; entry != _keyRoot && entry._index <= index; entry = entry._next)
                {
                    ;
                }
            }

            entry = entry._prev;
            if (entry._index != index)
            {
                _keyCount++;
                entry.InsertAfter(entry = new KeyframeEntry(index, value));
            }
            else
            {
                //There can be up to two keyframes with the same index.
                if (!parsing)
                {
                    entry._value = value; //Do this when editing
                }
                else
                {
                    //And this when parsing
                    _keyCount++;
                    KeyframeEntry temp = new KeyframeEntry(index, value);
                    entry.InsertAfter(temp);
                    entry = temp;
                }
            }

            return entry;
        }

        public KeyframeEntry Remove(int index)
        {
            KeyframeEntry entry = null;
            for (entry = _keyRoot._next; entry != _keyRoot && entry._index < index; entry = entry._next)
            {
                ;
            }

            if (entry._index == index)
            {
                entry.Remove();
                _keyCount--;
            }
            else
            {
                entry = null;
            }

            return entry;
        }

        public void Insert(int index)
        {
            KeyframeEntry entry = null;
            for (entry = _keyRoot._prev; entry != _keyRoot && entry._index >= index; entry = entry._prev)
            {
                if (++entry._index >= _frameLimit)
                {
                    entry = entry._next;
                    entry._prev.Remove();
                    _keyCount--;
                }
            }
        }

        public void Delete(int index)
        {
            KeyframeEntry entry = null;
            for (entry = _keyRoot._prev; entry != _keyRoot && entry._index >= index; entry = entry._prev)
            {
                if (entry._index == index || --entry._index < 0)
                {
                    entry = entry._next;
                    entry._prev.Remove();
                    _keyCount--;
                }
            }
        }

        public bool Equals(KeyframeArray obj)
        {
            if(obj._keyCount != _keyCount)
                return false;

            KeyframeEntry comp1 = _keyRoot;
            KeyframeEntry comp2 = obj._keyRoot;

            for (int i = 0; i < _keyCount; i++)
            {
                if(!comp1.Equals(comp2))
                    return false;

                comp1 = comp1._next;
                comp2 = comp2._next;
            }

            return true;
        }
    }
}