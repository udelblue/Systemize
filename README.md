# Systemize
 <div>
 <p align="center">
	Systemize is a document workflow system. This allows users to create templated workflows to provide a linear approval system for digitized documents. Some of the features are a history and timeline of all events. Metrics on time spent at each stage of the approval process. Basic authenication, and more. 
 </p>
 </div>
 <div>


![alt text](https://raw.githubusercontent.com/udelblue/Systemize/refs/heads/master/Screenshots/authentication.png "Authenication")

![alt text](https://raw.githubusercontent.com/udelblue/Systemize/refs/heads/master/Screenshots/details.png "Details")

![alt text](https://raw.githubusercontent.com/udelblue/Systemize/refs/heads/master/Screenshots/Metrics.png "Metrics")

![alt text](https://raw.githubusercontent.com/udelblue/Systemize/refs/heads/master/Screenshots/Timeline.png "Timeline")

 </div>

## Run
To run the project please follow the following commands below. 

### Run the following command to create a new migration and update the database

- delete the files in migrations folder
- to run update connection string in appsettings.json

run the following command in the terminal
```bash

dotnet ef migrations add InitialCreate
dotnet ef database update

```

or 

```bash	
Add-Migration InitialCreate
Update-Database
```
