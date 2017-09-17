using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ShoppingCartApi.Controllers
{

    public class ShoppingCartController : ApiController
    {
        public class MyCart
        {

            public List<Item> Items { get; set; }


        }
        public class Item
        {
            public int Count { get; set; }

            public Product Product { get; set; }

        }
        
        
        MyShoppingCartEntities db = new MyShoppingCartEntities();
      
        [Route("api/GetProducts")]
        [HttpGet]
        public List<Product> GetProducts()
        {
            db.Configuration.ProxyCreationEnabled = false;

            return db.Products.ToList();
        }
        [HttpGet]
        [Route("api/AddToCart/{Id}")]
        public bool AddToCart(int Id)
        {
            MyCart sc = null;

            if (HttpContext.Current.Session["sc"] == null)
                sc = new MyCart();
            else
                sc = HttpContext.Current.Session["sc"] as MyCart;

            if (sc.Items == null)
                sc.Items = new List<Item>();

            var db = new MyShoppingCartEntities();
            db.Configuration.ProxyCreationEnabled = false;

            var prod = db.Products.Find(Id);

            Item item = new Item();
            item.Count = 1;
            item.Product = prod;

            var product = sc.Items.Find(i => i.Product.Id == Id);
            if (product != null)
            {
                product.Count++;
            }
            else
            {
                sc.Items.Add(item);
            }

            HttpContext.Current.Session["sc"] = sc;
            return true;
        }

        [HttpGet]
        [Route("api/GetCart")]
       
        public MyCart GetCart()
        {
            
             return HttpContext.Current.Session["sc"] as MyCart;
            
        }
    }
}