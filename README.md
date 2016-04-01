
This is a test project to learn basic usage of [ZermoMQ](http://zeromq.org/)/[NetMQ](http://netmq.readthedocs.org/en/latest/)...

It included:

 * A simple netmq server (Echo server)
 * A zeromq client
 * A netmq client

Usage:

 * Run the Echo server
 * Use zeromq client or netmq client to send request and get reply form the Echo server. Example:

``` 
	netmqclient tcp://127.0.0.1:12345 hello
```



