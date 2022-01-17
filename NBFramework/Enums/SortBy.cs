using System.ComponentModel.DataAnnotations;

namespace Core.Enums
{
    public enum SortBy
    {
        [Display(Name = "OrderBy")]
        ASC = 1,

        [Display(Name = "OrderByDescending")]
        DESC = 2
    }
}
