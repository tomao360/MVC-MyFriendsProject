using MyFriends.Models;
using System.ComponentModel.DataAnnotations;

namespace MyFriends.ViewsModels
{
    public class VMFriendWithImage
    {
        public VMFriendWithImage()
        {
            Friend = new Friend();
        }

        public Friend Friend { get; set; }


        [Display(Name = "Add Image")]
        public IFormFile File { get; set; }
    }
}
