using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using DBLibrary.Entities;

namespace DBLibrary.DAL
{
    public class BookDAO
    {
        string cnnString;
        public BookDAO(string cnnString)
        {
            this.cnnString = cnnString;
        }

        //add a book record
        public void AddRecord(Book book)
        {
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            //add record can lead to SQL injection
            //will switch to parameters to avoid that
            SqlCommand cmd = new SqlCommand($"insert into book (category,title,author,description,year,isbn,publisher,price)" +
                $"values (@Category,@Title,@Author,@Description,@Year,@ISBN,@Publisher,@Price)");
            cmd.Parameters.AddWithValue("@Category", book.Category.ToString());
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Description", book.Description);
            cmd.Parameters.AddWithValue("@Year", book.Year);
            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@Price", book.Price);
            cmd.Connection = cnn;

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        // list the book information
        public List<Book> ReadAll()
        {
            List<Book> books = new List<Book>();
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price from book");
                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows) { 
                    while (reader.Read())
                    {
                        books.Add(new Book(Convert.ToInt32(reader[0]), (Category)Enum.Parse(typeof(Category), reader[1].ToString()),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8])));
                    }
                }
            }
            return books;
        }

        //delete a book record
        public void DeleteRecord(int bID)
        {
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand($"delete from book where BookID = {bID}");
            cmd.Connection = cnn;

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        //Update a book records
        public void UpdateRecord(Book book)
        {
            SqlConnection cnn = new SqlConnection(cnnString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand($"update book set category=@Category,title=@Title,author=@Author,description=@Description,year=@Year," +
                $"isbn=@ISBN,publisher=@Publisher,price=@Price where bookID=@BookID");
            cmd.Parameters.AddWithValue("@BookID", book.BookID);
            cmd.Parameters.AddWithValue("@Category", book.Category.ToString());
            cmd.Parameters.AddWithValue("@Title", book.Title);
            cmd.Parameters.AddWithValue("@Author", book.Author);
            cmd.Parameters.AddWithValue("@Description", book.Description);
            cmd.Parameters.AddWithValue("@Year", book.Year);
            cmd.Parameters.AddWithValue("@ISBN", book.ISBN);
            cmd.Parameters.AddWithValue("@Publisher", book.Publisher);
            cmd.Parameters.AddWithValue("@Price", book.Price);

            cmd.Connection = cnn;

            cmd.ExecuteNonQuery();
            cnn.Close();
        }

        //find a book by id
        public Book FindByID(int id)
        {
            Book book = null; 
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price " +
                    $"from book where bookID=@BookID");
                cmd.Parameters.AddWithValue("@BookID", id);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    reader.Read();
                    book = new Book(Convert.ToInt32(reader[0]), (Category)Enum.Parse(typeof(Category), reader[1].ToString()),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8]));
                }
            }
            return book;
        }

        // find the book that has exaclty same book title
        public Book FindByBookTitle(string title)
        {
            Book book = null;
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price from book where title=@Title");
                cmd.Parameters.AddWithValue("@Title", title);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    book = new Book(Convert.ToInt32(reader[0]), (Category)Enum.Parse(typeof(Category), reader[1].ToString()),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8]));
                }
            }
            return book;
        }

        // find the books that have the similer title with LIKE %TItle
        public List<Book> FindByBooksTitle(string title)
        {
            List<Book> books = new List<Book>();
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price " +
                                                $"from book where title like @Title");

                cmd.Parameters.AddWithValue("@Title", "%" + title + "%");

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category.TryParse(reader[1].ToString(), out Category ocategory);

                        books.Add(new Book(Convert.ToInt32(reader[0]), ocategory,
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8])));
                    }
                }
            }
            return books;
        }

        // find the books with title keyword and category 
        public List<Book> FindByBooksTitle(string title, string cate)
        {
            List<Book> books = new List<Book>();
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price " +
                                                $"from book where category = @Category and title like @Title");

                cmd.Parameters.AddWithValue("@Title", "%" + title + "%");
                cmd.Parameters.AddWithValue("@Category", cate);

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Category.TryParse(reader[1].ToString(), out Category ocategory);

                        books.Add(new Book(Convert.ToInt32(reader[0]), ocategory,
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8])));
                    }
                }
            }
            return books;
        }

        public List<Book> FindByBooksByCategory(string category)
        {
            List<Book> books = new List<Book>();
            //open connection
            using (SqlConnection cnn = new SqlConnection(cnnString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand($"select bookID,category,title,author,description,year,isbn,publisher,price " +
                                                $"from book where UPPER(category) = @BookCategory");

                cmd.Parameters.AddWithValue("@BookCategory", category.ToUpper());

                cmd.Connection = cnn;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        books.Add(new Book(Convert.ToInt32(reader[0]), (Category)Enum.Parse(typeof(Category), reader[1].ToString()),
                            reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), Convert.ToInt32(reader[5]),
                            reader[6].ToString(), reader[7].ToString(), (float)Convert.ToDecimal(reader[8])));
                    }
                }
            }
            return books;
        }
    }
}
