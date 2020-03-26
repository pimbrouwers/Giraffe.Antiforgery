# Giraffe.Antiforgery

[![NuGet Info](https://buildstats.info/nuget/Giraffe.Antiforgery?includePreReleases=true)](https://www.nuget.org/packages/Giraffe.Antiforgery/)

Provides support for CSRF token generation and validation using the [Microsoft.AspNetCore.Antiforgery](https://www.nuget.org/packages/Microsoft.AspNetCore.Antiforgery/) package.

## Getting Started


```f#
open Giraffe.Antiforgery
open Giraffe.GiraffeViewEngine.Antiforgery

// rest of code

let formView (token : AntiforgeryTokenSet) = 
    html [] [
        body [] [
                form [ _method "post" ] [
                        antiforgeryInput token
                        input [ _type "submit"; _value "Submit" ]
                    ]                                
            ]
    ]
	
let csrfHandler (token : AntiforgeryTokenSet) : HttpHandler = 
    fun (next: HttpFunc) (ctx : HttpContext) ->                                
        htmlView (formView token) next ctx

let webApp =
    choose [
        GET >=> choose [
                // using htmlView helper
                route "/token" >=> choose [ 
                        GET  >=> csrfHtmlView formView
                        POST >=> requiresCsrfToken (text "intruder!") >=> text "oh hi there ;)" 
                    ]
                // manual token handler
                route "/token" >=> choose [ 
                    GET  >=> csrfTokenizer csrfHandler
                    POST >=> requiresCsrfToken (text "intruder!") >=> text "oh hi there ;)" 
                ]
                route "/" >=> text "hello" 
            ]
        RequestErrors.NOT_FOUND "Not Found"
    ]

// rest of code
```

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

