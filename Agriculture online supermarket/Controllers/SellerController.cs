using Agriculture_online_supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Reflection;

namespace Agriculture_online_supermarket.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller
        public ActionResult SellerIndex()
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //取的卖家所有售卖商品信息填充模型
            string ShpID = (string)Session["id"];
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetAllCmdInfo(ShpID);
            IList<SellerIndexViewModel> SellerIndexList = DataSetToIList<SellerIndexViewModel>(ds1, 0);
            return View(SellerIndexList);//View(models) 正在售卖商品列表
        }

        public ActionResult ProductInfo(string ProductID)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取指定商品的信息，填充模型
            DataSet ds1;
            string ShpID = (string)Session["id"];
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetCmdInfo(ProductID, ShpID);
            ProductInfoViewModel productInfo = DataSetToModel<ProductInfoViewModel>(ds1, 0);
            return View(productInfo);//View(model)商品信息(此model应该在提交SaveProductInfo()时作为参数）
        }
        public ActionResult SellerOrder()
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取所有订单并填充模型
            string ShpID = (string)Session["id"];
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetAllIdtInfo(ShpID);
            IList<SellerOrderViewModel> SellerOrderList = DataSetToIList<SellerOrderViewModel>(ds1, 0);
            return View(SellerOrderList);//View(models) 订单列表
        }
        public ActionResult Delivery(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetDelivery(OrderId);
            DeliveryViewModel delivery = DataSetToModel<DeliveryViewModel>(ds1, 0);
            return View(delivery);//View(model) 订单发货和修改发货信息都用此Action，model中的物流信息等可能为空。
        }
        public ActionResult SellerOrderDetail(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetIdtInfo(OrderId);
            SellerOrderDetailViewModel sellerOrder = DataSetToModel<SellerOrderDetailViewModel>(ds1, 0);
            return View(sellerOrder);//View(model) 订单信息
        }
        public ActionResult AddProduct()
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //准备空模型
            return View("ProductInfo");//View(model) 待填充的空模型
        }
        public ActionResult DeleteProduct(string ProductId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //删除商品
            LinkToSQL sql = new LinkToSQL();
            sql.DeleteProduct(ProductId);
            return RedirectToAction("SellerIndex");
        }
        public ActionResult SaveProductInfo(ProductInfoViewModel product)
        {
            //将模型中数据保存
            string ShpID = (string)Session["id"];
            LinkToSQL sql = new LinkToSQL();
            if (product.productId == "") sql.AddCmdInfo(ShpID, product.productName, product.productInfo, product.unit, product.unitPrice.ToString());
            else sql.UpdateCmdInfo(ShpID, product.productId, product.productName, product.productInfo, product.unit, product.unitPrice.ToString());
            return RedirectToAction("SellerIndex");
        }
        public ActionResult Deliver(DeliveryViewModel delivery)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeLogistics(delivery.orderId, delivery.LogisticsID);
            sql.ChangeState(delivery.orderId, "发货中");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult Refuse(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "订单取消");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult AgreeRefund(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "同意退款");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult DisagreeRefund(string OrderId)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态 
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "拒绝退款");
            return RedirectToAction("SellerOrder");
        }

        private bool needRedirect()
        {
            if (Session["state"] == null)
                return true;
            return !((int)Session["state"] == 2);
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
        /// <summary>
        /// 获得跳转返回
        /// </summary>
        private ActionResult redirectAction
        {
            get
            {
                if ((int)Session["state"] == 1 || (int)Session["state"] == 0)
                {
                    return RedirectToAction("Index", "Customer");
                }
                else if ((int)Session["state"] == 3)
                {
                    return RedirectToAction("AdminIndex", "Admin");
                }

                else
                {
                    return null;
                }
            }
        }

    }
}