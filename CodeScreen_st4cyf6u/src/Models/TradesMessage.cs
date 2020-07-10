using System;
using System.Collections.Generic;
using System.Text;

namespace CodeScreen.Assessments.TradeCancelling.src.Models
{
    class TradesMessage
    {

        public string CompanyName { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderDate { get; set; }
        public string OrderType { get; set; }
    }



    class CompanyWiseDate
    {

        public string CompanyName { get; set; }
        public DateTime Startdate { get; set; }
        public DateTime EndDate { get; set; }

        public long order { get; set; }
        public long cancel { get; set; }
    }
}



