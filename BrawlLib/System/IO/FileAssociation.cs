using Microsoft.Win32;

namespace System.IO
{
    public abstract class FileAssociation
    {
        protected string _extension;
        public string Extension { get { return _extension; } }

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
        public string Name { get { return _name; } }

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
            return Object.Equals(t1, t2);
        }
        public static bool operator !=(FileType t1, FileType t2) { return !(t1 == t2); }

        public override bool Equals(object obj)
        {
            if (obj is FileType)
                return _name.Equals(((FileType)obj)._name, StringComparison.OrdinalIgnoreCase);
            return base.Equals(obj);
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }

    internal class wFileAssociation : FileAssociation
    {
        private string _regPath;

        public override FileType FileType
        {
            get
            {
                string typeName = Registry.GetValue(_regPath, "", null) as string;
                if (String.IsNullOrEmpty(typeName))
                    return null;

                return FileType.Get(typeName);
            }
            set
            {
                if ((value == null) || String.IsNullOrEmpty(value.Name))
                    Delete();
                else
                    Registry.SetValue(_regPath, "", value.Name);
            }
        }

        internal wFileAssociation(string ext)
        {
            _extension = ext;
            _regPath = "HKEY_CLASSES_ROOT\\" + _extension;
        }

        public override void Delete()
        {
            if (!String.IsNullOrEmpty(_extension))
            {
                try { Registry.ClassesRoot.DeleteSubKeyTree(_extension); }
                catch (Exception) { }
            }
        }
    }
    internal class wFileType : FileType
    {
        private string _regPath;

        public override string Icon
        {
            get { return Registry.GetValue(_regPath + "\\DefaultIcon", "", null) as string; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    try { Registry.ClassesRoot.DeleteSubKeyTree(_name + "\\DefaultIcon"); }
                    catch (Exception) { }
                else
                    Registry.SetValue(_regPath + "\\DefaultIcon", "", value);
            }
        }

        internal wFileType(string name)
        {
            _name = name;
            _regPath = String.Format("HKEY_CLASSES_ROOT\\{0}", _name);
        }

        public override string GetCommand(string verb)
        {
            return Registry.GetValue(String.Format("{0}\\shell\\{1}\\command", _regPath, verb), "", null) as string;
        }

        public override void SetCommand(string verb, string command)
        {
            if (String.IsNullOrEmpty(command))
                try { Registry.ClassesRoot.DeleteSubKeyTree(String.Format("{0}\\shell\\{1}", _regPath, verb)); }
                catch (Exception) { }
            else
                Registry.SetValue(String.Format("{0}\\shell\\{1}\\command", _regPath, verb), "", command);
        }

        public override void Delete() { try { Registry.ClassesRoot.DeleteSubKeyTree(_name); } catch (Exception) { } }
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
