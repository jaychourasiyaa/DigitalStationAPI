using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DigitalStation.Contract.Models;

public class Login : Base
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsLoginDeactived {get;set;} = false;
    public int? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
}
