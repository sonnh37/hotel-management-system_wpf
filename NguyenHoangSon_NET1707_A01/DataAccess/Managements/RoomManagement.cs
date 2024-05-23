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
    public class RoomManagement : BaseManagement<RoomInformation>
    {
        private static RoomManagement instance = null;
        private static readonly object instanceLock = new object();
        private FUMiniHotelManagementContext _context;
       
        public RoomManagement(FUMiniHotelManagementContext context) : base(context)
        {
            _context = context;
        }

        public static RoomManagement Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomManagement(new FUMiniHotelManagementContext());
                    }
                    return instance;
                }
            }
        }

        public void Add(RoomInformation room)
        {

            RoomInformation p = FindOne(item => item.RoomId.Equals(room.RoomId));
            if (p == null)
            {
                base.Add(room);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The room is already exist");
            }
        }

        public void Delete(RoomInformation room)
        {
            RoomInformation p = FindOne(item => item.RoomId.Equals(room.RoomId));
            if (p != null)
            {
                base.Delete(room);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The room does not exist");
            }

        }

        public RoomInformation FindOne(Expression<Func<RoomInformation, bool>> predicate)
        {
            RoomInformation room = null;
            try
            {
                room = _context.RoomInformations.SingleOrDefault(predicate);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return room;
        }

        public IEnumerable<RoomInformation> FindAll(Expression<Func<RoomInformation, bool>> predicate)
        {
            List<RoomInformation> rooms = new List<RoomInformation>();
            rooms = base.GetAll(predicate);
            return rooms;
        }

        public void Update(RoomInformation room)
        {
            RoomInformation p = FindOne(item => item.RoomId.Equals(room.RoomId));
            if (p != null)
            {
                base.Update(p);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("The room does not exist");
            }
        }
    }
}
