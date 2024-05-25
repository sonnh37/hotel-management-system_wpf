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
    public class RoomTypeRepository : IRoomTypeRepository
    {
        public void Add(RoomType roomtype)
        {
            RoomTypeManagement.Instance.Add(roomtype);
        }

        public void Delete(RoomType roomtype)
        {
            RoomTypeManagement.Instance.Delete(roomtype);
        }

        public IEnumerable<RoomType> GetAll()
        {
            return RoomTypeManagement.Instance.GetAll();
        }

        public void Update(RoomType roomtype)
        {
            RoomTypeManagement.Instance.Update(roomtype);
        }

        public RoomType GetById(int id)
        {
            return RoomTypeManagement.Instance.GetById(id);
        }

        public IEnumerable<RoomType> GetAllByFilter(RoomTypeView filter)
        {
            return filter != null ? RoomTypeManagement.Instance.FindAll(roomtype =>
                    (filter.RoomTypeId == null || roomtype.RoomTypeId == filter.RoomTypeId) &&
                    (filter.RoomTypeName == null || roomtype.RoomTypeName.ToLower().Trim().Contains(filter.RoomTypeName.ToLower().Trim())) &&
                    (filter.TypeDescription == null || roomtype.TypeDescription.ToLower().Trim().Contains(filter.TypeDescription.ToLower().Trim())) &&
                    (filter.TypeNote == null || roomtype.TypeNote.ToLower().Trim().Contains(filter.TypeNote.ToLower().Trim())))
                : GetAll();
        }
    }
}
