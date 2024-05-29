using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_subd
{
    public class Good
    {
        public int id;
        public string name;
        public int quantity;
        public int price;

        public Good(int id, string name, int price, int quantity)
        {
            this.id = id;
            this.name = name;
            this.quantity = quantity;
            this.price = price;
        }
    }
}
