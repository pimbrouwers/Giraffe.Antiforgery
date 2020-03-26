module Giraffe.Antiforgery

open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Antiforgery
open Giraffe
open Giraffe.GiraffeViewEngine

// Generates a CSRF token using the Microsoft.AspNetCore.Antiforgery package,
// which is fed into the provided handler
let csrfTokenizer (handler : AntiforgeryTokenSet -> HttpHandler) : HttpHandler =
    fun (next: HttpFunc) (ctx : HttpContext) ->                
        let antiFrg = ctx.GetService<IAntiforgery>()
        (antiFrg.GetAndStoreTokens ctx |> handler) next ctx

// Injects a newly generated CSRF token into a Giraffe.GiraffeViewEngine.XmlNode
let csrfHtmlView (view : AntiforgeryTokenSet -> XmlNode) : HttpHandler =            
    let handler token : HttpHandler =
        fun (next: HttpFunc) (ctx : HttpContext) ->              
            htmlView (view token) next ctx

    csrfTokenizer handler  

// Checks the presence and validity of CSRF token and calls invalidTokenHandler on failure
let requiresCsrfToken (invalidTokenHandler : HttpHandler) : HttpHandler =
    fun (next: HttpFunc) (ctx : HttpContext) ->                                
        let antiFrg = ctx.GetService<IAntiforgery>()
        let valid = antiFrg.IsRequestValidAsync(ctx) |> Async.AwaitTask |> Async.RunSynchronously
        (if valid then next else invalidTokenHandler earlyReturn) ctx
