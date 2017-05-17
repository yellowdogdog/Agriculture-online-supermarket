using Agriculture_online_supermarket.Models;
using Agriculture_online_supermarket.Persistent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.CustGetAllCmdInfo();


            IList<IndexModel> CustomerIndexList = DataSetToIList<IndexModel>(ds1, 0);
            return View(CustomerIndexList);//View(models) 要显示的商品列表
        }
        public ActionResult Detail(string id)
        {
            //填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.CustGetCmdInfo(id);
            DetailViewModel productInfo = DataSetToModel<DetailViewModel>(ds1, 0);
            return View(productInfo);//View(model) 商品信息（例如名字、库存）
        }
        public ActionResult Orders()
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //填充模型
            string CustID = (string)Session["id"];
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetAllCustOrdInfo(CustID);
            IList<CustomerOrderViewModel> CustomerOrderList = DataSetToIList<CustomerOrderViewModel>(ds1, 0);
            return View(CustomerOrderList);//View(models) 订单信息
        }
        public ActionResult ShoppingCart()
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //填充模型
            string CustID = (string)Session["id"];
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetCustShopCInfo(CustID);
            IList<CustomerShoppingcartModle> CustomerShopCList = DataSetToIList<CustomerShoppingcartModle>(ds1, 0);
            return View(CustomerShopCList);//View(models) 购物车里的商品信息
        }
        //[HttpPost] 
        //public ActionResult Cashier(/*string ProductId 假设，只买了1个*/)
        //{
        //    //检查登入状态
        //    //填充模型
        //    return View();//View(model) 结算时显示清单
        //}
        public ActionResult OrderDetail(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.CustGetOrdDeInfo(OrderId);
            CustomerOrderDetailModel CustomerOrder = DataSetToModel<CustomerOrderDetailModel>(ds1, 0);
            return View(CustomerOrder);//View(model) 订单状态、物流号等
        }

        public ActionResult Search(string SearchContent)
        {
            List<IndexModel> models = new List<IndexModel>();
            
            //搜索并填充模型
            
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetSearchResult(SearchContent);
           
            DataTable dt = ds1.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IndexModel model = new IndexModel(dt.Rows[i]["CmdName"].ToString(), Convert.ToDouble(dt.Rows[i]["CmdUP"]), dt.Rows[i]["PhotoUrl"].ToString(),dt.Rows[i]["ShpID"].ToString(), (int)dt.Rows[i]["CmdID"]);
                models.Add(model);
            }
            return View("Index", models); // 搜索结果
            //return View("Index", DataBase.getIndexModelByKeyWord(SearchContent));
        }


        public ActionResult AddToShoppingCart(CustomerShoppingcartModle ShopC)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //添加购物车操作
            LinkToSQL sql = new LinkToSQL();
            sql.AddShoppingCart(ShopC.userId,ShopC.sellerId, ShopC.productId, ShopC.productNum,"购物车", DateTime.Now.ToLocalTime(),ShopC.TotalMoney);
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult Purchase(/*string ProductId 假设，只买了1个*/)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
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
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取用户购物车，填充收银台模型
            string CustID = (string)Session["id"];
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetShoppingCart(CustID);
            
           
            List<CashierViewModel> models = new List<CashierViewModel>();
            DataTable dt = ds1.Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CashierViewModel model = new CashierViewModel(dt.Rows[i][0].ToString(),dt.Rows[i][1].ToString(), dt.Rows[i][2].ToString(), Convert.ToDouble(dt.Rows[i][3]), Convert.ToInt32(dt.Rows[i][4]));
                models.Add(model);
            }
            return View("Cashier",models);//View("Cashier" model) 结算时显示清单
        }
        public ActionResult SubmitOrder(List<CashierViewModel> models)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更新订单状态，更新库存，更新双方余额（假设直接到账）
            LinkToSQL sql = new LinkToSQL();
            int Cot = models.Count();
            foreach (CashierViewModel element in models) 
            {
                sql.UpdateCustOrder(element.OrderId,"待发货");
                sql.UpdateProductInfo(element.OrderId, element.productnum);
                sql.UpdateBalance(element.OrderId);
            }

            
            return RedirectToAction("Orders");
        }
        public ActionResult DeleteProduct(string OrderId)//假设数据库利用订单表存储购物车
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //删除购物中的某个物品
            LinkToSQL sql = new LinkToSQL();
            sql.DeleteShoppingCart(OrderId);
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult IncreaseNum(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //尝试增加购物车订单商品的数量（检查库存）
            LinkToSQL sql = new LinkToSQL();
            DataSet ds1;
            ds1 = sql.CustNumCompare(OrderId);
            DataTable dt = ds1.Tables[0];
            if (Convert.ToInt32(dt.Rows[0][0])+1 <= Convert.ToInt32(dt.Rows[0][1])) //这边要改！！
                sql.UpdateShoppingCart(OrderId,1);
            else Response.Write(@"<script>alert('库存不足！无法增加数量！');</script>");
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult DecreaseNum(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //尝试减少购物车订单商品的数量（检查库存）
            LinkToSQL sql = new LinkToSQL();
            DataSet ds1;
            ds1 = sql.CustGetCmdInfo(id);
            DataTable dt = ds1.Tables[0];
            if (Convert.ToInt32(dt.Rows[0][0]) - 1 <= Convert.ToInt32(dt.Rows[0][1])) //这边要改！！
                sql.UpdateShoppingCart(id, -1);
            else Response.Write(@"<script>alert('库存已不足！请重新选择数量！');</script>");
            return RedirectToAction("ShoppingCart");
        }
        public ActionResult ConfirmReceipt(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //确认收货动作：更新订单
            LinkToSQL sql = new LinkToSQL();
            sql.UpdateCustOrder(id, "已收货");
            return RedirectToAction("Orders");
        }
        public ActionResult ApplyRefund(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //申请退款动作：更新订单
            LinkToSQL sql = new LinkToSQL();
            sql.UpdateCustOrder(id, "申请退款中");
            return RedirectToAction("Orders");
        }
        

        public T DataSetToModel<T>(DataSet ds, int tableIndex)
        {
            DataTable dt = ds.Tables[tableIndex];
            T _t = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] propertys = _t.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (pi.Name.Equals(dt.Columns[i].ColumnName))
                    {
                        if (dt.Rows[0][i] != DBNull.Value)
                            pi.SetValue(_t, dt.Rows[0][i], null);
                        else
                            pi.SetValue(_t, null, null);

                    }
                }
            }
            return _t;
        }
        public IList<T> DataSetToIList<T>(DataSet ds, int tableIndex)
        {
            if (ds == null || ds.Tables.Count < 0)
                return null;
            if (tableIndex > ds.Tables.Count - 1)
                return null;
            if (tableIndex < 0)
                tableIndex = 0;
            DataTable dt = ds.Tables[tableIndex];

            IList<T> result = new List<T>();
            for (int j = 0; j < dt.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (pi.Name.Equals(dt.Columns[i].ColumnName))
                        {
                            if (dt.Rows[j][i] != DBNull.Value)
                                pi.SetValue(_t, dt.Rows[j][i], null);
                            else
                                pi.SetValue(_t, null, null);
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
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