using System;
using System.Runtime.InteropServices;

namespace BrawlLib.SSBBTypes
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct GDOR
    {
        public const uint Tag = 0x524F4447;
        public const int Size = 0x08;
        public uint _tag;
        public bint _count;

        public GDOR(int count)
        {
            _tag = Tag;
            _count = count;
        }

        public VoidPtr this[int index] { get { return (VoidPtr)((byte*)Address + Offsets(index)); } }
        public uint Offsets(int index) { return *(buint*)((byte*)Address + 0x08 + (index * 4)); }
        private VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
    }
   [StructLayout(LayoutKind.Sequential, Pack = 1)]
   public unsafe struct GDOREntry
   {
       public const int Size = 0x90;

       public bfloat _pad0;

       // 4 bytes, follows same pattern as model index field.
       public byte _unk0;
       public byte _unk1;
       public byte _unk2;
       public byte _unk3;

       public int _pad1;
       public bint _unkInt;
       fixed byte _pad2[20];

       // Most likely the z, y, and z override coordinates for the door.
       public bfloat _xOverride;
       public bfloat _yOverride;
       public bfloat _zOverride;

       fixed byte _stageID[3];
       public byte _doorIndex;

       // Specifies model index to use, among other unknown things.
       public byte _unk4;
       public byte _unk5;
       public byte _mdlIndex;
       public byte _unk6;

       // Possibly rotation override?
       public bfloat _unkFloat0;
       public bfloat _unkFloat1;
       
       // Triggers and padding
       fixed byte _trigger0[4];
       public byte _unk7;
       public byte _unk8;
       public byte _unk9;
       public byte _unk10;
       public uint _nulls;
       fixed byte _trigger1[4];
       fixed byte _trigger2[4];

       // Brawl's SSE files use float values of 1.0 (3f800000) as padding to ensure
       // commonly interfacing files will match. The choice to use a float is unknown,
       // however it could have been a debugging failsafe.
       fixed byte _pad4[60];

       public string DoorID
       {
           get
           {
               byte[] bytes = new byte[3];
               string s1 = "";
               for (int i = 0; i < 3; i++)
               {
                   bytes[i] = *(byte*)((VoidPtr)Address + 0x30 + i);
                   if (bytes[i].ToString("x").Length < 2) { s1 += bytes[i].ToString("x").PadLeft(2, '0'); }
                   else
                   { s1 += bytes[i].ToString("x").ToUpper(); }
               }
               return s1;

           }
           set
           {

               if (value == null)
                   value = "";

               fixed (byte* ptr = _stageID)
               {
                   for (int i = 0; i < value.Length; i++)
                   {
                       ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                   }
               }
           }
       }
       public string Trigger0
       {
           get
           {
               byte[] bytes = new byte[4];
               string s1 = "";
               for (int i = 0; i < 4; i++)
               {
                   bytes[i] = *(byte*)((VoidPtr)Address + 0x40 + i);
                   if (bytes[i].ToString("x").Length < 2) { s1 += bytes[i].ToString("x").PadLeft(2, '0'); }
                   else
                   { s1 += bytes[i].ToString("x").ToUpper(); }
               }
               return s1;

           }
           set
           {

               if (value == null)
                   value = "";

               fixed (byte* ptr = _trigger0)
               {
                   for (int i = 0; i < value.Length; i++)
                   {
                       ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                   }
               }
           }
       }
       public string Trigger1
       {
           get
           {
               byte[] bytes = new byte[4];
               string s1 = "";
               for (int i = 0; i < 4; i++)
               {
                   bytes[i] = *(byte*)((VoidPtr)Address + 0x4C + i);
                   if (bytes[i].ToString("x").Length < 2) { s1 += bytes[i].ToString("x").PadLeft(2, '0'); }
                   else
                   { s1 += bytes[i].ToString("x").ToUpper(); }
               }
               return s1;

           }
           set
           {

               if (value == null)
                   value = "";

               fixed (byte* ptr = _trigger1)
               {
                   for (int i = 0; i < value.Length; i++)
                   {
                       ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                   }
               }
           }
       }
       public string Trigger2
       {
           get
           {
               byte[] bytes = new byte[4];
               string s1 = "";
               for (int i = 0; i < 4; i++)
               {
                   bytes[i] = *(byte*)((VoidPtr)Address + 0x50 + i);
                   if (bytes[i].ToString("x").Length < 2) { s1 += bytes[i].ToString("x").PadLeft(2, '0'); }
                   else
                   { s1 += bytes[i].ToString("x").ToUpper(); }
               }
               return s1;

           }
           set
           {

               if (value == null)
                   value = "";

               fixed (byte* ptr = _trigger2)
               {
                   for (int i = 0; i < value.Length; i++)
                   {
                       ptr[i / 2] = Convert.ToByte(value.Substring(i++, 2), 16);
                   }
               }
           }
       }
       public float Pad4
       {
           set
           {
               fixed (byte* ptr = _pad4)
               {
                   for (int i = 0; i <= 60; i+=4)
                   {
                       ptr[i] = 0x3f;
                       ptr[i + 1] = 0x80;
                       ptr[i + 2] = 0x00;
                       ptr[i + 3] = 0x00;
                   }
               }
           }
       }
       public byte Pad2
       {
           set
           {
               fixed (byte* ptr = _pad2)
               {
                   for (int i = 0; i <= 19; i++)
                   {
                       ptr[i] = 0;
                   }
               }
           }
       }

       public GDOREntry(string stageID, string trigger0, string trigger1, string trigger2, int modelIndex)
       {
           _pad0 = 1.0f;
           _unk0 = _unk1 = _unk2 = _unk3 =
           _unk4 = _unk5 = _unk6 = 0;
           _pad1 = 0;
           _unkInt = 0;
           _xOverride = _yOverride = _zOverride = 
           _unkFloat0 = _unkFloat1 = 0;
           _unk7 = 1;
           _unk8 = _unk9 = _unk10 = 0;
           _nulls = 0xffffffff;
           _mdlIndex = (byte)modelIndex;
           _doorIndex = 0;
           DoorID = stageID;
           Trigger0 = trigger0;
           Trigger1 = trigger1;
           Trigger2 = trigger2;
           Pad2 = 0;
           Pad4 = 1.0f;
       }

       public VoidPtr Address { get { fixed (void* ptr = &this)return ptr; } }
   }
}
