<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Login</title>

    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css" />
    <script src="http://code.jquery.com/jquery-1.11.1.min.js"></script>
    <script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>

    <script type="text/javascript">
        sessionStorage.setItem("LOGGED", -1);

        $(function () {
            $("[id*=butLogin]").click(function () {
                var obj = {};
                obj.pEmail = window.btoa($.trim($("[id*=inpEmail]").val()));
                obj.pPassword = window.btoa($.trim($("[id*=inpPassword]").val()));

                $.ajax({
                    type: 'POST',
                    url: 'Login.aspx/LogOn',
                    data: JSON.stringify(obj),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (msg) {
                        if (!msg.d.includes("invalid")) {
                            $("[id*=inpEmail]").css("border-style", "none");
                            $("[id*=inpPassword]").css("border-style", "none");
                            sessionStorage.setItem("LOGGED", msg.d.toString().split(':')[0]);
                            sessionStorage.setItem("ADMIN", msg.d.toString().split(':')[1]);
                            window.location.replace("Customer.aspx");
                        }
                        else {
                            $("[id*=inpEmail]").css("border-style", "solid");
                            $("[id*=inpPassword]").css("border-style", "solid");
                            sessionStorage.setItem("LOGGED", -1);
                            alert(msg.d);
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
    <form id="frmLogin" runat="server">
        <div id="divMain" style="text-align: -webkit-center;">
            Login do Usuário
            <div id="divFields" style="text-align: -webkit-center; border: cyan; border-style: solid; border-width: thin; border-radius: 5px; height: auto; width: 300px;">
                <table id="tabFields">
                    <tr>
                        <td>E-mail:</td>
                        <td>
                            <input id="inpEmail" style="border-style:none; border-color:red; border-width:thin;" />
                        </td>
                    </tr>
                    <tr>
                        <td>Password:</td>
                        <td>
                            <input type="password" id="inpPassword" style="border-style:none; border-color:red; border-width:thin;" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <button id="butLogin">LOGIN</button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </form>
</body>
</html>
