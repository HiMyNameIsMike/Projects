using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace ConsoleApplication3
{
    class Note
    {

        // dodać id do klasy?
        public DateTime date { get; set; } // date related to note
        public string text { get; set; } // text of note
    }
    public class Program
    {
        public static void Main(string[] args)
        {

            // podzielić 
            var mth = DateTime.Now.Month;
            var yr = DateTime.Now.Year;
            new Calendar().CreateCalendar2(yr, mth);

            Console.ReadKey();
        }
    }
    class Calendar
    {
        List<Note> allNotes = new List<Note>();
        string constSeparator = "#@create2017xx";
        public void CreateCalendar(int chosenYear, int chosenMonth)
        {
            Console.Clear();
            var chosenMonthList = createCalendar(chosenYear, chosenMonth);
            var charNumber = 23;
            var pasteSpace = 0;
            foreach (string day in chosenMonthList)
            {
                var dayNum = day.Substring(0, 2);
                dayNum = dayNum.Trim();
                pasteSpace = charNumber - day.Count();
                Console.WriteLine(day + pasteSpaces(pasteSpace) + "||  liczba notatek: " + noteCount(allNotes, dayNum, chosenYear, chosenMonth));
            }
            menu("1", chosenYear, chosenMonth);
        }
        public static string pasteSpaces(int n)
        {
            return new String(' ', n);
        }

        public static string getDate()
        {
            var currentDate = DateTime.Now;
            return currentDate.ToString();
        }

        public static int getMonth(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        public static List<string> createCalendar(int year, int month)
        {
            var daysInMonth = getMonth(year, month);
            List<string> calendar = new List<string>();
            for (int i = 1; i <= daysInMonth; i++)
            {
                // string.Format
                var dateForCalendar = DateTime.Parse(year + "-" + month + "-" + i).ToString("dd/MM/yy, dddd");
                calendar.Add(dateForCalendar);
            }
            return calendar;
        }

        public void menu(string chosenCal, int chosenYear, int chosenMonth)
        {
            string choice = "";
            Console.WriteLine("Please select option");
            Console.WriteLine("1 - previous month");
            Console.WriteLine("2 - next month");
            Console.WriteLine("3 - add note");
            Console.WriteLine("4 - display all notes");
            Console.WriteLine("5 - switch view");
            Console.WriteLine("6 - delete note");
            Console.WriteLine("7 - close the application");
            Console.WriteLine("8 - edit note");
            choice = Console.ReadLine();

            //switch
            if (choice == "2")
            {
                chosenYear = handleMonth("add", chosenYear, chosenMonth)[0];
                chosenMonth = handleMonth("add", chosenYear, chosenMonth)[1];
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
            else if (choice == "1")
            {
                chosenYear = handleMonth("substract", chosenYear, chosenMonth)[0];
                chosenMonth = handleMonth("substract", chosenYear, chosenMonth)[1];
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);

            }
            else if (choice == "3")
            {
                Console.WriteLine("To which day of chosen month you want to add a note?");
                var chosenDay = Console.ReadLine();
                int chosenDayInt = 0;
                if (!Int32.TryParse(chosenDay, out chosenDayInt))
                {
                    Console.WriteLine("Wrong input of date format. Please inster just the day number, for instance 1 or 15. Press any key to return to main menu.");
                    Console.ReadKey();
                }
                else {
                    addNote(chosenDayInt, chosenYear, chosenMonth);
                }
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
            else if (choice == "4")
            {
                foreach (Note textNote in allNotes)
                {
                    Console.WriteLine("{0} , {1}", textNote.date, textNote.text);
                }
                Console.ReadKey();
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
            else if (choice == "5")
            {

                var newChosenCal = "";
                if (chosenCal == "1")
                {
                    newChosenCal = "2";
                }
                else
                {
                    newChosenCal = "1";
                }
                createCalChosen(newChosenCal, chosenYear, chosenMonth);
            }
            else if (choice == "6")
            {
                deleteNote();
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
            else if (choice == "7")
            {
                Environment.Exit(Environment.ExitCode);
            }

            else if (choice == "8")
            {
                editNote();
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
            else
            {
                Console.WriteLine("Wrong menu input. Please insert number from 1 to 8. Press any key to go back.");
                Console.ReadKey();
                Console.Clear();
                createCalChosen(chosenCal, chosenYear, chosenMonth);
            }
        }

        public static List<int> handleMonth(string toDo, int chosenYear, int chosenMonth)
        {
            List<int> results = new List<int>();
            if (toDo == "add")
            {
                if (chosenMonth == 12)
                {
                    chosenYear += 1;
                    chosenMonth = 1;
                }
                else
                {
                    chosenMonth += 1;
                }
            }
            else if (toDo == "substract")
            {
                if (chosenMonth == 1)
                {
                    chosenYear += -1;
                    chosenMonth = 12;
                }
                else
                {
                    chosenMonth += -1;
                }
            }
            results.Add(chosenYear);
            results.Add(chosenMonth);
            return results;
        }
        public void addNote(int day, int chosenYear, int chosenMonth)
        {
            var datesCount = getMonth(chosenYear, chosenMonth);
            if (day > datesCount)
            {
                Console.WriteLine("There is no such day in given month. Press any key to return to main menu");
                Console.ReadKey();
            }
            else {
                var date = DateTime.Parse(chosenYear.ToString() + "-" + chosenMonth.ToString() + "-" + day.ToString());
                Console.WriteLine("Please write the text for your note");
                var noteText = Console.ReadLine();
                Note note = new Note();
                note.date = date;
                note.text = noteText;
                allNotes.Add(note);
                saveNote(date, noteText);
                Console.WriteLine("Your note has been added! press any key to proceed");
                Console.ReadKey();
            }

        }

        public string noteCount(List<Note> list, string day, int chosenYear, int chosenMonth)
        {
            var dayInt = Int32.Parse(day);
            if (list.Any())
            {
                var dateChosen = new DateTime(chosenYear, chosenMonth, dayInt);
                var countOfNotes = list.Where(note => note.date == dateChosen).Count();

                return countOfNotes.ToString();
            }
            else return "0";
        }

        public void CreateAltCalendar(int chosenYear, int chosenMonth)
        {
            Console.Clear();
            var chosenMonthList = createCalendar(chosenYear, chosenMonth);
            var firstDayOfMonth = new DateTime(chosenYear, chosenMonth, 1);
            var firstDayOfMonthW = firstDayOfMonth.DayOfWeek.ToString();
            string[] daysOfWeek = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            var dayNo = 0;
            for (var i = 1; i < 8; i++)
            {
                if (daysOfWeek[i - 1] == firstDayOfMonthW.ToString())
                {
                    dayNo = i;
                }
                if (dayNo == 7)
                {
                    dayNo = 0;
                }
            }
            var stringCalendar = "  Sun Mo  Tue Wed Th  Fr  Sat " + Environment.NewLine;
            if (dayNo > 0)
            {
                stringCalendar = stringCalendar + new String(' ', dayNo * 4);
            }
            foreach (string day in chosenMonthList)
            {
                var dayNum = day.Substring(0, 2);
                dayNum = dayNum.Trim();
                var dayNumInt = Int32.Parse(dayNum);
                if (dayNumInt % 7 == (7 - dayNo) % 7)
                {
                    stringCalendar = stringCalendar + "  " + dayNum + Environment.NewLine;
                }
                else {
                    stringCalendar = stringCalendar + "  " + dayNum;
                }

            }
            var MTH = new DateTime(chosenYear, chosenMonth, 1);
            Console.WriteLine(MTH.ToString("MMMM/yyyy"));
            Console.WriteLine(stringCalendar);
            menu("2", chosenYear, chosenMonth);
        }

        public void createCalChosen(string chosenCal, int chosenYear, int chosenMonth)
        {
            if (chosenCal == "1")
            {
                CreateCalendar(chosenYear, chosenMonth);
            }
            else if (chosenCal == "2")
            {
                CreateAltCalendar(chosenYear, chosenMonth);
            }
        }
        public void saveNote(DateTime date, string text)
        {
            string fullNote = date.ToShortDateString() + constSeparator + text;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"Notes.txt", true))
                file.WriteLine(fullNote);
        }
        public void loadNotes()
        {
            List<string> notesLoaded = new List<string>();
            try
            {   // Open the text file using a stream reader.
                string line;


                using (System.IO.StreamReader file = new System.IO.StreamReader(@"Notes.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    while ((line = file.ReadLine()) != null)
                    {
                        notesLoaded.Add(line);
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            try {
                foreach (string noteLoad in notesLoaded)
                {
                    string[] dateAndNote = noteLoad.Split(new string[] { constSeparator }, StringSplitOptions.None);
                    Note note = new Note();
                    var noteText = dateAndNote[1];
                    var date = DateTime.Parse(dateAndNote[0]);
                    note.date = date;
                    note.text = noteText;
                    allNotes.Add(note);
                }
            }
            catch (Exception)
            {

                //do nothing
            }
        }
        //best workaround ever
        public void CreateCalendar2(int yr, int mth)
        {
            loadNotes();
            CreateCalendar(yr, mth);
        }
        public void deleteNote()
        {
            int rowNumber = 0;
            foreach (Note textNote in allNotes)
            {
                rowNumber++;
                Console.WriteLine("{2}: {0} , {1}", textNote.date, textNote.text, rowNumber);
            }
            Console.WriteLine();
            Console.WriteLine("Which note would you like to delete?");
            var lineNumber = Console.ReadLine();
            int lineNumberInt = new int();
            if (!Int32.TryParse(lineNumber, out lineNumberInt))
            {
                Console.WriteLine("The format of the input is not correct. Press any key to return to main menu.");
                Console.ReadKey();
            }
            else {
                if (lineNumberInt < 0 || lineNumberInt > allNotes.Count)
                {
                    Console.WriteLine("Such note is not available. Press any key to return to main menu.");
                    Console.ReadKey();
                }
                else {
                    allNotes.RemoveAt(lineNumberInt - 1);

                    string line = null;
                    int line_number = 0;
                    int line_to_delete = lineNumberInt;

                    List<string> notesOutput = new List<string>();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(@"Notes.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            line_number++;

                            if (line_number == line_to_delete)
                                continue;

                            notesOutput.Add(line);
                        }
                    }
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"Notes.txt"))
                    {
                        var notes = new StringBuilder();
                        foreach (string note in notesOutput)
                        {
                            notes.AppendLine(note);
                        }
                        writer.Write(notes);
                    }
                }
                }
            }
        public void editNote()
        {
            int rowNumber = 0;
            foreach (Note textNote in allNotes)
            {
                rowNumber++;
                Console.WriteLine("{2}: {0} , {1}", textNote.date, textNote.text, rowNumber);
            }
            Console.WriteLine();
            Console.WriteLine("Which note would you like to edit?");
            var lineNumber = Console.ReadLine();
            int lineNumberInt = new int();
            if (!Int32.TryParse(lineNumber, out lineNumberInt))
            {
                Console.WriteLine("The format of the input is not correct. Press any key to return to main menu.");
                Console.ReadKey();
            }
            else {
                if (lineNumberInt < 0 || lineNumberInt > allNotes.Count)
                {
                    Console.WriteLine("Such note is not available. Press any key to return to main menu.");
                    Console.ReadKey();
                }
                else {
                    Console.WriteLine("Please input the text for a new note.");
                    var newNoteText = Console.ReadLine();

                    string line = null;
                    int lineNumberCurrent = 0;
                    int lineToEdit = lineNumberInt;

                    List<string> notesOutput = new List<string>();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(@"Notes.txt"))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            lineNumberCurrent++;

                            var noteToEdit = new Note();
                            if (lineNumberCurrent == lineToEdit)
                            {
                                var dateOfEdited = allNotes[lineNumberInt - 1].date;
                                allNotes.RemoveAt(lineNumberInt - 1);
                                var editedNote = new Note();
                                editedNote.text = newNoteText;
                                editedNote.date = dateOfEdited;
                                allNotes.Add(editedNote);
                                string fullNoteEdited = dateOfEdited.ToShortDateString() + constSeparator + newNoteText;
                                notesOutput.Add(fullNoteEdited);
                                continue;
                            }
                         

                            notesOutput.Add(line);
                        }
                    }
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(@"Notes.txt"))
                    {
                        var notes = new StringBuilder();
                        foreach (string note in notesOutput)
                        {
                            notes.AppendLine(note);
                        }
                        writer.Write(notes);
                    }
                    Console.WriteLine("Your note has been edited! press any key to proceed");
                    Console.ReadKey();
                }
            }
        }


    }
        }



