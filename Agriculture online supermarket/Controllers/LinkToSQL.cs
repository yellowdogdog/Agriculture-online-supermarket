using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Agriculture_online_supermarket.Persistent;
using System.Collections;

namespace Agriculture_online_supermarket.Controllers
{
    public class LinkToSQL
    {
        public void DeleteProduct(String ProductID)
        {
            //给出产品ProductID，在数据库中删除该产品
            string sql = "delete from db_Commodity where  CmdID=" + ProductID;
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void ChangeState(String OrderID, String IDState)
        {
            //给出订单OrderID，将此订单的状态改为IDState
            string sql = " update db_Indent set IdtStatus=" + IDState + " from  db_Indent  where IdtID=" + OrderID;
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);

        }
        public DataSet GetAllCmdInfo(string ShpID)
        {
            //获得该卖家的所有商品信息
            string sql = "select CmdID productId,CmdName productName,CmdInventory Inventory from db_Commodity WHere ShpID like%'" + ShpID + "%'";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }

        public DataSet GetCmdInfo(string CmdID, string ShpID)
        {
            //根据商品ID返回该商品信息
            string sql = "select CmdID productId,CmdName productName,CmdInfo productInfo,CmdInventory Inventory,CmdUnit unit,CmdUP unitPrice from db_Commodity WHere ShpID like %'" + ShpID
                                    + "%' and CmdID like %" + CmdID + "%'"; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
        }


        public DataSet GetAllIdtInfo(string ShpID)
        {
            //获得该卖家的所有订单信息
            string sql = "select IdtID orderId,cmdName productName,IdtNum productNum,IdtStatus orderStatus,IdtDate orderDate "
                         + "from db_Indent db_Indent a,db_Commodity b, db_Shop c "
                         + " where a.ShpID=b.ShpID and  a.CmdID=b.CmdID and a.ShpID=c.ShpID and ShpID=" + ShpID; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }
        public DataSet GetIdtInfo(string IdtID)
        {
            //获得该订单ID的所有信息
            string sql = "select IdtID IdtID,CmdName CmdName,ShperName ShperName,Logistics LogisticsID,IdtNum IdtNum,IdtStatus IdtStatus,IdtDate IdtDate,IdtTP IdtTP "
                        + "from db_Indent db_Indent a,db_Commodity b, db_Shop c "
                        + " where a.ShpID=b.ShpID and  a.CmdID=b.CmdID and a.ShpID=c.ShpID and IdtID=" + IdtID; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }

        public DataSet GetDelivery(string OrderId)
        {
            string sql = "select IdtID orderId,CmdName productName,IdtNum productNum,Logistics LogisticsID from db_Indent a,db_Commodity b "
                       + "WHere a.ShpID=b.ShpID and  a.CmdID=b.CmdID  and IdtID=" + OrderId;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
            /// orderId//订单号ID
            ///  productName//商品名
            ///  productNum//商品数量
            ///  LogisticsID//物流单号
        }

        public void ChangeLogistics(String orderId, String LogisticsID)
        {
            //修改订单ID为orderId的物流号为LogisticsID
            Hashtable ht = new Hashtable();
            ht.Add("Logistics", "'" + LogisticsID + "'");
            String where = " Where IdtID= '" + orderId + "'";
            DataBase db = new DataBase();
            db.Updata("db_Indent", ht, where);

        }

        public void AddCmdInfo(String ShpID, String productName, String productInfo, String unit, String unitPrice)
        {
            //新增商品信息,生成一个商品编号
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
            String sql = "Select CmdID from db_Commodity where ShpID='" + ShpID + "'";
            DataBase db = new DataBase();
            int max = db.Count(sql);
            max++;
            Hashtable hs = new Hashtable();
            hs.Add("CmdID", max.ToString());
            hs.Add("ShpID", "'" + ShpID + "'");
            hs.Add("CmdName", "'" + productName + "'");
            hs.Add("CmdInfo", "'" + productInfo + "'");
            hs.Add("CmdUnit", "'" + unit + "'");
            hs.Add("CmdUP", unitPrice);
            db.Insert("db_Commodity", hs);

        }

        public void UpdateCmdInfo(String ShpID, String productId, String productName, String productInfo, String unit, String unitPrice)
        {
            //更新商品信息商品ID为productId的商品信息
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
            String where = " Where ShpID='" + ShpID + "'and CmdID='" + productId + "'";
            Hashtable ht = new Hashtable();
            ht.Add("CmdName", "'" + productName + "'");
            ht.Add("CmdInfo", "'" + productInfo + "'");
            ht.Add("CmdUnit", "'" + unit + "'");
            ht.Add("CmdUP", unitPrice);
            DataBase db = new DataBase();
            db.Updata("db_Commodity", ht, where);

        }

        public DataSet CustGetAllCmdInfo() //所有的商品以及其信息
        {

        }
        public DataSet CustGetCmdInfo(string id)//商品号为id的商品详细信息
        {

        }
        public DataSet GetAllCustOrdInfo(string CustID)//所有买家id为CustID的订单及信息
        {

        }
        public DataSet GetCustShopCInfo(string CustID)//所有买家id为CustID的购物车及信息
        {

        }
        public DataSet CustGetOrdDeInfo(string OrderId)  //订单号为 orderid的订单详情（订单状态、物流号）
        {

        }
        public DataSet GetSearchResult(string SearchContent)//商品名中包含有SearchContent的所有商品及信息
        {

        }
        public void AddShoppingCart(string username, string address, string productId, string productName, string productInfo, int productNum, string unit, double unitPrice, double TotalMoney)    //加入购物车表,,为什么没有表单号呢？自己生成？我数据多了点，你看看哪些可以不要，跟页面要协商。
        {

        }
        public DataSet  GetShoppingCart(string CustID)// 得到用户ID为CustID的购物车内所有商品的string productid，string productname，double unitprice，int productnum请按照我的顺序
        {

        }
        public void UpdateCustOrder1(string productid, string status)//不知道为什么收银台就是没有订单号，所以你这边就直接用 商品号  去查找订单，然后改成的状态为status
        {

        }
        public void UpdateCustOrder2(string OrderId, string status)//这边还有这个就有订单号，我们先分开，然后再合并吧。
        {

        }
        public void UpdateProductInfo(string productid, int productnum)//改变商品号为productid的库存，这边productnum是已经购买走的，所以直接库存 减 productnum
        {

        }
        public void DeleteShoppingCart(string OrderId)// 删除订单号为OrderId的购物车内的商品订单
        {

        }
        public void UpdateShoppingCart(string OrderId, int cnum) // cnum表示买家对购物车内某个订单（OrderId）的商品数量进行加或者减，我在外面调好了正负，你直接加就行。
        {

        }
        
               
        
    }
}