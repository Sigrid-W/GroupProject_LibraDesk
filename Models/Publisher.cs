using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{
    /// <summary>
    /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
    /// object class for Publisher made according to the database structure
    /// </summary>
    public class Publisher : Book
    {
        int publishId;
        string publishName;

        public int PublishId { get => publishId; set => publishId = value; }
        public string PublishName { get => publishName; set => publishName = value; }

        public Publisher() 
        { 
        }

        public Publisher(int publishId, string publishName, int bookId, string bookName, string authorName, int genreId, int amount, int availability)
            : base(bookId, bookName, authorName, genreId, publishId, amount, availability)
        {
            this.PublishId = publishId;
            this.PublishName = publishName;
        }

        public override string ToString()
        {
            return $"{PublishId} - {PublishName} : {BookName}";
        }
    }
}

