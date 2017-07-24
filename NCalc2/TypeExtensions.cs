using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System
{

    public enum MyTypeCode
    {
        Boolean,
        Byte,
        SByte,
        Char,
        DateTime,
        Decimal,
        Double,
        Single,
        Int16,
        Int32,
        Int64,
        UInt16,
        UInt32,
        UInt64,
        String,
        Empty,
        Object
    }

    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, MyTypeCode> TypeCodeMap =
            new Dictionary<Type, MyTypeCode>
            {
                {typeof(bool), MyTypeCode.Boolean},
                {typeof(byte), MyTypeCode.Byte},
                {typeof(sbyte), MyTypeCode.SByte},
                {typeof(char), MyTypeCode.Char},
                {typeof(DateTime), MyTypeCode.DateTime},
                {typeof(decimal), MyTypeCode.Decimal},
                {typeof(double), MyTypeCode.Double},
                {typeof(float), MyTypeCode.Single},
                {typeof(short), MyTypeCode.Int16},
                {typeof(int), MyTypeCode.Int32},
                {typeof(long), MyTypeCode.Int64},
                {typeof(ushort), MyTypeCode.UInt16},
                {typeof(uint), MyTypeCode.UInt32},
                {typeof(ulong), MyTypeCode.UInt64},
                {typeof(string), MyTypeCode.String}
            };

        public static MyTypeCode GetTypeCode(this object obj)
        {
            if (obj == null)
                return MyTypeCode.Empty;

            MyTypeCode tc;
            var type = obj.GetType();
            if (!TypeCodeMap.TryGetValue(type, out tc))
            {
                tc = MyTypeCode.Object;
            }

            return tc;
        }
    }
}
