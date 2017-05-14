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
}