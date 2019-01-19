using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Utility
{
    public static class CultureHelper
    {
        public enum LanguageEnum
        {
            [Description("zh-TW")]
            Chinese,
            [Description("en-US")]
            English
        }

        /// <summary>
        /// 取得合法語系名稱(尚未實作則給予預設值)
        /// </summary>
        /// <param name="name">語系名稱 (e.g. en-US)</param>
        public static string GetImplementedCulture(string name)
        {
            // give a default culture just in case
            string cultureName = GetDefaultCulture();

            // check if it's implemented
            if (EnumHelper.TryGetValueFromDescription<LanguageEnum>(name))
                cultureName = name;

            return cultureName;
        }

        /// <summary>
        /// 取得預設 語系名稱
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultCulture()
        {
            return LanguageEnum.English.GetDescription();
        }

        /// <summary>
        /// 取得目前 語系
        /// </summary>
        /// <returns></returns>
        public static LanguageEnum GetCurrentLanguage()
        {
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;

            // get implemented culture name
            currentCulture = GetImplementedCulture(currentCulture);

            // get language by implemented culture name
            return EnumHelper.GetValueFromDescription<LanguageEnum>(currentCulture);
        }
    }
}
