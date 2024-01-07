
using TrainnigApI.Model;

namespace TrainnigApI.Model
{
    public class Account: BaseNormalEntity
    {
       
        public string ?AccountName { get; set; }
        public double TotalBalance { get; set; }
      
    }

}
