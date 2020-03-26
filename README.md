# Giraffe.Antiforgery

Provides support for CSRF token generation and validation using the [Microsoft.AspNetCore.Antiforgery](https://www.nuget.org/packages/Microsoft.AspNetCore.Antiforgery/) package.

## Handlers

### `csrfTokenizer`

Generates a CSRF token using the [Microsoft.AspNetCore.Antiforgery](https://www.nuget.org/packages/Microsoft.AspNetCore.Antiforgery/) package, which is fed into the provided handler.

```f#
csrfTokenizer: (handler : AntiforgeryTokenSet -> HttpHandler) -> (next: HttpFunc) -> (ctx : HttpContext) -> HttpFuncResult
```


### `csrfHtmlView`

Injects a newly generated CSRF token into a Giraffe.GiraffeViewEngine.XmlNode. Think enriched `htmlView`.

```f#
csrfHtmlView: (view : AntiforgeryTokenSet -> XmlNode) -> (next: HttpFunc) -> (ctx : HttpContext) -> HttpFuncResult
```


### `requiresCsrfToken` 

Checks the presence and validity of CSRF token and calls invalidTokenHandler on failure. Analogous to `requiresAuthentication`.

```f#
requiresCsrfToken: (invalidTokenHandler : HttpHandler) -> (next: HttpFunc) -> (ctx : HttpContext) -> HttpFuncResult
```

## Html

### `antiforgeryInput`

Generates the hidden CSRF input using the [Giraffe.GiraffeViewEngine](https://github.com/giraffe-fsharp/Giraffe/blob/master/src/Giraffe/GiraffeViewEngine.fs)

```f#
antiforgeryInput (token : AntiforgeryTokenSet) -> XmlNode
```

> Requires `open Giraffe.GiraffeViewEngine.Antiforgery`

