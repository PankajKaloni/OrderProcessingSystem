using OrderProcessingSystem.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace OrderProcessingSystem.Controllers
{
    public class HomeController : Controller
    {
        private OrderProcessingSystemEntities orderProcessingSystemEntities = new OrderProcessingSystemEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Customer user)
        {
            var cust = orderProcessingSystemEntities.Customers.FirstOrDefault(x => x.Customer_Name == user.Customer_Name && x.Password == user.Password);

            if (cust != null)
            {
                TempData["CustomerId"] = cust.Customer_Id; // Store customer ID in TempData
                return RedirectToAction("GetProductsList");
            }
            else
            {
                // If username and password don't match, add an error message to ModelState
                ModelState.AddModelError(string.Empty, "Username and password do not match.");
                return View("Index"); // Return to login page
            }
        }

        // View List of Products
        public ActionResult GetProductsList()
        {
            // Check if TempData["CustomerId"] is not null before accessing it
            int? customerId = TempData["CustomerId"] as int?;
            if (!customerId.HasValue)
            {
                //  redirect to the login page
                return RedirectToAction("Index");
            }

            // Retrieve products from the database
            var products = orderProcessingSystemEntities.Database.SqlQuery<Product>("GetProductList").ToList();

            // Map products to the view model
            var model = products.Select(p => new Product
            {
                Product_ID = p.Product_ID,
                Product_Name = p.Product_Name,
                Product_Price = p.Product_Price
            }).ToList();

            //] = customerId.Value; Store the customer ID in TempData for use in other actions
            ViewBag.CustomerId = customerId;
            return View(model);
        }

        // Select Products to Order
        [HttpPost]
        public ActionResult SelectionOfProducts(List<Product> products)
        {
            // Check if TempData["cust_id"] is not null before accessing it
            int? customerId = TempData["CustomerId"] as int?;
            if (!customerId.HasValue)
            {
                // redirect to the login page
                return RedirectToAction("Index");
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Customer_Id", typeof(int));
            dataTable.Columns.Add("Product_Id", typeof(int));
            dataTable.Columns.Add("Quantity", typeof(int));

            // Populate the DataTable with the values from the products list
            foreach (var item in products)
            {
                if (item.Quantity > 0)
                {
                    dataTable.Rows.Add(customerId.Value, item.Product_ID, item.Quantity);
                }
            }

            var customerOrders = new SqlParameter("@Orders", SqlDbType.Structured);
            customerOrders.Value = dataTable;
            customerOrders.TypeName = "dbo.CustomerOrders";

            string sql = "EXEC InsertOrderDetails @Orders";

            orderProcessingSystemEntities.Database.ExecuteSqlCommand(sql, customerOrders);
            TempData["data"] = customerId.Value;
            return RedirectToAction("GetOrderDetails");
        }

        // Get Order Details for a particular customer
        public ActionResult GetOrderDetails()
        {
            // Check if TempData["data"] is not null before accessing it
            int? customerId = TempData["data"] as int?;
            if (!customerId.HasValue)
            {
                // redirect to the login page
                return RedirectToAction("Index");
            }

            SqlParameter parms = new SqlParameter("@Customer_Id", customerId.Value);
            var data = orderProcessingSystemEntities.Database.SqlQuery<GetOrderDetails_Result>("GetOrderDetails @Customer_Id", parms).ToList();
            return View(data);
        }
    }
}
