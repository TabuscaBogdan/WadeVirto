function manageAccount() {
    var email = localStorage.getItem("MusicEmail");
    var token = localStorage.getItem("VirtoUserToken");

    if (!email || !token || email=="undefined") {
        window.location.replace("https://localhost:44372/LoginPage.html");
    }

    var accBtn = document.getElementById("AccManagement");
    accBtn.innerHTML = email;
    accBtn.setAttribute("onClick", "javascript: showJsonData();");;
    //document.getElementById("AccManagement").innerHTML = email;

}
function showJsonData() {
    var rows = document.getElementsByTagName('tr');
    for (row in rows) {
        console.log(rows[row].data);
    }
    //console.log(element.data);
}

//---------------------------------------------------------
function AddElementToList(element,listId) {

    var targetTable = document.getElementById(listId);
    var parentList = element.parentElement;
    parentList.removeChild(element);
    targetTable.appendChild(element);
}
//---------------------------------------------------------
function CreateSongsTable(songs,ids) {
    var table = document.createElement('table');
    table.style = "width:100%; border-spacing:0;";
    table.id = "table" + ids;


    var table_header = document.createElement('tr');

    var table_header_desc1 = document.createElement('th');
    table_header_desc1.innerHTML = "Song Name: "
    table_header.appendChild(table_header_desc1);

    var table_header_desc2 = document.createElement('th');
    table_header_desc2.innerHTML = "Length: "
    table_header.appendChild(table_header_desc2);

    var table_header_desc3 = document.createElement('th');
    table_header_desc3.innerHTML = "Author: "
    table_header.appendChild(table_header_desc3);

    var table_header_desc4 = document.createElement('th');
    table_header_desc4.innerHTML = "Link: "
    table_header.appendChild(table_header_desc4);

    var table_header_desc5 = document.createElement('th');
    table_header_desc5.innerHTML = "Add To List:"
    table_header.appendChild(table_header_desc5);

    var table_header_desc6 = document.createElement('th');
    table_header_desc6.innerHTML = "Click To Add:"
    table_header.appendChild(table_header_desc6);

    table.appendChild(table_header);

    index = 0;
    for (song in songs) {

        var sng = songs[song];

        var table_content = document.createElement('tr');
        table_content.id = "tr" + ids + index;
        index = index + 1;

        

        var table_content_desc1 = document.createElement('td');
        table_content_desc1.innerHTML = sng["dcTitle"];
        table_content.appendChild(table_content_desc1);

        var table_content_desc2 = document.createElement('td');
        table_content_desc2.innerHTML = sng["length"];
        table_content.appendChild(table_content_desc2);

        var table_content_desc3 = document.createElement('td');
        var authorLink = document.createElement('a');
        var mkr = sng["foafMaker"];
        authorLink.href = mkr["id"];
        authorLink.innerHTML = sng["foafMaker"]["foafName"];
        table_content_desc3.appendChild(authorLink);
        table_content.appendChild(table_content_desc3);

        var table_content_desc4 = document.createElement('td');
        var songLink = document.createElement('a');
        songLink.href = sng["id"];
        songLink.innerHTML = "SongLink";
        table_content_desc4.appendChild(songLink);
        table_content.appendChild(table_content_desc4);


        //--------------------List Select-------------------------
        var table_content_list = document.createElement('select');
        var tableLists = document.getElementsByTagName('table');
        for (tbl in tableLists) {
            try {
                var op = document.createElement('option');
                op.value = tableLists[tbl].id;
                op.innerHTML = tableLists[tbl].id.substr(5);
                table_content_list.appendChild(op);
            }
            catch (err) {
                op.innerHTML = "Not Available";
            }
            
        }

        var table_content_desc5 = document.createElement('td');
        table_content_desc5.appendChild(table_content_list);
        table_content.appendChild(table_content_desc5);
        //---------------------------Button----------------------
        var table_content_desc6 = document.createElement('td');
        var addBtn = document.createElement('button');
        addBtn.className = "accBtn";
        addBtn.innerHTML = "Add";
        addBtn.onclick = function () { AddElementToList(table_content, table_content_list.value); };

        table_content_desc6.appendChild(addBtn);

        table_content.appendChild(table_content_desc6);

        //-------------------------------------------------------
        table_content.data = sng;

        table.appendChild(table_content);
    }
    return table;
}

function RenderLists(songLists) {
    var page = document.getElementById("basic-accordian");
    for (var key in songLists) {
        var new_list = document.createElement('div');
        new_list.className = "accordion_headings";
        new_list.innerHTML = key;
        //do more stuff here
        var listOfSongs = songLists[key];
        var songs = listOfSongs["songs"];

        var content_node = document.createElement('div');
        var content_child_node = document.createElement('div');
        content_child_node.className = "accordion_child";

        var header_node = document.createElement('h1');
        header_node.innerHTML = "Song List:";
        content_child_node.appendChild(header_node);

        var renderedTable = CreateSongsTable(songs, key);

        content_child_node.appendChild(renderedTable);
        
        //----
        page.appendChild(new_list);
        page.appendChild(content_child_node);
    }
}

async function GetUserLists() {
    var token = localStorage.getItem("VirtoUserToken");
    const response = await fetch('https://localhost:44316/api/ProcessRequest/Songs?token=' + token);
    const songLists = await response.json();
    console.log(songLists);
    RenderLists(songLists);
}