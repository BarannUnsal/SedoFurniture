namespace MensaApp.API.Areas.Admin.Models.SubCategories
{
    public class SubCategoryCreateViewModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
