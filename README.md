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
    <img src="">
  </li>
</ul>


## Question 04
