# askme-reference-api

API reference implementation for askme projects 


## stack
* [aspnetcore-7.0](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/apis?view=aspnetcore-7.0)
* [xunit](https://xunit.net/)
* [moq](https://github.com/moq/moq4)
* [mongodb](https://www.mongodb.com/docs/drivers/csharp/v2.21/)

## get started
```
docker run -d -p 27017:27017 --name=askme-reference mongo:latest
```

## cleaning

get the container id
```
 docker ps --filter name=askme-reference
```

stop container
```
 docker stop <container id>
```
remove container
```
 docker rm <container id>
```


## todo
* use fluent assertions
* use linq to moq
* use docker-compose for up/down database
