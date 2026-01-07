using ProyectoInterfazNatural.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoInterfazNatural.ViewModels
{
    public class PerfilLibroViewModel
    {
        public User User { get; set; }
        public Book Book { get; set; }
        public ICommand AddToLibraryCommand { get; }
        public PerfilLibroViewModel(User user, Book book) { 
            User= user;
            Book= book;
            AddToLibraryCommand= new Command(async () =>
            {
                // Aseguramos que la lista existe
                if (User.myBooks == null)
                {
                    User.myBooks = new List<Book>();
                }

                // miramos a ver si el usuario tiene ya el libro en su biblioteca
                if (User.myBooks.Any(b => b.ID == Book.ID))
                {
                    await Application.Current.MainPage.DisplayAlert("Información", "Este libro ya está en tu biblioteca.", "OK");
                    return;
                }

                // Añadimos libro si todo va bien
                User.myBooks.Add(Book);
                await Application.Current.MainPage.DisplayAlert("Añadido", $"\"{Book.Title}\" se ha añadido a tu biblioteca.", "OK");
            });
        }
    }
}
