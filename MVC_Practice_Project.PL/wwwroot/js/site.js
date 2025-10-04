// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.getElementById("SearchInput").addEventListener("keyup", (e) => {
//    console.log(e.target.value)

//    const xhr = new XMLHttpRequest();
    const url = `https://localhost:44302/Employee?SearchInput=${e.target.value}`;
    fetch(url)
        .then(r => r.text())
        .then(html => {
            let parser = new DOMParser();
            let doc = parser.parseFromString(html, "text/html");
            let tablePart = doc.getElementById("EmployeesContainer").innerHTML;
            document.getElementById("EmployeesContainer").innerHTML = tablePart;
        });


})