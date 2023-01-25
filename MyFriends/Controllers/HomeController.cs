using Microsoft.AspNetCore.Mvc;
using MyFriends.Models;
using MyFriends.ViewsModels;
using System.Diagnostics;
using System.Data.Entity;

namespace MyFriends.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        // הפונקציה שעולה ראשונה
        public IActionResult Index()
        {
            // טעינת רשימת החברים ממסד הנתונים כולל התמונות שלהם
            List<Friend> friends = DataLayer.Data.Friends.Include(f=>f.Images).ToList(); // Converts the Friends table to a list, include the friend's images from the Images table, and enters it to friends
            return View(friends);
        }

        // פונקציה לעריכת פרטי חבר
        public IActionResult Edit(int? id)
        {
            // אם לא התקבל קוד חבר, שולח לדף הראשי
            if (id == null) return RedirectToAction(nameof(Index));

            Friend friend = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == id);
            // אם לא מצא את החבר
            if (friend == null) return RedirectToAction("Index");

            return View(friend);
        }

        // פונקציה שמקבלת את הטופס
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(Friend friend)
        {
            if (friend == null) return RedirectToAction(nameof(Index));
            Friend friendDB = DataLayer.Data.Friends.ToList().Find(f => f.ID == friend.ID);
            if (friend == null) return RedirectToAction("Index");
            friendDB.FirstName = friend.FirstName;
            friendDB.LastName = friend.LastName;
            friendDB.Email = friend.Email;
            friendDB.PhoneNumber = friend.PhoneNumber;
            friendDB.City = friend.City;
            friendDB.Street = friend.Street;
            friendDB.HomeNumber = friend.HomeNumber;
            DataLayer.Data.SaveChanges();

            return View("Details", new VMFriendWithImage { Friend = friendDB });
        }

        // פונקציה למחיקת חבר
        public IActionResult Delete(int id)
        {
            Friend friend = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == id);
            if (friend == null) return RedirectToAction("Index");
            return View(friend);
        }

        [HttpPost]
        public IActionResult Delete(Friend friend)
        {
            Friend friendDB = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == friend.ID);
            if (friendDB == null) return RedirectToAction("Index");

            // מחיקת כל התמונות שבטווח של החבר הזה
            DataLayer.Data.Images.RemoveRange(friendDB.Images);
            // מחיקת החבר
            DataLayer.Data.Friends.Remove(friendDB);

            DataLayer.Data.SaveChanges();
            return RedirectToAction("Index");
        }

        // פונקציה למחיקת תמונה
        public IActionResult DeleteImage(int id)
        {
            Image image = DataLayer.Data.Images.Include(i => i.Friend).FirstOrDefault(i => i.ID == id); // תכלול את החבר שהתמונה הזאת שייכת לו
            
            // מוצאים לפי התמונה את החבר
            Friend friend = image.Friend;
            // מחיקת התמונה מהחבר
            DataLayer.Data.Images.Remove(image);
            // שמירת הנתונים
            DataLayer.Data.SaveChanges();

            return View("Details", new VMFriendWithImage { Friend = friend });
        }

        // פונקציה המקבלת תמונה עבור חבר קיים במערכת
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult AddImage(VMFriendWithImage VM)
        {
            // במידה ולא התקבל אובייקט, חזרה לדף הבית
            if (VM == null) return RedirectToAction("Index");
            // מציאת החבר במסד נתונים שהמשתמש רוצה להוסיף לו תמונה
            Friend friend = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == VM.Friend.ID);
            // במידה ולא נמצא החבר, חוזר לדף הבית
            if (friend == null) return RedirectToAction("Index");
            if (VM.File != null)
            {
                friend.AddImage(VM.File);
                DataLayer.Data.SaveChanges();
            }

            return View("Details", new VMFriendWithImage { Friend = friend});
        }

        // פונקציה להצגת פרטי חבר
        public IActionResult Details(int? id)
        {
            // אם לא התקבל קוד חבר, שולח לדף הראשי
            if (id == null) return RedirectToAction(nameof(Index));

            // כל החברים שגרים בחדרה
            //List<Friend> friends = DataLayer.Data.Friends.Include(f => f.Images).ToList().FindAll(f => f.City == "Hadera");

            //Friend friend = DataLayer.Data.Friends.ToList().Find(f => f.ID == id);  // לא עדכני 
            Friend friend = DataLayer.Data.Friends.Include(f => f.Images).FirstOrDefault(f => f.ID == id);
            // אם לא מצא את החבר
            if (friend == null) return RedirectToAction("Index");

            return View(new VMFriendWithImage { Friend = friend});  // Create View Model and add to it the friend you want to show
        }

        // פונקציה ליצירת חבר חדש
        public IActionResult Create()
        {
            // שליחה לדף תצוגה של מודל מוכן המכיל גם חבר חדש וגם מקום לתמונה
            return View(new VMFriendWithImage());
        }

        // פונקציה שמקבלת את הטופס המלא מהמשתמש/תצוגה
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(VMFriendWithImage VM)
        {
            // הוספת החבר החדש לטבלה של החברים 
            DataLayer.Data.Friends.Add(VM.Friend);
            // הוספת התמונה לחבר
            VM.Friend.AddImage(VM.File);
            // שמירת הנתונים במסד הנתונים
            DataLayer.Data.SaveChanges();
            // צריך להחליט האם עוברים מכאן לרשימה הכללית או לפרטי החבר הנוכחי
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}