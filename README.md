# helloWorldOnContainers
### Run `docker compose up` in the `src` folder to run application 

### Services:
* Identity,
* Words,
* Competition.

### Use cases:
* `All roles` authenticates using login and password credentials,
* `User` creates and browses collections of words,
* `User` adds favourite words from collections to dictionary,
* `User` competes in words learning with another one in real time,
* `Moderator` checks `User` collections for mistakes,
* `SuperAdmin` appoints `Moderators`.

### Technologies:
* `IdentityServer4` - user authentication and authorization,
* `SignalR` - real time words competitions,
* `Ocelot` - API Gateway,
* `RabbitMQ?` - message broker,
* `Docker` - containerize services,
* `Swagger` - API documentation,
* `Hangfire` - updating `collection of the week`.

### Application scheme<br>
![alt text](helloWorldOnContainers.drawio.png)