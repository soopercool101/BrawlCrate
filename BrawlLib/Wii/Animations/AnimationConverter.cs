using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Models;
using System;

namespace BrawlLib.Wii.Animations
{
    internal static unsafe class AnimationConverter
    {
        #region Decoding

        public static KeyframeCollection DecodeKeyframes(VoidPtr entry, NW4RAnimationNode node, int arrayCount,
                                                         params float[] defaults)
        {
            //If the node is null, assume the programmer has created a new entry and accessed
            //the keyframe collection for the first time before assigning the parent and will
            //set the frame count later manually.
            int numFrames = node == null ? 1 : node.FrameCount + (node.Loop ? 1 : 0);

            KeyframeCollection kf;
            if (node is CHR0Node && entry)
            {
                kf = DecodeCHR0Keyframes((CHR0Entry*) entry, numFrames);
            }
            else if (node is SRT0Node && entry)
            {
                kf = DecodeSRT0Keyframes((SRT0TextureEntry*) entry, numFrames);
            }
            else
            {
                kf = new KeyframeCollection(arrayCount, numFrames, defaults);
            }

            if (node != null)
            {
                kf.Loop = node.Loop;
            }

            return kf;
        }

        public static KeyframeArray DecodeSHP0Keyframes(SHP0KeyframeEntries* entry, SHP0Node node)
        {
            //If the node is null, assume the programmer has created a new entry and accessed
            //the keyframe collection for the first time before assigning the parent and will
            //set the frame count later manually.
            int numFrames = node == null ? 1 : node.FrameCount + (node.Loop ? 1 : 0);

            KeyframeArray kf = new KeyframeArray(numFrames) {Loop = node.Loop};

            if (entry == null)
            {
                return kf;
            }

            int fCount = entry->_numEntries;
            BVec3* vec = entry->Entries;
            for (int i = 0; i < fCount; i++, vec++)
            {
                kf.SetFrameValue((int) vec->_y, vec->_z, true)._tangent = vec->_x;
            }

            return kf;
        }

        public static KeyframeCollection DecodeSRT0Keyframes(SRT0TextureEntry* entry, int numFrames)
        {
            KeyframeCollection kf = new KeyframeCollection(5, numFrames, 1, 1);

            if (entry == null)
            {
                return kf;
            }

            bfloat* sPtr = (bfloat*) entry->Data;
            SRT0Code code = entry->Code;

            if (!code.NoScale)
            {
                if (code.ScaleIsotropic)
                {
                    if (code.FixedScaleX)
                    {
                        kf[0, 0, 1] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 0, 1);
                    }
                }
                else
                {
                    if (code.FixedScaleX)
                    {
                        kf[0, 0] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 0);
                    }

                    if (code.FixedScaleY)
                    {
                        kf[0, 1] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 1);
                    }
                }
            }

            if (!code.NoRotation)
            {
                if (code.FixedRotation)
                {
                    kf[0, 2] = *sPtr++;
                }
                else
                {
                    DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 2);
                }
            }

            if (!code.NoTranslation)
            {
                if (code.FixedX)
                {
                    kf[0, 3] = *sPtr++;
                }
                else
                {
                    DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 3);
                }

                if (code.FixedY)
                {
                    kf[0, 4] = *sPtr++;
                }
                else
                {
                    DecodeFrames(kf, (VoidPtr) sPtr + *(buint*) sPtr++, AnimDataFormat.I12, 4);
                }
            }

            return kf;
        }

        public static KeyframeCollection DecodeCHR0Keyframes(CHR0Entry* entry, int numFrames)
        {
            KeyframeCollection kf = new KeyframeCollection(9, numFrames, 1, 1, 1);

            if (entry == null)
            {
                return kf;
            }

            bfloat* sPtr = (bfloat*) entry->Data;
            AnimationCode code = entry->Code;
            AnimDataFormat format;

            if (code.HasScale)
            {
                format = code.ScaleDataFormat;
                if (code.IsScaleIsotropic)
                {
                    if (code.IsScaleZFixed)
                    {
                        kf[0, 0, 1, 2] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 0, 1, 2);
                    }
                }
                else
                {
                    if (code.IsScaleXFixed)
                    {
                        kf[0, 0] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 0);
                    }

                    if (code.IsScaleYFixed)
                    {
                        kf[0, 1] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 1);
                    }

                    if (code.IsScaleZFixed)
                    {
                        kf[0, 2] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 2);
                    }
                }
            }

            if (code.HasRotation)
            {
                format = code.RotationDataFormat;
                if (code.IsRotationIsotropic)
                {
                    if (code.IsRotationZFixed)
                    {
                        kf[0, 3, 4, 5] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 3, 4, 5);
                    }
                }
                else
                {
                    if (code.IsRotationXFixed)
                    {
                        kf[0, 3] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 3);
                    }

                    if (code.IsRotationYFixed)
                    {
                        kf[0, 4] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 4);
                    }

                    if (code.IsRotationZFixed)
                    {
                        kf[0, 5] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 5);
                    }
                }
            }

            if (code.HasTranslation)
            {
                format = code.TranslationDataFormat;
                if (code.IsTranslationIsotropic)
                {
                    if (code.IsTranslationZFixed)
                    {
                        kf[0, 6, 7, 8] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 6, 7, 8);
                    }
                }
                else
                {
                    if (code.IsTranslationXFixed)
                    {
                        kf[0, 6] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 6);
                    }

                    if (code.IsTranslationYFixed)
                    {
                        kf[0, 7] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 7);
                    }

                    if (code.IsTranslationZFixed)
                    {
                        kf[0, 8] = *sPtr++;
                    }
                    else
                    {
                        DecodeFrames(kf, (VoidPtr) entry + *(buint*) sPtr++, format, 8);
                    }
                }
            }

            return kf;
        }

        private static void DecodeFrames(KeyframeCollection kf, void* dataAddr, AnimDataFormat format,
                                         params int[] arrays)
        {
            int fCount;
            float vStep, vBase;
            switch (format)
            {
                case AnimDataFormat.I4:
                {
                    I4Header* header = (I4Header*) dataAddr;
                    fCount = header->_entries;
                    vStep = header->_step;
                    vBase = header->_base;

                    foreach (int x in arrays)
                    {
                        I4Entry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                        {
                            kf.SetFrameValue(x, entry->FrameIndex, vBase + entry->Step * vStep, true)._tangent =
                                entry->Tangent;
                        }
                    }

                    break;
                }

                case AnimDataFormat.I6:
                {
                    I6Header* header = (I6Header*) dataAddr;
                    fCount = header->_numFrames;
                    vStep = header->_step;
                    vBase = header->_base;

                    foreach (int x in arrays)
                    {
                        I6Entry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                        {
                            kf.SetFrameValue(x, entry->FrameIndex, vBase + entry->_step * vStep, true)._tangent =
                                entry->Tangent;
                        }
                    }

                    break;
                }

                case AnimDataFormat.I12:
                {
                    I12Header* header = (I12Header*) dataAddr;
                    fCount = header->_numFrames;

                    foreach (int x in arrays)
                    {
                        I12Entry* entry = header->Data;
                        for (int i = 0; i < fCount; i++, entry++)
                        {
                            kf.SetFrameValue(x, (int) entry->_index, entry->_value, true)._tangent = entry->_tangent;
                        }
                    }

                    break;
                }

                case AnimDataFormat.L1:
                {
                    L1Header* header = (L1Header*) dataAddr;
                    vStep = header->_step;
                    vBase = header->_base;

                    foreach (int x in arrays)
                    {
                        byte* sPtr = header->Data;
                        for (int i = 0; i < kf.FrameLimit; i++)
                        {
                            kf.SetFrameValue(x, i, vBase + *sPtr++ * vStep, true).GenerateTangent();
                        }
                    }

                    break;
                }

                case AnimDataFormat.L2:
                {
                    L1Header* header = (L1Header*) dataAddr;
                    vStep = header->_step;
                    vBase = header->_base;

                    foreach (int x in arrays)
                    {
                        bushort* sPtr = (bushort*) header->Data;
                        for (int i = 0; i < kf.FrameLimit; i++)
                        {
                            kf.SetFrameValue(x, i, vBase + *sPtr++ * vStep, true).GenerateTangent();
                        }
                    }

                    break;
                }

                case AnimDataFormat.L4:
                {
                    foreach (int x in arrays)
                    {
                        bfloat* sPtr = (bfloat*) dataAddr;
                        for (int i = 0; i < kf.FrameLimit; i++)
                        {
                            kf.SetFrameValue(x, i, *sPtr++, true).GenerateTangent();
                        }
                    }

                    break;
                }
            }
        }

        #endregion

        #region Size Calculation

        public static int CalculateCHR0Size(KeyframeCollection kf, out int entrySize, out AnimationCode code)
        {
            int dataSize = 0;
            entrySize = 8;

            code = AnimationCode.Default;

            for (int i = 0; i < 3; i++)
            {
                dataSize += EvaluateCHR0Group(ref code, kf, i, ref entrySize);
            }

            if (!code.HasRotation && !code.HasTranslation)
            {
                code.IgnoreRotAndTrans = true;
                code.Identity = !code.HasScale;
            }
            else
            {
                code.IgnoreRotAndTrans = false;
                code.Identity = false;
            }

            return dataSize;
        }

        public static int CalculateSRT0Size(KeyframeCollection kf, out int entrySize, out SRT0Code code)
        {
            int dataSize = 0;
            entrySize = 4;

            code = SRT0Code.Default;

            for (int i = 0; i < 3; i++)
            {
                dataSize += EvaluateSRT0Group(ref code, kf, i, ref entrySize);
            }

            return dataSize;
        }

        private const float scaleError = 0.0005f;
        private const float tanError = 0.001f;

        private static int EvaluateCHR0Group(ref AnimationCode code, KeyframeCollection kf, int group,
                                             ref int entrySize)
        {
            int index = group * 3;
            int numFrames = kf.FrameLimit;
            int dataLen = 0;
            int maxEntries;
            int evalCount;
            int scaleSpan;
            //bool useLinear = group == 1;

            bool exist = false;
            bool isotropic = group == 0;
            AnimDataFormat format = AnimDataFormat.None;

            KeyframeEntry[] roots = new KeyframeEntry[3];

            KeyframeEntry[][] arr = new KeyframeEntry[3][];
            int[] count = new int[3];
            bool[] isExist = new bool[3];
            bool[] isFixed = new bool[3];
            bool[] isScalable = new bool[3];
            float[] floor = new float[3];
            float[] ceil = new float[3];

            KeyframeEntry entry;
            int eCount = 0;

            float min;
            float max;
            int maxIndex = 0;

            //Initialize values
            for (int i = 0; i < 3; i++)
            {
                entry = roots[i] = kf._keyArrays[index + i]._keyRoot;
                count[i] = kf._keyArrays[index + i]._keyCount;
                isExist[i] = count[i] > 0;
                isFixed[i] = count[i] <= 1;

                if (!isFixed[i])
                {
                    min = float.MaxValue;
                    max = float.MinValue;

                    for (entry = entry._next; entry._index != -1; entry = entry._next)
                    {
                        min = Math.Min(entry._value, min);
                        max = Math.Max(entry._value, max);
                    }

                    floor[i] = min;
                    ceil[i] = max;

                    maxIndex = Math.Max(entry._prev._index, maxIndex);
                }
            }

            if (exist = isExist[0] || isExist[1] || isExist[2])
            {
                if (group == 0)
                {
                    if (isFixed[0] != isFixed[1] || isFixed[0] != isFixed[2])
                    {
                        isotropic = false;
                    }
                    else if (count[0] != count[1] || count[0] != count[2])
                    {
                        isotropic = false;
                    }
                    else
                    {
                        KeyframeEntry e1 = roots[0], e2 = roots[1], e3 = roots[2];
                        for (int i = count[0]; i-- > 0;)
                        {
                            e1 = e1._next;
                            e2 = e2._next;
                            e3 = e3._next;
                            if (e1._index != e2._index || e1._index != e3._index ||
                                e1._value != e2._value || e1._value != e3._value)
                            {
                                isotropic = false;
                                break;
                            }
                        }
                    }
                }

                if (isotropic)
                {
                    evalCount = 1;
                    maxEntries = count[0];
                }
                else
                {
                    evalCount = 3;
                    maxEntries = Math.Max(Math.Max(count[0], count[1]), count[2]);
                    //useLinear &= (count[0] == numFrames) && (count[1] == numFrames) && (count[2] == numFrames);
                }

                scaleSpan = @group == 1 ? 255 : maxIndex <= 255 ? 4095 : maxIndex <= 2047 ? 65535 : -1;
                //scaleSpan = useLinear ? 255 : 4095;

                //Determine if values are scalable
                for (int i = 0; i < evalCount; i++)
                {
                    isScalable[i] = true;
                    if (isFixed[i] || scaleSpan == -1)
                    {
                        continue;
                    }

                    //float* pValue = value[i];
                    eCount = count[i];

                    float basev, range, step, distance, val;

                    basev = floor[i];
                    range = ceil[i] - basev;


                    //Evaluate spans until we reach a success.
                    //A success means that compression using that span is possible. 
                    //No further evaluation necessary.
                    SpanBegin:
                    int span = scaleSpan;
                    int spanEval = scaleSpan - 32;

                    float tanScale = scaleSpan == 4095 ? 32.0f : 256.0f;
                    if (scaleSpan != 255)
                    {
                        for (entry = roots[i]._next; entry._index != -1; entry = entry._next)
                        {
                            //Ignore entries that don't need interp.
                            if (entry._index - entry._prev._index >= 1 && entry._prev._index != -1 ||
                                entry._next._index - entry._index >= 1 && entry._next._index != -1)
                            {
                                val = entry._tangent * tanScale;
                                val += val < 0 ? -0.5f : 0.5f;
                                if (Math.Abs((int) val / tanScale - entry._tangent) > tanError)
                                {
                                    span = spanEval;
                                    break;
                                }
                            }
                        }
                    }

                    if (span > spanEval && range == 0.0f)
                    {
                        continue;
                    }

                    SpanStep:
                    if (span > spanEval)
                    {
                        step = range / span;

                        //if span <= 255, check every frame instead!
                        if (span <= 255)
                        {
                            for (int x = 0; x < numFrames; x++)
                            {
                                val = kf[x, index + i];
                                distance = (val - basev) / step + 0.5f;
                                distance = Math.Abs(val - (basev + (int) distance * step));

                                //If distance is too large change span and retry
                                if (distance > scaleError)
                                {
                                    span--;
                                    goto SpanStep;
                                }
                            }
                        }
                        else
                        {
                            for (entry = roots[i]._next; entry._index != -1; entry = entry._next)
                            {
                                val = entry._value;
                                distance = (val - basev) / step + 0.5f;
                                distance = Math.Abs(val - (basev + (int) distance * step));

                                //If distance is too large change span and retry
                                if (distance > scaleError)
                                {
                                    span--;
                                    goto SpanStep;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (scaleSpan <= 255 && maxIndex <= 255)
                        {
                            scaleSpan = 4095;
                        }
                        else if (scaleSpan <= 4095 && maxIndex <= 2047)
                        {
                            scaleSpan = 65535;
                        }
                        else
                        {
                            scaleSpan = -1;
                            isScalable[i] = false;
                            continue;
                        }

                        goto SpanBegin;
                    }
                }

                //Determine format only if there are unfixed entries
                if (!isFixed[0] || !isFixed[1] || !isFixed[2])
                {
                    bool scale = isotropic ? isScalable[0] : isScalable[0] && isScalable[1] && isScalable[2];
                    float frameSpan = (float) numFrames / maxEntries;

                    if (scale)
                    {
                        if (@group == 1 && scaleSpan <= 255 && frameSpan < 4.0f)
                        {
                            format = AnimDataFormat.L1;
                        }
                        else if (scaleSpan <= 4095 && maxIndex <= 255)
                        {
                            format = AnimDataFormat.I4;
                        }
                        else if (frameSpan > 1.5f && maxIndex <= 2047)
                        {
                            format = AnimDataFormat.I6;
                        }
                        else if (@group == 1 && frameSpan <= 3.0f)
                        {
                            format = AnimDataFormat.L4;
                        }
                        else
                        {
                            format = AnimDataFormat.I12;
                        }
                    }
                    else if (@group == 1 && frameSpan <= 3.0f)
                    {
                        format = AnimDataFormat.L4;
                    }
                    else
                    {
                        format = AnimDataFormat.I12;
                    }
                }

                //calculate size
                for (int i = 0; i < evalCount; i++)
                {
                    entrySize += 4;

                    if (!isFixed[i])
                    {
                        switch (format)
                        {
                            case AnimDataFormat.I12:
                                dataLen += 8 + count[i] * 12;
                                break;

                            case AnimDataFormat.I4:
                                dataLen += 16 + count[i] * 4;
                                break;

                            case AnimDataFormat.I6:
                                dataLen += (16 + count[i] * 6).Align(4);
                                break;

                            case AnimDataFormat.L1:
                                dataLen += (8 + numFrames).Align(4);
                                break;

                            case AnimDataFormat.L4:
                                dataLen += numFrames * 4;
                                break;
                        }
                    }
                }

                //Should we compress here?
            }
            else //Set isotropic to true, so it sets the default value.
            {
                isotropic = true;
            }

            if (group == 0)
            {
                code.IgnoreScale = !exist;
            }

            code.SetExists(group, exist);
            code.SetIsIsotropic(group, isotropic);
            for (int i = 0; i < 3; i++)
            {
                code.SetIsFixed(index + i, isFixed[i]);
            }

            code.SetFormat(group, format);

            return dataLen;
        }

        private static int EvaluateSRT0Group(ref SRT0Code code, KeyframeCollection kf, int group, ref int entrySize)
        {
            //SRT0s always use I12

            //group
            //0 = scale
            //1 = rot
            //2 = trans

            int index = group == 0 ? 0 : group == 1 ? 2 : 3;
            int numFrames = kf.FrameLimit;
            int dataLen = 0;
            KeyframeEntry[] roots = new KeyframeEntry[2];
            bool exist = false;
            bool isotropic = group == 0;
            int[] count = new int[2];
            bool[] isExist = new bool[2];
            bool[] isFixed = new bool[2];

            for (int i = 0; i < (group == 1 ? 1 : 2); i++)
            {
                roots[i] = kf._keyArrays[index + i]._keyRoot;
                count[i] = kf._keyArrays[index + i]._keyCount;
                isExist[i] = count[i] > 0;
                isFixed[i] = count[i] <= 1;
            }

            if (exist = isExist[0] || isExist[1])
            {
                if (group == 0)
                {
                    if (isFixed[0] != isFixed[1])
                    {
                        isotropic = false;
                    }
                    else if (count[0] != count[1])
                    {
                        isotropic = false;
                    }
                    else
                    {
                        KeyframeEntry e1 = roots[0], e2 = roots[1];
                        for (int i = count[0]; i-- > 0;)
                        {
                            e1 = e1._next;
                            e2 = e2._next;
                            if (e1._index != e2._index ||
                                e1._value != e2._value)
                            {
                                isotropic = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (group == 0 && !isotropic)
            {
                code.ScaleIsotropic = false;
            }

            for (int i = 0; i < (group == 1 ? 1 : 2); i++)
            {
                if (exist)
                {
                    switch (group)
                    {
                        case 0:
                            code.NoScale = false;
                            break;
                        case 1:
                            code.NoRotation = false;
                            break;
                        case 2:
                            code.NoTranslation = false;
                            break;
                    }

                    if (!(group == 0 && i == 1 && code.ScaleIsotropic))
                    {
                        entrySize += 4;
                    }

                    if (!isFixed[i])
                    {
                        switch (group)
                        {
                            case 0:
                                switch (i)
                                {
                                    case 0:
                                        code.FixedScaleX = false;
                                        break;
                                    case 1:
                                        code.FixedScaleY = false;
                                        break;
                                }

                                break;
                            case 1:
                                code.FixedRotation = false;
                                break;
                            case 2:
                                switch (i)
                                {
                                    case 0:
                                        code.FixedX = false;
                                        break;
                                    case 1:
                                        code.FixedY = false;
                                        break;
                                }

                                break;
                        }

                        if (!(group == 0 && i == 1 && code.ScaleIsotropic))
                        {
                            dataLen += 8 + count[i] * 12;
                        }
                    }
                }
            }

            return dataLen;
        }

        #endregion

        #region Encoding

        public static void EncodeCHR0Keyframes(KeyframeCollection kf, VoidPtr entryAddress, VoidPtr dataAddress,
                                               AnimationCode code)
        {
            CHR0Entry* header = (CHR0Entry*) entryAddress;
            header->_code = (uint) code._data;
            header->_stringOffset = 0;

            bint* pOffset = (bint*) entryAddress + 2;

            //Write values/offset and encode groups
            for (int i = 0, x = 0; i < 3; i++, x += 3)
            {
                if (code.GetExists(i))
                {
                    AnimDataFormat format = code.GetFormat(i);
                    if (i == 0 && code.GetIsIsotropic(i))
                    {
                        if (code.GetIsFixed(2))
                        {
                            *(bfloat*) pOffset++ = kf._keyArrays[2]._keyRoot._next._value;
                        }
                        else
                        {
                            *pOffset++ = dataAddress - entryAddress;
                            dataAddress += EncodeEntry(x, format, kf, dataAddress);
                        }
                    }
                    else
                    {
                        for (int y = 0, z = x; y < 3; y++, z++)
                        {
                            if (code.GetIsFixed(z))
                            {
                                *(bfloat*) pOffset++ = kf._keyArrays[z]._keyRoot._next._value;
                            }
                            else
                            {
                                *pOffset++ = dataAddress - entryAddress;
                                dataAddress += EncodeEntry(z, format, kf, dataAddress);
                            }
                        }
                    }
                }
            }
        }

        public static void EncodeSRT0Keyframes(KeyframeCollection kf, VoidPtr entryAddress, VoidPtr dataAddress,
                                               SRT0Code code)
        {
            SRT0TextureEntry* header = (SRT0TextureEntry*) entryAddress;
            header->Code = code;

            bint* pOffset = (bint*) entryAddress + 1;

            int index = 0;

            //Write values/offset and encode groups
            for (int type = 0; type < 3; type++)
            {
                bool has = false;
                switch (type)
                {
                    case 0:
                        has = !code.NoScale;
                        break;
                    case 1:
                        has = !code.NoRotation;
                        break;
                    case 2:
                        has = !code.NoTranslation;
                        break;
                }

                for (int axis = 0; axis < (type == 1 ? 1 : 2); axis++, index++)
                {
                    if (has)
                    {
                        if (code.ScaleIsotropic && type == 0)
                        {
                            if (axis == 0)
                            {
                                if (code.FixedScaleX)
                                {
                                    *(bfloat*) pOffset++ = kf._keyArrays[0]._keyRoot._next._value;
                                }
                                else
                                {
                                    *pOffset = (int) dataAddress - (int) pOffset;
                                    pOffset++;
                                    dataAddress += EncodeEntry(index, AnimDataFormat.I12, kf, dataAddress);
                                }
                            }
                        }
                        else
                        {
                            //This gets the fixed bit. Same order, so it can be indexed
                            if (code._data[5 + index])
                            {
                                *(bfloat*) pOffset++ = kf._keyArrays[index]._keyRoot._next._value;
                            }
                            else
                            {
                                *pOffset = (int) dataAddress - (int) pOffset;
                                pOffset++;
                                dataAddress += EncodeEntry(index, AnimDataFormat.I12, kf, dataAddress);
                            }
                        }
                    }
                }
            }
        }

        private static int EncodeEntry(int index, AnimDataFormat format, KeyframeCollection kf, VoidPtr addr)
        {
            int numFrames = kf.FrameLimit;
            KeyframeEntry frame, root = kf._keyArrays[index]._keyRoot;
            bfloat* pVal = (bfloat*) addr;
            float val, frameScale = numFrames <= 1 ? 1 : 1.0f / (numFrames - 1);
            float min, max, stride, step;
            int span, i;
            int keyCount = kf._keyArrays[index]._keyCount;

            if (format == AnimDataFormat.L4)
            {
                //Use all frames, just in case not all frames are key.
                for (i = 0; i < numFrames; i++)
                {
                    *pVal++ = kf[i, index];
                }

                return numFrames * 4;
            }

            if (format == AnimDataFormat.I12)
            {
                I12Header* header = (I12Header*) addr;
                *header = new I12Header(keyCount, frameScale);

                I12Entry* entry = header->Data;
                for (frame = root._next; frame._index != -1; frame = frame._next)
                {
                    *entry++ = new I12Entry(frame._index, frame._value, frame._tangent);
                }

                return keyCount * 12 + 8;
            }

            //Get floor/ceil/stride
            min = float.MaxValue;
            max = float.MinValue;
            for (frame = root._next; frame != root; frame = frame._next)
            {
                val = frame._value;

                if (val > max)
                {
                    max = val;
                }

                if (val < min)
                {
                    min = val;
                }
            }

            stride = max - min;

            if (format == AnimDataFormat.L1)
            {
                //Find best span
                span = EvalSpan(255, 32, min, stride, root, true);
                step = stride / span;

                L1Header* header = (L1Header*) addr;
                *header = new L1Header(step, min);

                byte* dPtr = header->Data;
                for (i = 0; i < numFrames; i++)
                {
                    *dPtr++ = (byte) ((kf[i, index] - min) / step + 0.5f);
                }

                //Fill remaining bytes
                while ((i++ & 3) != 0)
                {
                    *dPtr++ = 0;
                }

                return (8 + numFrames).Align(4);
            }

            if (format == AnimDataFormat.I4)
            {
                //Find best span
                span = EvalSpan(4095, 32, min, stride, root, false);
                step = stride / span;

                I4Header* header = (I4Header*) addr;
                *header = new I4Header(keyCount, frameScale, step, min);

                I4Entry* entry = header->Data;

                for (frame = root._next; frame._index != -1; frame = frame._next)
                {
                    val = (frame._value - min) / step;
                    val += val < 0 ? -0.5f : 0.5f;

                    *entry++ = new I4Entry(frame._index, (int) val, frame._tangent);
                }

                return keyCount * 4 + 16;
            }

            if (format == AnimDataFormat.I6)
            {
                //Find best span
                span = EvalSpan(65535, 32, min, stride, root, false);
                step = stride / span;

                I6Header* header = (I6Header*) addr;
                *header = new I6Header(keyCount, frameScale, step, min);

                I6Entry* entry = header->Data;

                for (frame = root._next; frame._index != -1; frame = frame._next)
                {
                    val = (frame._value - min) / step;
                    val += val < 0 ? -0.5f : 0.5f;

                    *entry++ = new I6Entry(frame._index, (int) val, frame._tangent);
                }

                //Fill remaining bytes
                if ((keyCount & 1) != 0)
                {
                    entry->_data = 0;
                }

                return (keyCount * 6 + 16).Align(4);
            }

            return 0;
        }

        public static int EvalSpan(int maxSpan, int maxIterations, float valBase, float valStride, KeyframeEntry root,
                                   bool evalAll)
        {
            KeyframeEntry entry;
            float bestError = float.MaxValue;
            float worstError;
            float step, error, val;
            int bestSpan = maxSpan, count;

            if (maxIterations <= 0)
            {
                maxIterations = maxSpan - 2;
            }

            for (int i = 0; i < maxIterations; i++)
            {
                worstError = float.MinValue;
                step = valStride / maxSpan;

                for (entry = root._next; entry != root; entry = entry._next)
                {
                    if (evalAll)
                    {
                        count = entry._next == root ? 1 : entry._next._index - entry._index;
                    }
                    else
                    {
                        count = 1;
                    }

                    for (int x = 0; x < count; x++)
                    {
                        val = entry.Interpolate(x);
                        error = (val - valBase) / step + 0.5f;
                        error = Math.Abs(val - (valBase + (int) error * step));

                        if (error > scaleError)
                        {
                            goto Next;
                        }

                        if (error > worstError)
                        {
                            worstError = error;
                        }
                    }
                }

                if (worstError < bestError)
                {
                    bestError = worstError;
                    bestSpan = maxSpan;
                }

                Next:
                maxSpan--;
            }

            return bestSpan;
        }

        #endregion
    }
}