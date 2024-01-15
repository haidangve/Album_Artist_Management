using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Windows.Controls.Primitives;

namespace _255FP_Nguyen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //List to store artists and album
        private List<Album> albumList;
        private List<Artist> artistList;

        //Set the connection source to the database
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='D:\SaskPoly\COMP255\Final\255FP-Nguyen\ArtistAlbum.mdf';Integrated Security=True";

        //create new instance of album and artist
        Album currentAlbum = new Album();
        Artist currentArtist = new Artist();

        //Index variable for selected album and artist
        int currentSelectedAlbumIndex = -1;
        int currentSelectedArtistIndex = -1;

        public MainWindow()
        {
            InitializeComponent();

            //Initialize lists
            artistList = new List<Artist>();
            albumList = new List<Album>();

            //Call for method to load the artists in the listbox
            LoadArtists();
        }

        //-----------------------------------------
        //METHOD TO LOAD THE ARTISTS IN THE LISTBOX
        public void LoadArtists()
        {
            //Clear the listbox and the artist list
            lsbArtistsListBox.Items.Clear();
            artistList = new List<Artist>();

            //Connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Set the address of the database to connect to
                connection.ConnectionString = connectionString;

                //Open connection
                connection.Open();

                //SQL command to select all records from the Artists table
                string sqlStatement = $"SELECT * FROM Artists";

                // Creating an object to send our statement with
                SqlCommand selectStatement = new SqlCommand(sqlStatement, connection);

                //Read and add artists to the list and list box
                using (SqlDataReader reader = selectStatement.ExecuteReader())
                {
                    //loop through the records
                    while (reader.Read())
                    {
                        Artist newArtist = new Artist(
                            (int)reader["ArtistID"],
                            (string)reader["ArtistName"],
                            (int)reader["BillboardRank"]
                            );

                        artistList.Add(newArtist);
                        lsbArtistsListBox.Items.Add(newArtist);
                    }
                }
                //close connection
                connection.Close();

                //Set the selected index in the listbox to the current artist
                if (currentSelectedArtistIndex >= 0 && currentSelectedArtistIndex < lsbArtistsListBox.Items.Count)
                {
                    lsbArtistsListBox.SelectedIndex = currentSelectedArtistIndex;
                }
            }
        }

        //------------------------------------------
        //Event handler for the selection change in the artist listbox
        private void lsbArtistsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Update the current artist and index
            currentArtist = (Artist)lsbArtistsListBox.SelectedItem;
            currentSelectedArtistIndex = lsbArtistsListBox.SelectedIndex;

            //Call for method to display the artist data in textboxes
            DisplayArtist();

            //check if there's any existing artist
            if (currentArtist != null)
            {
                //if yes, call for method to load the albums of that artist in the album listbox
                //parse in the currently selected artistID to match with the album records
                LoadAlbums(currentArtist.ArtistID);
            }
        }

        //--------------------------------------------
        //METHOD TO LOAD THE ALBUMS IN THE LISTBOX
        //int artistID: currently selected artistID
        public void LoadAlbums(int artistID)
        {
            //Clear existing items in the Albums listbox and list
            lsbAlbumsListBox.Items.Clear();
            albumList = new List<Album>();

            //connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //open connection
                connection.Open();

                //SQL statement to select albums from the selected artist, using matching artistID
                string sqlStatement = $"SELECT * FROM Albums WHERE ArtistID = '{artistID}'";

                // Creating an object to send our statement with
                SqlCommand selectStatement = new SqlCommand(sqlStatement, connection);

                // Read and add albums to the list and listbox
                using (SqlDataReader reader = selectStatement.ExecuteReader())
                {
                    // Loop through the records
                    while (reader.Read())
                    {
                        Album newAlbum = new Album(
                            (int)reader["AlbumID"],
                            (int)reader["ArtistID"],
                            (string)reader["AlbumTitle"],
                            (string)reader["Genre"],
                            (DateTime)reader["ReleaseDate"]
                            );
                        albumList.Add(newAlbum);
                        lsbAlbumsListBox.Items.Add(newAlbum);
                    }
                }
                //close connectiong
                connection.Close();

                //Set the selected index in the listbox to the current artist
                if (currentSelectedAlbumIndex >= 0 && currentSelectedAlbumIndex < lsbAlbumsListBox.Items.Count)
                {
                    lsbAlbumsListBox.SelectedIndex = currentSelectedAlbumIndex;
                }
            }
        }

        //---------------------------------------
        //Event handler for the selection change in the album listbox
        private void lsbAlbumsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Update the current album and index
            currentAlbum = (Album)lsbAlbumsListBox.SelectedItem;
            currentSelectedAlbumIndex = lsbAlbumsListBox.SelectedIndex;

            //Call for method the display the album data in textboxes
            DisplayAlbum();

        }

        //------------------------------------------
        //METHOD TO DISPLAY THE ALBUM DATA IN TEXTBOXES
        public void DisplayAlbum()
        {
            //clear the textboxes if no album is selected
            if (currentSelectedAlbumIndex == -1)
            {
                txtAlbumID.Clear();
                txtAlbumTitle.Clear();
                txtGenre.Clear();
                txtReleaseDate.Clear();
            }
            else
            {
                //Display the album details in textboxes
                txtAlbumID.Text = currentAlbum.AlbumID.ToString();
                txtAlbumTitle.Text = currentAlbum.AlbumTitle;
                txtGenre.Text = currentAlbum.Genre;
                txtReleaseDate.Text = currentAlbum.ReleaseDate.ToShortDateString();
            }
        }

        //----------------------------------------
        //METHOD TO DISPLAY THE ARTIST DATA IN TEXTBOXES
        public void DisplayArtist()
        {
            //Clear the textboxes if no artist is selected
            if (currentSelectedArtistIndex == -1)
            {
                txtArtistID.Clear();
                txtArtistName.Clear();
                txtBillboardRank.Clear();
            }
            else
            {
                //Display the artist details in textboxes
                txtArtistID.Text = currentArtist.ArtistID.ToString();
                txtArtistName.Text = currentArtist.ArtistName;
                txtBillboardRank.Text = currentArtist.BillboardRank.ToString();
            }
        }

        //--------------------------------------
        //Event handler for save artist button
        private void btnSaveArtist_Click(object sender, RoutedEventArgs e)
        {
            //check if the data in textboxes is valid or not(blank)
            if (IsArtistDataValid())
            {
                //Call for method to save the current artist details
                StoreCurrentArtist();

                //Connect to database
                using (SqlConnection connection = new SqlConnection())
                {
                    //Set the address of the database to connect to
                    connection.ConnectionString = connectionString;

                    //open connection
                    connection.Open();

                    //SQL statement to update artist details
                    string sql = $"UPDATE Artists SET " +
                        $"ArtistName = '{currentArtist.ArtistName}', " +
                        $"BillboardRank = '{currentArtist.BillboardRank}' " +
                        $"WHERE ArtistID = {currentArtist.ArtistID}";

                    //Create a command to send to sql database
                    using (SqlCommand UpdateCommand = new SqlCommand(sql, connection))
                    {
                        //Execute update statement
                        UpdateCommand.ExecuteNonQuery();
                    }
                    //close connection
                    connection.Close();
                }
                //Call for method to load artist in listbox
                LoadArtists();

                //Set the selected artist index to the current artist index
                currentSelectedArtistIndex = lsbArtistsListBox.Items.Count - 1;
                lsbArtistsListBox.SelectedIndex = currentSelectedArtistIndex;
                // Display the details of the newly saved artist
                DisplayArtist();
            }
        }

        //-----------------------------------------
        //Event handler for delete artist button
        private void btnDeleteArtist_Click(object sender, RoutedEventArgs e)
        {
            //connect to database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //open connection
                connection.Open();

                // Write SQL command to delete all albums for the artist
                string deleteAlbumsSql = $"DELETE FROM Albums WHERE ArtistID = {currentArtist.ArtistID}";

                // Execute delete command for albums
                using (SqlCommand deleteAlbumsCommand = new SqlCommand(deleteAlbumsSql, connection))
                {
                    deleteAlbumsCommand.ExecuteNonQuery();
                }

                // Write SQL command to delete the artist
                string deleteArtistSql = $"DELETE FROM Artists WHERE ArtistID = {currentArtist.ArtistID}";

                // Execute delete command for the artist
                using (SqlCommand deleteArtistCommand = new SqlCommand(deleteArtistSql, connection))
                {
                    deleteArtistCommand.ExecuteNonQuery();
                }

                //close connection
                connection.Close();
            }
            //Call for method to load the artist in listbox
            LoadArtists();

            //Set the selected artist index to the current artist index
            currentSelectedArtistIndex = lsbArtistsListBox.Items.Count - 1;
            lsbArtistsListBox.SelectedIndex = currentSelectedArtistIndex;
            // Display the details of the artist selected(the one before the deleted artist)
            DisplayArtist();
        }

        //---------------------------------------
        //Event handler for Save new artist button
        private void btnSaveNewArtist_Click(object sender, RoutedEventArgs e)
        {
            //check if the current artist data is valid or not
            if (IsArtistDataValid())
            {
                //Call for method to save the current artist details
                StoreCurrentArtist();

                //connect to database
                using (SqlConnection connection = new SqlConnection())
                {
                    //Setting the address of the database to connect to
                    connection.ConnectionString = connectionString;

                    //Open connection
                    connection.Open();

                    //writing SQL statement to get the max artist ID from the databse
                    string sql = "SELECT MAX(ArtistID) FROM Artists;";
                    //new variables to hold the new artist ID
                    int newArtistID;

                    //Execute a command to send to SQL database
                    using (SqlCommand SelectCommand = new SqlCommand(sql, connection))
                    {
                        //Increment the current account number 
                        newArtistID = (int)SelectCommand.ExecuteScalar();
                        newArtistID++;
                    }

                    //Assign it to the new account number
                    currentArtist.ArtistID = newArtistID;

                    //SQL command to insert new artist
                    sql = "INSERT INTO Artists (ArtistID, ArtistName, BillboardRank) " +
                        "VALUES (" +
                        $"{currentArtist.ArtistID}, " +
                        $"'{currentArtist.ArtistName}', " +
                        $"'{currentArtist.BillboardRank}');";

                    //execute sql command to insert artist
                    using (SqlCommand InsertCommand = new SqlCommand(sql, connection))
                    {
                        InsertCommand.ExecuteNonQuery();
                    }
                    //close connection 
                    connection.Close();
                }
                //Call for method to load the artist in listbox
                LoadArtists();

                //Set the selected artist index to the current artist index
                currentSelectedArtistIndex = lsbArtistsListBox.Items.Count - 1;
                lsbArtistsListBox.SelectedIndex = currentSelectedArtistIndex;
                // Display the details of the newly added artist
                DisplayArtist();
            }
        }

        //-------------------------------------------
        //Event handler for save album button
        private void btnSaveAlbum_Click(object sender, RoutedEventArgs e)
        {
            //check if the current album data is valid or not
            if (IsAlbumDataValid())
            {
                //Call for method to save the current album details
                StoreCurrentAlbum();

                //Connect to the database
                using (SqlConnection connection = new SqlConnection())
                {
                    // Setting the address of the database to connect to
                    connection.ConnectionString = connectionString;

                    //open connection
                    connection.Open();

                    //SQL Statement to update album details
                    string sql = $"UPDATE Albums SET " +
                        $"AlbumTitle = '{currentAlbum.AlbumTitle}', " +
                        $"Genre = '{currentAlbum.Genre}', " +
                        $"ReleaseDate = '{currentAlbum.ReleaseDate.ToString("yyyy-MM-dd")}' " +
                        $"WHERE AlbumID = {currentAlbum.AlbumID} AND ArtistID = {currentAlbum.ArtistID}";

                    //Create a command to send to SQL database
                    using (SqlCommand updateCommand = new SqlCommand(sql, connection))
                    {
                        //Execute the command
                        updateCommand.ExecuteNonQuery();
                    }
                    //close connection
                    connection.Close();
                }
                //Call for method to load the albums into listbox
                LoadAlbums(currentAlbum.ArtistID);

                //Set the selected index to the current selected album index
                currentSelectedAlbumIndex = lsbAlbumsListBox.Items.Count - 1;
                lsbAlbumsListBox.SelectedIndex = currentSelectedAlbumIndex;
                //Display album details in textboxes
                DisplayAlbum();
            }
        }

        //-------------------------------
        //Event handler for delete album button
        private void btnDeleteAlbum_Click(object sender, RoutedEventArgs e)
        {
            //Connect to the database
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();

                //Write new SQL command to delete the account from the database
                string sql = $"DELETE FROM Albums WHERE AlbumID = {currentAlbum.AlbumID} AND ArtistID = {currentAlbum.ArtistID}";

                //execute delete command
                using (SqlCommand DeleteCommand = new SqlCommand(sql, connection))
                {
                    DeleteCommand.ExecuteNonQuery();
                }
                //close connection
                connection.Close();
            }
            //Call for method to load the albums into listbox
            LoadAlbums(currentArtist.ArtistID);

            //Set the selected index to the current selected album( the one defore the deleted album)
            currentSelectedAlbumIndex = lsbAlbumsListBox.Items.Count - 1;
            lsbAlbumsListBox.SelectedIndex = currentSelectedAlbumIndex;
            //Display the album details in textboxes
            DisplayAlbum();
        }

        //-------------------------------------------
        //Event handler for the save new album button
        private void btnSaveNewAlbum_Click(object sender, RoutedEventArgs e)
        {
            //Check if the album data is valid or not
            if (IsAlbumDataValid())
            {
                //Call for method to save the current album details
                StoreCurrentAlbum();

                //connect to the database
                using (SqlConnection connection = new SqlConnection())
                {
                    // Setting the address of the database to connect to
                    connection.ConnectionString = connectionString;

                    //Open connection
                    connection.Open();

                    //writing SQL statement to get the max account number from the databse
                    string sql = "SELECT MAX(AlbumID) FROM Albums;";
                    //new variables to hold the new account number
                    int newAlbumID;

                    //Execute a command to send to SQL database
                    using (SqlCommand SelectCommand = new SqlCommand(sql, connection))
                    {
                        //Increment the new albumID
                        newAlbumID = (int)SelectCommand.ExecuteScalar();
                        newAlbumID++;
                    }

                    //Assign it to the current albumID to be created
                    currentAlbum.AlbumID = newAlbumID;

                    //SQL Statement to insert new album
                    sql = "INSERT INTO Albums (AlbumID, ArtistID, AlbumTitle, Genre, ReleaseDate) " +
                            "VALUES (" +
                            $"{currentAlbum.AlbumID}, " +
                            $"{currentAlbum.ArtistID}, " +
                            $"'{currentAlbum.AlbumTitle}', " +
                            $"'{currentAlbum.Genre}', " +
                            $"'{currentAlbum.ReleaseDate.ToShortDateString()}');";

                    //execute sql command to insert
                    using (SqlCommand InsertCommand = new SqlCommand(sql, connection))
                    {
                        InsertCommand.ExecuteNonQuery();
                    }
                    //close connection 
                    connection.Close();
                }
                //Call for method to load albums into listbox
                LoadAlbums(currentArtist.ArtistID);

                //Set the index to the newly added album
                currentSelectedAlbumIndex = lsbAlbumsListBox.Items.Count - 1;
                lsbAlbumsListBox.SelectedIndex = currentSelectedAlbumIndex;

                //Display album details in textboxes
                DisplayAlbum();
            }
        }

        //-------------------------------
        //Method to check if the artist data is valid or not
        //Rule: ArtistName cannot be blank
        private bool IsArtistDataValid()
        {
            //if artist name is blank, show message as lable
            if (string.IsNullOrWhiteSpace(txtArtistName.Text))
            {
                lblArtistError.Content = "Artist Name cannot be blank.";
                return false;
            }

            // Clear any previous error messages
            lblArtistError.Content = "";

            return true;
        }

        //-------------------------------
        //Method to check if the album data is valid or not
        //Rule: AlbumTitle, ReleaseDate and Genre cannot be blank
        private bool IsAlbumDataValid()
        {
            //if AlbumTitle is blank, show message as lable
            if (string.IsNullOrWhiteSpace(txtAlbumTitle.Text))
            {
                lblAlbumError.Content = "Album Name cannot be blank.";
                return false;
            }

            //if ReleaseDate is blank, show message as lable
            if (string.IsNullOrWhiteSpace(txtReleaseDate.Text))
            {
                lblAlbumError.Content = "Release Date cannot be blank.";
                return false;
            }

            //if Genre is blank, show message as lable
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            {
                lblAlbumError.Content = "Genre cannot be blank.";
                return false;
            }

            // Clear any previous error messages
            lblAlbumError.Content = "";

            return true;
        }
        
        //----------------------------------
        //Method to save the current artist details
        public void StoreCurrentArtist()
        {
            //store ArtistID if not blank
            if (!string.IsNullOrEmpty(txtArtistID.Text))
            {
                //set new variable for artist ID
                int itsArtistID = 0;

                if (int.TryParse(txtArtistID.Text, out itsArtistID))
                {
                    currentArtist.ArtistID = itsArtistID;
                }
                else
                {
                    currentArtist.ArtistID = 0;
                }
            }
            //store artist name
            currentArtist.ArtistName = txtArtistName.Text;

            //store billboard rank(null value accepted)
            if (int.TryParse(txtBillboardRank.Text, out int billboardRank))
            {
                currentArtist.BillboardRank = billboardRank;
            }
            else
            {
                // Set BillboardRank to null if parsing fails
                currentArtist.BillboardRank = null;
            }
        }

        //-------------------------------------
        //Method to store current album details
        public void StoreCurrentAlbum()
        {
            //store albumID if not blank
            if (!string.IsNullOrEmpty(txtAlbumID.Text))
            {
                //set new variable for artist ID
                int itsAlbumID = 0;

                if (int.TryParse(txtAlbumID.Text, out itsAlbumID))
                {
                    currentAlbum.AlbumID = itsAlbumID;
                }
                else
                {
                    currentAlbum.AlbumID = 0;
                }
            }

            //store ArtistID
            if (currentArtist != null)
            {
                //store ArtistID
                currentAlbum.ArtistID = currentArtist.ArtistID;
            }
            else
            {
                //In case artist ID is null
                currentAlbum.ArtistID = 0; 
            }

            //store album title
            currentAlbum.AlbumTitle = txtAlbumTitle.Text;

            //store genre
            currentAlbum.Genre = txtGenre.Text;

            //store release date
            if (DateTime.TryParse(txtReleaseDate.Text, out DateTime parsedReleaseDate))
            {
                currentAlbum.ReleaseDate = parsedReleaseDate;
            }
            else
            {
                // Set ReleaseDate to DateTime.MinValue if parsing fails
                currentAlbum.ReleaseDate = DateTime.MinValue;
            }
        }
    }
}