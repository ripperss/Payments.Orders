using PayMents.Orders.Application.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtuhTests.Common;

public static class userRegisterDtoFactory
{
    public static UserRegisterDto CreateCorrectUserRegisterDto()
    {
        return new UserRegisterDto(
            UserName: "fdf"
            , Email: "fdfd"
            , Phone: "fdfd"
            , Password: "fdfd");
    }
    public static UserRegisterDto CreateNoCorrectUserRegisterDto()
    {
        return new UserRegisterDto(
            UserName: ""
            , Email: "fdfd"
            , Phone: ""
            , Password: "fdfd");
    }
}
