const addNewRecord = function (myDate, title, status) {
    if (document.getElementById("inputRecTime").value != "") {
        let xhrA = new XMLHttpRequest();

        const body = JSON.stringify({ AppDateTime: myDate, Title: title, Status: status });

        xhrA.open("POST", '/api/records', true);

        xhrA.setRequestHeader('Content-Type', 'application/json');

        xhrA.onload = () => {
            if (xhrA.status == 200) {
                removeRecordRow();
                getUserRecords();
                document.getElementById("inputRecTime").value = "";
                document.getElementById("inputRecText").value = ""
            }
            else {
                alert("Something went wrong! Error " + xhrA.status);
            }
        };

        xhrA.send(body);
    }

}

const sServerOffset = document.querySelector("#serverTimeZoneLabel").innerHTML;
var ServerTimeZone = "";// "+02:00";

if (sServerOffset >= 0) {
    if (sServerOffset < 10) {
        ServerTimeZone = "+0" + sServerOffset + ":00";
    }
    else {
        ServerTimeZone = "+" + sServerOffset + ":00";
    }
}
else {
    if (sServerOffset > -10) {
        ServerTimeZone = "-0" + (-sServerOffset) + ":00";
    }
    else {
        ServerTimeZone = "-" + (-sServerOffset) + ":00";
    }
}

//2022-01-11T11:04:17+00:00
document.getElementById("inputRecSubmit").addEventListener("click", () => {
    const dateArr = document.getElementById("selDateHeader").innerHTML.split(".");

    const myDay = dateArr[2].toString() + "-" + (dateArr[1] < 10 ? "0" + dateArr[1] : dateArr[1]).toString() + "-" + (dateArr[0] < 10 ? "0" + dateArr[0] : dateArr[0]).toString();

    const myTime = document.getElementById("inputRecTime").value;
    const myTitle = document.getElementById("inputRecText").value;

    const myDate = myDay.toString() + "T" + myTime.toString() + ":00" + ServerTimeZone;

    addNewRecord(myDate, myTitle, 0);

});

const addDelClickEvent = function () {
    const delCells = document.getElementsByClassName("recordsCellDel");

    const selDateH = document.getElementById("selDateHeader").innerHTML;

    for (var i = 0; i < delCells.length; i++) {
        const myId = delCells.item(i).getAttribute("id").substring(7); //recordCell

        let delRec = new XMLHttpRequest();

        delCells.item(i).addEventListener("click", () => {

            delRec.open("DELETE", "/api/records/" + myId.toString());

            delRec.onload = () => {
                if (delRec.status == 200) {
                    removeRecordRow();
                    getUserRecords();
                    resetEditTable();
                }
                else {
                    alert("Something went wrong! Error " + delRec.status);
                }
            };
            delRec.send();
        });
    }

}


const editRecord = function (id, myDate, title, description, status) {
    let xhrE = new XMLHttpRequest();

    const body = JSON.stringify({ Id: id, AppDateTime: myDate, Title: title, Description: description, Status: status });

    xhrE.open("PUT", '/api/records/' + id, true);

    xhrE.setRequestHeader('Content-Type', 'application/json');

    xhrE.onload = () => {
        if (xhrE.status == 200) {
            removeRecordRow();
            getUserRecords();
        }
        else {
            alert("Something went wrong! Error " + xhrE.status);
        }
    };

    xhrE.send(body);
}

const resetEditTable = function () {
    document.querySelector("#editIdCell").innerHTML = "";
    document.querySelector("#editDateCell").innerHTML = document.getElementById("selDateHeader").innerHTML;

    document.querySelector('input[name = "editTimeInput"]').value = "";
    document.querySelector('input[name = "editTitleInput"]').value = "Title";
    document.querySelector('textarea[name = "editDescInput"]').value = "Description";

    document.querySelector("#editToStart").removeAttribute("selected");
    document.querySelector("#editInProgress").removeAttribute("selected");
    document.querySelector("#editDone").removeAttribute("selected");

    document.querySelector("#editToStart").setAttribute("selected", "true");
}

document.getElementById("editSaveCell").addEventListener("click", () => {
    const myId = document.getElementById("editIdCell").innerHTML;
    const dateArr = document.getElementById("editDateCell").innerHTML.split(".");

    const myDay = dateArr[2].toString() + "-" + (dateArr[1] < 10 ? "0" + dateArr[1] : dateArr[1]).toString() + "-" + (dateArr[0] < 10 ? "0" + dateArr[0] : dateArr[0]).toString();

    const myTime = document.querySelector('input[name = "editTimeInput"]').value;
    const myTitle = document.querySelector('input[name = "editTitleInput"]').value;
    const myDescription = document.querySelector('textarea[name= "editDescInput"]').value;
    const myStatus = document.getElementById("editStatusInput").selectedIndex;

    const myDate = myDay.toString() + "T" + myTime.toString() + ":00" + ServerTimeZone;

    if (myId != "") { editRecord(myId, myDate, myTitle, myDescription, myStatus); }
});

const colorEditSel = function () {
    const tEditStatus = document.querySelector("#editStatusInput");

    tEditStatus.addEventListener('change', (event) => {

        if (event.target.value == "ToStart") {
            tEditStatus.setAttribute("style", "color:rgba(var(--bs-success-rgb),1)!important");
        }
        else if (event.target.value == "InProgress") {
            tEditStatus.setAttribute("style", "color:rgba(var(--bs-warning-rgb),1)!important");
        }
        else {
            tEditStatus.setAttribute("style", "color:rgba(var(--bs-danger-rgb),1)!important");
        }
    });
}

colorEditSel();