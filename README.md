# Acquired .Net Library 1.0.0

## Description ##
The Acquired API Library for Java enables you to work with Acquired APIs.

## Environment ##
ASP.net core 2.0  
MVC  
C#

The SDK core is in "Lib" file writen by c#.  
So if you use the old .NET Framwork you can just focus the "Lib" files;


## Directory ##
```html
|--wwwroot
    |--css
        general.css
|--Views (html files)
    |--Examples
        Auth_cshtml
        Refund.cshtml
        ...
|--Controllers (deal post data)
    ExamplesController.cs
|--Lib (sdk core)
    |--AQPay
       AQPayConfig.cs
       AQPayCommon.cs
       AQPay.cs
readme.md

else file could ignore
``` 

## Documentation  ##
https://docs.acquired.com/api.php

## Installation ##
You can simply Download the Release

#NuGet#
download api library from nuget.org (include AQPayCommon.cs and AQPay.cs which you can find code in Lib/AQpay)
https://www.nuget.org/packages/acquired.com
or install it from package manager PM> Install-Package acquired.com -Version 1.0.1

## Examples ##
#### Get start

1. set config parameters in AQPayConfig.cs.
2. import the below file in the example controller.

```php
using Acquiredapisdkdotnet.Lib.AQPay;
```

#### How to use
It is very simply to use like this:
1. new a AQPay obj.
```net
AQPay aqpay = new AQPay();
```
2. set parameters.
```java
aqpay.SetParam("amount", "1");
```
3. post parameters according to your transaction type.
```java
JObject result = aqpay.Capture();
```
4. deal response.
```java
if(aqpay.IsSignatureValid(result)) {
    
    // do your job
    
}
```
