using Microsoft.AspNetCore.Identity;
using System;

namespace Obs.Models
{
    public enum UserDomain
    {
        User,
        Admin,
        Ogrenci,
        Akademisyen,
        Super,
        Kullanici
    }
    public enum AdminDomain
    {
        User,
        Admin,
        Ogrenci,
        Akademisyen,
        Super,
        Kullanici
    }
    public enum GuestDomain
    {
        User,
        Admin,
        Ogrenci,
        Akademisyen,
        Super,
        Kullanici
    }

    public class AppUser : IdentityUser<Guid>
    {
        public UserDomain Domain { get; set; }
    }
    public class AdminRole : IdentityUser<Guid>
    {
        public AdminDomain Domain { get; set; }
    }
    public class GuestRole : IdentityUser<Guid>
    {
        public GuestDomain Domain { get; set; }
        public string Name { get; internal set; }
    }
}