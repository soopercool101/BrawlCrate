using BrawlLib.SSBB.Types;
using System;

namespace BrawlLib.Internal
{
    public class Event
    {
        public void SetEventEvent(long v)
        {
            eventEvent = v;
            ResolveEventEvent();
        }

        public long GetEventEvent()
        {
            return eventEvent;
        }

        //Resolve for the string-hex version of the event Event and adjust the number
        //of parameters to the amount indicated by the event Event.
        public void ResolveEventEvent()
        {
            eventId = eventEvent.ToString("X");
            eventId = eventId.PadLeft(8, '0');
            if (eventId.Length > 8)
            {
                eventId = eventId.Substring(eventId.Length - 8);
            }

            lParameters = (eventEvent & 0xFF00) / 0x100;

            Array.Resize(ref parameters, (int) lParameters);
            for (int i = 0; i < lParameters; i++)
            {
                parameters[i] = new Param();
            }
        }

        public string eventId;
        public long pParameters;
        public long lParameters;
        public Param[] parameters;
        public long eventEvent;
    }

    public class Param
    {
        public ArgVarType _type;
        public long _data;
    }
}