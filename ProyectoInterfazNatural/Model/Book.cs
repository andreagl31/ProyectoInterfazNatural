using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInterfazNatural.Model
{
    [AddINotifyPropertyChangedInterface]
    public class Book
    {
        public int ID { get; set; }
        public string ImageSource { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Gender { get; set; }
        public string Ranking { get; set; }
        public string IBSN { get; set; }
        public Book(int id, string imageSource, string title, string description, string author, string genere, string ranking, string ibsn)
        {
            ID = id;
            ImageSource = imageSource;
            Title = title;
            Description = description;
            Author = author;
            Gender = genere;
            Ranking = ranking;
            IBSN = ibsn;
        }

    }
}
