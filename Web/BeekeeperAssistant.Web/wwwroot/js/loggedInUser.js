﻿const apiList = document.querySelector(".api-options");
const beehiveList = document.querySelector(".beehive-options");
const queenList = document.querySelector(".queen-options");
const sidebarArrows = document.querySelectorAll(".primary .fas.fa-angle-left");

const apiButton = document.querySelector(".apiaries-drop-btn");
apiButton.addEventListener("click", (e) => {

    if (apiList.classList.contains("show")) {
        showHideMenu(true, apiList, document.querySelector(".apiaries-drop-btn .fa-angle-left"));
    }
    else {
        showHideMenu(false, apiList, document.querySelector(".apiaries-drop-btn .fa-angle-left"));
    }
})

const beehivesButton = document.querySelector(".beehives-drop-btn");
beehivesButton.addEventListener("click", (e) => {

    if (beehiveList.classList.contains("show")) {
        showHideMenu(true, beehiveList, document.querySelector(".beehives-drop-btn .fa-angle-left"));
    }
    else {
        showHideMenu(false, beehiveList, document.querySelector(".beehives-drop-btn .fa-angle-left"));
    }
})

const queensButton = document.querySelector(".queens-drop-btn");
queensButton.addEventListener("click", (e) => {

    if (queenList.classList.contains("show")) {
        showHideMenu(true, queenList, document.querySelector(".queens-drop-btn .fa-angle-left"));
    }
    else {
        showHideMenu(false, queenList, document.querySelector(".queens-drop-btn .fa-angle-left"));
    }
})

//  Functions
function showHideMenu(addOrRemove, list, arrow) {
    // backgroundToBase();
    removeClass([beehiveList, queenList, apiList], "show");
    removeClass(sidebarArrows, "rotate-270");

    if (addOrRemove) {
        list.classList.remove("show");
        arrow.classList.remove("rotate-270");
    }
    else {
        list.classList.add("show");
        arrow.classList.add("rotate-270");
    }
    // button.style.background = btnBackground;
}

function removeClass(elements, className) {
    if (elements.length == 1) {
        elements.classList.remove(className);
    } else {
        elements.forEach(element => {
            element.classList.remove(className);
        });   
    }
}

var greetMessageElement = document.getElementById("greet-message");
var currentDateTime = new Date();

if (currentDateTime.getHours() < 12) {
    greetMessageElement.innerText = "Добро утро";
} else if (currentDateTime.getHours() < 18) {
    greetMessageElement.innerText = "Добър ден";
}
else {
    greetMessageElement.innerText = "Добър вечер";
}

const dataPicker = Array.from(document.querySelectorAll("#date-picker"));

dataPicker.forEach(date => {
    date.value = currentDateTime.getDate() + '.'
        + ('0' + (currentDateTime.getMonth() + 1)).slice(-2) + '.'
        + ('0' + currentDateTime.getFullYear()).slice(-2);
})
