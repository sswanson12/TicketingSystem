namespace Ticketing_System
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string ticketsFile = @"Files\tickets.csv";
            var continueLoop = true;
            //ask to read data from file or create file from data
            while (continueLoop)
            {
                Console.Write("Welcome to the ticketing system! You may read an existing file, or you can write to one!\n" +
                              "Please enter (1) in order to read, or (2) to write. Press any other key to end program.");
                var answer = Convert.ToInt32(Console.ReadLine());
                switch (answer)
                {
                    case 1:
                        Console.WriteLine(ReadFile(ticketsFile));
                        break;
                    case 2:
                        WriteFile(ticketsFile);
                        break;
                    default:
                        continueLoop = false;
                        break;
                }
            }
            //if read file, display file contents while skipping header
            Console.WriteLine(ReadFile(ticketsFile));
            //if write file, add header to file and ask user for necessary information
            WriteFile(ticketsFile);
        }

        private static string ReadFile(string file)
        {
            var ticketString = "";
            StreamReader sr = new StreamReader(file);
            
            sr.ReadLine(); //Skips header

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                var column = line.Split(',');
                var watchingColumn = column[6].Split('|');

                ticketString +=
                    $"Ticket ID: {column[0]} - Summary: {column[1]} - Status: {column[2]} - Priority: {column[3]} "
                    + $"Submitter: {column[4]} - Assigned: {column[5]} - Watching: ";

                for (var i = 0; i < watchingColumn.Length; i++)
                {  //Lists persons watching seperated by commas rather than pipes.
                    ticketString += $"{watchingColumn[i]}, ";
                }

                ticketString = ticketString.Remove(ticketString.Length - 2, 2); //Removes redundant comma & space.
                ticketString += "\n";
            }

            sr.Close();
            return ticketString;
        }

        private static void WriteFile(string file)
        {
            StreamReader sr = new StreamReader(file);
            
            var necessaryFields = sr.ReadLine();
            var columns = necessaryFields.Split(',');

            Console.WriteLine($"You must enter the following fields: {columns[0]}, {columns[1]}, {columns[2]}, {columns[3]}, {columns[4]}, {columns[5]}, {columns[6]}.");

            sr.Close();
            
            var inputedFields = new String[columns.Length];
            
            for (var i = 0; i < inputedFields.Length; i++)
            {
                inputedFields[i] = TakeInput(file, i);
            }

            var fullInput = string.Join(",", inputedFields);
            

            StreamWriter sw = new StreamWriter(file, true);
            sw.WriteLine(); //Ensures that data is placed on a new line
            sw.Write(fullInput);
            sw.Close();
        }

        static string TakeInput(string file, int instance)
        {
            StreamReader sr = new StreamReader(file);
            
            var necessaryFields = sr.ReadLine();
            var columns = necessaryFields.Split(',');
            
            sr.Close();

            if (instance == 6)
            {
                Console.Write("How many people are watching? ");
                int peopleWatching = Convert.ToInt32(Console.ReadLine());
                var peopleString = "";
                for (var i = 0; i < peopleWatching; i++)
                {
                    Console.Write($"Enter person {i + 1}: ");
                    peopleString += Console.ReadLine() + "|";
                }
                peopleString = peopleString.Remove(peopleString.Length - 1, 1);
                return peopleString;
            }
            else
            {
                Console.Write($"Enter {columns[instance]}: ");
                string input = "" + Console.ReadLine();

                while (input == "")
                {
                    Console.Write("Make sure not to leave your answers blank! try again: ");
                    input += Console.ReadLine();
                }

                return input;
            }
        }
    }
}