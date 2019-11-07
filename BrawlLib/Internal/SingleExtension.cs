namespace BrawlLib.Internal
{
    public static class SingleExtension
    {
        public static unsafe float Reverse(this float value)
        {
            *(uint*) &value = ((uint*) &value)->Reverse();
            return value;
        }

        //private static double _double2fixmagic = 68719476736.0f * 1.5f;
        //public static unsafe Int32 ToInt32(this Single value)
        //{
        //    double v = value + _double2fixmagic;
        //    return *((int*)&v) >> 16; 
        //}

        public static float Clamp(this float value, float min, float max)
        {
            return value <= min ? min : value >= max ? max : value;
        }

        /// <summary>
        /// Remaps values outside of a range into the first multiple of that range.
        /// When it comes to signed numbers, negative is highest.
        /// For example, -128 (0xFF) vs 127 (0x7F).
        /// Because of this, the max value is non-inclusive while the min value is.
        /// </summary>
        public static float RemapToRange(this float value, float min, float max)
        {
            //Check if the value is already in the range
            if (value < max && value >= min)
            {
                return value;
            }

            //Get the distance between max and min
            float range = max - min;

            //First figure out how many multiples of the range there are.
            //Dividing the value by the range and cutting off the decimal places
            //will return the number of multiples of whole ranges in the value.
            //Those multiples need to be subtracted out.
            value -= range * (int) (value / range);

            //Now the value is in the range of +range to -range.
            //The value needs to be within +(range/2) to -(range/2).
            value += value > max ? -range : value < min ? range : 0;

            //Max value is non-inclusive
            if (value == max)
            {
                value = min;
            }

            return value;
        }
    }
}