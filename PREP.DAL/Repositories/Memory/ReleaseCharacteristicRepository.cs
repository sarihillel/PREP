using PREP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PREP.DAL.Repositories.Memory
{
    public class ReleaseCharacteristicRepository : GenericRepository<PREPContext, ReleaseCharacteristic>, IReleaseCharacteristicRepository
    {
        public List<ReleaseCharacteristic> AddCharacteristic()
        {
            List<ReleaseCharacteristic> ReleaseCharacteristic = new List<ReleaseCharacteristic> { };
            using (ICharacteristicRepository db = new CharacteristicRepository())
            {
                //get all family Product who's active
                IEnumerable<Characteristic> Characteristic = db.Get().Where(c=>c.IsDeleted==false); //m => m.IsVisible == true

                //in save needs get releaseid and add it
                Characteristic.ToList().ForEach(sh => ReleaseCharacteristic.Add(new ReleaseCharacteristic() { CharacteristicID = sh.CharacteristicID, Characteristic = sh }));

            }
            return ReleaseCharacteristic;

        }
    }
}
