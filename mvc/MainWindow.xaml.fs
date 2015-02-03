namespace ViewModels

open System
open System.Windows
open System.Windows.Data

open FSharp.Desktop.UI
open FsXaml
open MahApps.Metro.Controls

type MainWindow = XAML<"MainWindow.xaml", true>

[<AbstractClass>]
type ScoreModel() =
    inherit Model()

    abstract ScoreA: int with get, set
    abstract ScoreB: int with get, set

type ScoringEvents = IncA | DecA | IncB | DecB | New

type MainView(root : MainWindow) as x = 
    inherit View<ScoringEvents, ScoreModel, MetroWindow>(root.Root)

    override x.EventStreams = 
        [
            let buttonClicks =
                [
                    root.IncAButton, IncA
                    root.DecAButton, DecA
                    root.IncBButton, IncB
                    root.DecBButton, DecB
                    root.NewGameButton, New
                ]
                |> List.map (fun (btn, evt) -> btn.Click |> Observable.mapTo evt)
            yield! buttonClicks
        ]

    override x.SetBindings model = 
        let root = MainWindow(x.Root)
        let scoreFormat (s:int) = s.ToString("D2") :> obj // function should return obj in expression
        Binding.OfExpression
            <@
                root.ScoreALabel.Content <- scoreFormat model.ScoreA
                root.ScoreBLabel.Content <- scoreFormat model.ScoreB
            @>

type MainController() =

    interface IController<ScoringEvents, ScoreModel> with

        member x.InitModel model =
            model.ScoreA <- 0
            model.ScoreB <- 0
        
        member x.Dispatcher = function
            | IncA -> Sync x.IncA
            | DecA -> Sync x.DecA
            | IncB -> Sync x.IncB
            | DecB -> Sync x.DecB
            | New -> Sync x.NewGame

    member x.IncA(model : ScoreModel) =
        model.ScoreA <- model.ScoreA + 1
    member x.IncB(model : ScoreModel) =
        model.ScoreB <- model.ScoreB + 1
    member x.DecA(model : ScoreModel) =
        model.ScoreA <- model.ScoreA - 1
    member x.DecB(model : ScoreModel) =
        model.ScoreB <- model.ScoreB - 1
    member x.NewGame(model : ScoreModel) =
        model.ScoreA <- 0
        model.ScoreB <- 0
