using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public record PhoneNumber :ValueObject
    {
        private static readonly Regex IranPhoneRegex = new(
            @"^(\+98|0)?9\d{9}$",
            RegexOptions.Compiled | RegexOptions.Singleline);

        public string Value { get; private set; }

        private PhoneNumber() { } // برای EF Core

        public PhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("شماره موبایل الزامی است.");

            var trimmed = value.Trim();

            // نرمال‌سازی شماره (مثلاً تبدیل +98912 به 0912)
            if (trimmed.StartsWith("+98"))
                trimmed = "0" + trimmed[3..];
            else if (trimmed.StartsWith("98"))
                trimmed = "0" + trimmed[2..];

            if (!IranPhoneRegex.IsMatch(trimmed))
                throw new ArgumentException("فرمت شماره موبایل نامعتبر است.");

            Value = trimmed;
        }

        public static implicit operator string(PhoneNumber phone) => phone.Value;
        public static explicit operator PhoneNumber(string phone) => new(phone);

      
        public override string ToString() => Value;
    }
}
