# IPT-A2

## Question 01

Create a Windows Service which downloads a web page after every 5 minutes. Hint: You can use the “Process” class available in Dot Net to execute the code which you had created in Assignment 1 Question 1.

## Question 02

Create a Windows Service which parses the data from the previous question after every 10 minutes. You should use the parsing logic implemented in the previous assignment (A1Q2) by including the logic as a class library (dll). After every 10 minutes, a new file would be created and the previous one will remain in the same folder.
<br>
<br>
Note: Folder logic is the same as mentioned in Assignment 1.

## Question 03
Create a timer based Azure Function which is responsible to combine XML files and generate a JSON file for each of the script. You will have one JSON file output per script. The input for this service is the output directory of Question 2. The output of this question will be as following:
<br>
<br>

<ul>
  <li>Folder Structure:
  <ul>
    <li>some folder\CategoryNameFolder\scriptFile-Accumulated.json</li>
    <li>Example:
      <ul>
        <li>D:\Output\AutomobileAssembler\AtlasHondaLimited.json</li>
      </ul>
      </li>
  </ul>
  </li>
  <li>
    JSON File Structure:
  </li>
  <br>
  <img src="https://github.com/mustafabawany/IPT-A2/blob/main/Filestructure.png">
</ul>
<br>
The service will execute after every 20 minutes. If the JSON file does not exist, it will create a new file but
if the file already exists, it will append to the same file. The field lastUpdatedOn will be changed whenever
the file has been last modified. Once the service has read the XML file and generated a JSON file, it will
delete the XML file.

## Question 04

Create an Azure Function (Http Trigger) where the user will pass the script name as a query string parameter and the function will return the data from the json file created in Question 3.
<br>
Example: Browser URL: http://localhost/myAzureFunction/AtlasHondaLimited
<br>
<br>
Output:
<br>
<br>
<img src="https://github.com/mustafabawany/IPT-A2/blob/main/Filestructure.png">
