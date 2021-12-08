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

    var tabs = document.querySelectorAll('.tablinks');
    
    if (tabs.length > 0) {
        var tabsWidth = 0;
        tabs.forEach(tab => {
            tabsWidth += tab.offsetWidth;
        });

        if (tabsWidth >= screenWidth) {
            var tabWidth = screenWidth / tabs.length;
            tabs.forEach(tab => {
                var i = tab.children[0];
                i.classList += ' m0';
                
                tab.innerHTML = i.outerHTML;
                tab.style.width = tabWidth + 'px';
            });
        }
    }
}

if (screenWidth <= 767) {

    // style tab links
    var tabs = document.querySelectorAll('.tablinks');
    
    if (tabs.length > 0) {
        var tabsWidth = 0;
        tabs.forEach(tab => {
            tabsWidth += tab.offsetWidth;
        });

        if (tabsWidth >= screenWidth) {
            var tabWidth = screenWidth / tabs.length;
            tabs.forEach(tab => {
                var i = tab.children[0];
                i.classList += ' m0';

                tab.innerHTML = i.outerHTML;
                tab.style.width = tabWidth + 'px';
            });
        }
    }
}
