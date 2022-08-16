using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedApiOdersdetails.Models
{
    [Table("tblOrder")]
    public class Order
    {
        [Key]
        public int Oid { get; set; }
        public int invoiceNo { get; set; }
        public string custName { get; set; }
        public string billingAddress { get; set; }
        public string shippingAddress { get; set; }
        public List<ordetDetails> ordetDetailsList { get; set; }
        public double totalorderAmmount { get; set; }
    }
}
