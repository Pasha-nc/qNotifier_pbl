const addDateClickEvent = function () {

    const dateCells = document.querySelectorAll(".calendarCellWeekdays, .calendarCellHolydays");

    for (var i = 0; i < dateCells.length; i++) {

        dateCells.item(i).addEventListener("click", (x) => {

            if (x.srcElement.innerHTML != "") {
                document.getElementById("selDateHeader").innerHTML = x.srcElement.innerHTML + "." + document.getElementById("selectedMonthCell").innerHTML;
                removeRecordRow(); getUserRecords();
            }
        });

    }    
}

const getDatesWithRecords = function () {

    let xhrDR = new XMLHttpRequest();

    const selMonthDR = document.getElementById('selectedMonthCell').innerHTML;

    xhrDR.open("GET", "/calendar/getdateswithrecords/?selectedMonth=" + selMonthDR);

    xhrDR.onload = () => {

        let responseDR = JSON.parse(xhrDR.response);

        document.querySelectorAll('[id*="calendarCell"]').forEach(function (elem) { if (responseDR.includes(Number(elem.innerHTML))) { elem.style.backgroundColor = "#cfff87"; } else { elem.style.backgroundColor = "#ffffff"; } } );
    }
    xhrDR.send();
}

const getCalendarData = function () {

    let xhrM = new XMLHttpRequest();

    const selMonth = document.getElementById('selectedMonthCell').innerHTML;    

    xhrM.open("GET", "/calendar/getcalendardata/?selectedMonth=" + selMonth);

    xhrM.onload = () => {

        let response = JSON.parse(xhrM.response);

        var j = 1;

        const startDay = response.selectedMonthStartingDay;

        const totalDays = response.daysInMonth;

        for (var i = startDay; i < totalDays + startDay; i++) {

            document.querySelector("#calendarCell" + i.toString()).innerHTML = j;

            document.querySelector("#selectedMonthCell").innerHTML = response.selectedMonth.toString() + "." + response.selectedYear.toString();

            j++;
        }

        addDateClickEvent();
        getDatesWithRecords();
    }
    xhrM.send();
}

getCalendarData();


const changeMonth = function (offset) {

    let xhrM = new XMLHttpRequest();

    const selMonth = document.getElementById('selectedMonthCell').innerHTML;

    xhrM.open("GET", "/calendar/changemonth/?selectedMonth=" + selMonth + "&offset=" + offset);

    xhrM.onload = () => {

        let response = JSON.parse(xhrM.response);

        var j = 1;

        const startDay = response.selectedMonthStartingDay;

        const totalDays = response.daysInMonth;

        for (var i = 0; i < 42; i++) {

            if (i >= startDay && i < totalDays + startDay) {

                document.querySelector("#calendarCell" + i.toString()).innerHTML = j;

                j++;
            }
            else {
                document.querySelector("#calendarCell" + i.toString()).innerHTML = "";
            }


            document.querySelector("#selectedMonthCell").innerHTML = response.selectedMonth.toString() + "." + response.selectedYear.toString();
        }
        getDatesWithRecords();
    }
    xhrM.send();
}

document.getElementById('prevMonth').addEventListener("click", () => { changeMonth(-1) });

document.getElementById('nextMonth').addEventListener("click", () => { changeMonth(1) });