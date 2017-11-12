using System;
using System.Web;

namespace Nop.Web.Framework.Localization
{
    public class LocalizedString : MarshalByRefObject, IHtmlString
    {
        private readonly string _localized;
        private readonly string _scope;
        private readonly string _textHint;
        private readonly object[] _args;

        /// <summary>
        /// 目前就使用了这一个构造函数
        /// </summary>
        /// <param name="localized"></param>
        public LocalizedString(string localized)
        {
            _localized = localized;
        }
        /// <summary>
        /// 常用
        /// </summary>
        public string Text
        {
            get { return _localized; }
        }
        /// <summary>
        /// 没写.Text的话输出就直接ToString了
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _localized;
        }


        public LocalizedString(string localized, string scope, string textHint, object[] args)
        {
            _localized = localized;
            _scope = scope;
            _textHint = textHint;
            _args = args;
        }

        public static LocalizedString TextOrDefault(string text, LocalizedString defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;
            return new LocalizedString(text);
        }

        public string Scope
        {
            get { return _scope; }
        }

        public string TextHint
        {
            get { return _textHint; }
        }

        public object[] Args
        {
            get { return _args; }
        }

        

        public string ToHtmlString()
        {
            return _localized;
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            if (_localized != null)
                hashCode ^= _localized.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var that = (LocalizedString)obj;
            return string.Equals(_localized, that._localized);
        }

    }
}
