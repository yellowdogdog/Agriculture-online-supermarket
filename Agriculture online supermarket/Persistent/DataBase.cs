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
                // "Data Source=PC-20110101LQHV\\FIRSTDATABASE;Initial Catalog=BBS;Trusted_Connection=yes"
                "server=.;database=OLSHOP;user=sa;pwd=Hhhhhh123456"
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
        public static Boolean CustomerRegisterInsert(string CustomerId,
                                         string passwordMD5,//密码加密后的MD5
                                         string name, //昵称
                                         string address,

                                         string phone) //返回是否插入成功

        {
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
        static public void login(out int state, out string name,string UserId, string MD5password)
        {
            DataBase db = new DataBase();
            string sqlcheck = "";
            string sqllogin = "";
            sqlcheck = "Select * from db_Shopper where ShperID='" + UserId + "'";
            int exist = db.Count(sqlcheck);
            if (exist == 0)
            {
                sqlcheck = "Select * from db_Shop where ShpID='" + UserId + "'";
                exist = db.Count(sqlcheck);
                if (exist == 0)
                {
                    sqlcheck = "Select * from db_Manager where MngID='" + UserId + "'";
                    exist = db.Count(sqlcheck);
                    if (exist == 0)
                    {
                        state = -1;
                        name = "";
                    }
                    else
                    {//Admin user name exist
                        sqllogin = "Select * from db_Manager where MngID='" + UserId + "' and MngPSW=‘" + MD5password + "'";
                        DataRow dr = db.GetDataRow(sqllogin);
                        if (dr != null)
                        {
                            state = 3;
                            name = (string)dr[0];
                        }else
                        {
                            state = -2;
                            name = "";
                        }
                    }
                }
                else
                {//Seller user name exist
                    sqllogin = "Select * from db_Shop where ShpID='" + UserId + "' and ShpPSW=‘" + MD5password + "'";
                    DataRow dr = db.GetDataRow(sqllogin);
                    if (dr != null)
                    {
                        state = 2;
                        name = (string)dr[0];
                    }else
                    {
                        state = -2;
                        name = "";
                    }
                }
            }
            else
            {//Customer user name exist
                sqllogin = "Select ShperName from db_Shopper where ShperID='" + UserId + "' and ShperPSW=‘" + MD5password + "'"; 
                DataRow dr = db.GetDataRow(sqllogin);
                if (dr != null)
                {
                    state = 1;
                    name = (string)dr[0];
                }else
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
            sql = "Select * from db_Commodity   where CmdName=" + "'" + keyword + "'";
            DataSet ds = db.GetDataSet(sql);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                double price = Convert.ToDouble(dr["CmdUP"].ToString());//dr["CmdUP"].ToString()+"/"+dr["CmdUnit"].ToString();由于商品的不同所以单价以字符串的形式给出
                DataRow drtmp = db.GetDataRow("Select * from db_Shopper where ShperID='" + dr["ShpID"] + "'");
                String SellerName = drtmp["ShpName"].ToString();
                IndexModel im = new IndexModel(dr["CmdName"].ToString(),
                           price,
                            "/Content/images/productimage/vegetable1.png",//dr["UID"].ToString(),
                           SellerName,
                           keyword);
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
    }
}