using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyEvent.Models;
#nullable disable warnings
public class DB : DbContext
{
    public DB(DbContextOptions options) : base(options)
    {
    }

        // DB Sets
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Member> Members { get; set; }

        internal void SaveChanegs()
        {
            throw new NotImplementedException();
        }
    

        public DbSet<Category> Categories { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Detail> Details { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

    public class Address
    {
        [Key, MaxLength(8)]
        public string Id { get; set; }


        [MaxLength(100)]
        public string Premise { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        [MaxLength(5)]
        public string Postcode { get; set; }  // Use string, not int, to preserve leading 0s

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Event Event { get; set; }
    }

    
    public class Category
    {
        [Key, MaxLength(8)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public List<Event> Events { get; set; } = [];
    }

 
 

    public class Event
    {
        [Key, MaxLength(8)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string Title { get; set; }

        public string ImageUrl { get; set; }

     
        public string AddressId { get; set; }
        public Address Address { get; set; }

     
        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public Detail Detail { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }




    public class Detail
    {
        [Key]
        [MaxLength(8)]
        public string Id { get; set; }

        [MaxLength(100)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Organizer { get; set; }

        [MaxLength(100)]
        public string ContactEmail { get; set; }

        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string EventId { get; set; }
        public Event Event { get; set; }
    }

    public class User
    {
        [Key, MaxLength(8)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        public string Hash { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string PhotoURL { get; set; }
        // TODO
        public string Role => GetType().Name;
    }

    public class Ticket
    {
        public int TicketId { get; set; }

        public string EventId { get; set; }
        public Event Event { get; set; }

        public int BuyerId { get; set; } // who paid
        public User Buyer { get; set; }

        [Required]
        public string HolderName { get; set; } 
        public string? HolderEmail { get; set; } 

        public DateOnly PurchaseDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    }



    // TODO
    public class Admin : User
    {
        public List<Event> OrganizedEvents { get; set; } = [];
    }

    // TODO
    public class Member : User
    {

    }

}




