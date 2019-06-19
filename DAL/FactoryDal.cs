using System;

namespace DAL
{
    public class FactoryDal
    {
        public IDal GetInstance()
        {
            return new DalImp();
        }
    }
}
