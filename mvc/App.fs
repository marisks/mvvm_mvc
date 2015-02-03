module main

open System
open FsXaml
open ViewModels
open FSharp.Desktop.UI

type App = XAML<"App.xaml">

[<STAThread>]
[<EntryPoint>]
let main argv =
    let app = App().Root

    let model = ScoreModel.Create()
    let view = MainView(MainWindow())
    let controller = MainController()
    let mvc = Mvc(model, view, controller)
    use __ = mvc.Start()

    app.Run(view.Root)