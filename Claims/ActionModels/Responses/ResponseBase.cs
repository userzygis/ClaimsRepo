using System.ComponentModel.DataAnnotations;

namespace Claims.ActionModels.Responses
{
    public abstract class ResponseBase
    {
        [Required]
        public string Error { get; set; }
    }
}
