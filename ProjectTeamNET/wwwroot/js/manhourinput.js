const ERR_001 = "の対象を選択してください";
const ERR_002 = "出力結果が 1000 件を超えています。検索条件を絞り込んでください"; 
const ERR_005 = "該当データが存在しません";
const ERR_018 = "１日の工数合計が２４ｈを超えることはできません";
const ERR_019 = "番号も24時間入力します";

const WAR_003 = "合計工数が８ｈ末満です";
const WAR_008 = "テーマが存在します"; //theme is existed
const WAR_009 = "未入力のデータが存在します!"; // There is undefine data!
const WAR_010 = "必須フィールドは空ではありません!";// Required fields are not empty!
const WAR_020 = "保存するものはありません!";
const WAR_021 = "保存されていない変更を続行しますか？";
const WAR_022 = "入力番号は00から99の間でなければなりません";
const WAR_023 = "対応するデータが存在しません";

const CONF_001 = "この行を削除しますか？";
const CONF_002 = "選択中行のテーマを変更します。よろしいですか？";
const CONF_003 = "保存前ですが処理を続行しますか？";

const titletooltip24 = "労働時間の合計が24hを超えることはできません";
const titletooltip8 = "合計工数が8h未満です";

//render alert
const WAR_ALERT = "alert-warning d-block";
const SUCCESS = "alert-success d-block";
const DANGER = "alert-danger d-block";
const HEAD_ALERT = "<strong>アラート</strong> - ";
const HEAD_ERROR = "<strong>エラー</strong> - ";

let namePageLoad = $('#pageHistory').val();
let dateTitle = new Date(getDateTitle());
let numDayOfMonth = getDaysInMonth(dateTitle.getFullYear(), dateTitle.getMonth() - 1);
let dateSelect = null;
let themeNo = null;
let themeName = null;
let workContentClass = null;
let listDeleted = new Array();
let listInserted = new Array();
let listNeedUpdate = new Array();
let listForUpdate = new Array();
let holidayArr = new Array();
let dayGetOfWeek = new Array();
let sumByDay = new Array();
let subByDay = new Array();
let eventTableFlag = 0; // catch any event in table

//load init data
$(document).ready(function () {
    loadInit();
});
//render day table and save user screen item when click Day button 
function loadDay() {
    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {
            savePageHistory("Day");
            loadDayTable(dateSelect);
        }
    } else {
        savePageHistory("Day");
        loadDayTable(dateSelect);
    }
}
//render week table and save user screen item when click week button 
function loadWeek() {
    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {
            savePageHistory("Week");
            loadWeekTable(dateSelect);
        }
    } else {
        savePageHistory("Week");
        loadWeekTable(dateSelect);
    }
}
//render month table and save user screen item when click month button 
function loadMonth() {
    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {
            savePageHistory("Month");
            loadMonthTable(dateSelect);
        }
    } else {
        savePageHistory("Month");
        loadMonthTable(dateSelect);
    }
}
//load init data from database
function loadInit() {

    let daypageLoad = getDateTitle();
    if (namePageLoad == "Month") {
        loadMonthTable(daypageLoad);

    } else if (namePageLoad == "Week") {
        loadWeekTable(daypageLoad);

    } else {
        loadDayTable(daypageLoad);
    }
}
//go to previous time
function gotoPrevious() {
    let pageActive = $('#btnNow').text();
    let dateTitle = getDateTitle();
    let prevDate = gotoDate(dateTitle, -1);
    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {

            if (pageActive == '今日') {
                loadDayTable(prevDate);
            }
            if (pageActive == '今週') {
                loadWeekTable(prevDate);
            }
            if (pageActive == '今月') {
                loadMonthTable(gotoMonth(dateTitle, -1));
            }
        }
    } else {
        if (pageActive == '今日') {
            loadDayTable(prevDate);
        }
        if (pageActive == '今週') {
            loadWeekTable(prevDate);
        }
        if (pageActive == '今月') {
            loadMonthTable(gotoMonth(dateTitle, -1));
        }
    }
}
//goto next time
function gotoNext() {

    let pageActive = $('#btnNow').text();
    let dateTitle = getDateTitle();
    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {
            if (pageActive == '今日') {
                loadDayTable(gotoDate(dateTitle, 1));
            }
            if (pageActive == '今週') {
                loadWeekTable(gotoDate(dateTitle, 7));
            }
            if (pageActive == '今月') {
                loadMonthTable(gotoDate(dateTitle, numDayOfMonth));
            }
        }
    } else {
        if (pageActive == '今日') {
            loadDayTable(gotoDate(dateTitle, 1));
        }
        if (pageActive == '今週') {
            loadWeekTable(gotoDate(dateTitle, 7));
        }
        if (pageActive == '今月') {
            loadMonthTable(gotoDate(dateTitle, numDayOfMonth));
        }
    }
}
function gotoNow() {
    let nowDate = formatDate(new Date(Date.now()));

    if (eventTableFlag > 0) {
        if (confirm(WAR_021)) {
            displayLoad(nowDate);
        }
    } else {
        displayLoad(nowDate);
    }
}
function displayLoad(dateStr) {
    let pageName = $('#btnNow').text();
    if (pageName == '今日') {
        loadDayTable(dateStr);
    }
    if (pageName == '今週') {
        loadWeekTable(dateStr);
    }
    if (pageName == '今月') {
        loadMonthTable(dateStr);
    }
}
//change theme selected
function changeTheme(el) {
    if (confirm(CONF_002)) {
        let obj = {};
        obj.Theme_no = $(el).closest("tr").find(".Theme_No").val();
        obj.Work_contents_class = $(el).closest("tr").find(".WC_Class").val();
        obj.Work_contents_code = $(el).closest("tr").find(".WC_Code").val();
        obj.Work_contents_detail = $(el).closest("tr").find(".WC_Detail").val();
        obj.Year = parseInt($(el).closest('tr').find(".Year").val());
        obj.Month = parseInt($(el).closest("tr").find(".Month").val());
        obj.Theme_name = $(el).closest("tr").find(".ThemeName").text();
        handleDialogOK(obj, el);
    }
}
function handleDialogOK(obj, el) {

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
//btn change theme
$('#btnChange').on('click', function () {
    let isStop = false;
    var arr = Object.values($("#modal3").val());
    let oldVal = arr[0];//old data    
    let el = arr[1];
    let workContentCode = $(`#workContentCode2 :selected`).val();
    let workContentDetail = $(`#detailCode2`).val();
    //check null value
    if (!themeNo || !themeName || !workContentClass || !workContentDetail
        || !workContentCode || workContentCode == '内容選択...') {
        createModal('WARNING', WAR_010, 'alert');
        return;
    }
    //check exist in list inserted
    if (listInserted) {
        listInserted.forEach(item => {
            if (item.Theme_no == themeNo) {
                isStop = true;
                return;
            }
        })
    }
    //check exist in list changed
    if (listForUpdate) {
        listForUpdate.forEach(item => {
            if (item.Theme_no == themeNo) {
                isStop = true;
                return;
            }
        })
    }
    //show messge erro
    if (isStop) {
        createModal('WARNING', WAR_008, 'alert');
        return;
    }
    //New data selected
    let data = {};
    data.year = oldVal.Year;
    data.month = oldVal.Month;
    data.Theme_no = themeNo;

    //check exist in db
    ajaxPost(`/ManhourInput/CheckExistTheme`, data).done(function (result) {
        //check session
        if (result.url != undefined) {
            var origin = window.location.origin;
            if (origin != undefined && origin != null) {
                window.location = origin + result.url; //return login view
                return;
            }
        }
        if (result == false) {
            listNeedUpdate.push(oldVal);
            obj = {};
            obj.Theme_no = themeNo;
            obj.Work_contents_class = workContentClass;
            obj.Work_contents_code = workContentCode;
            obj.Work_contents_detail = workContentDetail;

            listForUpdate.push(obj);
            $('#modal3').modal('hide');
            //Change information row changed
            $(el).closest("tr").find(".ThemeName").text(themeName);
            $(el).closest("tr").find(".ThemeNo").text(themeNo);
            $(el).closest("tr").find(".WContent").text($(`#workContentCode2 :selected`).text());
            $(el).closest("tr").find(".Detail").text(workContentDetail);

            $(el).closest("tr").find(".Theme_Name").val(themeName);
            $(el).closest("tr").find(".Theme_No").val(themeNo);
            $(el).closest("tr").find(".WC_Class").val(workContentClass);
            $(el).closest("tr").find(".WC_Code").val(workContentCode);
            $(el).closest("tr").find(".Detail").val(workContentDetail);

            //set default data 
            eventTableFlag = 1;
            $(`#themeSelected1`).val(''); $(`#themeSelected2`).val('');
            $(`#workContentCode1`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#workContentCode2`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#detailCode1`).val(''); $(`#detailCode2`).val('');
            themeNo = null; themeName = null; workContentClass = null;
        } else {
            createModal('WARNING', WAR_008, 'alert');
            $(`#themeSelected1`).val(''); $(`#themeSelected2`).val('');
            $(`#workContentCode1`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#workContentCode2`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#detailCode1`).val(''); $(`#detailCode2`).val('');
            themeNo = null; themeName = null; workContentClass = null;
            return;
        }
    });
});
//render  alert
function renderAlert(type, head, contents) {
    $("#alert").addClass(type);
    $("#alert").html(head + contents);
    setTimeout(function () {
        $("#alert").removeClass(type);
        $("#alert").html('');
    }, 3000);
}
//delete this row clicked 
function deleteTheme(el) {

    if (confirm(CONF_001) == true) {
        $(el).closest('tr').remove();
        let obj = {};

        obj.Year = parseInt($(el).closest('tr').find(".Year").val());
        obj.Month = parseInt($(el).closest('tr').find(".Month").val());
        obj.User_no = $(el).closest('tr').find(".User_No").val();
        obj.Theme_no = $(el).closest('tr').find(".Theme_No").val();
        obj.Work_contents_class = $(el).closest('tr').find(".WC_Class").val();
        obj.Work_contents_code = $(el).closest('tr').find(".WC_Code").val();
        obj.Work_contents_detail = $(el).closest('tr').find(".WC_Detail").val();

        listDeleted.push(obj);
        eventTableFlag = 1;
        //Update total hour for day
        let nowDate = new Date().getDate();
        let sumHourMonth = 0;
        for (let i = 1; i <= numDayOfMonth; i++) {

            sumHourMonth += parseFloat($(el).closest('tr').find(`.day${i}`).val());
            let totalHourDay = $(`.day${i}`).sum();
            let index = isHoliday(holidayArr, i);
            let text = '';
            if (totalHourDay == 0) {//hour working equal 0
                if (i < nowDate && index === -1) { //is not a horliday and less than date now
                    text += `<i  class="fas fa-exclamation-circle text-danger"
                         data-toggle="tooltip" title="${titletooltip8}"></i>${totalHourDay.toFixed(1)}`;
                }

                else { //is horliday or more than date now
                    text += totalHourDay.toFixed(1);
                }
            } else {
                if (totalHourDay < 8 && i < nowDate && index === -1) { //is not a normal working time and less than date now and not a holiday

                    text += `<i class="fas fa-exclamation-circle text-warning"
                        data-toggle="tooltip" title="${titletooltip8}"></i> ${totalHourDay.toFixed(1)}`;

                } else { // is a normal working time

                    text += `<i data-toggle="tooltip" title="${titletooltip8}"></i> ${totalHourDay.toFixed(1)}`;

                }
            }

            $(`#Totalday${i}`).html(text);
            $(`#Totalinputday${i}`).html(text);
            $(`#TotalinputHourday${i}`).html(text);
            if (index === -1) {
                $(`#missHour${i}`).text((8 - totalHourDay) <= 0 ? '' : (8 - totalHourDay) == 8 ? '' : (8 - totalHourDay).toFixed(1));
            }

        }
        //display total hour in month 
        $(el).closest('tr').find(`.total`).text(sumHourMonth.toFixed(1));
        $(el).closest('tr').find(`.Total`).val(sumHourMonth.toFixed(1));
        $('#totalHour').text($('.Total').sum().toFixed(1));


    }

}
//save data change
function save() {
    if (eventTableFlag > 0) {
        if (confirm(CONF_003)) {

            let dayGet = 'day' + new Date(getDateTitle()).getDate();
            let listData = new Array();
            let saveData = {};
            let isStop = false;

            if ($('#tbody tr').length == 0 && listDeleted.length == 0) {
                renderAlert(WAR_ALERT, HEAD_ALERT, WAR_020);
                return;
            }

            //for each tr in table add to obj then add to list data need saved
            $('#tbody tr').each(function () {
                let obj = {};

                //value
                obj.Year = parseInt($(this).find(".Year").val());
                obj.Month = parseInt($(this).find(".Month").val());
                obj.User_no = $(this).find(".User_No").val();
                obj.Theme_no = $(this).find(".Theme_No").val();
                obj.Work_contents_class = $(this).find(".WC_Class").val();
                obj.Work_contents_code = $(this).find(".WC_Code").val();
                obj.Work_contents_detail = $(this).find(".WC_Detail").val();
                obj.pin_flg = $(this).find(".Pin_flg").val() == 'true' ? true : false;
                obj.total = parseFloat($(this).find(".Total").val());

                //convert 2021/06/23 => 20210623 for fixdate varchar(8)
                obj.fix_date = formatDate(new Date()).split('/').join([]);

                //get input hour for month and valid data
                for (let i = 1; i <= numDayOfMonth; i++) {
                    let inputHour = parseFloat($(this).find(`.day${i}`).val());
                    let totalHourOfDay = $(`.day${i}`).sum();

                    if (inputHour < 0 || isNaN(inputHour)) {//valid input hour
                        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_009);
                        isStop = true;
                        return false;
                    }
                    if (totalHourOfDay > 24) {
                        renderAlert(DANGER, HEAD_ERROR, ERR_018);
                        isStop = true;
                        return false;
                    }
                    obj[`day${i}`] = inputHour;

                }

                //get input hour for day
                if ($(this).find(".inputHour").val()) {
                    let inputHour = parseFloat($(this).find(".inputHour").val());
                    let totalHourOfDay = $(".inputHour").sum();
                    if (inputHour < 0 || isNaN(inputHour)) {//valid input hour
                        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_009);
                        isStop = true;
                        return false;
                    }
                    if (totalHourOfDay > 24) {
                        renderAlert(DANGER, HEAD_ERROR, ERR_018);
                        isStop = true;
                        return false;
                    }
                    obj[dayGet] = inputHour;
                }

                //get input hour for week
                if (dayGetOfWeek.length > 0) {
                    dayGetOfWeek.forEach(date => {
                        let inputHour = parseFloat($(this).find(`.input${date}`).val());
                        let totalHourOfDay = $(`.input${date}`).sum();
                        if (inputHour < 0 || isNaN(inputHour)) {//valid input hour
                            renderAlert(WAR_ALERT, HEAD_ALERT, WAR_009);
                            isStop = true;
                            return false;
                        }
                        if (totalHourOfDay > 24) {
                            renderAlert(DANGER, HEAD_ERROR, ERR_018);
                            isStop = true;
                            return false;
                        }
                        obj[`${date}`] = inputHour;
                    })
                }
                // Add to list data
                listData.push(obj);

            });

            if (isStop) {
                return;
            }
            //list manhour change
            saveData.Update = listData;
            listdata = [];
            //list manhour deleted
            saveData.Delete = listDeleted;
            listDeleted = [];
            //list manhour insert
            saveData.Insert = listInserted;
            listInserted = [];
            //list manhour insert
            saveData.NeedUpdate = listNeedUpdate;
            listNeedUpdate = [];
            //list manhour insert
            saveData.ForUpdate = listForUpdate;
            listNeedUpdate = [];
            eventTableFlag = 0;
            $.ajax({
                url: "/ManhourInput/Save",
                data: JSON.stringify(saveData),
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                success: function (result) {
                    //check session
                    if (result.url != undefined) {
                        var origin = window.location.origin;
                        if (origin != undefined && origin != null) {
                            window.location = origin + result.url; //return login view
                            return;
                        }
                    }
                    renderAlert(SUCCESS, '', result)
                    return;
                }
            });
        }
    } else {
        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_020);
    }

}
/* Handle select theme event*/
//load select theme history modal
function loadSelectTheme() {
    $('#slThemeBody').html('');
    $.ajax({
        url: `/ManhourInput/GetHistoryThemes`,
        method: "POST",
        success: function (result) {
            //check session
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }

            //set history value 
            $('#slThemeNo').val(result.themeNo != null ? result.themeNo : '');
            $('#slThemeName').val(result.themeName != null ? result.themeName : '')
            $('#comboxGroup').val(result.accountingGroupCode != null ? result.accountingGroupCode : '');
            $('#comboxObject').val(result.salesObjectCode != null ? result.salesObjectCode : '');

            //checked by sold flag to 
            if (result.soldFlg == "未売上") {
                $("#unsold").attr("checked", true);
            }
            if (result.soldFlg == "売上済") {
                $("#sold").attr("checked", true);
            }
            if (result.soldFlg == "全て") {
                $("#all").attr("checked", true);
            }
        }
    });
    $('#modal1').modal('show');
}
//Search theme
function searchTheme() {

    let soldFlg = "";
    if ($("#unsold").is(":checked")) {
        soldFlg = "未売上";
    }
    if ($("#sold").is(":checked")) {
        soldFlg = "売上済";
    }
    if ($("#all").is(":checked")) {
        soldFlg = "全て";
    }

    let obj = {};
    obj.ThemeNo = $('#slThemeNo').val() == "" ? null : $('#slThemeNo').val();
    obj.ThemeName = $('#slThemeName').val() == "" ? null : $('#slThemeName').val();
    obj.AccountingGroupCode = $('#comboxGroup').val() == "" ? null : $('#comboxGroup').val();
    obj.SalesObjectCode = $('#comboxObject').val() == "" ? null : $('#comboxObject').val();
    obj.SoldFlg = soldFlg;

    ajaxPost(`/ManhourInput/SearchThemes`, obj).done(function (result) {
        //check session
        if (result.url != undefined) {
            var origin = window.location.origin;
            if (origin != undefined && origin != null) {
                window.location = origin + result.url; //return login view
                return;
            }
        }
        if (result.themes.length != 0) {
            if (result.themes.length > 1000) {
                createModal('WARNING', ERR_002, 'alert');
                return;
            }
            let tbody = '';
            result.themes.forEach(data => {
                tbody += `<tr>
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
            $('#slThemeBody').html(tbody)
        } else {
            createModal('WARNING', WAR_023, 'alert');
            $('#slThemeBody').html('')
        }
    });
}
//get information when onclick add theme form checked row
function choiceTheme() {

    $("#slThemeBody tr").each(function () {

        if ($(this).closest('tr').find("input[type=checkbox]").prop('checked')) {
            themeNo = $(this).find(".ThemeNo").val();
            workContentClass = $(this).find(".WorkContentClass").val();
            themeName = $(this).find(".ThemeName").val();

            //load select list by class code
            $(`#workContentCode1`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#workContentCode2`).html('<option disabled selected hidden>内容選択...</option>');
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
            $('#themeSelected1').val(themeNo + '[' + themeName + ']');
            $('#themeSelected2').val(themeNo + '[' + themeName + ']');
        }

    });
}
/*handle select theme evet*/
function ajaxPost(url, sendData) {

    var result = $.ajax({
        type: 'POST',
        url: url,
        data: sendData,
    });

    return result;
}
//add new theme to table
function addTheme() {
    let isStop = false;
    let workcontent = $(`#workContentCode1 :selected`).text();
    let workContentCode = $(`#workContentCode1 :selected`).val();
    let workContentDetail = $(`#detailCode1`).val();
    let dateTitle = new Date(getDateTitle());
    let dayGet = new Date(getDateTitle()).getDate();
    let year = dateTitle.getFullYear();
    let month = dateTitle.getMonth() + 1;

    //valid data not null or empty
    if (!themeNo || !themeName || !workContentClass || !workContentDetail
        || !workContentCode || workContentCode == '内容選択...') {
        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_010);
        return;
    }

    //check existed themes in new theme
    if (listInserted) {
        listInserted.forEach(item => {
            if (item.Theme_no == themeNo) {
                isStop = true;
                return;
            }
        })
    }
    if (isStop) {
        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_008);
        return;
    }

    let obj = {};
    obj.Theme_no = themeNo;
    obj.Year = year;
    obj.Month = month;
    obj.Work_contents_class = workContentClass;
    obj.Work_contents_code = workContentCode;
    obj.Work_contents_detail = workContentDetail;

    ajaxPost(`/ManhourInput/CheckExistTheme`, obj).done(function (result) {
        eventTableFlag = 1;
        //check session
        if (result.url != undefined) {
            var origin = window.location.origin;
            if (origin != undefined && origin != null) {
                window.location = origin + result.url; //return login view
                return;
            }
        }
        if (result == false) { //if thems is not existed in db start add new record
            var rowAdd = '';
            rowAdd += `<tr>
                        <td>
                            <div class="text-center">
                                <i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                            </div>
                        </td>
                            <input type="hidden" class="Year"       name="Year" value="${year}"/>
                            <input type="hidden" class="Month"      name="Month" value="${month}"/>
-                            <input type="hidden" class="Theme_No"   name="ThemeNo" value="${themeNo}"/>
                            <input type="hidden" class="WC_Class"   name="WorkContentClass" value="${workContentClass}"/>
                            <input type="hidden" class="WC_Code"    name="WorkContentCode" value="${workContentCode}"/>
                            <input type="hidden" class="WC_Detail"  name="WorkContentCode" value="${workContentDetail}"/>
                            <input type="hidden" class="pin_flg"    name="Pin_flg" value="${false}" />
                            <input type="hidden" class="Total"      name="Total" value="0.0" />`
            for (let i = 1; i <= numDayOfMonth; i++) {
                rowAdd += `<input type="hidden" name="${'day' + i}" value="0.0" class="${'day' + i}">`
            }
            rowAdd += ` <td class="ThemeNo">${themeNo}</td>
                                <td class="ThemeName">${themeName}</td>`
            let pageActive = $('#btnNow').text();
            //add row for day table
            if (pageActive == '今日') {
                rowAdd += `<td class="WContent">${workcontent}</td>
                            <td class="Detail">${workContentDetail}</td>
                            <td>
                                        <input type="text" onclick = "this.select();setOldVal(this,'${`inputHourday${dayGet}`}');"
                                             onchange = "onChangeValid(this,'inputHourday${dayGet}')"
                                             onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        value="0.0" class="form-control table-big-input inputHourday${dayGet}">
                            </td>
                            <td>
                                <div>
                                    <button class="btn btn-sm btn-outline-secondary mr-2" onclick="changeTheme(this)">
                                                <i class="fas fa-exchange-alt"></i> テーマ変更
                                    </button>
                   
                                   <button class="btn btn-sm btn-outline-secondary mr-2" onclick="deleteTheme(this)">
                                    <i class="far fa-trash-alt"></i> 削除</button>
                                 <div>       
                             </td>
                           </tr>`;
                $('#tbody').append(rowAdd);
            }
            //add row for week table 
            if (pageActive == '今週') {
                rowAdd += `<td class="WContent">${workcontent}</td>
                                <td class="Detail">${workContentDetail}</td>
                                <td class="total">0.0</td>`;
                dayGetOfWeek.forEach(date => {
                    rowAdd += `<td class="${getNumInStr(date)}" >
                                        <input onclick="this.select();setOldVal(this,'${'input' + date}');"
                                        onchange="onChangeValid(this,'input${date}')"
                                        onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122" type="text"
                                        value="0.0" class="form-control table-big-input input${date}">
                                   </td>`
                });

                rowAdd += `<td>
                                    <button class="btn btn-sm mr-2" onclick="changeTheme(this)">
                                                <i class="fas fa-exchange-alt"></i> 
                                    </button>
                                </td>
                                <td>
                                   <button class="btn btn-sm mr-2" onclick="deleteTheme(this)">
                                    <i class="far fa-trash-alt"></i> </button>
                                 <div>       
                             </td>
                           </tr>`;
                $('#tbody').append(rowAdd);
                setHolidayBackground(holidayArr);
            }
            //add row for month table
            if (pageActive == '今月') {
                rowAdd = `<tr>
                        <td>
                            <div class="text-center">
                                <i class="fas fa-thumbtack" style="color: #D3D3D3;"></i>
                            </div>
                        </td>
                            <input type="hidden" class="Year"       name="Year" value="${year}"/>
                            <input type="hidden" class="Month"      name="Month" value="${month}"/>
                            <input type="hidden" class="Theme_No"   name="ThemeNo" value="${themeNo}"/>
                            <input type="hidden" class="WC_Class"   name="WorkContentClass" value="${workContentClass}"/>
                            <input type="hidden" class="WC_Code"    name="WorkContentCode" value="${workContentCode}"/>
                            <input type="hidden" class="WC_Detail"  name="WorkContentCode" value="${workContentDetail}"/>
                            <input type="hidden" class="pin_flg"    name="Pin_flg" value="${false}" />
                            <input type="hidden" class="Total"      name="Total" value="0.0" />
                        <td class="ThemeNo">${themeNo}</td>
                        <td class="ThemeName">${themeName}</td>
                        <td class="Detail">${workContentDetail}</td>
                        <td class="total">0.0</td>`;

                for (let i = 1; i <= numDayOfMonth; i++) {
                    rowAdd += `<td class="${i}">
                                        <input  onclick="this.select();setOldVal(this,'${'day' + i}');" 
                                        onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        onchange="onChangeValid(this,'${'day' + i}')"
                                        type="text" value="0.0" 
                                        class="form-control table-input ${'day' + i}">
                                    </td>`

                }
                rowAdd += `   <td><i onclick="changeTheme(this)" class="fas fa-exchange-alt"></i></td>    
                                  <td><i onclick="deleteTheme(this)" class="far fa-trash-alt"></i></td>
                            </tr>`;
                $('#tbody').append(rowAdd);
                setHolidayBackground(holidayArr);
            }

            let obj = {};
            obj.Year = parseInt(year);
            obj.Month = parseInt(month);
            obj.Theme_no = themeNo;
            obj.Work_contents_class = workContentClass;
            obj.Work_contents_code = workContentCode;
            obj.Work_contents_detail = workContentDetail;

            listInserted.push(obj);
            //set theme information to null and default option
            $(`#themeSelected1`).val(''); $(`#themeSelected2`).val('');
            $(`#workContentCode1`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#workContentCode2`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#detailCode1`).val('');
            $(`#detailCode2`).val('');
            themeNo = null; themeName = null; workContentClass = null;
        }
        else {
            renderAlert(WAR_ALERT, HEAD_ALERT, WAR_008);
            $(`#themeSelected1`).val(''); $(`#themeSelected2`).val('');
            $(`#workContentCode1`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#workContentCode2`).html('<option disabled selected hidden>内容選択...</option>');
            $(`#detailCode1`).val(''); $(`#detailCode2`).val('');
            themeNo = null; themeName = null; workContentClass = null;
        }
    });
}
//sum number in a class
(function ($) {
    $.fn.sum = function () {
        var sum = 0;
        $(this).each(function (index, element) {
            if ($(element).val() != "")
                sum += parseFloat($(element).val());
        });
        return sum;
    };
})(jQuery);
//format date return string date 2021/06/18
function formatDate(date) {
    return date.toLocaleDateString("ja-JP");
}
//get date title
function getDateTitle() {
    return $('#dateTitle').text().trim().substring(0, 10);
}
//previous date
function gotoDate(dateSt, dayNumber) {

    const date = new Date(dateSt);
    const newDate = date.setDate(date.getDate() + dayNumber);
    const nextDate = new Date(newDate).toLocaleDateString("ja-JP");

    return nextDate;
}
//go to month table
function gotoMonth(dateStr, monthNumber) {

    const date = new Date(dateStr);
    const newDate = date.setMonth(date.getMonth() + monthNumber);
    const nextMonth = new Date(newDate).toLocaleDateString("ja-JP");
    return nextMonth;
}
function dayOfWeek(date) {
    return new Date(date).toLocaleString('ja-JP', { weekday: 'long' }).substring(0, 1);
}
//valid detail code in 00-99
function validDetailCode(name) {
    let code = $(`#${name}`).val();
    if (code.length != 2 || code < 0 || code > 99 || isNaN(code)) {
        $(`#${name}`).val('');
        return renderAlert(WAR_ALERT, HEAD_ALERT, WAR_022);
    }
}
//change button active by table render
function changeBtActive() {
    let titlePage = window.location.href.split("/")[4];
    if (titlePage.toLowerCase() == "day") {
        $("#btDay").addClass("active not-allowed");
        $("#btWeek").removeClass("active not-allowed");
        $("#btMonth").removeClass("active not-allowed");
        return;
    }
    if (titlePage.toLowerCase() == "week") {
        $("#btWeek").addClass("active not-allowed");
        $("#btDay").removeClass("active not-allowed");
        $("#btMonth").removeClass("active not-allowed");
        return;
    } else {
        $("#btWeek").removeClass("active not-allowed");
        $("#btDay").removeClass("active not-allowed");
        $("#btMonth").addClass("active not-allowed");
        return;
    }
}
/*handle main table event*/
//render day table by ajax
function loadDayTable(dateStr) {
    //set empty list data to save when change time 
    dayGetOfWeek = [];
    listDeleted = [];
    listInserted = [];
    listNeedUpdate = [];
    listForUpdate = [];
    eventTableFlag = 0;
    if (dateStr == null) {
        dateStr = getDateTitle();
    }
    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //check session
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }
            dateSelect = result.dateSelect;
            //set holiday in an array global
            holidayArr = result.holidays;
            //render thead
            var thead = `<tr>
                                <th style="width:5%" ></th> 
                                <th style="width:15%">テーマNo</th>
                                <th style="width:30%" > テーマ名</th>
                                <th style="width:15%">内容</th>
                                <th style="width:15%">内容詳細</th>
                                <th style="width:10%">工数</th>
                                <th style="width:20%">操作</th> 
                            </tr>`;
            $('#thead').html(thead);
            //render tbody
            let sum = 0;
            let dayGet = 'day' + new Date(result.dateSelect).getDate();
            let tbody = '';
            result.manhourDatas.forEach(data => {
                sum += data[dayGet];
                let themeName = data.theme_name1.length < 20 ?
                    `<td class="ThemeName">${data.theme_name1}</td>`
                    : `<td class="ThemeName" data-toggle="tooltip" title="${data.theme_name1}">${data.theme_name2}</td>`;
                tbody += `<tr> 
                                    <td>
                                        <div class="text-center"> ${data.pin_flg == true ? `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                    </td>
                                    <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                    <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                    <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                    <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                    <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                    <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                    <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                    <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                    <input type="hidden" class="Total" name="Total" value="${data.total}" />`
                for (let i = 1; i <= numDayOfMonth; i++) {
                    tbody += `<input type="hidden" name="${'day' + i}" value="${data['day' + i]}" class="${'day' + i}">`
                }
                tbody += `<td class="ThemeNo">${data.theme_no}</td> 
                                    ${themeName}
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_code_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>

                                    <td>
                                        <input type="text" onclick = "this.select();setOldVal(this,'${`inputHour${dayGet}`}');"
                                             onchange = "onChangeValid(this,'inputHour${dayGet}')"
                                             onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"
                                        value="${data[dayGet].toFixed(1)}" class="form-control table-big-input inputHour${dayGet}">
                                    </td>                                  
                                    <td>
                                        <button class="btn btn-sm btn-outline-secondary mr-2" onclick="changeTheme(this)">
                                        <i class="fas fa-exchange-alt"></i> テーマ変更</button>
                                        <button class="btn btn-sm btn-outline-secondary mr-2" onclick="deleteTheme(this)">
                                        <i class="far fa-trash-alt"></i> 削除</button>
                                    </td>
                            </tr>`;
            })
            $('#tbody').html(tbody);
            //render tfoot
            var tfoot = `<tr><td></td><td></td><td></td><td></td><td>合計</td>`
            let nowDate = new Date().getDate();
            let numDayGet = getNumInStr(dayGet);
            let index = isHoliday(result.holidays, numDayGet);
            if (sum == 0) {
                if (index === -1 && numDayGet < nowDate) { //is not a holiday
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-danger"
                                        data-toggle="tooltip" title="${titletooltip8}">
                                        </i>
                                    ${sum.toFixed(1)}
                                   </td>`;
                }

                else {
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">${sum.toFixed(1)}</td>`;
                }

            }
            else
                if (sum < 8 && numDayGet < nowDate && index === -1) { //is not normal working time and less than now date and not a holiday

                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-warning"
                                          data-toggle="tooltip" title="${titletooltip8}">
                                        </i> ${sum.toFixed(1)}
                                    </td>`
                } else { //is a normal working time
                    tfoot += `<td class="${numDayGet}" id="TotalinputHourday${numDayGet}">${sum.toFixed(1)}</td>`;
                }

            tfoot += `<td></td></tr><tr><td></td><td></td><td></td><td></td><td>不足工数</td>
                      <td id="missHour${getNumInStr(dayGet)}">${(8 - sum) > 0 ? ((8 - sum).toFixed(1) == 8 ? "" : (8 - sum).toFixed(1)) : ""}</td>
                      <td></td>
                      </tr>`;
            $('#tfoot').html(tfoot);

            //change text layout
            $("#dateTitle").text(result.dateSelect + " (" + dayOfWeek(result.dateSelect) + ")");
            window.history.pushState('page2', 'Title', '/ManhourInput/Day');
            changeBtActive();
            $("#btnNow").text('今日');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }

    });

}
//render week table by ajax
function loadWeekTable(dateStr) {
    //set empty list data to save when load
    dayGetOfWeek = [];
    listDeleted = [];
    listInserted = [];
    listNeedUpdate = [];
    listForUpdate = [];
    eventTableFlag = 0;

    if (!dateStr) {
        dateStr = getDateTitle();
    }
    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //check session
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }
            dateSelect = result.dateSelect;
            let dateSelected = result.listDateOfWeek;
            let fromDate = dateSelected[0];
            let toDate = dateSelected[dateSelected.length - 1].substring(8, 10);
            window.history.pushState('page2', 'Title', '/ManhourInput/Week');
            changeBtActive();
            $("#dateTitle").text(fromDate + '-' + toDate);
            $("#btnNow").text('今週');

            holidayArr = result.holidays;
            dayGetOfWeek = [];
            let thead = `<tr> 
                                <th style="width:4%" ></th>
                                <th style="width:10%">テーマNo</th>
                                <th style="width:18%">テーマ名</th>
                                <th style="width:6%">内容</th>
                                <th style="width:6%">内容詳細</th>
                                <th style="width:6%">週計</th>`
            dateSelected.forEach(date => {
                thead += `<th style="width:6%">${date.substring(5, date.length)}-${dayOfWeek(date)}</th>`
                dayGetOfWeek.push("day" + new Date(date).getDate())//day of week [day1,day2]
            })

            thead += `<th style="width:4%" ></th >
                                <th style="width:4%"></th>
                       </tr>`;
            $('#thead').html(thead);

            // set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {
                let themeName = data.theme_name1.length < 20 ?
                    `<td class="ThemeName">${data.theme_name1}</td>`
                    : `<td class="ThemeName" data-toggle="tooltip" title="${data.theme_name1}">${data.theme_name2}</td>`;
                tbody += `<tr>
                                     <td>
                                        <div class="text-center"> ${data.pin_flg == true ? `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                    </td>
                                    <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                    <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                    <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                    <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                    <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                    <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                    <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                    <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                    <input type="hidden" class="Total" name="Total" value="${data.total}" />`

                for (let i = 1; i <= numDayOfMonth; i++) {
                    tbody += `<input type="hidden" name="${'day' + i}" value="${data['day' + i].toFixed(1)}" class="${'day' + i}">`
                }
                tbody += `<td class="ThemeNo">${data.theme_no}</td> 
                                    ${themeName}
                                    <td class="WContent">${data.work_contents_code}:${data.work_contents_code_name}</td>
                                    <td class="Detail">${data.work_contents_detail}</td>
                                    <td class="total">${data.total.toFixed(1)}</td>`

                dayGetOfWeek.forEach(date => {
                    tbody += `<td class="${getNumInStr(date)}">
                                    <input  onclick = "this.select();setOldVal(this,'${'input' + date}');"
                                    onchange = "onChangeValid(this,'input${date}')"
                                    onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? null : event.charCode >= 46 && event.charCode <= 122"  type="text"
                                    value="${data[date].toFixed(1)}" class="form-control table-big-input input${date}"></td>`
                })
                tbody += `          <td>
                                        <button class="btn btn-sm" onclick="changeTheme(this)">
                                            <i class="fas fa-exchange-alt"></i>
                                        </button>
                                   </td>    
                                   <td>
                                        <button class="btn btn-sm" onclick="deleteTheme(this)">
                                        <i class="far fa-trash-alt"></i>
                                        </button>
                                  </td>
                            </tr>`;
            });
            $('#tbody').html(tbody);

            sumByDay = [];
            dayGetOfWeek.forEach(data => {
                sumByDay.push($(`.${data}`).sum());
            })
            // sub by column
            subByDay = [];
            dayGetOfWeek.forEach(data => {
                subByDay.push(8 - $(`.${data}`).sum());
            });

            //set tfoot for table
            let tfoot = `<tr><td></td><td></td><td></td><td></td><td>合計</td><td id="totalHour">${$(`.Total`).sum()}</td>`;

            let dayGet = 0;
            let nowMonth = new Date().getMonth() + 1;
            let month = new Date(getDateTitle()).getMonth() + 1;
            let nowDate = new Date().getDate();
            sumByDay.forEach(item => {
                let numDayGet = getNumInStr(dayGetOfWeek[dayGet]);
                let isHoli = isHoliday(result.holidays, numDayGet);
                if (item == 0) {
                    if (isHoli === -1 && numDayGet < nowDate && month == nowMonth) { //is not a holiday
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-danger"
                                        data-toggle="tooltip" title="${titletooltip8}">
                                        </i>
                                    ${item.toFixed(1)}
                                   </td>`;
                    }

                    else {
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">${item.toFixed(1)}</td>`;
                    }

                }
                else
                    if (item < 8 && numDayGet < nowDate && isHoli === -1) { //is not normal working time

                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">
                                        <i class="fas fa-exclamation-circle text-warning"
                                          data-toggle="tooltip" title="${titletooltip8}">
                                        </i> ${item.toFixed(1)}
                                    </td>`
                    } else { //is a normal working time
                        tfoot += `<td class="${numDayGet}" id="Totalinputday${numDayGet}">${item.toFixed(1)}</td>`;
                    }

                dayGet++;
            });

            // Handle event in tfoot
            tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td></td><td></td><td>不足工数</td><td></td>`
            dayGet = 0;
            subByDay.forEach(item => {
                let numDayGet = getNumInStr(dayGetOfWeek[dayGet]);
                let isHoli = isHoliday(result.holidays, numDayGet);

                if (item <= 0 || isHoli !== -1 || item == 8) {
                    tfoot += `<td id="missHour${numDayGet}"></td>`
                } else {
                    tfoot += `<td class="${numDayGet}" id="missHour${numDayGet}">${item.toFixed(1)}</td>`
                }
                dayGet++;
            });
            tfoot += `<td></td><td></td>/tr>`;
            $('#tfoot').html(tfoot);

            //Set backgorund for horliday
            setHolidayBackground(result.holidays);
        }

    });

}
//check day is a holiday
function isHoliday(holidayArr, num) {
    return holidayArr.findIndex(e => e == num);
}
//return string form datetime
function formatDateToStr(date) {
    var d = new Date(date),
        month = '' + (d.getMonth() + 1),
        day = '' + d.getDate(),
        year = d.getFullYear();

    if (month.length < 2) month = '0' + month;
    if (day.length < 2) day = '0' + day;

    return [year, month, day].join('/');
}
//render manhour month table
function loadMonthTable(dateStr) {

    //set empty list data to save when load 
    dayGetOfWeek = [];
    listDeleted = [];
    listInserted = [];
    listNeedUpdate = [];
    listForUpdate = [];
    eventTableFlag = 0;

    if (!dateStr) {
        dateStr = getDateTitle();
    }
    $.ajax({
        url: "/ManhourInput/LoadDatas",
        data: { dateStr: dateStr },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //check session
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }
            //Change layout information
            let dateGet = new Date(result.dateSelect);
            let firstDay = new Date(dateGet.getFullYear(), dateGet.getMonth(), 1);
            numDayOfMonth = getDaysInMonth(dateGet.getFullYear(), dateGet.getMonth() + 1);
            window.history.pushState('page2', 'Title', '/ManhourInput/Month');
            $("#btnNow").text('今月');
            $("#dateTitle").text(formatDateToStr(firstDay) + "-" + numDayOfMonth);
            changeBtActive();

            dateSelect = result.dateSelect;
            holidayArr = result.holidays;
            //Set thead for table
            let thead = `tr><th></th><th>テーマNo</th><th>テーマ名</th><th>内容</th><th>月計</th>`

            for (let i = 1; i <= numDayOfMonth; i++) {
                thead += `<th>${i}</th>`
            }

            thead += `<th colspan ="2"> 操作</th >
                          </tr>`;
            $('#thead').html(thead);

            //Set tbody for table
            var tbody = '';
            result.manhourDatas.forEach(data => {

                //show compact name if theme_name 1 is too long
                let themeName = data.theme_name1.length < 20 ?
                    `<td class="ThemeName">${data.theme_name1}</td>`
                    : `<td class="ThemeName" data-toggle="tooltip" title="${data.theme_name1}">${data.theme_name2}</td>`;
                tbody += `<tr>
                                <td>
                                     <div class="text-center"> ${data.pin_flg == true ? //display pin icon
                        `<i class="fas fa-thumbtack"></i>` : `<i class="fas fa-thumbtack"  style="color: #D3D3D3;"></i>`}</div>
                                </td>
                                <td class="ThemeNo">${data.theme_no}</td> 
                                ${themeName}
                                <td class="Detail">${data.work_contents_detail}</td>
                                <td class="total">${data.total.toFixed(1)}</td>
                                <input type="hidden" class="Year"   name="Year" value="${data.year}" />
                                <input type="hidden" class="Month"  name="Month" value="${data.month}" />
                                <input type="hidden" class="User_No" name="User_No" value="${data.user_no}" />
                                <input type="hidden" class="Theme_No" name="Theme_No" value="${data.theme_no}" />
                                <input type="hidden" class="WC_Class" name="WorkContentClass" value="${data.work_contents_class}" />
                                <input type="hidden" class="WC_Code" name="WorkContentCode" value="${data.work_contents_code}" />
                                <input type="hidden" class="WC_Detail" name="WorkContentDetail" value="${data.work_contents_detail}" />
                                <input type="hidden" class="Pin_flg" name="Pin_flg" value="${data.pin_flg}" />
                                <input type="hidden" class="Total" name="Total" value="${data.total}" />`

                for (let i = 1; i <= numDayOfMonth; i++) {

                    tbody += `<td class="${i}">
                                    <input  onclick="this.select();setOldVal(this,'${'day' + i}');" 
                                    onkeypress="return (event.charCode == 8 || event.charCode == 0 || event.charCode == 13) ? 
                                    null : event.charCode >= 46 && event.charCode <= 122"
                                    onchange="onChangeValid(this,'${'day' + i}')"
                                    type="text" value="${data['day' + i].toFixed(1)}" 
                                    class="form-control table-input ${'day' + i}">
                             </td>`
                }

                tbody += `   <td><i onclick="changeTheme(this)" class="fas fa-exchange-alt"></i></td>    
                             <td><i class="far fa-trash-alt" onclick="deleteTheme(this)"></i></td>
                          </tr>`;
            })

            $('#tbody').html(tbody);

            // loop by day of month to sum by column
            sumByDay = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                sumByDay.push($('.day' + i).sum());
            }

            // loop by day of month to sub by column
            subByDay = [];
            for (i = 1; i <= numDayOfMonth; i++) {
                subByDay.push(8 - $('.day' + i).sum());
            }

            let tfoot = `<tr><td></td><td></td><td>合計</td><td></td><td id="totalHour">${$('.Total').sum().toFixed(1)}</td>`

            let dayGet = 1;
            let nowDate = new Date().getDate();
            let nowMonth = new Date().getMonth() + 1;
            let month = new Date(getDateTitle()).getMonth() + 1;

            sumByDay.forEach(item => {
                const isHoli = isHoliday(result.holidays, dayGet);

                if (item == 0) {//hour working equal 0
                    if (isHoli === -1 && dayGet < nowDate && month == nowMonth) { //is not a horliday and less than date now
                        tfoot += `<td class="${dayGet}" id="Totalday${dayGet}">
                                        <i  class="fas fa-exclamation-circle text-danger"
                                            data-toggle="tooltip" title="${titletooltip8}">
                                        </i>
                                    ${item.toFixed(1)}
                                   </td>`;
                    }

                    else { //is horliday or more than date now

                        tfoot += `<td class="${dayGet}" id="Totalday${dayGet}"> ${item.toFixed(1)}</td>`;
                    }

                } else if (item < 8 && dayGet < nowDate && isHoli === -1 && month == nowMonth) { //is not a normal working time and less than date now

                    tfoot += `<td class="${dayGet}" id="Totalday${dayGet}">
                                        <i class="fas fa-exclamation-circle text-warning"
                                        data-toggle="tooltip" title="${titletooltip8}"></i> ${item.toFixed(1)}
                                  </td>`
                } else { // is a normal working time

                    tfoot += `<td class="${dayGet}" id="Totalday${dayGet}">${item.toFixed(1)}</td>`;

                }
                dayGet++;
            });

            tfoot += `<td></td><td></td></tr><tr><td></td><td></td><td>残工数</td><td></td><td></td>`
            dayGet = 1;
            subByDay.forEach(item => {
                const isHoli = isHoliday(result.holidays, dayGet);

                if (item <= 0 || isHoli !== -1 || item == 8) {
                    tfoot += `<td class="${dayGet}" id="missHour${dayGet}"></td>`
                } else {
                    tfoot += `<td class="${dayGet}" id="missHour${dayGet}">${item.toFixed(1)}</td>`
                }

                dayGet++;
            });

            tfoot += `<td></td><td></td></tr>`;
            $('#tfoot').html(tfoot)

            //Set backgorund for holiday
            setHolidayBackground(result.holidays);

        }

    });

}
//set background for holiday column
function setHolidayBackground(holidays) {

    holidays.forEach(data => {
        $(`.${data}`).css("background-color", "#f5c6cb");
    })
    //set background for date now
    let now = new Date().getDate();
    let nowmonth = new Date().getMonth() + 1;
    let month = new Date(getDateTitle()).getMonth() + 1;
    if (nowmonth == month) {
        $(`.${now}`).css("background-color", "#bee5eb");
    }
}
//get number in string 
function getNumInStr(str) {
    return str.replace(/^.*?(\d+).*/, '$1');
}
//save page clicked
function savePageHistory(pageName) {
    $.ajax({
        url: "/ManhourInput/SavePageHistory",
        data: { pageName: pageName },
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            //check session
            if (result.url != undefined) {
                var origin = window.location.origin;
                if (origin != undefined && origin != null) {
                    window.location = origin + result.url; //return login view
                    return;
                }
            }
            if (result == false) {
                renderAlert(DANGER, HEAD_ERROR, "エラーが発生しました");
            }
        }
    });
}
//return number day of the month
function getDaysInMonth(year, month) {
    return new Date(year, month, 0).getDate();
}
//show display to show tooltip 
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
})
//cretae alert modal
function createModal(title, message, type) {
    return customModal(title, message, type);
}
function customModal(head, body, type) {
    if (type == 'alert') {
        $('#modal-head').html('<h4 class= "modal-title" style="color:#ffc107">' + head + '</h4 > ');
        $('#modal-body').html('<p>' + body + '</p>');
        $('#modal-footer').html('<button type = "button" class= "btn btn-primary" data-dismiss="modal">OK</button> ');
        $('#custom-modal').modal('show');
    } else if (type == 'confirm') {
        $('#modal-head').html('<h4 class= "modal-title" style="color:#ffc107">' + head + '</h4 > ');
        $('#modal-body').html('<p>' + body + '</p> ');
        $('#modal-footer').html('<button type = "button" class= "btn btn-primary" id ="ok-btn" >OK</button > ' +
            '<button type = "button" class= "btn btn-danger" id="cancel-btn" > Cancel</button>');
        $('#custom-modal').modal('show');
        $('#cancel-btn').on('click', function () {
            $('#custom-modal').modal('hide');
            return false;
        });
        $('#ok-btn').on('click', function () {
            $('#custom-modal').modal('hide');
            return true;
        });
    }
}
//display muilty modal to add z-index
$(document).on('show.bs.modal', '.modal', function () {
    var zIndex = 1040 + (10 * $('.modal:visible').length);
    $(this).css('z-index', zIndex);
    setTimeout(function () {
        $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
    }, 0);
});
//make checkbox like radio
$(document).on('click', 'input[type="checkbox"]', function () {
    $('input[type="checkbox"]').not(this).prop('checked', false);
});
//show date time picker
$('#datepicker').datepicker({
    format: "yyyy/mm/dd",
    language: "ja",
    autoclose: true,
    orientation: 'bottom right'
}).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    var origin = window.location.origin;
    if (origin != undefined && origin != null) {
        window.location = origin + '/ManhourInput/Index?dateSt=' + formatDate(date);
    }
});
//tooltip toogle
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
//onclick to set old value
let oldVal = null;
function setOldVal(el, name) {
    oldVal = $(el).closest('tr').find(`.${name}`).val();
}
//onchange valid value input hour
function onChangeValid(el, name) {
    eventTableFlag = 1;
    let inputHour = parseFloat($(el).closest('tr').find(`.${name}`).val());
    let nowDate = new Date().getDate();
    let dayGet = getNumInStr(name);
    let inputChange = $(el).closest('tr').find(`.${name}`);

    //valid hour input
    if (inputHour < 0 || isNaN(inputHour)) {
        renderAlert(WAR_ALERT, HEAD_ALERT, WAR_009);
        inputChange.val(oldVal); oldVal = null; //not valid set old value after change
        eventTableFlag = 0;
        return;
    }
    if (inputHour > 24) {
        renderAlert(DANGER, HEAD_ERROR, ERR_019);
        inputChange.val(oldVal); oldVal = null;
        eventTableFlag = 0;
        return;
    }
    let totalHourDay = $(`.${name}`).sum(); //sum hour value in class 

    $(el).closest('tr').find(`.day${dayGet}`).val(inputHour.toFixed(1));
    $(el).closest('tr').find(`.inputday${dayGet}`).val(inputHour.toFixed(1));
    $(el).closest('tr').find(`.inputHourday${dayGet}`).val(inputHour.toFixed(1));

    //set vlaue for sum hour by day array by new value
    sumByDay[dayGet - 1] = totalHourDay;

    let isHoli = isHoliday(holidayArr, dayGet);
    let text = '';

    //get text with value of totalHourDay
    if (totalHourDay == 0) {//hour working equal 0

        if (dayGet < nowDate && isHoli === -1) { //is not a horliday and less than date now
            text += `<i  class="fas fa-exclamation-circle text-danger"
                         data-toggle="tooltip" title="${titletooltip8}">
                    </i>${totalHourDay.toFixed(1)}`;
        }

        else { //is horliday or more than date now
            text += totalHourDay.toFixed(1);
        }

    } else {
        if (totalHourDay < 8 && dayGet < nowDate && isHoli === -1) { //is not a normal working time and less than date now and not a holiday

            text += `<i class="fas fa-exclamation-circle  text-warning"
                        data-toggle="tooltip" title="${titletooltip8}">
                    </i> ${totalHourDay.toFixed(1)}`;

        } else
            if (totalHourDay > 24) {
                text += `<i  class="fas fa-exclamation-circle  text-danger"
                         data-toggle="tooltip" title="${titletooltip24}">
                    </i>${totalHourDay.toFixed(1)}`;
            }
            else { // is a normal working time

                text += `${totalHourDay.toFixed(1)}`;

            }
    }

    //dispaly total hour by day
    $(`#Total${name}`).html(text);

    //total hour by month
    let sumHourMonth = 0;
    for (let i = 1; i <= numDayOfMonth; i++) {
        sumHourMonth += parseFloat($(el).closest('tr').find(`.day${i}`).val());
    }

    //change total hour in month  text and val
    $(el).closest('tr').find(`.total`).text(sumHourMonth.toFixed(1));
    $(el).closest('tr').find(`.Total`).val(sumHourMonth.toFixed(1));
    $('#totalHour').text($('.Total').sum().toFixed(1));

    //set miss hour 
    if (isHoli === -1) {
        $(`#missHour${dayGet}`).text((8 - totalHourDay) <= 0 ? '' : (8 - totalHourDay) == 8 ? '' : (8 - totalHourDay).toFixed(1));
    }

}
//export csv
function exportCSV() {
    var url = "/ManhourInput/ExportCSV?dateStr=" + getDateTitle();
    $(location).attr('href', url);
}
