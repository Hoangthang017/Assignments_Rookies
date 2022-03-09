using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Entities
{
    public class UserImage
    {
        public Guid userId { get; set; }

        [ForeignKey("userId")]
        public User User { get; set; }

        public int ImageId { get; set; }

        [ForeignKey("ImageId")]
        public Image Image { get; set; }
    }
}