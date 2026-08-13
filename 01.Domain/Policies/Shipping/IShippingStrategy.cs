using _01.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace _01.Domain.Policies.Shipping
{
    // Domain/Shipping/IShippingStrategy.cs
    public interface IShippingStrategy
    {
        string Key { get; }   // "standard" | "heavy" | "plus"

        // قوانین صلاحیت: هر استراتژی خودش میداند در چه شرایطی کار میکند
        bool IsApplicableTo(Weight totalWeight, bool isPlusMember);

        // محاسبه خالص هزینه — هیچ Repository اینجا نیست
        Money CalculateCost(Weight totalWeight);
    }

}
