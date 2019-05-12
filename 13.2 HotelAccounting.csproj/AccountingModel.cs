using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelAccounting
{
    class AccountingModel : ModelBase
    {
        private double price;
        private int nightsCount;
        private double discount;
        private double total;

        public double Price
        {
            get { return price; }
            set
            {
                if (value >= 0)
                    price = value;
                else
                    throw new ArgumentException();
                NewTotal();
                Notify(nameof(Price));
            }
        }

        public int NightsCount
        {
            get { return nightsCount; }
            set
            {
                if (value > 0)
                    nightsCount = value;
                else
                    throw new ArgumentException();
                NewTotal();
                Notify(nameof(NightsCount));
            }
        }

        public double Discount
        {
            get { return discount; }
            set
            {
                discount = value;
                NewTotal();
                Notify(nameof(Discount));
            }
        }

        public double Total
        {
            get { return total; }
            set
            {
                if (value > 0)
                    total = value;
                else throw new ArgumentException();
                Notify(nameof(Total));
                discount = (100 * (1 - total / (nightsCount * price)));
                Notify(nameof(Discount));
            }
        }

        private void NewTotal()
        {
            total = price * nightsCount * (1 - discount / 100);
            if (total < 0)
                throw new ArgumentException();
            Notify(nameof(Total));
        }
    }
}