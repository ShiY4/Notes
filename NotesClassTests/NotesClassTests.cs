

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NotesClass;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Reflection;
//using System.Windows.Controls;

namespace NotesTests
{
    [TestClass]
    public class NotesTest
    {
        [TestMethod]
        public void ObjectInitialization()
        {
            NotesClass.NotesClass note = new NotesClass.NotesClass();
            Dictionary<string, FileStream> exp = new Dictionary<string, FileStream>();

            string[] titles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.rtf");
            for (int i = 0; i < titles.Length; i++)
            {

                string cur = Path.GetFileNameWithoutExtension(titles[i]);
                note.get_file(cur).Close();
                FileStream fs = new FileStream(titles[i], FileMode.Open);

                exp.Add(cur, fs);
                fs.Close();
            }
            List<FileStream> files = new List<FileStream>(exp.Values);
            List<string> tit1 = new List<string>(exp.Keys);
            //NotesClass.NotesClass expected_note = new NotesClass.NotesClass(exp);
            //  Assert.AreEqual(tit, note.get_titles());
            for (int i = 0; i < titles.Length; i++)
            {
                //  Assert.AreEqual(tit[i], (note.get_titles())[i]);
                Assert.AreEqual(tit1[i], (note.get_titles())[i]);
                Assert.AreEqual(files[i], (note.get_file(tit1[i])));
            }

        }


        /*   [TestMethod]
          public void Add()
          {
              //Assembly sample = Assembly.Load(".\PresentationFramework.dll");

               System.Type TextRange = sample.GetType("System.Documents.TextRange", true, true);
               System.Type FlowDocument = sample.GetType("System.Document.FlowDocument", true, true);

               object doc = System.Activator.CreateInstance(FlowDocument);
               object range = System.Activator.CreateInstance(TextRange);

               MethodInfo constructor = TextRange.GetMethod("TextRange");"

               NotesClass.NotesClass note = new NotesClass.NotesClass();
            FileStream fs = File.Create ("testFile.rtf");
                      FlowDocument doc = new FlowDocument();
                      TextRange range = new TextRange(doc.ContentStart, doc.ContentEnd);
                     range.Load(fs, DataFormats.Rtf);
                    fs.Close();


                    note.add_file("testFile", range);
                    List<string> tit1 = note.get_titles();
                    Assert.AreEqual("testFile.rtf", tit1[tit1.Count - 1]);
            //  Assembly sample = Assembly.ReflectionOnlyLoadFrom("C:\\Program Files\\dotnet\\packs\\Microsoft.WindowsDesktop.App.Ref\\3.1.0\\ref\\netcoreapp3.1\\PresentationFramework.dll");

          }*/

      [TestMethod]
      public void Remove_Existing_File()
      {
              FileStream fs = File.Create("testFile.rtf");
              fs.Close();
              NotesClass.NotesClass note = new NotesClass.NotesClass();
              note.remove_file("testFile");
              Assert.AreEqual(false, File.Exists("testFile.rtf"));
              Assert.IsFalse((note.get_titles()).Count>0);
          } 
      [TestMethod]
      public void Remove_NonExisting_File()
      {

          NotesClass.NotesClass note = new NotesClass.NotesClass();
              Assert.ThrowsException<FileNotFoundException>(() => note.remove_file("testFile")); 

      }
    }
        

    }


