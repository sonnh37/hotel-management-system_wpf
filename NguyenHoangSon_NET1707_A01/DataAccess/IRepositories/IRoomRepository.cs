using BusinessObject.Models;
using BusinessObject.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IRoomRepository
    {
        IEnumerable<RoomInformation> GetAll();
        void Add(RoomInformation room);
        void Update(RoomInformation room);
        void Delete(RoomInformation room);
        IEnumerable<RoomInformation> GetAllByFilter(RoomView filter);
    }
}
