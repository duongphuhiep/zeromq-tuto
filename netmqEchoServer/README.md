
This is a [ZermoMQ](http://zeromq.org/)/[NetMQ](http://netmq.readthedocs.org/en/latest/) **server**.

It will listen on the socket "tcp://127.0.0.1:12345" (see `app.config`).

This application server can be launched
 
 * as a standalone console application
 * or deployed and start as a Windows Service


# Run as standalone console application

Compile and run `netmqEchoServer.exe`

# Run as Windows Service

This project used a [Windows Service Wrapper `winsw`](https://github.com/kohsuke/winsw)

The `netmqEchoServerService.exe` is the service wrapper [`winsw-1.18-bin.exe`](https://github.com/kohsuke/winsw) renamed by convention

You can Install / Uninstall / the Window Service by command line:

```
netmqEchoServerService.exe install
netmqEchoServerService.exe uninstall
netmqEchoServerService.exe start
netmqEchoServerService.exe stop
```

