using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _255FP_Nguyen
{
    internal class Artist
    {
        //properties + getters and setters
        public int ArtistID {  get; set; }
        public string ArtistName { get; set; }
        public Nullable<int> BillboardRank {  get; set; }//Nullable <int> indicates that BillboardRank can be left null

        //constructors
        public Artist()
        {

        }

        public Artist(int ArtistID, string ArtistName, Nullable<int> BillboardRank)
        {
            this.ArtistID = ArtistID;
            this.ArtistName = ArtistName;
            this.BillboardRank = BillboardRank;
        }

        //override method format the artists details which is displayed in listbox
        public override string ToString()
        {
            return $"{ArtistID, -20}{ArtistName, -57}{BillboardRank}";
        }

        //override method GetHashCode to provide a custom hash code based on the ArtistID property.
        public override int GetHashCode()
        {
            return ArtistID.GetHashCode();
        }
    }
}
