# ASPCoreGPSService   
Aspcore an example of using webapi with elements of microservices pattern and file storage information.        
Purpose: Takes a pair of two gps coordinates and calculates the distance between them.
### Build 🤯    
    
`dotnet publish -r win10-x64`   
or    
`dotnet publish -r linux-x64`    

### Run 🧨

`dotnet ./gps-service.dll`    
or start platform-specific binary.    

### Use 📯

Start service.

Send 
```HTTP
POST */api/v1/gps/store
```
with `application/json` body data
```JSON
[
	{"Latitude": "30.515968", "Longitude": "59.906666"},	
	{"Latitude": "30.482558", "Longitude": "59.907405"},	
	
	{"Latitude": "30.482558", "Longitude": "59.907405"},
	{"Latitude": "30.468565", "Longitude": "59.919820"},	
	
	{"Latitude": "30.309195", "Longitude": "59.936327"},
	{"Latitude": "37.637795", "Longitude": "55.764125"}
]
```

And receive response:

```JSON
{
    "sessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
    "message": "Entity applied in database: 6"
}
```
with `200 OK`


after use SessionID and send  
```HTTP
VIEW */api/v1/gprs/bySession?sid=46af2fbe-1c67-4232-9781-049389a80a42
```

and receive response:

```JSON
{
    "message": "found",
    "result": {
        "sessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
        "zipBytes": "UEsDBBQAAAAIADkPq...."
    }
}
```

zipBytes - base64 string of zip file which contains a json file.

JSON File:

```JSON
[
  {
    "Pairs": {
      "Item1": {
        "UID": "7687b98c-6860-45c8-bcdf-af0064beb7cc",
        "Latitude": 30.309195,
        "Longitude": 59.93633,
        "SessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
        "State": "Complete"
      },
      "Item2": {
        "UID": "d50a332c-7952-4e89-b5fb-171a22b2f588",
        "Latitude": 37.637794,
        "Longitude": 55.764126,
        "SessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
        "State": "Complete"
      }
    },
    "Distance": 901136.2,
    "SessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
    "Timestamp": "2019-05-07T22:57:50.6360298+00:00"
  }
]

```



### API ⛓

#### POST /api/v1/gps/store
Storing gps position array for subsequent processing.

Body:
```JSON
[
	{"Latitude": "30.309195", "Longitude": "59.936327"},
	{"Latitude": "37.637795", "Longitude": "55.764125"}
]
```

Response:

##### OK 200
Body:
```JSON
{
    "sessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
    "message": "Entity applied in database: 6"
}
```
```
sessionID - your session id))0
message   - information trace message
```
##### BadRequest 400  
The number of coordinate elements must be a multiple of two
##### InternalError 500   
Failed applied entity to database.


#### VIEW /api/v1/gps/bySession
Query params:
```
- sid - your session ID
```
Response:

##### OK 200

Body:
```JSON
{
    "message": "found",
    "result": {
        "sessionID": "46af2fbe-1c67-4232-9781-049389a80a42",
        "zipBytes": "UEsDBBQAAAAIADkPq...."
    }
}
```

```
message           - information trace message
result->sessionID - your session id
result->zipBytes  - base64 string of zip file bytes
```


##### BadRequest 400
`sid` cannot be empty.
##### NotFound   404
Task of given `sid` not found.


