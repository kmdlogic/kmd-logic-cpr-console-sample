# CPR Client

The CPR client was generated using [Autorest](https://github.com/Azure/autorest) from the included [OpenAPI specification](https://swagger.io/specification/).

```shell
autorest --input-file=cpr.swagger.json --output-folder=Client --namespace=Kmd.Logic.Cpr.ConsoleSample.Client --override-client-name=CprClient --clear-output-folder --csharp --add-credentials
```