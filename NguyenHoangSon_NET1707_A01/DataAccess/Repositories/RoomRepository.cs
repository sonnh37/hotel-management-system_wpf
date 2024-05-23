using BusinessObject.Models;
using DataAccess.IRepositories;
using DataAccess.Managements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public void Add(RoomInformation room)
        {
            RoomManagement.Instance.Add(room);
        }

        public void Delete(RoomInformation room)
        {
            RoomManagement.Instance.Delete(room);
        }

        public IEnumerable<RoomInformation> List()
        {
            return RoomManagement.Instance.GetAll();
        }

        public void Update(RoomInformation room)
        {
            RoomManagement.Instance.Update(room);
        }
    }
}
