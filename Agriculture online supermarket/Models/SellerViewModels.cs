using System;
using System.ComponentModel.DataAnnotations;

namespace Agriculture_online_supermarket.Models {
    /// <summary>
    /// 卖家首页显示商品列表模型
    /// </summary>
    public class SellerIndexViewModel
    {
        public string ProductId
        {
            get;set;
        }
        public string ProductName
        {
            get;set;
        }
        /// <summary>
        /// 商品库存
        /// </summary>
        public double Inventory
        {
            get;set;
        }
        public SellerIndexViewModel(string ProductId,string ProductName,double Inventory)
        {
            this.ProductId = ProductId;
            this.ProductName = ProductName;
            this.Inventory = Inventory;
        }
        public SellerIndexViewModel()
        {

        }
    }
    /// <summary>
    /// 商品信息修改/上传 模型 （原goods）
    /// </summary>
    public class ProductInfoViewModel
    {
        public string productId
        {
            get;set;
        }
        [Required(ErrorMessage = "商品名称必须填")]
        public string productName
        {
            get;set;
        }
        /// <summary>
        /// 商品信息CmdInfo
        /// </summary>
        [StringLength(120, ErrorMessage = "商品信息太长了，长度不要超过120")]
        public string productInfo
        {
            get;set;
        }
        /// <summary>
        /// 商品库存
        /// </summary>
        [Required(ErrorMessage = "商品库存必须填")]
        public double Inventory
        {
            get;set;
        }
        /// <summary>
        /// 计价单位 CmdUnit
        /// </summary>
        public string unit
        {
            get;set;
        }
        /// <summary>
        /// 商品单价 UP
        /// </summary>
        [Required(ErrorMessage = "商品单价必须填")]
        public double unitPrice
        {
            get;set;
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
        public ProductInfoViewModel(string productId,string productName,string productInfo,double Inventory,string unit,double unitPrice)
        {
            this.productId = productId;
            this.productName = productName;
            this.productInfo = productInfo;
            this.Inventory = Inventory;
            this.unit = unit;
            this.unitPrice = unitPrice;
        }
        public ProductInfoViewModel()
        {

        }
        
    }
    /// <summary>
    /// 卖家订单模型
    /// </summary>
    public class SellerOrderViewModel
    {
        public string orderId
        {
            get;set;
        }
        public string productName
        {
            get;set;
        }
        /// <summary>
        /// 货物数量 IdtNum
        /// </summary>
        public double productNum
        {
            get;set;
        }
        /// <summary>
        /// 订单状态 IdtStatus
        /// </summary>
        public string orderStatus
        {
            get;set;
        }
        /// <summary>
        /// 订单日期 Datetime
        /// </summary>
        public DateTime orderDate
        {
            set;get;
        }
        public SellerOrderViewModel()
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
        public SellerOrderViewModel(string orderId,string productName,double productNum,string orderStatus,DateTime orderDate)
        {
            this.orderId = orderId;
            this.productName = productName;
            this.productNum = productNum;
            this.orderStatus = orderStatus;
            this.orderDate = orderDate;
        }

    }
    /// <summary>
    /// 卖家商品详情模型（原Order）
    /// </summary>
    public class SellerOrderDetailViewModel
    {
        public string IdtID { set; get; }//订单号ID
        public string CmdName { set; get; }//商品名称
        public string ShperName { set; get; }//买家昵称
        public string LogisticsID { set; get; }//物流单号
        public int IdtNum { set; get; }//订单数量
        public string IdtStatus { set; get; }//订单状态
        public DateTime IdtDate { set; get; }//订单日期
        public double IdtTP { set; get; }//订单总价

    }
    /// <summary>
    /// 发货模型
    /// </summary>
    public class DeliveryViewModel
    {
        /// <summary>
        /// 订单id
        /// </summary>
        public string orderId
        {
            get;set;
        }
        //商品名称
        public string productName
        {
            get;set;
        }
        //商品数量
        public double productNum
        {
            get;set;
        }
        /// <summary>
        /// 物流号，需要卖家用户填写
        /// </summary>
        [Required]
        public string LogisticsID
        {
            get;set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">订单ID IdtID</param>
        /// <param name="productName">商品名称 CmdName</param>
        /// <param name="productNum">货物数量 IdtNum </param>
        /// <param name="LogisticsID">物流号</param>
        public DeliveryViewModel(string orderId,string productName,double productNum,string LogisticsID)
        {
            this.orderId = orderId;
            this.productName = productName;
            this.productNum = productNum;
            this.LogisticsID = LogisticsID;
        }
        
        public DeliveryViewModel()
        {

        }
    }
}