using BusinessObject.Models;
using BusinessObject.Views;
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

        public IEnumerable<RoomInformation> GetAll()
        {
            return RoomManagement.Instance.GetAll();
        }

        public void Update(RoomInformation room)
        {
            RoomManagement.Instance.Update(room);
        }

        public IEnumerable<RoomInformation> GetAllByFilter(RoomView filter)
        {
            return filter != null ? RoomManagement.Instance.FindAll(room =>
                    (filter.RoomId == null || room.RoomId == filter.RoomId) &&
                    (filter.RoomNumber == null || room.RoomNumber.ToLower().Trim().Contains(filter.RoomNumber.ToLower().Trim())) &&
                    (filter.RoomDetailDescription == null || (room.RoomDetailDescription != null && room.RoomDetailDescription.ToLower().Trim().Contains(filter.RoomDetailDescription.ToLower().Trim()))) &&
                    (filter.RoomMaxCapacity == null || room.RoomMaxCapacity == filter.RoomMaxCapacity) &&
                    (filter.RoomTypeId == null || room.RoomTypeId == filter.RoomTypeId) &&
                    (filter.RoomPricePerDay == null || room.RoomPricePerDay == filter.RoomPricePerDay))
                : GetAll();
        }
    }
}
