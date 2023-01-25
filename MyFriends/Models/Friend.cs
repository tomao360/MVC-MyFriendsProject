using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriends.Models
{
    public class Friend
    {
        public Friend()
        {
            Images = new List<Image>();  // Initialize the Images list
        }


        [Key] 
        public int ID { get; set; }


        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; } = "";  // "" Gives it a defult value


        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; } = "";  


        [Display(Name = "Full Name"), NotMapped]
        public string FullName { get { return FirstName + " " + LastName; } } // Just gets the value of the Full Name from the sirst + last name


        //[Display(Name = "Date of Birth"), DataType(DataType.Date)]
        //public DateTime Date { get; set; }


        [Display(Name = "Phone Number"), Phone(ErrorMessage = "Please enter a correct phone number")]
        public string PhoneNumber { get; set; }


        [Display(Name = "Email"), Required, EmailAddress(ErrorMessage = "Please enter a correct email address")]
        public string Email { get; set; } 


        [Display(Name = "City")]
        public string City { get; set; } = "";


        [Display(Name = "Street")]
        public string Street { get; set; } = "";


        [Display(Name = "Home Number")]
        public string HomeNumber { get; set; } = "";


        [Display(Name = "Address"), NotMapped]  // NotMapped -> Dont enter this property to DB
        public string Address { get { return City + " " + Street + " " + HomeNumber; } }


        // A list of images for each friend
        public List<Image> Images { get; set; }


        // A function that adds an image to a friend
        // פונקציה המוסיפה תמונה לחבר
        public void AddImage(IFormFile file)  // The function gets IFormFile because the user enters an image
        {
            if (file == null) return;

            // יצירת תמונה חדשה והוספתה לרשימת התמונות
            Images.Add(new Image { Friend = this, SetImage = file });  // The Image has 4 sets 
        }
    }
}
