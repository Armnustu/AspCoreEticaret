using Microsoft.CodeAnalysis.CSharp.Syntax;
using SahinTürkEticaretCore.ViewComponents;
using XAct;

namespace SahinTürkEticaretCore.Models
{
    public class Cls_Order
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public string MyCart { get; set; } //10=1&20=1&30=4&40=2
        public decimal UnitPrice { get; set; }
        public string ProductName { get; set; }
        public int Kdv { get; set; }
        public int MyProperty { get; set; }
        public string PhotoPath { get; set; }

        public static string tckimlik_vergi_no = "";
        public bool AddToMyCart(string id)//sepete ekleme işlemi yapılır
        {
            bool exists = false;
            if (MyCart == "") //bir seferliğine bu satır çalışacaktır
            {
                MyCart = id + "=1"; //30=1 
            }
            else
            {
                string[] MyCartArray = MyCart.Split('&');//7=1&36=1 aradaki & kaldırılır 7=1 36=1 haline dönüştürülür.Bütün veriler Mychart yer aılır
                for (int i = 0; i < MyCartArray.Length; i++)//aynı ürünün eklenip eklenmediği kontrol edilir
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('='); ///eşitlik ifadesi kaldırılır diziye dönüştürülür
                    if (MyCartArrayLoop[0] == id)
                    {
                        exists = true; //aynı ürün daha önce sepete eklenmiş
                    }
                }
                if (exists == false) //ürün daha önce sepete eklenmemişse 2. seferliğine eklenecektir.
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1";
                }
            }
            return exists;
        }
        public List<Cls_Order> SelectMyCart()//Bütün cookie değerleri MyCharta yer alır sonra listeye ekleyip gönderiyor
        {
            //10=1&
            //20=1&
            //30=4&
            //40=2
            List<Cls_Order> list = new List<Cls_Order>();
            string[] MyCartArray = MyCart.Split('&');

            if (MyCartArray[0] != "")
            {
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int MyCartID = Convert.ToInt32(MyCartArrayLoop[0]);

                    Products? prd = context.products.FirstOrDefault(p => p.ProductID == MyCartID);

                    //veritabanındaki verileri propertylere yazdırıyorum
                    Cls_Order ord = new Cls_Order();
                    ord.ProductID = prd.ProductID;
                    ord.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    ord.UnitPrice = prd.UnitPrice;
                    ord.ProductName = prd.ProductName;
                    ord.PhotoPath = prd.PhotoPath;
                    ord.Kdv = prd.Kdv;
                    list.Add(ord);
                }
            }
            return list;
        }

        DataContext context = new DataContext();
        public void DeleteFromMyCart(string id)
        {
            //10=1    --
            //20=1  
            //30=4
            //40=2
            //10=1&20=1&30=1&65=1
            string[] MyCartArray = MyCart.Split('&');
            string NewMyCart = "";
            int count = 1;

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                //ProductID ile adet ayrıldı
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                //for her döndüğünde dizinin sıfırıncı alanındaki degeri (10,20,30,40) MyCartID ye atadım
                string MyCartID = MyCartArrayLoop[0];
                if (MyCartID != id)
                {

                    //sepetten silinmeyeck olanlar buraya girecek
                    if (count == 1)
                    {
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }
                }
                //else
                //{
                //    //buraya girerse bu silinecek olan üründür.bunu NewMyCartArray icine eklemeyecegim
                //    //sepetten silinecek olan buraya girecek
                //}
            }
            MyCart = NewMyCart;
        }
        public void EfaturaCreate()
        {
        }
        public string OrderCreate(string email)
        {
            List<Cls_Order> siplist = SelectMyCart();
            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
            DateTime OrderDate = DateTime.Now;
            foreach (var item in siplist)
            {
                Order order = new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.users.FirstOrDefault(u => u.Email == email).UserID;
                order.ProductID = item.ProductID;
                order.Quantity = item.Quantity;
                context.orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }
        //View 
        public async Task<List<Vw_MyOrder>> SelectMyOrders(string email)
        {
            int UserID =  context.users.FirstOrDefault(u => u.Email == email).UserID;
            List<Vw_MyOrder> myorders = context.vm_MyOrders.Where(o => o.UserID == UserID).ToList();
            return myorders;
        }
        

    }

}
