// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.
#r @"..\mbrace.stockticker\bin\debug\mbrace.stockticker.dll"
#load "Library1.fs"
#load @"..\packages\MBrace.Azure.Standalone.0.6.5-alpha\MBrace.Azure.fsx"
open mbrace.stockrunner
open mbrace.stockticker


let mySymbols = [ "MSFT"; "AAPL"; "GOOG" ]

open MBrace.Azure
open MBrace.Azure.Client
open MBrace

let myStorageConnectionString = "..."
let myServiceBusConnectionString = "..."

let cluster = Runtime.GetHandle(MBrace.Azure.Configuration.Default.WithStorageConnectionString(myStorageConnectionString).WithServiceBusConnectionString(myServiceBusConnectionString))
cluster.ShowWorkers()

let work = mySymbols
           |> List.map(fun symbol ->
            cloud {
                let data = StockDataConverter().Run(symbol).Result
                let! file = CloudFile.WriteAllText(data, sprintf "output/%s.json" symbol)
                return file.Path
             })
             |> Cloud.Parallel
             |> cluster.CreateProcess

work.ShowInfo()
work.AwaitResult()

