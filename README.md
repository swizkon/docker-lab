# docker-lab




### Mark Heath

https://markheath.net/post/transfer-files-docker-windows-containers

```
docker run -d -p 80 --name datatest1 microsoft/iis:nanoserver
$ip = docker inspect -f "{{.NetworkSettings.Networks.nat.IPAddress}}" datatest1
Start-Process -FilePath [http://$ip](http://$ip)
```
