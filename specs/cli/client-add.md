# kiota client add

## Description 

`kiota client add` allows a developer to add a new API client to the `kiota-config.json` file. If no `kiota-config.json` file is found, a new `kiota-config.json` file would be created in thr current working directory. The command will add a new entry to the `clients` section of the `kiota-config.json` file. Once this is done, a local copy of the OpenAPI description is generated and kept in the `.kiota/descriptions` folder.

When executing, a new API entry will be added and will use the `--client-name` parameter as the key for the map. When loading the OpenAPI description, it will store the location of the description in the `descriptionLocation` property. If `--include-path` or `--exclude-path` are provided, they will be stored in the `includePatterns` and `excludePatterns` properties respectively.

Every time an API client is added, a copy of the OpenAPI description file will be stored in the `./.kiota/{client-name}` folder. The files will be named using the API client name. This will allow the CLI to detect changes in the description and avoid downloading the description again if it hasn't changed. 

At the same time, an [API Manifest](https://www.ietf.org/archive/id/draft-miller-api-manifest-01.html#section-2.5-3) file will be generated (if non existing) or edited (if already existing) to represent the surface of the API being used. This file will be named `apimanifest.json` and next to the `kiota-config.json`. This file will be used to generate the code files.

Once the `kiota-config.json` file is generated and the OpenAPI description file is saved locally, the code generation will be executed and then the API Manifest would become available.

## Parameters

| Parameters | Required | Example | Description |
| -- | -- | -- | -- |
| `--config-location \| --cl` | No | ../../ | A location where to find or create the `kiota-config.json` file. When not specified it will find an ancestor `kiota-config.json` file and if not found, will use the defaults. Defaults to `./`. |
| `--client-name \| --cn` | Yes | graphDelegated | Name of the client. Unique within the parent API. If not provided, defaults to --class-name or its default. |
| `--openapi \| -d` | Yes | https://raw.githubusercontent.com/microsoftgraph/msgraph-metadata/master/openapi/v1.0/openapi.yaml | The location of the OpenAPI description in JSON or YAML format to use to generate the SDK. Accepts a URL or a local path. |
| `--search-key \| --sk` | No | github::microsoftgraph/msgraph-metadata/graph.microsoft.com/v1.0 | The search key used to locate the OpenAPI description. |
| `--include-path \| -i` | No | /me/chats#GET | A glob pattern to include paths from generation. Accepts multiple values. Defaults to no value which includes everything. |
| `--exclude-path \| -e` | No | \*\*/users/\*\* | A glob pattern to exclude paths from generation. Accepts multiple values. Defaults to no value which excludes nothing. |
| `--language \| -l` | Yes | csharp | The target language for the generated code files or for the information. |
| `--class-name \| -c` | No | GraphClient | The name of the client class. Defaults to `Client`. |
| `--namespace-name \| -n` | No | Contoso.GraphApp | The namespace of the client class. Defaults to `Microsoft.Graph`. |
| `--backing-store \| -b` | No | | Defaults to `false` |
| `--exclude-backward-compatible \| --ebc` | No |  | Whether to exclude the code generated only for backward compatibility reasons or not. Defaults to `false`. |
| `--serializer \| -s` | No | `Contoso.Json.CustomSerializer` | One or more module names that implements ISerializationWriterFactory. Default are documented [here](https://learn.microsoft.com/openapi/kiota/using#--serializer--s). |
| `--deserializer \| --ds` | No | `Contoso.Json.CustomDeserializer` | One or more module names that implements IParseNodeFactory. Default are documented [here](https://learn.microsoft.com/en-us/openapi/kiota/using#--deserializer---ds). |
| `--structured-media-types \| -m` | No | `application/json` |Any valid media type which will match a request body type or a response type in the OpenAPI description. Default are documented [here](https://learn.microsoft.com/en-us/openapi/kiota/using#--structured-mime-types--m). |
| `--skip-generation \| --sg` | No | true | When specified, the generation would be skipped. Defaults to false. |
| `--output \| -o` | No | ./generated/graph/csharp | The output directory or file path for the generated code files. Defaults to `./output`. |

> [!NOTE] 
> It is not required to use the CLI to add new clients. It is possible to add a new client by adding a new entry in the `clients` section of the `kiota-config.json` file. See the [kiota-config.json schema](../schemas/kiota-config.json.md) for more information.

## Using `kiota client add`

```bash
kiota client add --client-name "graphDelegated" --openapi "https://raw.githubusercontent.com/microsoftgraph/msgraph-metadata/master/openapi/v1.0/openapi.yaml" --include-path "**/users/**" --language csharp --class-name "GraphClient" --namespace-name "Contoso.GraphApp" --backing-store --exclude-backward-compatible --serializer "Contoso.Json.CustomSerializer" --deserializer "Contoso.Json.CustomDeserializer" -structured-mime-types "application/json" --output "./generated/graph/csharp"
```

_The resulting `kiota-config.json` file will look like this:_

```jsonc
{
  "version": "1.0.0",
  "clients": {
    "graphDelegated": {
      "descriptionLocation": "https://raw.githubusercontent.com/microsoftgraph/msgraph-metadata/master/openapi/v1.0/openapi.yaml",
      "includePatterns": ["**/users/**"],
      "excludePatterns": [],
      "language": "csharp",
      "outputPath": "./generated/graph/csharp",
      "clientClassName": "GraphClient",
      "clientNamespaceName": "Contoso.GraphApp",
      "features": {
        "structuredMediaTypes": [
          "application/json"
        ],
        "usesBackingStore": true,
        "includeAdditionalData": true
      }
    }
  }
}
```

_The resulting `apimanifest.json` file will look like this:_

```jsonc
{
  "apiDependencies": {
    "graphDelegated": {
      "x-ms-apiDescriptionHash": "9EDF8506CB74FE44...",
      "apiDescriptionUrl": "https://raw.githubusercontent.com/microsoftgraph/msgraph-metadata/master/openapi/v1.0/openapi.yaml",
      "apiDeploymentBaseUrl": "https://graph.microsoft.com",
      "apiDescriptionVersion": "v1.0",
      "requests": [
        {
          "method": "GET",
          "uriTemplate": "/users"
        },
        {
          "method": "POST",
          "uriTemplate": "/users"
        },
        {
          "method": "GET",
          "uriTemplate": "/users/$count"
        },
        {
          "method": "GET",
          "uriTemplate": "/users/{user-id}"
        },
        {
          "method": "PATCH",
          "uriTemplate": "/users/{user-id}"
        },
        {
          "method": "DELETE",
          "uriTemplate": "/users/{user-id}"
        }
      ]
    }
  }
}
```

## File structure
```bash
/
 └─.kiota
    └─definitions
       └─graphDelegated.yaml
 └─generated
    └─graph
       └─csharp
          └─... # Generated code files
          └─GraphClient.cs       
 └─apimanifest.json
 └─kiota-config.json 
```