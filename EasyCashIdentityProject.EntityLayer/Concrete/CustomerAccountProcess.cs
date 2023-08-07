using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCashIdentityProject.EntityLayer.Concrete
{
    public class CustomerAccountProcess
    {
        public int CustomerAccountProcessID { get; set; }
        public  string ProcessType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessDate { get; set; }
        public int? SenderID { get; set; }
        public int? ReceiverID { get; set; }
        public CustomerAccount SenderCustomer { get; set; } //gönderen/borçlandıran müşteri
        public CustomerAccount ReceiverCustomer { get; set; } //alan/alacaklandıran müşteri
        public string Description { get; set; }
    }

    /* id - işlem türü(gelen giden ödeme) - miktar - tarih - alıcı - gönderici */
}
