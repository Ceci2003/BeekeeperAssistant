let beehiveList = document.querySelector(".beehive-options");
let queenList = document.querySelector(".queen-options");
let apiList = document.querySelector(".api-options");

let apiButton = document.querySelector(".apiary-button");
apiButton.addEventListener("click", (e) => {

    if (!apiList.classList.contains("show")) {
        backgrounToBase();
        removeClass([beehiveList, queenList, apiList], "show")
        apiList.classList.add("show");
        apiButton.style.background = "#F0BB00";
    }
    else {
        removeClass([beehiveList, queenList, apiList], "show")
        apiList.classList.remove("show");
        apiButton.style.background = "#463525";
    }
})

let beehiveButton = document.querySelector(".beehive-button");
beehiveButton.addEventListener("click", (e) => {

    if (!beehiveList.classList.contains("show")) {
        backgrounToBase();
        removeClass([beehiveList, queenList, apiList], "show")
        beehiveList.classList.add("show");
        beehiveButton.style.background = "#F0BB00";
    }
    else {
        removeClass([beehiveList, queenList, apiList], "show")
        beehiveList.classList.remove("show");
        beehiveButton.style.background = "#463525";
    }
})


let queenButton = document.querySelector(".queen-button");
queenButton.addEventListener("click", (e) => {

    if (!queenList.classList.contains("show")) {
        backgrounToBase();
        removeClass([beehiveList, queenList, apiList], "show")
        queenList.classList.add("show");
        queenButton.style.background = "#F0BB00";
    }
    else {
        removeClass([beehiveList, queenList, apiList], "show")
        queenList.classList.remove("show");
        queenButton.style.background = "#463525";
    }
})

function removeClass(elements, className) {
    elements.forEach(element => {
        element.classList.remove(className);
    });
}

function backgrounToBase() {
    queenButton.style.background = "#463525";
    beehiveButton.style.background = "#463525";
    apiButton.style.background = "#463525";
}