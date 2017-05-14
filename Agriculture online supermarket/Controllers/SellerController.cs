using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Agriculture_online_supermarket.Controllers
{
    public class SellerController : Controller
    {
        // GET: Seller

        public ActionResult SellerIndex()
        {
            //检查登入状态
           // if (needRedirect())
          //  {
          //       return RedirectToAction; 
           // }
            string ShpID;
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1=sql.GetAllCmdInfo(ShpID);
            //取的卖家所有售卖商品信息填充模型
            return View();//View(models) 正在售卖商品列表
        }

        public ActionResult ProductInfo(string ProductID)
        {
            //检查登入状态
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetCmdInfo(ProductID);
            //获取指定商品的信息，填充模型
            return View();//View(model)商品信息(此model应该在提交SaveProductInfo()时作为参数）
        }
        public ActionResult SellerOrder()
        {
            //检查登入状态
            //获取所有订单并填充模型
            string ShpID;
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetAllIdtInfo(ShpID);
            return View();//View(models) 订单列表
        } 
        public ActionResult Delivery(string OrderId)
        {
            //检查登入状态
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetIdtInfo(OrderId);
            return View();//View(model) 订单发货和修改发货信息都用此Action，model中的物流信息等可能为空。
        }
        public ActionResult SellerOrderDetail(/*string OrderId*/)
        {
            //检查登入状态
            //获取订单信息并填充模型
            DataSet ds1;
            LinkToSQL sql = new LinkToSQL();
            ds1 = sql.GetIdtInfo(OrderId);
            return View();//View(model) 订单信息
        }
        public ActionResult AddProduct()
        {
            //检查登入状态
            //准备空模型
            return View("ProductInfo");//View(model) 待填充的空模型
        }
        public ActionResult DeleteProduct(string ProductId)
        {
            //检查登入状态
            //删除商品
            LinkToSQL sql=new LinkToSQL(); 
            sql.DeleteProduct(ProductId);
            return RedirectToAction("SellerIndex");
        }
        public ActionResult SaveProductInfo(/*Model model*/)
        {
            //将模型中数据保存
            return RedirectToAction("SellerIndex");
        }
        public ActionResult Deliver(string OrderId)
        {
            //检查登入状态
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "发货中");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult Refuse(string OrderId)
        {
            //检查登入状态
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "订单取消");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult AgreeRefund(string OrderId)
        {
            //检查登入状态
            //更改相应订单的状态
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "同意退款");
            return RedirectToAction("SellerOrder");
        }
        public ActionResult DisagreeRefund(string OrderId)
        {
            //检查登入状态
            //更改相应订单的状态 
            LinkToSQL sql = new LinkToSQL();
            sql.ChangeState(OrderId, "拒绝退款");
            return RedirectToAction("SellerOrder");
        }
    }
}