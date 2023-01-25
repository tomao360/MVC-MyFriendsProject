using System.Data.Entity;

namespace MyFriends.Models
{
    public class DataLayer:DbContext
    {
        private DataLayer() : base("Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=MyFriends;Data Source=localhost\\SQLEXPRESS")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DataLayer>());
            // כאשר מסד הנתונים ריק, מפעיל את הפונקציה הזורעת
            if (Friends.Count() == 0) Seed();
        }


        // יצירת מודל פנימי סטטי 
        private static DataLayer data;
        
        // קישור למודל הפנימי
        public static DataLayer Data
        {
            get
            {
                // אתחול בפעם הראשונה בלבד
                if (data == null) data = new DataLayer();
                return data;
            }          
        }

        // טבלת חברים
        public DbSet<Friend> Friends { get;set; }

        // טבלת תמונות
        public DbSet<Image> Images { get;set; }


        // פונקציה הזורעת את מסד הנתונים בפעם הראשונה 
        private void Seed()
        {
            // מכניסים חבר ראשון למערכת עם יצירת הדאטה בייס
            // יצירת חבר ראשוני בטבלה
            Friend friend = new Friend
            {
                FirstName = "Tamara",
                LastName = "Osipov",
                City = "Hadera",
                Email = "tomao360@gmail.com",
                PhoneNumber = "0520000000",
                //Date = DateTime.Now.AddYears(-29),
            }; 
            // הוספת החבר לטבלה
            Friends.Add(friend);
            // שמירת שינויים
            SaveChanges();
        }
    }
}
