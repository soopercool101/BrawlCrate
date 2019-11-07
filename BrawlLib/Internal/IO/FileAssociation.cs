using Microsoft.Win32;
using System;

namespace BrawlLib.Internal.IO
{
    public abstract class FileAssociation
    {
        protected string _extension;
        public string Extension => _extension;

        public static FileAssociation Get(string extension)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return new wFileAssociation(extension);

                //case PlatformID.Unix:
                //    return new lFileAssociation(extension);
            }

            return null;
        }

        public abstract FileType FileType { get; set; }
        public abstract void Delete();
    }

    public abstract class FileType
    {
        protected string _name;
        public string Name => _name;

        public static FileType Get(string name)
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return new wFileType(name);

                //case PlatformID.Unix:
                //    return new lFileType(name);
            }

            return null;
        }

        public abstract string Icon { get; set; }

        public abstract string GetCommand(string verb);
        public abstract void SetCommand(string verb, string command);

        public abstract void Delete();

        public static bool operator ==(FileType t1, FileType t2)
        {
            return Equals(t1, t2);
        }

        public static bool operator !=(FileType t1, FileType t2)
        {
            return !(t1 == t2);
        }

        public override bool Equals(object obj)
        {
            if (obj is FileType)
            {
                return _name.Equals(((FileType) obj)._name, StringComparison.OrdinalIgnoreCase);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    internal class wFileAssociation : FileAssociation
    {
        private readonly string _regPath;

        public override FileType FileType
        {
            get
            {
                string typeName = Registry.GetValue(_regPath, "", null) as string;
                if (string.IsNullOrEmpty(typeName))
                {
                    return null;
                }

                return FileType.Get(typeName);
            }
            set
            {
                if (value == null || string.IsNullOrEmpty(value.Name))
                {
                    Delete();
                }
                else
                {
                    Registry.SetValue(_regPath, "", value.Name);
                }
            }
        }

        internal wFileAssociation(string ext)
        {
            _extension = ext;
            _regPath = "HKEY_CLASSES_ROOT\\" + _extension;
        }

        public override void Delete()
        {
            if (!string.IsNullOrEmpty(_extension))
            {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree(_extension);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }
    }

    internal class wFileType : FileType
    {
        private readonly string _regPath;

        public override string Icon
        {
            get => Registry.GetValue(_regPath + "\\DefaultIcon", "", null) as string;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    try
                    {
                        Registry.ClassesRoot.DeleteSubKeyTree(_name + "\\DefaultIcon");
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
                else
                {
                    Registry.SetValue(_regPath + "\\DefaultIcon", "", value);
                }
            }
        }

        internal wFileType(string name)
        {
            _name = name;
            _regPath = $"HKEY_CLASSES_ROOT\\{_name}";
        }

        public override string GetCommand(string verb)
        {
            return Registry.GetValue($"{_regPath}\\shell\\{verb}\\command", "", null) as string;
        }

        public override void SetCommand(string verb, string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                try
                {
                    Registry.ClassesRoot.DeleteSubKeyTree($"{_regPath}\\shell\\{verb}");
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            else
            {
                Registry.SetValue($"{_regPath}\\shell\\{verb}\\command", "", command);
            }
        }

        public override void Delete()
        {
            try
            {
                Registry.ClassesRoot.DeleteSubKeyTree(_name);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }

    //internal class lFileAssociation : FileAssociation
    //{
    //    internal lFileAssociation(string extension) { _extension = extension; }
    //}
    //internal class lFileType : FileType
    //{
    //    internal lFileType(string name) { _name = name; }
    //}
}