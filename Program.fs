open System
open System.IO
open System.Web

open DownKyi.Core.BiliApi.Login
open DownKyi.Core.Utils


[<EntryPoint>]
let main argv =
    Environment.CurrentDirectory <- @"Z:\bili\DownKyi-1.5.9"

    let cookies =
        if false then
            Console.Write("Cookie:")
            Console.ReadLine()
        else
            File.ReadAllText("../cookies.txt")

    let url =
        let uri = UriBuilder("https://fuck-qrcode.org/")
        let query = HttpUtility.ParseQueryString(uri.Query)

        query.Add("Expires", TimeSpan.FromDays(1.0).TotalSeconds.ToString())

        for kv in cookies.Split(Array.singleton "; ", StringSplitOptions.RemoveEmptyEntries) do
            let tmp = kv.Split(Array.singleton '=', 2)
            let value = tmp.[1] |> HttpUtility.UrlDecode
            query.Add(tmp.[0], value)
            printfn $"{tmp.[0]}: {value}"

        uri.Query <- query.ToString()

        uri.ToString()

    for cookie in ObjectHelper.ParseCookie(url).GetCookies(Uri("https://www.bilibili.com")) do
        printfn $"{cookie.Name}:{cookie.Value} -> {cookie.Expires}"

    printfn $"save: {LoginHelper.SaveLoginInfoCookies(url)}"
    printfn $"dump: {LoginHelper.GetLoginInfoCookiesString()}"

    0
