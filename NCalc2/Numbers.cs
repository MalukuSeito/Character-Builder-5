using System;

namespace NCalc
{
    public class Numbers
    {
        private static object ConvertIfString(object s)
        {
            if (s is String|| s is char)
            {
                return Decimal.Parse(s.ToString());
            }

            return s;
        }

        public static object Add(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Boolean:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'"); 
                        case MyTypeCode.Byte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'"); 
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'"); 
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                    }
                    break;
                case MyTypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
                        case MyTypeCode.Byte: return (Byte)a + (Byte)b;
                        case MyTypeCode.SByte: return (Byte)a + (SByte)b;
                        case MyTypeCode.Int16: return (Byte)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Byte)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Byte)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Byte)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Byte)a + (Int64)b;
                        case MyTypeCode.UInt64: return (Byte)a + (UInt64)b;
                        case MyTypeCode.Single: return (Byte)a + (Single)b;
                        case MyTypeCode.Double: return (Byte)a + (Double)b;
                        case MyTypeCode.Decimal: return (Byte)a + (Decimal)b;
                    }
                    break;
                case MyTypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
                        case MyTypeCode.Byte: return (SByte)a + (Byte)b;
                        case MyTypeCode.SByte: return (SByte)a + (SByte)b;
                        case MyTypeCode.Int16: return (SByte)a + (Int16)b;
                        case MyTypeCode.UInt16: return (SByte)a + (UInt16)b;
                        case MyTypeCode.Int32: return (SByte)a + (Int32)b;
                        case MyTypeCode.UInt32: return (SByte)a + (UInt32)b;
                        case MyTypeCode.Int64: return (SByte)a + (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case MyTypeCode.Single: return (SByte)a + (Single)b;
                        case MyTypeCode.Double: return (SByte)a + (Double)b;
                        case MyTypeCode.Decimal: return (SByte)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
                        case MyTypeCode.Byte: return (Int16)a + (Byte)b;
                        case MyTypeCode.SByte: return (Int16)a + (SByte)b;
                        case MyTypeCode.Int16: return (Int16)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Int16)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Int16)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Int16)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Int16)a + (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
                        case MyTypeCode.Single: return (Int16)a + (Single)b;
                        case MyTypeCode.Double: return (Int16)a + (Double)b;
                        case MyTypeCode.Decimal: return (Int16)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
                        case MyTypeCode.Byte: return (UInt16)a + (Byte)b;
                        case MyTypeCode.SByte: return (UInt16)a + (SByte)b;
                        case MyTypeCode.Int16: return (UInt16)a + (Int16)b;
                        case MyTypeCode.UInt16: return (UInt16)a + (UInt16)b;
                        case MyTypeCode.Int32: return (UInt16)a + (Int32)b;
                        case MyTypeCode.UInt32: return (UInt16)a + (UInt32)b;
                        case MyTypeCode.Int64: return (UInt16)a + (Int64)b;
                        case MyTypeCode.UInt64: return (UInt16)a + (UInt64)b;
                        case MyTypeCode.Single: return (UInt16)a + (Single)b;
                        case MyTypeCode.Double: return (UInt16)a + (Double)b;
                        case MyTypeCode.Decimal: return (UInt16)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
                        case MyTypeCode.Byte: return (Int32)a + (Byte)b;
                        case MyTypeCode.SByte: return (Int32)a + (SByte)b;
                        case MyTypeCode.Int16: return (Int32)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Int32)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Int32)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Int32)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Int32)a + (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
                        case MyTypeCode.Single: return (Int32)a + (Single)b;
                        case MyTypeCode.Double: return (Int32)a + (Double)b;
                        case MyTypeCode.Decimal: return (Int32)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
                        case MyTypeCode.Byte: return (UInt32)a + (Byte)b;
                        case MyTypeCode.SByte: return (UInt32)a + (SByte)b;
                        case MyTypeCode.Int16: return (UInt32)a + (Int16)b;
                        case MyTypeCode.UInt16: return (UInt32)a + (UInt16)b;
                        case MyTypeCode.Int32: return (UInt32)a + (Int32)b;
                        case MyTypeCode.UInt32: return (UInt32)a + (UInt32)b;
                        case MyTypeCode.Int64: return (UInt32)a + (Int64)b;
                        case MyTypeCode.UInt64: return (UInt32)a + (UInt64)b;
                        case MyTypeCode.Single: return (UInt32)a + (Single)b;
                        case MyTypeCode.Double: return (UInt32)a + (Double)b;
                        case MyTypeCode.Decimal: return (UInt32)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
                        case MyTypeCode.Byte: return (Int64)a + (Byte)b;
                        case MyTypeCode.SByte: return (Int64)a + (SByte)b;
                        case MyTypeCode.Int16: return (Int64)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Int64)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Int64)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Int64)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Int64)a + (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
                        case MyTypeCode.Single: return (Int64)a + (Single)b;
                        case MyTypeCode.Double: return (Int64)a + (Double)b;
                        case MyTypeCode.Decimal: return (Int64)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
                        case MyTypeCode.Byte: return (UInt64)a + (Byte)b;
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
                        case MyTypeCode.UInt16: return (UInt64)a + (UInt16)b;
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
                        case MyTypeCode.UInt32: return (UInt64)a + (UInt32)b;
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'ulong'");
                        case MyTypeCode.UInt64: return (UInt64)a + (UInt64)b;
                        case MyTypeCode.Single: return (UInt64)a + (Single)b;
                        case MyTypeCode.Double: return (UInt64)a + (Double)b;
                        case MyTypeCode.Decimal: return (UInt64)a + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Single:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
                        case MyTypeCode.Byte: return (Single)a + (Byte)b;
                        case MyTypeCode.SByte: return (Single)a + (SByte)b;
                        case MyTypeCode.Int16: return (Single)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Single)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Single)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Single)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Single)a + (Int64)b;
                        case MyTypeCode.UInt64: return (Single)a + (UInt64)b;
                        case MyTypeCode.Single: return (Single)a + (Single)b;
                        case MyTypeCode.Double: return (Single)a + (Double)b;
                        case MyTypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Double:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
                        case MyTypeCode.Byte: return (Double)a + (Byte)b;
                        case MyTypeCode.SByte: return (Double)a + (SByte)b;
                        case MyTypeCode.Int16: return (Double)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Double)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Double)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Double)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Double)a + (Int64)b;
                        case MyTypeCode.UInt64: return (Double)a + (UInt64)b;
                        case MyTypeCode.Single: return (Double)a + (Single)b;
                        case MyTypeCode.Double: return (Double)a + (Double)b;
                        case MyTypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                    }
                    break;

                case MyTypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
                        case MyTypeCode.Byte: return (Decimal)a + (Byte)b;
                        case MyTypeCode.SByte: return (Decimal)a + (SByte)b;
                        case MyTypeCode.Int16: return (Decimal)a + (Int16)b;
                        case MyTypeCode.UInt16: return (Decimal)a + (UInt16)b;
                        case MyTypeCode.Int32: return (Decimal)a + (Int32)b;
                        case MyTypeCode.UInt32: return (Decimal)a + (UInt32)b;
                        case MyTypeCode.Int64: return (Decimal)a + (Int64)b;
                        case MyTypeCode.UInt64: return (Decimal)a + (UInt64)b;
                        case MyTypeCode.Single: return (Decimal)a + Convert.ToDecimal(b);
                        case MyTypeCode.Double: return (Decimal)a + Convert.ToDecimal(b);
                        case MyTypeCode.Decimal: return (Decimal)a + (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        public static object AddChecked(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();
            checked
            {
                switch (typeCodeA)
                {
                    case MyTypeCode.Boolean:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'bool'");
                            case MyTypeCode.Byte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Single: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Double: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'bool' and 'byte'");
                        }
                        break;
                    case MyTypeCode.Byte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'byte' and 'bool'");
                            case MyTypeCode.Byte: return (Byte)a + (Byte)b;
                            case MyTypeCode.SByte: return (Byte)a + (SByte)b;
                            case MyTypeCode.Int16: return (Byte)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Byte)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Byte)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Byte)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Byte)a + (Int64)b;
                            case MyTypeCode.UInt64: return (Byte)a + (UInt64)b;
                            case MyTypeCode.Single: return (Byte)a + (Single)b;
                            case MyTypeCode.Double: return (Byte)a + (Double)b;
                            case MyTypeCode.Decimal: return (Byte)a + (Decimal)b;
                        }
                        break;
                    case MyTypeCode.SByte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'bool'");
                            case MyTypeCode.Byte: return (SByte)a + (Byte)b;
                            case MyTypeCode.SByte: return (SByte)a + (SByte)b;
                            case MyTypeCode.Int16: return (SByte)a + (Int16)b;
                            case MyTypeCode.UInt16: return (SByte)a + (UInt16)b;
                            case MyTypeCode.Int32: return (SByte)a + (Int32)b;
                            case MyTypeCode.UInt32: return (SByte)a + (UInt32)b;
                            case MyTypeCode.Int64: return (SByte)a + (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'sbyte' and 'ulong'");
                            case MyTypeCode.Single: return (SByte)a + (Single)b;
                            case MyTypeCode.Double: return (SByte)a + (Double)b;
                            case MyTypeCode.Decimal: return (SByte)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'bool'");
                            case MyTypeCode.Byte: return (Int16)a + (Byte)b;
                            case MyTypeCode.SByte: return (Int16)a + (SByte)b;
                            case MyTypeCode.Int16: return (Int16)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Int16)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Int16)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Int16)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Int16)a + (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'short' and 'ulong'");
                            case MyTypeCode.Single: return (Int16)a + (Single)b;
                            case MyTypeCode.Double: return (Int16)a + (Double)b;
                            case MyTypeCode.Decimal: return (Int16)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ushort' and 'bool'");
                            case MyTypeCode.Byte: return (UInt16)a + (Byte)b;
                            case MyTypeCode.SByte: return (UInt16)a + (SByte)b;
                            case MyTypeCode.Int16: return (UInt16)a + (Int16)b;
                            case MyTypeCode.UInt16: return (UInt16)a + (UInt16)b;
                            case MyTypeCode.Int32: return (UInt16)a + (Int32)b;
                            case MyTypeCode.UInt32: return (UInt16)a + (UInt32)b;
                            case MyTypeCode.Int64: return (UInt16)a + (Int64)b;
                            case MyTypeCode.UInt64: return (UInt16)a + (UInt64)b;
                            case MyTypeCode.Single: return (UInt16)a + (Single)b;
                            case MyTypeCode.Double: return (UInt16)a + (Double)b;
                            case MyTypeCode.Decimal: return (UInt16)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'bool'");
                            case MyTypeCode.Byte: return (Int32)a + (Byte)b;
                            case MyTypeCode.SByte: return (Int32)a + (SByte)b;
                            case MyTypeCode.Int16: return (Int32)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Int32)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Int32)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Int32)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Int32)a + (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'int' and 'ulong'");
                            case MyTypeCode.Single: return (Int32)a + (Single)b;
                            case MyTypeCode.Double: return (Int32)a + (Double)b;
                            case MyTypeCode.Decimal: return (Int32)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'unit' and 'bool'");
                            case MyTypeCode.Byte: return (UInt32)a + (Byte)b;
                            case MyTypeCode.SByte: return (UInt32)a + (SByte)b;
                            case MyTypeCode.Int16: return (UInt32)a + (Int16)b;
                            case MyTypeCode.UInt16: return (UInt32)a + (UInt16)b;
                            case MyTypeCode.Int32: return (UInt32)a + (Int32)b;
                            case MyTypeCode.UInt32: return (UInt32)a + (UInt32)b;
                            case MyTypeCode.Int64: return (UInt32)a + (Int64)b;
                            case MyTypeCode.UInt64: return (UInt32)a + (UInt64)b;
                            case MyTypeCode.Single: return (UInt32)a + (Single)b;
                            case MyTypeCode.Double: return (UInt32)a + (Double)b;
                            case MyTypeCode.Decimal: return (UInt32)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'bool'");
                            case MyTypeCode.Byte: return (Int64)a + (Byte)b;
                            case MyTypeCode.SByte: return (Int64)a + (SByte)b;
                            case MyTypeCode.Int16: return (Int64)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Int64)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Int64)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Int64)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Int64)a + (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'long' and 'ulong'");
                            case MyTypeCode.Single: return (Int64)a + (Single)b;
                            case MyTypeCode.Double: return (Int64)a + (Double)b;
                            case MyTypeCode.Decimal: return (Int64)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'bool'");
                            case MyTypeCode.Byte: return (UInt64)a + (Byte)b;
                            case MyTypeCode.SByte: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'sbyte'");
                            case MyTypeCode.Int16: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'short'");
                            case MyTypeCode.UInt16: return (UInt64)a + (UInt16)b;
                            case MyTypeCode.Int32: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'int'");
                            case MyTypeCode.UInt32: return (UInt64)a + (UInt32)b;
                            case MyTypeCode.Int64: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'ulong' and 'ulong'");
                            case MyTypeCode.UInt64: return (UInt64)a + (UInt64)b;
                            case MyTypeCode.Single: return (UInt64)a + (Single)b;
                            case MyTypeCode.Double: return (UInt64)a + (Double)b;
                            case MyTypeCode.Decimal: return (UInt64)a + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Single:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'float' and 'bool'");
                            case MyTypeCode.Byte: return (Single)a + (Byte)b;
                            case MyTypeCode.SByte: return (Single)a + (SByte)b;
                            case MyTypeCode.Int16: return (Single)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Single)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Single)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Single)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Single)a + (Int64)b;
                            case MyTypeCode.UInt64: return (Single)a + (UInt64)b;
                            case MyTypeCode.Single: return (Single)a + (Single)b;
                            case MyTypeCode.Double: return (Single)a + (Double)b;
                            case MyTypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Double:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'double' and 'bool'");
                            case MyTypeCode.Byte: return (Double)a + (Byte)b;
                            case MyTypeCode.SByte: return (Double)a + (SByte)b;
                            case MyTypeCode.Int16: return (Double)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Double)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Double)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Double)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Double)a + (Int64)b;
                            case MyTypeCode.UInt64: return (Double)a + (UInt64)b;
                            case MyTypeCode.Single: return (Double)a + (Single)b;
                            case MyTypeCode.Double: return (Double)a + (Double)b;
                            case MyTypeCode.Decimal: return Convert.ToDecimal(a) + (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Decimal:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '+' can't be applied to operands of types 'decimal' and 'bool'");
                            case MyTypeCode.Byte: return (Decimal)a + (Byte)b;
                            case MyTypeCode.SByte: return (Decimal)a + (SByte)b;
                            case MyTypeCode.Int16: return (Decimal)a + (Int16)b;
                            case MyTypeCode.UInt16: return (Decimal)a + (UInt16)b;
                            case MyTypeCode.Int32: return (Decimal)a + (Int32)b;
                            case MyTypeCode.UInt32: return (Decimal)a + (UInt32)b;
                            case MyTypeCode.Int64: return (Decimal)a + (Int64)b;
                            case MyTypeCode.UInt64: return (Decimal)a + (UInt64)b;
                            case MyTypeCode.Single: return (Decimal)a + Convert.ToDecimal(b);
                            case MyTypeCode.Double: return (Decimal)a + Convert.ToDecimal(b);
                            case MyTypeCode.Decimal: return (Decimal)a + (Decimal)b;
                        }
                        break;
                }

                return null;
            }
        }

        public static object Soustract(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Boolean:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
                        case MyTypeCode.Byte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                    }
                    break;
                case MyTypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
                        case MyTypeCode.SByte: return (Byte)a - (SByte)b;
                        case MyTypeCode.Int16: return (Byte)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Byte)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Byte)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Byte)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Byte)a - (Int64)b;
                        case MyTypeCode.UInt64: return (Byte)a - (UInt64)b;
                        case MyTypeCode.Single: return (Byte)a - (Single)b;
                        case MyTypeCode.Double: return (Byte)a - (Double)b;
                        case MyTypeCode.Decimal: return (Byte)a - (Decimal)b;
                    }
                    break;
                case MyTypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
                        case MyTypeCode.SByte: return (SByte)a - (SByte)b;
                        case MyTypeCode.Int16: return (SByte)a - (Int16)b;
                        case MyTypeCode.UInt16: return (SByte)a - (UInt16)b;
                        case MyTypeCode.Int32: return (SByte)a - (Int32)b;
                        case MyTypeCode.UInt32: return (SByte)a - (UInt32)b;
                        case MyTypeCode.Int64: return (SByte)a - (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case MyTypeCode.Single: return (SByte)a - (Single)b;
                        case MyTypeCode.Double: return (SByte)a - (Double)b;
                        case MyTypeCode.Decimal: return (SByte)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
                        case MyTypeCode.SByte: return (Int16)a - (SByte)b;
                        case MyTypeCode.Int16: return (Int16)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Int16)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Int16)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Int16)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Int16)a - (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
                        case MyTypeCode.Single: return (Int16)a - (Single)b;
                        case MyTypeCode.Double: return (Int16)a - (Double)b;
                        case MyTypeCode.Decimal: return (Int16)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
                        case MyTypeCode.SByte: return (UInt16)a - (SByte)b;
                        case MyTypeCode.Int16: return (UInt16)a - (Int16)b;
                        case MyTypeCode.UInt16: return (UInt16)a - (UInt16)b;
                        case MyTypeCode.Int32: return (UInt16)a - (Int32)b;
                        case MyTypeCode.UInt32: return (UInt16)a - (UInt32)b;
                        case MyTypeCode.Int64: return (UInt16)a - (Int64)b;
                        case MyTypeCode.UInt64: return (UInt16)a - (UInt64)b;
                        case MyTypeCode.Single: return (UInt16)a - (Single)b;
                        case MyTypeCode.Double: return (UInt16)a - (Double)b;
                        case MyTypeCode.Decimal: return (UInt16)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
                        case MyTypeCode.SByte: return (Int32)a - (SByte)b;
                        case MyTypeCode.Int16: return (Int32)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Int32)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Int32)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Int32)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Int32)a - (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
                        case MyTypeCode.Single: return (Int32)a - (Single)b;
                        case MyTypeCode.Double: return (Int32)a - (Double)b;
                        case MyTypeCode.Decimal: return (Int32)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
                        case MyTypeCode.SByte: return (UInt32)a - (SByte)b;
                        case MyTypeCode.Int16: return (UInt32)a - (Int16)b;
                        case MyTypeCode.UInt16: return (UInt32)a - (UInt16)b;
                        case MyTypeCode.Int32: return (UInt32)a - (Int32)b;
                        case MyTypeCode.UInt32: return (UInt32)a - (UInt32)b;
                        case MyTypeCode.Int64: return (UInt32)a - (Int64)b;
                        case MyTypeCode.UInt64: return (UInt32)a - (UInt64)b;
                        case MyTypeCode.Single: return (UInt32)a - (Single)b;
                        case MyTypeCode.Double: return (UInt32)a - (Double)b;
                        case MyTypeCode.Decimal: return (UInt32)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
                        case MyTypeCode.SByte: return (Int64)a - (SByte)b;
                        case MyTypeCode.Int16: return (Int64)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Int64)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Int64)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Int64)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Int64)a - (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
                        case MyTypeCode.Single: return (Int64)a - (Single)b;
                        case MyTypeCode.Double: return (Int64)a - (Double)b;
                        case MyTypeCode.Decimal: return (Int64)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
                        case MyTypeCode.UInt16: return (UInt64)a - (UInt16)b;
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
                        case MyTypeCode.UInt32: return (UInt64)a - (UInt32)b;
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
                        case MyTypeCode.UInt64: return (UInt64)a - (UInt64)b;
                        case MyTypeCode.Single: return (UInt64)a - (Single)b;
                        case MyTypeCode.Double: return (UInt64)a - (Double)b;
                        case MyTypeCode.Decimal: return (UInt64)a - (Decimal)b;
                    }
                    break;

                case MyTypeCode.Single:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
                        case MyTypeCode.SByte: return (Single)a - (SByte)b;
                        case MyTypeCode.Int16: return (Single)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Single)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Single)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Single)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Single)a - (Int64)b;
                        case MyTypeCode.UInt64: return (Single)a - (UInt64)b;
                        case MyTypeCode.Single: return (Single)a - (Single)b;
                        case MyTypeCode.Double: return (Single)a - (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Double:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
                        case MyTypeCode.SByte: return (Double)a - (SByte)b;
                        case MyTypeCode.Int16: return (Double)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Double)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Double)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Double)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Double)a - (Int64)b;
                        case MyTypeCode.UInt64: return (Double)a - (UInt64)b;
                        case MyTypeCode.Single: return (Double)a - (Single)b;
                        case MyTypeCode.Double: return (Double)a - (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
                        case MyTypeCode.SByte: return (Decimal)a - (SByte)b;
                        case MyTypeCode.Int16: return (Decimal)a - (Int16)b;
                        case MyTypeCode.UInt16: return (Decimal)a - (UInt16)b;
                        case MyTypeCode.Int32: return (Decimal)a - (Int32)b;
                        case MyTypeCode.UInt32: return (Decimal)a - (UInt32)b;
                        case MyTypeCode.Int64: return (Decimal)a - (Int64)b;
                        case MyTypeCode.UInt64: return (Decimal)a - (UInt64)b;
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'float'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'double'");
                        case MyTypeCode.Decimal: return (Decimal)a - (Decimal)b;
                    }
                    break;
            }

            return null;
        }
        public static object SoustractChecked(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();
            checked
            {
                switch (typeCodeA)
                {
                    case MyTypeCode.Boolean:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'bool'");
                            case MyTypeCode.Byte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Single: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Double: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'bool' and 'byte'");
                        }
                        break;
                    case MyTypeCode.Byte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'byte' and 'bool'");
                            case MyTypeCode.SByte: return (Byte)a - (SByte)b;
                            case MyTypeCode.Int16: return (Byte)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Byte)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Byte)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Byte)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Byte)a - (Int64)b;
                            case MyTypeCode.UInt64: return (Byte)a - (UInt64)b;
                            case MyTypeCode.Single: return (Byte)a - (Single)b;
                            case MyTypeCode.Double: return (Byte)a - (Double)b;
                            case MyTypeCode.Decimal: return (Byte)a - (Decimal)b;
                        }
                        break;
                    case MyTypeCode.SByte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'bool'");
                            case MyTypeCode.SByte: return (SByte)a - (SByte)b;
                            case MyTypeCode.Int16: return (SByte)a - (Int16)b;
                            case MyTypeCode.UInt16: return (SByte)a - (UInt16)b;
                            case MyTypeCode.Int32: return (SByte)a - (Int32)b;
                            case MyTypeCode.UInt32: return (SByte)a - (UInt32)b;
                            case MyTypeCode.Int64: return (SByte)a - (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'sbyte' and 'ulong'");
                            case MyTypeCode.Single: return (SByte)a - (Single)b;
                            case MyTypeCode.Double: return (SByte)a - (Double)b;
                            case MyTypeCode.Decimal: return (SByte)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'bool'");
                            case MyTypeCode.SByte: return (Int16)a - (SByte)b;
                            case MyTypeCode.Int16: return (Int16)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Int16)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Int16)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Int16)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Int16)a - (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'short' and 'ulong'");
                            case MyTypeCode.Single: return (Int16)a - (Single)b;
                            case MyTypeCode.Double: return (Int16)a - (Double)b;
                            case MyTypeCode.Decimal: return (Int16)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ushort' and 'bool'");
                            case MyTypeCode.SByte: return (UInt16)a - (SByte)b;
                            case MyTypeCode.Int16: return (UInt16)a - (Int16)b;
                            case MyTypeCode.UInt16: return (UInt16)a - (UInt16)b;
                            case MyTypeCode.Int32: return (UInt16)a - (Int32)b;
                            case MyTypeCode.UInt32: return (UInt16)a - (UInt32)b;
                            case MyTypeCode.Int64: return (UInt16)a - (Int64)b;
                            case MyTypeCode.UInt64: return (UInt16)a - (UInt64)b;
                            case MyTypeCode.Single: return (UInt16)a - (Single)b;
                            case MyTypeCode.Double: return (UInt16)a - (Double)b;
                            case MyTypeCode.Decimal: return (UInt16)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'bool'");
                            case MyTypeCode.SByte: return (Int32)a - (SByte)b;
                            case MyTypeCode.Int16: return (Int32)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Int32)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Int32)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Int32)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Int32)a - (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'int' and 'ulong'");
                            case MyTypeCode.Single: return (Int32)a - (Single)b;
                            case MyTypeCode.Double: return (Int32)a - (Double)b;
                            case MyTypeCode.Decimal: return (Int32)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'uint' and 'bool'");
                            case MyTypeCode.SByte: return (UInt32)a - (SByte)b;
                            case MyTypeCode.Int16: return (UInt32)a - (Int16)b;
                            case MyTypeCode.UInt16: return (UInt32)a - (UInt16)b;
                            case MyTypeCode.Int32: return (UInt32)a - (Int32)b;
                            case MyTypeCode.UInt32: return (UInt32)a - (UInt32)b;
                            case MyTypeCode.Int64: return (UInt32)a - (Int64)b;
                            case MyTypeCode.UInt64: return (UInt32)a - (UInt64)b;
                            case MyTypeCode.Single: return (UInt32)a - (Single)b;
                            case MyTypeCode.Double: return (UInt32)a - (Double)b;
                            case MyTypeCode.Decimal: return (UInt32)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'bool'");
                            case MyTypeCode.SByte: return (Int64)a - (SByte)b;
                            case MyTypeCode.Int16: return (Int64)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Int64)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Int64)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Int64)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Int64)a - (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'long' and 'ulong'");
                            case MyTypeCode.Single: return (Int64)a - (Single)b;
                            case MyTypeCode.Double: return (Int64)a - (Double)b;
                            case MyTypeCode.Decimal: return (Int64)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
                            case MyTypeCode.SByte: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'double'");
                            case MyTypeCode.Int16: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'short'");
                            case MyTypeCode.UInt16: return (UInt64)a - (UInt16)b;
                            case MyTypeCode.Int32: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'int'");
                            case MyTypeCode.UInt32: return (UInt64)a - (UInt32)b;
                            case MyTypeCode.Int64: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'long'");
                            case MyTypeCode.UInt64: return (UInt64)a - (UInt64)b;
                            case MyTypeCode.Single: return (UInt64)a - (Single)b;
                            case MyTypeCode.Double: return (UInt64)a - (Double)b;
                            case MyTypeCode.Decimal: return (UInt64)a - (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Single:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'bool'");
                            case MyTypeCode.SByte: return (Single)a - (SByte)b;
                            case MyTypeCode.Int16: return (Single)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Single)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Single)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Single)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Single)a - (Int64)b;
                            case MyTypeCode.UInt64: return (Single)a - (UInt64)b;
                            case MyTypeCode.Single: return (Single)a - (Single)b;
                            case MyTypeCode.Double: return (Single)a - (Double)b;
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'float' and 'decimal'");
                        }
                        break;

                    case MyTypeCode.Double:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'bool'");
                            case MyTypeCode.SByte: return (Double)a - (SByte)b;
                            case MyTypeCode.Int16: return (Double)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Double)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Double)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Double)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Double)a - (Int64)b;
                            case MyTypeCode.UInt64: return (Double)a - (UInt64)b;
                            case MyTypeCode.Single: return (Double)a - (Single)b;
                            case MyTypeCode.Double: return (Double)a - (Double)b;
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'double' and 'decimal'");
                        }
                        break;

                    case MyTypeCode.Decimal:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'bool'");
                            case MyTypeCode.SByte: return (Decimal)a - (SByte)b;
                            case MyTypeCode.Int16: return (Decimal)a - (Int16)b;
                            case MyTypeCode.UInt16: return (Decimal)a - (UInt16)b;
                            case MyTypeCode.Int32: return (Decimal)a - (Int32)b;
                            case MyTypeCode.UInt32: return (Decimal)a - (UInt32)b;
                            case MyTypeCode.Int64: return (Decimal)a - (Int64)b;
                            case MyTypeCode.UInt64: return (Decimal)a - (UInt64)b;
                            case MyTypeCode.Single: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'float'");
                            case MyTypeCode.Double: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'decimal' and 'double'");
                            case MyTypeCode.Decimal: return (Decimal)a - (Decimal)b;
                        }
                        break;
                }
            }
            return null;
        }
        public static object Multiply(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
                        case MyTypeCode.SByte: return (Byte)a * (SByte)b;
                        case MyTypeCode.Int16: return (Byte)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Byte)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Byte)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Byte)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Byte)a * (Int64)b;
                        case MyTypeCode.UInt64: return (Byte)a * (UInt64)b;
                        case MyTypeCode.Single: return (Byte)a * (Single)b;
                        case MyTypeCode.Double: return (Byte)a * (Double)b;
                        case MyTypeCode.Decimal: return (Byte)a * (Decimal)b;
                    }
                    break;
                case MyTypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
                        case MyTypeCode.SByte: return (SByte)a * (SByte)b;
                        case MyTypeCode.Int16: return (SByte)a * (Int16)b;
                        case MyTypeCode.UInt16: return (SByte)a * (UInt16)b;
                        case MyTypeCode.Int32: return (SByte)a * (Int32)b;
                        case MyTypeCode.UInt32: return (SByte)a * (UInt32)b;
                        case MyTypeCode.Int64: return (SByte)a * (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case MyTypeCode.Single: return (SByte)a * (Single)b;
                        case MyTypeCode.Double: return (SByte)a * (Double)b;
                        case MyTypeCode.Decimal: return (SByte)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
                        case MyTypeCode.SByte: return (Int16)a * (SByte)b;
                        case MyTypeCode.Int16: return (Int16)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Int16)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Int16)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Int16)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Int16)a * (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
                        case MyTypeCode.Single: return (Int16)a * (Single)b;
                        case MyTypeCode.Double: return (Int16)a * (Double)b;
                        case MyTypeCode.Decimal: return (Int16)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
                        case MyTypeCode.SByte: return (UInt16)a * (SByte)b;
                        case MyTypeCode.Int16: return (UInt16)a * (Int16)b;
                        case MyTypeCode.UInt16: return (UInt16)a * (UInt16)b;
                        case MyTypeCode.Int32: return (UInt16)a * (Int32)b;
                        case MyTypeCode.UInt32: return (UInt16)a * (UInt32)b;
                        case MyTypeCode.Int64: return (UInt16)a * (Int64)b;
                        case MyTypeCode.UInt64: return (UInt16)a * (UInt64)b;
                        case MyTypeCode.Single: return (UInt16)a * (Single)b;
                        case MyTypeCode.Double: return (UInt16)a * (Double)b;
                        case MyTypeCode.Decimal: return (UInt16)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
                        case MyTypeCode.SByte: return (Int32)a * (SByte)b;
                        case MyTypeCode.Int16: return (Int32)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Int32)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Int32)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Int32)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Int32)a * (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
                        case MyTypeCode.Single: return (Int32)a * (Single)b;
                        case MyTypeCode.Double: return (Int32)a * (Double)b;
                        case MyTypeCode.Decimal: return (Int32)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
                        case MyTypeCode.SByte: return (UInt32)a * (SByte)b;
                        case MyTypeCode.Int16: return (UInt32)a * (Int16)b;
                        case MyTypeCode.UInt16: return (UInt32)a * (UInt16)b;
                        case MyTypeCode.Int32: return (UInt32)a * (Int32)b;
                        case MyTypeCode.UInt32: return (UInt32)a * (UInt32)b;
                        case MyTypeCode.Int64: return (UInt32)a * (Int64)b;
                        case MyTypeCode.UInt64: return (UInt32)a * (UInt64)b;
                        case MyTypeCode.Single: return (UInt32)a * (Single)b;
                        case MyTypeCode.Double: return (UInt32)a * (Double)b;
                        case MyTypeCode.Decimal: return (UInt32)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
                        case MyTypeCode.SByte: return (Int64)a * (SByte)b;
                        case MyTypeCode.Int16: return (Int64)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Int64)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Int64)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Int64)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Int64)a * (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
                        case MyTypeCode.Single: return (Int64)a * (Single)b;
                        case MyTypeCode.Double: return (Int64)a * (Double)b;
                        case MyTypeCode.Decimal: return (Int64)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
                        case MyTypeCode.UInt16: return (UInt64)a * (UInt16)b;
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
                        case MyTypeCode.UInt32: return (UInt64)a * (UInt32)b;
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
                        case MyTypeCode.UInt64: return (UInt64)a * (UInt64)b;
                        case MyTypeCode.Single: return (UInt64)a * (Single)b;
                        case MyTypeCode.Double: return (UInt64)a * (Double)b;
                        case MyTypeCode.Decimal: return (UInt64)a * (Decimal)b;
                    }
                    break;

                case MyTypeCode.Single:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
                        case MyTypeCode.SByte: return (Single)a * (SByte)b;
                        case MyTypeCode.Int16: return (Single)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Single)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Single)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Single)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Single)a * (Int64)b;
                        case MyTypeCode.UInt64: return (Single)a * (UInt64)b;
                        case MyTypeCode.Single: return (Single)a * (Single)b;
                        case MyTypeCode.Double: return (Single)a * (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Double:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
                        case MyTypeCode.SByte: return (Double)a * (SByte)b;
                        case MyTypeCode.Int16: return (Double)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Double)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Double)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Double)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Double)a * (Int64)b;
                        case MyTypeCode.UInt64: return (Double)a * (UInt64)b;
                        case MyTypeCode.Single: return (Double)a * (Single)b;
                        case MyTypeCode.Double: return (Double)a * (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
                        case MyTypeCode.SByte: return (Decimal)a * (SByte)b;
                        case MyTypeCode.Int16: return (Decimal)a * (Int16)b;
                        case MyTypeCode.UInt16: return (Decimal)a * (UInt16)b;
                        case MyTypeCode.Int32: return (Decimal)a * (Int32)b;
                        case MyTypeCode.UInt32: return (Decimal)a * (UInt32)b;
                        case MyTypeCode.Int64: return (Decimal)a * (Int64)b;
                        case MyTypeCode.UInt64: return (Decimal)a * (UInt64)b;
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'float'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'double'");
                        case MyTypeCode.Decimal: return (Decimal)a * (Decimal)b;
                    }
                    break;
            }

            return null;
        }
        public static object MultiplyChecked(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();
            checked
            {
                switch (typeCodeA)
                {
                    case MyTypeCode.Byte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'byte' and 'bool'");
                            case MyTypeCode.SByte: return (Byte)a * (SByte)b;
                            case MyTypeCode.Int16: return (Byte)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Byte)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Byte)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Byte)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Byte)a * (Int64)b;
                            case MyTypeCode.UInt64: return (Byte)a * (UInt64)b;
                            case MyTypeCode.Single: return (Byte)a * (Single)b;
                            case MyTypeCode.Double: return (Byte)a * (Double)b;
                            case MyTypeCode.Decimal: return (Byte)a * (Decimal)b;
                        }
                        break;
                    case MyTypeCode.SByte:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'bool'");
                            case MyTypeCode.SByte: return (SByte)a * (SByte)b;
                            case MyTypeCode.Int16: return (SByte)a * (Int16)b;
                            case MyTypeCode.UInt16: return (SByte)a * (UInt16)b;
                            case MyTypeCode.Int32: return (SByte)a * (Int32)b;
                            case MyTypeCode.UInt32: return (SByte)a * (UInt32)b;
                            case MyTypeCode.Int64: return (SByte)a * (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'sbyte' and 'ulong'");
                            case MyTypeCode.Single: return (SByte)a * (Single)b;
                            case MyTypeCode.Double: return (SByte)a * (Double)b;
                            case MyTypeCode.Decimal: return (SByte)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'bool'");
                            case MyTypeCode.SByte: return (Int16)a * (SByte)b;
                            case MyTypeCode.Int16: return (Int16)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Int16)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Int16)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Int16)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Int16)a * (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'short' and 'ulong'");
                            case MyTypeCode.Single: return (Int16)a * (Single)b;
                            case MyTypeCode.Double: return (Int16)a * (Double)b;
                            case MyTypeCode.Decimal: return (Int16)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt16:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ushort' and 'bool'");
                            case MyTypeCode.SByte: return (UInt16)a * (SByte)b;
                            case MyTypeCode.Int16: return (UInt16)a * (Int16)b;
                            case MyTypeCode.UInt16: return (UInt16)a * (UInt16)b;
                            case MyTypeCode.Int32: return (UInt16)a * (Int32)b;
                            case MyTypeCode.UInt32: return (UInt16)a * (UInt32)b;
                            case MyTypeCode.Int64: return (UInt16)a * (Int64)b;
                            case MyTypeCode.UInt64: return (UInt16)a * (UInt64)b;
                            case MyTypeCode.Single: return (UInt16)a * (Single)b;
                            case MyTypeCode.Double: return (UInt16)a * (Double)b;
                            case MyTypeCode.Decimal: return (UInt16)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'bool'");
                            case MyTypeCode.SByte: return (Int32)a * (SByte)b;
                            case MyTypeCode.Int16: return (Int32)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Int32)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Int32)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Int32)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Int32)a * (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'int' and 'ulong'");
                            case MyTypeCode.Single: return (Int32)a * (Single)b;
                            case MyTypeCode.Double: return (Int32)a * (Double)b;
                            case MyTypeCode.Decimal: return (Int32)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt32:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'uint' and 'bool'");
                            case MyTypeCode.SByte: return (UInt32)a * (SByte)b;
                            case MyTypeCode.Int16: return (UInt32)a * (Int16)b;
                            case MyTypeCode.UInt16: return (UInt32)a * (UInt16)b;
                            case MyTypeCode.Int32: return (UInt32)a * (Int32)b;
                            case MyTypeCode.UInt32: return (UInt32)a * (UInt32)b;
                            case MyTypeCode.Int64: return (UInt32)a * (Int64)b;
                            case MyTypeCode.UInt64: return (UInt32)a * (UInt64)b;
                            case MyTypeCode.Single: return (UInt32)a * (Single)b;
                            case MyTypeCode.Double: return (UInt32)a * (Double)b;
                            case MyTypeCode.Decimal: return (UInt32)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Int64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'bool'");
                            case MyTypeCode.SByte: return (Int64)a * (SByte)b;
                            case MyTypeCode.Int16: return (Int64)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Int64)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Int64)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Int64)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Int64)a * (Int64)b;
                            case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'long' and 'ulong'");
                            case MyTypeCode.Single: return (Int64)a * (Single)b;
                            case MyTypeCode.Double: return (Int64)a * (Double)b;
                            case MyTypeCode.Decimal: return (Int64)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.UInt64:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'bool'");
                            case MyTypeCode.SByte: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'sbyte'");
                            case MyTypeCode.Int16: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'short'");
                            case MyTypeCode.UInt16: return (UInt64)a * (UInt16)b;
                            case MyTypeCode.Int32: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'int'");
                            case MyTypeCode.UInt32: return (UInt64)a * (UInt32)b;
                            case MyTypeCode.Int64: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'ulong' and 'long'");
                            case MyTypeCode.UInt64: return (UInt64)a * (UInt64)b;
                            case MyTypeCode.Single: return (UInt64)a * (Single)b;
                            case MyTypeCode.Double: return (UInt64)a * (Double)b;
                            case MyTypeCode.Decimal: return (UInt64)a * (Decimal)b;
                        }
                        break;

                    case MyTypeCode.Single:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'bool'");
                            case MyTypeCode.SByte: return (Single)a * (SByte)b;
                            case MyTypeCode.Int16: return (Single)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Single)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Single)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Single)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Single)a * (Int64)b;
                            case MyTypeCode.UInt64: return (Single)a * (UInt64)b;
                            case MyTypeCode.Single: return (Single)a * (Single)b;
                            case MyTypeCode.Double: return (Single)a * (Double)b;
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'float' and 'decimal'");
                        }
                        break;

                    case MyTypeCode.Double:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'bool'");
                            case MyTypeCode.SByte: return (Double)a * (SByte)b;
                            case MyTypeCode.Int16: return (Double)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Double)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Double)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Double)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Double)a * (Int64)b;
                            case MyTypeCode.UInt64: return (Double)a * (UInt64)b;
                            case MyTypeCode.Single: return (Double)a * (Single)b;
                            case MyTypeCode.Double: return (Double)a * (Double)b;
                            case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'double' and 'decimal'");
                        }
                        break;

                    case MyTypeCode.Decimal:
                        switch (typeCodeB)
                        {
                            case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'bool'");
                            case MyTypeCode.SByte: return (Decimal)a * (SByte)b;
                            case MyTypeCode.Int16: return (Decimal)a * (Int16)b;
                            case MyTypeCode.UInt16: return (Decimal)a * (UInt16)b;
                            case MyTypeCode.Int32: return (Decimal)a * (Int32)b;
                            case MyTypeCode.UInt32: return (Decimal)a * (UInt32)b;
                            case MyTypeCode.Int64: return (Decimal)a * (Int64)b;
                            case MyTypeCode.UInt64: return (Decimal)a * (UInt64)b;
                            case MyTypeCode.Single: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'float'");
                            case MyTypeCode.Double: throw new InvalidOperationException("Operator '*' can't be applied to operands of types 'decimal' and 'double'");
                            case MyTypeCode.Decimal: return (Decimal)a * (Decimal)b;
                        }
                        break;
                }
            }
            return null;
        }
        public static object Divide(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'byte' and 'bool'");
                        case MyTypeCode.SByte: return (Byte)a / (SByte)b;
                        case MyTypeCode.Int16: return (Byte)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Byte)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Byte)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Byte)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Byte)a / (Int64)b;
                        case MyTypeCode.UInt64: return (Byte)a / (UInt64)b;
                        case MyTypeCode.Single: return (Byte)a / (Single)b;
                        case MyTypeCode.Double: return (Byte)a / (Double)b;
                        case MyTypeCode.Decimal: return (Byte)a / (Decimal)b;
                    }
                    break;
                case MyTypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'bool'");
                        case MyTypeCode.SByte: return (SByte)a / (SByte)b;
                        case MyTypeCode.Int16: return (SByte)a / (Int16)b;
                        case MyTypeCode.UInt16: return (SByte)a / (UInt16)b;
                        case MyTypeCode.Int32: return (SByte)a / (Int32)b;
                        case MyTypeCode.UInt32: return (SByte)a / (UInt32)b;
                        case MyTypeCode.Int64: return (SByte)a / (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case MyTypeCode.Single: return (SByte)a / (Single)b;
                        case MyTypeCode.Double: return (SByte)a / (Double)b;
                        case MyTypeCode.Decimal: return (SByte)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'bool'");
                        case MyTypeCode.SByte: return (Int16)a / (SByte)b;
                        case MyTypeCode.Int16: return (Int16)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Int16)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Int16)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Int16)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Int16)a / (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'short' and 'ulong'");
                        case MyTypeCode.Single: return (Int16)a / (Single)b;
                        case MyTypeCode.Double: return (Int16)a / (Double)b;
                        case MyTypeCode.Decimal: return (Int16)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ushort' and 'bool'");
                        case MyTypeCode.SByte: return (UInt16)a / (SByte)b;
                        case MyTypeCode.Int16: return (UInt16)a / (Int16)b;
                        case MyTypeCode.UInt16: return (UInt16)a / (UInt16)b;
                        case MyTypeCode.Int32: return (UInt16)a / (Int32)b;
                        case MyTypeCode.UInt32: return (UInt16)a / (UInt32)b;
                        case MyTypeCode.Int64: return (UInt16)a / (Int64)b;
                        case MyTypeCode.UInt64: return (UInt16)a / (UInt64)b;
                        case MyTypeCode.Single: return (UInt16)a / (Single)b;
                        case MyTypeCode.Double: return (UInt16)a / (Double)b;
                        case MyTypeCode.Decimal: return (UInt16)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'bool'");
                        case MyTypeCode.SByte: return (Int32)a / (SByte)b;
                        case MyTypeCode.Int16: return (Int32)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Int32)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Int32)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Int32)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Int32)a / (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'int' and 'ulong'");
                        case MyTypeCode.Single: return (Int32)a / (Single)b;
                        case MyTypeCode.Double: return (Int32)a / (Double)b;
                        case MyTypeCode.Decimal: return (Int32)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'uint' and 'bool'");
                        case MyTypeCode.SByte: return (UInt32)a / (SByte)b;
                        case MyTypeCode.Int16: return (UInt32)a / (Int16)b;
                        case MyTypeCode.UInt16: return (UInt32)a / (UInt16)b;
                        case MyTypeCode.Int32: return (UInt32)a / (Int32)b;
                        case MyTypeCode.UInt32: return (UInt32)a / (UInt32)b;
                        case MyTypeCode.Int64: return (UInt32)a / (Int64)b;
                        case MyTypeCode.UInt64: return (UInt32)a / (UInt64)b;
                        case MyTypeCode.Single: return (UInt32)a / (Single)b;
                        case MyTypeCode.Double: return (UInt32)a / (Double)b;
                        case MyTypeCode.Decimal: return (UInt32)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'bool'");
                        case MyTypeCode.SByte: return (Int64)a / (SByte)b;
                        case MyTypeCode.Int16: return (Int64)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Int64)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Int64)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Int64)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Int64)a / (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'long' and 'ulong'");
                        case MyTypeCode.Single: return (Int64)a / (Single)b;
                        case MyTypeCode.Double: return (Int64)a / (Double)b;
                        case MyTypeCode.Decimal: return (Int64)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '-' can't be applied to operands of types 'ulong' and 'bool'");
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'short'");
                        case MyTypeCode.UInt16: return (UInt64)a / (UInt16)b;
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'int'");
                        case MyTypeCode.UInt32: return (UInt64)a / (UInt32)b;
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'ulong' and 'long'");
                        case MyTypeCode.UInt64: return (UInt64)a / (UInt64)b;
                        case MyTypeCode.Single: return (UInt64)a / (Single)b;
                        case MyTypeCode.Double: return (UInt64)a / (Double)b;
                        case MyTypeCode.Decimal: return (UInt64)a / (Decimal)b;
                    }
                    break;

                case MyTypeCode.Single:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'bool'");
                        case MyTypeCode.SByte: return (Single)a / (SByte)b;
                        case MyTypeCode.Int16: return (Single)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Single)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Single)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Single)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Single)a / (Int64)b;
                        case MyTypeCode.UInt64: return (Single)a / (UInt64)b;
                        case MyTypeCode.Single: return (Single)a / (Single)b;
                        case MyTypeCode.Double: return (Single)a / (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'float' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Double:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'bool'");
                        case MyTypeCode.SByte: return (Double)a / (SByte)b;
                        case MyTypeCode.Int16: return (Double)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Double)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Double)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Double)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Double)a / (Int64)b;
                        case MyTypeCode.UInt64: return (Double)a / (UInt64)b;
                        case MyTypeCode.Single: return (Double)a / (Single)b;
                        case MyTypeCode.Double: return (Double)a / (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'double' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'bool'");
                        case MyTypeCode.SByte: return (Decimal)a / (SByte)b;
                        case MyTypeCode.Int16: return (Decimal)a / (Int16)b;
                        case MyTypeCode.UInt16: return (Decimal)a / (UInt16)b;
                        case MyTypeCode.Int32: return (Decimal)a / (Int32)b;
                        case MyTypeCode.UInt32: return (Decimal)a / (UInt32)b;
                        case MyTypeCode.Int64: return (Decimal)a / (Int64)b;
                        case MyTypeCode.UInt64: return (Decimal)a / (UInt64)b;
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'float'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '/' can't be applied to operands of types 'decimal' and 'double'");
                        case MyTypeCode.Decimal: return (Decimal)a / (Decimal)b;
                    }
                    break;
            }

            return null;
        }

        public static object Modulo(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            MyTypeCode typeCodeA = a.GetTypeCode();
            MyTypeCode typeCodeB = b.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Byte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'byte' and 'bool'");
                        case MyTypeCode.SByte: return (Byte)a % (SByte)b;
                        case MyTypeCode.Int16: return (Byte)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Byte)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Byte)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Byte)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Byte)a % (Int64)b;
                        case MyTypeCode.UInt64: return (Byte)a % (UInt64)b;
                        case MyTypeCode.Single: return (Byte)a % (Single)b;
                        case MyTypeCode.Double: return (Byte)a % (Double)b;
                        case MyTypeCode.Decimal: return (Byte)a % (Decimal)b;
                    }
                    break;
                case MyTypeCode.SByte:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'bool'");
                        case MyTypeCode.SByte: return (SByte)a % (SByte)b;
                        case MyTypeCode.Int16: return (SByte)a % (Int16)b;
                        case MyTypeCode.UInt16: return (SByte)a % (UInt16)b;
                        case MyTypeCode.Int32: return (SByte)a % (Int32)b;
                        case MyTypeCode.UInt32: return (SByte)a % (UInt32)b;
                        case MyTypeCode.Int64: return (SByte)a % (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'sbyte' and 'ulong'");
                        case MyTypeCode.Single: return (SByte)a % (Single)b;
                        case MyTypeCode.Double: return (SByte)a % (Double)b;
                        case MyTypeCode.Decimal: return (SByte)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'bool'");
                        case MyTypeCode.SByte: return (Int16)a % (SByte)b;
                        case MyTypeCode.Int16: return (Int16)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Int16)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Int16)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Int16)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Int16)a % (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'short' and 'ulong'");
                        case MyTypeCode.Single: return (Int16)a % (Single)b;
                        case MyTypeCode.Double: return (Int16)a % (Double)b;
                        case MyTypeCode.Decimal: return (Int16)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt16:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ushort' and 'bool'");
                        case MyTypeCode.SByte: return (UInt16)a % (SByte)b;
                        case MyTypeCode.Int16: return (UInt16)a % (Int16)b;
                        case MyTypeCode.UInt16: return (UInt16)a % (UInt16)b;
                        case MyTypeCode.Int32: return (UInt16)a % (Int32)b;
                        case MyTypeCode.UInt32: return (UInt16)a % (UInt32)b;
                        case MyTypeCode.Int64: return (UInt16)a % (Int64)b;
                        case MyTypeCode.UInt64: return (UInt16)a % (UInt64)b;
                        case MyTypeCode.Single: return (UInt16)a % (Single)b;
                        case MyTypeCode.Double: return (UInt16)a % (Double)b;
                        case MyTypeCode.Decimal: return (UInt16)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'bool'");
                        case MyTypeCode.SByte: return (Int32)a % (SByte)b;
                        case MyTypeCode.Int16: return (Int32)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Int32)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Int32)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Int32)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Int32)a % (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'int' and 'ulong'");
                        case MyTypeCode.Single: return (Int32)a % (Single)b;
                        case MyTypeCode.Double: return (Int32)a % (Double)b;
                        case MyTypeCode.Decimal: return (Int32)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt32:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'uint' and 'bool'");
                        case MyTypeCode.SByte: return (UInt32)a % (SByte)b;
                        case MyTypeCode.Int16: return (UInt32)a % (Int16)b;
                        case MyTypeCode.UInt16: return (UInt32)a % (UInt16)b;
                        case MyTypeCode.Int32: return (UInt32)a % (Int32)b;
                        case MyTypeCode.UInt32: return (UInt32)a % (UInt32)b;
                        case MyTypeCode.Int64: return (UInt32)a % (Int64)b;
                        case MyTypeCode.UInt64: return (UInt32)a % (UInt64)b;
                        case MyTypeCode.Single: return (UInt32)a % (Single)b;
                        case MyTypeCode.Double: return (UInt32)a % (Double)b;
                        case MyTypeCode.Decimal: return (UInt32)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.Int64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'bool'");
                        case MyTypeCode.SByte: return (Int64)a % (SByte)b;
                        case MyTypeCode.Int16: return (Int64)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Int64)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Int64)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Int64)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Int64)a % (Int64)b;
                        case MyTypeCode.UInt64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'long' and 'ulong'");
                        case MyTypeCode.Single: return (Int64)a % (Single)b;
                        case MyTypeCode.Double: return (Int64)a % (Double)b;
                        case MyTypeCode.Decimal: return (Int64)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.UInt64:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'bool'");
                        case MyTypeCode.SByte: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'sbyte'");
                        case MyTypeCode.Int16: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'short'");
                        case MyTypeCode.UInt16: return (UInt64)a % (UInt16)b;
                        case MyTypeCode.Int32: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'int'");
                        case MyTypeCode.UInt32: return (UInt64)a % (UInt32)b;
                        case MyTypeCode.Int64: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'ulong' and 'long'");
                        case MyTypeCode.UInt64: return (UInt64)a % (UInt64)b;
                        case MyTypeCode.Single: return (UInt64)a % (Single)b;
                        case MyTypeCode.Double: return (UInt64)a % (Double)b;
                        case MyTypeCode.Decimal: return (UInt64)a % (Decimal)b;
                    }
                    break;

                case MyTypeCode.Single:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'bool'");
                        case MyTypeCode.SByte: return (Single)a % (SByte)b;
                        case MyTypeCode.Int16: return (Single)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Single)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Single)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Single)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Single)a % (Int64)b;
                        case MyTypeCode.UInt64: return (Single)a % (UInt64)b;
                        case MyTypeCode.Single: return (Single)a % (Single)b;
                        case MyTypeCode.Double: return (Single)a % (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'float' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Double:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'bool'");
                        case MyTypeCode.SByte: return (Double)a % (SByte)b;
                        case MyTypeCode.Int16: return (Double)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Double)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Double)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Double)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Double)a % (Int64)b;
                        case MyTypeCode.UInt64: return (Double)a % (UInt64)b;
                        case MyTypeCode.Single: return (Double)a % (Single)b;
                        case MyTypeCode.Double: return (Double)a % (Double)b;
                        case MyTypeCode.Decimal: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'double' and 'decimal'");
                    }
                    break;

                case MyTypeCode.Decimal:
                    switch (typeCodeB)
                    {
                        case MyTypeCode.Boolean: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'bool'");
                        case MyTypeCode.SByte: return (Decimal)a % (SByte)b;
                        case MyTypeCode.Int16: return (Decimal)a % (Int16)b;
                        case MyTypeCode.UInt16: return (Decimal)a % (UInt16)b;
                        case MyTypeCode.Int32: return (Decimal)a % (Int32)b;
                        case MyTypeCode.UInt32: return (Decimal)a % (UInt32)b;
                        case MyTypeCode.Int64: return (Decimal)a % (Int64)b;
                        case MyTypeCode.UInt64: return (Decimal)a % (UInt64)b;
                        case MyTypeCode.Single: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'float'");
                        case MyTypeCode.Double: throw new InvalidOperationException("Operator '%' can't be applied to operands of types 'decimal' and 'decimal'");
                        case MyTypeCode.Decimal: return (Decimal)a % (Decimal)b;
                    }
                    break;
            }

            return null;
        }
        public static object Max(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            if (a == null && b == null)
            {
                return null;
            }

            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            MyTypeCode typeCodeA = a.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Byte:
                    return Math.Max((Byte)a, Convert.ToByte(b));
                case MyTypeCode.SByte:
                    return Math.Max((SByte)a, Convert.ToSByte(b));
                case MyTypeCode.Int16:
                    return Math.Max((Int16)a, Convert.ToInt16(b));
                case MyTypeCode.UInt16:
                    return Math.Max((UInt16)a, Convert.ToUInt16(b));
                case MyTypeCode.Int32:
                    return Math.Max((Int32)a, Convert.ToInt32(b));
                case MyTypeCode.UInt32:
                    return Math.Max((UInt32)a, Convert.ToUInt32(b));
                case MyTypeCode.Int64:
                    return Math.Max((Int64)a, Convert.ToInt64(b));
                case MyTypeCode.UInt64:
                    return Math.Max((UInt64)a, Convert.ToUInt64(b));
                case MyTypeCode.Single:
                    return Math.Max((Single)a, Convert.ToSingle(b));
                case MyTypeCode.Double:
                    return Math.Max((Double)a, Convert.ToDouble(b));
                case MyTypeCode.Decimal:
                    return Math.Max((Decimal)a, Convert.ToDecimal(b));
            }

            return null;
        }
        public static object Min(object a, object b)
        {
            a = ConvertIfString(a);
            b = ConvertIfString(b);

            if (a == null && b == null)
            {
                return null;
            }

            if (a == null)
            {
                return b;
            }

            if (b == null)
            {
                return a;
            }

            MyTypeCode typeCodeA = a.GetTypeCode();

            switch (typeCodeA)
            {
                case MyTypeCode.Byte:
                    return Math.Min((Byte)a, Convert.ToByte(b));
                case MyTypeCode.SByte:
                    return Math.Min((SByte)a, Convert.ToSByte(b));
                case MyTypeCode.Int16:
                    return Math.Min((Int16)a, Convert.ToInt16(b));
                case MyTypeCode.UInt16:
                    return Math.Min((UInt16)a, Convert.ToUInt16(b));
                case MyTypeCode.Int32:
                    return Math.Min((Int32)a, Convert.ToInt32(b));
                case MyTypeCode.UInt32:
                    return Math.Min((UInt32)a, Convert.ToUInt32(b));
                case MyTypeCode.Int64:
                    return Math.Min((Int64)a, Convert.ToInt64(b));
                case MyTypeCode.UInt64:
                    return Math.Min((UInt64)a, Convert.ToUInt64(b));
                case MyTypeCode.Single:
                    return Math.Min((Single)a, Convert.ToSingle(b));
                case MyTypeCode.Double:
                    return Math.Min((Double)a, Convert.ToDouble(b));
                case MyTypeCode.Decimal:
                    return Math.Min((Decimal)a, Convert.ToDecimal(b));
            }

            return null;
        }

    }
}
