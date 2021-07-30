var deletedThemeArr = [];
let holiday = [];
let today = new Date();
let row = 1;
let listForUpdate = new Array();
let listNeedUpdate = new Array();
const HEADER = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31]
const TOOLTIPWARNING = '<i class="fas fa-exclamation-circle text-warning error" data-toggle="tooltip" title="合計工数が8h未満です"></i>';
const TOOLTIPDANGER = '<i class="fas fa-exclamation-circle text-danger hour-max" data-toggle="tooltip" title="労働時間の合計が24hを超えることはできません。"></i> ';
const TOOLTIP = '<i class="error" data-toggle="tooltip" title=""></i>';
const ERR_018 = "１日の工数合計が２４ｈを超えることはできません";
const WAR_008 = "テーマが存在します";
const WAR_009 = "総労働時間は> 0時間です";
const WAR_010 = "必須フィールドは空ではありません!";// Required fields are not empty!
//format date return string date 2021/06/18
formatDate = function (date) {
    return date.toISOString().slice(0, 10).replace(/-/g, "/");
}

// get user
$("#groups").change(function users() {
    var group = $("#groups").val();
    $.ajax({
        url: "/ManhourUpdate/GetUser/" + group,
        method: 'Post',
        success: function (result) {
            let user = "";
            $.each(result.data, function (i, v) {
                v.forEach(data => {
                    user += `<option value="${data.value}">${data.text}</option>`;
                })
            })
            $('#users').html(user);
        }
    });
})

// search
async function search() {
    $('#tbody').empty();
    $('#thead').empty();
    $('#tfoot').empty();
    var obj = {};
    var date = $("#month").val();
    obj.Year = date.split('/')[0].toString();
    obj.Month = date.split('/')[1].toString();
    obj.Group = $("#groups").val();
    obj.User = $("#users").val();
    await $.ajax({
        url: "/ManhourUpdate/Search",
        method: 'Post',
        data: obj,
        success: function (result) {
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }
            let tbody = "";
            let tfoot = "";
            holiday = result.data.holiday;
            row = 1;
            // add
            let thead = `<tr>
                            <th></th>
                            <th>テーマNo</th>
                            <th style="width:50px;">テーマ名</th>
                            <th>内容</th>
                            <th>月計</th> `
            for (var i of HEADER) {
                thead += `<th style = "width : 42px;">${i}</th>`
            }
            thead += `<th colspan="2">操作</th> </tr>`
            $('.table-striped > #thead').append(thead);
            var tmp = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0];
            result.data.models.forEach(data => {
                var themeName = data.theme_name1.length < 15 ? `<td class="ThemeName">${data.theme_name1}</td>` : `<td class="ThemeName" data-toggle="tooltip" title="${data.theme_name1}">${data.theme_name2}</td>`;
                tbody += `<tr>
                                <td><div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                                <td class="ThemeNo">${data.theme_no}</td>                               
                                ${themeName}
                                <td class="WContent">${data.work_contents_code}</td>
                                <td class="sum${'row' + row} day0">${data.total.toFixed(1)}</td>
                                <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                <input type="hidden" class="Group_Code" name="Group_Code" value="${data.group_code}" />
                                <input type="hidden" class="Site_Code" name="Site_Code" value="${data.site_code}" />
                                <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                <input type="hidden" class="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}" />
                                <input type="hidden" class="WorkContentCode" name="WorkContentCode" value="${data.work_contents_code}" />
                                <input type="hidden" class="WorkContentDetail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                <input type="hidden" class="pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="Total Total${'row' + row}" name="Total" value="${data.total}" />  `
                for (var i of HEADER) {
                    tbody += `<td class = "${i}"><input type="text" value="${data['day' + i].toFixed(1)}" class="form-control table-input ${'day' + i} ${'row' + row}"></td>`
                }
                row++;
                tbody += `<td><div class="text-center"><i class="fas fa-exchange-alt" onclick="checkHour(this)" ></i></div></td><td >
                             <div class="text-center delete-Theme"><i class="far fa-trash-alt"></i></div></td>
                        </tr >`;
                tmp[0] += data.total;
                for (var i = 1; i < tmp.length; i++) {
                    tmp[i] += data['day' + i];
                }
            });
            $('#tbody').append(tbody);
            // daily total calculation                      
            tfoot = `<tr>
                        <td></td>
                        <td></td>
                        <td>合計</td>
                        <td></td>`
            let day = 0;
            // sum day
            for (var item in tmp) {
                var sum = setSumAndTooltip(tmp[item], day);
                tfoot += `<td class=" ${day} ${'sumday' + day}">${sum}</td>`;
                day++;
            }
            tfoot += ` </tr> 
                      <tr>
                        <td></td>
                        <td></td>
                        <td>残工数</td>
                        <td></td>
                        <td></td>`
            // time difference
            for (var i = 1; i < tmp.length; i++) {
                if (8 - (tmp[i]) < 0 || 8 - (tmp[i]) == 8 || i >= today.getDate() || holiday.find(element => element == i) != undefined) {
                    tfoot += `<td class="${i} ${'missingday' + i}"></td>`
                } else {
                    tfoot += `<td class="${i} ${'missingday' + i}">${(8 - tmp[i]).toFixed(1)}</td> `
                }
            }
            // color holiday                                  
            $('#tfoot').append(tfoot);
            result.data.holiday.forEach(data => {
                $(`.${data}`).css("background-color", "#f5c6cb");
            })
            // color today
            $(`.${today.getDate()}`).css("background-color", "#bee5eb");
            $('#warningCSV').empty();
            // alert <8h 
            checkTotalHours();
        }
    });
}

// Export file CSV
$('#ExportCsv').click(function () {
    var _group = $('#groups').val();
    var _user = $('#users').val();
    var url = "/ManhourUpdate/ExportCSV?user=" + _user + "&group=" + _group;
    $(location).attr('href', url);
});

// Import file CSV
$("#fileCSVimport").on('change', function () {
    var files = $('#fileCSVimport').prop("files");
    var url = "/ManhourUpdate/ImportCSV";
    formData = new FormData();
    formData.append("file", files[0]);
    $('#fileCSVimport').val("");
    jQuery.ajax({
        type: 'POST',
        url: url,
        data: formData,
        contentType: false,
        processData: false,
        success: async function (result) {
            $('#warningCSV').empty();
            if (result.messages == "CSVアップロードが正常終了しました") {
                await search();
                $('#warningCSV').append(`<div class="alert alert-primary mb-2"  role="alert">
                                             CSVアップロードが正常終了しました</div >`);
            }
            else {
                $('#warningCSV').append(`<div class="alert alert-warning mb-2"  role="alert">
                                            <strong> アラート</strong > ${result.messages} `);
            }
        },
        error: function () {
            alert("Error occurs");
        }
    });
});

//save information into DB
$("#btSave").on("click", function () {
    let dayGet = 'day' + new Date().getDate();
    let listData = new Array();
    for (var i = 1; i <= 31; i++) {
        var sum = parseInt($('.sumday' + i)[0].innerText.trim());
        if (sum > 24) {
            return alertGeneral('danger', ERR_018);
        }
    }
    //for each tr in table add to obj then add to list data need saved
    $('#tbody tr').each(function () {
        let obj = {};
        obj.Year = parseInt($(this).find(".Year").val());
        obj.Month = parseInt($(this).find(".Month").val());
        obj.User_no = $(this).find(".User_No").val();
        obj.Group_code = $(this).find(".Group_Code").val();
        obj.Site_code = $(this).find(".Site_Code").val();
        obj.Theme_no = $(this).find(".Theme_No").val();
        obj.Work_contents_class = $(this).find(".WorkContentClass").val();
        obj.Work_contents_code = $(this).find(".WorkContentCode").val();
        obj.Work_contents_detail = $(this).find(".WorkContentDetail").val();
        obj.Pin_flg = false;
        obj.Total = parseFloat($(this).find(".Total").val());
        obj.fix_date = formatDate(new Date()).split('/').join([]);
        //get date value for obj
        for (let i = 1; i < 32; i++) {
            obj[`day${i}`] = parseFloat($(this).find(`.day${i}`).val());
        }
        // Add to list data
        listData.push(obj);
    });
    var data = {};
    data.save = listData;
    data.delete = deletedThemeArr;
    deletedThemeArr = [];
    $.ajax({
        url: "/ManhourUpdate/Save",
        data: JSON.stringify(data),
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //results after saving
            alertGeneral('primary', '- セーブに成功！');
        }
    });

});

//Search theme
$("#searchTheme").on("click",
    function () {
        let soldFlg = "";
        if ($("#inlineRadio1").is(":checked")) {
            soldFlg = "未売上";
        }
        if ($("#inlineRadio2").is(":checked")) {
            soldFlg = "売上済";
        }
        if ($("#inlineRadio3").is(":checked")) {
            soldFlg = "全て";
        }
        let obj = {};
        obj.ThemeNo = $('#themeNo').val() == "" ? null : $('#themeNo').val()
        obj.ThemeName = $('#themeName').val() == "" ? null : $('#themeName').val()
        obj.AccountingGroupCode = $('#groupThemes').val() == "" ? null : $('#groupThemes').val()
        obj.SalesObjectCode = $('#salesObject').val() == "" ? null : $('#salesObject').val()
        obj.SoldFlg = soldFlg
        $.ajax({
            url: `/ManhourUpdate/SearchThemes`,
            method: "POST",
            data: obj,
            success: function (result) {
                if (result.url != undefined) {
                    var origin = window.location.origin;
                    if (origin != undefined && origin != null) {
                        window.location = origin + result.url; //return login view
                        return;
                    }
                }
                let tbody = '';
                result.themes.forEach(data => {
                    tbody +=
                        `<tr>
                                    <td>
                                        <div class="form-check text-center">
                                             <input class="form-check-input position-static radio" name="SelectTheme" type="checkbox" id="Checkbox" value="option1" aria-label="..."/>                                         
                                        </div>
                                    </td>   
                                    <input type="hidden" class ="WorkContentClass" name="WorkContentClass" value="${data.work_contents_class}"/>
                                    <input type="hidden" class ="ThemeNo" name="ThemeNo" value="${data.theme_no}"/>
                                    <input type="hidden" class ="ThemeName" name="ThemeName" value="${data.theme_name1}"/> 
                                    <td>${data.theme_no}</td>
                                    <td width="200px">${data.theme_name1}</td>`
                    tbody += data.sold_flg == true ? `<td>売上済</td></tr >` : `<td>未売上</td></tr >`;
                });
                //render to table body
                $('#slThemeBody').html(tbody);
            }
        });
    });

//make checkbox like radio
$(document).on('click', 'input[type="checkbox"]', function () {
    $('input[type="checkbox"]').not(this).prop('checked', false);
});

let themeNo = null;
let themeName = null;
let workContentClass = null;
let soldFlag = null;

//get information when onclick add theme form checked row
function choiceTheme() {

    $("#slThemeBody tr").each(function () {

        if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
            themeNo = $(this).find(".ThemeNo").val();
            workContentClass = $(this).find(".WorkContentClass").val();
            themeName = $(this).find(".ThemeName").val();

            //load select list by class code
            $(`#workContentCode1`).html('<option>内容選択...</option>');
            $(`#workContentCode2`).html('<option>内容選択...</option>');
            $.ajax({
                url: "/ManhourInput/GetWorkContentByClass",
                data: { classCode: workContentClass },
                type: "GET",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {

                    result.forEach(item => {
                        var option = new Option(`${item.work_contents_code} [${item.work_contents_code_name}]`, `${item.work_contents_code}`);

                        $(`#workContentCode2`).append(option);
                        var option = new Option(`${item.work_contents_code} [${item.work_contents_code_name}]`, `${item.work_contents_code}`);
                        $(`#workContentCode1`).append(option);
                    })
                }

            });

            $('#modal1').modal('hide');
            $('#theme').val(themeNo + '[' + themeName + ']');
            $('#themeSelected2').val(themeNo + '[' + themeName + ']');
        }

    });
}

/* Handle select theme evet*/
$('#workContentDetail').on("change", function () {
    var workDetail = $('#workContentDetail').val();
    if (workDetail.match(/[a-z\ -/]+/gi) || workDetail.length > 2 || workDetail.length < 2 || parseInt(workDetail)<0) {
        $('#workContentDetail').val("");
        $('#warningSave').append(`<div class="alert alert-warning mb-2"  role="alert">
                                            <strong> アラート</strong > - その他の入力値00-99！</div >`)
        return setTimeout(function () {
            $('#warningSave').empty();
        }, 1000);
    }
});

// add Theme in table
$("#addTheme").on("click",
    function () {
        let workContentCode = $('#workContentCode1').val();
        let workContentDetail = $('#workContentDetail').val();
        if (!themeNo || !themeName || !workContentClass || !workContentDetail || !workContentCode) {
            return alertGeneral("warning", WAR_010);
        }
        if (!checkTheme(themeNo)) {
            return alert(WAR_008);
        }
        var rowAdd = '';
        let dateTitle = new Date();
        let year = dateTitle.getFullYear();
        let month = dateTitle.getMonth() + 1;
        rowAdd += `<tr>
                        <td>
                            <div class="text-center"><i class="fas fa-thumbtack" style="color: #D3D3D3;"></div></td>
                        <td class="ThemeNo">${themeNo}</td>
                        <td class="ThemName">${themeName}</td>
                        <td class="WContent">${workContentCode}</td>
                        <td class="sum${'row' + row} day0">0.0</td>
                        <input type="hidden" class="Year"   name="Year" value="${year}" />
                        <input type="hidden" class="Month"  name="Month" value="${month}" />
                        <input type="hidden" class="User_No" name="User_No" value="${$('#users').val()}" />
                        <input type="hidden" class="Group_Code" name="Group_Code" value="${$('#groups').val()}" />
                        <input type="hidden" class="Site_Code" name="Site_Code" value="${siteCode}" />
                        <input type="hidden" class="Theme_No" name="Theme_No" value="${themeNo}" />
                        <input type="hidden" class="WorkContentClass" name="WorkContentClass" value="${workContentClass}" />
                        <input type="hidden" class="WorkContentCode" name="WorkContentCode" value="${workContentCode}" />
                        <input type="hidden" class="WorkContentDetail" name="WorkContentDetail" value="${workContentDetail}" />
                        <input type="hidden" class="pin_flg" name="Pin_flg" value="" />
                        <input type="hidden" class="Total" name="Total" value="0.0" /> `
        for (var i of HEADER) {
            rowAdd += `<td class = "${i}">
                                        <input type="text" value="0.0" class="form-control table-input ${'day' + i} ${'row' + row}">
                                    </td>`

        }
        row++;
        rowAdd += `<td>
                                    <div class="text-center"><i class="fas fa-exchange-alt" onclick="checkHour(this)" ></i></div></td>
                                <td >
                                    <div class="text-center delete-Theme"><i class="far fa-trash-alt"></i></div></td>
                                </tr>`;
        $('#tbody').append(rowAdd);
        holiday.forEach(data => {
            $(`.${data}`).css("background-color", "#f5c6cb");
        })
        var start = new Date();
        // colo today
        $(`.${start.getDate()}`).css("background-color", "#bee5eb");
        //set theme information to null
        themeNo = null; themeName = null; workContentClass = null;
        $('#theme').val("");
    });

// delete 1 row
$("#tbody").on("click", ".delete-Theme", function () {
    var obj = {};
    if (confirm('Do you want to delete this row?')) {
        obj.Year = parseInt($(this).closest('tr').find('.Year').val());
        obj.Month = parseInt($(this).closest('tr').find('.Month').val());
        obj.User_no = $(this).closest('tr').find('.User_No').val();
        obj.Theme_no = $(this).closest('tr').find('.Theme_No').val();
        obj.Work_contents_class = $(this).closest('tr').find('.WorkContentClass').val();
        obj.Work_contents_code = $(this).closest('tr').find('.WorkContentCode').val();
        obj.Work_contents_detail = $(this).closest('tr').find('.WorkContentDetail').val();
        deletedThemeArr.push(obj);
        $(this).closest('tr').remove();
        var sum = 0;
        for (var i = 1; i <= 31; i++) {
            var addressRow = 'day' + i;
            var sumValueRow = 0;
            $('.' + addressRow).each(function () {
                sumValueRow += parseFloat($(this).val());
            });
            sum += sumValueRow;
            var sumAndtooltip = setSumAndTooltip(sumValueRow, i);
            $('.sum' + addressRow).html(sumAndtooltip);

            if (8 - sumValueRow <= 0 || 8 - sumValueRow == 8 || i > today.getDate() || holiday.find(element => element == i) != undefined) {
                $('.missing' + addressRow).html('');
            } else {
                $('.missing' + addressRow).html((8 - sumValueRow).toFixed(1));

            }
        }
        $('.sumday0').html(sum.toFixed(1));
    }
});

function loadSelectTheme() {
    $('#slThemeBody').html('');
    $.ajax({
        url: `/ManhourInput/GetHistoryThemes`,
        method: "POST",
        success: function (result) {
            if (result == null) {
                return;
            }
            //set history value 
            $('#themeNo').val(result.themeNo != null ? result.themeNo : '');
            $('#themeName').val(result.themeName != null ? result.themeName : '')
            $('#groupThemes').val(result.accountingGroupCode != null ? result.accountingGroupCode : '');
            $('#salesObject').val(result.salesObjectCode != null ? result.salesObjectCode : '');
            //checked by sold flag to 
            if (result.soldFlg == "未売上") {
                $("#inlineRadio1").attr("checked", true);
            }
            if (result.soldFlg == "売上済") {
                $("#inlineRadio2").attr("checked", true);
            }
            if (result.soldFlg == "全て") {
                $("#inlineRadio3").attr("checked", true);
            }
        }
    });
    $('#modal1').modal('show');

}

//Update manhour when change hour on client
$("#tbody").on("click", "input", function () {
    $(this).select();
});
let valDayChange = 0;
$(".table-responsive").on("click", ".table-input", function (e) {
    valDayChange = parseFloat($(this).val());
});
$(".table-responsive").on("change", ".table-input", function (e) {
    e.stopPropagation();
    var valueChange = parseFloat($(this).val());
    if (isNaN(valueChange)) {
        valueChange = valDayChange;
        valDayChange = 0;
    }
    if (valueChange > 24) {
        valueChange = valDayChange;
        valDayChange = 0;
        $(this).val(valueChange.toFixed(1));
        return alertGeneral("warning", ERR_018);
    }
    if (valueChange < 0) {
        valueChange = valDayChange;
        valDayChange = 0;
        $(this).val(valueChange.toFixed(1));
        return alertGeneral("warning", WAR_009);
    }
    $(this).val(valueChange.toFixed(1));
    var addressRow;
    var addressCol;
    var sumValueRow = 0;
    var sumValueCol = 0;
    var sumValueMonth = 0;
    var classAll = $(this).attr('class').split(' ');
    // find address column and row.
    $.each(classAll, function (i, className) {
        if (className.indexOf('row') != -1) {
            addressRow = className;
        }
        if (className.indexOf('day') != -1) {
            addressCol = className;
        }
    });
    // calculator sum value row and column.
    $('.' + addressRow).each(function () {
        sumValueRow += parseFloat($(this).val());
    });
    var a = '.sum' + addressRow;
    $('.sum' + addressRow).html(sumValueRow.toFixed(1));
    $('.Total' + addressRow).val(sumValueRow.toFixed(1));
    // change value follow column you choose and change sumValueMonth
    changeValueByColumn(addressCol, sumValueCol, sumValueMonth);
    checkTotalHours();
});

//funtion change manhour one colum
function changeValueByColumn(addressCol, sumValueCol, sumValueMonth) {
    $('.' + addressCol).each(function () {
        sumValueCol += parseFloat($(this).val());
    });
    $('.sum' + addressCol).html(sumValueCol.toFixed(1));

    for (let i = 1; i <= 31; i++) {
        sumValueMonth += parseFloat($('.sumday' + i).html());
    }
    $('.sumday0').html(sumValueMonth.toFixed(1));

    let day = addressCol.match(/[+-]?\d+(.\d+)?/g);
    let dayInt = parseInt(day);
    let checkHoliday = holiday.find(element => element == day);
    // add tooltip
    if ($(".sum" + addressCol).closest("td").hasClass("table-danger") == false) {
        var tooltip = setSumAndTooltip(sumValueCol, dayInt);
        $(".sum" + addressCol).html(tooltip);
    }
    //missing manhour 1 date check manhour > 0 && manhour < 8
    if (8 - sumValueCol <= 0 || 8 - sumValueCol == 8 || dayInt >= today.getDate() || checkHoliday != undefined) {
        $(".missing" + addressCol).html("");
    } else {
        $(".missing" + addressCol).html((8 - sumValueCol).toFixed(1));
    }
}

$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});

function checkHour(el) {
    var sum31Day = $(el).closest("tr").find(".day0").text();
    if (sum31Day == '0.0') {
        handleDialogOK(el);
    }
    else {
        if (confirm("選択中行のテーマを変更します。よろしいですか？") == true) {
            handleDialogOK(el);
        }
    }
}
function handleDialogOK(el) {
    let obj = {};
    obj.Theme_no = $(el).closest("tr").find(".Theme_No").val();
    obj.Work_contents_class = $(el).closest("tr").find(".WorkContentClass").val();
    obj.Work_contents_code = $(el).closest("tr").find(".WorkContentCode").val();
    obj.Work_contents_detail = $(el).closest("tr").find(".WorkContentDetail").val();
    obj.Year = parseInt($(el).closest('tr').find(".Year").val());
    obj.Month = parseInt($(el).closest("tr").find(".Month").val());
    obj.Theme_name = $(el).closest("tr").find(".ThemeName").text();

    $("#modalThemeNo").val(obj.Theme_no);
    $("#modalThemeName").val(obj.Theme_name);
    $("#modalWC").val(obj.Work_contents_code);
    $("#modalDetail").val(obj.Work_contents_detail);
    let paramIn = {
        1: obj,
        2: el
    };
    $("#modal3").val(paramIn);
    $("#modal3").modal('show');
}
$('#btnChange').on('click', function () {
    var arr = Object.values($("#modal3").val());
    let el = arr[1];
    let workContentCode = $(`#workContentCode2 :selected`).val();
    let workContentDetail = $(`#detailCode2`).val();
    //check null value
    if (!themeNo || !themeName || !workContentClass || !workContentDetail || !workContentCode) {
        alert(WAR_010);
        return;
    }
    if (!checkTheme(themeNo)) {
        return alert(WAR_008);
    }
    $(el).closest("tr").find(".ThemeName").text(themeName);
    $(el).closest("tr").find(".ThemeNo").text(themeNo);
    $(el).closest("tr").find(".Theme_No").val(themeNo);
    $(el).closest("tr").find(".WContent").text($(`#workContentCode2 :selected`).val());
    $(el).closest("tr").find(".Detail").text(workContentDetail);
    $("#modal3").modal('hide');
})


// alert <8h 
function checkTotalHours() {
    $('#warningDay').empty();
    for (var i = 1; i <= 31; i++) {
        if (i < today.getDate()) {
            var dayCheck = parseFloat($('.sumday' + i).text().trim());
            if (dayCheck < 8 && holiday.find(element => element == i) == undefined) {
                $('#warningDay').append(`<div class="alert alert-warning mb-2"  role="alert">
                                            <strong> アラート</strong > - 合計工数が8h未満のデータが存在します！</div >`);
                break;
            }
        }
        else {
            break;
        }

    }
}

// add tooltip
function setSumAndTooltip(sum, day) {
    var sumAndtooltip = "";
    if (day == 0) {
        sumAndtooltip = `${sum.toFixed(1)}${TOOLTIP}`
    }
    else if (sum > 24) {
        sumAndtooltip = ` ${sum.toFixed(1)}${TOOLTIPDANGER}`
    }
    else if (sum == 8 || holiday.find(element => element == day) != undefined || day >= today.getDate() || (sum > 8 && sum <= 24)) {
        sumAndtooltip = ` ${sum.toFixed(1)}${TOOLTIP}`
    }
    else if (sum == 0) {
        sumAndtooltip = ` ${sum.toFixed(1)}${TOOLTIPDANGER} `
    }
    else {
        sumAndtooltip = `${sum.toFixed(1)}${TOOLTIPWARNING} `
    }
    return sumAndtooltip;
}

// show error
function alertGeneral(color, notify) {
    $('#warningSave').append(`<div class="alert alert-${color} mb-2"  role="alert">
                                            <strong> アラート </strong > ${notify}！</div >`);
    setTimeout(function () {
        $('#warningSave').empty();
    }, 1000);
}
// check if the theme exists
function checkTheme(theme) {

    var themes = $('.ThemeNo')
    for (var i = 0; i < themes.length; i++) {
        if (themes[i].innerText == theme) {
            return false;
        }
    }
    return true;
}
