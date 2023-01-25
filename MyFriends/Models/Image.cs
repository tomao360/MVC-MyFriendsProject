using System.ComponentModel.DataAnnotations;

namespace MyFriends.Models
{
    public class Image
    {
        public Image() { }


        [Key]
        public int ID { get; set; }


        [Required]
        public Friend Friend { get; set; }


        [Display(Name = "Image")]
        public byte[] MyImage { get; set; }  // We save the image in DB in SQL as a byte array


        // A property that enters an image to DB
        // תכונת הוספה של תמונה 
        public IFormFile SetImage
        {
            set
            {
                // בדיקה אם התמונה קיימת
                if (value == null) return;
                // יוצרים מקום בזיכרון המכיל קובץ 
                MemoryStream stream = new MemoryStream();
                // העתקת הקובץ מהמשתמש למקום שנוצר בזיכרון
                value.CopyTo(stream);
                // הפיכת הזיכרון למערך כדי שיוכל להיכנס למסד הנתונים
                MyImage = stream.ToArray();
            }
        }

    }
}
