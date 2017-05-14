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
            string sql = "delete from db_Commodity where  CmdID="+ProductID;
            DataBase db = new DataBase();
            db.ExecuteSQL(sql);
        }
        public void ChangeState(String OrderID, String IDState)
        {
            //给出订单OrderID，将此订单的状态改为IDState
            string sql = " update db_Indent set IdtStatus="+IDState+ " from  db_Indent  where IdtID=" + OrderID;
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

        public void AddCmdInfo(String ShpID,String productName, String productInfo, String unit, String unitPrice)
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

        public void UpdateCmdInfo(String ShpID,String productId, String productName, String productInfo, String unit, String unitPrice)
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
    }
}