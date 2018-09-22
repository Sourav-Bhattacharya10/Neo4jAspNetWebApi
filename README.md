# Neo4jAspNetWebApi
This code aims at providing basic understanding of creating ASP.NET Web API using Neo4j as graph database.

## Package Dependency
This project requires mainly three packages:
* Neo4j Driver Nuget


```shell
Install-Package Neo4j.Driver -Version 1.6.1
```

## Source Code Breakdown
* Import the required package.
* Create Two Helper classes, one for managing the nodes and the other for managing the relationships.
* Write the helper functions using Neo4j driver to perform CRUD operations.
* Write API functions.
* Test them using Postman

## Data Format
Nodes will be displayed in the following format:
```shell
{
    "name": "Charlie",
    "marks": 90,
    "favoritesubject": "English"
}
```

Relationships will be displayed in the following format:
```shell
{
        "From": "Alice",
        "To": "Bob",
        "Type": "Knows"
}
```

## Quickstart
* Clone this repository
* Change ServerUri, Username, Password in the Web.config accordingly
* Press F5 to run the project

![A.png](/Screenshots/A.png)


![B.png](/Screenshots/B.png)


## API call using Postman
### For Nodes
* GET http://localhost:59808/api/Student
Response:
Status Code : 200
Response Body:
```shell
[
    {
        "name": "Charlie",
        "marks": 90,
        "favoritesubject": "English"
    },
    ...
]
```
-> *Displays all the Students*

* GET http://localhost:59808/api/Student?name=Charlie
Response:
Status Code : 200
Response Body:
```shell
[
    {
        "name": "Charlie",
        "marks": 90,
        "favoritesubject": "English"
    }
]
```
-> *Displays the student with specified name*

* POST http://localhost:59808/api/Student
Request Header:
Content-Type : application/json

Request Body :
```shell
{
	"name" : "Danny",
	"marks" : 75,
	"favoritesubject" : "Science"
}
```

Response:
Status Code : 201
Response Body :
```shell
Inserted
```

-> *Creates the student*

* PUT http://localhost:59808/api/Student
Request Header:
Content-Type : application/json

Request Body :
Request Body :
```shell
{
	"name" : "Danny",
	"marks" : 40,
	"favoritesubject" : "English"
}
```

Response:
Status Code : 200
Response Body :
```shell
{
	"name" : "Danny",
	"marks" : 40,
	"favoritesubject" : "English"
}
```

-> *Updates the student*

* DELETE http://localhost:59808/api/Student?name=Danny
Response:
Status Code : 200
Response Body :
```shell
Deleted
```

-> *Deletes the student with specified name*


### For Relationships
* GET http://localhost:59808/api/Relationship
Response:
Status Code : 200
Response Body:
```shell
[
    {
        "From": "Alice",
        "To": "Bob",
        "Type": "Knows"
    },
    ...
]
```
-> *Displays all the Relationships*

* GET http://localhost:59808/api/Relationship?type=Classmate
Response:
Status Code : 200
Response Body:
```shell
[
    {
        "From": "Alice",
        "To": "Bob",
        "Type": "Knows"
    },
    ...
]
```
-> *Displays the relationships with specified type*

* POST http://localhost:59808/api/Relationship
Request Header:
Content-Type : application/json

Request Body :
```shell
{
	"From" : "Bob",
	"To" : "Charlie",
	"Type" : "Classmate"
}
```

Response:
Status Code : 201
Response Body :
```shell
Created
```

-> *Creates the relationship*

* PUT http://localhost:59808/api/Relationship?oldtype=Classmate&newtype=Knows
Response:
Status Code : 200
Response Body :
```shell
Updated
```

-> *Updates the relationship*

* DELETE http://localhost:59808/api/Relationship?type=Classmate
Response:
Status Code : 200
Response Body :
```shell
Deleted
```

-> *Deletes the relationship with specified name*