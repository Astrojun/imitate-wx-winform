using System;
using System.Collections.Generic;
using System.Text;

namespace WindowsFormsTest2.ClassInfo
{
    public class MyFormatter : IFormatProvider,ICustomFormatter
    {

        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter))
            {
                return this;
            }
            return null;
        }

        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                if (arg is IFormattable)
                {
                    return ((IFormattable)arg).ToString(format, formatProvider);
                }
                return arg.ToString();
            }
            else
            {
                if (format.Length > 10 && format.Substring(0, 11) == "MyFormatter")
                {
                    double result;
                    if (double.TryParse(arg.ToString(), out result))
                    {
                        return result > 10000 ? (result / 10000).ToString("f" + format.Substring(11)) + "万" : result.ToString("f" + format.Substring(11));
                    }
                    return arg.ToString();
                }
                else
                {
                    if (arg is IFormattable)
                    {
                        return ((IFormattable)arg).ToString(format, formatProvider);
                    }
                    return arg.ToString();
                }
            }
        }
    }
}
