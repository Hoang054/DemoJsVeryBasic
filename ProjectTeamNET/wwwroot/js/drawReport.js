var localStorage1 = JSON.parse(sessionStorage.getItem('drawObject'));
var thead = "";
var numberDay = ((new Date(localStorage1[0].toDate)).getTime() - (new Date(localStorage1[0].fromDate)).getTime()) / (1000 * 3600 * 24);

var fromDate = new Date(localStorage1[0].fromDate);
var toDate = new Date(localStorage1[0].toDate);
var columnarr = localStorage1[0].column.split(",");

//add header (has month and day)
$.each(columnarr, function (i, selectedItem) {
    if (selectedItem != "monthlytotal" && selectedItem != "dailytotal") {
        if (selectedItem == "theme") {
            thead += `<th>テーマNo</th><th>テーマ名</th>`;
        }
        else if (selectedItem == "user") {
            thead += `<th>ユーザNo</th><th>ユーザ名</th>`;
        }
        else if (selectedItem == "affiliation") {
            thead += `<th>所属コード</th><th>所属名</th>`;
        }
        else if (selectedItem == "workcontent") {
            thead += `<th>作業内容名</th><th>作業内容コード</th>`;
        }
        else if (selectedItem == "overalltotal") {
            thead += `<th>工数合計</th>`;
        }
        else if (selectedItem == "detailworkcontent") {
            thead += `<th>作業内容詳細</th>`;
        }
    }
    if (numberDay <= 31) {
        if (selectedItem == "monthlytotal") {
            if (fromDate.getMonth() == toDate.getMonth()) {
                thead += `<th>${fromDate.getMonth() + 1}月合計</th>`;
            }
            else if (fromDate.getFullYear() == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>${i}月合計</th>`;
            }
        }
        if (selectedItem == "dailytotal") {
            if (fromDate.getMonth() == toDate.getMonth()) {
                for (let i = fromDate.getDate(); i <= toDate.getDate(); i++)
                    thead += `<th>${i}日合計</th>`;
            }
            else {
                for (let i = fromDate.getDate(); i <= new Date(fromDate.getFullYear(), fromDate.getMonth() + 1, 0).getDate(); i++)
                    thead += `<th>${i}日合計</th>`;
                // total month = 3
                if (toDate.getMonth() - fromDate.getMonth() > 1) {
                    for (let i = 1; i <= new Date(fromDate.getFullYear(), 2, 0).getDate(); i++)
                        thead += `<th>${i}日合計</th>`;
                }

                for (let i = 1; i <= toDate.getDate(); i++)
                    thead += `<th>${i}日合計</th>`;
            }
        }
    }
    else {
        if (selectedItem == "monthlytotal") {
            if (fromDate.getFullYear() == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>${i}月合計</th>`;
            }
            else if (fromDate.getFullYear() + 1 == toDate.getFullYear()) {
                for (let i = fromDate.getMonth() + 1; i <= 12; i++)
                    thead += `<th>${i}月合計</th>`;
                for (let i = 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>${i}月合計</th>`;
            }
            else {
                for (let i = fromDate.getMonth() + 1; i <= 12; i++)
                    thead += `<th>${i}月合計</th>`;
                var year = fromDate.getFullYear() + 1;
                while (year < toDate.getFullYear()) {
                    for (let i = 1; i <= 12; i++) {
                        thead += `<th>${i}月合計</th>`;
                    }
                    year++;
                }
                for (let i = 1; i <= toDate.getMonth() + 1; i++)
                    thead += `<th>${i}月合計</th>`;
            }
        }
    }
})
var tbody = "";
var total = "<td>合計</td>";
var numbertd = 0;

//add content
$.each(localStorage1, function (i, manhourScr) {
    numbertd = 0;
    var content = "";
    $.each(columnarr, function (j, selectedItem) {
        if (selectedItem != "monthlytotal" && selectedItem != "dailytotal") {
            if (selectedItem == "theme") {
                content += `<td>${manhourScr.themeCode}</td><td>${manhourScr.themeName}</td>`;
                numbertd += 2;
            }
            else if (selectedItem == "user") {
                content += `<td>${manhourScr.userCode}</td><td>${manhourScr.userName}</td>`;
                numbertd += 2;
            }
            else if (selectedItem == "detailworkcontent") {
                content += `<td>${manhourScr.workContentDetail}</td>`
                numbertd++;
            }
            else if (selectedItem == "workcontent") {
                content += `<td>${manhourScr.workContentCodeName}</td><td>${manhourScr.workContentCode}</td>`;
                numbertd += 2;
            }
            else if (selectedItem == "affiliation") {
                content += `<td>${manhourScr.groupCode}</td><td>${manhourScr.groupName}</td>`;
                numbertd += 2;
            }
            else if (selectedItem == "overalltotal") {
                content += `<td>${manhourScr.overalltotal}</td>`;
            }
        }
        if (numberDay <= 31) {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < manhourScr.monthly.length; i++) {
                    content += `<td>${manhourScr.monthly[i]}</td>`;
                }
            }
            if (selectedItem == "dailytotal") {
                for (let i = 0; i < manhourScr.daily.length; i++) {
                    content += `<td>${manhourScr.daily[i]}</td>`;
                }
            }
        }
        else {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < manhourScr.monthly.length; i++) {
                    content += `<td>${manhourScr.monthly[i]}</td>`;
                }
            }
        }
    })
    tbody += `<tr>${content}</tr>`;
})

//add total
for (let i = 1; i < numbertd; i++) {
    total += "<td></td>";
}
if (localStorage1[0].total == 1) {
    $.each(columnarr, function (j, selectedItem) {
        if (selectedItem == "overalltotal") {
            var overalltotal = 0;
            $.each(localStorage1, function (index, value) {
                overalltotal += value.overalltotal;
            })
            total += `<td>${overalltotal}</td>`;
        }
        if (numberDay <= 31) {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < localStorage1[0].monthly.length; i++) {
                    var month_total = 0;
                    $.each(localStorage1, function (index, value) {
                        month_total += value.monthly[i];
                    })
                    total += `<td>${month_total}</td>`;
                }
            }
            else if (selectedItem == "dailytotal") {
                for (let i = 0; i < localStorage1[0].daily.length; i++) {
                    var day_total = 0;
                    $.each(localStorage1, function (index, value) {
                        day_total += value.daily[i];
                    })
                    total += `<td>${day_total}</td>`;
                }
            }
        }
        else {
            if (selectedItem == "monthlytotal") {
                for (let i = 0; i < localStorage1[0].monthly.length; i++) {
                    var month_total = 0;
                    $.each(localStorage1, function (index, value) {
                        month_total += value.monthly[i];
                    })
                    total += `<td>${month_total}</td>`;
                }
            }
        }
    })
    tbody += `<tr>${total}</tr>`;
}

$('#draw').append(`
    <table class="table table-striped table-bordered table-sm" style="width:100px; overflow-y: scroll;">
        <thead class="thead-light">
            <tr>
                ${thead}
            </tr>
        </thead>
        <tbody>
            ${tbody}
        </tbody>
    </table>`);