using Agriculture_online_supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Reflection;
using System.IO;

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
            IList<SellerIndexViewModel> SellerIndexList = DataSetToIList<SellerIndexViewModel>(new SellerIndexViewModel(),ds1);
            return View(SellerIndexList);//View(models) 正在售卖商品列表
        }

        public ActionResult ProductInfo(string id)
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
            ds1 = sql.GetCmdInfo(id, ShpID);
            ProductInfoViewModel productInfo = DataSetToModel<ProductInfoViewModel>(new ProductInfoViewModel(),ds1.Tables[0].Rows[0]);
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
            IList<SellerOrderViewModel> SellerOrderList = DataSetToIList<SellerOrderViewModel>(new SellerOrderViewModel(),ds1);
            return View(SellerOrderList);//View(models) 订单列表
        }
        public ActionResult Delivery(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetDelivery(id);
            DeliveryViewModel delivery = DataSetToModel<DeliveryViewModel>(new DeliveryViewModel(), ds1.Tables[0].Rows[0]);
            return View(delivery);//View(model) 订单发货和修改发货信息都用此Action，model中的物流信息等可能为空。
        }
        public ActionResult SellerOrderDetail(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetIdtInfo(id);
            SellerOrderDetailViewModel sellerOrder = DataSetToModel<SellerOrderDetailViewModel>(new SellerOrderDetailViewModel(),ds1.Tables[0].Rows[0]);
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
        public ActionResult DeleteProduct(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //删除商品
            LinkToSQL sql = new LinkToSQL();
            sql.DeleteProduct(id);
            return RedirectToAction("SellerIndex");
        }
        public ActionResult SaveProductInfo(ProductInfoViewModel product, HttpPostedFileBase file)
        {
            
            string imagePath="";
            
            //将模型中数据保存
            string ShpID = (string)Session["id"];
            if (file != null)
            {
                
                imagePath = "/Content/images/productimage/" + ShpID +DateTime.Now.ToString("yyyyMMddHmmss") + Path.GetExtension(file.FileName);
                file.SaveAs(System.Web.HttpContext.Current.Server.MapPath("~"+imagePath));
            }
            LinkToSQL sql = new LinkToSQL();
            if (product.productId == null) sql.AddCmdInfo(ShpID, product.productName, product.productInfo, product.unit, product.unitPrice.ToString(), imagePath);
            else
            {                              
                sql.UpdateCmdInfo(ShpID, product.productId, product.productName, product.productInfo, product.unit, product.unitPrice.ToString(), imagePath);

            }
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
        public ActionResult Refuse(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(id, "订单取消");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult AgreeRefund(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(id, "同意退款");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult DisagreeRefund(string id)
        {
            //检查登入状态
            if (needRedirect())
            {
                return redirectAction;
            }
            //更改相应订单的状态 
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(id, "拒绝退款");
            return RedirectToAction("SellerOrder");
        }

        private bool needRedirect()
        {
            if (Session["state"] == null)
                return true;
            return !((int)Session["state"] == 2);
        }
        public static List<T> DataSetToIList<T>(T entity, DataSet ds) where T : new()
        {
            List<T> lists = new List<T>();
            if (ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow row in ds.Tables[0].Rows)
                { lists.Add(DataSetToModel(new T(), row)); }
            }
            return lists;
        }
        public static T DataSetToModel<T>(T entity, DataRow row) where T : new()
        {   //初始化 如果为null  
            if (entity == null){     entity = new T();   }   //得到类型   
            Type type = typeof(T);   //取得属性集合  
            PropertyInfo[] pi = type.GetProperties();
            foreach (PropertyInfo item in pi){
                //给属性赋值    
                if (row[item.Name] != null && row[item.Name] != DBNull.Value) {
                    if (item.PropertyType == typeof(System.Nullable<System.DateTime>)) {
                        item.SetValue(entity, Convert.ToDateTime(row[item.Name].ToString()), null);
                    } else {
                        item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                    }
                }
            }
        return entity; } 

      /*  public T DataSetToModel<T>(DataSet ds, int tableIndex)
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
        }*/

            /// <summary>
            /// 获得跳转返回
            /// </summary>
        private ActionResult redirectAction
        {
            get
            {
                if (Session["state"] == null)
                {
                    return RedirectToAction("Index", "Customer");
                }
                if ((int)Session["state"] == 1 || (int)Session["state"] == 0)
                {
                    return RedirectToAction("Index", "Customer");
                }
                else if ((int)Session["state"] == 3)
                {
                    return RedirectToAction("AdminIndex", "Account");
                }

                else
                {
                    return null;
                }
            }
        }

    }
}