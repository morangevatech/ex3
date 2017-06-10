﻿src = "../page/js/jquery.mazeBoard.js";

$("#rows").val(localStorage.getItem("rows"));
$("#cols").val(localStorage.getItem("cols"));
if (localStorage.getItem("algo") != null) {
    $('#algo').val(localStorage.getItem("algo"));
}

(function ($) {   
    $("#btnGenerateMaze").click(function () {
        var apiUrl = "../../api/Single/GenerateMaze";
        var maze = {
            Name: $("#name").val(),
            Rows: $("#rows").val(),
            Cols: $("#cols").val(),
            InitialPosRow: null,
            InitialPosCol: null,
            GoalPosRow:null,
            GoalPosCol: null,
            MazePath:null
        };
        $.get(apiUrl, { Name:maze.Name, Rows:maze.Rows, Cols:maze.Cols})
        .done(function (maze) {
            /*alert("Name: " + maze.Name +
                "\nCols: " + maze.Cols +
                "\nRows: " + maze.Rows +
                "\nInitialPosRow: " + maze.InitialPosRow +
                "\nInitialPosCol: " + maze.InitialPosCol +
                "\nGoalPosRow: " + maze.GoalPosRow +
                "\nGoalPosCol: " + maze.GoalPosCol +
                "\nMazePath: " + maze.MazePath);*/          
            document.getElementById("mazeCanvas").tabIndex = 0;
            $("#mazeCanvas").mazeBoard(maze);
            $("#canvas-div").show();
            document.getElementById("mazeCanvas").focus();
        })
    })
    })(jQuery);
        var apiUrl = "../../api/Single/SolveMaze";
        var solution = {
        Name: $("#name").val(),
        Algo: $("#algo").val(),
        MazeSolution: null,
    };
    $.get(apiUrl, { Name: solution.Name, Algo: solution.Algo })
    .done(function (solution) {
        alert("Name: " + solution.Name +
             "\nAlgo: " + solution.Algo +
             "\nMazeSolution: " + solution.MazeSolution);
        
    })
});