using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraDesk.Models
{
    /// <summary>
    /// Xin, Anagha,Elijah and Aryan, Aug 20,2025
    /// Read, Write and Update the database for the library management system
    /// </summary>
    public class DBhandler
    {

        static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string dbPath = System.IO.Path.Combine(baseDirectory, "LibraryDB.db");
        static string connect_string = $@"Data Source={dbPath}";

        //Manage Renting System
        public List<Book> SearchBooksByName(string bookName)// Search books for rent
        {
            List<Book> books = new List<Book>();

            using (SQLiteConnection connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = "SELECT BookId, BookName, AuthorName, GenreId, PublishId, Amount, Availability " +
                             "FROM Book WHERE BookName LIKE @searchName";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@searchName", $"%{bookName}%");

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Book book = new Book(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                reader.GetInt32(6)
                            );
                            books.Add(book);
                        }
                    }
                }
            }

            return books;
        }
    


        public int InsertRent(RentDetail rental) // add new rent item in rent table
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();

                string sqlRent = @"INSERT INTO Rent(RentDate, MemberId, LibrarianId)
                               VALUES(@RentDate, @MemberId, @LibrarianId);
                               SELECT last_insert_rowid();";

                using (var cmd = new SQLiteCommand(sqlRent, connection))
                {
                    cmd.Parameters.AddWithValue("@RentDate", rental.RentDate);
                    cmd.Parameters.AddWithValue("@MemberId", rental.MemberId);
                    cmd.Parameters.AddWithValue("@LibrarianId", rental.LibrarianId);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void InsertRentDetail(int rentId, RentDetail rental)// add new rent item in rent details table
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = @"INSERT INTO RentDetails(RentId, BookId, DueDate)
                           VALUES(@RentId, @BookId, @DueDate)";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@RentId", rentId);
                    cmd.Parameters.AddWithValue("@BookId", rental.BookId);
                    cmd.Parameters.AddWithValue("@DueDate", rental.DueDate);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DecreaseBook(int bookId)// decrease book quantity when rented
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = @"UPDATE Book
                       SET Amount = Amount - 1
                       WHERE BookId = @BookId AND Amount > 0"; // prevents negative values

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<RentDetail> GetRentRecords(int rentId) // get rent records by rent id
        {
            List<RentDetail> records = new List<RentDetail>();
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = @"SELECT r.RentId, r.RentDate, r.MemberId, r.LibrarianId,
                                  rd.RentDetailId, rd.BookId, rd.DueDate, rd.ReturnDate
                           FROM Rent r
                           INNER JOIN RentDetails rd ON r.RentId = rd.RentId
                           WHERE r.RentId = @RentId";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@RentId", rentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int rId = reader.GetInt32(0);
                            DateTime rentDate = reader.GetDateTime(1);
                            int memberId = reader.GetInt32(2);
                            int librarianId = reader.GetInt32(3);
                            int rentDetailId = reader.GetInt32(4);
                            int bookId = reader.GetInt32(5);
                            DateTime dueDate = reader.GetDateTime(6);
                            DateTime returnDate = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);

                            RentDetail record = new RentDetail(rentDetailId, bookId, dueDate, returnDate, rId, memberId, librarianId, rentDate);
                            records.Add(record);
                        }
                    }
                }
            }
            return records;
        }

        public void ReturnBook(int rentDetailId, DateTime returnDate)   // update return date when book is returned
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = "UPDATE RentDetails SET ReturnDate = @ReturnDate WHERE RentDetailId = @RentDetailId";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@ReturnDate", returnDate);
                    cmd.Parameters.AddWithValue("@RentDetailId", rentDetailId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void IncreaseBook(int bookId) // increase book quantity when returned
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = @"UPDATE Book
                       SET Amount = Amount + 1
                       WHERE BookId = @BookId";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.ExecuteNonQuery();
                }

                string updateAvailability = @"UPDATE Book
                                      SET Availability = 1
                                      WHERE BookId = @BookId AND Amount > 0";
                using (var cmd2 = new SQLiteCommand(updateAvailability, connection))
                {
                    cmd2.Parameters.AddWithValue("@BookId", bookId);
                    cmd2.ExecuteNonQuery();
                }
            }
        }

        //manage Members
        public List<Member> SearchMembers(string memberName) // Search members by name
        {
            List<Member> members = new List<Member>();

            using (SQLiteConnection connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string sql = "SELECT MemberId, MemberName FROM Member WHERE MemberName LIKE @searchName";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@searchName", $"%{memberName}%");

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Member member = new Member(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            );
                            members.Add(member);
                        }
                    }
                }
            }

            return members;
        }

        public int AddMember(Member member)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connect_string))
            {
                conn.Open();
                string sql = "INSERT INTO Member (MemberName) VALUES (@Name); SELECT last_insert_rowid();";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", member.MemberName);
                    int id = Convert.ToInt32(cmd.ExecuteScalar()); 
                    return id;
                }
            }
        }

        public void EditMember(Member member)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string query = "UPDATE Member SET MemberName = @MemberName WHERE MemberId = @MemberId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MemberName", member.MemberName);
                    cmd.Parameters.AddWithValue("@MemberId", member.MemberId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveMember(int memberId)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connect_string))
            {
                connection.Open();
                string query = "DELETE FROM Member WHERE MemberId = @MemberId";
                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MemberId", memberId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Manage Books
        public bool IsValidPublisher(int publishId)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connect_string))
            {
                conn.Open();
                string sql = "SELECT COUNT(1) FROM Publish WHERE PublishId = @PublishId";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PublishId", publishId);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public int AddBook(Book book)
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();

                // Check if PublishId exists
                string checkPublisher = "SELECT COUNT(*) FROM Publish WHERE PublishId = @PublishId";
                using (var checkCmd = new SQLiteCommand(checkPublisher, connection))
                {
                    checkCmd.Parameters.AddWithValue("@PublishId", book.PublishId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                        throw new InvalidOperationException("Invalid PublishId.");
                }

                string sql = @"INSERT INTO Book (BookName, AuthorName, GenreId, PublishId, Amount)
                       VALUES (@BookName, @AuthorName, @GenreId, @PublishId, @Amount);
                       SELECT last_insert_rowid();";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@BookName", book.BookName);
                    cmd.Parameters.AddWithValue("@AuthorName", book.AuthorName);
                    cmd.Parameters.AddWithValue("@GenreId", book.GenreId);
                    cmd.Parameters.AddWithValue("@PublishId", book.PublishId);
                    cmd.Parameters.AddWithValue("@Amount", book.Amount);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void EditBook(Book book)
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();

                // Check if PublishId exists
                string checkPublisher = "SELECT COUNT(*) FROM Publish WHERE PublishId = @PublishId";
                using (var checkCmd = new SQLiteCommand(checkPublisher, connection))
                {
                    checkCmd.Parameters.AddWithValue("@PublishId", book.PublishId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                        throw new InvalidOperationException("Invalid PublishId.");
                }

                string sql = @"UPDATE Book
                       SET BookName = @BookName,
                           AuthorName = @AuthorName,
                           GenreId = @GenreId,
                           PublishId = @PublishId,
                           Amount = @Amount
                       WHERE BookId = @BookId";

                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@BookName", book.BookName);
                    cmd.Parameters.AddWithValue("@AuthorName", book.AuthorName);
                    cmd.Parameters.AddWithValue("@GenreId", book.GenreId);
                    cmd.Parameters.AddWithValue("@PublishId", book.PublishId);
                    cmd.Parameters.AddWithValue("@Amount", book.Amount); 
                    cmd.Parameters.AddWithValue("@BookId", book.BookId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveBook(int bookId)
        {
            using (var connection = new SQLiteConnection(connect_string))
            {
                connection.Open();

                // Check if the book is rented
                string checkSql = "SELECT COUNT(*) FROM RentDetails WHERE BookId = @BookId";
                using (var checkCmd = new SQLiteCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@BookId", bookId);
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count > 0)
                        throw new InvalidOperationException("Cannot delete book: it is currently rented.");
                }

                // Safe to delete
                string sql = "DELETE FROM Book WHERE BookId = @BookId";
                using (var cmd = new SQLiteCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@BookId", bookId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
