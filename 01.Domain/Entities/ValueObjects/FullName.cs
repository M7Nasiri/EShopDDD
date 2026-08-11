using _01.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Entities.ValueObjects
{
    public  record FullName
    {
        public string Value { get; }
        public FullName(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new EShopFullNameException();
            }
            Value = value.Trim();
        }
        public static implicit operator string(FullName name) => name.Value;
        public static implicit operator FullName(string name) => new(name);
    }
}
