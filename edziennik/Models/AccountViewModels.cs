﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using edziennik.Validators;

namespace edziennik.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Obecne hasło")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi się składać z minimum  {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź nowe hasło")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "Podane hasła nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserListItemViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }
        [Display(Name = "Email potwierdzony")]
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }
    }

    public class UserDetailsViewModel : UserListItemViewModel
    {
        [Display(Name = "Avatar użytkownika")]
        public string AvatarUrl { get; set; }
        [Display(Name = "Role użytkownika")]
        public string[] UserRoles { get; set; }

        public List<SelectListItem> Roles { get; set; }
    }

    public class UserEditViewModel : UserDetailsViewModel { }
          
    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Kod")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "Pole Pesel jest wymagane.")]
        [Display(Name = "Pesel")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole Hasło jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }

    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Pole Login jest wymagane.")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "Pole Pesel jest wymagane.")]
        //[Display(Name = "Pesel")]       
        [Pesel (ErrorMessage = "Wprowadzono nieprawidłowy numer pesel")]
        [UniquePesel(ErrorMessage = "Podany numer pesel już istnieje ")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [RegularExpression(@"^[a-zA-Z ąćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$", ErrorMessage = "Imię może składac się tylko z liter")]
        [StringLength(30, ErrorMessage = "{0} musi się składać minimum z {2} znaków.", MinimumLength = 2)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z ąćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$", ErrorMessage = "Imię może składac się tylko z liter")]
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [RegularExpression(@"^[a-zA-Z ąćęłńóśźżĄĆĘŁŃÓŚŹŻ]+$", ErrorMessage = "Imię może składac się tylko z liter")]
        [StringLength(100, ErrorMessage = "{0} musi się składać minimum z {2} znaków.", MinimumLength = 2)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Email potwierdzony")]
        public bool EmailConfirmed { get; set; }

    }


    public class EditorRegisterViewModel : RegisterViewModel
    {

    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Pole Hasło jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi się składać minimum z {2} znaków..", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Podane hasła nie są takie same.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
