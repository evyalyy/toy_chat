using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models;

public class Message
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public UInt64 Id { get; set; }

    public string Content { get; set; }

    public UserUuid UserId { get; set; }
}