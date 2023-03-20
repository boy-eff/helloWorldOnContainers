# helloWorldOnContainers
### Run `docker compose up` in the `src` folder to run application 

### Services:
* Identity,
* Words,
* Achievements.

### Use cases:
* `All roles` authenticate using login and password credentials,
* `User` creates and browses collections of words,
* `User` adds favourite words from collections to dictionary,
* `User` learns words using word tests,

### Technologies:
* `IdentityServer4` - user authentication and authorization,
* `SignalR` - real time words tests passing,
* `Ocelot` - API Gateway,
* `RabbitMQ` - message broker,
* `Docker` - containerize services,
* `Swagger` - API documentation,
* `Quartz` - updating `collection of the week`, amount of views of collections and publishing user anniversary message.

### Application scheme<br>
![alt text](helloWorldOnContainers.drawio.png)
