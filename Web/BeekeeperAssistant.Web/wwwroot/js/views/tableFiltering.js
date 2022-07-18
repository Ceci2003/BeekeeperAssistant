window.onload = hideFilter();

function hideFilter() {
  var filter = document.getElementById("order");
  var column = document.getElementById("column");
  var input = document.getElementById("input");

  if (filter.selectedIndex == 0) {
    column.classList.add("slide-bck-left");
    column.classList.remove("slide-right");

    if (window.innerWidth < 767) {
      input.classList.add("up");
    }
    else {
      input.classList.add("slide-bck-left-btn");
    }
  } else {
    column.classList.remove("slide-bck-left");
    column.classList.add("slide-right");

    if (window.innerWidth < 767) {
      input.classList.remove("up");
    }
    else {
      input.classList.remove("slide-bck-left-btn");
    }
  }
}
