using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _255FP_Nguyen
{
    internal class Album
    {
        //new properties + getters and setters
        public int AlbumID {  get; set; }
        public int ArtistID {  get; set; }
        public string AlbumTitle {  get; set; }
        public string Genre {  get; set; }
        public DateTime ReleaseDate { get; set; }

        //constructors
        public Album()
        {

        }

        public Album(int AlbumID, int ArtistID, string AlbumTitle, string Genre, DateTime ReleaseDate)
        {
            this.AlbumID = AlbumID;
            this.ArtistID = ArtistID;
            this.AlbumTitle = AlbumTitle;
            this.Genre = Genre;
            this.ReleaseDate = ReleaseDate;
        }

        //override method format the albums details which is displayed in listbox
        public override string ToString()
        {
            return $"{ArtistID, -12}{AlbumID, -11}{AlbumTitle, -34}{Genre, -20}{ReleaseDate.ToShortDateString()}";
        }

        //override method GetHashCode to provide a custom hash code based on the AlbumID property.
        public override int GetHashCode()
        {
            return AlbumID.GetHashCode() ;
        }
    }
}
