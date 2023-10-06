namespace HouseRentingSystem.Data.Models 
{
    using Microsoft.AspNetCore.Identity;

    /// <summary>
    /// This is our custom user class that from now on is the default ASP.Net Core Identity
    /// This is how we add additional info to the bu ilt-in Identity User , or modify it.
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid> 
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid();
            this.RentedHouses = new HashSet<House>();
        }

        public virtual ICollection<House> RentedHouses { get; set; }
    }
}
