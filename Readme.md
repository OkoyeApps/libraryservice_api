# Simple Library Service Api
This is a simple api service that simulate an online library system where users can borrow and return books like they would their local libraries

# Tools
- .Net core 3.1
- Visual Studio code and Visual Studio
- Mongo Db
- Docker

# How to run
This project was developed with .Net core and can run on any platforms.
But if you want to view the code, you can clone this project and open it with your favourite editor.
> Run with Visual Studio
  - Clone the project
  - Run nuget restore
  - Make sure mongo db is running on and on default port 27017 else change the connection string in the appsetting.json
  - Build and run
> Run with Docker 
  - Run docker-compose up from the root directory of the project.
  - Note: The docker compose script provides two connection string one local and the other an atlas connection string. this was so because mongo advanced operations like Aggregation and Transaction works with mongo replica set and your local mongo might not support that. You could provide your favourite url in place.