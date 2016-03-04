﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.GreenThumb.BusinessObjects
{
    public class MoneyPending : Money
    {
        /// <summary>
        /// Author: Ryan Taylor
        /// Data Transfer Object to represent Pending Money Donations  from the
        /// Database
        /// 
        /// Added 3/3 By Trevor Glisch
        /// </summary>
        public int DonationID { get; set; }
        public int NeedID { get; set; }
        public DateTime Date { get; set; }

        public MoneyPending(int userID, bool active,
            int donationID, int needID, DateTime date)
            : base(userID, active)
        {
            this.DonationID = donationID;
            this.NeedID = needID;
            this.Date = date;
        }
    }
}
