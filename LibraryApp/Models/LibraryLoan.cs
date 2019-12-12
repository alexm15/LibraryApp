using System;

namespace LibraryApp.Models
{
    public class LibraryLoan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public DateTime RentedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}