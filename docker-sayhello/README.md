# Say Hello

docker run -d -p 8090:80 -e "SayHello:ServiceName=SlickRIck" --rm --name myapp sayhelloapp

Run with 3 consoles


Console 1
```
cd src/SayHelloApp
dotnet run --useurl=http://localhost:5061 "SayHello:ServiceName=SubService1"
```

Console 2
```
cd src/SayHelloApp
dotnet run --useurl=http://localhost:5062 "SayHello:ServiceName=SubService2"
```

Console 3
```
cd src/SayHelloApp
dotnet run --useurl=http://localhost:5000 "SayHello:ServiceName=MasterService" "SayHello:Siblings=http://localhost:5061 http://localhost:5062"
```
 
Navigate to http://localhost:5000 and http://localhost:5000/home/siblings to see the outcome of poor discovery

https://www.hanselman.com/blog/BuildingRunningAndTestingNETCoreAndASPNETCore21InDockerOnARaspberryPiARM32.aspx

https://www.hanselman.com/blog/HowToBuildAKubernetesClusterWithARMRaspberryPiThenRunNETCoreOnOpenFaas.aspx