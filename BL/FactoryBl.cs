using System;

namespace BL
{
    public class FactoryBl
    {
        public IBl GetInstance()
        {
            return new BlImp();
        }
    }
}
