namespace Daf.SharedModule.Domain.BaseVo
{
    public abstract record Size : GreaterThanZeroDouble
    {
        protected Size(double value) : base(value)
        {
        }

        public abstract Size Sub(double value);
        public abstract Size Sub(Size value);
        public abstract Size Mul(double value);
    }

}

