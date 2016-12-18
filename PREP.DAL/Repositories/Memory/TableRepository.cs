using PREP.DAL.Models;
using PREP.DAL.TableViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL.Repositories.Memory
{
    public class TableRepository : GenericRepository<PREPContext, Table>, ITableRepository
    {
        /// <summary>
        /// return Table By Type of Entity
        /// </summary>
        /// <param name="TName"></param>
        /// <returns></returns>
        public Table GetTableByType(Type TName)
        {
            return FindSingle(c => c.Name == TName.Name.ToString());
        }
        public int GetTableIdByTableName(string TableName)
        {
            return DbSet.AsNoTracking().FirstOrDefault(a=>a.Name== TableName).TableID;
        }

    }
}
