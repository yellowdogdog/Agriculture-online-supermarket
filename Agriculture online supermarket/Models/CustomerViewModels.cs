using System;

namespace Agriculture_online_supermarket.Models
{
    public class IndexModel{
        public string productName
        {
            set;get;
        }
        public double price
        {
            get;set;
        }
        public string imagePath
        {
            set;get;
        }
        public string sellerName
        {
            set;get;
        }
        public string productID {
            get;set;
        }
        public IndexModel(string productName,double price ,string imagePath,string sellerName,string productID)
        {
            this.productName = productName;
            this.imagePath = imagePath;
            this.sellerName = sellerName;
            this.productName = productName;
            this.productID = productID;
        }
        public IndexModel() { }
        
    }
    public class CashierViewModel
    {
        public string productid
        {
            get; set;
        }
        public string productname
        {
            get; set;
        }
        public double unitprice
        {
            get; set;
        }
        public int productnum
        {
            get; set;
        }
        public CashierViewModel(string productid,string productname,double unitprice,int productnum)
        {
            this.productid = productid;
            this.productname = productname;
            this.unitprice = unitprice;
            this.productnum = productnum;
        }
        public CashierViewModel() { }
        
    }
    public class DetailViewModel
    {
        public string productId
        {
            get; set;
        }
        //[Required(ErrorMessage = "商品名称必须填")]
        public string productName
        {
            get; set;
        }

        //[StringLength(120, ErrorMessage = "商品信息太长了，长度不要超过120")]
        public string productInfo
        {
            get; set;
        }
        /// <summary>
        /// 商品库存
        /// </summary>
        //[Required(ErrorMessage = "商品库存必须填")]
        public double Inventory
        {
            get; set;
        }
        /// <summary>
        /// 计价单位 CmdUnit
        /// </summary>
        public string unit
        {
            get; set;
        }
        /// <summary>
        /// 商品单价 UP
        /// </summary>
        //[Required(ErrorMessage = "商品单价必须填")]
        public double unitPrice
        {
            get; set;
        }

        /// <summary>
        /// 商品图片路径
        /// </summary>
        public string imagePath
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId">商品id CmdID</param>
        /// <param name="productName">商品名称 CmdName</param>
        /// <param name="productInfo">商品信息 CmdInfo</param>
        /// <param name="Inventory">商品库存 CmdInventory</param>
        /// <param name="unit">计数单位 CmdUnit</param>
        /// <param name="unitPrice">商品单价 CmdUP</param>
        public DetailViewModel(string productId, string productName, string productInfo, double Inventory, string unit, double unitPrice, string imagePath)
        {
            this.productId = productId;
            this.productName = productName;
            this.productInfo = productInfo;
            this.Inventory = Inventory;
            this.unit = unit;
            this.unitPrice = unitPrice;
            this.imagePath = imagePath;
        }
        public DetailViewModel()
        {

        }

    }


    /// <summary>
    /// 买家订单模型
    /// </summary>
    public class CustomerOrderViewModel
    {

        /// <summary>
        /// 卖家用户名
        /// </summary>
        public string username
        {
            get; set;
        }
        public string orderId
        {
            get; set;
        }
        public string productName
        {
            get; set;
        }
        public string productNo
        {
            get; set;
        }
        /// <summary>
        /// 货物数量 IdtNum
        /// </summary>
        public double productNum
        {
            get; set;
        }
        /// <summary>
        /// 订单状态 IdtStatus
        /// </summary>
        public string orderStatus
        {
            get; set;
        }
        /// <summary>
        /// 订单日期 Datetime
        /// </summary>
        public DateTime orderDate
        {
            set; get;
        }

        /// <summary>
        /// 物流编号
        /// </summary>
        public string LogisticsNo
        {
            set; get;
        }
        public double TotalMoney
        {
            set; get;
        }
        public CustomerOrderViewModel()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单号ID IdtID</param>
        /// <param name="productName">商品ID CmdID</param>
        /// <param name="productNum">货物数量 IdtNum</param>
        /// <param name="orderStatus">订单状态 IdtStatus</param>
        /// <param name="orderDate">订单日期 IdtDate</param>
        public CustomerOrderViewModel(string username, string orderId, string productName, string productNo, double productNum, string orderStatus, DateTime orderDate, string LogisticsNo, double TotalMoney)
        {
            this.username = username;
            this.orderId = orderId;
            this.productName = productName;
            this.productNo = productNo;
            this.productNum = productNum;
            this.orderStatus = orderStatus;
            this.orderDate = orderDate;
            this.TotalMoney = TotalMoney;
            this.LogisticsNo = LogisticsNo;
        }

    }
}