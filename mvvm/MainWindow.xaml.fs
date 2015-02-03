namespace ViewModels

open System
open System.Windows
open FSharp.ViewModule
open FSharp.ViewModule.Validation
open FsXaml

type MainView = XAML<"MainWindow.xaml", true>

type Score = {
    ScoreA: int
    ScoreB: int
}

type MainViewModel() as self = 
    inherit ViewModelBase()

    let defaultScore = { ScoreA = 0; ScoreB = 0}
    let mutable score = defaultScore

    let scoreA = self.Factory.Backing(<@ self.ScoreA @>, "00")
    let scoreB = self.Factory.Backing(<@ self.ScoreB @>, "00")

    let updateScore() =
        self.ScoreA <- score.ScoreA.ToString("D2")
        self.ScoreB <- score.ScoreB.ToString("D2")

    let incA() = score <- { score with ScoreA = score.ScoreA + 1 }
    let decA() = score <- { score with ScoreA = score.ScoreA - 1 }
    let incB() = score <- { score with ScoreB = score.ScoreB + 1 }
    let decB() = score <- { score with ScoreB = score.ScoreB - 1 }
    let newGame() = score <- defaultScore
    
    let buildCommands (incA, decA, incB, decB, newGame) =
        let commands = [incA; decA; incB; decB; newGame] |> List.map (fun f -> f >> updateScore)
        match commands with
        | [A; B; C; D; E] -> A, B, C, D, E
        | _ -> failwith "Error"

    let (incACommand, decACommand, incBCommand, decBCommand, newGameCommand) = buildCommands(incA, decA, incB, decB, newGame)

    member self.ScoreA with get() = scoreA.Value and set value = scoreA.Value <- value
    member self.ScoreB with get() = scoreB.Value and set value = scoreB.Value <- value

    member self.IncA = self.Factory.CommandSync(incACommand)
    member self.DecA = self.Factory.CommandSync(decACommand)
    member self.IncB = self.Factory.CommandSync(incBCommand)
    member self.DecB = self.Factory.CommandSync(decBCommand)
    member self.NewGame = self.Factory.CommandSync(newGameCommand)
