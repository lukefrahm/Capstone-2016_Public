﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.GreenThumb.BusinessObjects
{
    public class SoilPending : Soil
    {
        public int DonatedID { get; set; }
        public int NeededID { get; set; }
        public DateTime Date { get; set; }

        public SoilPending(int id, string name, string soilType, int userID, bool active,
            int donatedID, int neededID, DateTime date)
            : base(id, name, soilType, userID, active)
        {
            this.DonatedID = donatedID;
            this.NeededID = neededID;
            this.Date = date;
        }
    }
}
