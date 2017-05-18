using System;
using System.Data;
using Agriculture_online_supermarket.Persistent;
using System.Collections;
using System.Web;

namespace Agriculture_online_supermarket.Controllers
{
    public class LinkToSQL
    {
        public void DeleteProduct(String ProductID)
        {
            //给出产品ProductID，在数据库中删除该产品
            string sql = "delete from db_Commodity where  CmdID='" + ProductID + "'";
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void ChangeState(String OrderID, String IDState)
        {
            //给出订单OrderID，将此订单的状态改为IDState
            string sql = " update db_Indent set IdtStatus=" + IDState + " from  db_Indent  where IdtID='" + OrderID + "'";
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);

        }
        public DataSet GetAllCmdInfo(string ShpID)
        {
            //获得该卖家的所有商品信息
            string sql = "select CmdID productId,CmdName productName,CmdInventory Inventory from db_Commodity WHere ShpID= '" + ShpID + "'";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }

        public DataSet GetCmdInfo(string CmdID, string ShpID)
        {
            //根据商品ID返回该商品信息
            string sql = "select CmdID productId,CmdName productName,CmdInfo productInfo,CmdInventory Inventory,CmdUnit unit,CmdUP unitPrice,PhotoUrl imagePath from db_Commodity WHere ShpID='" + ShpID
                                    + "' and CmdID='" + CmdID + "'"; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
        }


        public DataSet GetAllIdtInfo(string ShpID)
        {
            //获得该卖家的所有订单信息
            string sql = "select IdtID orderId,cmdName productName,IdtNum productNum,IdtStatus orderStatus,IdtDate orderDate "
                         + "from db_Indent a,db_Commodity b, db_Shop c "
                         + " where a.ShpID=b.ShpID and  a.CmdID=b.CmdID and a.ShpID=c.ShpID and a.ShpID='" + ShpID + "'";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }
        public DataSet GetIdtInfo(string IdtID)
        {
            //获得该订单ID的所有信息
            string sql = "select IdtID IdtID,CmdName CmdName,ShperName ShperName,Logistics LogisticsID,IdtNum IdtNum,IdtStatus IdtStatus,IdtDate IdtDate,IdtTP IdtTP "
                        + "from db_Indent a,db_Commodity b, db_Shop c "
                        + " where a.ShpID=b.ShpID and  a.CmdID=b.CmdID and a.ShpID=c.ShpID and IdtID='" + IdtID + "'";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;

        }

        public DataSet GetDelivery(string OrderId)
        {
            string sql = "select IdtID orderId,CmdName productName,IdtNum productNum,Logistics LogisticsID from db_Indent a,db_Commodity b "
                       + "WHere a.ShpID=b.ShpID and  a.CmdID=b.CmdID  and IdtID='" + OrderId + "'"; 
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

        public void AddCmdInfo(String ShpID, String productName, String productInfo, String Inventory, String unit, String unitPrice, string ImagePath)
        {
            //新增商品信息,生成一个商品编号
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
            String sql = "Select max(CmdID) from db_Commodity";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            int max = Convert.ToInt32( ds.Tables[0].Rows[0][0].ToString())+1;
            Hashtable hs = new Hashtable();
            hs.Add("CmdID", max.ToString());
            hs.Add("ShpID", "'" + ShpID + "'");
            hs.Add("CmdName", "'" + productName + "'");
            hs.Add("CmdInfo", "'" + productInfo + "'");
            hs.Add("CmdInventory", "'" + Inventory + "'");
            hs.Add("CmdUnit", "'" + unit + "'");
            hs.Add("CmdUP", unitPrice);
            hs.Add("PhotoUrl", "'" + ImagePath + "'");
            db.Insert("db_Commodity", hs);

        }

        public void UpdateCmdInfo(String ShpID, String productId, String productName, String productInfo, String Inventory, String unit, String unitPrice, string ImagePath)
        {
            //更新商品信息商品ID为productId的商品信息
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
            String where = " Where ShpID='" + ShpID + "'and CmdID='" + productId + "'";
            Hashtable ht = new Hashtable();
            ht.Add("CmdName", "'" + productName + "'");
            ht.Add("CmdInfo", "'" + productInfo + "'");
            ht.Add("CmdInventory", "'" + Inventory + "'");
            ht.Add("CmdUnit", "'" + unit + "'");
            ht.Add("CmdUP", unitPrice);
            ht.Add("PhotoUrl", "'" + ImagePath + "'");
            DataBase db = new DataBase();
            db.Updata("db_Commodity", ht, where);

        }
    

        /// <summary>
        /// 以下为买家部分
        /// </summary>
        /// <returns></returns>
        public DataSet CustGetAllCmdInfo() //所有的商品以及其信息
        {
            String sql = "Select CmdName productName,CmdUP price,a.PhotoUrl imagePath,b.ShpName sellerName,a.CmdID productID from db_Commodity a,db_Shop b where a.ShpID=b.ShpID";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public DataSet CustGetCmdInfo(string id)//商品号为id的商品详细信息
        {
            String sql = "Select CmdID productId,CmdName productName,CmdInfo productInfo,CmdInventory Inventory,CmdUnit unit,CmdUP unitPrice,PhotoUrl imagePath from db_Commodity where CmdID='" + id + "'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public DataSet GetAllCustOrdInfo(string CustID)//所有买家id为CustID的订单及信息
        {
            String sql = "Select * from  db_Indent where ShperID='" + CustID + "'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public DataSet GetCustShopCInfo(string CustID)//所有买家id为CustID的购物车及信息
        {
            String sql = "Select * from  db_Indent where IdtStatus='购物车' and ShperID='" + CustID + "'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public DataSet CustGetOrdDeInfo(string OrderId)  //订单号为 orderid的订单详情（订单状态、物流号）
        {
            String sql = "Select * from  db_Indent where IdtID='" + OrderId + "'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public DataSet GetSearchResult(string SearchContent)//商品名中包含有SearchContent的所有商品及信息
        {
            String sql = "Select * from  db_Commodity where CmdName LIKE'%" + SearchContent + "%'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public void AddShoppingCart(string userId, string sellerId, string productId, int productNum, string status, DateTime IdtDate, double TotalMoney)
        //加入购物车表,即订单的第一次产生过程，请数据库为它自动生成订单号
        {
            Hashtable ht = new Hashtable();
            string sql = "select IdtID from db_Indent order by IdtID desc";
            DataBase db = new DataBase();
            int max = db.Count(sql);
            max++;
            ht.Add("IdtID", max.ToString());
            ht.Add("ShpID", "'" + sellerId + "'");
            ht.Add("ShperID", "'" + userId + "'");
            ht.Add("CmdID", "'" + productId + "'");
            ht.Add("IdtNum", productNum.ToString());
            ht.Add("IdtStatus", "'" + status + "'");
            ht.Add("IdtDate", IdtDate.ToString());
            ht.Add("IdtTP", TotalMoney.ToString());
            db.Insert("db_Indent", ht);
        }
        public DataSet GetShoppingCart(string CustID)
        // 得到用户ID为CustID的购物车内所有商品的string orderId,string productid，string productname，double unitprice，int productnum请按照我的顺序
        {
            String sql = "Select IdtID,a.CmdID,CmdName,CmdUP,IdtNum from db_Commodity a, db_Indent b where a.CmdID=b.CmdID and IdtStatus='购物车' and ShperID='" + CustID + "'";
            DataBase db = new DataBase();
            return db.GetDataSet(sql);
        }
        public void UpdateCustOrder(string OrderId, string status)//给你收银台的订单号，然后改成的状态为status
        {
            String sql = "update db_Indent set IdtStatus='" + status + "' where IdtID='" + OrderId + "'";
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void UpdateProductInfo(string OrderId, int productnum)
        //改变商品号为productid的库存，这边productnum是已经购买走的，所以直接库存 减 productnum
        {
            String sql = "update db_Commodity set CmdInventory=CmdInventory-" + productnum.ToString() + "where IdtID='" + OrderId + "'";
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void DeleteShoppingCart(string OrderId)
        // 删除订单号为OrderId的购物车内的商品订单
        {
            String sql = "delete from db_Indent where IdtID=" + OrderId.ToString();
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void UpdateShoppingCart(string OrderId, int cnum)
        // cnum表示买家对购物车内某个订单（OrderId）的商品数量进行加或者减，我在外面调好了正负，你直接加就行。
        {
            String sql = "update db_Indent set IdtNum=IdtNum+" + cnum.ToString() + "where IdtID='" + OrderId + "'";
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void UpdateBalance(string OrederId)
        //这个这个说出来你可能不信，这个给你订单id，表明这个订单支付完成，现在需要你把他的买家卖家的余额都更新一下。
        {
            String sql = "select * from db_Indent where IdtID=" + OrederId;
            DataBase db = new DataBase();
            DataRow dr = db.GetDataRow(sql);
            //db.ExecuteSQL(sql);
            sql = "update db_Shop set ShpBLCE=ShpBLCE+" + dr["IdtTP"].ToString() + "where ShpID='" + dr["ShpID"] + "'";
            db.ExecuteSQL(sql);
            sql = "update db_Shopper set ShperBLCE=ShperBLCE-" + dr["IdtTP"].ToString() + "where ShperID='" + dr["ShperID"] + "'";
            db.ExecuteSQL(sql);
        }
        public DataSet CustNumCompare(string OrderId)//给我该订单号的商品数量 和 该商品的 库存数量
        {
            //String sql = "select CmdID,ShpID from db_Indent where IdtID='" + OrderId + "'";
            DataBase db = new DataBase();
            //DataRow dr = db.GetDataRow(sql);

            string sql = "select IdtNum, CmdInventory from db_Indent a,db_Commodity b where a.CmdID = b.CmdID and a.ShpID = b.ShpID and a.IdtID = "+OrderId.ToString();
            return db.GetDataSet(sql);

        }



    }
}