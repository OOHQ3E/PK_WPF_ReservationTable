using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationTable_PK
{
    public class Reservation
    {
        public Reservation() { }
        public Reservation(int seatrow, int seatcolumn)
        {
            ReservedBy = reservedby;
            SeatRow = seatrow;
            SeatColumn = seatcolumn;
        }

        public Reservation(string reservedby, int seatrow, int seatcolumn) : this(seatrow, seatcolumn)
        {
            ReservedBy = reservedby;
        }
        public Reservation(int id, string reservedby, int seatrow, int seatcolumn) : this(reservedby, seatrow, seatcolumn)
        {
            ID = id;
        }


        private int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string reservedby;

        public string ReservedBy
        {
            get { return reservedby; }
            set { reservedby = value; }
        }
        private int seatrow;

        public int SeatRow
        {
            get { return seatrow; }
            set { seatrow = value; }
        }
        private int seatcolumn;

        public int SeatColumn
        {
            get { return seatcolumn; }
            set { seatcolumn = value; }
        }


    }
}
