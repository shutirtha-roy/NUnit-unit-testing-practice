using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitLibrary.ClassesWithDependencies.Repository
{
    public interface IUnitOfWork
    {
        IQueryable<T> Query<T>();
    }
}
