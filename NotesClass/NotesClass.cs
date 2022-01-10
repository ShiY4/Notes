using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Windows;
namespace NotesClass
{
    public class NotesClass
    {
       
        Dictionary<string, FileStream> notes = new Dictionary<string, FileStream>();  
       public NotesClass ()
        {
            string[] titles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.rtf");
            for (int i=0; i<titles.Length; i++)
            {
                FileStream fs = new FileStream(titles[i], FileMode.Open);
                notes.Add(titles[i], fs);
            }
                       
        }
        public Dictionary <string, FileStream> get_notes()
        {
            return notes;
        }
        public NotesClass(Dictionary<string, FileStream> a_notes)
        {
            notes = a_notes;
        }

        public FileStream get_file(string title)
        {
            return notes[Directory.GetCurrentDirectory() + "\\" + title + ".rtf"];
        }

      

        public void remove_file(string title)
        {
            if (File.Exists(title + ".rtf"))
            {
                string full_name = Directory.GetCurrentDirectory() + "\\" + title + ".rtf";
                notes[full_name].Close();
                File.Delete(full_name);
                notes.Remove(full_name);
            }
            else throw  (new FileNotFoundException());
        }

        public void add_file(string title, TextRange doc)
        {
            FileStream fs = new FileStream($"{title}.rtf", FileMode.CreateNew);
            doc.Save(fs, DataFormats.Rtf);
            notes.Add(Directory.GetCurrentDirectory() + "\\" + title + ".rtf", fs);
        }


        public List<string> get_titles ()
        {
            List<string> titles = new List<string> ();
            foreach (KeyValuePair<string, FileStream> note in notes)
            {
                string corrected_title = Path.GetFileNameWithoutExtension(note.Key);
                titles.Add(corrected_title);
            }
            return titles;
        }

      
    }
}
