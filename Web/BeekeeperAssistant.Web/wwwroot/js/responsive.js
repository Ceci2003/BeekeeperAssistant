var screenWidth = screen.width;
    console.log();

var allTable = document.querySelectorAll('.all-table');
var allResponsiveTable = document.querySelectorAll('.all-table-responsive');
if (screenWidth <= 1023) {
    allTable.forEach(table => {
        table.style.display = 'none';
    });
    allResponsiveTable.forEach(table => {
        table.style.display = 'block'
    });
}
else {
    
}

// window.addEventListener('resize', function(event){
//     var screenWidth = screen.width;

//     var allTable = document.querySelectorAll(".all-table");
//     var allResponsiveTable = document.querySelectorAll(".all-table-responsive");
//     if (screenWidth <= 1023) {
//         console.log(screenWidth);
//         allTable.forEach((table) => {
//             table.style.display = "none";
//         });
//         allResponsiveTable.forEach((table) => {
//             table.style.display = "block";
//         });
//     } else {
//         console.log(screenWidth);
//         allTable.forEach((table) => {
//             table.style.display = "table";
//         });
//         allResponsiveTable.forEach((table) => {
//             table.style.display = "none";
//         });
//     }
// });
