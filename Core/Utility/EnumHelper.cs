using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{

        public class EnumHelper
        {
            /// <summary>
            /// 透過標籤描述內容反射出Enum數值
            /// </summary>
            /// <typeparam name="T">Enum類別</typeparam>
            /// <param name="description">Enum描述標籤</param>
            /// <returns>Enum數值</returns>
            public static T GetValueFromDescription<T>(string description)
            {
                var type = typeof(T);
                if (!type.IsEnum) throw new InvalidOperationException();

                foreach (var field in type.GetFields())
                {
                    var attribute = Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attribute != null)
                    {
                        if (attribute.Description == description)
                            return (T)field.GetValue(null);
                    }
                    else
                    {
                        if (field.Name == description)
                            return (T)field.GetValue(null);
                    }
                }

                throw new ArgumentException("Not found.", "description");
            }

            /// <summary>
            /// 是否能透過標籤描述內容反射出Enum數值
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="description"></param>
            /// <returns></returns>
            public static bool TryGetValueFromDescription<T>(string description)
            {
                bool isOkToGetValueFromDescription = true;

                try
                {
                    GetValueFromDescription<T>(description);
                }
                catch (Exception)
                { isOkToGetValueFromDescription = false; }


                return isOkToGetValueFromDescription;
            }

        }
    }
