using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{ /// <summary>
  /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
  /// object class for RentDetail made according to the database structure
  /// </summary>
    public class RentDetail : Rent
    {
        int rentDetailId;
        int bookId;
        DateTime dueDate;
        DateTime returnDate;


        public int RentDetailId { get => rentDetailId; set => rentDetailId = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }

        public RentDetail() { }

        public RentDetail(int rentDetailId, int bookId, DateTime dueDate, DateTime returnDate, int rentId, int memberId, int librarianId, DateTime rentDate) : base(rentId, memberId, librarianId, rentDate)
        {
            this.RentDetailId = rentDetailId;
            this.BookId = bookId;
            this.DueDate = dueDate;
            this.ReturnDate = returnDate;
        }

        public override string ToString()
        {
            return $"Rent ID: {RentId}, Book ID: {BookId}, Rent Date: {RentDate}";
        }
    }
}
