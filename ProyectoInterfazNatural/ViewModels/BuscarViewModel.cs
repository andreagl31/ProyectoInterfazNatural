using ProyectoInterfazNatural.Model;
using ProyectoInterfazNatural.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProyectoInterfazNatural.ViewModels
{
    public class BuscarViewModel
    {
        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Book> AllBooks;
        public string SearchText { get; set; }
        public ICommand SeeMoreCommand { get; set; }
        public ICommand SearchCommand { get; }
        public BuscarViewModel(User user)
        {
            AllBooks=new ObservableCollection<Book>{
                new Book(1,"aquiles.jpg","La canción de Aquiles","Grecia en la era de los héroes." +
                "Patroclo, un príncipe joven y torpe, ha sido exiliado al reino de Ftía, " +
                "donde vive a la sombra del rey Peleo y su hijo divino, Aquiles." +
                "\r\n\r\nPero el destino nunca está lejos de los talones de Aquiles. Cuando se extiende la noticia del rapto de Helena de Esparta, se convoca a los hombres de Grecia para asediar la ciudad de Troya. " +
                "Aquiles, seducido por la promesa de un destino glorioso, se une a la causa, y Patroclo, lo sigue a la guerra. " +
                "Poco podía imaginar que los años siguientes iban a poner a prueba todo cuanto habían aprendido y todo cuanto valoraban profundamente.","Madeline Miller","Narrativa Histórica","8.5","9788413622132"),
                new Book(2, "ecodestino.jpg", "El eco del destino (Time Keeper 1)",
                "En un mundo donde un amuleto puede alterar el tiempo, Nathan Tabiz sabe que no debe usarlo, pero cuando domina ese poder arriesga el equilibrio del mundo… y descubre que la persona de la que está enamorado morirá en tres días, poniendo todo su deber y sus emociones en conflicto.",
                "Iria G. Parente & Selene M. Pascual", "Fantasía juvenil", "8.2", "9788427240759"),
                new Book(3, "nickcharlie.jpg", "Nick y Charlie",
                "Nick y Charlie, la pareja perfecta, se enfrentan al mayor reto de su relación cuando Nick se va a la universidad y Charlie se queda atrás en el instituto, obligándolos a cuestionarse si su amor puede sobrevivir a la distancia.",
                "Alice Oseman", "Romance juvenil LGBTQ+", "8.4", "9780008389666"),
                new Book(4, "thebones.jpg", "The Bones Beneath My Skin",
                "Después de perder a su familia y su trabajo, Nate regresa a la cabaña familiar en Oregon y encuentra a Alex y a una niña misteriosa llamada Artemis, desatando una aventura de supervivencia, vínculos inesperados y secretos que cambiarán sus vidas.",
                "TJ Klune", "Fantasía/Drama contemporáneo", "8.5", "9781035002306"),
                new Book(5, "mentirosos.jpg", "Perfectos mentirosos",
                "En una universidad elitista dominada por fiestas y jerarquías sociales, Jude planea desenmascarar los oscuros secretos de los tres hermanos Cash, famosos por su atractivo y poder… en una historia de mentiras, intrigas y verdades ocultas.",
                "Alex Mírez", "Romance juvenil / Thriller ligero", "7.9", "9788418057618"),
                new Book(6, "caraval.jpg", "Caraval",
                "Scarlett y su hermana Donatella reciben una invitación para participar en el mágico y legendario juego Caraval, donde la línea entre realidad y fantasía se difumina. Scarlett debe encontrar a su hermana antes de que el juego termine o los oscuros secretos de Caraval los consuman.",
                "Stephanie Garber", "Fantasía juvenil", "8.6", "9788410239166"),
                new Book(7, "evelynhugo.jpg", "Los siete maridos de Evelyn Hugo",
                "Evelyn Hugo, una legendaria estrella de Hollywood de casi ochenta años, decide revelar la verdad de su vida y sus siete matrimonios a la periodista Monique Grant, desvelando amor, ambición y secretos que transformarán ambas vidas.",
                "Taylor Jenkins Reid", "Narrativa contemporánea / Ficción histórica", "9.1", "9788416517275"),
                new Book(8, "heartsong.jpg", "Heartsong (La canción del corazón)",
                 "Robbie Fontaine ha ido de manada en manada toda su vida formando lazos temporales para mantenerse cuerdo, pero lo que siempre ha deseado es ser amado y encontrar un hogar al que pertenecer. Cuando un brujo lo presenta a la manada de Caswell, Robbie se convierte en el segundo de la Alfa Michelle Hughes y por fin tiene un lugar estable. " +
                 "Sin embargo, todo comienza a sentirse vacío cuando sueños extraños y amenazas de lobos traidores emergen alrededor de Caswell. Mientras intenta descubrir quién es el traidor y encontrar su propio camino, Robbie deberá enfrentarse a dudas profundas y a peligros inesperados en su manada y en su corazón.",
                 "T.J. Klune", "Fantasía paranormal / Romance", "8.3", "9788412622409")};
                Books= new ObservableCollection<Book>(AllBooks);
            SearchCommand = new Command(() =>
            {
                Books.Clear();
                //Si es blanco añadimos todos los libros a la lista
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    foreach(var book in AllBooks)
                        Books.Add(book);
                    return;
                }
                //Si no es blanco añadimos solo los que contengan el nombre buscado
                //filtramos todos los libros que contengan la palabra buscada
                var filtered = AllBooks
                .Where(b => b.Title.ToLower().Contains(SearchText.ToLower()));
                //Añadimos al array de los libros actuales
                foreach (var book in filtered)
                    Books.Add(book);

            });
            //recibe un libro como parámetro
            SeeMoreCommand = new Command<Book>(async (Book book) =>
            {
                //Navegamos a la vista del perfil del libro, tenemos que hacerlo desde aqui para que sea mas sencillo
                var perfilLibroView = new Views.PerfilLibroView(user, book);
                await Application.Current.MainPage.Navigation.PushAsync(perfilLibroView);
            });

        }
    }
}
