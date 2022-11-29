document.addEventListener('DOMContentLoaded', function () {
    var listView1 = document.querySelector('.list-view-1');
    var listView2 = document.querySelector('.list-view-2');
    var listView3 = document.querySelector('.list-view-3');
    var gridView1 = document.querySelector('.grid-view-1');
    var gridView2 = document.querySelector('.grid-view-2');
    var gridView3 = document.querySelector('.grid-view-3');
    var projectsList1 = document.querySelector('.box-1');
    var projectsList2 = document.querySelector('.box-2');
    var projectsList3 = document.querySelector('.box-3');

    if (listView1 != undefined)
    listView1.addEventListener('click', function () {
        ListView(listView1, gridView1, projectsList1);
    });
    if (listView2 != undefined)
    listView2.addEventListener('click', function () {
        ListView(listView2, gridView2, projectsList2);
    });
    if (listView3 != undefined)
    listView3.addEventListener('click', function () {
        ListView(listView3, gridView3, projectsList3);
    });
    if (gridView1 != undefined)
    gridView1.addEventListener('click', function () {
        GridView(listView1, gridView1, projectsList1);
    });
    if (gridView2 != undefined)
    gridView2.addEventListener('click', function () {
        GridView(listView2, gridView2, projectsList2);
    });
    if (gridView3 != undefined)
    gridView3.addEventListener('click', function () {
        GridView(listView3, gridView3, projectsList3);
    });

    function ListView(listView, gridView, projectsList) {
        gridView.classList.remove('active');
        listView.classList.add('active');
        projectsList.classList.remove('jsGridView');
        projectsList.classList.add('jsListView');
    }
    function GridView(listView, gridView, projectsList) {
        gridView.classList.add('active');
        listView.classList.remove('active');
        projectsList.classList.remove('jsListView');
        projectsList.classList.add('jsGridView');
    }

    document.querySelector('.messages-btn').addEventListener('click', function () {
        document.querySelector('.messages-section').classList.add('show');
    });

    document.querySelector('.messages-close').addEventListener('click', function () {
        document.querySelector('.messages-section').classList.remove('show');
    });
});