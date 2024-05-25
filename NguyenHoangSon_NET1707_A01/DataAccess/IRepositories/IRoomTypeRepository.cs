using BusinessObject.Models;
using BusinessObject.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IRoomTypeRepository
    {
        void Add(RoomType roomtype);
        void Delete(RoomType roomtype);
        IEnumerable<RoomType> GetAll();
        IEnumerable<RoomType> GetAllByFilter(RoomTypeView filter);
        RoomType GetById(int id);
        void Update(RoomType roomtype);
    }
}
