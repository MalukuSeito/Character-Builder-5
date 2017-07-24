using System;
using System.Reflection;

namespace NCalc.Domain
{
	public class ValueExpression : LogicalExpression
	{
        public ValueExpression(object value, ValueType type)
        {
            Value = value;
            Type = type;
        }

        public ValueExpression(object value)
        {
            switch (value.GetTypeCode())
            {
                case MyTypeCode.Boolean :
                    Type = ValueType.Boolean;
                    break;

                case MyTypeCode.DateTime :
                    Type = ValueType.DateTime;
                    break;

                case MyTypeCode.Decimal:
                case MyTypeCode.Double:
                case MyTypeCode.Single:
                    Type = ValueType.Float;
                    break;

                case MyTypeCode.Byte:
                case MyTypeCode.SByte:
                case MyTypeCode.Int16:
                case MyTypeCode.Int32:
                case MyTypeCode.Int64:
                case MyTypeCode.UInt16:
                case MyTypeCode.UInt32:
                case MyTypeCode.UInt64:
                    Type = ValueType.Integer;
                    break;

                case MyTypeCode.String:
                    Type = ValueType.String;
                    break;

                default:
                    throw new EvaluationException("This value could not be handled: " + value);
            }

            Value = value;
        }

        public ValueExpression(string value)
        {
            Value = value;
            Type = ValueType.String;
        }

        public ValueExpression(int value)
        {
            Value = value;
            Type = ValueType.Integer;
        }

        public ValueExpression(float value)
        {
            Value = value;
            Type = ValueType.Float;
        }

        public ValueExpression(DateTime value)
        {
            Value = value;
            Type = ValueType.DateTime;
        }

        public ValueExpression(bool value)
        {
            Value = value;
            Type = ValueType.Boolean;
        }

        public object Value { get; set; }
        public ValueType Type { get; set; }

        public override void Accept(LogicalExpressionVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

	public enum ValueType
	{
		Integer,
		String,
		DateTime,
		Float,
		Boolean
	}
}
