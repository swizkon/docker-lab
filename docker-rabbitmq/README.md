# docker-rabbitmq


```
#
docker pull rabbitmq:3-management

# Run with defaults...
docker run -d -p 15672:15672 -p 5672:5672 --name rabbitmq-docker rabbitmq:3-management
```

Verify that it´s runing at http://localhost:15672/#/
Login guest:guest






Based on:
https://levelup.gitconnected.com/rabbitmq-with-docker-on-windows-in-30-minutes-172e88bb0808
