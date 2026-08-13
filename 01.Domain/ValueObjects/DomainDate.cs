using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.ValueObjects
{
    public sealed record DomainDate
    {
        public DateTime Value { get; }

        public DomainDate(DateTime value)
        {
            Value = value.Kind switch
            {
                DateTimeKind.Utc => value,
                _ => value.ToUniversalTime()
            };
        }

        public static DomainDate Now =>
            new(DateTime.UtcNow);

        public bool IsBefore(DomainDate other)
        {
            ArgumentNullException.ThrowIfNull(other);

            return Value < other.Value;
        }

        public bool IsAfter(DomainDate other)
        {
            ArgumentNullException.ThrowIfNull(other);

            return Value > other.Value;
        }

        public static bool operator <(
            DomainDate left,
            DomainDate right) =>
            left.Value < right.Value;

        public static bool operator >(
            DomainDate left,
            DomainDate right) =>
            left.Value > right.Value;

        public static bool operator <=(
            DomainDate left,
            DomainDate right) =>
            left.Value <= right.Value;

        public static bool operator >=(
            DomainDate left,
            DomainDate right) =>
            left.Value >= right.Value;

        public static explicit operator DateTime(
            DomainDate date) =>
            date.Value;

        public static explicit operator DomainDate(
            DateTime date) =>
            new(date);

        public override string ToString() =>
            Value.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
