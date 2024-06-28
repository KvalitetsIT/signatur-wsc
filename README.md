# Introduction
This setup includes a docker-compose setup for calling Signaturs API. 

The proxy (webservice-consumer(wsc)) uses the OIO-IDWS-REST standard for calling the endpoint.

The proxy is based on the KitCaddy project, which can be found [here](https://github.com/KvalitetsIT/kitcaddy/).

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
| sts.cer | The public certificate for STS endpoint |
| wscCerts/certificate.cer | The public certificate for the client (YOU!). This is just a test certificate |
| wscCerts/key.pem | The private key for the client (YOU!). This is just a test key. In production, always keep your key safe! |

## Flow
The (OIO-IDWS-REST) proxy will first call Signaturs STS (Secure-Token-Service). The STS will issue a token basen on the client-certificate.

The token will together with the client certificate give access to Signaturs API.

You can read more about the OIO-IDWS-REST protocol [here](https://digst.dk/it-loesninger/standarder/oio-identity-based-web-services-12-oio-idws/).


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

# Token
If the user provides a valid certificate known by the STS, a token is issued with the following structure.

The token is provided in the "x-sessiondata" header as base64 encoded String.

```
"definitions": {
	"ID": {
		"type": "string"
	},
	"Sessionid": {
		"type": "string"
	},
	"Authenticationtoken": {
		"type": "string"
	},
	"Timestamp": {
		"type": "string"
	},
	"Hash": {
		"type": "string"
	},
	"UserAttributes": {
		"type": "object",
		"properties": {
			"dk:nextstepcitizen:attribute:it-system": {
				"type": "array",
				"items": {
					"type": "string"
				}
			},
			"dk:signatur:portalapi:clientid": {
				"type": "array",
				"items": {
					"type": "string"
				}
			}
		}
	},
	"SessionAttributes": {
		"type": "object",
		"properties": { }
	},
	"ClientCertHash": {
		"type": "string"
	}
}
```

Example

```
{
    "ID": "63998f56dc5c9a8630314618",
    "Sessionid": "3e184afe-5347-46c7-812a-5b75f74cbfb2",
    "Authenticationtoken": "***TOKEN***",
    "Timestamp": "2022-12-14T16:54:16.449Z",
    "Hash": "K7QcveNcIKcvjDApL1fwwQ==",
    "UserAttributes": {
        "dk:nextstepcitizen:attribute:it-system": [
            "na"
        ],
        "dk:signatur:portalapi:clientid": [
            "2474"
        ]
    },
    "SessionAttributes": {},
    "ClientCertHash": "ef58ce70d110d150a38eecd85e05eb89628c4d18"
}

```

# Example code
Kombit maintain example code for OIOIDWS WSC [here](https://github.com/kombit/tokenservice_oioidws-net/tree/main).














