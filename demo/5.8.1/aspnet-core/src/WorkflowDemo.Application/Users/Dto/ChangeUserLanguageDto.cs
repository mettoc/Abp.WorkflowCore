using System.ComponentModel.DataAnnotations;

namespace WorkflowDemo.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}