window.onload = hideFilter();

function hideFilter() {
  var filter = document.getElementById("order");
  var column = document.getElementById("column");

  if (filter.selectedIndex == 0) {
    column.classList.add("slide-bck-left");
    column.classList.remove("slide-right");
    
    document.getElementById("input").classList.add("slide-bck-left-btn");
  } else {
    column.classList.remove("slide-bck-left");
    column.classList.add("slide-right");
  }
}
