﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.GreenThumb.BusinessObjects
{
    public class Supply : Item
    {
        public int UserID { get; set; }
        public bool Active { get; set; }

        public Supply(int id, string name, int userID, bool active) : base(id, name)
        {
            this.UserID = userID;
            this.Active = active;
        }
    }
}
