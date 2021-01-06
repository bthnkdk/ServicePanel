using System;
using System.Collections.Generic;

namespace Domain
{
    public class Currency : Entity
    {
        public Currency()
        {
            CurrencyRates = new HashSet<CurrencyRate>();
        }
        public override string AuthorityCode => "DEF";
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<CurrencyRate> CurrencyRates { get; set; }

    }
    public class CurrencyRate : Entity
    {
        public int CurrencyId { get; set; }
        public float Buy { get; set; }
        public float Sell { get; set; }
        public DateTime Date { get; set; }
        public DateTime ActualRateDate { get; set; }

        public virtual Currency Currency { get; set; }
    }

    public class CurrencyModel
    {
        public double Buy { get; set; }
        public double Sell { get; set; }
    }
}
