using System;
using System.Collections.Generic;
using System.IO;
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
using System.Globalization;
using NotesClass;

namespace Notes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NotesClass.NotesClass notes;
        string language;
        public MainWindow()
        {
            InitializeComponent();
            language = "rus-RU";
            notes = new NotesClass.NotesClass();
            List<string> titles = notes.get_titles();
            foreach (string t in titles)
            {
                NotesList.Items.Add(t);
            }

        }
    
        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            CreationWindow creationWindow = new CreationWindow(ref notes); /*Создали окошко*/
            creationWindow.Owner = this; 
            creationWindow.CreationWindowClosed += CreationWindowClosed_React; 
            creationWindow.Show();
        }

        private void CreationWindowClosed_React(object sender, EventArgs e) /*После закрытия окошка создания записки*/
        {
           if (!NotesList.Items.Contains(((CreationWindow)sender).get_title())) /*получили заголовок*/
            { NotesList.Items.Add(((CreationWindow)sender).get_title());
                notes = ((CreationWindow)sender).get_tmp();  } /*И добавили его в листбокс, если такие данные уже содержаться, то мы ничего не делаем*/
            else
            {
                notes = ((CreationWindow)sender).get_tmp();
            }
        }

        private void NotesList_ItemDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = NotesList.SelectedIndex; 
            string tit = (string) NotesList.Items[index];
            FileStream note_file = notes.get_file(tit);
            CreationWindow s_window = new CreationWindow(tit, ref note_file, ref notes);
            s_window.Owner = this;
            s_window.CreationWindowClosed += CreationWindowClosed_React;
            s_window.Show();

        }

        private void DelButton_Click(object sender, RoutedEventArgs e)
        {
            int index = NotesList.SelectedIndex;
            string tit = (string)NotesList.Items[index];
            NotesList.Items.RemoveAt(index);
            notes.remove_file(tit);
        }

        private void Lang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            language = (cb.SelectedItem as ComboBoxItem).Tag.ToString();

            if (language != null)
            {
                CultureInfo lang = new CultureInfo(language);

                if (lang != null)
                {
                    App.Language = lang;
                }

            }
        }
    }
}
