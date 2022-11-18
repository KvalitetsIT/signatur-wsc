# Introduction
This setup includes a docker-compose setup for calling Signaturs API. 

The proxy (webservice-consumer(wsc)) uses the OIO-IDWS-REST standard for calling the endpoint.

## Prerequisites
You need:

 * Docker 20+
 * Docker-compose 1.29+

## Project structure

The git project has the following structure

```
.
├── compose.yaml
├── config
│   └── Caddyfile-wsc.json
├── LICENSE
├── portalapiCerts
│   └── certificate.cer
├── README.md
├── stsCerts
│   └── sts.cer
└── wscCerts
    ├── certificate.cer
    └── key.pem
```

This table explains how the files are used.

| File name | Description |
|----------------------|---------- |
| compose.yaml | The compose file starts two containers. A reverse proxy that supports the OIO-IDWS-REST protocol and a mongodb for caching |
| Caddyfile-wsc.json | The configuration file for the reverse proxy|
| portalapiCerts/certificate.cer | The public certificate for the api endpoint |
| sts.cer | The public certificate for STS endpoint |
| wscCerts/certificate.cer | The public certificate for the client (YOU!). This is just a test certificate |
| wscCerts/key.pem | The private key for the client (YOU!). This is just a test key. In production, always keep your key safe! |

## Flow
The (OIO-IDWS-REST) proxy will first call Signaturs STS (Secure-Token-Service). The STS will issue a token basen on the client-certificate.

The token will together with the client certificate give access to Signaturs API.

You can read more about the OIO-IDWS-REST protocol [here](https://www.digitaliser.dk/resource/5988041).


## Running the example
Follow these steps to run the example. These steps assumes that you cloned this git repo and have prerequisites in place:

```
docker-compose up 
```

The setup uses a default port *8080*. It can be changed in the compose.yaml file (if the port is already in use).

When the containers are running you can call the api. This can be done with the following curl.

```
curl -H "content-type: application/json" -d '{"ClientId":"1524","PeriodFrom":"2022-10-01T00:00:00","PeriodTo":"2022-10-30T23:59:59"}' http://localhost:8080/recruitment/statistics/1
```

## Running in production
Before you can access the production api you will need the following.

* Obtain a public/private key-pair that identifies you as a client. Send the public part (and only the public part to Signatur). Signatur will upload the certificate into the STS.







