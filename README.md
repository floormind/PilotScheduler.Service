# PilotScheduling.Service

This is an API based project to provide availability or pilots based on flight schedule. 

## Endpoints

There are two endpoints on the API and they are

1. RequestPilot
    1. An endpoint that fetches a pilot that is currently available based on the location, departure date and return date.

2. ScheduleFlight
    1. An endpoint that will schedule/reserve a pilot based on the provided departure date and return date for a given pilot.

## Assumptions Taken

1. I assume that Authentication is something that is needed, I have not implemented authentication and authorization in this solution, however if i was to I will create a different service to that acts as an authentication service, this service will validate users based on their login credentials and then return a token (JWT). The token will then be used to make a request to the PilotScheduler.Service project.

2. As this is just a small project, I have not implemented a database, however I have used LiteDB which is a MongoDB like database that is persisted to file. This is good for test purposes. If you're running this test for the first time, you may need to delete tempdata.db under the PilotScheduler.Service project in order to have a new database create once you start the project. 

3. The test are current testing the controller return object, in a real world scenario, we could add test for the repository as that's where the real logic is.

4. In regards to the repository, I have added an implementation called DummyDatabseRepository, this is just to stress the fact that that this isnt a real database, but once a real one has been implements, the new repository for that database can just implement the interface IDatabaseRepository. 

5. I have added a few interfaces to simulate the various rules that apply to requestion a pilot, 
    1. IFlyableFromLocation => A rule that checks for the location a pilot is able to fly from. 
    1. IFlyableOnDay => A rule that checks for the day of the week a pilot is able to fly on.

## Extra
1. I have also added a docker file which can be used to run this project. 
    1. First build a docker image by navigating to the base path of the project and running
        1. ```docker build -t pilot-scheduler .```
    1. Run the image as a container by executing the below 
        1. ```docker run -d -p 5000:5000 pilot-scheduler```

## Sample Request
1. To call the request pilot endpoint, run the following in postman or any other tool you prefere 
    1. ```https://localhost:5001/Schedule/request-pilot/Munich/2021-05-01T09%3A00%3A00Z/2021-05-01T11%3A00%3A00Z``` 
    1. The result should be similar to the below json document. 
    1. ``` json
        {
            "id": "419d8f08-69b4-49e3-9e4e-07261c2a7a63",
            "pilotId": 1,
            "requestCount": 1
        }
        ```
