using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;
using Newtonsoft.Json;

namespace quoteAPPAbaxter
{
    //Programmer Aaron Baxter
    //QuoteApp
    //Quote application. Saves Quotes to a Json file.
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        int int_random = 0;
        // hello there
        string filename = "zavefiles.txt";
        string loadedContent = "";
        string unconvertedText = "";

        IFolder folder = FileSystem.Current.LocalStorage;

        public Quotes[] arr_QuotesCopy = new Quotes[20];


        public MainPage()
        {
            InitializeComponent();

            Creating();

            Author.Text = loadedContent;

            MainBackground.BackgroundColor = Color.FromHex("EDEDED");


        }

        private async void Creating() // Checks to see if the file exists if not it creates a blank file.
        {
            ExistenceCheckResult folderExists = await folder.CheckExistsAsync(filename);

            if (folderExists != ExistenceCheckResult.FileExists)
            {
                loadedContent = "";

                IFile file = await folder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                await file.WriteAllTextAsync(loadedContent);
            }
            else
            {
                loadJson();
            }
        }

        private async void SaveJson() // Copies the items in the Array (arr_QuotesCopy) to the file.
        {

            string Jsondata = JsonConvert.SerializeObject(arr_QuotesCopy);

            IFile file = await folder.GetFileAsync(filename);
            await file.WriteAllTextAsync(Jsondata);

        }

        private async void loadJson() //Loads the File and deserializes it and put it in the array (arr_QuotesCopy)
        {

            IFile file = await folder.GetFileAsync(filename);

            unconvertedText = await file.ReadAllTextAsync();

            Quotes[] arr_Quotes = JsonConvert.DeserializeObject<Quotes[]>(unconvertedText);

            try
            {
                arr_Quotes.CopyTo(arr_QuotesCopy, 0);
            }
            catch
            {
            }

        }

        private void btn_favs_Clicked(object sender, EventArgs e)//It makes the Quote thats displayed a Favourite or not
        {

            if (btn_fav.Text == "STAR")
            {
                btn_fav.Text = "NONE";
                arr_QuotesCopy[int_random].favourite = false;
            }
            else
            {
                btn_fav.Text = "STAR";
                arr_QuotesCopy[int_random].favourite = true;
            }
            SaveJson();
        }

        private async void btn_favpage_Clicked(object sender, EventArgs e)//It displays all the quotes that have been favourited
        {
            loadJson();

            Random random_generator = new Random(DateTime.Now.Millisecond);



            string thes = "";
            int counter = 0;

            for (int x = 0; x < 20; x++)
            {
                try
                {
                    if (arr_QuotesCopy[x].favourite)
                    {
                        thes = thes + arr_QuotesCopy[x].quote + "\n" + arr_QuotesCopy[x].author + "\n \n";
                        counter++;
                        if (counter > 9)
                        {
                            break;
                        }
                    }

                }
                catch
                {

                }

            }

            await Navigation.PushModalAsync(new FavPage(thes));


        }

        private void Random(object sender, EventArgs args)//chooses a random Quote and displays it
        {
            Random random_generator = new Random(DateTime.Now.Millisecond);

            int_random = random_generator.Next(0, 19);

            if (arr_QuotesCopy[0] == null)
            {
                DisplayAlert("Error", "No Quotes to display", "OK :(");
            }
            else
            {
                while (true)
                {
                    if (arr_QuotesCopy[int_random] == null)
                    {
                        int_random = random_generator.Next(0, 19);
                    }
                    else
                    {
                        Author.Text = arr_QuotesCopy[int_random].author;
                        Quote.Text = arr_QuotesCopy[int_random].quote;

                        if (arr_QuotesCopy[int_random].favourite)
                        {
                            btn_fav.Text = "STAR";
                        }
                        else
                        {
                            btn_fav.Text = "NONE";
                        }


                        break;
                    }
                }
            }

        }

        private void Add(object sender, EventArgs args)// adds a quote to the array and saves it in the file.
        {


            try
            {
                if (Input_Author.Text != "" || Input_Author.Text != "")
                {
                    for (int x = 0; x < 20; x++)
                    {
                        if (arr_QuotesCopy[x] == null)
                        {
                            if (Input_Author.Text != null || Input_Quote.Text != null)
                            {
                                arr_QuotesCopy[x] = new Quotes();

                                arr_QuotesCopy[x].author = Input_Author.Text;
                                arr_QuotesCopy[x].quote = Input_Quote.Text;

                                SaveJson();

                                break;
                            }


                        }
                        else if (x == 19)
                        {
                            arr_QuotesCopy[0] = new Quotes();

                            arr_QuotesCopy[0].author = Input_Author.Text;
                            arr_QuotesCopy[0].quote = Input_Quote.Text;

                            SaveJson();

                            break;
                        }
                    }

                    Input_Quote.Text = "";
                    Input_Author.Text = "";
                }
            }
            catch
            {

            }


        }

        private void Next(object sender, EventArgs args)//Scrolls through the array
        {
            try
            {
                try
                {
                    int_random++;

                    Author.Text = arr_QuotesCopy[int_random].author;
                    Quote.Text = arr_QuotesCopy[int_random].quote;

                    if (arr_QuotesCopy[int_random].favourite)
                    {
                        btn_fav.Text = "STAR";
                    }
                    else
                    {
                        btn_fav.Text = "NONE";
                    }


                }
                catch
                {
                    int_random = 0;

                    Author.Text = arr_QuotesCopy[int_random].author;
                    Quote.Text = arr_QuotesCopy[int_random].quote;

                    if (arr_QuotesCopy[int_random].favourite)
                    {
                        btn_fav.Text = "STAR";
                    }
                    else
                    {
                        btn_fav.Text = "NONE";
                    }

                }
            }
            catch
            {
                DisplayAlert("Error", "No Quotes to display", "OK :(");
            }


        }

        private void Previous(object sender, EventArgs args)//Scrolls through the array
        {
            int counter = 0;

            try
            {
                int_random--;

                if (int_random < 0)
                {
                    int_random = 20;
                }

                while (true)
                {
                    counter++;

                    try
                    {
                        if(counter == 21)
                        {
                            DisplayAlert("Error", "No Quotes to display", "OK :(");
                            break;
                        }


                        Author.Text = arr_QuotesCopy[int_random].author;
                        Quote.Text = arr_QuotesCopy[int_random].quote;

                        if (arr_QuotesCopy[int_random].favourite)
                        {
                            btn_fav.Text = "STAR";
                        }
                        else
                        {
                            btn_fav.Text = "NONE";
                        }
                        break;
                    }
                    catch
                    {
                        int_random--;
                    }
                }
            }
            catch
            {
                DisplayAlert("Error", "No Quotes to display", "OK :(");
            }


        }

        private void Delete(object sender, EventArgs e)//Deletes the Quote thats displayed
        {

            try
            {
                List<Quotes> the_lst = new List<Quotes>(arr_QuotesCopy);

                the_lst.RemoveAt(int_random);

                Array.Clear(arr_QuotesCopy, 0, arr_QuotesCopy.Length);

                arr_QuotesCopy = the_lst.ToArray();

                Quote.Text = "";
                Author.Text = "";

                SaveJson();
            }
            catch
            {

            }
        }

    }
}
