using System;
using System.Xml;

namespace UniTiled.Util
{
    public class XmlHelper
    {
        public static T GetAttribute<T>(XmlNode node, string name, T defaultValue)
        {
            if (node.Attributes == null)
                return defaultValue;

            var attr = node.Attributes[name];
            if (attr == null)
                return defaultValue;

            return (T)Convert.ChangeType(attr.InnerText, typeof(T));
        }
    }
}

