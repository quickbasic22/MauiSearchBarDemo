using System.Reflection;
using Windows.System;

namespace MauiSearchBarDemo;

public partial class MainPage : ContentPage
{
    const double MaxMatches = 5500;
    string bookText;
    private bool myisbusy;
    public bool MyIsBusy
    {
        get => myisbusy;
        set
        {
            myisbusy = value;
            OnPropertyChanged("MyIsBusy");
        }
    }
    public MainPage()
    {
        InitializeComponent();
        MyIsBusy = false;
            using (StreamReader reader = File.OpenText(@"C:\Users\quick\source\repos\MauiSearchBarDemo\MauiSearchBarDemo\Resources\Raw\mobydick.txt"))
        {
            bookText = reader.ReadToEnd();
        }
        BindingContext = this;
        
    }

    private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        resultsStack.Children.Clear();
    }

    private void searchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        resultsScroll.Content = null;
        resultsStack.Children.Clear();
        
        MyIsBusy = true;
        
        SearchBookForText(searchBar.Text);
        
        MyIsBusy = false;
        
        resultsScroll.Content = resultsStack;

    }

    private void SearchBookForText(string searchText)
    {
        int count = 0;
        bool isTruncated = false; 

        using (StringReader reader = new StringReader(bookText))
        {
            int lineNumber = 0;
            string line;
           
            while (null != (line = reader.ReadLine()))
            {
                lineNumber++;
                int index = 0;

                while (-1 != (index = (line.IndexOf(searchText, index, StringComparison.OrdinalIgnoreCase))))
                {
                    if (count == MaxMatches)
                    {
                        isTruncated = true;
                        break;
                    }
                    index += 1;

                    resultsStack.Children.Add(new Label
                    {
                        Text = String.Format("Found at line {0}, offset {1}", lineNumber, index)
                    });
                    count++;
                    
                }
                if (isTruncated)
                {
                    break;
                }
            }
        }
        
        resultsStack.Children.Add(new Label
        {
            Text = String.Format("{0} match{1} found{2}", count, count == 1 ? "" : "es", isTruncated ? " - stopped" : "")
        });
    }
}

