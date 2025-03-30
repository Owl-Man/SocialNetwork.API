using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.DataAccess.Entities;

public class UserEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id;

    public string FirstName { get; set; } = string.Empty;

    public string SecondName { get; set; } = string.Empty;

    public string Bio { get; set; } = string.Empty;
    
    public List<PostEntity> Posts { get; set; } = new();
}