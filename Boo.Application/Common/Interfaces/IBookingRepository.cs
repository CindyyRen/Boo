﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boo.Domain.Entities;

namespace Boo.Application.Common.Interfaces
{
    public interface IBookingRepository : IRepository<Booking>
    {
        void Update(Booking entity);
        void UpdateStatus(int bookingId, string bookingStatus);
        void UpdateStripePaymentID(int bookingId, string sessionId, string paymentIntentId);
    }
}
