# SaaSProductsImportApp
This tool is used to fetch the data from the provider feeds

**Steps to solve the problem:**

1. Provider Feed (xml, json,yaml etc) 

2. Convert into XML

3. Sanitize the file with XSL  

4. Get the standard XML file with req tags 

5. Used Linq to fetch data  

6. Show data to console

7. Push data to DB                                      
 




  1) **Installation steps**: 
  - As it's .net core console app so no need to install the application. Just double click on the executeable file.
  
  2) **How to run your code / tests**: 
    -There are two ways to run the code, either download the repo code, build and run the code locally.
     Or directly download the release folder, and double click on the application to run the code. The release folder contains the release build created from the base code(SaaSProductsImportApp/Assignment/coding/Release/), as well as there are two more folder as well;

      a) XSL -> This folder contains the .xsl files to parse the provider feeds data. If we need to consume the more provider data then we should create an new .xsl file as well to get the required data from input.

      b) feed-products -> this folder contains the provider input feed downlaoded from the emails.

      Note-In order to run the application, these folders i.g. XSL and folder that contains provider input feeds, should share the same folder location. Violating this rule will close or terminate the application immidiately.
    
  4) **Where to find your code**: 
  -Code is availbale at the branch base location. Also, DB assignment is available at the below location.
   DB   -> SaaSProductsImportApp/Assignment/database/

  6) **Was it your first time writing a unit test, using a particular framework, etc?**:
      -Worked previously on the unit and playwrite testing framework to write testcase.
  8) **What would you have done differently if you had had more time**:
   
   i) Not able to write testcases properly.
    
   ii) Could improve the naming convention and folder structure.
   
  iii) Could think about other apporch to get this task done, etc.


