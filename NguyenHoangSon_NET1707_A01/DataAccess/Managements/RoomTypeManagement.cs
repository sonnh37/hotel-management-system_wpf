using BusinessObject.Context;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Managements
{
    public class RoomTypeManagement : BaseManagement<RoomType>
    {
        private static RoomTypeManagement instance = null;
        private static readonly object instanceLock = new object();
        private FUMiniHotelManagementContext _context;

        public RoomTypeManagement(FUMiniHotelManagementContext context) : base(context)
        {
            _context = context;
        }

        public static RoomTypeManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomTypeManagement(new FUMiniHotelManagementContext());
                    }
                    return instance;
                }
            }
        }

        public void Add(RoomType roomtype)
        {

            RoomType p = FindOne(item => item.RoomTypeId.Equals(roomtype.RoomTypeId));
            if (p == null)
            {
                base.Add(roomtype);
            }
            else
            {
                throw new Exception("The roomtype is already exist");
            }
        }

        public void Delete(RoomType roomtype)
        {
            RoomType p = FindOne(item => item.RoomTypeId.Equals(roomtype.RoomTypeId));
            if (p != null)
            {
                base.Delete(roomtype);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The roomtype does not exist");
            }

        }

        public RoomType FindOne(Expression<Func<RoomType, bool>> predicate)
        {
            RoomType roomtype = null;
            try
            {
                roomtype = _context.RoomTypes.SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return roomtype;
        }

        public IEnumerable<RoomType> FindAll(Expression<Func<RoomType, bool>> predicate)
        {
            List<RoomType> roomtypes = new List<RoomType>();
            roomtypes = base.GetAll(predicate);
            return roomtypes;
        }

        public IEnumerable<RoomType> GetAll()
        {
            List<RoomType> roomtypes = new List<RoomType>();
            roomtypes = base.GetAll();

            return roomtypes;
        }
        public RoomType GetById(int id)
        {
            var queryable = base.GetQueryable<RoomType>();

            return queryable.Where(cus => cus.RoomTypeId == id).SingleOrDefault();
        }

        public void Update(RoomType roomtype)
        {
            RoomType p = FindOne(item => item.RoomTypeId == roomtype.RoomTypeId);
            if (p != null)
            {
                base.Update(p);
            }
            else
            {
                throw new Exception("The roomtype does not exist");
            }
        }
    }
}
