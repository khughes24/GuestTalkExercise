using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;

class Program
{
    //Main body of the program
    static void Main(string[] args)
    {
        //Set our initial variables and the path to our JSON file
        int shutDown = 0;
        Random rand = new Random();
        string path = @"D:\path.json";
        //Check if our JSON file exists, if not populate a JSON and create the file
        if (!(File.Exists(path))){
            List<data> _data = new List<data>();
            _data.Add(new data()
            {
                GUID = Guid.NewGuid(),
                Name = "Kate Hughes",
                PhoneNo = "011223344",
                lapTime = rand.Next(50,100),
                lapCount = rand.Next(10,20),
                teamName = "Golden Eggs"

            });

            string json = JsonSerializer.Serialize(_data);
            File.WriteAllText(path, json);
        }
        //Keep looping the main selection switch case until the user has finished all desired operations
        while(shutDown == 0){
            Console.WriteLine("Please choose an option 'ADD','VIEW','DELETE','DELETEALL','EXPORT','EXIT':");
            string userOption = Console.ReadLine();
            string selector = "";
            //Force the input into a regular format to allow for greater flexiblity
            if (userOption != ""){
                selector = userOption.ToUpper();
            }
            
            switch(selector){
                case "ADD":
                    addData();
                    break;
                 case "VIEW":
                    viewData();
                    break;  
                case "DELETE":
                    deleteData();
                    break;  
                case "DELETEALL":
                    deleteFile();
                    break;  
                case "EXPORT":
                    exportData();
                    break;
                case "EXIT":
                    shutDown = 1;
                    break; 
                default:
                    Console.WriteLine("Incorrect mode");  
                    break;       
            }
        }
        
    }

    //Handles deletion of the JSON file
    public static void deleteFile(){
        Console.WriteLine("Are you sure you want to delete this record? Y/N");
        string userConfirm = Console.ReadLine();
        if (userConfirm.ToUpper() == "Y"){
            string path = @"D:\path.json";
            try
            {
                File.Delete(path);
            } catch (IOException){
                return;
            }
            }
    }


    //Converts the JSON file into a CSV file which can be opened by EXCEL
    public static void exportData(){

        //Set the inital variables and paths
        string path = @"D:\path.json";
        var json = File.ReadAllText(path);
        List<data> list = JsonSerializer.Deserialize<List<data>>(json, _options);
        string csvFile = @"D:\Output.csv";
        string separator = ",";
        //Create a string builder to handle the creation and formating of the new CSV
        StringBuilder output = new StringBuilder();
        String[] headings = { "RacerID", "Name", "Contact Number", "Best Lap", "Lap Count", "Team Name" };
        output.AppendLine(string.Join(separator, headings));
        //Loop through each line in the CSV file and add it to the in construction string
        foreach (data item in list){
            String[] newLine = { item.GUID.ToString(), item.Name, item.PhoneNo, item.lapTime.ToString(), item.lapCount.ToString(), item.teamName };
            output.AppendLine(string.Join(separator, newLine));
        }

        //Try and write the data to the CSV, if not throw an error and tell the user it either failed or worked
        try{
            File.AppendAllText(csvFile, output.ToString());
            Console.WriteLine("CSV export complete.");
        }
        catch(Exception ex)
        {
            Console.WriteLine("Data could not be written to the CSV file.");
            return;
        }
    }


    //Delete a selected row of the JSON
    public static void deleteData(){

        //Set the inital variables and paths
        string path = @"D:\path.json";
        var json = File.ReadAllText(path);
        List<data> list = JsonSerializer.Deserialize<List<data>>(json, _options);
        //Get the number of lines in the JSON
        int listLen = list.Count;
        //Check if there are actually any lines to delete
        if (listLen == 0){
            Console.WriteLine("There are no records to delete.");
            return;
        }
        Console.WriteLine("Please select a record to delete (Number of records: " + listLen + " )");
        string userOption = Console.ReadLine();
        //Check to make sure the user has entered a number
        var isNumeric = int.TryParse(userOption, out int selector);
        if (isNumeric == false){
            Console.WriteLine("Invalid value entered. Returning to main menu");
            return;
        }
         //Check to make sure the user has entered a valid number corresponding to the JSON lines
        if (selector > listLen || selector < 1 ){
            Console.WriteLine("Invalid record selected. Returning to main menu");
            return;
        }
        //Output the selected line and then confirm with the user if it should be removed
        Console.WriteLine(list[selector-1].GUID + "," + list[selector-1].Name + "," + list[selector-1].PhoneNo);
        Console.WriteLine("Are you sure you want to delete this record? Y/N");
        string userConfirm = Console.ReadLine();
        //If yes then remove the record from the list and rewrite the file
        if (userConfirm.ToUpper() == "Y"){
            list.RemoveAt(selector-1);
            string jsonUpdate = JsonSerializer.Serialize(list);
            File.WriteAllText(path, jsonUpdate);  
            Console.WriteLine("Record deleted");
            return;
        }
    }

    //Create options for the JSON Serializer to user
    private static readonly JsonSerializerOptions _options = new()
{
    PropertyNameCaseInsensitive = true
};

    //Output the data in the terminal for the user
    public static void viewData(){

        //Set the inital variables and paths
        string path = @"D:\path.json";
        //Read the JSON into a list
        var json = File.ReadAllText(path);
        List<data> list = JsonSerializer.Deserialize<List<data>>(json, _options);
        //Loop through the list and output to the user
        foreach (data item in list){
            Console.WriteLine(item.GUID + "," + item.Name + "," + item.PhoneNo +  "," + item.lapTime + "," + item.lapCount + "," + item.teamName);
        }
    }

    //Add a new record to the JSON file
    public static void addData(){

        //Set the inital variables and paths
        string path = @"D:\path.json";
        int userLapTime = 0;
        int userLapCount = 0;
        //Read the JSON into a list
        var json = File.ReadAllText(path);
        List<data> list = JsonSerializer.Deserialize<List<data>>(json, _options);
        //Get the new values to add to the JSON file
        Console.WriteLine("Please enter a name:");
        string userName = Console.ReadLine();
        Console.WriteLine("Please enter a phone number:");
        string userNumber = Console.ReadLine();
        int lapTimeValid = 0;
        //Get & Check if laptime is a valid value
        while(lapTimeValid == 0 ){
            Console.WriteLine("Please enter the fastest lap time:");
            string userLapString = Console.ReadLine();
            var isNumeric = int.TryParse(userLapString, out userLapTime);
            if (isNumeric == true){
                lapTimeValid = 1;
            }
        }
        int lapCountValid = 0;
        //Get & Check if lapcount is a valid value
        while(lapCountValid == 0 ){
            Console.WriteLine("Please enter the number of laps:");
            string userLapCountString = Console.ReadLine();
            var isNumeric = int.TryParse(userLapCountString, out userLapCount);
            if (isNumeric == true){
                lapCountValid = 1;
            }
        }
        Console.WriteLine("Please enter the team name:");
        string userTeamName = Console.ReadLine();
        //Create the new data value and add it to the list
        list.Add(new data()
            {
                GUID = Guid.NewGuid(),
                Name = userName,
                PhoneNo = userNumber,
                lapTime = userLapTime,
                lapCount = userLapCount,
                teamName = userTeamName
            });
        string jsonUpdate = JsonSerializer.Serialize(list);
        //Write the new list to the JSON file
        File.WriteAllText(path, jsonUpdate);    
 
    }
}