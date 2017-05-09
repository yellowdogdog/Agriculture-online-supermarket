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
}