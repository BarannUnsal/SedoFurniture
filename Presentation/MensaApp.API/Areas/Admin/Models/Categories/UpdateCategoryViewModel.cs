namespace MensaApp.API.Areas.Admin.Models.Categories
{
    public class UpdateCategoryViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
