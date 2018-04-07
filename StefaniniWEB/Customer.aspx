<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Customer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="MainStyles.css" rel="stylesheet" />
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css" />
    <script src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>
    <script src="http://code.jquery.com/ui/1.9.2/jquery-ui.js"></script>

    <script type="text/javascript">
        if (sessionStorage.getItem("LOGGED") != null) {
            if (sessionStorage.getItem("LOGGED") < 1) {
                window.location.replace("Login.aspx");
            }
        }
        else if (sessionStorage.getItem("LOGGED") == null) {
            window.location.replace("Login.aspx");
        }

        if (sessionStorage.getItem("ADMIN") != null) {
            if (sessionStorage.getItem("ADMIN") != "True") {
                $("#labSeller").css("display", "none");
                $("#ddlSeller").css("display", "none");
            }
            else {
                $("#labSeller").css("display", "block");
                $("#ddlSeller").css("display", "block");
            }
        }

        $(document).ready(function () {
            $('#inpLast').datepicker({
                dateFormat: 'yy-mm-dd'
            });

            $('#inpUntil').datepicker({
                dateFormat: 'yy-mm-dd'
            });
        });

        $(document).ready(function () {
            $(function () {
                $("[id*=butSearch]").click(function () {
                    var obj = {};

                    if (sessionStorage.getItem("ADMIN") != null) {
                        if (sessionStorage.getItem("ADMIN").toUpperCase() == "TRUE") {
                            obj.pAdmin = window.btoa("TRUE");
                        }
                        else {
                            obj.pAdmin = window.btoa("FALSE");
                        }
                    }
                    else {
                        obj.pAdmin = window.btoa("FALSE");
                    }

                    if (sessionStorage.getItem("LOGGED") != null) {
                        obj.pID = window.btoa(sessionStorage.getItem("LOGGED"));
                    }
                    else {
                        obj.pID = window.btoa("-1");
                    }

                    obj.pName = window.btoa($.trim($("[id*=inpName]").val()));
                    obj.pGender = window.btoa($("#ddlGender option:selected").text());
                    obj.pCity = window.btoa($("#ddlCity option:selected").text());
                    obj.pRegion = window.btoa($("#ddlRegion option:selected").text());
                    obj.pLast = window.btoa($.trim($("[id*=inpLast]").val()));
                    obj.pUntil = window.btoa($.trim($("[id*=inpUntil]").val()));
                    obj.pClass = window.btoa($("#ddlClass option:selected").text());
                    if ($("#ddlSeller option:selected").text() != "") {
                        obj.pSeller = window.btoa($("#ddlSeller option:selected").attr('value'));
                    }
                    else {
                        obj.pSeller = window.btoa($("#ddlSeller option:selected").text());
                    }

                    $.ajax({
                        type: 'POST',
                        url: 'Customer.aspx/Search',
                        data: JSON.stringify(obj),
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        success: function (msg) {
                            $("#tabCustomers tr").remove();
                            $('#tabCustomers').append(msg.d);
                        },
                        error: function (msg, merr, derr) {
                            alert(msg.d);
                        }
                    });

                    return false;
                });
            });
        });

        $(document).ready(function () {
            $(function () {
                $("[id*=butClear]").click(function () {
                    $('#inpName').val("");
                    $('#inpLast').val("");
                    $('#inpUntil').val("");
                    $("#ddlGender").val(0).trigger("change");
                    $("#ddlCity").val(0).trigger("change");
                    $("#ddlRegion").val(0).trigger("change");
                    $("#ddlClass").val(0).trigger("change");

                    if (sessionStorage.getItem("ADMIN") != null) {
                        if (sessionStorage.getItem("ADMIN").toUpperCase() == "TRUE") {
                            $("#ddlSeller").val(0).trigger("change");
                        }
                    }

                    $("#tabCustomers tr").remove();

                    return false;
                });
            });
        });

        $(document).ready(function () {
            $('#ddlCity').on('change', function () {
                var obj = {};
                                
                obj.pCity = window.btoa($("#ddlCity option:selected").text());
                
                $.ajax({
                    type: 'POST',
                    url: 'Customer.aspx/SetRegion',
                    data: JSON.stringify(obj),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {
                        var options = msg.d.split(';');

                        $('#ddlRegion').find('option').remove();

                        for (var i = 0; i < options.length; i++) {
                            if (options[i].split(':').length > 1) {
                                var values = options[i].split(':');
                                $('#ddlRegion').append('<option value="' + values[0] + '">' + values[1] + '</option>');
                            }
                        }

                        if (obj.pCity == "") {
                            $('#ddlRegion').val(0).trigger("change");
                        }
                        else {
                            $('#ddlRegion').val(1).trigger("change");
                        }
                    },
                    error: function (msg, merr, derr) {
                        alert(msg.d);
                    }
                });

                return false;
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="divMain">
            <div id="divFields" style="width: 60%; margin-left: auto; margin-right: auto;">
                <table>
                    <tr>
                        <td class="ColToRight">Name:</td>
                        <td class="ColToLeft">
                            <input id="inpName" />
                        </td>
                        <td class="ColToRight">Gender:</td>
                        <td class="ColToLeft">
                            <select id="ddlGender" runat="server">
                                <option></option>
                            </select>
                        </td>
                        <td class="ColToRight">
                            <button id="butSearch" style="float: right;">Search</button>
                        </td>
                    </tr>
                    <tr>
                        <td class="ColToRight">City:</td>
                        <td class="ColToLeft">
                            <select id="ddlCity" runat="server">
                                <option></option>
                            </select>
                        </td>
                        <td class="ColToRight">Region:</td>
                        <td class="ColToLeft">
                            <select id="ddlRegion" runat="server">
                                <option></option>
                            </select>
                        </td>
                        <td class="ColToRight">
                            <button id="butClear" style="float: right;">Clear Fields</button>
                        </td>
                    </tr>
                    <tr>
                        <td class="ColToRight">Last Purchase:</td>
                        <td class="ColToLeft">
                            <input type="text" id="inpLast" />
                        </td>
                        <td class="ColToRight">Until:</td>
                        <td class="ColToLeft">
                            <input type="text" id="inpUntil" />
                        </td>
                        <td class="ColToRight"></td>
                    </tr>
                    <tr>
                        <td class="ColToRight">Classification:</td>
                        <td class="ColToLeft">
                            <select id="ddlClass" runat="server">
                                <option></option>
                            </select>
                        </td>
                        <td class="ColToRight">
                            <label id="labSeller">Seller:</label>
                        </td>
                        <td class="ColToLeft">
                            <select id="ddlSeller" runat="server">
                                <option></option>
                            </select>
                        </td>
                        <td class="ColToRight"></td>
                    </tr>
                </table>
            </div>
            <hr />
            <table id="tabCustomers" style="width: 80%; margin-left: auto; margin-right: auto;">
                <thead>
                    <tr>
                        <td>Classification</td>
                        <td>Name</td>
                        <td>Phone</td>
                        <td>Gender</td>
                        <td>City</td>
                        <td>Region</td>
                        <td>Last Purchase</td>
                        <td>Seller</td>
                    </tr>
                </thead>
            </table>
        </div>
    </form>
</body>
</html>
