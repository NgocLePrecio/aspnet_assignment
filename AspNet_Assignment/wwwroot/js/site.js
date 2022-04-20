// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let curRowId = '';
let curRowNum = 0;
let title = '';
let ext = '';

$(function () {
    $('#btnDelete').attr('disabled', 'disabled');
    $('#btnUpdate').attr('disabled', 'disabled');
    LoadData();

    $('#btnAddFile').on('click', function () {
        if (window.FormData !== undefined) {
            let fileUpload = $('#fileUpload')[0];
            let files = fileUpload.files;
            if (files.length === 0) {
                alert('No file chosen');
                return;
            }
            let fileData = new FormData();
            for (var i = 0; i < files.length; i++) {
                //console.log(files[i])
                fileData.append(files[i].name, files[i])
            }

            // Adding user to FormData object  
            fileData.append('username', $('#user_login').text())

            for (var p of fileData) {
                let name = p[0];
                let value = p[1];
                console.log(name, value)
            }

            $.ajax({
                url: '/FileUploaded',
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                beforeSend: function () {
                    $('#overlay').fadeIn(300);
                },
                success: function (result) {
                    console.log(result);
                    let newRow = '';
                    for (var i = 0; i < result.length; i++) {
                        newRow = `<tr><td><input type='radio' name='rowId' data-id='${result[i].fileId}'/> <i class="fa fa-file-text-o"></i></td><td><a href='/FileUploaded/${result[i].fileId}'>${result[i].fileName}</a></td><td>${result[i].createdAt}</td><td>${result[i].createdBy}</td><td></td></tr>`;
                        $('tbody').append(newRow);
                    }
                    $('#fileUpload').val('');
                },
                error: function (err) {
                    alert(err.statusText);
                },
                complete: function () {
                    $('#overlay').fadeOut(150);
                }
            });
        }
        else {
            alert("FormData is not supported.");
        }
        
    })

    $('#btnUpdate').on('click', function (evt) {
        let top = 0;
        let left = 0;
        if ($('#updateModal').hasClass('dialogHidden')) {
            $('#updateModal').removeClass('dialogHidden');
        }
        $('#updateModal').addClass('dialogVisible');
        let fullName = $(`table tbody tr:eq(${curRowNum}) td:eq(1) a`).text();
        ext = fullName.split('.').pop();
        title = fullName.substring(0, fullName.lastIndexOf('.'));
        $('#dataTitle').val(title);
        var windowHeight = $(window).height();
        var windowWidth = $(window).width();
        var dialogHeight = $('#updateModal').outerHeight();
        var dialogWidth = $('#updateModal').outerWidth();
        top =
            (windowHeight - dialogHeight) / 2 +
            $(window).scrollTop() +
            'px';
        left =
            (windowWidth - dialogWidth) / 2 + $(window).scrollLeft() + 'px';
        $('#updateModal').css('top', top);
        $('#updateModal').css('left', left);
        $('#backgroundDiv')
            .addClass('dialogBackground')
            .height(windowHeight)
            .width(windowWidth)
            .css('display', 'block');
        evt.preventDefault();
    });

    $('#CancelUpdateBtn').on('click', function (evt) {
        closeModal(evt);
    });

    $('#OkUpdateBtn').on('click', function (evt) {

        let fileData = new FormData();
        let newTitle = $('#dataTitle').val().toString();
        let user = $('#user_login').text();
        fileData.append('fileName', newTitle + '.' + ext);
        fileData.append('createdBy', user);
        $.ajax({
            url: `/FileUploaded/${curRowId}`,
            type: "PUT",
            contentType: false, // Not to set any content header  
            processData: false,
            data: fileData,
            beforeSend: function () {
                $('#overlay').fadeIn(300);
            },
            success: function (result) {
                $(`table tbody tr:eq(${curRowNum}) td:eq(1) a`).text(result.fileName);
                $(`table tbody tr:eq(${curRowNum}) td:eq(2)`).text(result.createdAt);
                $(`table tbody tr:eq(${curRowNum}) td:eq(3)`).text(result.createdBy);
            },
            error: function (err) {
                alert(err.statusText);
            },
            complete: function () {
                $('#overlay').fadeOut(150);
            }
        });
        closeModal(evt);
    });

    // Radio Button Click
    $('tbody').on('click', 'input:radio[name="rowId"]', function () {
        curRowId = $('input[name="rowId"]:checked').attr('data-id');
        curRowNum = $(this).parent().parent().index();
        $('#btnDelete').removeAttr('disabled');
        $('#btnUpdate').removeAttr('disabled');
    });

    $('#btnDelete').on('click', function () {
        $.ajax({
            url: `/FileUploaded/${curRowId}`,
            type: "DELETE",
            beforeSend: function () {
                $('#overlay').fadeIn(300);
            },
            success: function (result) {
                $(`table tbody tr:eq(${curRowNum})`).remove();
                $('#btnDelete').attr('disabled', 'disabled');
                $('#btnUpdate').attr('disabled', 'disabled');
            },
            error: function (err) {
                alert(err.statusText);
            },
            complete: function () {
                $('#overlay').fadeOut(150);
            }
        });
    });
})

function LoadData(){
    $.ajax({
        url: '/FileUploaded',
        type: "GET",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        beforeSend: function () {
            $('#overlay').fadeIn(300);
        },
        success: function (result) {
            console.log(result);
            let newRow = '';
            for (var i = 0; i < result.length; i++) {
                newRow = `<tr><td><input type='radio' name='rowId' data-id='${result[i].fileId}'/> <i class="fa fa-file-text-o"></i></td><td><a href='/FileUploaded/${result[i].fileId}'>${result[i].fileName}</a></td><td>${result[i].createdAt}</td><td>${result[i].createdBy}</td><td></td></tr>`;
                $('tbody').append(newRow);
            }
        },
        error: function (err) {
            alert(err.statusText);
        },
        complete: function () {
            $('#overlay').fadeOut(150);
        }
    });
}

const closeModal = (evt) => {
    $('#dataTitle').val('');
    if ($('#backgroundDiv').hasClass('dialogBackground')) {
        $('#backgroundDiv')
            .removeClass('dialogBackground')
            .css('display', 'none');
    }
    if ($('#updateModal').hasClass('dialogVisible')) {
        $('#updateModal').removeClass('dialogVisible');
    }
    $('#updateModal').addClass('dialogHidden');
    evt.preventDefault();
};

