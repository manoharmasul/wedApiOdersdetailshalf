using Dapper;
using wedApiOdersdetails.Context;
using wedApiOdersdetails.Models;
using wedApiOdersdetails.Repository.Interface;

namespace wedApiOdersdetails.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _context;
        public OrderRepository(DapperContext context)
        {
            _context = context;
        }

       

        public async Task<int> AddOrder(Order order)
        {
            double result = 0;
            var qry = @"insert into tblOrder (invoiceNo,custName,billingAddress,shippingAddress,totalorderAmmount) 
               values (@invoiceNo,@custName,@billingAddress,@shippingAddress,@totalorderAmmount);SELECT CAST(SCOPE_IDENTITY() as int)";
            List<ordetDetails> olist = new List<ordetDetails>();
            olist = order.ordetDetailsList.ToList();
           using(var connetion=_context.CreateConnection())
           {
                int res = await connetion.QuerySingleAsync<int>(qry,order);
               if(res!=0)
               {
                    result = await IsertUpdateOrder(olist, res);
                    order.totalorderAmmount= result;
                    var qry1 = @"insert into tblOrder (totalorderAmmount) values(@totalorderAmmount)";
                    var result1 = await connetion.ExecuteAsync(qry1, order);

                }
           }
            return Convert.ToInt32(result);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            List<Order> olist = new List<Order>();
            var qry = @"select * from tblOrder";
            using(var connection=_context.CreateConnection())
            {
                var order = await connection.QueryAsync<Order>(qry);

                olist = order.ToList();
                foreach (var od in olist)
                {
                 
                 var result = await connection.QueryAsync<ordetDetails>
                        (@"select * from tblorderDetails where Oid=@oid", new { oid = od.Oid });
                    od.ordetDetailsList = result.ToList();

                }
                return olist;
                
            }
          
        }

        public async Task<Order> GetOrderById(int id)
        {
            Order order = new Order();
            var qry = "select * from tblOrder where Oid=@oid";
            using(var con = _context.CreateConnection())
            {
                var rawOrder=await con.QueryAsync<Order>(qry, new { oid = id });
                order = rawOrder.FirstOrDefault();
                if(order!=null)
                {
                    var orderdetailsrow = await con.QueryAsync<ordetDetails>
                        (@"select * from tblorderDetails where Oid=@oid", new { oid = id });
                    order.ordetDetailsList = orderdetailsrow.ToList();
                   
                }
                return order;
            }
        }
        public async Task<double> IsertUpdateOrder(List<ordetDetails> olist,int rest)
        {
            int ret = 0;
            double grandtotal = 0;
            if(rest!=0)
            {
                using(var connection=_context.CreateConnection())
                {
                    foreach(ordetDetails od in olist)
                    {
                        od.Oid = rest;
                        var pqry = "select Price from Product where pId=@pid";
                        var resultprice=await connection.QuerySingleAsync<int>(pqry, new { pid = od.pId });
                        od.totalAmmount = resultprice * od.Qty;
                        var qry = @"insert into tblorderDetails (Oid,pId,Qty,totalAmmount) values(@Oid,@pId,@Qty,@totalAmmount)";
                        var result1 = await connection.ExecuteAsync(qry,od);

                        ret = ret + result1;
                        grandtotal = grandtotal + od.totalAmmount;
                    }
                }
            }
            return grandtotal;
        }
      public async Task<int> UpdateOrder(Order order)
        {
            double ret;
            var qry= @"update tblOrder set invoiceNo=@invoiceNo,custName=@custName,billingAddress=@billingAddress,
              shippingAddress=@shippingAddress,totalAmmount=@totalAmmount where Odi=@oid";
            using (var connection = _context.CreateConnection())
           {
                var result = await connection.ExecuteAsync(qry, order);
                ret = await IsertUpdateOrder(order.ordetDetailsList, order.Oid);
                order.totalorderAmmount = ret;
                return result;
            }
        }
    }
}
