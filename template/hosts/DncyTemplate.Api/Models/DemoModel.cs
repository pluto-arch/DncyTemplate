using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DncyTemplate.Api.Models
{
    public class DemoModel
    {
        // valid error msg is dataannotation key in DefaultDataAnnotation.resx
        [Required]
        [Display(Name = "DemoModel.Name")]
        public string Name { get; set; }

        // StringLengthInvalid is dataannotation key in DataAnnotation.resx
        [StringLength(maximumLength:10,MinimumLength = 3,ErrorMessage = "StringLengthInvalid")]
        [Display(Name = "DemoModel.Address")]
        public string Address { get; set; }
    }
}