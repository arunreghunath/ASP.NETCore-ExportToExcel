using System.ComponentModel.DataAnnotations;

namespace Demo.ExportToExcel.Web.Models
{
    public class GuestResponse
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public bool? WillAttend { get; set; }
    }
}
