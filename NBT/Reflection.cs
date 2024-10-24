using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConMaster.Deepslate.NBT
{
    /*
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    internal sealed class NBTTagTypeAttribute : Attribute
    {
        private static Dictionary<Type, NBTTagType> _dictionary = new();
        public NBTTagTypeAttribute(Type selfType, NBTTagType type)
        {
            TagType = type;
        }
        public readonly NBTTagType TagType;
        public static NBTTagType? GetTagType(object value) => value.GetType().GetCustomAttribute<NBTTagTypeAttribute>()?.TagType;
    }*/
}
