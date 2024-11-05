namespace app.Domain.Agregate.ObjectValues
{
    public class Price
    {
        private float _price;
        private void setPrice(float price)
        {
            _price = price;
            if (_price < 0)
            {
                _price = 0;
                throw new ArgumentException("this price is not valided");
            }
        }
        public float getValue(float price)
        { 
            setPrice(price);
            return _price;
        }
    }
}
