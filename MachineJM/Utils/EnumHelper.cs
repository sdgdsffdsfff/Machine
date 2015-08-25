using System;
using System.ComponentModel;
using System.Reflection;

namespace MachineJMDll.Utils
{
    /// <summary>
    /// 枚举工具
    /// </summary>
    public class EnumHelper
    {
        #region 根据枚举字段名，获取枚举项描述
        /// <summary>
        /// 根据枚举字段名，获取枚举项描述，例：EnumHelper.GetDescriptionByKey(typeof(Sex), "Man")
        /// </summary>
        public static string GetDescriptionByKey(Type T, string fieldName)
        {
            FieldInfo fieldInfo = T.GetField(fieldName);

            if (fieldInfo != null)
            {
                Object[] objArray = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (objArray != null && objArray.Length > 0)
                {
                    DescriptionAttribute da = (DescriptionAttribute)objArray[0];
                    return da.Description;
                }
                else
                {
                    return fieldName;
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 根据枚举值，获取枚举项描述
        /// <summary>
        /// 根据枚举值，获取枚举项描述，例：EnumHelper.GetDescriptionByVal(typeof(Sex), 1)
        /// </summary>
        public static string GetDescriptionByVal(Type T, int val)
        {
            return EnumHelper.GetDescriptionByKey(T, Enum.Parse(T, val.ToString()).ToString());
        }
        #endregion

    }
}
