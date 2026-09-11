using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using EShop.Shared.Domain;

namespace _01.Domain.ValueObjects
{
    public record Email : ValueObject
    {
        private static readonly Regex EmailRegex = new(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; private set; }

        private Email() { } // برای EF Core

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("ایمیل الزامی است.");

            var trimmed = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(trimmed))
                throw new ArgumentException("فرمت ایمیل نامعتبر است.");

            Value = trimmed;
        }

        public static implicit operator string(Email email) => email.Value;
        public static explicit operator Email(string email) => new(email);

      
        public override string ToString() => Value;
    }
}
