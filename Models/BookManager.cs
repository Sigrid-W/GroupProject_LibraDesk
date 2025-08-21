using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{
    /// <summary>
    /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
    /// manages book operations such as adding, editing, and removing books
    /// </summary>
    public class BookManager
    {
        DBhandler db = new DBhandler();

        // Add book
        public void AddBook(Book book)
        {
            if (!db.IsValidPublisher(book.PublishId))
                throw new InvalidOperationException("Invalid Publisher ID");

            db.AddBook(book);
        }

        // Edit book
        public void EditBook(Book book)
        {
            db.EditBook(book);
        }

        // Remove book
        public void RemoveBook(int bookId)
        {
            db.RemoveBook(bookId);
        }
    }
}
