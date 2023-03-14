using System;
using Lab04.Domain.Model;

namespace Lab04.Domain.Interface;

public interface IPaymentCalculator
{
    public float GetPrice(BookingDocument bookingDocument);
}