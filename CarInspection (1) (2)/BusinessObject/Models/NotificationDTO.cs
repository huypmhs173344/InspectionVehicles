using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class NotificationDTO
    {
        public int NotificationId { get; set; }

        public int UserId { get; set; }

        public string Message { get; set; } = null!;

        public DateTime? SentDate { get; set; }

        public bool? IsRead { get; set; }

        public virtual User User { get; set; } = null!;

        public string FullName { get; set; } = null!;
    }
}
