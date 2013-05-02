﻿// LICENSE PLACEHOLDER

using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using OpenCBS.CoreDomain.Clients;
using OpenCBS.MultiLanguageRessources;
using OpenCBS.Services;

namespace OpenCBS.GUI.UserControl
{
    [Editor(typeof(CustomizableFieldClientEditor), typeof(UITypeEditor))]
    [TypeConverter(typeof(CustomClientFieldConverter))]
    public class CustomClientField
    {
        private readonly int _clientId;
        private readonly string _fullName;

        public CustomClientField(int clientId, string fullName)
        {
            _clientId = clientId;
            _fullName = fullName;
        }

        public CustomClientField(Person person)
        {
            _clientId = person.Id;
            _fullName = person.FullName;
        }

        public string FullName
        {
            get { return _fullName; }
        }

        public int ClientId
        {
            get { return _clientId; }
        }

        public override string ToString()
        {
            return this == Empty ? string.Empty : ClientId.ToString(CultureInfo.InvariantCulture);
        }

        public static readonly CustomClientField Empty = new CustomClientField(int.MinValue, MultiLanguageStrings.GetString("Common", "CFClientNotSelected"));
    }

    public class CustomClientFieldConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value == null) return CustomClientField.Empty;

            if (value is string)
            {
                int personId;
                if(!int.TryParse(value.ToString(), out personId)) return CustomClientField.Empty;

                var service = ServicesProvider.GetInstance().GetClientServices();
                var person = service.FindPersonById(personId);
                return new CustomClientField(person);
            }            

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null) return CustomClientField.Empty.FullName;

            if (destinationType == typeof(string))
                return ((CustomClientField) value).FullName;

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
