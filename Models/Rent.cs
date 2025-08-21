using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{ /// <summary>
  /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
  /// object class for Rent made according to the database structure
  /// </summary>
    public class Rent
    {
        int rentId;
        int memberId;
        int librarianId;
        DateTime rentDate;

        

        public int RentId { get => rentId; set => rentId = value; }
        public int MemberId { get => memberId; set => memberId = value; }
        public int LibrarianId { get => librarianId; set => librarianId = value; }
        public DateTime RentDate { get => rentDate; set => rentDate = value; }

        public Rent()
        {
        }

        public Rent(int rentId, int memberId, int librarianId, DateTime rentDate)
        {
            this.RentId = rentId;
            this.MemberId = memberId;
            this.LibrarianId = librarianId;
            this.rentDate = rentDate;
        }

        public override string ToString()
        {
            return $"Rent ID: {RentId}, Member ID: {MemberId}, Librarian ID: {LibrarianId}, Rent Date: {RentDate}";
        }
    }
}
