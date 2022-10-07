namespace MensaApp.API.Areas.Admin.Models.SubCategories
{
    public class SubCategoryUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
