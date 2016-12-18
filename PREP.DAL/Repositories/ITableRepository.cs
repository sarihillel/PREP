using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories
{
    public interface ITableRepository: IGenericRepository<Table>
    {
        /// <summary>
        /// return Table By Type of Entity
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        Table GetTableByType(Type TName);
        int GetTableIdByTableName(string TableName);
    }
   
}
