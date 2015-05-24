using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using edziennik.Validators;
using Models.Models;
using System;

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

    public class SubjectCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Id nauczyciela")]
        public string TeacherId { get; set; }

        [Display(Name = "Id klasy")]
        public int ClasssId { get; set; }

        [Display(Name = "Id sali")]
        public int ClassroomId { get; set; }

        [Display(Name = "Dzień zajęć")]
        public SchoolDay Day { get; set; }

        [Display(Name = "Godzina zajęć")]
        public int Hour { get; set; }

        public List<SelectListItem> Teachers { get; set; }

        public List<SelectListItem> Classrooms { get; set; }

        public List<SelectListItem> Classes { get; set; }

        public List<SelectListItem> Days { get; set; }

        public List<SelectListItem> Hours { get; set; }

    }

    public class MarkCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Wartość")]
        public double Value { get; set; }

        [Display(Name = "Id ucznia")]
        public string StudentId { get; set; }

        [Display(Name = "Id przedmiotu")]
        public int SubjectId { get; set; }

        [Display(Name = "Id nauczyciela")]
        public string TeacherId { get; set; }

        [Display(Name = "Opis")]
        public string Description { get; set; }

        public List<SelectListItem> Subjects { get; set; }

        public List<SelectListItem> Values { get; set; }
    }

    public class ClassCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Id wychowawcy")]
        public string TeacherId { get; set; }

        public List<SelectListItem> Teachers { get; set; }

    }

    public class ClassEditViewModel : ClassCreateViewModel
    {
        public List<Student> Students { get; set; }
    }
          

    public class PersonViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Display(Name = "Pesel")]
        public string Pesel { get; set; }
    }

    public class StudentViewModel : PersonViewModel
    {
        [Display(Name = "Klasa")]
        public string ClassName { get; set; }

        [Display(Name = "Numer telefonu komórkowego rodzica")]
        public string CellPhoneNumber { get; set; }

        public List<MarkViewModel> Marks { get; set; }
    }

    public class StudentListItemViewModel : PersonViewModel
    {
        [Display(Name = "Klasa")]
        public string ClassName { get; set; }
    }

    public class TeacherViewModel : PersonViewModel
    {
    }

    public class MarkViewModel
    {
        [Display(Name = "Nauczyciel")]
        public string Teacher { get; set; }

        [Display(Name = "Przedmiot")]
        public string Subject { get; set; }

        [Display(Name = "Ocena")]
        public double Value { get; set; }               
    }

    public class MarkListItemViewModel : MarkViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nauczyciel")]
        public string TeacherId { get; set; }

        [Display(Name = "Klasa")]
        public string Classs { get; set; } 

        [Display(Name = "Uczeń")]
        public string  Student { get; set; }

    }

    public class MarkDetailsViewModel : MarkListItemViewModel
    {
        [Display(Name = "Opis")]
        public string Description { get; set; }
    }

    public class SubjectViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Nauczyciel")]
        public string Teacher{ get; set; }

        [Display(Name = "Klasa")]
        public string Classs { get; set; }

        [Display(Name = "Sala")]
        public string Classroom { get; set; }

        [Display(Name = "Dzień zajęć")]
        public SchoolDay Day { get; set; }

        [Display(Name = "Godzina zajęć")]
        public int Hour { get; set; }
    }

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

    public class StudentSubjectMarks
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        public List<Mark> Marks { get; set; }
    }

    public class StudentAddMark
    {
        public string Id { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }
        [Display(Name = "Przedmiot")]
        public int SubjectId { get; set; }
        [Display(Name = "Ocena")]
        public double Mark { get; set; }
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

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Pole Pesel jest wymagane.")]
        [Display(Name = "Pesel")]       
        //[Pesel (ErrorMessage = "Wprowadzono nieprawidłowy numer pesel")]
        //[UniquePesel(ErrorMessage = "Podany numer pesel już istnieje ")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Pole Imię jest wymagane.")]
        [StringLength(30, ErrorMessage = "{0} musi się składać minimum z {2} znaków.", MinimumLength = 2)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Drugie imię")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Pole Nazwisko jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi się składać minimum z {2} znaków.", MinimumLength = 2)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Pole Email jest wymagane.")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class StudentRegisterViewModel : RegisterViewModel
    {
      [Display(Name = "Klasa")]
      public int ClassId { get; set; }

      [Display(Name = "Numer telefonu komórkowego rodzica")]
      [RegularExpression(@"\d{9}", ErrorMessage = "Niepoprawny numer !")]
      public string CellPhoneNumber { get; set; }
      
      public List<SelectListItem> Classes { get; set; }
    }

    public class StudentEditViewModel : StudentRegisterViewModel
    {
        public string Id { get; set;}
    }

    public class TeacherEditViewModel : RegisterViewModel
    {
        public string Id { get; set; }
    }


    public class TeacherRegisterViewModel : RegisterViewModel
    {

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

    public class LogListItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Data")]
        public DateTime Date { get; set; }

        [Display(Name = "Akcja")]
        public string Action { get; set; }

        [Display(Name = "Kto")]
        public string Who { get; set; }

        [Display(Name = "Co")]
        public string What { get; set; }

    }

    public class LogDetailsViewModel : LogListItemViewModel
    {
        [Display(Name = "Adres Ip")]
        public string Ip { get; set; }
        
        [Display(Name = "Komu")]
        public string WhatId { get; set; }
    }
}
