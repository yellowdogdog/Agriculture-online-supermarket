using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using System.IO;
using Agriculture_online_supermarket.Models;

namespace Agriculture_online_supermarket.Persistent
{
    public class DataBase
    {
        public DataBase()
        { }
        public static SqlConnection DBCon()
        {
            return new SqlConnection(
               "server=123.206.103.58;database=OLSHOP;user=sa;pwd=Hhhhhh123456"
                );
        }
        public DataSet GetDataSet(String SqlString)
        {
            SqlConnection conn = DBCon();
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(SqlString, conn);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            conn.Close();
            return dataset;
        }
        public DataRow GetDataRow(string SqlString)
        {
            DataSet dataset = GetDataSet(SqlString);
            if (dataset.Tables[0].Rows.Count > 0)
                return dataset.Tables[0].Rows[0];
            else
                return null;

        }
        public void ExecuteSQL(String SqlString)
        {
            SqlConnection conn = DBCon();
            conn.Open();
            SqlCommand comm = new SqlCommand(SqlString, conn);
            comm.ExecuteNonQuery();
        }
        /*public static void ShowString(String SqlString)
        {
            string filePath = "d:\\infor.txt";//这里是你的已知文件
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            fs.SetLength(0);//首先把文件清空了。
            sw.Write(SqlString);//写你的字符串。
            sw.Close();
        }*/
        public void Insert(String TableName, Hashtable ht)
        {
            int n = 0;
            string Fields = "(";
            string Values = "Values(";
            foreach (DictionaryEntry item in ht)
            {
                if (n != 0)
                {
                    Fields += ",";
                    Values += ",";
                }
                Fields += item.Key.ToString();
                Values += item.Value.ToString();
                n++;
            }
            Fields += ")";
            Values += ")";
            string SqlString = "insert into " + TableName + Fields + Values;

            ExecuteSQL(SqlString);
        }
        public void ExecuteSQL(ArrayList SqlStrings)
        {
            SqlConnection conn = DBCon();
            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            foreach (String str in SqlStrings)
            {
                comm.CommandText = str;
                //ShowString(str);
                comm.ExecuteNonQuery();
            }
            conn.Close();
        }
        public void Updata(String TableName, Hashtable ht, String Where)
        {
            int n = 0;
            string Fields = "";
            foreach (DictionaryEntry item in ht)
            {
                if (n != 0) Fields += ",";
                Fields += item.Key.ToString();
                Fields += "=";
                Fields += item.Value.ToString();
                n++;
            }
            Fields += " ";
            string SqlString = "Update " + TableName + " set " + Fields + Where;
            ExecuteSQL(SqlString);

        }
        public int Count(String SqlString)
        {

            SqlConnection conn = DBCon();
            conn.Open();
            SqlCommand comm = new SqlCommand(SqlString, conn);
            return Convert.ToInt32(comm.ExecuteScalar());
        }
        public static int exist(string KeyWord)//1:存在于Shop中,2存在于Shopper中，3：存在于manager中
        {
            String sql = "select count (*) from db_Shop where ShpID='" + KeyWord + "'";
            DataBase db = new DataBase();
            int c = db.Count(sql);
            if (c > 0) return 1;
            sql = "select count (*) from db_Shopper where ShperID='" + KeyWord + "'";
            c = db.Count(sql);
            if (c > 0) return 2;
            sql = "select count (*) from db_Manager where MngID='" + KeyWord + "'";
            c = db.Count(sql);
            if (c > 0) return 3;
            return -1;
        }
        public static Boolean CustomerRegisterInsert(string CustomerId,
                                         string passwordMD5,//密码加密后的MD5
                                         string name, //昵称
                                         string address,

                                         string phone) //返回是否插入成功

        {
            if (exist(CustomerId) == 1 || exist(CustomerId) == 2) return false;
            DataBase db = new DataBase();
            Hashtable ht = new Hashtable();
            ht.Add("ShperID", "'" + CustomerId + "'");
            ht.Add("ShperName", "'" + name + "'");
            ht.Add("ShperPSW", "'" + passwordMD5 + "'");
            ht.Add("ShperAD", "'" + address + "'");
            ht.Add("ShperPhone", "'" + phone + "'");
            db.Insert("db_Shopper", ht);
            return true;
        }
        /// <summary>
        /// 卖家注册信息插入
        /// </summary>
        /// <param name="SellerId"></param>
        /// <param name="passwordMD5">//密码加密后的MD5</param>
        /// <param name="name">昵称</param>
        /// <param name="address"></param>
        /// <param name="phone"></param>
        /// <returns>返回是否插入成功</returns>
        public static Boolean SellerInsert(string SellerId,
                                       string passwordMD5,
                                       string name,
                                       string address,

                                       string phone)
        {
            if (exist(SellerId) == 1 || exist(SellerId) == 2) return false;
            DataBase db = new DataBase();
            Hashtable ht = new Hashtable();
            ht.Add("ShpID", "'" + SellerId + "'");
            ht.Add("ShpName", "'" + name + "'");
            ht.Add("ShpPSW", "'" + passwordMD5 + "'");
            ht.Add("ShpAD", "'" + address + "'");
            ht.Add("ShpPhone", "'" + phone + "'");
            //User user = new User();
            //user.Add(ht);
            db.Insert("db_Shop", ht);
            return true;
        }
        /// <summary>
        /// 检查用户名密码并返回状态和信息（找到用户名并对比md5是否相同）
        /// </summary>
        /// <param name="state">
        /// -1：用户名不存在
        /// -2：密码错误
        /// 1：买家登入
        /// 2：卖家登入
        /// 3：管理员登入
        /// </param>
        /// <param name="name">用户昵称</param>
        /// <param name="MD5password">密码md5值</param>
        static public void login(out int state, out string name, string UserId, string MD5password)
        {
            DataBase db = new DataBase();
            string sqlcheck = "";
            string sqllogin = "";
            sqlcheck = "Select Count(*) from db_Shopper where ShperID='" + UserId + "'";
            int exist = db.Count(sqlcheck);
            if (exist == 0)
            {
                sqlcheck = "Select Count(*) from db_Shop where ShpID='" + UserId + "'";
                exist = db.Count(sqlcheck);
                if (exist == 0)
                {
                    sqlcheck = "Select Count(*) from db_Manager where MngID='" + UserId + "'";
                    exist = db.Count(sqlcheck);
                    if (exist == 0)
                    {
                        state = -1;
                        name = "";
                    }
                    else
                    {//Admin user name exist
                        sqllogin = "Select * from db_Manager where MngID='" + UserId + "' and MngPSW='" + MD5password + "'";
                        DataRow dr = db.GetDataRow(sqllogin);
                        if (dr != null)
                        {
                            state = 3;
                            name = (string)dr[0];
                        }
                        else
                        {
                            state = -2;
                            name = "";
                        }
                    }
                }
                else
                {//Seller user name exist
                    sqllogin = "Select * from db_Shop where ShpID='" + UserId + "' and ShpPSW='" + MD5password + "'";
                    DataRow dr = db.GetDataRow(sqllogin);
                    if (dr != null)
                    {
                        state = 2;
                        name = (string)dr[0];
                    }
                    else
                    {
                        state = -2;
                        name = "";
                    }
                }
            }
            else
            {//Customer user name exist
                sqllogin = "Select ShperName from db_Shopper where ShperID='" + UserId + "' and ShperPSW='" + MD5password + "'";
                DataRow dr = db.GetDataRow(sqllogin);
                if (dr != null)
                {
                    state = 1;
                    name = (string)dr[0];
                }
                else
                {
                    state = -2;
                    name = "";
                }

            }

            //switch (state)
            //{
            //    case 1:
            //        sqlcheck = "Select * from db_Shopper where ShperID='" + name + "'";
            //        sqllogin = "Select * from db_Shopper where ShperID='" + name + "' and ShperPSW=‘" + MD5password + "'"; break;
            //    case 2:
            //        sqlcheck = "Select * from db_Shop where ShpID='" + name + "'";
            //        sqllogin = "Select * from db_Shop where ShpID='" + name + "' and ShpPSW=‘" + MD5password + "'"; break;
            //    case 3:
            //        sqlcheck = "Select * from db_Manager where MngID='" + name + "'";
            //        sqllogin = "Select * from db_Manager where MngID='" + name + "' and MngPSW=‘" + MD5password + "'"; break;
            //}
            ////sql = "Select * from db_User where UID=" + "'" + UserID + "'";
            //int exist = db.Count(sqlcheck);
            //if (exist == 0) { state = -1; return; }
            //exist = db.Count(sqllogin);
            //if (exist == 0) { state = -2; return; }
            //switch
        }
        /// <summary>
        /// 根据搜索关键词查询填充首页商品信息模型
        /// </summary>
        /// <param name="keyword">查询关键词，若为空则返回全部</param>
        /// <returns>首页模型列表</returns>
        static public List<IndexModel> getIndexModelByKeyWord(string keyword)
        {
            List<IndexModel> tmp = new List<IndexModel>();
            DataBase db = new DataBase();
            string sql = "";
            if (keyword.Length == 0) sql = "Select * from db_Commodity";
            else
                sql = "Select * from db_Commodity   where CmdName like" + "'%" + keyword + "%'";
            DataSet ds = db.GetDataSet(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double price = Convert.ToDouble(dr["CmdUP"].ToString());//dr["CmdUP"].ToString()+"/"+dr["CmdUnit"].ToString();由于商品的不同所以单价以字符串的形式给出
                DataRow drtmp = db.GetDataRow("Select * from db_Shopper where ShperID='" + dr["ShpID"] + "'");
                String SellerName = drtmp["ShpName"].ToString();
                IndexModel im = new IndexModel(dr["CmdName"].ToString(),
                           price,
                            dr["PhotoUrl"].ToString(),
                           SellerName,
                           (int)dr["CmdID"]);
                tmp.Add(im);
            }
            return tmp;
            /* DataRow dr = db.GetDataRow(sql);
             if (dr != null)
             {
                 string price = Convert.ToDouble(dr["CmdUP"].ToString());//dr["CmdUP"].ToString()+"/"+dr["CmdUnit"].ToString();由于商品的不同所以单价以字符串的形式给出
                 DataRow drtmp = db.GetDataRow("Select * from db_Shopper where ShperID='" + name + "'");
                 String SellerName = drtmp["ShpName"].ToString(),//这里应该是Seller s=new Seller(ShpID); SellerName=s.Name;
                 IndexModel(dr["CmdName"].ToString(),
                            "暂无"//dr["UID"].ToString(),
                            price,
                            SellerName,
                            productID);
             }*/
        }

        UserinfoModel getUserInfo(string UserId)
        {

            UserinfoModel ufm;
            int e = exist(UserId);
            //DataBase db = new DataBase();
            if (e == 2)
            {
                string sql = "select * from db_Shopper where ShperID='" + UserId + "'";
                DataRow dr = this.GetDataRow(sql);
                String Name = dr["ShperName"].ToString();
                String PSW = dr["ShperPSW"].ToString();
                String Phone = dr["ShperPhone"].ToString();
                String Ad = dr["ShperAD"].ToString();
                double blc = Convert.ToDouble(dr["ShperBLCE"]);
                ufm = new UserinfoModel(UserId, Name, PSW, Ad, Phone, blc);
            }
            else
            if (e == 1)
            {
                string sql = "select * from db_Shop where ShpID='" + UserId + "'";
                DataRow dr = this.GetDataRow(sql);
                String Name = dr["ShpName"].ToString();
                String PSW = dr["ShpPSW"].ToString();
                String Phone = dr["ShpPhone"].ToString();
                String Ad = dr["ShpAD"].ToString();
                double blc = Convert.ToDouble(dr["ShpBLCE"]);
                ufm = new UserinfoModel(UserId, Name, PSW, Ad, Phone, blc);
            }
            else ufm = null;
            return ufm;
        }
        double getBalace(string UserId)//设定查找失败时返回-1
        {
            //DataBase db = new DataBase();
            int e = exist(UserId);
            if (e < 1 || e > 2) return -1;
            string sql;
            if (e == 2)
                sql = "select ShperBLCE from db_Shopper where ShperID='" + UserId + "'";
            else
                sql = "select ShpBLCE from db_Shop where ShpID='" + UserId + "'";
            //else blc = 0;
            DataRow dr = this.GetDataRow(sql);
            if (dr == null) return -1;
            if (e == 2) sql = "ShperBLCE";
            else sql = "ShpBLCE";
            double blc = Convert.ToDouble(dr[sql]);
            return blc;
        }
        void deleteAccount(string UserId)
        {
            int e = exist(UserId);
            if (e < 1 || e > 3) return;

            String sql = "delete from db_Shopper where ShperID='" + UserId + "'";
            if (e == 1) sql = "delete from db_Shop where ShpID='" + UserId + "'";
            if (e == 3) sql = "delete from db_Manager where MngID='" + UserId + "'";
            this.ExecuteSQL(sql);
        }
        void saveAccountInfo(UserinfoModel model)
        {
            Hashtable ht = new Hashtable();
            int e = exist(model.ID);
            if (e < 1 || e > 2) return;
            if (e == 1)
            {
                ht.Add("ShpName", "'" + model.name + "'");
                ht.Add("ShpPSW", "'" + model.passward + "'");
                ht.Add("ShpAD", "'" + model.adress + "'");
                ht.Add("ShpPhone", "'" + model.phoneNumber + "'");
                ht.Add("ShpBLCE", model.balance.ToString());
                String where = "Where ShpID='" + model.ID + "'";
                this.Updata("db_Shop", ht, where);
            }
            else
            {
                ht.Add("ShperName", "'" + model.name + "'");
                ht.Add("ShperPSW", "'" + model.passward + "'");
                ht.Add("ShperAD", "'" + model.adress + "'");
                ht.Add("ShperPhone", "'" + model.phoneNumber + "'");
                ht.Add("ShperBLCE", model.balance.ToString());
                String where = "Where ShperID='" + model.ID + "'";
                this.Updata("db_Shopper", ht, where);
            }
            //ht.Add("ShperID", "'"+model.ID+"'");

        }

        void payBalance(string UserId, double Money)
        {
            int e = exist(UserId);
            if (e < 1 || e > 2) return;
            String sql;
            if (e == 1)
            {
                sql = "update Shop Set ShpBLCE=ShpBLCE+" + Money.ToString() + " where ShpID='" + UserId + "'";
            }
            else
                sql = "update Shopper Set ShperBLCE=ShpBLCE+" + Money.ToString() + " where ShperID='" + UserId + "'";
            this.ExecuteSQL(sql);
        }
        void refundBalance(string UserId, double Money)
        {
            int e = exist(UserId);
            if (e < 1 || e > 2) return;
            String sql;
            if (e == 1)
            {
                sql = "update Shop Set ShpBLCE=ShpBLCE-" + Money.ToString() + " where ShpID='" + UserId + "'";
            }
            else
                sql = "update Shopper Set ShperBLCE=ShpBLCE-" + Money.ToString() + " where ShperID='" + UserId + "'";
            this.ExecuteSQL(sql);
        }
        /*public DataSet GetAllCmdInfo(string ShpID)
        {
            string sql = "select * from db_Commodity WHere ShpID like%'"+ShpID+"%'";
            DataBase db = new DataBase();
            DataSet ds= db.GetDataSet(sql);
            return ds;
            //获得该卖家的所有商品信息
        }
        public DataSet GetCmdInfo(string CmdID, string ShpID)
        {
            string sql = "select CmdID,CmdName,CmdInfo,CmdInventory,CmdUnitt,CmdUP from db_Commodity "
                             +"WHere ShpID like %'" + ShpID 
                                    + "%' and CmdID like %" + CmdID + "%'"; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
            //根据商品ID返回该商品信息
        }
        public DataSet GetAllIdtInfo(string ShpID)
        {
            string sql = "select IdtID,CmdID,IdtNum,IdtStatus,IdtDate from db_Indent WHere ShpID='" + ShpID + "'";
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
            //获得该卖家的所有订单信息
        }
        public DataSet GetIdtInfo(string IdtID)
        {
            string sql = "select CmdName ,ShperName,Logistics,IdtNum,IdtStatus ,IdtDate ,IdtTP "
                        + "from db_Indent db_Indent a,db_Commodity b, db_Shop c "
                        + " where a.ShpID=b.ShpID and  a.CmdID=b.CmdID and a.ShpID=c,ShpID and IdtID=" + IdtID ; ;
            DataBase db = new DataBase();
            DataSet ds = db.GetDataSet(sql);
            return ds;
            //获得该订单ID的所有信息
        }
        public DataSet GetDelivery(string OrderId)
        {
            string sql = "select IdtID,CmdName,IdtNum,Logistics from db_Indent a,db_Commodity b "
                       +"WHere a.ShpID=b.ShpID and  a.CmdID=b.CmdID  and IdtID=" + OrderId;
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
            Hashtable ht = new Hashtable();
            ht.Add("Logistics", "'"+LogisticsID+"'");
            String where = " Where IdtID= '" + orderId + "'";
            DataBase db = new DataBase();
            db.Updata("db_Indent",ht,where);
            //修改订单ID为orderId的物流号为LogisticsID
        }
       
        public void AddCmdInfo(String ShpID, String productName, String productInfo, String unit, String unitPrice)
        {
            String sql = "Select CmdID from db_Commodity where ShpID='"+ShpID+"'";
            DataBase db = new DataBase();
            int max = db.Count(sql);
            max++;
            Hashtable hs = new Hashtable();
            hs.Add("CmdID",max.ToString());
            hs.Add("ShpID","'"+ShpID+"'");
            hs.Add("CmdName", "'" + productName + "'");
            hs.Add("CmdInfo", "'" + productInfo+ "'");
            hs.Add("CmdUnit", "'" + unit+ "'");
            hs.Add("CmdUP",  unitPrice );
            db.Insert("db_Commodity",hs);
            //新增商品信息,生成一个商品编号
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
        }
        public void UpdateCmdInfo(String ShpID, String productId, String productName, String productInfo, String unit, String unitPrice)
        {
            String where = " Where ShpID='" + ShpID + "'and CmdID='" + productId + "'";
            Hashtable ht = new Hashtable();
            ht.Add("CmdName","'"+productName+"'");
            ht.Add("CmdInfo","'"+productInfo+"'");
            ht.Add("CmdUnit","'"+unit+"'");
            ht.Add("CmdUP",unitPrice);
            //ht.Add();
            DataBase db = new DataBase();
            db.Updata("db_Commodity",ht,where);
            //更新商品信息商品ID为productId的商品信息
            //productName 商品名，productInfo 商品信息,unit 商品单位,unitPrice 商品单价
        }
        */

    }
}