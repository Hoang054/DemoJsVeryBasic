var check = false;
var countUser = 0;
var GroupName = "<option></option>";
var up = ['user', 'theme', 'workcontent', 'detailworkcontent', 'affiliation'];
var down = ['monthlytotal', 'dailytotal', 'overalltotal'];
var countGroup = 1;
var count = 1;
const ERROR = "Something wrong! Please try later";
$.ajax({
    url: `/ManhourReport/GetsGroupName`,
    method: 'Get',
    success: function (response) {
        $.each(response, function (i, v) {
            GroupName += `<option value="${v.group_code}">${v.groupName}</option>`
        })
    }
});

//init get screenUserItem by userScreenName last action
if ($('#userScreenName > option').length > 1) {
    var idUserScr = [];
    var saveName = $('#userScreenName').val();
    var userScrSelect = $('#userScreenName > option')[1].value;
    var userScr = userScrSelect.substring(0, userScrSelect.length - 14);
    $.each($('#userScreenName > option'), function (index, value) {
        if(index != 0)
            idUserScr.push(parseInt(value.value.substring(value.value.length - 14, value.value.length)))
    });
    $('#userScreenName').val(userScr + Math.max(...idUserScr));
    changeCall();
}

function deleteUserScreenName() {
    var saveName = $('#userScreenName').val();
    if (saveName != "" && saveName != "保存名...") {
        var r = confirm('削除します。よろしいですか？');
        if (r == true) {
            $.ajax({
                url: `/ManhourReport/DeleteScreenUser/` + saveName,
                method: 'Delete',
                success: function (response) {
                    if (response != ERROR) {
                        reset();
                        $('#error').empty();
                        $("#error").append(success(response));
                        GetScreen();
                        $('#SaveName').val("");
                    }
                }
            });
        }
    }
}

//add 1 User follow GroupCode
function addUser(id) {
    var GroupCode = $(`#GroupId${id}`).val();
    if (GroupCode != "") {
        var existUser = [];
        $.each($(`.UserCount${id} option:selected`), function (i, e) {
            existUser.push(e.value);
        });
        $.ajax({
            url: `/ManhourReport/GetsUserName/${GroupCode}`,
            method: 'Get',
            success: function (response) {
                var options = '<option></option>';
                countUser++;
                $.each(response, function (i, v) {
                    if (!existUser.includes(v.userCode))
                        options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
                    else
                        options += (`<option value="${v.userCode}" style="display:none">${v.userCode}[${v.user_Name}]</option>`);
                });
                $(`#addUserinGroup${id}`).append(`<div class="UserCount${id}" style="margin-bottom: 15px; height: 30px" id="addUser${countUser}">
                                    <select class="form-control form-control-sm" id="selectUser${countUser}" onchange="UserChange(${id},${countUser})">
                                        ${options}
                                    </select></div>
                                    `);
                $(`#trashUser${id}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${countUser}del"><i class="far fa-trash-alt" onclick="delUser(addUser${countUser},${id})"></i></div>
                                        `);
            }
        });
        if ($(`.UserCount${id}`).length >= 9) {
            $(`#UserAdd${id}`).attr("hidden", true);
        }
    }
}

async function UserChange(index, countUser) {
    var UserCode = $(`#selectUser${countUser}`).val();
    debugger;
    var existUser = [];
    $.each($(`.UserCount${index} option:selected`), function (i, e) {
        existUser.push(e.value);
    });
    await $.each($(`[id*=selectUser]`), function (i, v) {
        if (v.id != `selectUser${countUser}` && v.closest('div').className == `UserCount${index}`) {
            $.each(v.options, function (index, options) {
                if (existUser.includes(options.value) && v.value != options.value) {
                    options.style.display = 'none';
                }
                else if (v.closest("div").className == `UserCount${index}`) {
                    options.style.display = '';
                }
            })
        }
    })
}
async function GroupChange(index) {
    var GroupCode = $(`#GroupId${index}`).val();
    var existGroup = [];
    $.each($(`[id*=GroupId] option:selected`), function (i, e) {
        existGroup.push(e.value);
    });
    await $.each($(`[id*=GroupId]`), function (i, v) {
        if (v.id != `GroupId${index}`) {
            console.log(v.value)
            $.each(v.options, function (index, options) {
                if (existGroup.includes(options.value) && v.value != options.value) {
                    options.style.display = 'none';
                }
                else {
                    options.style.display = '';
                }
            })
        }
    })
    var options = '<option></option>';
    if (GroupCode != "") {
        $.ajax({
            url: `/ManhourReport/GetsUserName/${GroupCode}`,
            method: 'Get',
            success: function (response) {
                $.each(response, function (i, v) {
                    options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
                });
                var countuser = $(`.UserCount${index}`).length;
                if (countuser == 0) {
                    addUser(index);
                }
                for (let i = 0; i < countuser; i++) {
                    $(`.UserCount${index} select`)[i].innerHTML = options;
                }
            }
        });
        $(`#UserAdd${index}`).removeAttr("hidden");
    }
    else {
        $(`#trashUser${index}`).empty();
        var countuser = $(`.UserCount${index}`).length;
        for (let i = 0; i < countuser; i++) {
            $(`.UserCount${index}`)[0].remove();
        }
        $(`#UserAdd${index}`).attr("hidden", true);
    }
}

//Delete 1 User
function delUser(userId, id) {
    if ($(`.UserCount${id}`).length <= 10) {
        $(`#UserAdd${id}`).removeAttr("hidden");
    }
    $(`#${userId.id}`).remove();
    $(`#${userId.id}del`).remove();
    var existUser = [];
    $.each($(`[class*=UserCount${id}] option:selected`), function (i, e) {
        existUser.push(e.value);
    });
    $.each($(`[id*=selectUser]`), function (i, v) {
        if (v.id != `selectUser${userId.id.match(/\d+/)[0]}`) {
            $.each(v.options, function (index, options) {
                if (existUser.includes(options.value)) {
                    options.style.display = 'none';
                }
                else {
                    options.style.display = '';
                }
            })
        }
    })
}
function delUserFirstGroup(GroupCode) {
    $(`#${GroupCode.id}`).remove();
    $(`#${GroupCode.id}del`).remove();
}

async function addGroup() {
    countGroup++;
    var existGroup = [];
    $.each($('[id*=GroupId] option:selected'), function (i, v) {
        existGroup.push(v.value);
    });
    GroupName = "<option></option>";
    await $.ajax({
        url: `/ManhourReport/GetsGroupName`,
        method: 'Get',
        success: function (response) {
            $.each(response, function (i, v) {
                if (!existGroup.includes(v.group_code))
                    GroupName += `<option value="${v.group_code}">${v.groupName}</option>`;
                else {
                    GroupName += `<option value="${v.group_code}" style="display: none">${v.groupName}</option>`;
                }
            });
        }
    });
    $('#AddGroup').append(`
        <div class="form-group row addGroup" id="Group${countGroup}">
            <label class="col-form-label col-md-2 text-right"></label>
            <div class="col-md-10">
                <div class="row align-items-center" id="addFirst">
                    <div class="col-md-4 input-group pl-0" style="margin-bottom: auto;">
                        <select class="form-control form-control-sm" id="GroupId${countGroup}" onchange="GroupChange(${countGroup})">
                            ${GroupName}
                        </select>
                        <div class="col-md-1"><i class="far fa-trash-alt" onclick="deleteGroup('Group${countGroup}')"></i></div>
                    </div>
                    <div class="col-md-3 input-group pl-0">
                        <div class="countUser" id="addUserinGroup${countGroup}" style="width: 100%"></div>
                        <div class="input-group pl-0 UserAdd" id="UserAdd${countGroup}" hidden>
                            <button class="btn btn-sm btn-outline-secondary mr-2" data-toggle="collapse" data-target="#collapseOne2" onclick="addUser(${countGroup})"><i class="fas fa-plus"></i> ユーザ追加</button>
                        </div>
                    </div>
                    <div class="col-md-4 pl-0" id="firstUser28002000000" style="margin-top:-15px">
                        <div id="trashUser${countGroup}"></div>
                    </div>
                </div>
            </div>
        </div>`);
    if ($('.addGroup').length >= 9) {
        $('#addGroup').attr("hidden", true);
    }
}

function deleteGroup(id) {
    var existGroup = [];
    $.each($(`[id*=GroupId] option:selected`), function (i, e) {
        existGroup.push(e.value);
    });
    $.each($('[id*=GroupId]'), function (i, v) {
        if (v.id != `GroupId${id}`) {
            $.each(v.options, function (index, options) {
                if (existGroup.includes(options.value)) {
                    options.style.display = 'none';
                }
                else {
                    options.style.display = '';
                }
            })
        }
    })
    $(`#${id}`).remove();
    if ($('.addGroup').length < 9) {
        $('#addGroup').removeAttr('hidden');
    }
}

async function addTheme() {
    var theme = "";
    await $.ajax({
        url: `/ManhourReport/GetsThemeName`,
        method: 'Get',
        success: function (response) {
            count++;
            $.each(response, function (i, v) {
                theme += `<option value="${v.themeCode}">${v.theme_Name}</option>`
            });
            
            $.ajax({
                url: `/ManhourReport/AddTheme/${count}`,
                method: 'Get',
                success: function (response) { $('#AddTheme').append(response); }
            });

            $('#AddModal').append(`
                        <div class="modal fade" id="Modal_${count}" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                            <div class="modal-dialog" role="document">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title" id="exampleModalLabel">Modal title</h5>
                                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>
                                    </div>
                                    <div class="modal-body">
                                        <select class="form-control form-control-sm" id="value${count}">${theme}</select>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                                        <button type="button" class="btn btn-primary" data-dismiss="modal" onclick="SaveTheme(${count})">Save changes</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                `);
        }
    });
    if ($('.themeadd').length >= 8) {
        $('#buttonAddTheme').attr("hidden", true);
    }
}

function SaveTheme(id) {
    $(`#theme_${id}`).val($(`#value${id} option:selected`).text());
    var tmp = $(`#value${id}`).val()
    $.ajax({
        url: `/ManhourReport/WorkContents/${tmp}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $(`#addWorkContent_${id}`).empty();
            $.each(response, function (i, v) {
                options += `<option value="${v.workCode}">${v.work_Content}</option>`
            });
            $(`#addWorkContent_${id}`).append(options);
        }
    })
}

function deleteTheme(id) {
    document.getElementById(`Theme_${id}`).remove();
    if ($('.themeadd').length < 9) {
        $('#buttonAddTheme').removeAttr('hidden');
    }
}

//add value option from unselected to selected
function AddSelect() {
    var option = $('#NotSelect option:selected');
    for (var i = 0; i < option.length; i++) {
        var item = option[i].innerText;
        if (item == "全体合計" || item == "月別合計" || item == "日別合計") {
            $('#Select').append(`<option value='${option[i].value}'>${option[i].innerText}</option>`)
        }
        else {
            $('#Select').prepend(`<option value='${option[i].value}'>${option[i].innerText}</option>`);
        }
        option[i].remove();
    }
}

//add value option from unselected to selected
function RemoveSelect() {
    var option = $('#Select option:selected');
    for (var i = 0; i < option.length; i++) {
        $('#NotSelect').append(`<option value='${option[i].value}'>${option[i].innerText}</option>`);
        option[i].remove();
    } 
}

function selectDown() {
    var selectShow = $("#Select").find(":selected");
    var next = selectShow.next("option");
    if (next.length != 0) {
        if (!down.includes(next[0].value) || down.includes(selectShow[0].value))
            next.after(selectShow);
    }    
}

function selectUp() {
    var selectShow = $("#Select").find(":selected");
    var pre = selectShow.prev("option");
    if (pre.length != 0) {
        if (!up.includes(pre[0].value) || up.includes(selectShow[0].value))
            pre.before(selectShow);
    } 
}

// change condition 絞り込み条件 and レイアウト設定 has save in database by 呼出
async function changeCall() {
    $('#error').empty();
    var surrogatekey = $('#userScreenName').val();
    if (surrogatekey != "" && surrogatekey != "保存名...") {
        countUser = 0;
        var numberGroup = 0;
        var group_code = [];
        var number_user_group_code = [];
        var UserNames = [];
        var numberTheme = 0;
        var Themes = [];
        var workContentCode = [];
        var workContentDetail = [];
        var selectedHeaderItems = [];
        var isTotal = 1;
        var typeDelimiter = 1;
        var isSingleQuote = 1;
        var fromDate = '';
        var toDate = '';
        var saveName = '';
        await $.ajax({
            url: `/ManhourReport/GetManhourReport/${surrogatekey}`,
            method: 'Get',
            success: function (response) {
                $.each(response, function (i, v) {
                    if (v.screen_item == "numberGroup") {
                        numberGroup = parseInt(v.screen_input);
                    }
                    else if (v.screen_item.includes('group_') && v.screen_item.length <= 8) {
                        group_code.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('group_') && v.screen_item.length > 8) {
                        UserNames.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('numberUserGroup')) {
                        number_user_group_code.push(parseInt(v.screen_input))
                    }
                    else if (v.screen_item.includes('numberTheme')) {
                        numberTheme = (parseInt(v.screen_input))
                    }
                    else if (v.screen_item.includes('themeNo_')) {
                        Themes.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('workContentCode')) {
                        workContentCode.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('workContentDetail')) {
                        workContentDetail.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('selectedHeaderItem')) {
                        selectedHeaderItems.push(v.screen_input)
                    }
                    else if (v.screen_item.includes('isTotal')) {
                        isTotal = (v.screen_input)
                    }
                    else if (v.screen_item.includes('typeDelimiter')) {
                        typeDelimiter = (v.screen_input)
                    }
                    else if (v.screen_item.includes('isSingleQuote')) {
                        isSingleQuote = (v.screen_input)
                    }
                    else if (v.screen_item.includes('fromDate')) {
                        fromDate = (v.screen_input)
                    }
                    else if (v.screen_item.includes('toDate')) {
                        toDate = (v.screen_input)
                        saveName = (v.save_name)
                    }
                });
            }
        });
        count = 1; countGroup++;
        $('#addUserinGroup1').empty();
        $('#trashUser1').empty();
        $('#AddGroup').empty();
        $('#AddTheme').empty();

        $('#fromDate').val(fromDate);
        $('#toDate').val(toDate);
        $('#SaveName').val(saveName);
        $('#GroupId').val(group_code[0]);

        countGroup = 1;
        //get users first group
        for (let i = 0; i < number_user_group_code[0]; i++) {
            countUser++;
            await GetsUser(countGroup, group_code[0], UserNames[countUser - 1], countUser);
            await UserChange(countGroup, countUser);
            $('#UserAdd1').removeAttr("hidden");
        }
        // Get value first group
        $(`#GroupId1`).val(group_code[0]);

        // add value group Gets value group in db
        if (numberGroup > 1) {
            for (let i = 1; i < numberGroup; i++) {
                await getGroup();
                await GroupChange(countGroup);
                $(`#GroupId${i + 1}`).val(group_code[i]);
                for (let j = 0; j < number_user_group_code[i]; j++) {
                    countUser++;
                    await GetsUser(countGroup, group_code[i], UserNames[countUser - 1], countUser);
                    await UserChange(countGroup, countUser);
                }
            }
            GroupChange(countGroup);
        }
        // add theme and Gets value theme in db
        for (let i = 0; i < numberTheme; i++) {
            if (i > 0)
                await addTheme();
            await save(i + 1, Themes[i].substring(0, 10))
            $(`#theme_${i + 1}`).val(Themes[i]);
            $(`#addWorkContent_${i + 1}`).val(workContentCode[i])
            $(`#addWorkDetail_${i + 1}`).val(workContentDetail[i])
        }

        $('#Select').empty();
        $('#NotSelect').empty();
        $('#NotSelect').append(`<option value="affiliation">所属</option>
                            <option value="user">ユーザ</option>
                            <option value="theme">テーマ</option>
                            <option value="workcontent">作業内容</option>
                            <option value="detailworkcontent">作業内容詳細</option>
                            <option value="overalltotal">全体合計</option>
                            <option value="monthlytotal">月別合計</option>
                            <option value="dailytotal">日別合計</option>
                            `);
        $.each(selectedHeaderItems, function (i, v) {
            $('#NotSelect').val(v);
            AddSelect();
        });
        if (isTotal == 0) {
            $('#isTotal1').prop('checked', false);
            $('#isTotal').prop('checked', true);
        }
        if (typeDelimiter == 0) {
            $('#typeDelimiter1').prop('checked', false);
            $('#typeDelimiter').prop('checked', true);
        }
        if (isSingleQuote == 0) {
            $('#isSingleQuote1').prop('checked', false);
            $('#isSingleQuote').prop('checked', true);
        }
    }
}

// get user by groupcode
async function GetsUser(groupCount, group_code, UserNo, count_user) {
    var existUser = [];
    $.each($(`.UserCount${groupCount} option:selected`), function (i, e) {
        existUser.push(e.value);
    });
    await $.ajax({
        url: `/ManhourReport/GetsUserName/${group_code}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $.each(response, function (i, v) {
                if (!existUser.includes(v.userCode)) {
                    options += (`<option value="${v.userCode}">${v.userCode}[${v.user_Name}]</option>`);
                }
                else{
                    options += (`<option value="${v.userCode}" style="display:none">${v.userCode}[${v.user_Name}]</option>`);
                }
            });
            $(`#addUserinGroup${groupCount}`).append(`<div class="UserCount${groupCount}" style="margin-bottom: 15px; height: 30px" id="addUser${count_user}">
                                    <select id="selectUser${countUser}" class="form-control form-control-sm" onchange="UserChange(${groupCount},${count_user})">
                                        ${options}
                                    </select></div>
                                    `);
            $(`#trashUser${groupCount}`).append(`<div style="margin-bottom: 15px; height: 30px" id="addUser${count_user}del"><i class="far fa-trash-alt" onclick="delUser(addUser${count_user})"></i></div>
                                        `);
        }
    });
    $(`#addUser${count_user} > select`).val(UserNo);
}

//get group available in data
async function getGroup() {
    countGroup++;
    var existGroup = [];
    $.each($('[id*=GroupId] option:selected'), function (i, v) {
        existGroup.push(v.value);
    });
    GroupName = "<option></option>";
    await $.ajax({
        url: `/ManhourReport/GetsGroupName`,
        method: 'Get',
        success: function (response) {
            $.each(response, function (i, v) {
                if (!existGroup.includes(v.group_code))
                    GroupName += `<option value="${v.group_code}">${v.groupName}</option>`;
                else {
                    GroupName += `<option value="${v.group_code}" style="display: none">${v.groupName}</option>`;
                }
            });
        }
    });
    $('#AddGroup').append(`
        <div class="form-group row addGroup" id="Group${countGroup}">
            <label class="col-form-label col-md-2 text-right"></label>
            <div class="col-md-10">
                <div class="row align-items-center" id="addFirst">
                    <div class="col-md-4 input-group pl-0" style="margin-bottom: auto;">
                        <select class="form-control form-control-sm" id="GroupId${countGroup}" onchange="GroupChange(${countGroup})">
                            ${GroupName}
                        </select>
                        <div class="col-md-1"><i class="far fa-trash-alt" onclick="deleteGroup('Group${countGroup}')"></i></div>
                    </div>
                    <div class="col-md-3 input-group pl-0">
                        <div class="countUser" id="addUserinGroup${countGroup}" style="width: 100%"></div>
                        <div class="input-group pl-0 UserAdd" id="UserAdd${countGroup}">
                            <button class="btn btn-sm btn-outline-secondary mr-2" data-toggle="collapse" data-target="#collapseOne2" onclick="addUser(${countGroup})"><i class="fas fa-plus"></i> ユーザ追加</button>
                        </div>
                    </div>
                    <div class="col-md-4 pl-0" id="firstUser28002000000" style="margin-top:-15px">
                        <div id="trashUser${countGroup}"></div>
                    </div>
                </div>
            </div>
        </div>`);
    if ($('.addGroup').length >= 9) {
        $('#addGroup').attr("hidden", true);
    }
}

// get list workcontents follow theme db
async function save(id, workcontent) {
    await $.ajax({
        url: `/ManhourReport/WorkContents/${workcontent}`,
        method: 'Get',
        success: function (response) {
            var options = '';
            $(`#addWorkContent_${id}`).empty();
            $.each(response, function (i, v) {
                options += `<option value="${v.workCode}">${v.work_Content}</option>`
            });
            $(`#addWorkContent_${id}`).append(options);
        }
    })
}

//check validate and save sessionStorage
function checkform() {
    $('#error').empty();
    var Obj = setObj();
    $.ajax({
        url: '/ManhourReport/CheckReport',
        method: 'POST',
        data: Obj,
        success: function (response) {
            $('#savename').empty();
            $('#toDate1').empty();
            $('#fromDate1').empty();
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/CreateData',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        var Object = [];
                        if (response.messenge == "") {
                            $.each(response.data, function (i, v) {
                                var objtmp = {};
                                objtmp.groupCode = v.groupCode;
                                objtmp.groupName = v.groupName;
                                objtmp.userCode = v.userCode;
                                objtmp.userName = v.userName;
                                objtmp.themeCode = v.themeCode;
                                objtmp.themeName = v.themeName;
                                objtmp.workContentCode = v.workContentCode;
                                objtmp.workContentCodeName = v.workContentCodeName;
                                objtmp.workContentDetail = v.workContentDetail;
                                objtmp.monthly = v.monthly;
                                objtmp.daily = v.daily;
                                objtmp.fromDate = v.fromDate;
                                objtmp.toDate = v.toDate;
                                objtmp.overalltotal = v.overalltotal;
                                objtmp.column = Obj.selectedHeaderItems;
                                objtmp.total = $("input[name='isTotal']:checked").val();
                                Object.push(objtmp)
                            });
                            sessionStorage.setItem('drawObject', JSON.stringify(Object));
                            window.open('/ManhourReport/ShowReport', '_blank');
                        }
                        else {
                            $("#error").append(error(response.messenge));
                        }
                    }
                });
            }
            else {
                if (response.toDate != "") {
                    $("#error").append(error(response.toDate));
                }
                else if (response.fromDate != "") {
                    $("#error").append(error(response.fromDate));
                }
                else if (response.date != "") {
                    $("#error").append(error(response.date));
                }
                else if (response.dateCalculate != "") {
                    $("#error").append(error(response.dateCalculate));
                }
                else if (response.user != "") {
                    $("#error").append(error(response.user));
                }
                else if (response.theme != "") {
                    $("#error").append(error(response.theme));
                }
                else if (response.headerItems != "") {
                    $("#error").append(error(response.headerItems));
                }
            }
        }
    })
}

//Get 絞り込み条件 AND レイアウト設定
function setObj() {
    $('#error').empty();
    var Obj = {} || data1;
    Obj.save = $("#SaveName").val();
    Obj.fromDate = $("#fromDate").val();
    Obj.toDate = $("#toDate").val();
    Obj.isTotal = $(`input[name=isTotal]:checked`).val();
    Obj.typeDelimiter = $(`input[name=typeDelimiter]:checked`).val();
    Obj.isSingleQuote = $(`input[name=isSingleQuote]:checked`).val();
    Obj.numberSelectedHeader = $('#Select option').length;
    Obj.selectedHeaderItems = "";
    for (let i = 0; i < $('#Select option').length; i++) {
        if (i == 0)
            Obj.selectedHeaderItems += $('#Select option')[i].value;
        else
            Obj.selectedHeaderItems += "," + $('#Select option')[i].value;
    }
    if ($("#theme_1").val() == "")
        Obj.numberTheme = 0;
    else {
        Obj.numberTheme = $('#AddTheme > div').length + 1;
    }
    Obj.themeNos = "";
    for (let i = 0; i < Obj.numberTheme; i++) {
        if (i == 0)
            Obj.themeNos += $(`#theme_${i + 1}`)[0].value;
        else
            Obj.themeNos += "," + $(`#theme_${i + 1}`)[0].value;
    }
    Obj.workContentCodes = "";
    for (let i = 0; i < $('#AddTheme > div').length + 1; i++) {
        if (i == 0)
            Obj.workContentCodes += $(`#addWorkContent_${i + 1}`)[0].value;
        else
            Obj.workContentCodes += "," + $(`#addWorkContent_${i + 1}`)[0].value;
    }
    Obj.workContentDetails = "";
    for (let i = 0; i < $('#AddTheme > div').length + 1; i++) {
        if (i == 0)
            Obj.workContentDetails += $(`#addWorkDetail_${i + 1}`)[0].value;
        else
            Obj.workContentDetails += "," + $(`#addWorkDetail_${i + 1}`)[0].value;
    }
    Obj.numberGroup = 0;
    Obj.Groups = "";
    Obj.Users = "";
    Obj.numberUser = "";
    var countGetUser = 0;
    for (let i = 0; i < $('select').length; i++) {
        if ($('select')[i].id.includes("GroupId")) {
            Obj.Groups += "," + $('select')[i].value;
            if ($('select')[i].id != "GroupId1")
                Obj.numberGroup = countGroup;
            else if ($('#GroupId1').val != "") {
                Obj.numberGroup = 1;
            }
        }

        else if ($('select')[i].id.includes("selectUser")) {
            Obj.Users += "," + $('select')[i].value;
        }
    }
    for (let i = 0; i < $('.countUser').length; i++) {
        Obj.numberUser += "," + (($('.countUser')[i].innerHTML).match(/<select/g) || []).length;
    }
    return Obj;
}

// out put CSV
function outputCSV() {
    $('#error').empty();
    var Obj = setObj();
    $.ajax({
        url: '/ManhourReport/CheckReport',
        method: 'POST',
        data: Obj,
        success: function (response) {
            $('#savename').empty();
            $('#toDate1').empty();
            $('#fromDate1').empty();
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/GetDataCSV',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        if (response.messenge == "") {
                            const data = encodeURI('data:text/csv;charset=utf-8,' + response.data);
                            const link = document.createElement('a');
                            link.setAttribute('href', data);
                            link.setAttribute('download', response.fileName);
                            link.click();
                        }
                        else
                            $("#error").append(error(response.messenge));
                    }
                });
            }
            else {
                if (response.toDate != "") {
                    $("#error").append(error(response.toDate));
                }
                else if (response.fromDate != "") {
                    $("#error").append(error(response.fromDate));
                }
                else if (response.date != "") {
                    $("#error").append(error(response.date));
                }
                else if (response.dateCalculate != "") {
                    $("#error").append(error(response.dateCalculate));
                }
                else if (response.user != "") {
                    $("#error").append(error(response.user));
                }
                else if (response.theme != "") {
                    $("#error").append(error(response.theme));
                }
                else if (response.headerItems != "") {
                    $("#error").append(error(response.headerItems));
                }
            }
        }
    })
}

// set default
function reset() {
    $('#error').empty();
    $('#fromDate').val(null);
    $('#toDate').val(null);
    $('#addUserinGroup1').empty();
    $('#trashUser1').empty();
    $('#AddGroup').empty();
    $('#GroupId1').val("");
    $('#AddTheme').empty();
    $('#addWorkDetail_1').val(null);
    $('#addWorkContent_1').empty();
    $('#theme_1').val(null);
    $('#Select').empty();
    $('#NotSelect').empty();
    $('#NotSelect').append(`<option value="affiliation">所属</option>
                            <option value="user">ユーザ</option>
                            <option value="theme">テーマ</option>
                            <option value="workcontent">作業内容</option>
                            <option value="detailworkcontent">作業内容詳細</option>
                            <option value="overalltotal">全体合計</option>
                            <option value="monthlytotal">月別合計</option>
                            <option value="dailytotal">日別合計</option>
                            `);
    $('#isTotal').prop('checked', false);
    $('#isTotal1').prop('checked', true);
    $('#typeDelimiter').prop('checked', false);
    $('#typeDelimiter1').prop('checked', true);
    $('#isSingleQuote').prop('checked', false);
    $('#isSingleQuote1').prop('checked', true);
    $('#UserAdd1').attr("hidden", true);
}

function saveUserScreenName() {
    $('#error').empty();
    var Obj = setObj();
    $.ajax({
        url: '/ManhourReport/CheckSave',
        method: 'POST',
        data: Obj,
        success: function (response) {
            
            if (Object.keys(response).length < 1) {
                $.ajax({
                    url: '/ManhourReport/SaveScreen',
                    method: 'POST',
                    data: Obj,
                    success: function (response) {
                        if (response != ERROR) {
                            $("#error").append(success(response));
                            GetScreen();
                        }
                    }
                });
            }
            else {
                if (response.savename != "") {
                    $("#error").append(error(response.savename));
                }
                else if (response.toDate != "") {
                    $("#error").append(error(response.toDate));
                }
                else if (response.fromDate != "") {
                    $("#error").append(error(response.fromDate));
                }
                else if (response.date != "") {
                    $("#error").append(error(response.date));
                }
                else if (response.dateCalculate != "") {
                    $("#error").append(error(response.dateCalculate));
                }
                else if (response.headerItems != "") {
                    $("#error").append(error(response.headerItems));
                }
            }
        }
    });
}

$('#datepicker1').datepicker({ autoclose: true, language: 'ja' }).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    $('#fromDate').val(`${date.getFullYear()}/${date.getMonth() >= 9 ? date.getMonth() + 1 : "0" + (date.getMonth()+1)}/${date.getDate() >= 10 ? date.getDate() : "0" + date.getDate()}`);
  });
$('#datepicker2').datepicker({ format: "yyyy/mm/dd", language: "ja", autoclose: true, orientation: 'bottom left' }).on('changeDate', function (ev) {
    var date = new Date(ev.date.valueOf());
    $('#toDate').val(`${date.getFullYear()}/${date.getMonth() >= 9 ? date.getMonth() + 1 : "0" + (date.getMonth() + 1)}/${date.getDate() >= 10 ? date.getDate(): "0" + date.getDate()}`);
});

// Get save name in current (db)
function GetScreen() {
    $.ajax({
        url: '/ManhourReport/GetsScreen',
        method: 'Get',
        success: function (response) {
            $('#userScreenName').empty();
            var options = "<option>保存名...</option>";
            $.each(response, function (i, v) {
                options += `<option value="${v.value}">${v.text}</option>`;
            });
            $('#userScreenName').append(options);
        }
    });
}
function error(messeges) {
    return `<div class="alert alert-danger mb-2" role="alert">
                                            <strong>アラート</strong> - ${messeges}
                                        </div>`;
}function success(messeges) {
    return `<div class="alert alert-success mb-2" role="alert">
                                            ${messeges}
                                        </div>`;
}