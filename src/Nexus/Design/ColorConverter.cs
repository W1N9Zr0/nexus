using System;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using Nexus.Graphics.Colors;

namespace Nexus.Design
{
	public class ColorConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return (sourceType == typeof(string));
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (!(value is string))
				throw GetConvertFromException(value);

			string stringValue = (string) value;

			if (stringValue.Length == 0)
				return new string[0];

			if (stringValue.StartsWith("#") && stringValue.Length == 7)
				return Color.FromHexRef(stringValue);

			string[] strArray = stringValue.Split(new[] { CultureInfo.CurrentCulture.TextInfo.ListSeparator[0] });
			if (strArray.Length == 3)
				return new Color(
					Convert.ToByte(strArray[0]),
					Convert.ToByte(strArray[1]),
					Convert.ToByte(strArray[2]));

			return base.ConvertFrom(context, culture, value);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				ConstructorInfo ci = typeof(Color).GetConstructor(new[] { typeof(double), typeof(double), typeof(double) });
				Color colour = (Color) value;
				return new InstanceDescriptor(ci, new object[] { colour.R, colour.G, colour.B });
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}