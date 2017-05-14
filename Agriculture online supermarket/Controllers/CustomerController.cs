using Agriculture_online_supermarket.Models;
using Agriculture_online_supermarket.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agriculture_online_supermarket.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            //检查用户状态
            if (needRedirect())
            {
                return redirectAction;
            }            
            // 用户
            //填充模型
            return View();//View(models) 要显示的商品列表
        }
        public ActionResult Detail(string id)
        {
            //填充模型
            return View();//View(model) 商品信息（例如名字、库存）
        }
        public ActionResult Orders()
        {
            //检查登入状态
            //填充模型
            return View();//View(models) 订单信息
        }
        public ActionResult ShoppingCart()
        {
            //检查登入状态
            //填充模型
            return View();//View(models) 购物车里的商品信息
        }
        //[HttpPost] 
        //public ActionResult Cashier(/*string ProductId 假设，只买了1个*/)
        //{
        //    //检查登入状态
        //    //填充模型
        //    return View();//View(model) 结算时显示清单
        //}
        public ActionResult OrderDetail(/*string OrderId*/)
        {
            //检查登入状态
            //填充模型
            return View(); //View(model) 订单状态、物流号等
        }

        public ActionResult Search(string SearchContent)
        {
            List<IndexModel> models = new List<IndexModel>();
            for (int i = 0; i < 5; i++)
            {
                IndexModel indexmodel = new IndexModel();
                indexmodel.imagePath = "/Content/images/productimage/vegetable1.png";
                indexmodel.price = 100;
                indexmodel.productName = "青菜";
                indexmodel.sellerName = "农民" + i;
                indexmodel.productID = "234";

                models.Add(indexmodel);
            }


            //搜索并填充模型
            return View("Index", models); // 搜索结果
            //return View("Index", DataBase.getIndexModelByKeyWord(SearchContent));
        }


        public ActionResult AddToShoppingCart(/*String productId*/)
        {
            //检查登入状态
            //添加购物车操作
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult Purchase(/*string ProductId 假设，只买了1个*/)
        {
            //检查登入状态
            // 填充模型
            //debug use
            List<CashierViewModel> models = new List<CashierViewModel>();
            CashierViewModel model = new CashierViewModel("1", "茄子", 5.2, 2);
            models.Add(model);
            model = new CashierViewModel("2", "草莓", 2.58, 10);
            models.Add(model);
            return View("Cashier",models);//View("Cashier" model) 结算时显示清单
        }
        public ActionResult PurchaseAll()
        {
            //检查登入状态
            //获取用户购物车，填充收银台模型
            return View("Cashier");//View("Cashier" model) 结算时显示清单
        }
        public ActionResult SubmitOrder(/*收银台列表模型*/)
        {
            //检查登入状态
            //更新订单状态，更新库存，更新双方余额（假设直接到账）
            return RedirectToAction("Orders");
        }
        public ActionResult DeleteProduct(/*string OrderId*/)//假设数据库利用订单表存储购物车
        {
            //检查登入状态
            //删除购物中的某个物品
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult IncreaseNum(/*string OrderId*/)
        {
            //检查登入状态
            //尝试增加购物车订单商品的数量（检查库存）
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult DecreaseNum(/*string OrderId*/)
        {
            //检查登入状态
            //尝试增加购物车订单商品的数量（检查库存）
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult ConfirmReceipt(/*string OrderId*/)
        {
            //检查登入状态
            //确认收获动作：更新订单
            return RedirectToAction("Orders");
        }
        public ActionResult ApplyRefund(/*string OrderId*/)
        {
            //检查登入状态
            //申请退款动作：更新订单
            return RedirectToAction("Orders");
        }
        private bool needRedirect()
        {
            if (Session["state"] == null)
                return false;
            return !((int)Session["state"] == 0 || (int)Session["state"] == 1);
        }
        /// <summary>
        /// 获得跳转返回
        /// </summary>
        private ActionResult redirectAction
        {
            get
            {
                if ((int)Session["state"] == 2)
                {
                    return RedirectToAction("SellerIndex","Seller");
                }else if ((int)Session["state"] == 3)
                {
                    return RedirectToAction("AdminIndex", "Admin");
                }else
                {
                    return null;
                }
            }
        }
    }
}