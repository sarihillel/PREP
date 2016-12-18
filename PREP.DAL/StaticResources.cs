using PREP.DAL.Repositories;
using PREP.DAL.Repositories.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PREP.DAL
{
    public static class StaticResources
    {
        private static Dictionary<string, int> _TableMap;
        public static Dictionary<string, int> TableMap
        {
            get
            {
                if (_TableMap == null)
                {
                    _TableMap = new Dictionary<string, int>();
                    using (ITableRepository db = new TableRepository())
                    {
                        //prop => prop.Name == table.Name || prop.Name == table.BaseType.Name
                        db.GetAll().ForEach(t => _TableMap.Add(t.Name, t.TableID));
                    }
                }
                return _TableMap;

            }
        }

        public static int GetTableID(string table)
        {
            return StaticResources.TableMap.FirstOrDefault(t => t.Key == table).Value;
        }

        public static string GetTableName(int id)
        {
            return StaticResources.TableMap.FirstOrDefault(t => t.Value == id).Key;
        }
    }
}
