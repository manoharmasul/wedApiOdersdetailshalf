using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedApiOdersdetails.Models
{
    [Table("tblorderDetails")]
    public class ordetDetails
    {
        [Key]
        public int orderDetailId { get; set; }
        public int Oid { get; set; }
        public int pId { get; set; }
        public int Qty { get; set; }
        public double totalAmmount { get; set; }
    }
}
