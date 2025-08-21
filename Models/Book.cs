using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{
    /// <summary>
    /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
    /// object class for Book made according to the database structure
    /// </summary>
    public class Book
    {
        int bookId;
        string bookName;
        string authorName;
        int genreId;
        int publishId;
        int amount;
        int availability;

        public int BookId { get => bookId; set => bookId = value; }
        public string BookName { get => bookName; set => bookName = value; }
        public string AuthorName { get => authorName; set => authorName = value; }
        public int GenreId { get => genreId; set => genreId = value; }
        public int PublishId { get => publishId; set => publishId = value; }
        public int Amount { get => amount; set => amount = value; }
        public int Availability { get => availability; set => availability = value; }
        
        

        public Book()
        {
        }

        public Book(int bookId, string bookName, string authorName, int genreId, int publishId, int amount, int availability)
        {
            this.BookId = bookId;
            this.BookName = bookName;
            this.AuthorName = authorName;
            this.GenreId = genreId;
            this.PublishId = publishId;
            this.Amount = amount;
            this.Availability = availability;
        }

        public override string ToString()
        {
            return $"{BookId} - {BookName} by {AuthorName}";
        }
    }
}
