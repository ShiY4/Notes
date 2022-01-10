using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using NotesClass;
//using System.Windows.Forms;
//using DataFormats = System.Windows.DataFormats;

namespace Notes
{
    /// <summary>
    /// Interaction logic for CreationWindow.xaml
    /// </summary>
    public partial class CreationWindow : Window
    {
        NotesClass.NotesClass n;
             
        private bool isChanged; /*Сигнализирует изменен ли был текст в окошке*/
        private string title; /*Переменная которая возвращает заголовка записки*/
        public CreationWindow(ref NotesClass.NotesClass obj) /**/
        {
            InitializeComponent();
            isChanged = false; /*Пока текст не менялся поэтому false*/
            n = obj;
            NoteEditor.TextChanged += NoteEditor_TextChanged; /*Добавляем обработчик событий, который реагирует на изменения. Отслеживание поля текста*/
        }

        private void NoteEditor_TextChanged(object sender, TextChangedEventArgs e) /*Сам обработчик события*/
        {
            isChanged = true; /*если произошли изменения, то  true*/
        }
        public bool isChanged_f()/*геттер для переменной, который позволяет получить значения*/
        {
            return isChanged;
        }

        public CreationWindow(string title, ref FileStream fs, ref NotesClass.NotesClass obj) //Повторное открывание записки, заголовок и данные передаются
         {
             InitializeComponent();
             this.set_view(title, ref fs); 
             isChanged = false;
             NoteEditor.TextChanged += NoteEditor_TextChanged;
             n = obj;

        }

    public event EventHandler CreationWindowClosed; /*Сигнал события, который реагирует на событие в окошке и в xaml свойство closing запускает*/

        private void CreationWIndow_Closed(object sender, System.ComponentModel.CancelEventArgs e) /*Функция, которая реагирует на закрытие окна*/
        {
            //string title;
            if (!Note_Title.Text.Equals("")) { title = Note_Title.Text; } /*Рассматривает случай, если поле заголовка не пустое, то переменные строке заголовка присваеваем данные, которые есть в этом поле*/
            else
            {
                //  Считываю файлы текстового поля и присваиваются в заголовок
                TextRange tmp = new TextRange(NoteEditor.Document.ContentStart, NoteEditor.Document.ContentEnd); /*читаем данные textRange. tmp впитывает все, что было в текстовом поле*/
                title = new string(tmp.Text.Where(c => !char.IsControl(c)).ToArray()); /*Заголовку выделяется память с помощью запроса, ему присваевается значения текста, которое есть в доке*/
            }
            string cur_dir = Directory.GetCurrentDirectory(); /*Строковая переменная получает строковую директория*/



            if (this.isChanged_f() && File.Exists(cur_dir + "\\" + title + ".rtf")) /*Случай. Если текст был изменен, но файл уже существует с таким названием*/
            {
                /*Был ли существующий файл отредактирован*/

                TextRange range = new TextRange(NoteEditor.Document.ContentStart, NoteEditor.Document.ContentEnd); /*Забирает все данные в нашем текстовом поле*/
                n.remove_file(title);
                n.add_file(title, range);
            }
            else if (this.isChanged_f()) /*Если текст был изменени, но такого файла не существует*/
            {
                /*Все то же самое ток без удаления*/
                
                TextRange range = new TextRange(NoteEditor.Document.ContentStart, NoteEditor.Document.ContentEnd);
                n.add_file(title, range);

            }
            else if (File.Exists(cur_dir + "\\" + title + ".rtf")) /*Если файл изменен не был, но мы его посмотрели*/
            {

            }

            CreationWindowClosed(this, EventArgs.Empty); /*Выпускается сигнал, что окошко закрылось*/

        }

        public NotesClass.NotesClass get_tmp ()
        {
            return n;
        }


        //Когда открываем уже существующую записку то ее заголовок и данные передаются в эту самую формочку
        public string get_title() /*Просто возвращает значение заголовка*/
        {
            return title;
        }

        public void set_view(string title, ref FileStream fs) /*Тот самый сет, с помощью которого устанавливается значение*/
        {
            Note_Title.Text = title; /*Присваевается заголовок полю заголовка*/
            TextRange doc = new TextRange(NoteEditor.Document.ContentStart, NoteEditor.Document.ContentEnd);
            doc.Load(fs, DataFormats.Rtf); /*Присваевается содержимое файла*/           
        }

        private void Bold_Button_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleBold.Execute(null, NoteEditor);
        }

        private void Underlined_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleUnderline.Execute(null, NoteEditor);
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
            EditingCommands.ToggleItalic.Execute(null, NoteEditor);
        }
    }
}

       