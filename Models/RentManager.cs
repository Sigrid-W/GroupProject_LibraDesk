using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace LibraDesk.Models
{ /// <summary>
  /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
  /// manager class for Rent to handle operations related to renting and returning books
  /// </summary>
    public class RentManager
    {
        static DBhandler db = new DBhandler();

        public static List<Book> books = new List<Book>();
        public static List<RentDetail> records = new List<RentDetail>();

        public static List<Book> SearchBook(string bookName)
        {
            DBhandler db = new DBhandler();
            return db.SearchBooksByName(bookName);
        }


        public static string RentBook(RentDetail rental) // Method to rent a book
        {
            int newRentId = db.InsertRent(rental);
            db.InsertRentDetail(newRentId, rental);
            return $"Book rented successfully! ID: {newRentId}";
        }

        public static List<RentDetail> SearchRent(int rentId) // Method to search for rent records by Rent ID
        {
            records = db.GetRentRecords(rentId);
            return records;
        }

        public static string ReturnBook(int rentDetailId, DateTime returnDate) // Method to return a book
        {
            db.ReturnBook(rentDetailId, returnDate);
            return "Book returned successfully!";
        }
    }
}
