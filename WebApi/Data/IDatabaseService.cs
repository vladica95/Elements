using Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public interface IDatabaseService
    {
         void AddData(ElementP elP);

         List<ElementP> GetData(string dateTime);

    }
}
